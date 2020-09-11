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
using Newtonsoft.Json;

namespace Dywq.Web.Areas.User.Controllers
{

    [Authorize]
    [Area("User")]
    public class UserController : BaseController
    {

        public UserController(IMediator mediator, ILogger<UserController> logger) : base(mediator, logger)
        {

        }

        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> Add()
        {
            return View();
        }


        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> Edit(GetUserCommand cmd)
        {
            _logger.LogInformation($"接收到请求{HttpContext.Request.Host}{HttpContext.Request.Path},参数 {JsonConvert.SerializeObject(cmd)}");
            var user = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(user);
        }

        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> List(GetUsersCommand cmd)
        {
            _logger.LogInformation($"接收到请求{HttpContext.Request.Host}{HttpContext.Request.Path},参数 {JsonConvert.SerializeObject(cmd)}");
            var users = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(users);
        }




    }
}