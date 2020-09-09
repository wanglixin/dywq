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

        IMediator _mediator;
        readonly ILogger<CompanyController> _logger;

        public CompanyController(IMediator mediator, ILogger<CompanyController> logger)
        {
            _mediator = mediator;
            _logger = logger;

        }

        [Authorize( Roles = Common.Role.Admin)]
        public async Task<IActionResult> Add(GetCompanyFieldsCommand cmd)
        {
            var fields= await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(fields);

        }



        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> List(GetCompanysCommand cmd)
        {
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }
    }
}