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
    /// <summary>
    /// 根据类型获取企业信息列表
    /// </summary>
    public class GetCompanyInfosByTypeCommand : IRequest<PageResult<CompanyInfoDTO>>
    {
        public int TypeId { get; set; }

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string LinkUrl { get; set; }
    }

    public class GetCompanyInfosByTypeCommandHandler : IRequestHandler<GetCompanyInfosByTypeCommand, PageResult<CompanyInfoDTO>>
    {

        readonly ICapPublisher _capPublisher;
        readonly ILogger<GetCompanyInfosByTypeCommandHandler> _logger;

        readonly IBaseRepository<Company> _companyRepository;
        readonly IBaseRepository<CompanyType> _companyTypeRepository;

        public GetCompanyInfosByTypeCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetCompanyInfosByTypeCommandHandler> logger,
            IBaseRepository<Company> companyRepository,
             IBaseRepository<CompanyType> companyTypeRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;

            _companyRepository = companyRepository;
            _companyTypeRepository = companyTypeRepository;
        }


        public async Task<PageResult<CompanyInfoDTO>> Handle(GetCompanyInfosByTypeCommand request, CancellationToken cancellationToken)
        {


            var companys = _companyRepository.Set().Where(x => x.CompanyTypeId == request.TypeId && x.Show && x.Status == 2);

            var count = await companys.CountAsync();

            if (count < 1) return PageResult<CompanyInfoDTO>.Success(null, 0, request.PageIndex, request.PageSize, "");
            var start = (request.PageIndex - 1) * request.PageSize;
            var end = start + request.PageSize;


            var data = await companys
                .OrderByDescending(x => x.CreatedTime)
                .Skip(start)
                .Take(request.PageSize)
                .ToListAsync();

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
                    //IntroduceImage = x.IntroduceImage,
                    //MainBusiness = x.MainBusiness,
                    //Show = x.Show,
                    //Sort = x.Sort,
                    //UpdatedTime = x.UpdatedTime,
                    CompanyName = x.Name
                };

                _data.Add(dto);

            });

            return PageResult<CompanyInfoDTO>.Success(_data, count, request.PageIndex, request.PageSize, request.LinkUrl);

        }
    }
}
