using DotNetCore.CAP;
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

namespace Dywq.Web.Application.Commands.Guestbook
{
    public class DeleteGuestbookCommand: IRequest<Result>
    {
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public int Id { get; set; }
    }

    public class DeleteGuestbookCommandHandler : BaseRequestHandler<DeleteGuestbookCommand, Result>
    {

        readonly IBaseRepository<Dywq.Domain.GuestbookAggregate.Guestbook> _guestbookRepository;

        public DeleteGuestbookCommandHandler(
             ICapPublisher capPublisher,
            ILogger<DeleteGuestbookCommandHandler> logger,
             IBaseRepository<Domain.GuestbookAggregate.Guestbook> guestbookRepository
            ) : base(capPublisher, logger)
        {
            _guestbookRepository = guestbookRepository;

        }

        public override async Task<Result> Handle(DeleteGuestbookCommand request, CancellationToken cancellationToken)
        {
            var item = await _guestbookRepository.Set().FirstOrDefaultAsync(x => x.Id == request.Id);
            if (item != null)
            {
                _guestbookRepository.Set().Remove(item);
            }
            else
            {
                return Result.Failure($"id={request.Id} 不存在");
            }
            return Result.Success(); ;
        }
    }
}
