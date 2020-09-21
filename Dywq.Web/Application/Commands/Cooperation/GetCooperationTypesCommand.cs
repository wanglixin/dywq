using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
using Dywq.Domain.CooperationAggregate;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Cooperation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Cooperation
{
    public class GetCooperationTypesCommand : IRequest<IEnumerable<CooperationTypeDTO>>
    {
    }

    public class GetCooperationTypesHandler : BaseRequestHandler<GetCooperationTypesCommand, IEnumerable<CooperationTypeDTO>>
    {
        readonly IBaseRepository<CooperationType> _cooperationTypeRepository;
        public GetCooperationTypesHandler(
            ICapPublisher capPublisher,
            ILogger<GetCooperationTypesHandler> logger,
            IBaseRepository<CooperationType> cooperationTypeRepository
            ) : base(capPublisher, logger)
        {
            _cooperationTypeRepository = cooperationTypeRepository; ;
        }

        public override async Task<IEnumerable<CooperationTypeDTO>> Handle(GetCooperationTypesCommand request, System.Threading.CancellationToken cancellationToken)
        {
            var types = await _cooperationTypeRepository.Set().OrderBy(x => x.Sort).ThenByDescending(x => x.Id).ToListAsync();

            var list = types.Select(x => new CooperationTypeDTO()
            {
                Id = x.Id,
                Name = x.Name,
                Sort = x.Sort
            });

            return list;

        }
    }
}
