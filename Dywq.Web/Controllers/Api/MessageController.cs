using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Infrastructure.Core;
using Dywq.Web.Application.Commands.Message;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : BaseApiController
    {
        public MessageController(IMediator mediator, ILogger<MessageController> logger, IWebHostEnvironment webhostEnvironment) : base(mediator, logger, webhostEnvironment)
        { }

        [Authorize(Roles = Common.Role.Admin)]
        public async Task<Result> Push(AddMessageCommand cmd)
        {
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }
    }
}