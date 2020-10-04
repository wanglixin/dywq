using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Commpany;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Message
{
    public class GetTodoInfoCommand : IRequest<StatisticInfoDto>
    {

        public int? CompanyId { get; set; }
    }


    public class GetTodoInfoCommandHandler : BaseRequestHandler<GetTodoInfoCommand, StatisticInfoDto>
    {

        readonly IBaseRepository<Domain.FinancingAggregate.Financing> _financingRepository;
        readonly IBaseRepository<Domain.CompanyAggregate.Company> _companyRepository;
        readonly IBaseRepository<Domain.CooperationAggregate.CooperationInfo> _cooperationInfoRepository;
        readonly IBaseRepository<Domain.Purchase.Purchase> _purchaseRepository;
        readonly IBaseRepository<PolicyArticle> _policyRepository;
        // readonly IBaseRepository<Domain.CompanyAggregate.Message> _messageRepository;


        public GetTodoInfoCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetTodoInfoCommandHandler> logger,
            IBaseRepository<Domain.FinancingAggregate.Financing> financingRepository,
            IBaseRepository<Domain.CompanyAggregate.Company> companyRepository,
            IBaseRepository<Domain.CooperationAggregate.CooperationInfo> cooperationInfoRepository,
            IBaseRepository<Domain.Purchase.Purchase> purchaseRepository,
            IBaseRepository<PolicyArticle> policyRepository
            //  IBaseRepository<Domain.CompanyAggregate.Message> messageRepository
            ) : base(capPublisher, logger)
        {
            _companyRepository = companyRepository;
            _financingRepository = financingRepository;
            _cooperationInfoRepository = cooperationInfoRepository;
            _purchaseRepository = purchaseRepository;
            _policyRepository = policyRepository;
            // _messageRepository = messageRepository;
        }

        public override async Task<StatisticInfoDto> Handle(GetTodoInfoCommand request, CancellationToken cancellationToken)
        {
            var info = new StatisticInfoDto
            {
                FinancingCount = await _financingRepository.Set().CountAsync(x => x.CompanyId == request.CompanyId && (x.Status == 0 || x.Status == -1)),
                CompanyInfoCount = await _companyRepository.Set().CountAsync(x => x.Id == request.CompanyId && (x.Status == 1 || x.Status == -1)),
                CooperationInfoCount = await _cooperationInfoRepository.Set().CountAsync(x => x.CompanyId == request.CompanyId && (x.Status == 0 || x.Status == -1))
                ,
                PurchaseCount0 = await _purchaseRepository.Set().CountAsync(x => x.CompanyId == request.CompanyId && (x.Status == 0 || x.Status == -1) & x.Type == 0),
                PurchaseCount1 = await _purchaseRepository.Set().CountAsync(x => x.CompanyId == request.CompanyId && (x.Status == 0 || x.Status == -1) & x.Type == 1)
               

            };


            //var pageData = await _messageRepository.GetPageDataAsync<Domain.CompanyAggregate.Message>(
            //    pageIndex: request.PageIndex,
            //    pageSize: request.PageSize,
            //    where: where);
            //var count = pageData.Total;

            // info.PolicyPageResult=


            return info;

        }
    }
}
