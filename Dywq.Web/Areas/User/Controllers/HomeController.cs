using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class HomeController : Controller
    {
        IMediator _mediator;
        readonly ILogger<HomeController> _logger;

        public HomeController(IMediator mediator, ILogger<HomeController> logger)
        {
            _mediator = mediator;
            _logger = logger;

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