using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
using Dywq.Domain.CompanyAggregate;
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

        public AddMessageCommandHandler(
            ICapPublisher capPublisher,
            ILogger<AddMessageCommandHandler> logger,
            IBaseRepository<Domain.CompanyAggregate.Message> messageRepository,
            IBaseRepository<Company> companyRepository
            ) : base(capPublisher, logger)
        {
            _messageRepository = messageRepository;
            _companyRepository = companyRepository;
        }

        public override async Task<Result> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {
            var content = string.Empty;
            switch (request.Type)
            {
                case 0: //惠企政策
                    {
                        var item = await _policyArticleRepository.Set().FirstOrDefaultAsync(x => x.Id == request.AssociationId);
                        if (item != null)
                        {
                            return Result.Failure("文章内容不存在");
                        }
                        content = item.ThemeTitle;
                    }
                    break;
                default: return Result.Failure("文章内容不存在");
            }

            var companyIds = string.Join(",", request.CompanyIds);
            var sb = new StringBuilder();

            var sql = $"SELECT CompanyId FROM [Message] where AssociationId = @id and type = {request.Type} and CompanyId in (@CompanyIds)";
            var message_exist = await _messageRepository.SqlQueryAsync<MessageDTO>(sql, new SqlParameter("@id", request.AssociationId), new SqlParameter("@CompanyIds", companyIds));

            var new_companyId_arr = request.CompanyIds.Except(message_exist.Select(x => x.CompanyId));
            foreach (var newId in new_companyId_arr)
            {
                sb.Append($"insert into [Message] (CreatedTime,CompanyId,[Type],IsRead,AssociationId,Content) values (GetDate(),{newId},{request.Type},0,{request.AssociationId},'{content}');");
            }

            var count = await _messageRepository.SqlUpdateAsync(sb.ToString());
            return Result.Success($"执行成功{count}条");

        }
    }
}
