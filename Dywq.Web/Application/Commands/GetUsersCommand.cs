using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.UserAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.User;
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
    public class GetUsersCommand : IRequest<PageResult<UserDTO>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string Key { get; set; }
    }

    public class GetUsersCommandHandler : IRequestHandler<GetUsersCommand, PageResult<UserDTO>>
    {

        readonly ICapPublisher _capPublisher;
        readonly ILogger<GetUsersCommandHandler> _logger;
        readonly IBaseRepository<User> _userRepository;
        readonly IBaseRepository<Company> _companyRepository;
        readonly IBaseRepository<CompanyUser> _companyUserRepository;
        readonly IBaseRepository<CompanyFieldData> _companyFieldDataRepository;

        public GetUsersCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetUsersCommandHandler> logger,
            IBaseRepository<User> userRepository,
            IBaseRepository<Company> companyRepository,
             IBaseRepository<CompanyUser> companyUserRepository,
             IBaseRepository<CompanyFieldData> companyFieldDataRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;

            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _companyUserRepository = companyUserRepository;
            _companyFieldDataRepository = companyFieldDataRepository;
        }


        public async Task<PageResult<UserDTO>> Handle(GetUsersCommand request, CancellationToken cancellationToken)
        {

            // Func<User, bool> condition = x => true;
            var where = "";
            var userSet = _userRepository.Set().AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.Key))
            {
                userSet = userSet.Where(x => x.UserName.Contains(request.Key));
                //condition = x => x.UserName.Contains(request.Key);
                where = $"UserName like '%{request.Key}%'";
            }

            //  var count = await userSet.CountAsync();

            //if (count < 1) return PageResult<UserDTO>.Success(null, 0, request.PageIndex, request.PageSize, "");
            //var start = (request.PageIndex - 1) * request.PageSize;
            //var end = start + request.PageSize;


            //var query = userSet.OrderByDescending(x => x.Id)
            //    .Skip(start)
            //    .Take(request.PageSize)
            //    .AsQueryable();

            //var data = await userSet.OrderByDescending(x => x.Id)
            //   .Skip(start)
            //   .Take(request.PageSize)
            //   .ToListAsync();
            //var data = query.ToList();


            var pageData = await _userRepository.GetPageDataAsync<User>(
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                where: where);
            var count = pageData.Total;
            if (count < 1) return PageResult<UserDTO>.Success(null, 0, request.PageIndex, request.PageSize, "");

            //_logger.LogInformation(query.ToSql());
            var data = pageData.Data;

            var ids = data.Select(x => x.Id);

            var companyUserids = await _companyUserRepository.Set().Where(x => ids.Contains(x.UserId)).ToListAsync();

            var user_company_arr = companyUserids.Select(x => new { x.CompanyId, x.UserId });

            var cids = user_company_arr.Select(x => x.CompanyId);

            var companys = await _companyRepository.Set().Where(x => cids.Contains(x.Id)).ToListAsync();

            var _data = new List<UserDTO>();

            data.ToList().ForEach(x =>
            {
                var u = new UserDTO
                {
                    CreatedTime = x.CreatedTime,
                    Id = x.Id,
                    Type = x.Type,
                    UserName = x.UserName,
                    LoginCount = x.LoginCount,
                    Mobile = x.Mobile,
                    LastLoginTime = x.LastLoginTime
                };
                var user_company = user_company_arr.FirstOrDefault(y => y.UserId == x.Id);
                if (user_company == null)
                {
                    u.CompanyName = "未绑定";
                }
                else
                {
                    u.CompanyId = user_company.CompanyId;
                    u.CompanyName = companys.FirstOrDefault(x => x.Id == user_company.CompanyId)?.Name;
                }

                _data.Add(u);

            });

            return PageResult<UserDTO>.Success(_data, count, request.PageIndex, request.PageSize, $"/user/user/list/?PageIndex=__id__&PageSize={request.PageSize}&key={request.Key}");

        }
    }
}
