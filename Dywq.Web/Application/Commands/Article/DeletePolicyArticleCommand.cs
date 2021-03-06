﻿using DotNetCore.CAP;
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
    public class DeleteExpertCommand : IRequest<Result>
    {
        [Required(ErrorMessage = "id不能为空")]
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public int Id { get; set; }
    }


    public class DeletePolicyArticleCommandHandler : BaseRequestHandler<DeleteExpertCommand, Result>
    {

        readonly IBaseRepository<PolicyArticle> _policyArticleRepository;

        public DeletePolicyArticleCommandHandler(
             ICapPublisher capPublisher,
            ILogger<DeletePolicyArticleCommandHandler> logger,
             IBaseRepository<PolicyArticle> policyArticleRepository
            ) : base(capPublisher, logger)
        {
            _policyArticleRepository = policyArticleRepository;

        }

        public override async Task<Result> Handle(DeleteExpertCommand request, CancellationToken cancellationToken)
        {
            var article = await _policyArticleRepository.Set().FirstOrDefaultAsync(x => x.Id == request.Id);
            if (article != null)
            {
                _policyArticleRepository.Set().Remove(article);
            }
            else
            {
                return Result.Failure($"id={request.Id} 不存在");
            }
            return Result.Success(); ;
        }
    }
}
