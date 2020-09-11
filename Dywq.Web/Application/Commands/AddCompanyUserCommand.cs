using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.UserAggregate;
using Dywq.Infrastructure.Core;
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
    public class AddCompanyUserCommand : IRequest<Result>
    {
        [Range(1, int.MaxValue, ErrorMessage = "用户id错误")]
        public int UserId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "用户id错误")]
        public int CompanyId { get; set; }
    }

    public class AddCompanyUserCommandHandler : IRequestHandler<AddCompanyUserCommand, Result>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<AddCompanyUserCommandHandler> _logger;

        readonly IBaseRepository<User> _userRepository;
        readonly IBaseRepository<CompanyUser> _companyUserRepository;
        readonly IBaseRepository<Company> _companyRepository;

        public AddCompanyUserCommandHandler(
             ICapPublisher capPublisher,
            ILogger<AddCompanyUserCommandHandler> logger,
             IBaseRepository<User> userRepository,
             IBaseRepository<CompanyUser> companyUserRepository,
             IBaseRepository<Company> companyRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;
            _userRepository = userRepository;
            _companyUserRepository = companyUserRepository;
            _companyRepository = companyRepository;
        }

        public async Task<Result> Handle(AddCompanyUserCommand request, CancellationToken cancellationToken)
        {
            var _user = await _userRepository.GetAsync(x => x.Id == request.UserId);
            if (_user == null) return Result.Failure("用户不存在");
            if (_user.Type == 1) return Result.Failure("管理员不能绑定");

            var _company = await _companyRepository.GetAsync(x => x.Id == request.CompanyId);

            if (_company == null) return Result.Failure("企业不存在");

            var _com_user = await _companyUserRepository.GetAsync(x => x.UserId == request.UserId);

            if (_com_user != null)
            {
                if (_com_user.CompanyId == _company.Id) return Result.Failure("不要重复绑定");
                _com_user.CompanyId = _company.Id;
                _com_user.UpdatedTime = DateTime.Now;

                await _companyUserRepository.UpdateAsync(_com_user);

                return Result.Success("修改成功");

            }
            else
            {
                _com_user = new CompanyUser()
                {
                     CompanyId=_company.Id,
                     UserId=_user.Id
                };

                await _companyUserRepository.AddAsync(_com_user);

                return Result.Success("绑定成功");
            }

         

          

        }
    }


}
