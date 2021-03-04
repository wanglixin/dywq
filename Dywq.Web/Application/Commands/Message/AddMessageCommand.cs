using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.News;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.Commpany;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Web.Application.Commands.Message
{
    public class AddMessageCommand : IRequest<Result>
    {
        public IEnumerable<int> CompanyIds { get; set; }
        public int AssociationId { get; set; }

        /// <summary>
        ///  0 惠企政策 
        /// </summary>
        public int Type { get; set; } = 0;
    }

    public class AddMessageCommandHandler : BaseRequestHandler<AddMessageCommand, Result>
    {

        readonly IBaseRepository<Domain.CompanyAggregate.Message> _messageRepository;
        readonly IBaseRepository<Company> _companyRepository;
        readonly IBaseRepository<PolicyArticle> _policyArticleRepository;
        readonly IBaseRepository<NoticeNews> _noticeNewsRepository;

        public AddMessageCommandHandler(
            ICapPublisher capPublisher,
            ILogger<AddMessageCommandHandler> logger,
            IBaseRepository<Domain.CompanyAggregate.Message> messageRepository,
            IBaseRepository<Company> companyRepository,
            IBaseRepository<PolicyArticle> policyArticleRepository,
            IBaseRepository<NoticeNews> noticeNewsRepository
            ) : base(capPublisher, logger)
        {
            _messageRepository = messageRepository;
            _companyRepository = companyRepository;
            _policyArticleRepository = policyArticleRepository;
            _noticeNewsRepository = noticeNewsRepository;
        }

        public override async Task<Result> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {
            if (request.CompanyIds.Count() < 1)
            {
                return Result.Failure("请至少选择一个公司");
            }
            var content = string.Empty;
            switch (request.Type)
            {
                case 0: //惠企政策
                    {
                        var item = await _policyArticleRepository.Set().FirstOrDefaultAsync(x => x.Id == request.AssociationId);
                        if (item == null)
                        {
                            return Result.Failure("文章内容不存在");
                        }
                        content = item.ThemeTitle;
                    }
                    break;
                case 1: //通知
                    {
                        var item = await _noticeNewsRepository.Set().FirstOrDefaultAsync(x => x.Id == request.AssociationId);
                        if (item == null)
                        {
                            return Result.Failure("文章内容不存在");
                        }
                        content = item.Title;
                    }
                    break;

                default: return Result.Failure("文章内容不存在");
            }

            var companyIds = string.Join(",", request.CompanyIds);
            var sb = new StringBuilder();

            var sql = $"SELECT CompanyId FROM [Message] where AssociationId = {request.AssociationId} and Type = {request.Type} and CompanyId in ({companyIds})";
            var message_exist = await _messageRepository.SqlQueryAsync<MessageDTO>(sql);

            var new_companyId_arr = request.CompanyIds.Except(message_exist.Select(x => x.CompanyId));

            if (new_companyId_arr.Count() < 1)
            {
                return Result.Success();
            }

            foreach (var newId in new_companyId_arr)
            {
                sb.Append($"insert into [Message] (CreatedTime,CompanyId,[Type],IsRead,AssociationId,Content) values (GetDate(),{newId},{request.Type},0,{request.AssociationId},'{content}');");
            }

            var count = await _messageRepository.SqlUpdateAsync(sb.ToString());
            return Result.Success($"执行成功{count}条");

        }
    }
}
