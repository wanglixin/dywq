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

namespace Dywq.Web.Application.Commands
{
    public class LoginCommand : IRequest<Result>
    {
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; private set; }

        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; private set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result>
    {
        readonly IUserRepository _userRepository;
        readonly ICapPublisher _capPublisher;


        public LoginCommandHandler(IUserRepository userRepository, ICapPublisher capPublisher)
        {
            _userRepository = userRepository;
            _capPublisher = capPublisher;
        }

        public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            //var pwd = md5();
            var user =await _userRepository.GetAsync(x => x.UserName == request.UserName
            && x.Password == request.Password);
            if (user == null) return Result.Failure("用户名或密码错误");
            return Result.Success("登陆成功");
        }
    }
}
