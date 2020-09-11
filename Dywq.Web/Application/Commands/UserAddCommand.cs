using DotNetCore.CAP;
using Dywq.Domain.UserAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Core.Utilitiy;
using Dywq.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands
{
    public class UserAddCommand : IRequest<Result>
    {
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }

        [RegularExpression("[0|1]", ErrorMessage = "类型不正确")]
        public string Type { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
    }

    public class UserAddCommandCommandHandler : IRequestHandler<UserAddCommand, Result>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<UserAddCommandCommandHandler> _logger;
        readonly IBaseRepository<User> _userRepository;
        readonly IMd5 _md5;
        public UserAddCommandCommandHandler(
            ICapPublisher capPublisher,
            ILogger<UserAddCommandCommandHandler> logger,
            IBaseRepository<User> userRepository,
            IMd5 md5)
        {
            _capPublisher = capPublisher;
            _logger = logger;
            _md5 = md5;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(UserAddCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.AnyAsync(x => x.UserName == request.UserName))
            {
                return Result.Failure("用户名已存在");
            }
            var pwd = _md5.Md5(request.Password);
            await _userRepository.AddAsync(new User()
            {
                Password = pwd,
                UserName = request.UserName,
                Type = Convert.ToInt32(request.Type)
            }, cancellationToken);

            return Result.Success();

        }
    }
}
