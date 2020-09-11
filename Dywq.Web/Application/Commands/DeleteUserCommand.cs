using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.UserAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands
{
    public class DeleteUserCommand : IRequest<Result>
    {
        [Range(1, int.MaxValue, ErrorMessage = "用户id错误")]
        public int Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<DeleteUserCommandHandler> _logger;

        readonly IBaseRepository<User> _userRepository;
        readonly IBaseRepository<CompanyUser> _companyUserRepository;

        public DeleteUserCommandHandler(
             ICapPublisher capPublisher,
            ILogger<DeleteUserCommandHandler> logger,
             IBaseRepository<User> userRepository,
             IBaseRepository<CompanyUser> companyUserRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;
            _userRepository = userRepository;
            _companyUserRepository = companyUserRepository;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var _user = await _userRepository.GetAsync(x => x.Id == request.Id);
            if (_user == null) return Result.Failure("用户不存在");
            if(_user.UserName.ToLower()=="admin") return Result.Failure("该管理员不能删除");

            await _userRepository.RemoveAsync(_user);

            var _coms = await _companyUserRepository.Set().Where(x => x.UserId == request.Id).ToListAsync();

            _coms.ForEach(async x =>
            {
                await _companyUserRepository.RemoveAsync(x);
            });

            return Result.Success("删除成功");

        }
    }
}
