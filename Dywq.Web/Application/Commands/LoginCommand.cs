using DotNetCore.CAP;
using Dywq.Domain.Abstractions;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Dywq.Infrastructure.Core.Utilitiy;
using Dywq.Domain.UserAggregate;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Dywq.Web.Dto.User;
using Dywq.Domain.CompanyAggregate;
using Dywq.Web.Common;

namespace Dywq.Web.Application.Commands
{
    public class LoginCommand : IRequest<LoginUserDTO>
    {
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginUserDTO>
    {
        readonly IBaseRepository<User> _userRepository;
        readonly IBaseRepository<CompanyUser> _companyUserRepository;
        readonly IBaseRepository<Company> _companyRepository;

        readonly IBaseRepository<CompanyFieldData> _companyFieldDataRepository;

        readonly ICapPublisher _capPublisher;
        readonly ILogger<LoginCommandHandler> _logger;
        readonly IMd5 _md5;


        public LoginCommandHandler(IBaseRepository<User> userRepository,
            IBaseRepository<CompanyUser> companyUserRepository,
            IBaseRepository<Company> companyRepository,
             IBaseRepository<CompanyFieldData> companyFieldDataRepository,
            ICapPublisher capPublisher,
            ILogger<LoginCommandHandler> logger,
            IMd5 md5)
        {
            _userRepository = userRepository;
            _companyUserRepository = companyUserRepository;
            _companyRepository = companyRepository;
            _companyFieldDataRepository = companyFieldDataRepository;
            _capPublisher = capPublisher;
            _logger = logger;
            _md5 = md5;
        }

        public async Task<LoginUserDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var pwd = _md5.Md5(request.Password);
            _logger.LogInformation(pwd);
            var user = await _userRepository.GetAsync(x => x.UserName == request.UserName
             && x.Password == pwd);
            if (user == null) return null;

            var dto = new LoginUserDTO();
            dto.UserName = user.UserName;
            dto.Id = user.Id;
            dto.Type = user.Type;

            var commpanyUser = await _companyUserRepository.GetAsync(x => x.UserId == user.Id);
            if (commpanyUser == null)
            {
                dto.CompanyName = "[未绑定企业]";
                dto.CompanyId = 0;

            }
            else
            {
                var commpany = await _companyRepository.GetAsync(x => x.Id == commpanyUser.CompanyId);
                dto.CompanyId = commpany.Id;
                dto.Logo = commpany.Logo;
                dto.CompanyName = commpany.Name;

                //获取企业名称

                //var companyFieldData = await _companyFieldDataRepository.GetAsync(x => x.Alias == CompanyFieldAlias.CompanyName);

                //dto.CompanyName = companyFieldData?.Value;
            }

            return dto;


        }
    }
}
