using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Commpany;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Dywq.Web.Dto.Commpany.CompanyFieldsDto;

namespace Dywq.Web.Application.Commands
{
    public class AddCompanyFieldDataCommand : IRequest<Result>
    {
        public string Logo { get; set; }
        public IEnumerable<FieldDataItemDto> FieldDataItems { get; set; }
    }

    public class AddCompanyFieldDataCommandHandler : IRequestHandler<AddCompanyFieldDataCommand, Result>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<GetCompanyFieldsCommandHandler> _logger;
        readonly IBaseRepository<CompanyFieldData> companyFieldDataRepository;
        readonly IBaseRepository<Company> companyRepository;

        public AddCompanyFieldDataCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetCompanyFieldsCommandHandler> logger,
            IBaseRepository<CompanyFieldData> _companyFieldDataRepository,
            IBaseRepository<Company> _companyRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;

            companyFieldDataRepository = _companyFieldDataRepository;
            companyRepository = _companyRepository;
        }

        public async Task<Result> Handle(AddCompanyFieldDataCommand request, CancellationToken cancellationToken)
        {
            var company = new Company()
            {
                Logo = request.Logo,
            };
            await companyRepository.AddAsync(company, cancellationToken);

            var ret = await companyRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            if (ret < 1) return Result.Failure("操作失败!");
          
            var items = request.FieldDataItems.Select(x => new CompanyFieldData
            {
                Alias = x.Alias,
                CompanyId = company.Id,
                FieldId = x.FieldId,
                Type = x.Type,
                Value = x.Value
            });

            await companyFieldDataRepository.BatchAddAsync(items);

            ret = await companyFieldDataRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            if (ret < 1) return Result.Failure("操作失败!");

            return Result.Success();

        }
    }
}
