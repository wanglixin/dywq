using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.CooperationAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Cooperation;
using Dywq.Web.Dto.Financing;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Financing
{
    public class GetFinancingsCommand : IRequest<PageResult<FinancingDTO>>
    {
        public int Id { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string LinkUrl { get; set; }

        public bool? Show { get; set; } = null;

        public int CompanyId { get; set; } = 0;

        public int Status { get; set; } = -999;

    }

    public class GetFinancingsCommandHandler : BaseRequestHandler<GetFinancingsCommand, PageResult<FinancingDTO>>
    {

        readonly IBaseRepository<Dywq.Domain.FinancingAggregate.Financing> _financingRepository;
        readonly IBaseRepository<Company> _companyRepository;

        public GetFinancingsCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetFinancingsCommandHandler> logger,
            IBaseRepository<Dywq.Domain.FinancingAggregate.Financing> financingRepository,
             IBaseRepository<Company> companyRepository
            ) : base(capPublisher, logger)
        {
            _financingRepository = financingRepository;
            _companyRepository = companyRepository;
        }

        public override async Task<PageResult<FinancingDTO>> Handle(GetFinancingsCommand request, CancellationToken cancellationToken)
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
                sb.Add($"Status = " + request.Status);
            }

            if (request.CompanyId != 0)
            {
                sb.Add($"CompanyId = " + request.CompanyId);
            }
            var where = string.Join(" and ", sb);


            var pageData = await _financingRepository.GetPageDataAsync<Domain.FinancingAggregate.Financing>(
               pageIndex: request.PageIndex,
               pageSize: request.PageSize,
               where: where,
               order: "Status asc,Sort asc,Id desc"
               );
            var count = pageData.Total;
            if (count < 1)
            {
                return PageResult<FinancingDTO>.Success(null, count, request.PageIndex, request.PageSize, request.LinkUrl);
            }

            var companyId_arr = pageData.Data.Select(x => x.CompanyId);

            var companys = _companyRepository.Set().Where(x => companyId_arr.Contains(x.Id));

            var data = pageData.Data.Select(x =>

                new FinancingDTO()
                {
                    Id = x.Id,
                    Content = x.Content,
                    CreatedTime = x.CreatedTime,
                    Show = x.Show,
                    Sort = x.Sort,
                    Title = x.Title,
                    UpdatedTime = x.UpdatedTime,
                    CompanyId = x.CompanyId,
                    CompanyName = companys.FirstOrDefault(c => c.Id == x.CompanyId)?.Name,
                    Status = x.Status,
                    Bank = x.Bank,
                    Pic = x.Pic
                }
            ); ;

            return PageResult<FinancingDTO>.Success(data, count, request.PageIndex, request.PageSize, request.LinkUrl);

        }
    }
}
