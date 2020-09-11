using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Controllers
{
    public class CompanyController : BaseController
    {

        public CompanyController(IMediator mediator, ILogger<BaseController> logger) : base(mediator, logger)
        {


        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Test()
        {


            return Content("test");
        }




    }
}