using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.InvestmentAggregate;
using Dywq.Domain.UserAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
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

namespace Dywq.Web.Application.Commands.Investment
{
    /// <summary>
    /// 编辑或新增
    /// </summary>
    public class EditInvestmentInfoCommand : IRequest<Result>
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
        public string InvestmentTypeId { get; set; }


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
        public string Status { get; set; } = "0";


        /// <summary>
        /// 用户id，可能是企业用户或管理员
        /// </summary>
        public LoginUserDTO LoginUser { get; set; }


    }

    public class EditInvestmentInfoCommandHandler : BaseRequestHandler<EditInvestmentInfoCommand, Result>
    {

        readonly IBaseRepository<InvestmentInfo> _InvestmentInfoRepository;
        readonly IBaseRepository<InvestmentType> _InvestmentTypeRepository;
        //readonly IBaseRepository<Company> _companyRepository;
        readonly IBaseRepository<User> _userRepository;
        readonly IBaseRepository<CompanyUser> _companyUserRepository;

        public EditInvestmentInfoCommandHandler(
            ICapPublisher capPublisher,
            ILogger<EditInvestmentInfoCommandHandler> logger,
            IBaseRepository<InvestmentInfo> InvestmentInfoRepository,
             IBaseRepository<InvestmentType> InvestmentTypeRepository,
             // IBaseRepository<Company> companyRepository,
             IBaseRepository<User> userRepository,
              IBaseRepository<CompanyUser> companyUserRepository
            ) : base(capPublisher, logger)
        {
            _InvestmentInfoRepository = InvestmentInfoRepository;
            _InvestmentTypeRepository = InvestmentTypeRepository;
            //_companyRepository = companyRepository;
            _userRepository = userRepository;
            _companyUserRepository = companyUserRepository;
        }

        public override async Task<Result> Handle(EditInvestmentInfoCommand request, CancellationToken cancellationToken)
        {
            var id = string.IsNullOrWhiteSpace(request.Id) ? 0 : Convert.ToInt32(request.Id);
            var show = request.Show == "1";
            var sort = Convert.ToInt32(request.Sort);

            var typeId = Convert.ToInt32(request.InvestmentTypeId);

            if (!await _InvestmentTypeRepository.AnyAsync(x => x.Id == typeId))
            {
                return Result.Failure($"typeId={typeId}类型不存在");
            }


            if (id <= 0) //新增
            {
                var item = new InvestmentInfo()
                {
                    Content = request.Content,
                    Show = true,
                    Sort = 0,
                    InvestmentTypeId = typeId,
                    Title = request.Title,
                    Status = 0,
                    UserId = request.LoginUser.Id
                };
                if (request.LoginUser.Type == 1)
                {
                    item.Status = 1;
                }
                await _InvestmentInfoRepository.AddAsync(item);
            }
            else
            {
                //修改
                var item = await _InvestmentInfoRepository.Set().FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return Result.Failure($"id={request.Id}错误,内容不存在");
                }

                item.Content = request.Content;
                item.Title = request.Title;
                item.InvestmentTypeId = typeId;
                item.Status = 0;


                if (request.LoginUser.Type == 1) //管理员修改
                {
                    if (string.IsNullOrWhiteSpace(request.Status))
                    {
                        return Result.Failure($"请选择审核状态");
                    }
                    var status = Convert.ToInt32(request.Status);


                    item.Show = show;
                    item.Sort = sort;
                    item.Status = status;
                }

                await _InvestmentInfoRepository.UpdateAsync(item);
            }

            return Result.Success();

        }
    }

}
