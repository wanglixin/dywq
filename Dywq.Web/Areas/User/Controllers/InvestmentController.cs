using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands.Cooperation;
using Dywq.Web.Application.Commands.Investment;
using Dywq.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class InvestmentController : BaseController
    {
        public InvestmentController(IMediator mediator, ILogger<InvestmentController> logger) : base(mediator, logger)
        {

        }

        [Authorize(Roles = Common.Role.Admin + "," + Common.Role.Editor)]
        public async Task<IActionResult> Edit(int? Id)
        {

            var types = await _mediator.Send(new GetInvestmentTypesCommand() { }, HttpContext.RequestAborted);
            ViewBag.types = types;
            if (Id.HasValue)
            { //修改

                var result = await _mediator.Send(new GetInvestmentInfosCommand() { Id = Id.Value}, HttpContext.RequestAborted);
                return View(result?.Data?.FirstOrDefault());
            }
            return View();
        }




        [Authorize(Roles = Common.Role.Admin + "," + Common.Role.Editor)]
        public async Task<IActionResult> List(GetInvestmentInfosCommand cmd)
        {
            //cmd.Status = -888;
            cmd.LoginUser = this.GetCurrentUser();
            cmd.LinkUrl = $"/user/investment/list?PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }

    }
}