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
using System.Threading;
using System.Threading.Tasks;
using static Dywq.Web.Dto.Commpany.CompanyFieldsDto;
using static Dywq.Web.Dto.Commpany.CompanyFieldsDto.CompanyFieldGroupItem;
using static Dywq.Web.Dto.Commpany.CompanyFieldsDto.CompanyFieldGroupItem.CompanyFieldItem;

namespace Dywq.Web.Application.Commands
{
    public class GetCompanyFieldsCommand : IRequest<CompanyFieldsDto>
    {

    }

    public class GetCompanyFieldsCommandHandler : IRequestHandler<GetCompanyFieldsCommand, CompanyFieldsDto>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<GetCompanyFieldsCommandHandler> _logger;
        readonly IBaseRepository<CompanyFieldGroup> companyFieldGroupRepository;
        readonly IBaseRepository<CompanyField> companyFieldRepository;

        IBaseRepository<CompanyFieldDefaultValue> companyFieldDefaultValueRepository;
        public GetCompanyFieldsCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetCompanyFieldsCommandHandler> logger,
            IBaseRepository<CompanyFieldGroup> _companyFieldGroupRepository,
            IBaseRepository<CompanyField> _companyFieldRepository,
             IBaseRepository<CompanyFieldDefaultValue> _companyFieldDefaultValueRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;

            companyFieldGroupRepository = _companyFieldGroupRepository;
            companyFieldRepository = _companyFieldRepository;
            companyFieldDefaultValueRepository = _companyFieldDefaultValueRepository;
        }

        public async Task<CompanyFieldsDto> Handle(GetCompanyFieldsCommand request, CancellationToken cancellationToken)
        {
            var dto = new CompanyFieldsDto();
            dto.Groups = new List<CompanyFieldGroupItem>();
            var groups = await companyFieldGroupRepository.Set().OrderBy(x => x.Sort).ToListAsync();
            var fields = await companyFieldRepository.Set().OrderBy(x => x.Sort).ToListAsync();
            var fieldVlaues = await companyFieldDefaultValueRepository.Set().OrderBy(x => x.Sort).ToListAsync();

            groups.ForEach(g =>
            {
                var group = new CompanyFieldGroupItem
                {
                    Id = g.Id,
                    Name = g.Name,
                    Sort = g.Sort,
                    CompanyFieldItems = new List<CompanyFieldItem>()
                };
                var _fields = fields.Where(f => f.GroupId == g.Id).ToList();
                _fields.ForEach(f =>
                {
                    var fieldItem = new CompanyFieldItem
                    {
                        Id = f.Id,
                        Name = f.Name,
                        Sort = f.Sort,
                        GroupId = f.GroupId,
                        Alias = f.Alias,
                        Type = f.Type,
                        Required = f.Required,
                        CompanyFieldDefaultValues = new List<CompanyFieldDefaultValueItem>()
                    };

                    var _fieldVlaues = fieldVlaues.Where(v => v.CompanyFieldId == f.Id).ToList();

                    _fieldVlaues.ForEach(v =>
                    {
                        var valueItem = new CompanyFieldDefaultValueItem
                        {
                            Id = v.Id,
                            Sort = v.Sort,
                            Text = v.Text,
                            Value = v.Value,
                            FieldId = v.CompanyFieldId
                        };

                        fieldItem.CompanyFieldDefaultValues.Add(valueItem);
                    });
                    group.CompanyFieldItems.Add(fieldItem);
                });

                dto.Groups.Add(group);
            });


            return dto;

        }
    }
}
