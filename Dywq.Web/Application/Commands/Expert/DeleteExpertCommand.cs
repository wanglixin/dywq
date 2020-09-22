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

namespace Dywq.Web.Application.Commands.Expert
{
    public class DeleteExpertCommand : IRequest<Result>
    {
        [Required(ErrorMessage = "id不能为空")]
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public int Id { get; set; }
    }


    public class DeleteExpertCommandHandler : BaseRequestHandler<DeleteExpertCommand, Result>
    {

        readonly IBaseRepository<Dywq.Domain.Expert.Expert> _expertRepository;

        public DeleteExpertCommandHandler(
             ICapPublisher capPublisher,
            ILogger<DeleteExpertCommandHandler> logger,
             IBaseRepository<Domain.Expert.Expert> expertRepository
            ) : base(capPublisher, logger)
        {
            _expertRepository = expertRepository;

        }

        public override async Task<Result> Handle(DeleteExpertCommand request, CancellationToken cancellationToken)
        {
            var article = await _expertRepository.Set().FirstOrDefaultAsync(x => x.Id == request.Id);
            if (article != null)
            {
                _expertRepository.Set().Remove(article);
            }
            else
            {
                return Result.Failure($"id={request.Id} 不存在");
            }
            return Result.Success();
        }
    }
}
