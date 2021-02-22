using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands.Purchase;
using Dywq.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class PurchaseController : BaseController
    {
        public PurchaseController(IMediator mediator, ILogger<PurchaseController> logger) : base(mediator, logger)
        {

        }


        [Authorize(Roles = Common.Role.User + "," + Common.Role.Editor + "," + Common.Role.Admin)]
        public async Task<IActionResult> EditC(int? Id, int type = 0)
        {
            ViewBag.type = type;
            if (Id.HasValue)
            { //修改

                var result = await _mediator.Send(new GetPurchasesCommand()
                {
                    Id = Id.Value,
                    CompanyId = this.CurrentUser.CompanyId,
                    Type = type
                }, HttpContext.RequestAborted);
                return View(result?.Data?.FirstOrDefault());
            }
            return View();
        }


        [Authorize(Roles = Common.Role.User + "," + Common.Role.Editor)]
        public async Task<IActionResult> ListC(GetPurchasesCommand cmd, int type = 0)
        {
            ViewBag.type = type;
            var user = this.CurrentUser;
            cmd.LoginUser = user;
            cmd.CompanyId = user.CompanyId;
            cmd.Type = type;
            cmd.LinkUrl = $"/user/Purchase/ListC?PageIndex=__id__&PageSize={cmd.PageSize}&type={type}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }


        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> List(GetPurchasesCommand cmd, int type = 0)
        {
            ViewBag.type = type;
            // cmd.Status = -888;
            cmd.LinkUrl = $"/user/purchase/list?PageIndex=__id__&PageSize={cmd.PageSize}&type={type}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }


        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> Edit(int? Id, int type = 0)
        {
            if (!Id.HasValue)
            {
                return Content("参数错误");
            }
            ViewBag.type = type;
            //修改
            var result = await _mediator.Send(new GetPurchasesCommand() { Id = Id.Value }, HttpContext.RequestAborted);
            return View(result?.Data?.FirstOrDefault());
        }


    }
}