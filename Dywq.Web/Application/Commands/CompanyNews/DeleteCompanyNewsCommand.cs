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
    public class DeleteCompanyNewsCommand : IRequest<Result>
    {
        [Required(ErrorMessage = "id不能为空")]
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public int Id { get; set; }

        /// <summary>
        /// 用户id，可能是企业用户或管理员
        /// </summary>
        public int UserId { get; set; }
    }


    public class DeleteCompanyNewsCommandHandler : BaseRequestHandler<DeleteCompanyNewsCommand, Result>
    {

        readonly IBaseRepository<Domain.CompanyAggregate.CompanyNews> _companyNewsRepository;
        readonly IBaseRepository<User> _userRepository;

        public DeleteCompanyNewsCommandHandler(
             ICapPublisher capPublisher,
            ILogger<DeleteCompanyNewsCommandHandler> logger,
             IBaseRepository<Domain.CompanyAggregate.CompanyNews> companyNewsRepository,
              IBaseRepository<User> userRepository
            ) : base(capPublisher, logger)
        {
            _companyNewsRepository = companyNewsRepository;
            _userRepository = userRepository;

        }

        public override async Task<Result> Handle(DeleteCompanyNewsCommand request, CancellationToken cancellationToken)
        {
            var article = await _companyNewsRepository.Set().FirstOrDefaultAsync(x => x.Id == request.Id);
            if (article == null)
            {
                return Result.Failure($"内容不存在");
            }
            //var user = await _userRepository.Set().FirstOrDefaultAsync(x => x.Id == request.UserId);

            //if (user == null)
            //{
            //    return Result.Failure($"用户不存在");
            //}

            //if (user.Type == 0) //用户删除
            //{
            //    if (article.Status != -1)
            //    {
            //        return Result.Failure($"不能删除请联系管理员");
            //    }
            //}
            await _companyNewsRepository.RemoveAsync(article);
            return Result.Success(); ;
        }
    }
}
