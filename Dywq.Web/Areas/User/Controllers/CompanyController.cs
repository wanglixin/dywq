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
    public class CompanyController : BaseController
    {
        public CompanyController(IMediator mediator, ILogger<CompanyController> logger) : base(mediator, logger)
        {

        }

        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> Add(GetCompanyFieldsCommand cmd)
        {
            var fields = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(fields);
        }



        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> List(GetCompanysCommand cmd)
        {
            cmd.LinkUrl = $"/user/company/list/?PageIndex=__id__&PageSize={cmd.PageSize}&key={cmd.Key}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }



        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> Edit(GetCompanyCommand cmd)
        {

            var fields = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(fields);
        }



        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> GetList(GetCompanysCommand cmd)
        {
            cmd.LinkUrl = $"javascript:getlist(__id__)";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return PartialView(result);
        }

    }
}