using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
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

namespace Dywq.Web.Application.Commands.Expert
{
    public class EditExpertCommand : IRequest<Result>
    {
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public string Id { get; set; }


        [Required(ErrorMessage = "名字不能为空")]
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        [Required(ErrorMessage = "请上传头像")]
        /// <summary>
        /// 头像图片
        /// </summary>
        public string Pic { get; set; }


        [Required(ErrorMessage = "职称不能为空")]
        /// <summary>
        /// 职称
        /// </summary>
        public string Title { get; set; }


        [Required(ErrorMessage = "简介不能为空")]
        /// <summary>
        /// 简介
        /// </summary>
        public string Introduction { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "排序值错误")]
        public string Sort { get; set; } = "0";

        public bool Show { get; set; } = true;


        [Required(ErrorMessage = "请选择类型")]
        /// <summary>
        /// 专家类型
        /// </summary>
        public string ExpertTypeId { get; set; }

        /// <summary>
        ///  0:默认 待审核 1：审核成功 -1：审核失败
        /// </summary>
        [Range(-1, 1, ErrorMessage = "审核状态错误")]
        public string Status { get; set; } = "0";

        public LoginUserDTO LoginUser { get; set; }
    }



    public class EditExpertCommandHandler : BaseRequestHandler<EditExpertCommand, Result>
    {
        IBaseRepository<Dywq.Domain.Expert.Expert> _expertRepository;
        IBaseRepository<Dywq.Domain.Expert.ExpertType> _expertTypeRepository;

        public EditExpertCommandHandler(
            ICapPublisher capPublisher,
            ILogger<EditExpertCommandHandler> logger,
            IBaseRepository<Dywq.Domain.Expert.Expert> expertRepository,
             IBaseRepository<Dywq.Domain.Expert.ExpertType> expertTypeRepository
            ) : base(capPublisher, logger)
        {
            _expertRepository = expertRepository;
            _expertTypeRepository = expertTypeRepository;
        }

        public override async Task<Result> Handle(EditExpertCommand request, CancellationToken cancellationToken)
        {
            var id = string.IsNullOrWhiteSpace(request.Id) ? 0 : Convert.ToInt32(request.Id);

            var sort = Convert.ToInt32(request.Sort);

            var expertTypeId = Convert.ToInt32(request.ExpertTypeId);

            if (!await _expertTypeRepository.AnyAsync(x => x.Id == expertTypeId))
            {
                return Result.Failure($"typeId={expertTypeId}类型不存在");
            }


            if (id <= 0) //新增
            {
                var item = new Domain.Expert.Expert()
                {
                    Show = request.Show,
                    Introduction = request.Introduction,
                    Title = request.Title,
                    Pic = request.Pic,
                    Name = request.Name,
                    Sort = sort,
                    ExpertTypeId = expertTypeId,
                    Status = 0
                };
                if (request.LoginUser.Type == 1)
                {
                    item.Status = 1;
                }
                await _expertRepository.AddAsync(item);
            }
            else
            {
                //修改
                var item = await _expertRepository.Set().FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return Result.Failure($"id={request.Id}内容不存在");
                }

                item.ExpertTypeId = expertTypeId;
                item.Introduction = request.Introduction;
                item.Name = request.Name;
                item.Pic = request.Pic;
                item.Show = request.Show;
                item.Title = request.Title;
                item.Sort = sort;

                if (request.LoginUser.Type == 1)
                {
                    item.Status = Convert.ToInt32(request.Status);
                }
                else
                {
                    item.Status = 0;
                }

                await _expertRepository.UpdateAsync(item);
            }

            return Result.Success();

        }
    }
}
