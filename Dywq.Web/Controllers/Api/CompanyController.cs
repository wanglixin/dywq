using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Infrastructure.Core;
using Dywq.Web.Application.Commands;
using Dywq.Web.Application.Commands.CompanyNews;
using Dywq.Web.Application.Commands.Financing;
using Dywq.Web.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dywq.Web.Controllers.Api
{


    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CompanyController : BaseApiController
    {
        public CompanyController(IMediator mediator, ILogger<CompanyController> logger, IWebHostEnvironment webhostEnvironment) : base(mediator, logger, webhostEnvironment)
        {
        }

        [HttpPost]
        [Authorize(Roles = Common.Role.Admin)]
        public async Task<Result> Add([FromBody]AddCompanyCommand cmd)
        {
            _logger.LogInformation($"接收到请求{HttpContext.Request.Host}{HttpContext.Request.Path},参数 {JsonConvert.SerializeObject(cmd)}");
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;

        }

        [HttpPost]
        [Authorize(Roles = Common.Role.Admin)]
        public async Task<Result> Edit([FromBody]EditCompanyCommand cmd)
        {
            _logger.LogInformation($"接收到请求{HttpContext.Request.Host}{HttpContext.Request.Path},参数 {JsonConvert.SerializeObject(cmd)}");
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;

        }

        [HttpPost]
        [Authorize(Roles = Common.Role.Admin)]
        public async Task<Result> Delete([FromBody]DeleteCompanyCommand cmd)
        {
            _logger.LogInformation($"接收到请求{HttpContext.Request.Host}{HttpContext.Request.Path},参数 {JsonConvert.SerializeObject(cmd)}");
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;

        }

        [HttpPost]
        [Authorize(Roles = Common.Role.User)]
        public async Task<Result> AddCompanyInfo(AddCompanyInfoCommand cmd)
        {
            var user = this.GetCurrentUser();
            cmd.CompanyId = user.CompanyId;
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }

        [Authorize(Roles = Common.Role.Admin)]
        public async Task<Result> CheckCompanyInfo(CheckCompanyInfoCommand cmd)
        {
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = Common.Role.Admin)]
        public async Task<Result> EditCompanyInfo(EditCompanyInfoCommand cmd)
        {
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }


        [Authorize(Roles = Common.Role.Admin)]
        public async Task<Result> Search(SearchCompanysCommand cmd)
        {
            cmd.LinkUrl = $"javascript:search(__id__)";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;

        }

        [HttpPost]
        [Authorize(Roles = Common.Role.User + "," + Common.Role.Editor + "," + Common.Role.Admin)]
        public async Task<Result> EditCompanyNewsC(EditCompanyNewsCommand cmd)
        {
            var user = this.GetCurrentUser();
            cmd.CompanyId = user.CompanyId.ToString();
            cmd.UserId = user.Id;

            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }


        [HttpPost]
        [Authorize(Roles = Common.Role.Admin)]
        public async Task<Result> EditCompanyNews(EditCompanyNewsCommand cmd)
        {
            var user = this.GetCurrentUser();
            cmd.UserId = user.Id;

            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }




        [HttpPost]
        [Authorize(Roles = Common.Role.Editor)]
        public async Task<Result> SubmitCheck(SubmitCheckFinancingCommand cmd)
        {
            var user = this.GetCurrentUser();
            cmd.UserId = user.Id;

            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }


    }
}