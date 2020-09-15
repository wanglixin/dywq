using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.UserAggregate;
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

    /// <summary>
    /// 管理员审核企业动态列表
    /// </summary>
    public class GetCompanyInfosCommand : IRequest<PageResult<CompanyInfoDTO>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string Key { get; set; }


        public string LinkUrl { get; set; }
    }

    public class GetCompanyInfosCommandHandler : IRequestHandler<GetCompanyInfosCommand, PageResult<CompanyInfoDTO>>
    {

        readonly ICapPublisher _capPublisher;
        readonly ILogger<GetCompanyInfosCommandHandler> _logger;

        readonly IBaseRepository<Company> _companyRepository;
        readonly IBaseRepository<CompanyType> _companyTypeRepository;

        public GetCompanyInfosCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetCompanyInfosCommandHandler> logger,
            IBaseRepository<Company> companyRepository,
             IBaseRepository<CompanyType> companyTypeRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;

            _companyRepository = companyRepository;
            _companyTypeRepository = companyTypeRepository;
        }


        public async Task<PageResult<CompanyInfoDTO>> Handle(GetCompanyInfosCommand request, CancellationToken cancellationToken)
        {

            var companySet = _companyRepository.Set().AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.Key))
            {
                companySet = companySet.Where(x => x.Name.Contains(request.Key));

            }

            var count = await companySet.CountAsync();

            if (count < 1) return PageResult<CompanyInfoDTO>.Success(null, 0, request.PageIndex, request.PageSize, "");
            var start = (request.PageIndex - 1) * request.PageSize;
            var end = start + request.PageSize;

            //int[] order = { 1, 2, -1, 0 };

            var query =  companySet
                .OrderBy(x => (x.Status + 3) % 4)
                //.OrderByDescending(x => x.Status == 1)
                //.ThenByDescending(x => x.Status == 2)
                //.ThenByDescending(x => x.Status == -1)
                //.ThenByDescending(x => x.Status == 0)
                .ThenBy(x => x.Sort)
                .ThenByDescending(x => x.Id)
                .Skip(start)
                .Take(request.PageSize);

            _logger.LogInformation(query.ToSql());

            var data = await query.ToListAsync();






            var companyTypes = await _companyTypeRepository.Set().ToListAsync();


            var _data = new List<CompanyInfoDTO>();

            data.ForEach(x =>
            {
                var type = companyTypes.FirstOrDefault(c => c.Id == x.CompanyTypeId);
                var dto = new CompanyInfoDTO
                {

                    Status = x.Status,
                    CompanyId = x.Id,
                    CompanyTypeId = x.CompanyTypeId,
                    CompanyTypeName = type?.Name,
                    Contact = x.Contact,
                    CooperationContent = x.CooperationContent,
                    Introduce = x.Introduce,
                    IntroduceImage = x.IntroduceImage,
                    MainBusiness = x.MainBusiness,
                    Show = x.Show,
                    Sort = x.Sort,
                    UpdatedTime = x.UpdatedTime,
                    CompanyName = x.Name
                };

                _data.Add(dto);

            });

            return PageResult<CompanyInfoDTO>.Success(_data, count, request.PageIndex, request.PageSize, request.LinkUrl);

        }
    }
}
