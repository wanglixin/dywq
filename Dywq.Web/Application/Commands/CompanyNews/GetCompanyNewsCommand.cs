using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.CooperationAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Commpany;
using Dywq.Web.Dto.Cooperation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.CompanyNews
{
    public class GetCompanyNewsCommand : IRequest<PageResult<CompanyNewsDto>>
    {
        public int Id { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string LinkUrl { get; set; }

        /// <summary>
        /// 政策类型id
        /// </summary>
        public string CompanyTypeId { get; set; } = "";
        public bool? Show { get; set; } = null;

        public int CompanyId { get; set; } = 0;

        public int Status { get; set; } = -999;

    }

    public class GetCompanyNewsCommandHandler : BaseRequestHandler<GetCompanyNewsCommand, PageResult<CompanyNewsDto>>
    {

        readonly IBaseRepository<Dywq.Domain.CompanyAggregate.CompanyNews> _companyNewsRepository;
        readonly IBaseRepository<CompanyType> _companyTypeRepository;
        readonly IBaseRepository<Company> _companyRepository;

        public GetCompanyNewsCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetCompanyNewsCommandHandler> logger,
            IBaseRepository<Dywq.Domain.CompanyAggregate.CompanyNews> companyNewsRepository,
             IBaseRepository<CompanyType> companyTypeRepository,
             IBaseRepository<Company> companyRepository
            ) : base(capPublisher, logger)
        {
            _companyNewsRepository = companyNewsRepository;
            _companyTypeRepository = companyTypeRepository;
            _companyRepository = companyRepository;
        }

        public override async Task<PageResult<CompanyNewsDto>> Handle(GetCompanyNewsCommand request, CancellationToken cancellationToken)
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

            if (request.Status != -999)
            {
                sb.Add($"Status = " + request.Status);
            }


            if (request.CompanyId != 0)
            {
                sb.Add($"CompanyId = " + request.CompanyId);
            }


            if (!string.IsNullOrWhiteSpace(request.CompanyTypeId))
            {
                sb.Add($"CompanyTypeId = " + request.CompanyTypeId);
            }
            var where = string.Join(" and ", sb);


            var pageData = await _companyNewsRepository.GetPageDataAsync<Domain.CompanyAggregate.CompanyNews>(
               pageIndex: request.PageIndex,
               pageSize: request.PageSize,
               where: where,
               order: "Status asc,Sort asc,Id desc"
               );
            var count = pageData.Total;
            if (count < 1)
            {
                return PageResult<CompanyNewsDto>.Success(null, count, request.PageIndex, request.PageSize, request.LinkUrl);
            }

            var types = await _companyTypeRepository.Set().ToListAsync();

            var companyId_arr = pageData.Data.Select(x => x.CompanyId);

            var companys = _companyRepository.Set().Where(x => companyId_arr.Contains(x.Id));

            var data = pageData.Data.Select(x =>

                new CompanyNewsDto()
                {
                    CompanyId = x.CompanyId,
                    CompanyName = companys.FirstOrDefault(c => c.Id == x.CompanyId)?.Name,
                    Id = x.Id,
                    CompanyTypeId = x.CompanyTypeId,
                    Contact = x.Contact,
                    CooperationContent = x.CooperationContent,
                    Introduce = x.Introduce,
                    IntroduceImage = x.IntroduceImage,
                    MainBusiness = x.MainBusiness,
                    Show = x.Show,
                    Sort = x.Sort,
                    Status = x.Status,
                    Title = x.Title,
                    CreatedTime = x.CreatedTime,
                    UpdatedTime = x.UpdatedTime
                }
            ); ;

            return PageResult<CompanyNewsDto>.Success(data, count, request.PageIndex, request.PageSize, request.LinkUrl);

        }
    }
}
