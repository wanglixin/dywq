using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.CooperationAggregate;
using Dywq.Domain.UserAggregate;
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

namespace Dywq.Web.Application.Commands.CompanyNews
{
    /// <summary>
    /// 编辑或新增
    /// </summary>
    public class EditCompanyNewsCommand : IRequest<Result>
    {
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public string Id { get; set; }


        public string CompanyId { get; set; }


        [Required(ErrorMessage = "请选择类型")]
        /// <summary>
        /// 类型id
        /// </summary>
        public string CompanyTypeId { get; set; }


        [Required(ErrorMessage = "标题不能为空")]
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }


        // [Required(ErrorMessage = "请上传图片")]
        /// <summary>
        /// 内容
        /// </summary>
        public string IntroduceImage { get; set; }



        [Required(ErrorMessage = "企业介绍不能为空")]
        /// <summary>
        /// 企业介绍，概况
        /// </summary>
        public string Introduce { get; set; }

        [Required(ErrorMessage = "主营业务不能为空")]
        /// <summary>
        /// 主营业务
        /// </summary>
        public string MainBusiness { get; set; }



        [Required(ErrorMessage = "合作内容不能为空")]
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
        public string Sort { get; set; } = "0";

        [RegularExpression("^[0|1]+$", ErrorMessage = "显示值错误")]
        public string Show { get; set; } = "1";


        [Required(ErrorMessage = "请选择审核状态")]
        [Range(-1, 1, ErrorMessage = "请选择审核状态")]
        /// <summary>
        /// 审核状态，0 提交信息审核，待审核 1：审核通过 -1：审核失败
        /// </summary>
        public string Status { get; set; } = "";


        /// <summary>
        /// 用户id，可能是企业用户或管理员
        /// </summary>
        public int UserId { get; set; }


    }

    public class EditCompanyNewsCommandHandler : BaseRequestHandler<EditCompanyNewsCommand, Result>
    {

        readonly IBaseRepository<Domain.CompanyAggregate.CompanyNews> _companyNewsRepository;
        readonly IBaseRepository<CompanyType> _companyTypeRepository;
        readonly IBaseRepository<User> _userRepository;
        readonly IBaseRepository<CompanyUser> _companyUserRepository;

        public EditCompanyNewsCommandHandler(
            ICapPublisher capPublisher,
            ILogger<EditCompanyNewsCommandHandler> logger,
            IBaseRepository<Domain.CompanyAggregate.CompanyNews> companyNewsRepository,
             IBaseRepository<CompanyType> companyTypeRepository,
             IBaseRepository<User> userRepository,
             IBaseRepository<CompanyUser> companyUserRepository
            ) : base(capPublisher, logger)
        {
            _companyNewsRepository = companyNewsRepository;
            _companyTypeRepository = companyTypeRepository;
            _userRepository = userRepository;
            _companyUserRepository = companyUserRepository;
        }

        public override async Task<Result> Handle(EditCompanyNewsCommand request, CancellationToken cancellationToken)
        {
            var id = string.IsNullOrWhiteSpace(request.Id) ? 0 : Convert.ToInt32(request.Id);
            var show = request.Show == "1";
            var sort = Convert.ToInt32(request.Sort);
            var status = Convert.ToInt32(request.Status);

            var typeId = Convert.ToInt32(request.CompanyTypeId);

            if (!await _companyTypeRepository.AnyAsync(x => x.Id == typeId))
            {
                return Result.Failure($"typeId={typeId}类型不存在");
            }

            var user = await _userRepository.Set().FirstOrDefaultAsync(x => x.Id == request.UserId);

            if (user == null)
            {
                return Result.Failure($"用户不存在");
            }

            if (id <= 0) //新增
            {
                var item = new Domain.CompanyAggregate.CompanyNews()
                {
                    // CompanyId = company_user.CompanyId,
                    CompanyTypeId = typeId,
                    Contact = request.Contact,
                    CooperationContent = request.CooperationContent,
                    Introduce = request.Introduce,
                    IntroduceImage = request.IntroduceImage,
                    MainBusiness = request.MainBusiness,
                    Show = true,
                    Sort = sort,
                    Status = 0,
                    Title = request.Title,
                    UserId = request.UserId

                };
                //if (user.Type != 0)
                //{
                //    return Result.Failure($"只能企业用户添加");
                //}
                if (user.Type == 0)//|| user.Type == 2)
                {
                    var company_user = await _companyUserRepository.Set().FirstOrDefaultAsync(x => x.UserId == request.UserId);
                    if (company_user == null)
                    {
                        return Result.Failure($"您还未绑定企业");
                    }
                    item.CompanyId = company_user.CompanyId;
                }
                else if (user.Type == 1)
                {
                    item.Status = 1;
                }
                await _companyNewsRepository.AddAsync(item);
            }
            else
            {
                //修改
                var item = await _companyNewsRepository.Set().FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return Result.Failure($"id={request.Id}错误,内容不存在");
                }

                if (user.Type != 1 && item.Status == 1)
                {
                    return Result.Failure($"当前状态不能修改！");
                }

                item.CompanyTypeId = typeId;
                item.Contact = request.Contact;
                item.CooperationContent = request.CooperationContent;
                item.Introduce = request.Introduce;
                item.IntroduceImage = request.IntroduceImage;
                item.MainBusiness = request.MainBusiness;
                //item.Show = show;
                //item.Sort = sort;
                item.Status = 0;
                item.Title = request.Title;


                if (user.Type == 0) //用户修改的情况
                {
                    if (item.Status != -1)
                    {
                        return Result.Failure($"当前状态不能修改");
                    }

                }

                if (user.Type == 1) //管理员修改
                {
                    if (string.IsNullOrWhiteSpace(request.Status))
                    {
                        return Result.Failure($"请选择审核状态");
                    }

                    item.Show = show;
                    item.Sort = sort;
                    item.Status = status;
                }


                await _companyNewsRepository.UpdateAsync(item);
            }

            return Result.Success();

        }
    }

}
