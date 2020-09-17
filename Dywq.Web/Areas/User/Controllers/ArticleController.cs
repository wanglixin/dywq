using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands.Article;
using Dywq.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class ArticleController : BaseController
    {
        public ArticleController(IMediator mediator, ILogger<ArticleController> logger) : base(mediator, logger)
        {

        }

        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> EditPartyBuildingArticle(int? Id)
        {
            if (Id.HasValue)
            { //修改
                var result = await _mediator.Send(new GetPartyBuildingArticlesCommand() { Id = Id.Value }, HttpContext.RequestAborted);
                return View(result?.Data?.FirstOrDefault());
            }
            return View();
        }


        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> PartyBuildingArticleList(GetPartyBuildingArticlesCommand cmd)
        {
            cmd.LinkUrl = $"/user/article/PartyBuildingArticleList?PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }

        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> EditPolicyArticle(int? Id)
        {

            var types = await _mediator.Send(new GetPolicyTypesCommand() { }, HttpContext.RequestAborted);

            ViewBag.types = types;

            if (Id.HasValue)
            { //修改
                var result = await _mediator.Send(new GetPolicyArticlesCommand() { Id = Id.Value }, HttpContext.RequestAborted);
                return View(result?.Data?.FirstOrDefault());
            }
            return View();
        }


        [Authorize(Roles = Common.Role.Admin)]
        public async Task<IActionResult> PolicyArticleList(GetPolicyArticlesCommand cmd)
        {
            cmd.LinkUrl = $"/user/article/PolicyArticleList?PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return View(result);
        }





    }
}