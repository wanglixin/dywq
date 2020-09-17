using DotNetCore.CAP;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands
{
    public abstract class BaseRequestHandler<T, R> : IRequestHandler<T, R> where T : IRequest<R>
    {
        private readonly ICapPublisher _capPublisher;
        private readonly ILogger<BaseRequestHandler<T, R>> _logger;
        public BaseRequestHandler(ICapPublisher capPublisher, ILogger<BaseRequestHandler<T, R>> logger)
        {
            _capPublisher = capPublisher;
            _logger = logger;
        }

        public abstract Task<R> Handle(T request, CancellationToken cancellationToken);
    }
}
