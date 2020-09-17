using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Article;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Article
{
    public class GetPolicyTypesCommand : IRequest<IEnumerable<PolicyTypeDTO>>
    {
    }

    public class GetPolicyTypesCommandHandler : BaseRequestHandler<GetPolicyTypesCommand, IEnumerable<PolicyTypeDTO>>
    {
        readonly IBaseRepository<PolicyType> _policyTypeRepository;
        public GetPolicyTypesCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetPolicyTypesCommandHandler> logger,
            IBaseRepository<PolicyType> policyTypeRepository
            ):base(capPublisher,logger)
        {
            _policyTypeRepository = policyTypeRepository;
        }

        public override async Task<IEnumerable<PolicyTypeDTO>> Handle(GetPolicyTypesCommand request, System.Threading.CancellationToken cancellationToken)
        {
            var types = await _policyTypeRepository.Set().OrderBy(x => x.Sort).ToListAsync();

            var list = types.Select(x => new PolicyTypeDTO()
            {
                Id = x.Id,
                Name = x.Name,
                Sort = x.Sort
            });

            return list;

        }
    }
}
