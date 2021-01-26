using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto;
using Dywq.Web.Dto.Commpany;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands
{
    public class SearchCompanysCommand : IRequest<PageResult<CompanyDTO>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string LinkUrl { get; set; }

        /// <summary>
        /// 搜索条件
        /// </summary>
        public IEnumerable<SearchDTO> Searches { get; set; } = new List<SearchDTO>();

    }

    public class SearchCompanysCommandHandler : BaseRequestHandler<SearchCompanysCommand, PageResult<CompanyDTO>>
    {

        readonly IBaseRepository<CompanyFieldData> _companyFieldDataRepository;
        readonly IBaseRepository<CompanyField> _companyFieldRepository;
        readonly IBaseRepository<Company> _companyRepository;

        readonly IBaseRepository<CompanyFieldDefaultValue> _companyFieldDefaultValueRepository;
        public SearchCompanysCommandHandler(
            ICapPublisher capPublisher,
            ILogger<SearchCompanysCommandHandler> logger,
            IBaseRepository<CompanyFieldData> companyFieldDataRepository,
            IBaseRepository<CompanyField> companyFieldRepository,
             IBaseRepository<CompanyFieldDefaultValue> companyFieldDefaultValueRepository,
             IBaseRepository<Company> companyRepository
            ) : base(capPublisher, logger)
        {
            _companyFieldDataRepository = companyFieldDataRepository;
            _companyFieldRepository = companyFieldRepository;
            _companyFieldDefaultValueRepository = companyFieldDefaultValueRepository;
            _companyRepository = companyRepository;
        }


        public override async Task<PageResult<CompanyDTO>> Handle(SearchCompanysCommand request, CancellationToken cancellationToken)
        {
            var condition = new List<string>();
            var name = string.Empty;
            foreach (var item in request.Searches)
            {
                if (string.IsNullOrWhiteSpace(item.Value))
                    continue;
                if (item.Type == 0)
                {
                    condition.Add($"( d.FieldId={item.FieldId} and d.Value like '%{item.Value}%' )");
                }
                else if (item.Type == -1)
                {
                    name = item.Value;
                    //condition.Add($"( t.Name like '%{item.Value}%' )");
                }
                else if (item.Type == 4) //复选框
                {
                    var id_arr = item.Value.Split(',');
                    var str_arr = id_arr.Select(x => $"d.Value like '%{x}%'");
                    condition.Add($"( d.FieldId={item.FieldId} and ({string.Join(" or ", str_arr)}) )");
                }
                else
                {
                    condition.Add($"( d.FieldId={item.FieldId} and d.Value='{item.Value}' )");
                }
            }

            var where = condition.Count > 0 ? $" {string.Join(" or ", condition)}" : "";

            if (string.IsNullOrWhiteSpace(where))
            {
                where = !string.IsNullOrWhiteSpace(name) ? $" where ( c.Name like '%{name}%' )" : "";
            }
            else
            {
                where = !string.IsNullOrWhiteSpace(name) ? $" where ( c.Name like '%{name}%' ) and ( {where} )" : $" where {where}";
            }


            var where2 = condition.Count > 0 ? $"where t.c={condition.Count}" : "";





            var sql = @$" select count(*) from (
   select c.id,count(c.id) c from Company as c left join CompanyFieldData as d on d.CompanyId=c.id  {where} group by c.id 
   ) as t {where2}";
            var count = await _companyRepository.SqlCountAsync(sql);

            this._logger.LogInformation($"count={sql}");

            if (count < 1) return PageResult<CompanyDTO>.Success(null, 0, request.PageIndex, request.PageSize, "");
            var start = (request.PageIndex - 1) * request.PageSize;
            var end = start + request.PageSize;

            sql = @$"  select * from (
	   select id CompanyId,Name,ROW_NUMBER() over(order by Id desc) Rowid from (
   select c.id,min(c.Name) Name,count(c.id) c from Company as c left join CompanyFieldData as d on d.CompanyId=c.id {where}   group by c.id 
   ) as t {where2} ) as tt where tt.Rowid>{start} and tt.Rowid<={end}";

            var data = await _companyRepository.SqlQueryAsync<CompanyDTO>(sql);

            this._logger.LogInformation($"data={sql}");

            var companyIdArr = data.Select(x => x.CompanyId);


            var alias = new List<string>()
            {
                 Common.CompanyFieldAlias.Industry,
                 Common.CompanyFieldAlias.BusinessNature,
                 Common.CompanyFieldAlias.IsPartyBranch
            };

            var fieldDatas = await _companyFieldDataRepository.Set().Where(x => companyIdArr.Contains(x.CompanyId) && alias.Contains(x.Alias)).ToListAsync();

            var types = new List<int>() { 3, 4, 5 };

            var sourceDic = new Dictionary<int, Dictionary<string, string>>();

            foreach (var item in data)
            {
                foreach (var a in alias)
                {
                    var showText = new List<string>();
                    var fieldData = fieldDatas.FirstOrDefault(x => x.CompanyId == item.CompanyId && x.Alias == a);
                    if (fieldData != null)
                    {
                        if (types.Contains(fieldData.Type))
                        {
                            if (!sourceDic.ContainsKey(fieldData.FieldId))
                            {
                                sourceDic[fieldData.FieldId] = new Dictionary<string, string>();
                                var values = await _companyFieldDefaultValueRepository.Set().Where(x => x.CompanyFieldId == fieldData.FieldId).ToListAsync();
                                values.ForEach(x =>
                                {
                                    sourceDic[fieldData.FieldId].Add(x.Value, x.Text);
                                });
                            }
                            fieldData?.Value?.Split(',')?.ToList()?.ForEach(x =>
                            {
                                if (sourceDic[fieldData.FieldId].ContainsKey(x))
                                {
                                    showText.Add(sourceDic[fieldData.FieldId][x]);
                                }
                            });
                        }
                        else
                        {
                            showText.Add(fieldData.Value);
                        }
                        var text = string.Join(",", showText);
                        switch (a)
                        {
                            case Common.CompanyFieldAlias.Industry: item.Industry = text; break;
                            case Common.CompanyFieldAlias.BusinessNature: item.BusinessNature = text; break;
                            case Common.CompanyFieldAlias.IsPartyBranch: item.IsPartyBranch = text; break;
                            default: break;
                        }
                    }
                }


            }

            return PageResult<CompanyDTO>.Success(data, count, request.PageIndex, request.PageSize, request.LinkUrl);
        }
    }
}
