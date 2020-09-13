using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Commpany;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands
{
    public class GetCompanyBannersCommand : IRequest<IEnumerable<CompanyInfoDTO>>
    {

        public int Count { get; set; } = 10;

    }

    public class GetCompanyBannersCommandHandler : IRequestHandler<GetCompanyBannersCommand, IEnumerable<CompanyInfoDTO>>
    {

        readonly ICapPublisher _capPublisher;
        readonly ILogger<GetCompanyBannersCommandHandler> _logger;

        readonly IBaseRepository<Company> _companyRepository;


        public GetCompanyBannersCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetCompanyBannersCommandHandler> logger,
            IBaseRepository<Company> companyRepository

            )
        {
            _capPublisher = capPublisher;
            _logger = logger;

            _companyRepository = companyRepository;

        }


        public async Task<IEnumerable<CompanyInfoDTO>> Handle(GetCompanyBannersCommand request, CancellationToken cancellationToken)
        {


            var data = await _companyRepository.Set().OrderByDescending(x => x.CreatedTime).Where(x => !string.IsNullOrWhiteSpace(x.IntroduceImage)  && x.Show && x.Status == 2).Take(request.Count).ToListAsync();

            var _data = new List<CompanyInfoDTO>();

            data.ForEach(x =>
            {
                var dto = new CompanyInfoDTO
                {

                    //Status = x.Status,
                    CompanyId = x.Id,
                    // CompanyTypeId = x.CompanyTypeId,
                    //Contact = x.Contact,
                    //CooperationContent = x.CooperationContent,
                    //Introduce = x.Introduce,
                    IntroduceImage = x.IntroduceImage,
                    //MainBusiness = x.MainBusiness,
                    //Show = x.Show,
                    //Sort = x.Sort,
                    //UpdatedTime = x.UpdatedTime,
                    CompanyName = x.Name
                };

                _data.Add(dto);

            });

            return _data;
        }
    }
}
