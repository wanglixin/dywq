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
    public class GetExpertsCommand : IRequest<PageResult<ExpertDTO>>
    {
        public int Id { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string LinkUrl { get; set; }

        /// <summary>
        /// 专家类型ID
        /// </summary>
        public string TypeId { get; set; } = "";
        public bool? Show { get; set; } = true;

        public int? Status { get; set; } = null;
    }


    public class GetExpertsCommandHandler : BaseRequestHandler<GetExpertsCommand, PageResult<ExpertDTO>>
    {

        readonly IBaseRepository<Dywq.Domain.Expert.Expert> _expertRepository;
        readonly IBaseRepository<Domain.Expert.ExpertType> _expertTypeRepository;

        public GetExpertsCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetExpertsCommandHandler> logger,
            IBaseRepository<Dywq.Domain.Expert.Expert> expertRepository,
             IBaseRepository<Domain.Expert.ExpertType> expertTypeRepository
            ) : base(capPublisher, logger)
        {
            _expertRepository = expertRepository;
            _expertTypeRepository = expertTypeRepository;
        }

        public override async Task<PageResult<ExpertDTO>> Handle(GetExpertsCommand request, CancellationToken cancellationToken)
        {
            var sb = new List<string>();


            if (request.Id != 0)
            {
                sb.Add($"Id = {request.Id}");
            }

            if (request.Show.HasValue)
            {
                sb.Add($"Show = " + (request.Show.Value ? 1 : 0));
            }

            if (!string.IsNullOrWhiteSpace(request.TypeId))
            {
                sb.Add($"ExpertTypeId = " + request.TypeId);
            }

            if (request.Status.HasValue)
            {
                sb.Add($"Status = " + request.Status.Value);
            }


            var where = string.Join(" and ", sb);


            var pageData = await _expertRepository.GetPageDataAsync<Domain.Expert.Expert>(
               pageIndex: request.PageIndex,
               pageSize: request.PageSize,
               where: where,
               order: "Sort asc,Id desc"
               );
            var count = pageData.Total;
            if (count < 1)
            {
                return PageResult<ExpertDTO>.Success(null, count, request.PageIndex, request.PageSize, request.LinkUrl);
            }

            var typeid_arr = pageData.Data.GroupBy(x => x.ExpertTypeId).Select(x => x.Key);

            var types = await _expertTypeRepository.Set().Where(x => typeid_arr.Contains(x.Id)).ToListAsync();

            var data = pageData.Data.Select(x =>

                new ExpertDTO()
                {
                    Id = x.Id,
                    ExpertTypeId = x.ExpertTypeId,
                    ExpertTypeName = types.FirstOrDefault(t => t.Id == x.ExpertTypeId)?.Name,
                    Name = x.Name,
                    Introduction = x.Introduction,
                    Pic = x.Pic,
                    Show = x.Show,
                    Sort = x.Sort,
                    Title = x.Title,
                    CreatedTime = x.CreatedTime,
                    UpdatedTime = x.UpdatedTime,
                    Status = x.Status
                }
            );

            return PageResult<ExpertDTO>.Success(data, count, request.PageIndex, request.PageSize, request.LinkUrl);

        }
    }
}
