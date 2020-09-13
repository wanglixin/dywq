using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Commpany;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands
{
    /// <summary>
    /// 根据企业id获取企业动态信息
    /// </summary>
    public class GetCompanyInfoCommand : IRequest<CompanyInfoDTO>
    {

        [Range(0, int.MaxValue, ErrorMessage = "企业id参数错误")]
        /// <summary>
        /// 企业id
        /// </summary>
        public int CompanyId { get; set; }

    }

    public class GetCompanyInfoCommandHandler : IRequestHandler<GetCompanyInfoCommand, CompanyInfoDTO>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<GetCompanyInfoCommandHandler> _logger;

        readonly IBaseRepository<Company> _companyRepository;
        readonly IBaseRepository<CompanyType> _companyTypeRepository;
        public GetCompanyInfoCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetCompanyInfoCommandHandler> logger,
            IBaseRepository<Company> companyRepository,
            IBaseRepository<CompanyType> companyTypeRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;
            _companyRepository = companyRepository;
            _companyTypeRepository = companyTypeRepository;
        }

        public async Task<CompanyInfoDTO> Handle(GetCompanyInfoCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.Set().FirstOrDefaultAsync(x => x.Id == request.CompanyId);
            if (company == null)
            {
                return null;
            }

            var type = await _companyTypeRepository.Set().FirstOrDefaultAsync(x => x.Id == company.CompanyTypeId);

            var dto = new CompanyInfoDTO
            {
                CompanyId = company.Id,
                CompanyTypeId = company.CompanyTypeId,
                CompanyTypeName = type?.Name,
                Contact = company.Contact,
                CooperationContent = company.CooperationContent,
                Introduce = company.Introduce,
                IntroduceImage = company.IntroduceImage,
                MainBusiness = company.MainBusiness,
                Show = company.Show,
                Sort = company.Sort,
                Status = company.Status,
                CompanyName = company.Name
        };

            return dto;

        }
}
}
