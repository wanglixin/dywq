using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands.Guestbook;
using Dywq.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class GuestbookController : BaseController
    {
        public GuestbookController(IMediator mediator, ILogger<GuestbookController> logger) : base(mediator, logger)
        {

        }

        [Authorize(Roles = Common.Role.Admin + "," + Common.Role.Editor)]
        public async Task<IActionResult> List(GetGuestbooksCommand cmd)
        {
            cmd.LinkUrl = $"/user/guestbook/list?PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }

        [Authorize(Roles = Common.Role.Admin + "," + Common.Role.Editor)]
        public async Task<IActionResult> Reply(int Id)
        {
            ViewBag.id = Id;
            return View();
        }

    }
}