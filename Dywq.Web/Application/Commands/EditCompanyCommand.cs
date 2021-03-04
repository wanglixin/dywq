using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Commpany;
using Dywq.Web.Dto.User;
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
    public class EditCompanyCommand : IRequest<Result>
    {
        [Range(1, int.MaxValue, ErrorMessage = "企业id错误")]
        public int CompanyId { get; set; }

       // [Required(ErrorMessage = "请上传logo")]
        public string Logo { get; set; }

        [Required(ErrorMessage = "请填写企业名称")]
        public string Name { get; set; }

        public IEnumerable<FieldDataItemDto> FieldDataItems { get; set; }

        public LoginUserDTO LoginUser { get; set; }
    }


    public class EditCompanyCommandHandler : IRequestHandler<EditCompanyCommand, Result>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<EditCompanyCommandHandler> _logger;
        readonly IBaseRepository<CompanyFieldData> _companyFieldDataRepository;
        readonly IBaseRepository<Company> _companyRepository;

        public EditCompanyCommandHandler(
            ICapPublisher capPublisher,
            ILogger<EditCompanyCommandHandler> logger,
            IBaseRepository<CompanyFieldData> companyFieldDataRepository,
            IBaseRepository<Company> companyRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;

            _companyFieldDataRepository = companyFieldDataRepository;
            _companyRepository = companyRepository;
        }

        public async Task<Result> Handle(EditCompanyCommand request, CancellationToken cancellationToken)
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

            //var companyName = request.FieldDataItems.FirstOrDefault(x => x.Alias == Common.CompanyFieldAlias.CompanyName);
            //if (!string.IsNullOrWhiteSpace(companyName.Value))
            //{
            //    //判断企业名称是否重复
            //    var exist = await _companyFieldDataRepository.AnyAsync(x => x.Alias == Common.CompanyFieldAlias.CompanyName && x.Value == companyName.Value && x.CompanyId != request.CompanyId);
            //    if (exist) return Result.Failure("企业名称重复");
            //}

            if (await _companyRepository.AnyAsync(x => x.Name == request.Name && x.Id != request.CompanyId)) return Result.Failure("企业名称重复");


            var company = await _companyRepository.Set().FindAsync(request.CompanyId);
            company.Logo = request.Logo;
            company.Name = request.Name;

            if (request.LoginUser.Type == 2 && company.Status != -1)
            {
                return Result.Failure("当前状态不能编辑");

            }

            if (request.LoginUser.Type == 2)
            {
                company.Status = 0;

            }
            await _companyRepository.UpdateAsync(company, cancellationToken);

            var ret = await _companyRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            if (ret < 1) return Result.Failure("操作失败!");


            var data = await _companyFieldDataRepository.Set().Where(x => x.CompanyId == request.CompanyId).ToListAsync();

            items.ForEach(x =>
            {
                x.CompanyId = company.Id;
            });

            foreach (var item in items)
            {
                var _item = data.FirstOrDefault(x => x.FieldId == item.FieldId);
                if (_item == null)
                {
                    //新增
                    _item = new CompanyFieldData()
                    {
                        Type = item.Type,
                        Value = item.Value,
                        FieldId = item.FieldId,
                        CompanyId = company.Id,
                        Alias = item.Alias
                    };

                    await _companyFieldDataRepository.AddAsync(_item);
                }
                else
                {
                    _item.Value = item.Value;
                    _item.Alias = item.Alias;
                    _item.UpdatedTime = DateTime.Now;
                    _item.Type = item.Type;

                    await _companyFieldDataRepository.UpdateAsync(_item);

                }
            }

            return Result.Success();

        }
    }

}
