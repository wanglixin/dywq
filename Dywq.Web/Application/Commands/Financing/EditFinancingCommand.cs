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
using Dywq.Infrastructure.Core.Extensions;

namespace Dywq.Web.Application.Commands.Financing
{
    /// <summary>
    /// 编辑或新增
    /// </summary>
    public class EditFinancingCommand : IRequest<Result>
    {
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public string Id { get; set; }

        //[Required(ErrorMessage = "请上传图片")]
        /// <summary>
        /// 图片
        /// </summary>
        public string Pic { get; set; }


        [Required(ErrorMessage = "标题不能为空")]
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        //[Required(ErrorMessage = "银行不能为空")]
        /// <summary>
        /// 银行
        /// </summary>
        public string Bank { get; set; }


        [Required(ErrorMessage = "内容不能为空")]
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }


        [RegularExpression("^[0-9]+$", ErrorMessage = "排序值错误")]
        public string Sort { get; set; } = "0";

        [RegularExpression("^[0|1]+$", ErrorMessage = "显示值错误")]
        public string Show { get; set; } = "1";


        [Required(ErrorMessage = "请选择审核状态")]
        [Range(-1, 1, ErrorMessage = "请选择审核状态")]
        /// <summary>
        /// 2:通过 -1：失败
        /// </summary>
        public string Status { get; set; } = "";


        /// <summary>
        /// 用户id，可能是企业用户或管理员
        /// </summary>
        public int UserId { get; set; }


    }

    public class EditEditFinancingCommandHandler : BaseRequestHandler<EditFinancingCommand, Result>
    {

        readonly IBaseRepository<Domain.FinancingAggregate.Financing> _financingRepository;
        readonly IBaseRepository<User> _userRepository;
        readonly IBaseRepository<CompanyUser> _companyUserRepository;

        public EditEditFinancingCommandHandler(
            ICapPublisher capPublisher,
            ILogger<EditEditFinancingCommandHandler> logger,
            IBaseRepository<Domain.FinancingAggregate.Financing> financingRepository,
             IBaseRepository<User> userRepository,
              IBaseRepository<CompanyUser> companyUserRepository
            ) : base(capPublisher, logger)
        {
            _financingRepository = financingRepository;
            _userRepository = userRepository;
            _companyUserRepository = companyUserRepository;
        }

        public override async Task<Result> Handle(EditFinancingCommand request, CancellationToken cancellationToken)
        {
            var id = string.IsNullOrWhiteSpace(request.Id) ? 0 : Convert.ToInt32(request.Id);
            var show = request.Show == "1";
            var sort = Convert.ToInt32(request.Sort);

            var user = await _userRepository.Set().FirstOrDefaultAsync(x => x.Id == request.UserId);

            if (user == null)
            {
                return Result.Failure($"用户不存在");
            }

            if (id <= 0) //新增
            {
                //if (user.Type != 0)
                //{
                //    return Result.Failure($"只有企业才可以添加");
                //}

                var item = new Domain.FinancingAggregate.Financing()
                {
                    Content = request.Content,
                    Show = true,
                    Sort = 0,
                    Bank = request.Bank,
                    Pic = request.Pic,
                    Title = request.Title,
                    // CompanyId = company_user.CompanyId,
                    Status = 0,
                    UserId = request.UserId,
                    Describe = request.Content.FilterHtml().Cut(300)

                };

                if (user.Type == 0)// || user.Type == 2)
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
                    item.Show = true;
                }

                await _financingRepository.AddAsync(item);
            }
            else
            {
                //修改
                var item = await _financingRepository.Set().FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return Result.Failure($"id={request.Id}错误,内容不存在");
                }
               



                if (user.Type != 1 && item.Status == 1)
                {
                    return Result.Failure($"当前状态不能修改！");
                }



                if (user.Type == 0)//|| user.Type == 2)
                {
                    var company_user = await _companyUserRepository.Set().FirstOrDefaultAsync(x => x.UserId == request.UserId);
                    if (company_user == null)
                    {
                        return Result.Failure($"您还未绑定企业");
                    }

                    if (item.CompanyId != company_user.CompanyId)
                    {
                        return Result.Failure($"您绑定的企业不符合");
                    }

                }

                item.Content = request.Content;
                item.Title = request.Title;
                item.Pic = request.Pic;
                item.Bank = request.Bank;
                item.Status = 0;
                item.Describe = request.Content.FilterHtml().Cut(300);

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
                    var status = Convert.ToInt32(request.Status);
                    item.Status = status;
                    item.Show = show;
                    item.Sort = sort;
                }


                await _financingRepository.UpdateAsync(item);
            }

            return Result.Success();

        }
    }

}
