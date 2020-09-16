using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
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

namespace Dywq.Web.Application.Commands.Article
{
    public class EditPartyBuildingArticleCommand : IRequest<Result>
    {
        [Range(0, int.MaxValue, ErrorMessage = "用户id错误")]
        public string Id { get; set; }


        [Required(ErrorMessage = "党建主题不能为空")]
        /// <summary>
        /// 党建主题
        /// </summary>
        public string ThemeTitle { get; set; }

        [Required(ErrorMessage = "副标题不能为空")]
        /// <summary>
        /// 副标题
        /// </summary>
        public string Subtitle { get; set; }


        [Required(ErrorMessage = "来源不能为空")]
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }


        [Required(ErrorMessage = "图片不能为空")]
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
    }



    public class EditPartyBuildingArticleCommandHandler : IRequestHandler<EditPartyBuildingArticleCommand, Result>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<EditPartyBuildingArticleCommandHandler> _logger;
        readonly IBaseRepository<PartyBuildingArticle> _partyBuildingArticleRepository;

        public EditPartyBuildingArticleCommandHandler(
            ICapPublisher capPublisher,
            ILogger<EditPartyBuildingArticleCommandHandler> logger,
            IBaseRepository<PartyBuildingArticle> partyBuildingArticleRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;
            _partyBuildingArticleRepository = partyBuildingArticleRepository;
        }

        public async Task<Result> Handle(EditPartyBuildingArticleCommand request, CancellationToken cancellationToken)
        {
            var id = string.IsNullOrWhiteSpace(request.Id) ? 0 : Convert.ToInt32(request.Id);
            var show = request.Show == "1";
            var sort = Convert.ToInt32(request.Sort);
            if (id <= 0) //新增
            {
                var article = new PartyBuildingArticle()
                {
                    Content = request.Content,
                    Pic = request.Pic,
                    Show = show,
                    Sort = sort,
                    Source = request.Source,
                    Subtitle = request.Subtitle,
                    ThemeTitle = request.ThemeTitle
                };
                await _partyBuildingArticleRepository.AddAsync(article);
            }
            else
            {
                //修改
                var article = await _partyBuildingArticleRepository.Set().FirstOrDefaultAsync(x => x.Id == id);
                if (article == null)
                {
                    return Result.Failure($"id={request.Id}错误文章不存在");
                }

                article.Content = request.Content;
                article.Pic = request.Pic;
                article.Show = show;
                article.Sort = sort;
                article.Source = request.Source;
                article.Subtitle = request.Subtitle;
                article.ThemeTitle = request.ThemeTitle;

                await _partyBuildingArticleRepository.UpdateAsync(article);

            }

            return Result.Success();

        }
    }
}
