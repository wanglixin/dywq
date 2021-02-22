using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Commpany;
using Dywq.Web.Dto.User;
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
    public class GetCompanysCommand : IRequest<PageResult<CompanyDTO>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Key { get; set; }

        public string LinkUrl { get; set; }

        public LoginUserDTO LoginUser { get; set; }
    }

    public class GetCompanysCommandHandler : IRequestHandler<GetCompanysCommand, PageResult<CompanyDTO>>
    {
        private readonly ICapPublisher _capPublisher;
        readonly ILogger<GetCompanyFieldsCommandHandler> _logger;
        readonly IBaseRepository<CompanyFieldData> _companyFieldDataRepository;
        readonly IBaseRepository<CompanyField> _companyFieldRepository;
        readonly IBaseRepository<Company> _companyRepository;

        IBaseRepository<CompanyFieldDefaultValue> _companyFieldDefaultValueRepository;
        public GetCompanysCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetCompanyFieldsCommandHandler> logger,
            IBaseRepository<CompanyFieldData> companyFieldDataRepository,
            IBaseRepository<CompanyField> companyFieldRepository,
             IBaseRepository<CompanyFieldDefaultValue> companyFieldDefaultValueRepository,
             IBaseRepository<Company> companyRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;

            _companyFieldDataRepository = companyFieldDataRepository;
            _companyFieldRepository = companyFieldRepository;
            _companyFieldDefaultValueRepository = companyFieldDefaultValueRepository;
            _companyRepository = companyRepository;
        }


        public async Task<PageResult<CompanyDTO>> Handle(GetCompanysCommand request, CancellationToken cancellationToken)
        {

            var condition = new List<string>();
            if (!string.IsNullOrWhiteSpace(request.Key))
            {
                //condition.Add($"(d.Alias='{Common.CompanyFieldAlias.CompanyName}' and d.Value like '%{request.Key}%')");
                condition.Add($"(Name like '%{request.Key}%')");
            }


            if (request.LoginUser.Type == 2) //如果是编辑
            {
                condition.Add($"(UserId = {request.LoginUser.Id})");
            }



            var where = condition.Count > 0 ? $" where {string.Join(" or ", condition)}" : "";

            var count = await _companyRepository.SqlCountAsync(@$"
SELECT count(*) FROM [Company] {where}");
            if (count < 1) return PageResult<CompanyDTO>.Success(null, 0, request.PageIndex, request.PageSize, "");

            var start = (request.PageIndex - 1) * request.PageSize;
            var end = start + request.PageSize;

            /*var data = await _companyRepository.SqlQueryAsync<CompanyDTO>(@$"select t.*,c.Name,c.Logo,c.CreatedTime from (
SELECT CompanyId,ROW_NUMBER() over(order by CompanyId desc) Rowid  FROM [CompanyFieldData] as d {where}  group by CompanyId
) as t left join Company as c on c.Id=t.CompanyId where t.Rowid>{start} and t.Rowid<={end}
;");*/

            var data = await _companyRepository.SqlQueryAsync<CompanyDTO>(@$"select * from (
SELECT Id as CompanyId,Name,Logo,CreatedTime,Status,ROW_NUMBER() over(order by Id desc) Rowid 
  FROM [Company] {where}
) as c where c.Rowid>{start} and c.Rowid<={end}");

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
