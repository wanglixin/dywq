using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
using Dywq.Domain.News;
using Dywq.Domain.SiteAggregate;
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

namespace Dywq.Web.Application.Commands.Site
{
    public class DeleteAboutUsCommand : IRequest<Result>
    {
        [Required(ErrorMessage = "id不能为空")]
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public int Id { get; set; }
    }


    public class DeleteAboutUsCommanddHandler : BaseRequestHandler<DeleteAboutUsCommand, Result>
    {

        readonly IBaseRepository<AboutUs> _aboutUsRepository;

        public DeleteAboutUsCommanddHandler(
             ICapPublisher capPublisher,
            ILogger<DeleteAboutUsCommanddHandler> logger,
             IBaseRepository<AboutUs> aboutUsRepository
            ) : base(capPublisher, logger)
        {
            _aboutUsRepository = aboutUsRepository;

        }

        public override async Task<Result> Handle(DeleteAboutUsCommand request, CancellationToken cancellationToken)
        {
            var item = await _aboutUsRepository.Set().FirstOrDefaultAsync(x => x.Id == request.Id);
            if (item != null)
            {
                _aboutUsRepository.Set().Remove(item);
            }
            else
            {
                return Result.Failure($"id={request.Id} 不存在");
            }
            return Result.Success(); ;
        }
    }
}
