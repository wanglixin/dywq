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

namespace Dywq.Web.Application.Commands.Article
{
    public class EditPolicyArticleCommand : IRequest<Result>
    {
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public string Id { get; set; }


        [Required(ErrorMessage = "主题不能为空")]
        /// <summary>
        /// 主题
        /// </summary>
        public string ThemeTitle { get; set; }

        [Required(ErrorMessage = "内容不能为空")]
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }


        [Required(ErrorMessage = "请选择类型")]
        /// <summary>
        /// 政策类型id
        /// </summary>
        public string PolicyTypeId { get; set; }


        [RegularExpression("^[0-9]+$", ErrorMessage = "排序值错误")]
        public string Sort { get; set; } = "0";

        [RegularExpression("^[0|1]+$", ErrorMessage = "显示值错误")]
        public string Show { get; set; } = "1";


        [Required(ErrorMessage = "来源不能为空")]
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }


        /// <summary>
        ///  0:默认 待审核 1：审核成功 -1：审核失败
        /// </summary>
        [Range(-1, 1, ErrorMessage = "审核状态错误")]
        public string Status { get; set; } = "0";

        public LoginUserDTO LoginUser { get; set; }
    }



    public class EditPolicyArticleCommandHandler : IRequestHandler<EditPolicyArticleCommand, Result>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<EditPolicyArticleCommandHandler> _logger;
        readonly IBaseRepository<PolicyArticle> _policyArticleRepository;
        readonly IBaseRepository<PolicyType> _policyTypeRepository;

        public EditPolicyArticleCommandHandler(
            ICapPublisher capPublisher,
            ILogger<EditPolicyArticleCommandHandler> logger,
            IBaseRepository<PolicyArticle> policyArticleRepository,
             IBaseRepository<PolicyType> policyTypeRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;
            _policyArticleRepository = policyArticleRepository;
            _policyTypeRepository = policyTypeRepository;
        }

        public async Task<Result> Handle(EditPolicyArticleCommand request, CancellationToken cancellationToken)
        {
            var id = string.IsNullOrWhiteSpace(request.Id) ? 0 : Convert.ToInt32(request.Id);
            var show = request.Show == "1";
            var sort = Convert.ToInt32(request.Sort);

            var typeId = Convert.ToInt32(request.PolicyTypeId);

            if (!await _policyTypeRepository.AnyAsync(x => x.Id == typeId))
            {
                return Result.Failure($"typeId={typeId}类型不存在");
            }


            if (id <= 0) //新增
            {
                var article = new PolicyArticle()
                {
                    Content = request.Content,
                    Show = show,
                    Sort = sort,
                    ThemeTitle = request.ThemeTitle,
                    PolicyTypeId = typeId,
                    Source = request.Source,
                    Status = 0
                };
                if (request.LoginUser.Type == 1)
                {
                    article.Status = 1;
                }
                await _policyArticleRepository.AddAsync(article);
            }
            else
            {
                //修改
                var article = await _policyArticleRepository.Set().FirstOrDefaultAsync(x => x.Id == id);
                if (article == null)
                {
                    return Result.Failure($"id={request.Id}错误文章不存在");
                }

                article.Content = request.Content;
                article.Show = show;
                article.Sort = sort;
                article.ThemeTitle = request.ThemeTitle;
                article.PolicyTypeId = typeId;
                article.Source = request.Source;


                if (request.LoginUser.Type == 1)
                {
                    article.Status = Convert.ToInt32(request.Status);
                }
                else
                {
                    article.Status = 0;
                }

                await _policyArticleRepository.UpdateAsync(article);

            }

            return Result.Success();

        }
    }
}
