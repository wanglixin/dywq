using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Article;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Article
{
    public class GetPartyBuildingArticlesCommand : IRequest<PageResult<PartyBuildingArticleDTO>>
    {
        public int Id { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string LinkUrl { get; set; }


        public bool? Show { get; set; } = null;

        public int? Status { get; set; } = null;
    }


    public class GetPartyBuildingArticlesCommandHandler : BaseRequestHandler<GetPartyBuildingArticlesCommand, PageResult<PartyBuildingArticleDTO>>
    {


        readonly IBaseRepository<PartyBuildingArticle> _partyBuildingArticleRepository;

        public GetPartyBuildingArticlesCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetPartyBuildingArticlesCommandHandler> logger,
            IBaseRepository<PartyBuildingArticle> partyBuildingArticleRepository
            ) : base(capPublisher, logger)
        {

            _partyBuildingArticleRepository = partyBuildingArticleRepository;
        }

        public override async Task<PageResult<PartyBuildingArticleDTO>> Handle(GetPartyBuildingArticlesCommand request, CancellationToken cancellationToken)
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


            var pageData = await _partyBuildingArticleRepository.GetPageDataAsync<PartyBuildingArticle>(
               pageIndex: request.PageIndex,
               pageSize: request.PageSize,
               where: where,
               order: "Sort asc,Id desc"
               );
            var count = pageData.Total;
            if (count < 1)
            {
                return PageResult<PartyBuildingArticleDTO>.Success(null, count, request.PageIndex, request.PageSize, request.LinkUrl);
            }
            var data = pageData.Data.Select(x => new PartyBuildingArticleDTO()
            {
                Id = x.Id,
                Content = x.Content,
                CreatedTime = x.CreatedTime,
                Pic = x.Pic,
                Show = x.Show,
                Sort = x.Sort,
                Source = x.Source,
                Subtitle = x.Subtitle,
                ThemeTitle = x.ThemeTitle,
                UpdatedTime = x.UpdatedTime,
                Status = x.Status
            });
            return PageResult<PartyBuildingArticleDTO>.Success(data, count, request.PageIndex, request.PageSize, request.LinkUrl);

        }
    }
}
