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
            var where = $"CompanyTypeId={request.TypeId} and Show=1 and Status=2";
            var pageData = await _companyRepository.GetPageDataAsync<Company>(
              pageIndex: request.PageIndex,
              pageSize: request.PageSize,
              where: where
              );

            var count = pageData.Total;
            if (count < 1) return PageResult<CompanyInfoDTO>.Success(null, 0, request.PageIndex, request.PageSize, "");

            var data = pageData.Data;
            var _data = new List<CompanyInfoDTO>();

            data.ToList().ForEach(x =>
            {
                var dto = new CompanyInfoDTO
                {
                    CompanyId = x.Id,
                    CompanyName = x.Name
                };

                _data.Add(dto);
            });

            return PageResult<CompanyInfoDTO>.Success(_data, count, request.PageIndex, request.PageSize, request.LinkUrl);

        }
    }
}
