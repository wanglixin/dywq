using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
using Dywq.Domain.News;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Article;
using Dywq.Web.Dto.News;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.News
{
    public class GetNoticeNewsCommand : IRequest<PageResult<NoticeNewsDTO>>
    {
        public int Id { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string LinkUrl { get; set; }

        public bool? Show { get; set; } = null;


        public int? Status { get; set; } = null;

    }


    public class GetNoticeNewsCommandHandler : BaseRequestHandler<GetNoticeNewsCommand, PageResult<NoticeNewsDTO>>
    {

        readonly IBaseRepository<NoticeNews> _noticeNewsRepository;

        public GetNoticeNewsCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetNoticeNewsCommandHandler> logger,
            IBaseRepository<NoticeNews> noticeNewsRepository
            ) : base(capPublisher, logger)
        {
            _noticeNewsRepository = noticeNewsRepository;
        }

        public override async Task<PageResult<NoticeNewsDTO>> Handle(GetNoticeNewsCommand request, CancellationToken cancellationToken)
        {
            var sb = new List<string>();


            if (request.Id != 0)
            {
                sb.Add($"Id = {request.Id}");
            }

            if (request.Show.HasValue)
            {
                sb.Add($"Show = " + (request.Show.Value ? 1 : 0));
            }

            if (request.Status.HasValue)
            {
                sb.Add($"Status = " + request.Status.Value);
            }



            var where = string.Join(" and ", sb);


            var pageData = await _noticeNewsRepository.GetPageDataAsync<NoticeNews>(
               pageIndex: request.PageIndex,
               pageSize: request.PageSize,
               where: where,
               order: "Sort asc,Id desc"
               );
            var count = pageData.Total;
            if (count < 1)
            {
                return PageResult<NoticeNewsDTO>.Success(null, count, request.PageIndex, request.PageSize, request.LinkUrl);
            }

            var types = await _noticeNewsRepository.Set().ToListAsync();

            var data = pageData.Data.Select(x =>
            new NoticeNewsDTO()
            {
                Id = x.Id,
                Content = x.Content,
                CreatedTime = x.CreatedTime,
                Show = x.Show,
                Sort = x.Sort,
                Pic = x.Pic,
                Title = x.Title,
                UpdatedTime = x.UpdatedTime,
                Source = x.Source,
                Status = x.Status
            }
            );

            return PageResult<NoticeNewsDTO>.Success(data, count, request.PageIndex, request.PageSize, request.LinkUrl);

        }
    }
}
