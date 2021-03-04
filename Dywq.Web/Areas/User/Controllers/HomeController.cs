using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands;
using Dywq.Web.Application.Commands.Message;
using Dywq.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class HomeController : BaseController
    {

        public HomeController(IMediator mediator, ILogger<HomeController> logger) : base(mediator, logger)
        {

        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (this.CurrentUser.Role == Common.Role.User)
            {
                var companyId = this.CurrentUser.CompanyId;
                var obj = await _mediator.Send(new GetTodoInfoCommand() { CompanyId = companyId }, HttpContext.RequestAborted);

                return View(obj);
            }
            else if (this.CurrentUser.Role == Common.Role.Admin)
            {
                return RedirectToAction("Statistic");
            }
            else if (this.CurrentUser.Role == Common.Role.Editor)
            {
                return RedirectToAction("StatisticForEditor");
            }
            return Content("角色类型不对");
        }

        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> Statistic()
        {
            var obj = await _mediator.Send(new GetAdminTodoInfoCommand() { }, HttpContext.RequestAborted);
            return View(obj);
        }

        [Authorize(Roles = Common.Role.Editor)]
        public async Task<IActionResult> StatisticForEditor()
        {
            var obj = await _mediator.Send(new GetEditorTodoInfoCommand() { LoginUser = this.GetCurrentUser() }, HttpContext.RequestAborted);
            return View(obj);
        }


        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> StatisticInfo(GetAdminTodoInfoCommand cmd)
        {
            cmd.Start = !cmd.Start.HasValue ? Convert.ToDateTime(DateTime.Now.ToString("yyyy-01-01")) : cmd.Start;
            cmd.End = !cmd.End.HasValue ? Convert.ToDateTime(DateTime.Now.ToString("yyyy-12-31")) : cmd.End;

            var obj = await _mediator.Send(cmd, HttpContext.RequestAborted);

            return View(obj);
        }



        [Authorize(Roles = Common.Role.User)]
        public async Task<IActionResult> GetMessage(GetMessagesCommand cmd)
        {
            var companyId = this.CurrentUser.CompanyId;
            cmd.CompanyId = companyId;
            //cmd.IsRead = 0;
            cmd.LinkUrl = "javascript:getdata(__id__)";
            var obj = await _mediator.Send(cmd, HttpContext.RequestAborted);

            

            return PartialView(obj);
        }


        public async Task<IActionResult> Test(GetCompanyFieldsCommand cmd)
        {
            var obj = await _mediator.Send(cmd, HttpContext.RequestAborted);

            return Json(obj);
        }

    }
}