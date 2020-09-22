using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands.Article;
using Dywq.Web.Application.Commands.Expert;
using Dywq.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class ExpertController : BaseController
    {

        public ExpertController(IMediator mediator, ILogger<ExpertController> logger) : base(mediator, logger)
        {

        }

        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> EditType(int? Id)
        {
            if (Id.HasValue)
            {
                var result = await _mediator.Send(new GetExpertTypesCommand() { Id = Id.Value }, HttpContext.RequestAborted);
                return View(result?.Data?.FirstOrDefault());
            }
            return View();

        }

        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> TypeList(GetExpertTypesCommand cmd)
        {
            cmd.LinkUrl = $"/user/expert/typeList?PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }


        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> Edit(int? Id)
        {

            var types = await _mediator.Send(new GetExpertTypesCommand() {  PageSize=int.MaxValue }, HttpContext.RequestAborted);

            ViewBag.types = types.Data;

            if (Id.HasValue)
            {
                var result = await _mediator.Send(new GetExpertsCommand() { Id = Id.Value }, HttpContext.RequestAborted);
                return View(result?.Data?.FirstOrDefault());
            }
            return View();

        }



        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> List(GetExpertsCommand cmd)
        {
            cmd.LinkUrl = $"/user/expert/list?PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }

    }
}