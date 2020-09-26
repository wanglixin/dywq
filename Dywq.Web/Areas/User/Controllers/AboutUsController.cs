using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands.News;
using Dywq.Web.Application.Commands.Site;
using Dywq.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class AboutUsController : BaseController
    {
        public AboutUsController(IMediator mediator, ILogger<AboutUsController> logger) : base(mediator, logger)
        {

        }
        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id.HasValue)
            { //修改
                var result = await _mediator.Send(new GetAboutUsCommand() { Id = Id.Value }, HttpContext.RequestAborted);
                return View(result?.Data?.FirstOrDefault()); 
            }
            return View();
        }


        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> List(GetAboutUsCommand cmd)
        {
            cmd.LinkUrl = $"/aboutus/news/list?PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }

    }
}