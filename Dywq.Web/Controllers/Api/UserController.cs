using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dywq.Domain.UserAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Core.Utilitiy;
using Dywq.Web.Application.Commands;
using Dywq.Web.Common;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dywq.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        IMediator _mediator;
        readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;

        }

        /// <summary>
        /// 请求登陆
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Login([FromBody]GetUserCommand cmd)
        {
            _logger.LogInformation($"接收到请求{HttpContext.Request.Host}{HttpContext.Request.Path},参数 {JsonConvert.SerializeObject(cmd)}");

            var user = await _mediator.Send(cmd, HttpContext.RequestAborted);

            if (user == null) return Result.Failure("用户名或密码错误");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(CompanyFieldAlias.CompanyName, user.CompanyName),
                new Claim("CompanyId", user.CompanyId.ToString())
             };

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "User"));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddDays(7),
            });
            return Result.Success("登陆成功");
        }




        public async Task<Result> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Result.Success();
        }
    }
}