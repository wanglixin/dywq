﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dywq.Web.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Dywq.Web.Application.Commands;
using MediatR;
using Dywq.Web.Application.Commands.Article;
using Dywq.Web.Application.Commands.Cooperation;
using Dywq.Web.Application.Commands.Financing;

namespace Dywq.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {

        public HomeController(IMediator mediator, ILogger<HomeController> logger) : base(mediator, logger)
        {


        }

        public async Task<IActionResult> Index()
        {
            //类型
            var types = await _mediator.Send(new GetCompanyTypesCommand(), HttpContext.RequestAborted);
            ViewBag.types = types;
            var type = types.FirstOrDefault()?.Id ?? 0;
            var companyInfos = await _mediator.Send(new GetCompanyInfosByTypeCommand()
            {
                PageIndex = 1,
                PageSize = 10,
                TypeId = type,
                LinkUrl = "javascript:getCompanyInfos(" + type + ",__id__,10)"

            }, HttpContext.RequestAborted);


            var banners = await _mediator.Send(new GetCompanyBannersCommand(), HttpContext.RequestAborted);
            ViewBag.banners = banners;

            //惠企政策

            var policyArticles = await _mediator.Send(new GetPolicyArticlesCommand() { Show = true, PageSize = 6}, HttpContext.RequestAborted);
            ViewBag.policyArticles = policyArticles.Data;

            //企业对接
            var cooperationInfos = await _mediator.Send(new GetCooperationInfosCommand() { Show = true, PageSize = 6, Status = 1 }, HttpContext.RequestAborted);
            ViewBag.cooperationInfos = cooperationInfos.Data;


            //企业对接
            var financings = await _mediator.Send(new GetFinancingsCommand() { Show = true, PageSize = 6, Status = 1 }, HttpContext.RequestAborted);
            ViewBag.financings = financings.Data;


            ViewBag.companyInfos = companyInfos;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
