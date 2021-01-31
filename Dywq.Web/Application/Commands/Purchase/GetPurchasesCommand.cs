using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.CooperationAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Cooperation;
using Dywq.Web.Dto.Financing;
using Dywq.Web.Dto.Purchase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Purchase
{
    public class GetPurchasesCommand : IRequest<PageResult<PurchaseDTO>>
    {
        public int Id { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string LinkUrl { get; set; }

        public bool? Show { get; set; } = null;

        public int CompanyId { get; set; } = 0;

        /// <summary>
        /// -888 管理员后台搜索
        /// </summary>
        public int Status { get; set; } = -999;

        public int Type { get; set; } = -999;


        public string Key { get; set; }


    }

    public class GetPurchasesCommandHandler : BaseRequestHandler<GetPurchasesCommand, PageResult<PurchaseDTO>>
    {

        readonly IBaseRepository<Dywq.Domain.Purchase.Purchase> _purchaseRepository;
        readonly IBaseRepository<Company> _companyRepository;

        public GetPurchasesCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetPurchasesCommandHandler> logger,
            IBaseRepository<Dywq.Domain.Purchase.Purchase> purchaseRepository,
             IBaseRepository<Company> companyRepository
            ) : base(capPublisher, logger)
        {
            _purchaseRepository = purchaseRepository;
            _companyRepository = companyRepository;
        }

        public override async Task<PageResult<PurchaseDTO>> Handle(GetPurchasesCommand request, CancellationToken cancellationToken)
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

            if (request.Type != -999)
            {
                sb.Add($"Type = " + request.Type);
            }

            if (!string.IsNullOrWhiteSpace(request.Key))
            {
                sb.Add($"ProductName like '%{ request.Key}%'");
            }




            var where = string.Join(" and ", sb);
            var pageData = await _purchaseRepository.GetPageDataAsync<Domain.Purchase.Purchase>(
               pageIndex: request.PageIndex,
               pageSize: request.PageSize,
               where: where,
               order: "Status asc,Sort asc,Id desc"
               );
            var count = pageData.Total;
            if (count < 1)
            {
                return PageResult<PurchaseDTO>.Success(null, count, request.PageIndex, request.PageSize, request.LinkUrl);
            }

            var companyId_arr = pageData.Data.Select(x => x.CompanyId);

            var companys = _companyRepository.Set().Where(x => companyId_arr.Contains(x.Id));

            var data = pageData.Data.Select(x =>

                new PurchaseDTO()
                {
                    Type = x.Type,
                    Status = x.Status,
                    Sort = x.Sort,
                    Show = x.Show,
                    ProductName = x.ProductName,
                    Mobile = x.Mobile,
                    Content = x.Content,
                    CompanyId = x.CompanyId,
                    CompanyName = companys.FirstOrDefault(c => c.Id == x.CompanyId)?.Name,
                    Contacts = x.Contacts,
                    Id = x.Id,
                    CreatedTime = x.CreatedTime,
                    UpdatedTime = x.UpdatedTime
                }
            ); ;

            return PageResult<PurchaseDTO>.Success(data, count, request.PageIndex, request.PageSize, request.LinkUrl);

        }
    }
}
