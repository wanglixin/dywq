using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands.News;
using Dywq.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class NewsController : BaseController
    {
        public NewsController(IMediator mediator, ILogger<NewsController> logger) : base(mediator, logger)
        {

        }
        [Authorize(Roles = Common.Role.Admin + "," + Common.Role.Editor)]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id.HasValue)
            { //修改
                var result = await _mediator.Send(new GetNoticeNewsCommand() { Id = Id.Value }, HttpContext.RequestAborted);
                return View(result?.Data?.FirstOrDefault());
            }
            return View();
        }


        [Authorize(Roles = Common.Role.Admin + "," + Common.Role.Editor)]
        public async Task<IActionResult> List(GetNoticeNewsCommand cmd)
        {
            cmd.LinkUrl = $"/user/news/list?PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }

    }
}