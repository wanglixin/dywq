using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands.Article;
using Dywq.Web.Application.Commands.Guestbook;
using Dywq.Web.Application.Commands.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Controllers
{
    public class ArticleController : BaseController
    {
        public ArticleController(IMediator mediator, ILogger<ArticleController> logger) : base(mediator, logger)
        {


        }

        public async Task<IActionResult> PartyBuilding(GetPartyBuildingArticlesCommand cmd)
        {
            cmd.Show = true;
            cmd.LinkUrl = $"/article/partyBuilding?PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);

            return View(result);
        }

        public async Task<IActionResult> PartyBuildingDetail(int id)
        {
            var result = await _mediator.Send(new GetPartyBuildingArticlesCommand() { Id = id }, HttpContext.RequestAborted);
            return View(result?.Data?.FirstOrDefault());
        }



        public async Task<IActionResult> Policy(GetPolicyArticlesCommand cmd)
        {
            var types = await _mediator.Send(new GetPolicyTypesCommand() { }, HttpContext.RequestAborted);
            ViewBag.types = types;

            var typeId = string.IsNullOrWhiteSpace(cmd.TypeId) ? types.FirstOrDefault()?.Id.ToString() : cmd.TypeId;

            cmd.TypeId = typeId;
            cmd.Show = true;
            cmd.LinkUrl = $"/article/policy?typeId={typeId}&PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);


            ViewBag.PolicyTypeId = Convert.ToInt32(typeId);



            return View(result);
        }

        public async Task<IActionResult> PolicyDetail(int id, string _source = "")
        {
            var result = await _mediator.Send(new GetPolicyArticlesCommand() { Id = id }, HttpContext.RequestAborted);

            if ("user" == _source && this.CurrentUser != null)
            {
                //更新阅读状态 已读

                await _mediator.Send(new EditMessageCommand() { AssociationId = id, Type = 0, CompanyId = this.CurrentUser.CompanyId }, HttpContext.RequestAborted);
            }

            return View(result?.Data?.FirstOrDefault());
        }


        public async Task<IActionResult> GetGuestbook(GetGuestbooksCommand cmd)
        {
            cmd.LinkUrl = $"/article/getGuestbook?PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);
            return PartialView(result);
        }




    }
}