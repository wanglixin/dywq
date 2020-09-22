using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Article;
using Dywq.Web.Dto.Expert;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Expert
{
    public class GetExpertTypesCommand : IRequest<PageResult<ExpertTypeDTO>>
    {
        public int Id { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string LinkUrl { get; set; }


    }


    public class GetExpertTypesCommandHandler : BaseRequestHandler<GetExpertTypesCommand, PageResult<ExpertTypeDTO>>
    {
        readonly IBaseRepository<Domain.Expert.ExpertType> _expertTypeRepository;

        public GetExpertTypesCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetExpertTypesCommandHandler> logger,
             IBaseRepository<Domain.Expert.ExpertType> expertTypeRepository
            ) : base(capPublisher, logger)
        {
            _expertTypeRepository = expertTypeRepository;
        }

        public override async Task<PageResult<ExpertTypeDTO>> Handle(GetExpertTypesCommand request, CancellationToken cancellationToken)
        {
            var sb = new List<string>();


            if (request.Id != 0)
            {
                sb.Add($"Id = {request.Id}");
            }

            var where = string.Join(" and ", sb);


            var pageData = await _expertTypeRepository.GetPageDataAsync<Domain.Expert.ExpertType>(
               pageIndex: request.PageIndex,
               pageSize: request.PageSize,
               where: where,
               order: "Sort asc,Id desc"
               );
            var count = pageData.Total;
            if (count < 1)
            {
                return PageResult<ExpertTypeDTO>.Success(null, count, request.PageIndex, request.PageSize, request.LinkUrl);
            }

            var data = pageData.Data.Select(x =>

                new ExpertTypeDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Sort = x.Sort,
                    CreatedTime = x.CreatedTime,
                    UpdatedTime = x.UpdatedTime
                }
            );

            return PageResult<ExpertTypeDTO>.Success(data, count, request.PageIndex, request.PageSize, request.LinkUrl);

        }
    }
}
