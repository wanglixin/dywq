using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
using Dywq.Domain.CooperationAggregate;
using Dywq.Domain.InvestmentAggregate;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Cooperation;
using Dywq.Web.Dto.Investment;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Investment
{
    public class GetInvestmentTypesCommand : IRequest<IEnumerable<InvestmentTypeDTO>>
    {
    }

    public class GetInvestmentTypesHandler : BaseRequestHandler<GetInvestmentTypesCommand, IEnumerable<InvestmentTypeDTO>>
    {
        readonly IBaseRepository<InvestmentType> _investmentTypeRepository;
        public GetInvestmentTypesHandler(
            ICapPublisher capPublisher,
            ILogger<GetInvestmentTypesHandler> logger,
            IBaseRepository<InvestmentType> investmentTypeRepository
            ) : base(capPublisher, logger)
        {
            _investmentTypeRepository = investmentTypeRepository; ;
        }

        public override async Task<IEnumerable<InvestmentTypeDTO>> Handle(GetInvestmentTypesCommand request, System.Threading.CancellationToken cancellationToken)
        {
            var types = await _investmentTypeRepository.Set().OrderBy(x => x.Sort).ThenByDescending(x => x.Id).ToListAsync();


            var list = new List<InvestmentTypeDTO>();
            foreach(var type in types)
            {
                list.Add(new InvestmentTypeDTO() { Id=type.Id, Name=type.Name,Sort=type.Sort });
            }

            //var list = types.Select(x => new InvestmentTypeDTO()
            //{
            //    Id = x.Id,
            //    Name = x.Name,
            //    Sort = x.Sort
            //}).ToList();

            return list;

        }
    }
}
