using DotNetCore.CAP;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Commpany;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Message
{
    public class GetMessagesCommand : IRequest<PageResult<MessageDTO>>
    {
        public int Id { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string LinkUrl { get; set; }

        public int? IsRead { get; set; } = null;

        public int? CompanyId { get; set; }
    }


    public class GetMessagesCommandHandler : BaseRequestHandler<GetMessagesCommand, PageResult<MessageDTO>>
    {

        readonly IBaseRepository<Domain.CompanyAggregate.Message> _messageRepository;

        public GetMessagesCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetMessagesCommandHandler> logger,
            IBaseRepository<Domain.CompanyAggregate.Message> messageRepository
            ) : base(capPublisher, logger)
        {
            _messageRepository = messageRepository;
        }

        public override async Task<PageResult<MessageDTO>> Handle(GetMessagesCommand request, CancellationToken cancellationToken)
        {
            var sb = new List<string>();


            if (request.Id != 0)
            {
                sb.Add($"Id = {request.Id}");
            }

            if (request.IsRead.HasValue)
            {
                sb.Add($"IsRead = " + (request.IsRead.Value));
            }

            if (request.CompanyId.HasValue)
            {
                sb.Add($"CompanyId = " + (request.CompanyId.Value));
            }

            var where = string.Join(" and ", sb);


            var pageData = await _messageRepository.GetPageDataAsync<Domain.CompanyAggregate.Message>(
               pageIndex: request.PageIndex,
               pageSize: request.PageSize,
               where: where,
               order: "Id desc"
               );
            var count = pageData.Total;
            if (count < 1)
            {
                return PageResult<MessageDTO>.Success(null, count, request.PageIndex, request.PageSize, request.LinkUrl);
            }

            var types = await _messageRepository.Set().ToListAsync();

            var data = pageData.Data.Select(x =>
            new MessageDTO()
            {
                Id = x.Id,
                Content = x.Content,
                CreatedTime = x.CreatedTime,
                UpdatedTime = x.UpdatedTime,
                IsRead = x.IsRead,
                AssociationId = x.AssociationId,
                CompanyId = x.CompanyId,
                Type = x.Type
            }
            );

            return PageResult<MessageDTO>.Success(data, count, request.PageIndex, request.PageSize, request.LinkUrl);

        }
    }
}
