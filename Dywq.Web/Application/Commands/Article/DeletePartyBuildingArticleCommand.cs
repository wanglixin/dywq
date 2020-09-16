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
    public class DeletePartyBuildingArticleCommand : IRequest<Result>
    {
        [Range(0, int.MaxValue, ErrorMessage = "用户id错误")]
        public int Id { get; set; }
    }


    public class DeletePartyBuildingArticleCommandHandler : IRequestHandler<DeletePartyBuildingArticleCommand, Result>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<DeletePartyBuildingArticleCommandHandler> _logger;

        readonly IBaseRepository<PartyBuildingArticle> _partyBuildingArticleRepository;

        public DeletePartyBuildingArticleCommandHandler(
             ICapPublisher capPublisher,
            ILogger<DeletePartyBuildingArticleCommandHandler> logger,
             IBaseRepository<PartyBuildingArticle> partyBuildingArticleRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;
            _partyBuildingArticleRepository = partyBuildingArticleRepository;

        }

        public async Task<Result> Handle(DeletePartyBuildingArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _partyBuildingArticleRepository.Set().FirstOrDefaultAsync(x => x.Id == request.Id);
            if (article != null)
            {
                _partyBuildingArticleRepository.Set().Remove(article);
            }
            else
            {
                return Result.Failure($"id={request.Id} 不存在");
            }
            return Result.Success(); ;
        }
    }
}
