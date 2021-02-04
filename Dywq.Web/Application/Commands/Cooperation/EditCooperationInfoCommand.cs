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

namespace Dywq.Web.Application.Commands.Cooperation
{
    /// <summary>
    /// 编辑或新增
    /// </summary>
    public class EditCooperationInfoCommand : IRequest<Result>
    {
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public string Id { get; set; }

        //[Required(ErrorMessage = "请选择企业")]
        //[Range(1, int.MaxValue, ErrorMessage = "企业id错误")]
        //public string CompanyId { get; set; }


        [Required(ErrorMessage = "请选择类型")]
        /// <summary>
        /// 类型id
        /// </summary>
        public string CooperationTypeId { get; set; }


        [Required(ErrorMessage = "标题不能为空")]
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }


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

    public class EditCooperationInfoCommandHandler : BaseRequestHandler<EditCooperationInfoCommand, Result>
    {

        readonly IBaseRepository<CooperationInfo> _cooperationInfoRepository;
        readonly IBaseRepository<CooperationType> _cooperationTypeRepository;
        //readonly IBaseRepository<Company> _companyRepository;
        readonly IBaseRepository<User> _userRepository;
        readonly IBaseRepository<CompanyUser> _companyUserRepository;

        public EditCooperationInfoCommandHandler(
            ICapPublisher capPublisher,
            ILogger<EditCooperationInfoCommandHandler> logger,
            IBaseRepository<CooperationInfo> cooperationInfoRepository,
             IBaseRepository<CooperationType> cooperationTypeRepository,
             // IBaseRepository<Company> companyRepository,
             IBaseRepository<User> userRepository,
              IBaseRepository<CompanyUser> companyUserRepository
            ) : base(capPublisher, logger)
        {
            _cooperationInfoRepository = cooperationInfoRepository;
            _cooperationTypeRepository = cooperationTypeRepository;
            //_companyRepository = companyRepository;
            _userRepository = userRepository;
            _companyUserRepository = companyUserRepository;
        }

        public override async Task<Result> Handle(EditCooperationInfoCommand request, CancellationToken cancellationToken)
        {
            var id = string.IsNullOrWhiteSpace(request.Id) ? 0 : Convert.ToInt32(request.Id);
            var show = request.Show == "1";
            var sort = Convert.ToInt32(request.Sort);

            var typeId = Convert.ToInt32(request.CooperationTypeId);

            if (!await _cooperationTypeRepository.AnyAsync(x => x.Id == typeId))
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
                //if (user.Type != 0)
                //{
                //    return Result.Failure($"只有企业才可以添加");
                //}
                var item = new CooperationInfo()
                {
                    Content = request.Content,
                    Show = true,
                    Sort = 0,
                    CooperationTypeId = typeId,
                    Title = request.Title,
                    Status = 0
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

                if (user.Type == 1)
                {
                    item.Status = 1;
                }
                await _cooperationInfoRepository.AddAsync(item);
            }
            else
            {
                //修改
                var item = await _cooperationInfoRepository.Set().FirstOrDefaultAsync(x => x.Id == id);
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
                    item.Content = request.Content;
                    item.Title = request.Title;
                    item.CooperationTypeId = typeId;
                    item.Status = 0;
                }
                //else if (user.Type == 2) //编辑
                //{
                //    if (item.Status != -1 && item.Status != 0)
                //    {
                //        return Result.Failure($"当前状态不能修改");
                //    }
                //    item.Content = request.Content;
                //    item.Title = request.Title;
                //    item.CooperationTypeId = typeId;
                //    item.Status = 0;
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
                    item.Title = request.Title;
                    item.CooperationTypeId = typeId;
                    item.Status = status;
                }
                else
                {
                    return Result.Failure($"用户类型错误");
                }

                await _cooperationInfoRepository.UpdateAsync(item);
            }

            return Result.Success();

        }
    }

}
