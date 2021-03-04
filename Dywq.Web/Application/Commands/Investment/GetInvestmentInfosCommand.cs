using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.CooperationAggregate;
using Dywq.Domain.InvestmentAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Cooperation;
using Dywq.Web.Dto.Investment;
using Dywq.Web.Dto.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Investment
{
    public class GetInvestmentInfosCommand : IRequest<PageResult<InvestmentInfoDTO>>
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
        /// 
        /// </summary>
        public int? Status { get; set; } = null;

        public LoginUserDTO LoginUser { get; set; }

    }

    public class GetInvestmentInfosCommandHandler : BaseRequestHandler<GetInvestmentInfosCommand, PageResult<InvestmentInfoDTO>>
    {

        readonly IBaseRepository<InvestmentInfo> _investmentInfoRepository;
        readonly IBaseRepository<InvestmentType> _investmentTypeRepository;
        readonly IBaseRepository<Company> _companyRepository;

        public GetInvestmentInfosCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetInvestmentInfosCommandHandler> logger,
            IBaseRepository<InvestmentInfo> investmentInfoRepository,
             IBaseRepository<InvestmentType> investmentTypeRepository,
             IBaseRepository<Company> companyRepository
            ) : base(capPublisher, logger)
        {
            _investmentInfoRepository = investmentInfoRepository;
            _investmentTypeRepository = investmentTypeRepository;
            _companyRepository = companyRepository;
        }

        public override async Task<PageResult<InvestmentInfoDTO>> Handle(GetInvestmentInfosCommand request, CancellationToken cancellationToken)
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

            if (request.Status.HasValue)
            {
                sb.Add($"Status = " + request.Status);
            }


            if (request.CompanyId != 0)
            {
                sb.Add($"CompanyId = " + request.CompanyId);
            }


            if (!string.IsNullOrWhiteSpace(request.TypeId))
            {
                sb.Add($"InvestmentTypeId = " + request.TypeId);
            }

            if (request.LoginUser != null && request.LoginUser.Type == 2)
            {
                sb.Add($"UserId = " + request.LoginUser.Id);
            }


            var where = string.Join(" and ", sb);


            var pageData = await _investmentInfoRepository.GetPageDataAsync<InvestmentInfo>(
               pageIndex: request.PageIndex,
               pageSize: request.PageSize,
               where: where,
               order: "Status asc,Sort asc,Id desc"
               );
            var count = pageData.Total;
            if (count < 1)
            {
                return PageResult<InvestmentInfoDTO>.Success(null, count, request.PageIndex, request.PageSize, request.LinkUrl);
            }

            var types = await _investmentTypeRepository.Set().ToListAsync();

            var companyId_arr = pageData.Data.Select(x => x.CompanyId);

            var companys = _companyRepository.Set().Where(x => companyId_arr.Contains(x.Id));

            var data = pageData.Data.Select(x =>

                new InvestmentInfoDTO()
                {
                    Id = x.Id,
                    Content = x.Content,
                    CreatedTime = x.CreatedTime,
                    Show = x.Show,
                    Sort = x.Sort,
                    Title = x.Title,
                    UpdatedTime = x.UpdatedTime,
                    InvestmentTypeId = x.InvestmentTypeId,
                    InvestmentTypeName = types.FirstOrDefault(t => t.Id == x.InvestmentTypeId)?.Name,
                    CompanyId = x.CompanyId,
                    CompanyName = companys.FirstOrDefault(c => c.Id == x.CompanyId)?.Name,
                    Status = x.Status,
                    Describe = x.Describe

                }
            ); ;

            return PageResult<InvestmentInfoDTO>.Success(data, count, request.PageIndex, request.PageSize, request.LinkUrl);

        }
    }
}
