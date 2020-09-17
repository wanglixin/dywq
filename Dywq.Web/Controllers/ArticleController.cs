﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands.Article;
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

            var typeId = string.IsNullOrWhiteSpace(cmd.PolicyTypeId) ? types.FirstOrDefault()?.Id.ToString() : cmd.PolicyTypeId;

            cmd.PolicyTypeId = typeId;
            cmd.Show = true;
            cmd.LinkUrl = $"/article/policy?PolicyTypeId={typeId}&PageIndex=__id__&PageSize={cmd.PageSize}";
            var result = await _mediator.Send(cmd, HttpContext.RequestAborted);


            ViewBag.PolicyTypeId = Convert.ToInt32(typeId);



            return View(result);
        }

        public async Task<IActionResult> PolicyDetail(int id)
        {
            var result = await _mediator.Send(new GetPolicyArticlesCommand() { Id = id }, HttpContext.RequestAborted);
            return View(result?.Data?.FirstOrDefault());
        }



    }
}