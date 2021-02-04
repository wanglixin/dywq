using DotNetCore.CAP;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Expert;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Expert
{
    public class GetAllExpertsCommand : IRequest<IEnumerable<ExpertGroupDTO>>
    {
        public int? Status { get; set; }
    }


    public class GetAllExpertsCommandHandler : BaseRequestHandler<GetAllExpertsCommand, IEnumerable<ExpertGroupDTO>>
    {

        readonly IBaseRepository<Dywq.Domain.Expert.Expert> _expertRepository;
        readonly IBaseRepository<Domain.Expert.ExpertType> _expertTypeRepository;

        public GetAllExpertsCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetAllExpertsCommandHandler> logger,
            IBaseRepository<Dywq.Domain.Expert.Expert> expertRepository,
             IBaseRepository<Domain.Expert.ExpertType> expertTypeRepository
            ) : base(capPublisher, logger)
        {
            _expertRepository = expertRepository;
            _expertTypeRepository = expertTypeRepository;
        }

        public override async Task<IEnumerable<ExpertGroupDTO>> Handle(GetAllExpertsCommand request, CancellationToken cancellationToken)
        {
            var sb = new List<string>();
            sb.Add($"e.show = 1");
            if (request.Status.HasValue)
            {
                sb.Add($"e.Status = " + request.Status.Value);
            }
            var where = string.Join(" and ", sb);
            var sql= @$"SELECT e.*,t.Name as 'ExpertTypeName'
  FROM [Expert] as e left join expertType as t on e.ExpertTypeId = t.Id where {where} order by t.Sort,e.Sort,e.Id desc";
            var experts = await _expertRepository.SqlQueryAsync<ExpertDTO>(sql);

            if (experts == null)
            {
                return null;
            }

            var expertGroups = new List<ExpertGroupDTO>();
            var expertDic = new Dictionary<ExpertTypeDTO, List<ExpertDTO>>();

            foreach (var expert in experts)
            {
                var typeid = expert.ExpertTypeId;
                var key = expertDic.Keys.FirstOrDefault(x => x.Id == typeid);
                if (key == null)
                {
                    key = new ExpertTypeDTO() { Id = typeid, Name = expert.ExpertTypeName };
                    expertDic[key] = new List<ExpertDTO>();
                }

                expertDic[key].Add(expert);

            }
            var result = expertDic.Select(x => new ExpertGroupDTO()
            {
                Experts = x.Value,
                ExpertType = x.Key
            });

            return result;

        }
    }
}
