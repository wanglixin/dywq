using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
using Dywq.Domain.InvestmentAggregate;
using Dywq.Domain.News;
using Dywq.Domain.SiteAggregate;
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
    public class GetEditorTodoInfoCommand : IRequest<EditorStatisticInfoDto>
    {
    }

    public class GetEditorTodoInfoCommanHandler : BaseRequestHandler<GetEditorTodoInfoCommand, EditorStatisticInfoDto>
    {

        readonly IBaseRepository<Domain.FinancingAggregate.Financing> _financingRepository;
        readonly IBaseRepository<Domain.CompanyAggregate.Company> _companyRepository;
        readonly IBaseRepository<Domain.CooperationAggregate.CooperationInfo> _cooperationInfoRepository;
        readonly IBaseRepository<Domain.Purchase.Purchase> _purchaseRepository;
        readonly IBaseRepository<PolicyArticle> _policyRepository;
        readonly IBaseRepository<Domain.CompanyAggregate.CompanyNews> _companyNewsRepository;

        readonly IBaseRepository<AboutUs> _aboutUsRepository;

        readonly IBaseRepository<Domain.Expert.Expert> _expertRepository;
        readonly IBaseRepository<InvestmentInfo> _investmentInfoRepository;
        readonly IBaseRepository<NoticeNews> _noticeNewsRepository;

        readonly IBaseRepository<PartyBuildingArticle> _partyBuildingArticleRepository;
        readonly IBaseRepository<PolicyArticle> _policyArticleRepository;





        public GetEditorTodoInfoCommanHandler(
            ICapPublisher capPublisher,
            ILogger<GetEditorTodoInfoCommanHandler> logger,
            IBaseRepository<Domain.FinancingAggregate.Financing> financingRepository,
            IBaseRepository<Domain.CompanyAggregate.Company> companyRepository,
            IBaseRepository<Domain.CooperationAggregate.CooperationInfo> cooperationInfoRepository,
            IBaseRepository<Domain.Purchase.Purchase> purchaseRepository,
            IBaseRepository<PolicyArticle> policyRepository,
               IBaseRepository<Domain.CompanyAggregate.CompanyNews> companyNewsRepository,
             IBaseRepository<AboutUs> aboutUsRepository,
             IBaseRepository<Domain.Expert.Expert> expertRepository,
             IBaseRepository<InvestmentInfo> investmentInfoRepository,
             IBaseRepository<NoticeNews> noticeNewsRepository,
             IBaseRepository<PartyBuildingArticle> partyBuildingArticleRepository,
             IBaseRepository<PolicyArticle> policyArticleRepository
            ) : base(capPublisher, logger)
        {
            _companyRepository = companyRepository;
            _financingRepository = financingRepository;
            _cooperationInfoRepository = cooperationInfoRepository;
            _purchaseRepository = purchaseRepository;
            _policyRepository = policyRepository;
            _companyNewsRepository = companyNewsRepository;
            _aboutUsRepository = aboutUsRepository;
            _expertRepository = expertRepository;
            _investmentInfoRepository = investmentInfoRepository;
            _noticeNewsRepository = noticeNewsRepository;
            _partyBuildingArticleRepository = partyBuildingArticleRepository;
            _policyArticleRepository = policyArticleRepository;
        }

        public override async Task<EditorStatisticInfoDto> Handle(GetEditorTodoInfoCommand request, CancellationToken cancellationToken)
        {
            var info = new EditorStatisticInfoDto();


            info = new EditorStatisticInfoDto
            {
                FinancingCount = await _financingRepository.Set().CountAsync(x => (x.Status == 0 && x.CompanyId == 0)),
                CompanyInfoCount = await _companyNewsRepository.Set().CountAsync(x => (x.Status == 0)),
                CooperationInfoCount = await _cooperationInfoRepository.Set().CountAsync(x => (x.Status == 0 && x.CompanyId == 0)),
                PurchaseCount0 = await _purchaseRepository.Set().CountAsync(x => (x.Status == 0) && x.Type == 0 && x.CompanyId == 0),
                PurchaseCount1 = await _purchaseRepository.Set().CountAsync(x => (x.Status == 0) && x.Type == 1 && x.CompanyId == 0),
                CooperationInfoTotalCount = await _cooperationInfoRepository.Set().CountAsync(x => (x.Status == 1 && x.CompanyId == 0)),
    

                AboutusCount = await _aboutUsRepository.Set().CountAsync(x => x.Status == 0),
                CompanyCount = await _companyRepository.Set().CountAsync(x => x.Status == 0),
                ExpertCount = await _expertRepository.Set().CountAsync(x => x.Status == 0),
                InvestmentCount = await _investmentInfoRepository.Set().CountAsync(x => x.Status == 0 && x.CompanyId == 0),
                NewsCount = await _noticeNewsRepository.Set().CountAsync(x => x.Status == 0),
                PartyBuildingArticleCount = await _partyBuildingArticleRepository.Set().CountAsync(x => x.Status == 0),
                PolicyArticleCount = await _policyArticleRepository.Set().CountAsync(x => x.Status == 0)
            };
            return info;

        }
    }
}
