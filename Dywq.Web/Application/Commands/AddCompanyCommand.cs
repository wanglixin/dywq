using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Commpany;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Dywq.Web.Dto.Commpany.CompanyFieldsDto;

namespace Dywq.Web.Application.Commands
{
    public class AddCompanyCommand : IRequest<Result>
    {
        [Required(ErrorMessage = "请上传logo")]
        public string Logo { get; set; }
        public IEnumerable<FieldDataItemDto> FieldDataItems { get; set; }
    }

    public class AddCompanyFieldDataCommandHandler : IRequestHandler<AddCompanyCommand, Result>
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

        public async Task<Result> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
        {
            //check 

            var items = new List<CompanyFieldData>();
            foreach (var x in request.FieldDataItems)
            {
                if (x.Required && string.IsNullOrWhiteSpace(x.Value))
                {
                    return Result.Failure($"{x.Name} 不能为空");
                }

                items.Add(new CompanyFieldData
                {
                    Alias = x.Alias,
                    // CompanyId = company.Id,
                    FieldId = x.FieldId,
                    Type = x.Type,
                    Value = x.Value
                });
            }

            var companyName = request.FieldDataItems.FirstOrDefault(x => x.Alias == Common.CompanyFieldAlias.CompanyName);
            if (!string.IsNullOrWhiteSpace(companyName.Value))
            {
                //判断企业名称是否重复
                var exist = await companyFieldDataRepository.AnyAsync(x => x.Alias == Common.CompanyFieldAlias.CompanyName && x.Value == companyName.Value);
                if (exist) return Result.Failure("企业名称重复");

            }

            var company = new Company()
            {
                Logo = request.Logo,
            };
            await companyRepository.AddAsync(company, cancellationToken);

            var ret = await companyRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            if (ret < 1) return Result.Failure("操作失败!");

            items.ForEach(x =>
            {
                x.CompanyId = company.Id;
            });

            await companyFieldDataRepository.BatchAddAsync(items);

            ret = await companyFieldDataRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            if (ret < 1) return Result.Failure("操作失败!");

            return Result.Success();

        }
    }
}
