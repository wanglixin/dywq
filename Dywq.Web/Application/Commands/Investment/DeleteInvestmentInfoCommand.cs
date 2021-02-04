using DotNetCore.CAP;
using Dywq.Domain.InvestmentAggregate;
using Dywq.Domain.UserAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Investment
{
    public class DeleteInvestmentInfoCommand : IRequest<Result>
    {
        [Range(1, int.MaxValue, ErrorMessage = "id错误")]
        public int Id { get; set; }


        public LoginUserDTO LoginUser { get; set; }
    }

    public class DeleteInvestmentInfoCommandHandler : BaseRequestHandler<DeleteInvestmentInfoCommand, Result>
    {

        readonly IBaseRepository<InvestmentInfo> _InvestmentInfoRepository;
        readonly IBaseRepository<User> _userRepository;
        readonly IBaseRepository<CompanyUser> _companyUserRepository;

        public DeleteInvestmentInfoCommandHandler(
             ICapPublisher capPublisher,
            ILogger<DeleteInvestmentInfoCommandHandler> logger,
             IBaseRepository<InvestmentInfo> InvestmentInfoRepository,
             IBaseRepository<User> userRepository,
             IBaseRepository<CompanyUser> companyUserRepository

            ) : base(capPublisher, logger)
        {
            _InvestmentInfoRepository = InvestmentInfoRepository;
            _userRepository = userRepository;
            _companyUserRepository = companyUserRepository;

        }

        public override async Task<Result> Handle(DeleteInvestmentInfoCommand request, CancellationToken cancellationToken)
        {

            var item = await _InvestmentInfoRepository.Set().FirstOrDefaultAsync(x => x.Id == request.Id);

            if (item == null)
            {
                return Result.Failure($"内容不存在");
            }
            _InvestmentInfoRepository.Set().Remove(item);

            return Result.Success();
        }
    }

}
