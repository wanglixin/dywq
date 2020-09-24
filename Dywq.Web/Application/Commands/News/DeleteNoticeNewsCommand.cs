using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
using Dywq.Domain.News;
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

namespace Dywq.Web.Application.Commands.News
{
    public class DeleteNoticeNewsCommand : IRequest<Result>
    {
        [Required(ErrorMessage = "id不能为空")]
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public int Id { get; set; }
    }


    public class DeleteNoticeNewsCommandHandler : BaseRequestHandler<DeleteNoticeNewsCommand, Result>
    {

        readonly IBaseRepository<NoticeNews> _noticeNewsRepository;

        public DeleteNoticeNewsCommandHandler(
             ICapPublisher capPublisher,
            ILogger<DeleteNoticeNewsCommandHandler> logger,
             IBaseRepository<NoticeNews> noticeNewsRepository
            ) : base(capPublisher, logger)
        {
            _noticeNewsRepository = noticeNewsRepository;

        }

        public override async Task<Result> Handle(DeleteNoticeNewsCommand request, CancellationToken cancellationToken)
        {
            var article = await _noticeNewsRepository.Set().FirstOrDefaultAsync(x => x.Id == request.Id);
            if (article != null)
            {
                _noticeNewsRepository.Set().Remove(article);
            }
            else
            {
                return Result.Failure($"id={request.Id} 不存在");
            }
            return Result.Success(); ;
        }
    }
}
