using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Commpany;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands
{
    public class GetCompanyTypesCommand : IRequest<IEnumerable<CompanyTypeDTO>>
    {
    }



    public class GetCompanyTypesCommandHandler : IRequestHandler<GetCompanyTypesCommand, IEnumerable<CompanyTypeDTO>>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<GetCompanyTypesCommandHandler> _logger;
        readonly IBaseRepository<CompanyType> _companyTypeRepository;
        public GetCompanyTypesCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetCompanyTypesCommandHandler> logger,
            IBaseRepository<CompanyType> companyTypeRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;
            _companyTypeRepository = companyTypeRepository;
        }

        public async Task<IEnumerable<CompanyTypeDTO>> Handle(GetCompanyTypesCommand request, System.Threading.CancellationToken cancellationToken)
        {
            var types = await _companyTypeRepository.Set().OrderBy(x => x.Sort).ToListAsync();

            var list = types.Select(x => new CompanyTypeDTO()
            {
                Id = x.Id,
                Name = x.Name,
                Sort = x.Sort
            });

            return list;

        }
    }
}
