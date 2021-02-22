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

namespace Dywq.Web.Application.Commands.Purchase
{
    /// <summary>
    /// 编辑或新增
    /// </summary>
    public class EditPurchaseCommand : IRequest<Result>
    {
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public string Id { get; set; }




        [Required(ErrorMessage = "产品名称不能为空")]
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        [Required(ErrorMessage = "联系人不能为空")]
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }


        [Required(ErrorMessage = "手机号不能为空")]
        [RegularExpression(@"^1\d{10}$", ErrorMessage = "手机号码格式不正确")]
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }



        [Required(ErrorMessage = "详细内容不能为空")]
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
        /// 0:提交信息审核，待审核 1：审核通过 -1：审核失败
        /// </summary>
        public string Status { get; set; } = "";


        /// <summary>
        /// 用户id，可能是企业用户或管理员
        /// </summary>
        public int UserId { get; set; }

        [Required(ErrorMessage = "请选择类型")]
        [RegularExpression("^[0|1]+$", ErrorMessage = "请选择类型")]
        /// <summary>
        /// 0:提交信息审核，待审核 1：审核通过 -1：审核失败
        /// </summary>
        public string Type { get; set; } = "";


    }

    public class EditPurchaseCommandHandler : BaseRequestHandler<EditPurchaseCommand, Result>
    {

        readonly IBaseRepository<Domain.Purchase.Purchase> _purchaseRepository;
        readonly IBaseRepository<User> _userRepository;
        readonly IBaseRepository<CompanyUser> _companyUserRepository;

        public EditPurchaseCommandHandler(
            ICapPublisher capPublisher,
            ILogger<EditPurchaseCommandHandler> logger,
            IBaseRepository<Domain.Purchase.Purchase> purchaseRepository,
             IBaseRepository<User> userRepository,
              IBaseRepository<CompanyUser> companyUserRepository
            ) : base(capPublisher, logger)
        {
            _purchaseRepository = purchaseRepository;
            _userRepository = userRepository;
            _companyUserRepository = companyUserRepository;
        }

        public override async Task<Result> Handle(EditPurchaseCommand request, CancellationToken cancellationToken)
        {
            var id = string.IsNullOrWhiteSpace(request.Id) ? 0 : Convert.ToInt32(request.Id);
            var show = request.Show == "1";
            var sort = Convert.ToInt32(request.Sort);
            var type = Convert.ToInt32(request.Type);

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

                var item = new Domain.Purchase.Purchase()
                {

                    Contacts = request.Contacts,
                    Content = request.Content,
                    Mobile = request.Mobile,
                    ProductName = request.ProductName,
                    Show = true,
                    Sort = 0,
                    Status = 0,
                    Type = type,
                    UserId = request.UserId
                };

                if (user.Type == 0)//|| user.Type == 2)
                {
                    var company_user = await _companyUserRepository.Set().FirstOrDefaultAsync(x => x.UserId == request.UserId);
                    if (company_user == null)
                    {
                        return Result.Failure($"您还未绑定企业");
                    }

                    item.CompanyId = company_user.CompanyId;
                }
                if (user.Type == 1)
                {
                    item.Status = 1;
                }



                await _purchaseRepository.AddAsync(item);
            }
            else
            {
                //修改
                var item = await _purchaseRepository.Set().FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return Result.Failure($"id={request.Id}错误,内容不存在");
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

                if (user.Type == 0 || user.Type == 2) //用户修改的情况
                {
                    if (item.Status != -1)
                    {
                        return Result.Failure($"当前状态不能修改");
                    }

                    // item.CompanyId = company_user.CompanyId;
                    item.Contacts = request.Contacts;
                    item.Content = request.Content;
                    item.Mobile = request.Mobile;
                    item.ProductName = request.ProductName;
                    item.Show = false;
                    item.Status = 0;
                    item.Type = type;
                }
                //else if (user.Type == 2) //编辑
                //{
                //    if (item.Status != -1 && item.Status != 0)
                //    {
                //        return Result.Failure($"当前状态不能修改");
                //    }
                //    item.Contacts = request.Contacts;
                //    item.Content = request.Content;
                //    item.Mobile = request.Mobile;
                //    item.ProductName = request.ProductName;
                //    item.Show = false;
                //    item.Status = 0;
                //    item.Type = type;
                //}
                else if (user.Type == 1) //管理员修改
                {

                    if (string.IsNullOrWhiteSpace(request.Status))
                    {
                        return Result.Failure($"请选择审核状态");
                    }
                    var status = Convert.ToInt32(request.Status);

                    item.Content = request.Content;
                    item.Show = show;
                    item.Sort = sort;
                    item.Contacts = request.Contacts;
                    item.Mobile = request.Mobile;
                    item.ProductName = request.ProductName;
                    item.Status = status;
                    item.Type = type;
                }
                else
                {
                    return Result.Failure($"用户类型错误");
                }

                await _purchaseRepository.UpdateAsync(item);
            }

            return Result.Success();

        }
    }

}
