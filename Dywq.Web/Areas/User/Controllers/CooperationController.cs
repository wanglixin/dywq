using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands.Cooperation;
using Dywq.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class CooperationController : BaseController
    {
        public CooperationController(IMediator mediator, ILogger<CooperationController> logger) : base(mediator, logger)
        {

        }

        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (!Id.HasValue)
            {
                return Content("参数错误");
            }

            var types = await _mediator.Send(new GetCooperationTypesCommand() { }, HttpContext.RequestAborted);

            ViewBag.types = types;

            //修改
            var result = await _mediator.Send(new GetCooperationInfosCommand() { Id = Id.Value }, HttpContext.RequestAborted);
            return View(result?.Data?.FirstOrDefault());
        }


        [Authorize(Roles = Common.Role.User + "," + Common.Role.Editor + "," + Common.Role.Admin)]
        public async Task<IActionResult> EditC(int? Id)
        {
            var types = await _mediator.Send(new GetCooperationTypesCommand() { }, HttpContext.RequestAborted);
            ViewBag.types = types;
            if (Id.HasValue)
            { //修改

                var result = await _mediator.Send(new GetCooperationInfosCommand() { Id = Id.Value, CompanyId = this.CurrentUser.CompanyId }, HttpContext.RequestAborted);
                return View(result?.Data?.FirstOrDefault());
            }
            return View();
        }




        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> List(GetCooperationInfosCommand cmd)
        {
            cmd.Status = -888;
            cmd.LinkUrl = $"/user/cooperation/list?PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }

        [Authorize(Roles = Common.Role.User + "," + Common.Role.Editor)]
        public async Task<IActionResult> ListC(GetCooperationInfosCommand cmd)
        {
            var user = this.CurrentUser;

            cmd.CompanyId = user.CompanyId;
            //cmd.Status = -888;

            cmd.LinkUrl = $"/user/cooperation/ListC?PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }


    }
}