using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
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
    public class DelCompanyCommand : IRequest<Result>
    {
        [Range(1, int.MaxValue, ErrorMessage = "企业id错误")]
        public int CompanyId { get; set; }
    }

    public class DelCompanyCommandHandler : IRequestHandler<DelCompanyCommand, Result>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<DelCompanyCommandHandler> _logger;

        readonly IBaseRepository<CompanyFieldData> _companyFieldDataRepository;

        readonly IBaseRepository<Company> _companyRepository;

        public DelCompanyCommandHandler(
             ICapPublisher capPublisher,
            ILogger<DelCompanyCommandHandler> logger,
             IBaseRepository<CompanyFieldData> companyFieldDataRepository,
             IBaseRepository<Company> companyRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;
            _companyFieldDataRepository = companyFieldDataRepository;
            _companyRepository = companyRepository;
        }

        public async Task<Result> Handle(DelCompanyCommand request, CancellationToken cancellationToken)
        {
            var _company = await _companyRepository.Set().FindAsync(request.CompanyId);
            if (_company != null)
            {
                 _companyRepository.Set().Remove(_company);
            }

            var _data = await _companyFieldDataRepository.Set().Where(x => x.CompanyId == request.CompanyId).ToListAsync();

            _companyFieldDataRepository.Set().RemoveRange(_data);

            return Result.Success(); ;
        }
    }
}
