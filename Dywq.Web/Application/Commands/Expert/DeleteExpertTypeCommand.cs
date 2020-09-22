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
    public class DeleteExpertTypeCommand : IRequest<Result>
    {
        [Required(ErrorMessage = "id不能为空")]
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public int Id { get; set; }
    }


    public class DeleteExpertTypeCommandHandler : BaseRequestHandler<DeleteExpertTypeCommand, Result>
    {

        readonly IBaseRepository<Dywq.Domain.Expert.ExpertType> _expertTypeRepository;

        public DeleteExpertTypeCommandHandler(
             ICapPublisher capPublisher,
            ILogger<DeleteExpertTypeCommandHandler> logger,
             IBaseRepository<Domain.Expert.ExpertType> expertTypeRepository
            ) : base(capPublisher, logger)
        {
            _expertTypeRepository = expertTypeRepository;

        }

        public override async Task<Result> Handle(DeleteExpertTypeCommand request, CancellationToken cancellationToken)
        {
            var article = await _expertTypeRepository.Set().FirstOrDefaultAsync(x => x.Id == request.Id);
            if (article != null)
            {
                _expertTypeRepository.Set().Remove(article);
            }
            else
            {
                return Result.Failure($"id={request.Id} 不存在");
            }
            return Result.Success();
        }
    }
}
