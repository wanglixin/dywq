using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Infrastructure.Core;
using Dywq.Web.Application.Commands.Cooperation;
using Dywq.Web.Application.Commands.Investment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dywq.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class InvestmentController : BaseApiController
    {
        public InvestmentController(IMediator mediator, ILogger<InvestmentController> logger, IWebHostEnvironment webhostEnvironment) : base(mediator, logger, webhostEnvironment)
        { }

        [Authorize(Roles = Common.Role.Admin + "," + Common.Role.Editor)]
        [HttpPost]
        public async Task<Result> Edit([FromBody]EditInvestmentInfoCommand cmd)
        {
            var user = this.GetCurrentUser();
            cmd.LoginUser = user;
            _logger.LogInformation($"接收到请求{HttpContext.Request.Host}{HttpContext.Request.Path},参数 {JsonConvert.SerializeObject(cmd)}");

            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }


        [Authorize(Roles = Common.Role.Admin)]
        public async Task<Result> Delete(DeleteInvestmentInfoCommand cmd)
        {
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }


        [HttpPost]
        public async Task<Result> List([FromBody]GetInvestmentInfosCommand cmd)
        {
            _logger.LogInformation($"接收到请求{HttpContext.Request.Host}{HttpContext.Request.Path},参数 {JsonConvert.SerializeObject(cmd)}");
            cmd.Show = true;
            cmd.Status = 1;
            cmd.PageSize = 5;
            cmd.PageIndex = 1;
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }

    }
}