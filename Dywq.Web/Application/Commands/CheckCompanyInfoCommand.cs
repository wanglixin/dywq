using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands
{
    /// <summary>
    /// 管理员审核企业动态
    /// </summary>
    public class CheckCompanyInfoCommand : IRequest<Result>
    {

        [Range(1, int.MaxValue, ErrorMessage = "企业id参数错误")]
        /// <summary>
        /// 企业id
        /// </summary>
        public int CompanyId { get; set; }

        [Range(-1, 2, ErrorMessage = "审核状态错误")]
        /// <summary>
        /// 2:通过 -1：失败
        /// </summary>
        public int Status { get; set; }

    }

    public class CheckCompanyInfoCommandHandler : IRequestHandler<CheckCompanyInfoCommand, Result>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<CheckCompanyInfoCommandHandler> _logger;

        readonly IBaseRepository<Company> _companyRepository;

        public CheckCompanyInfoCommandHandler(
            ICapPublisher capPublisher,
            ILogger<CheckCompanyInfoCommandHandler> logger,
            IBaseRepository<Company> companyRepository)
        {
            _capPublisher = capPublisher;
            _logger = logger;
            _companyRepository = companyRepository;
        }

        public async Task<Result> Handle(CheckCompanyInfoCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.Set().FirstOrDefaultAsync(x => x.Id == request.CompanyId);
            if (company == null)
            {
                return Result.Failure("企业不存在");
            }

            if (company.Status != 1)
            {
                return Result.Failure("当前状态不能审核 ");
            }
            if (request.Status != -1 && request.Status != 2)
            {
                return Result.Failure("审核状态错误 ");
            }


            company.Status = request.Status;
            await _companyRepository.UpdateAsync(company);

            return Result.Success();

        }
    }
}
