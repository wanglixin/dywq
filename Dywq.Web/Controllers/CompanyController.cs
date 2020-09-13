using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands;
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


        public async Task<IActionResult> Detail(int id)
        {
            var result = await _mediator.Send(new GetCompanyInfoCommand() { CompanyId = id }, HttpContext.RequestAborted);

            return View(result);
        }




        public async Task<IActionResult> GetCompanyInfosByType(int type, int pageIndex = 1, int pageSize = 10, string linkUrl = "")
        {
            var result = await _mediator.Send(new GetCompanyInfosByTypeCommand() { TypeId = type, LinkUrl = linkUrl, PageIndex = pageIndex, PageSize = pageSize }, HttpContext.RequestAborted);
            return PartialView(result);
        }

    }
}