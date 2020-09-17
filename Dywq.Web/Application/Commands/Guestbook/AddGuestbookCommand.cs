using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Domain.GuestbookAggregate;
using System.Threading;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Dywq.Web.Application.Commands.Guestbook
{
    /// <summary>
    /// 添加留言或回复
    /// </summary>
    public class AddGuestbookCommand : IRequest<Result>
    {

        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public string Id { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "用户id错误")]
        /// <summary>
        /// 留言用户，或管理员id
        /// </summary>
        public string UserId { get; set; }


        [Required(ErrorMessage = "内容不能为空")]
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }


        [Range(0, 1, ErrorMessage = "类型错误")]
        /// <summary>
        /// 留言类型：0：用户留言 1：管理员回复
        /// </summary>
        public string Type { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "回复id错误")]
        /// <summary>
        /// 回复的留言id，默认为0
        /// </summary>
        public string ReplyId { get; set; }
    }


    public class AddGuestbookCommandHandler : BaseRequestHandler<AddGuestbookCommand, Result>
    {
        readonly IBaseRepository<Domain.GuestbookAggregate.Guestbook> _guestbookRepository;

        public AddGuestbookCommandHandler(
            ICapPublisher capPublisher,
            ILogger<AddGuestbookCommandHandler> logger,
            IBaseRepository<Domain.GuestbookAggregate.Guestbook> guestbookRepository
            ) : base(capPublisher, logger)
        {
            _guestbookRepository = guestbookRepository;
        }

        public override async Task<Result> Handle(AddGuestbookCommand request, CancellationToken cancellationToken)
        {
            var replyId = string.IsNullOrWhiteSpace(request.ReplyId) ? 0 : Convert.ToInt32(request.ReplyId);
            if (replyId > 0)
            {
                var _guestbook = await _guestbookRepository.Set().FirstOrDefaultAsync(x => x.Id == replyId);

                if (_guestbook == null)
                {
                    return Result.Failure($"回复留言[replyId={replyId}]不存在");
                }

                if (_guestbook.Type != 0) //只能是用户的留言可以回复
                {
                    return Result.Failure($"只能是用户留言可以回复");
                }

                //目前只能回复一次

                if (!await _guestbookRepository.AnyAsync(x => x.ReplyId == replyId))
                {
                    return Result.Failure($"已经回复过");
                }
            }

            var guestBook = new Domain.GuestbookAggregate.Guestbook()
            {
                Content = request.Content,
                ReplyId = replyId,
                Type = Convert.ToInt32(request.Type),
                UserId = Convert.ToInt32(request.UserId)
            };

            await _guestbookRepository.AddAsync(guestBook);

            return Result.Success();

        }
    }
}
