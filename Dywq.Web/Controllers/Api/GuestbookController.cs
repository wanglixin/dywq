using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Infrastructure.Core;
using Dywq.Web.Application.Commands.Guestbook;
using Dywq.Web.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dywq.Web.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GuestbookController : BaseApiController
    {
        public GuestbookController(IMediator mediator, ILogger<ArticleController> logger, IWebHostEnvironment webhostEnvironment) : base(mediator, logger, webhostEnvironment)
        {


        }

        [Authorize(Roles = Role.User + "," + Role.Admin + "," + Role.Editor)]
        [HttpPost]
        public async Task<Result> AddGuestbook([FromBody]AddGuestbookCommand cmd)
        {
           
            cmd.LoginUser = this.GetCurrentUser();
            cmd.UserId = cmd.LoginUser.Id;
            _logger.LogInformation($"接收到请求{HttpContext.Request.Host}{HttpContext.Request.Path},参数 {JsonConvert.SerializeObject(cmd)}");

            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }



        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<Result> Delete([FromBody]DeleteGuestbookCommand cmd)
        {
            _logger.LogInformation($"接收到请求{HttpContext.Request.Host}{HttpContext.Request.Path},参数 {JsonConvert.SerializeObject(cmd)}");

            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }
    }
}