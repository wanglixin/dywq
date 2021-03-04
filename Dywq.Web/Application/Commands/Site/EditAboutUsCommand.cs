using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
using Dywq.Domain.News;
using Dywq.Domain.SiteAggregate;
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

namespace Dywq.Web.Application.Commands.Site
{
    public class EditAboutUsCommand : IRequest<Result>
    {
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public string Id { get; set; }


        [Required(ErrorMessage = "标题不能为空")]
        /// <summary>
        /// 主题
        /// </summary>
        public string Title { get; set; }



       // [Required(ErrorMessage = "请上传图片")]
        /// <summary>
        /// 图片
        /// </summary>
        public string Pic { get; set; }


        [Required(ErrorMessage = "内容不能为空")]
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }


        [RegularExpression("^[0-9]+$", ErrorMessage = "排序值错误")]
        public string Sort { get; set; } = "0";

        [RegularExpression("^[0|1]+$", ErrorMessage = "显示值错误")]
        public string Show { get; set; } = "1";

        /// <summary>
        ///  0:默认 待审核 1：审核成功 -1：审核失败
        /// </summary>
        [Range(-1, 1, ErrorMessage = "审核状态错误")]
        public string Status { get; set; } = "0";

        public LoginUserDTO LoginUser { get; set; }

    }



    public class EditAboutUsCommandHandler : BaseRequestHandler<EditAboutUsCommand, Result>
    {
        readonly IBaseRepository<AboutUs> _aboutUsRepository;

        public EditAboutUsCommandHandler(
            ICapPublisher capPublisher,
            ILogger<EditAboutUsCommandHandler> logger,
            IBaseRepository<AboutUs> aboutUsRepository
            ) : base(capPublisher, logger)
        {

            _aboutUsRepository = aboutUsRepository;
        }

        public override async Task<Result> Handle(EditAboutUsCommand request, CancellationToken cancellationToken)
        {
            var id = string.IsNullOrWhiteSpace(request.Id) ? 0 : Convert.ToInt32(request.Id);
            var show = request.Show == "1";
            var sort = Convert.ToInt32(request.Sort);


            if (id <= 0) //新增
            {
                var item = new AboutUs()
                {
                    Content = request.Content,
                    Show = show,
                    Sort = sort,
                    Title = request.Title,
                    Pic = request.Pic,
                    Status = 0
                };
                if (request.LoginUser.Type == 1)
                {
                    item.Status = 1;
                }
                await _aboutUsRepository.AddAsync(item);
            }
            else
            {
                //修改
                var item = await _aboutUsRepository.Set().FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return Result.Failure($"id={request.Id}内容不存在");
                }


                if (!request.LoginUser.IsAdmin && item.Status == 1)
                {
                    return Result.Failure($"当前状态不能修改！");
                }


                item.Content = request.Content;
                item.Show = show;
                item.Sort = sort;
                item.Title = request.Title;
                item.Pic = request.Pic;



                if (request.LoginUser.Type == 1)
                {
                    item.Status = Convert.ToInt32(request.Status);
                }
                else
                {
                    item.Status = 0;
                }

                await _aboutUsRepository.UpdateAsync(item);

            }

            return Result.Success();

        }
    }
}
