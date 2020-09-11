using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
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
using static Dywq.Web.Dto.Commpany.CompanyFieldsDTO;
using static Dywq.Web.Dto.Commpany.CompanyFieldsDTO.CompanyFieldGroupItem;
using static Dywq.Web.Dto.Commpany.CompanyFieldsDTO.CompanyFieldGroupItem.CompanyFieldItem;

namespace Dywq.Web.Application.Commands
{
    
    public class GetCompanyCommand : IRequest<CompanyEditDto>
    {
        [Range(1, int.MaxValue, ErrorMessage = "企业id错误")]
        public int CompanyId { get; set; }
    }

    public class GetCompanyCommandHandler : IRequestHandler<GetCompanyCommand, CompanyEditDto>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<GetCompanyFieldsCommandHandler> _logger;
        readonly IBaseRepository<CompanyFieldGroup> _companyFieldGroupRepository;
        readonly IBaseRepository<CompanyField> _companyFieldRepository;

        readonly IBaseRepository<CompanyFieldData> _companyFieldDataRepository;

        IBaseRepository<CompanyFieldDefaultValue> _companyFieldDefaultValueRepository;
        IBaseRepository<Company> _companyRepository;

        public GetCompanyCommandHandler(
             ICapPublisher capPublisher,
            ILogger<GetCompanyFieldsCommandHandler> logger,
            IBaseRepository<CompanyFieldGroup> companyFieldGroupRepository,
            IBaseRepository<CompanyField> companyFieldRepository,
             IBaseRepository<CompanyFieldDefaultValue> companyFieldDefaultValueRepository,
             IBaseRepository<CompanyFieldData> companyFieldDataRepository,
             IBaseRepository<Company> companyRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;

            _companyFieldGroupRepository = companyFieldGroupRepository;
            _companyFieldRepository = companyFieldRepository;
            _companyFieldDefaultValueRepository = companyFieldDefaultValueRepository;
            _companyFieldDataRepository = companyFieldDataRepository;
            _companyRepository = companyRepository;
        }

        public async Task<CompanyEditDto> Handle(GetCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = new CompanyEditDto();
            var dto = new CompanyFieldsDTO
            {
                Groups = new List<CompanyFieldGroupItem>()
            };
            var _company = await _companyRepository.Set().FindAsync(request.CompanyId);
            company.CompanyId = _company.Id;
            company.Logo = _company.Logo;
            company.Name = _company.Name;

            var groups = await _companyFieldGroupRepository.Set().OrderBy(x => x.Sort).ToListAsync();
            var fields = await _companyFieldRepository.Set().OrderBy(x => x.Sort).ToListAsync();
            var fieldVlaues = await _companyFieldDefaultValueRepository.Set().OrderBy(x => x.Sort).ToListAsync();

            var data = await _companyFieldDataRepository.Set().Where(x => x.CompanyId == request.CompanyId).ToListAsync();


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
                    var _value = data.FirstOrDefault(x => x.FieldId == f.Id);
                    var fieldItem = new CompanyFieldItem
                    {
                        Id = f.Id,
                        Name = f.Name,
                        Sort = f.Sort,
                        GroupId = f.GroupId,
                        Alias = f.Alias,
                        Type = f.Type,
                        Required = f.Required,
                        CompanyFieldDefaultValues = new List<CompanyFieldDefaultValueItem>(),
                        Value = _value?.Value
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

            company.Fields = dto;
            return company;


        }
    }
}
