using DotNetCore.CAP;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Message
{
    public class EditMessageCommand : IRequest<Result>
    {
        [Range(1, int.MaxValue, ErrorMessage = "企业id错误")]
        public int CompanyId { get; set; }

        public int Type { get; set; }

        public int AssociationId { get; set; }

    }


    public class EditMessageCommandHandler : BaseRequestHandler<EditMessageCommand, Result>
    {
        readonly IBaseRepository<Dywq.Domain.CompanyAggregate.Message> _messageRepository;

        public EditMessageCommandHandler(
            ICapPublisher capPublisher,
            ILogger<EditMessageCommandHandler> logger,
            IBaseRepository<Domain.CompanyAggregate.Message> messageRepository
            ) : base(capPublisher, logger)
        {
            _messageRepository = messageRepository;
        }

        public override async Task<Result> Handle(EditMessageCommand request, CancellationToken cancellationToken)
        {
            var message = _messageRepository.Set().FirstOrDefault(x => x.CompanyId == request.CompanyId && x.Type == request.Type && x.AssociationId == request.AssociationId && x.IsRead == 0);
            if (message != null)
            {
                message.IsRead = 1;
                await _messageRepository.UpdateAsync(message);
            }
            return Result.Success();

        }
    }
}
