using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands.News;
using Dywq.Web.Application.Commands.Site;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Controllers
{
    public class AboutUsController : BaseController
    {
        public AboutUsController(IMediator mediator, ILogger<AboutUsController> logger) : base(mediator, logger)
        {


        }





        public async Task<IActionResult> Index()
        {
            var cmd = new GetAboutUsCommand
            {
                PageIndex = 1,
                PageSize = 100,
                Show = true
            };
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result.Data);
        }

        public async Task<IActionResult> Detail(int Id)
        {
            var pageData = await _mediator.Send(new GetAboutUsCommand()
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