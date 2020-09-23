using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dywq.Web.Application.Commands;
using Dywq.Web.Application.Commands.Cooperation;
using Dywq.Web.Application.Commands.Expert;
using Dywq.Web.Application.Commands.Financing;
using Dywq.Web.Application.Commands.Purchase;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Controllers
{
    public class CompanyController : BaseController
    {

        public CompanyController(IMediator mediator, ILogger<BaseController> logger) : base(mediator, logger)
        {


        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Test()
        {


            return Content("test");
        }


        public async Task<IActionResult> Detail(int id)
        {
            var result = await _mediator.Send(new GetCompanyInfoCommand() { CompanyId = id }, HttpContext.RequestAborted);

            return View(result);
        }




        public async Task<IActionResult> GetCompanyInfosByType(int type, int pageIndex = 1, int pageSize = 10, string linkUrl = "")
        {
            var result = await _mediator.Send(new GetCompanyInfosByTypeCommand() { TypeId = type, LinkUrl = linkUrl, PageIndex = pageIndex, PageSize = pageSize }, HttpContext.RequestAborted);
            return PartialView(result);
        }




        public async Task<IActionResult> Cooperation(int? Id, int pageIndex = 1, int pageSize = 10)
        {
            if (!Id.HasValue)
            {
                var result = await _mediator.Send(new GetCooperationTypesCommand() { }, HttpContext.RequestAborted);
                return View(result);
            }

            var pageData = await _mediator.Send(new GetCooperationInfosCommand()
            {
                LinkUrl = $"/Company/Cooperation/{Id}?PageIndex=__id__&PageSize={pageSize}",
                PageIndex = pageIndex,
                PageSize = pageSize,
                Show = true,
                Status = 1,
                TypeId = Id.ToString()
            }, HttpContext.RequestAborted);
            return View("CooperationList", pageData);
        }


        [Route("/Company/Cooperation/Detail/{id}")]
        public async Task<IActionResult> CooperationDetail(int Id)
        {
            var pageData = await _mediator.Send(new GetCooperationInfosCommand()
            {
                Id = Id,
                PageIndex = 1,
                PageSize = 1,
                Show = true,
                Status = 1
            }, HttpContext.RequestAborted);
            return View(pageData?.Data?.FirstOrDefault());
        }




        public async Task<IActionResult> Financing(int pageIndex = 1, int pageSize = 10)
        {
            var pageData = await _mediator.Send(new GetFinancingsCommand()
            {
                LinkUrl = $"/Company/Financing?PageIndex=__id__&PageSize={pageSize}",
                PageIndex = pageIndex,
                PageSize = pageSize,
                Show = true,
                Status = 1,
            }, HttpContext.RequestAborted);
            return View(pageData);
        }

        public async Task<IActionResult> Expert()
        {
            var data = await _mediator.Send(new GetAllExpertsCommand()
            {

            }, HttpContext.RequestAborted);
            return View(data);
        }

        [Route("/Company/Expert/{id}")]
        public async Task<IActionResult> ExpertDetail(int id)
        {
            var data = await _mediator.Send(new GetExpertsCommand()
            {
                Id = id,
                PageIndex = 1,
                PageSize = 1,
                Show = true

            }, HttpContext.RequestAborted);
            return View("ExpertDetail", data?.Data?.FirstOrDefault());
        }


        public async Task<IActionResult> Purchase(int type = 0, int pageIndex = 1, int pageSize = 10)
        {
            ViewBag.type = type;
            var pageData = await _mediator.Send(new GetPurchasesCommand()
            {
                LinkUrl = $"/company/purchase?pageIndex=__id__&pageSize={pageSize}",
                PageIndex = pageIndex,
                PageSize = pageSize,
                Show = true,
                Status = 1,
                Type = type
            }, HttpContext.RequestAborted);
            return View(pageData);
        }


        [Route("/company/purchase/detail/{id}")]
        public async Task<IActionResult> PurchaseDetail(int Id)
        {
            var pageData = await _mediator.Send(new GetPurchasesCommand()
            {
                Id = Id,
                PageIndex = 1,
                PageSize = 1,
                Show = true,
                Status = 1
            }, HttpContext.RequestAborted);
            return View(pageData?.Data?.FirstOrDefault());
        }

        public async Task<IActionResult> PurchaseSearch(string key, int type = 0)
        {
            ViewBag.type = type;
            var pageData = await _mediator.Send(new GetPurchasesCommand()
            {
                PageIndex = 1,
                PageSize = 8,
                Show = true,
                Status = 1,
                Type = type,
                Key = key
            }, HttpContext.RequestAborted);
            return PartialView(pageData.Data);
        }




    }
}