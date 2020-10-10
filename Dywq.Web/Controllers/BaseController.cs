using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dywq.Web.Common;
using Dywq.Web.Dto.User;
using Dywq.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Controllers
{

    public class BaseController : Controller
    {


        protected IMediator _mediator;
        protected ILogger<BaseController> _logger;

        public BaseController(IMediator mediator, ILogger<BaseController> logger)
        {
            _mediator = mediator;
            _logger = logger;

            

        }

        protected LoginUserDTO CurrentUser { get; set; }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            CurrentUser = this.GetCurrentUser();
            ViewBag.CurrentUser = CurrentUser;

        }
        public LoginUserDTO GetCurrentUser()
        {

            if (!HttpContext.User.Identity.IsAuthenticated)
                return null;

            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            var model = new LoginUserDTO();
            model.UserName = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var role = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            model.Type = role == Role.User ? 0 : 1;

            int.TryParse(claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value, out int id);
            model.Id = id;

            int.TryParse(claimsIdentity.Claims.FirstOrDefault(x => x.Type == "CompanyId")?.Value, out int companyId);
            model.CompanyId = companyId;

            model.CompanyName = claimsIdentity.Claims.FirstOrDefault(x => x.Type == CompanyFieldAlias.CompanyName)?.Value;

            model.Logo = claimsIdentity.Claims.FirstOrDefault(x => x.Type == "Logo")?.Value;

            return model;

        }

    }





}