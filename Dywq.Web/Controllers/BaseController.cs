using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dywq.Web.Common;
using Dywq.Web.Dto.User;
using Dywq.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dywq.Web.Controllers
{

    public class BaseController : Controller
    {
        protected UserDTO CurrentUser { get; set; }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            CurrentUser = this.GetCurrentUser();
            ViewBag.CurrentUser = CurrentUser;

        }
        public UserDTO GetCurrentUser()
        {

            if (!HttpContext.User.Identity.IsAuthenticated)
                return null;

            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            var model = new UserDTO();
            model.UserName = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            int.TryParse(claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value, out int type);
            model.Type = type;

            int.TryParse(claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value, out int id);
            model.Id = id;

            int.TryParse(claimsIdentity.Claims.FirstOrDefault(x => x.Type == "CompanyId")?.Value, out int companyId);
            model.CompanyId = companyId;

            model.CompanyName = claimsIdentity.Claims.FirstOrDefault(x => x.Type == CompanyFieldAlias.CompanyName)?.Value;

            return model;

        }

    }





}