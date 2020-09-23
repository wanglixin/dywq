using DotNetCore.CAP;
using Dywq.Domain.CooperationAggregate;
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

namespace Dywq.Web.Application.Commands.Purchase
{
    public class DeletePurchaseCommand : IRequest<Result>
    {
        [Range(1, int.MaxValue, ErrorMessage = "id错误")]
        public int Id { get; set; }


        /// <summary>
        /// 用户id，可能是企业用户或管理员
        /// </summary>
        public int UserId { get; set; }
    }

    public class DeletePurchaseCommandHandler : BaseRequestHandler<DeletePurchaseCommand, Result>
    {

        readonly IBaseRepository<Domain.Purchase.Purchase> _purchaseRepository;
        readonly IBaseRepository<User> _userRepository;
        readonly IBaseRepository<CompanyUser> _companyUserRepository;

        public DeletePurchaseCommandHandler(
             ICapPublisher capPublisher,
            ILogger<DeletePurchaseCommandHandler> logger,
             IBaseRepository<Domain.Purchase.Purchase> purchaseRepository,
             IBaseRepository<User> userRepository,
             IBaseRepository<CompanyUser> companyUserRepository

            ) : base(capPublisher, logger)
        {
            _purchaseRepository = purchaseRepository;
            _userRepository = userRepository;
            _companyUserRepository = companyUserRepository;

        }

        public override async Task<Result> Handle(DeletePurchaseCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Set().FirstOrDefaultAsync(x => x.Id == request.UserId);

            if (user == null)
            {
                return Result.Failure($"用户不存在");
            }

            if (user.Type == 0) //用户删除
            {
                var company_user = await _companyUserRepository.Set().FirstOrDefaultAsync(x => x.UserId == request.UserId);
                if (company_user == null)
                {
                    return Result.Failure($"您还未绑定企业");
                }

                var item = await _purchaseRepository.Set().FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == company_user.CompanyId);

                if (item == null)
                {
                    return Result.Failure($"内容不存在");
                }
                _purchaseRepository.Set().Remove(item);
            }
            else if (user.Type == 1)
            {

                var item = await _purchaseRepository.Set().FirstOrDefaultAsync(x => x.Id == request.Id);

                if (item == null)
                {
                    return Result.Failure($"内容不存在");
                }
                _purchaseRepository.Set().Remove(item);
            }
            else
            {
                return Result.Failure($"用户类型错误");
            }

            return Result.Success();
        }
    }

}
