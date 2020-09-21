using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands.Financing;
using Dywq.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class FinancingController : BaseController
    {
        public FinancingController(IMediator mediator, ILogger<FinancingController> logger) : base(mediator, logger)
        {

        }

        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (!Id.HasValue)
            {
                return Content("参数错误");
            }

            //修改
            var result = await _mediator.Send(new GetFinancingsCommand() { Id = Id.Value }, HttpContext.RequestAborted);
            return View(result?.Data?.FirstOrDefault());
        }


        [Authorize(Roles = Common.Role.User)]
        public async Task<IActionResult> EditC(int? Id)
        {
            if (Id.HasValue)
            { //修改

                var result = await _mediator.Send(new GetFinancingsCommand() { Id = Id.Value, CompanyId = this.CurrentUser.CompanyId }, HttpContext.RequestAborted);
                return View(result?.Data?.FirstOrDefault());
            }
            return View();
        }




        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> List(GetFinancingsCommand cmd)
        {
            cmd.LinkUrl = $"/user/financing/list?PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }

        [Authorize(Roles = Common.Role.User)]
        public async Task<IActionResult> ListC(GetFinancingsCommand cmd)
        {
            var user = this.CurrentUser;

            cmd.CompanyId = user.CompanyId;

            cmd.LinkUrl = $"/user/financing/ListC?PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }


    }
}