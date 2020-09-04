using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Infrastructure.Core;
using Dywq.Web.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        IMediator _mediator;
        readonly ILogger<UserController> _logger;
        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// 请求登陆
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> Login([FromBody]LoginCommand cmd)
        {
            _logger.LogInformation($"接收到请求/api/user/login,参数 UserName={cmd.UserName} Password={cmd.Password}");
            return await _mediator.Send(cmd, HttpContext.RequestAborted);
        }
    }
}