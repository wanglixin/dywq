using DotNetCore.CAP;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.UserAggregate;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Guestbook;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Guestbook
{
    public class GetGuestbooksCommand : IRequest<PageResult<GuestbookDTO>>
    {
        public int Id { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string LinkUrl { get; set; }
    }

    public class GetGuestbooksCommandHandler : BaseRequestHandler<GetGuestbooksCommand, PageResult<GuestbookDTO>>
    {
        readonly IBaseRepository<Domain.GuestbookAggregate.Guestbook> _guestbookRepository;
        readonly IBaseRepository<Company> _companyRepository;
        readonly IBaseRepository<CompanyUser> _companyUserRepository;

        public GetGuestbooksCommandHandler(
            ICapPublisher capPublisher,
            ILogger<GetGuestbooksCommandHandler> logger,
            IBaseRepository<Domain.GuestbookAggregate.Guestbook> guestbookRepository,
            IBaseRepository<Company> companyRepository,
            IBaseRepository<CompanyUser> companyUserRepository
            ) : base(capPublisher, logger)
        {
            _companyRepository = companyRepository;
            _guestbookRepository = guestbookRepository;
            _companyUserRepository = companyUserRepository;
        }

        public override async Task<PageResult<GuestbookDTO>> Handle(GetGuestbooksCommand request, CancellationToken cancellationToken)
        {
            var sb = new List<string>
            {
                "Type = 0"
            };

            if (request.Id != 0)
            {
                sb.Add($"Id = {request.Id}");
            }
            var where = string.Join(" and ", sb);

            var pageData = await _guestbookRepository.GetPageDataAsync<Domain.GuestbookAggregate.Guestbook>(
              pageIndex: request.PageIndex,
              pageSize: request.PageSize,
              where: where,
              order: "Id desc"
              );
            var count = pageData.Total;
            if (count < 1)
            {
                return PageResult<GuestbookDTO>.Success(null, count, request.PageIndex, request.PageSize, request.LinkUrl);
            }
            var data = pageData.Data;

            var id_arr = data.Select(x => x.Id);
            var replys = await _guestbookRepository.Set().Select(x => new GuestbookDTO()
            {
                Content = x.Content,
                ReplyId = x.ReplyId,
                Type = x.Type,
                UpdatedTime = x.UpdatedTime,
                UserId = x.UserId,
                CreatedTime = x.CreatedTime
            }).Where(x => id_arr.Contains(x.ReplyId)).ToListAsync();


            var uid_arr = data.Select(x => x.UserId);
            var company_user_arr = await _companyUserRepository.Set().Select(x => new { x.UserId, x.CompanyId }).Where(x => uid_arr.Contains(x.UserId)).ToListAsync();
            var companyid_arr = company_user_arr.Select(x => x.CompanyId);

            var companys = new List<Company>();

            if (companyid_arr.Count() > 0)
            {
                companys = await _companyRepository.Set().Where(x => companyid_arr.Contains(x.Id)).ToListAsync();
            }

            var dtos = new List<GuestbookDTO>();
            foreach (var item in data)
            {
                var dto = new GuestbookDTO()
                {
                    Content = item.Content,
                    CreatedTime = item.CreatedTime,
                    ReplyId = item.ReplyId,
                    Type = item.Type,
                    UpdatedTime = item.UpdatedTime,
                    UserId = item.UserId,
                    GuestbookReply = replys.FirstOrDefault(x => x.ReplyId == item.Id)
                };

                var company_user = company_user_arr.FirstOrDefault(x => x.UserId == item.UserId);
                if (company_user != null)
                {
                    var company = companys.FirstOrDefault(x => x.Id == company_user.CompanyId);
                    dto.CompanyName = company?.Name;
                    dto.CompanyLogo = company?.Logo;
                }

                dtos.Add(dto);
            }

            return PageResult<GuestbookDTO>.Success(dtos, count, request.PageIndex, request.PageSize, request.LinkUrl);

        }
    }
}
