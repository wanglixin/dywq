using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Article;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Article
{
    public class GetPolicyArticlesCommand : IRequest<PageResult<PolicyArticleDTO>>
    {
        public int Id { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string LinkUrl { get; set; }

        /// <summary>
        /// 政策类型id
        /// </summary>
        public string PolicyTypeId { get; set; } = "";
        public bool? Show { get; set; } = null;
    }


    public class GetPolicyArticlesCommandHandler : BaseRequestHandler<GetPolicyArticlesCommand, PageResult<PolicyArticleDTO>>
    {

        readonly IBaseRepository<PolicyArticle> _policyArticleRepository;
        readonly IBaseRepository<PolicyType> _policyTypeRepository;

        public GetPolicyArticlesCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetPolicyArticlesCommandHandler> logger,
            IBaseRepository<PolicyArticle> policyArticleRepository,
             IBaseRepository<PolicyType> policyTypeRepository
            ):base(capPublisher, logger)
        {
            _policyArticleRepository = policyArticleRepository;
            _policyTypeRepository = policyTypeRepository;
        }

        public override async Task<PageResult<PolicyArticleDTO>> Handle(GetPolicyArticlesCommand request, CancellationToken cancellationToken)
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

            if (!string.IsNullOrWhiteSpace(request.PolicyTypeId))
            {
                sb.Add($"PolicyTypeId = " + request.PolicyTypeId);
            }
            var where = string.Join(" and ", sb);


            var pageData = await _policyArticleRepository.GetPageDataAsync<PolicyArticle>(
               pageIndex: request.PageIndex,
               pageSize: request.PageSize,
               where: where,
               order: "Sort asc,Id desc"
               );
            var count = pageData.Total;
            if (count < 1)
            {
                return PageResult<PolicyArticleDTO>.Success(null, count, request.PageIndex, request.PageSize, request.LinkUrl);
            }

            var types = await _policyTypeRepository.Set().ToListAsync();

            var data = pageData.Data.Select(x =>

                new PolicyArticleDTO()
                {
                    Id = x.Id,
                    Content = x.Content,
                    CreatedTime = x.CreatedTime,
                    Show = x.Show,
                    Sort = x.Sort,
                    ThemeTitle = x.ThemeTitle,
                    UpdatedTime = x.UpdatedTime,
                    PolicyTypeId = x.PolicyTypeId,
                    PolicyTypeName = types.FirstOrDefault(t => t.Id == x.PolicyTypeId)?.Name
                }
            );

            return PageResult<PolicyArticleDTO>.Success(data, count, request.PageIndex, request.PageSize, request.LinkUrl);

        }
    }
}
