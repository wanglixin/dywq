using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands.News;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Controllers
{
    public class NewsController : BaseController
    {
        public NewsController(IMediator mediator, ILogger<NewsController> logger) : base(mediator, logger)
        {


        }

        public async Task<IActionResult> List(GetNoticeNewsCommand cmd)
        {
            cmd.Show = true;
            cmd.LinkUrl = $"/News/List?PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }

        public async Task<IActionResult> Detail(int Id)
        {
            var pageData = await _mediator.Send(new GetNoticeNewsCommand()
            {
                Id = Id,
                PageIndex = 1,
                PageSize = 1,
                Show = true
            }, HttpContext.RequestAborted);
            return View(pageData?.Data?.FirstOrDefault());
        }
    }
}