using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
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

namespace Dywq.Web.Application.Commands.CompanyNews
{
    public class SubmitCheckCompanyNewsCommand : IRequest<Result>
    {
        [Required(ErrorMessage = "id不能为空")]
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public int Id { get; set; }

        /// <summary>
        /// 用户id，可能是企业用户或管理员
        /// </summary>
        public int UserId { get; set; }
    }


    public class SubmitCheckCompanyNewsCommandHandler : BaseRequestHandler<SubmitCheckCompanyNewsCommand, Result>
    {

        readonly IBaseRepository<Domain.CompanyAggregate.CompanyNews> _companyNewsRepository;
        readonly IBaseRepository<User> _userRepository;

        public SubmitCheckCompanyNewsCommandHandler(
             ICapPublisher capPublisher,
            ILogger<SubmitCheckCompanyNewsCommandHandler> logger,
             IBaseRepository<Domain.CompanyAggregate.CompanyNews> companyNewsRepository,
              IBaseRepository<User> userRepository
            ) : base(capPublisher, logger)
        {
            _companyNewsRepository = companyNewsRepository;
            _userRepository = userRepository;

        }

        public override async Task<Result> Handle(SubmitCheckCompanyNewsCommand request, CancellationToken cancellationToken)
        {
            var article = await _companyNewsRepository.Set().FirstOrDefaultAsync(x => x.Id == request.Id);
            if (article == null)
            {
                return Result.Failure($"内容不存在");
            }
            var user = await _userRepository.Set().FirstOrDefaultAsync(x => x.Id == request.UserId);

            if (user == null)
            {
                return Result.Failure($"用户不存在");
            }
            if (article.Status != -1 && article.Status != 0)
            {
                return Result.Failure($"当前状态不能提交审核");
            }
            if (user.Type != 2) //用户删除
            {
                return Result.Failure($"只有编辑才能提交审核");
            }
            article.Status = 1;
            await _companyNewsRepository.UpdateAsync(article);
            return Result.Success(); ;
        }
    }
}
