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

    /// <summary>
    /// 管理员编辑企业动态信息
    /// </summary>
    public class EditCompanyInfoCommand : IRequest<Result>
    {

        [RegularExpression("^[0-9]+$", ErrorMessage = "企业id参数错误")]
        /// <summary>
        /// 企业id
        /// </summary>
        public string CompanyId { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "企业类型id参数错误")]
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

        [RegularExpression("^[0-9]+$", ErrorMessage = "排序值错误")]
        public string Sort { get; set; }

        [RegularExpression("^[0|1]+$", ErrorMessage = "排序值错误")]
        public string Show { get; set; }

    }

    public class EditCompanyInfoCommandHandler : IRequestHandler<EditCompanyInfoCommand, Result>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<EditCompanyInfoCommandHandler> _logger;

        readonly IBaseRepository<Company> _companyRepository;
        readonly IBaseRepository<CompanyType> _companyTypeRepository;

        public EditCompanyInfoCommandHandler(
            ICapPublisher capPublisher,
            ILogger<EditCompanyInfoCommandHandler> logger,
            IBaseRepository<Company> companyRepository,
            IBaseRepository<CompanyType> companyTypeRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;
            _companyRepository = companyRepository;
            _companyTypeRepository = companyTypeRepository;
        }

        public async Task<Result> Handle(EditCompanyInfoCommand request, CancellationToken cancellationToken)
        {
            var companyId = Convert.ToInt32(request.CompanyId);
            var companyTypeId = Convert.ToInt32(request.CompanyTypeId);
            var sort = Convert.ToInt32(request.Sort);
            var show = Convert.ToInt32(request.Show) == 1;
            var company = await _companyRepository.Set().FirstOrDefaultAsync(x => x.Id == companyId);
            if (company == null)
            {
                return Result.Failure("企业不存在");
            }

            if (!await _companyTypeRepository.AnyAsync(x => x.Id == companyTypeId))
            {
                return Result.Failure("企业类型id不存在");
            }

            //company.Status = 1;
            company.IntroduceImage = request.IntroduceImage;
            company.Contact = request.Contact;
            company.CooperationContent = request.CooperationContent;
            company.Introduce = request.Introduce;
            company.MainBusiness = request.MainBusiness;
            company.CompanyTypeId = companyTypeId;
            company.Sort = sort;
            company.Show = show;

            await _companyRepository.UpdateAsync(company);

            return Result.Success();

        }
    }
}
