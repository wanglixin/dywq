using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Infrastructure.Core;
using Dywq.Web.Application.Commands;
using Dywq.Web.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dywq.Web.Controllers.Api
{

    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CompanyController : ControllerBase
    {
        IMediator _mediator;
        readonly ILogger<CompanyController> _logger;

        public CompanyController(IMediator mediator, ILogger<CompanyController> logger)
        {
            _mediator = mediator;
            _logger = logger;

        }

        [Authorize(Roles = Common.Role.Admin)]
        public async Task<Result> AddCompany([FromBody]AddCompanyFieldDataCommand cmd)
        {
            _logger.LogInformation($"接收到请求{HttpContext.Request.Host}{HttpContext.Request.Path},参数 {JsonConvert.SerializeObject(cmd)}");
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;

        }


        [Authorize(Roles =Common.Role.Admin)]
        public async Task<Result> Test()
        {

            return Result.Success("测试"); ;

        }


    }
}