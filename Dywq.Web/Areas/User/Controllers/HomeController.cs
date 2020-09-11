using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands;
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

        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> Test(GetCompanyFieldsCommand cmd)
        {
            var obj = await _mediator.Send(cmd, HttpContext.RequestAborted);

            return Json(obj);
        }

    }
}