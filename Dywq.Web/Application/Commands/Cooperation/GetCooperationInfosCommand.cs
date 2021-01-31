using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.CooperationAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Cooperation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Cooperation
{
    public class GetCooperationInfosCommand : IRequest<PageResult<CooperationInfoDTO>>
    {
        public int Id { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string LinkUrl { get; set; }

        /// <summary>
        /// 政策类型id
        /// </summary>
        public string TypeId { get; set; } = "";
        public bool? Show { get; set; } = null;

        public int CompanyId { get; set; } = 0;

        /// <summary>
        /// -888 管理员后台获取
        /// </summary>
        public int Status { get; set; } = -999;

    }

    public class GetCooperationInfosCommandHandler : BaseRequestHandler<GetCooperationInfosCommand, PageResult<CooperationInfoDTO>>
    {

        readonly IBaseRepository<CooperationInfo> _cooperationInfoRepository;
        readonly IBaseRepository<CooperationType> _cooperationTypeRepository;
        readonly IBaseRepository<Company> _companyRepository;

        public GetCooperationInfosCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetCooperationInfosCommandHandler> logger,
            IBaseRepository<CooperationInfo> cooperationInfoRepository,
             IBaseRepository<CooperationType> cooperationTypeRepository,
             IBaseRepository<Company> companyRepository
            ) : base(capPublisher, logger)
        {
            _cooperationInfoRepository = cooperationInfoRepository;
            _cooperationTypeRepository = cooperationTypeRepository;
            _companyRepository = companyRepository;
        }

        public override async Task<PageResult<CooperationInfoDTO>> Handle(GetCooperationInfosCommand request, CancellationToken cancellationToken)
        {
            var sb = new List<string>();


            if (request.Id != 0)
            {
                sb.Add($"Id = {request.Id}");
            }

            if (request.Show.HasValue)
            {
                sb.Add($"Show = " + (request.Show.Value ? 1 : 0));
            }

            if (request.Status != -999)
            {
                if (request.Status == -888)
                {
                    sb.Add($"Status in (1,2,-1)");
                }
                else
                {
                    sb.Add($"Status = " + request.Status);
                }
               
            }


            if (request.CompanyId != 0)
            {
                sb.Add($"CompanyId = " + request.CompanyId);
            }


            if (!string.IsNullOrWhiteSpace(request.TypeId))
            {
                sb.Add($"CooperationTypeId = " + request.TypeId);
            }
            var where = string.Join(" and ", sb);


            var pageData = await _cooperationInfoRepository.GetPageDataAsync<CooperationInfo>(
               pageIndex: request.PageIndex,
               pageSize: request.PageSize,
               where: where,
               order: "Status asc,Sort asc,Id desc"
               );
            var count = pageData.Total;
            if (count < 1)
            {
                return PageResult<CooperationInfoDTO>.Success(null, count, request.PageIndex, request.PageSize, request.LinkUrl);
            }

            var types = await _cooperationTypeRepository.Set().ToListAsync();

            var companyId_arr = pageData.Data.Select(x => x.CompanyId);

            var companys = _companyRepository.Set().Where(x => companyId_arr.Contains(x.Id));

            var data = pageData.Data.Select(x =>

                new CooperationInfoDTO()
                {
                    Id = x.Id,
                    Content = x.Content,
                    CreatedTime = x.CreatedTime,
                    Show = x.Show,
                    Sort = x.Sort,
                    Title = x.Title,
                    UpdatedTime = x.UpdatedTime,
                    CooperationTypeId = x.CooperationTypeId,
                    CooperationTypeName = types.FirstOrDefault(t => t.Id == x.CooperationTypeId)?.Name,
                    CompanyId = x.CompanyId,
                    CompanyName = companys.FirstOrDefault(c => c.Id == x.CompanyId)?.Name,
                    Status = x.Status

                }
            ); ;

            return PageResult<CooperationInfoDTO>.Success(data, count, request.PageIndex, request.PageSize, request.LinkUrl);

        }
    }
}
