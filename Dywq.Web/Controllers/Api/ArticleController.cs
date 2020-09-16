using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Infrastructure.Core;
using Dywq.Web.Application.Commands.Article;
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
    public class ArticleController : BaseApiController
    {

        public ArticleController(IMediator mediator, ILogger<ArticleController> logger, IWebHostEnvironment webhostEnvironment) : base(mediator, logger, webhostEnvironment)
        {


        }
        [Authorize(Roles = Common.Role.Admin)]
        [HttpPost]
        public async Task<Result> EditPartyBuildingArticle([FromBody]EditPartyBuildingArticleCommand cmd)
        {
            _logger.LogInformation($"接收到请求{HttpContext.Request.Host}{HttpContext.Request.Path},参数 {JsonConvert.SerializeObject(cmd)}");

            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }


        [Authorize(Roles = Common.Role.Admin)]
        public async Task<Result> DeletePartyBuildingArticle(DeletePartyBuildingArticleCommand cmd)
        {
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }
    }
}