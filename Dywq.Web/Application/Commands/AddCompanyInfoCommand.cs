using Dywq.Infrastructure.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using Dywq.Infrastructure.Repositories;
using Dywq.Domain.CompanyAggregate;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Dywq.Web.Application.Commands
{
    /// <summary>
    /// 企业用户上传企业信息
    /// </summary>
    public class AddCompanyInfoCommand : IRequest<Result>
    {

        //[Range(1, int.MaxValue, ErrorMessage = "企业id参数错误")]
        /// <summary>
        /// 企业id
        /// </summary>
        public int CompanyId { get; set; }

        [RegularExpression(@"^[1-9]+[\d]*$", ErrorMessage = "企业类型id参数错误")]
        /// <summary>
        /// 企业类型id
        /// </summary>
        public string CompanyTypeId { get; set; }


        [Required(ErrorMessage = "企业介绍图片不能为空")]
        /// <summary>
        /// 企业介绍图片
        /// </summary>
        public string IntroduceImage { get; set; }


        [Required(ErrorMessage = "企业概况不能为空")]
        /// <summary>
        /// 企业介绍，概况
        /// </summary>
        public string Introduce { get; set; }


        [Required(ErrorMessage = "主营业务不能为空")]
        /// <summary>
        /// 主营业务
        /// </summary>
        public string MainBusiness { get; set; }


        [Required(ErrorMessage = "拟对外合作内容不能为空")]
        /// <summary>
        /// 合作内容
        /// </summary>
        public string CooperationContent { get; set; }


        [Required(ErrorMessage = "联系方式不能为空")]
        /// <summary>
        /// 联系方式，联系信息
        /// </summary>
        public string Contact { get; set; }

    }

    public class AddCompanyInfoCommandHandler : IRequestHandler<AddCompanyInfoCommand, Result>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<AddCompanyInfoCommandHandler> _logger;

        readonly IBaseRepository<Company> _companyRepository;
        readonly IBaseRepository<CompanyType> _companyTypeRepository;

        public AddCompanyInfoCommandHandler(
            ICapPublisher capPublisher,
            ILogger<AddCompanyInfoCommandHandler> logger,
            IBaseRepository<Company> companyRepository,
            IBaseRepository<CompanyType> companyTypeRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;
            _companyRepository = companyRepository;
            _companyTypeRepository = companyTypeRepository;
        }

        public async Task<Result> Handle(AddCompanyInfoCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.Set().FirstOrDefaultAsync(x => x.Id == request.CompanyId);
            if (company == null)
            {
                return Result.Failure("企业不存在");
            }

            if (company.Status != 0 && company.Status != -1)
            {
                return Result.Failure("当前状态不能修改 ");
            }
            var companyTypeId = Convert.ToInt32(request.CompanyTypeId);
            if (!await _companyTypeRepository.AnyAsync(x => x.Id == companyTypeId))
            {
                return Result.Failure("企业类型id不存在");
            }

            company.Status = 1;
            company.IntroduceImage = request.IntroduceImage;
            company.Contact = request.Contact;
            company.CooperationContent = request.CooperationContent;
            company.Introduce = request.Introduce;
            company.MainBusiness = request.MainBusiness;
            company.CompanyTypeId = companyTypeId;

            await _companyRepository.UpdateAsync(company);

            return Result.Success();

        }
    }
}
