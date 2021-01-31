using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
using Dywq.Domain.CooperationAggregate;
using Dywq.Domain.News;
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
    public class SubmitCheckPurchaseCommand : IRequest<Result>
    {
        [Required(ErrorMessage = "id不能为空")]
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public int Id { get; set; }

        /// <summary>
        /// 用户id，可能是企业用户或管理员
        /// </summary>
        public int UserId { get; set; }
    }


    public class SubmitCheckPurchaseCommandHandler : BaseRequestHandler<SubmitCheckPurchaseCommand, Result>
    {

        readonly IBaseRepository<Dywq.Domain.Purchase.Purchase> _purchaseRepository;
        readonly IBaseRepository<User> _userRepository;

        public SubmitCheckPurchaseCommandHandler(
             ICapPublisher capPublisher,
            ILogger<SubmitCheckPurchaseCommandHandler> logger,
             IBaseRepository<Dywq.Domain.Purchase.Purchase> purchaseRepository,
              IBaseRepository<User> userRepository
            ) : base(capPublisher, logger)
        {
            _purchaseRepository = purchaseRepository;
            _userRepository = userRepository;

        }

        public override async Task<Result> Handle(SubmitCheckPurchaseCommand request, CancellationToken cancellationToken)
        {
            var item = await _purchaseRepository.Set().FirstOrDefaultAsync(x => x.Id == request.Id);
            if (item == null)
            {
                return Result.Failure($"内容不存在");
            }
            var user = await _userRepository.Set().FirstOrDefaultAsync(x => x.Id == request.UserId);

            if (user == null)
            {
                return Result.Failure($"用户不存在");
            }
            if (item.Status != -1 && item.Status != 0)
            {
                return Result.Failure($"当前状态不能提交审核");
            }
            if (user.Type != 2) //用户删除
            {
                return Result.Failure($"只有编辑才能提交审核");
            }
            item.Status = 1;
            await _purchaseRepository.UpdateAsync(item);
            return Result.Success(); ;
        }
    }
}
