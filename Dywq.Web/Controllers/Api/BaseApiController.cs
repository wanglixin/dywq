using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dywq.Infrastructure.Core;
using Dywq.Web.Common;
using Dywq.Web.Dto.User;
using Dywq.Web.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dywq.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseApiController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger<BaseApiController> _logger;
        protected readonly IWebHostEnvironment _webhostEnvironment;

       

        public BaseApiController(IMediator mediator, ILogger<BaseApiController> logger, IWebHostEnvironment webhostEnvironment)
        {
            _mediator = mediator;
            _logger = logger;
            _webhostEnvironment = webhostEnvironment;

        }


        [NonAction]
        /// <summary>
        /// 返回图片路径
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task<Result<string>> Upload(IFormFile file, string dir)
        {
            _logger.LogInformation("上传");
            _logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(file));
            string webRootPath = _webhostEnvironment.WebRootPath;
            var fileName = Guid.NewGuid().ToString().ToLower();
            var ext = Path.GetExtension(file.FileName).ToLower();
            fileName = $"{fileName}{ext}";
            _logger.LogInformation(fileName);
            var filePath = Path.Combine(webRootPath, @$"upload/{dir}/");
            _logger.LogInformation(filePath);
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

            using (var stream = System.IO.File.Create($"{filePath}{fileName}"))
            {
                await file.CopyToAsync(stream);
            }
            var _fileName = $"/upload/{dir}/{fileName}";
            return Result<string>.Success(_fileName);
        }

        [Authorize]
        public async Task<Result<string>> UploadImg(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLower();
            var list = new List<string>
            {
                ".png",
                ".jpg",
                ".jpeg",
                ".gif",
                ".bmp"
            };
            if (!list.Contains(ext)) return Result<string>.Failure("图片格式不正确");

            return await Upload(file, "logo");

        }

        public LoginUserDTO GetCurrentUser()
        {

            if (!HttpContext.User.Identity.IsAuthenticated)
                return null;

            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            var model = new LoginUserDTO();
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