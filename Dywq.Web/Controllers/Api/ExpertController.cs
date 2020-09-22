using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Infrastructure.Core;
using Dywq.Web.Application.Commands.Expert;
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
    public class ExpertController : BaseApiController
    {
        public ExpertController(IMediator mediator, ILogger<ExpertController> logger, IWebHostEnvironment webhostEnvironment) : base(mediator, logger, webhostEnvironment)
        {


        }

        [Authorize(Roles = Common.Role.Admin)]
        [HttpPost]
        public async Task<Result> EditExpertType(EditExpertTypeCommand cmd)
        {
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }

        [Authorize(Roles = Common.Role.Admin)]
        [HttpPost]
        public async Task<Result> ExpertTypeDelete(DeleteExpertTypeCommand cmd)
        {
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }

        [Authorize(Roles = Common.Role.Admin)]
        [HttpPost]
        public async Task<Result> Edit(EditExpertCommand cmd)
        {
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }

        public async Task<Result> Delete(DeleteExpertCommand cmd)
        {
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return result;
        }


    }
}