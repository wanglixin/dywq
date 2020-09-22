using DotNetCore.CAP;
using Dywq.Domain.ArticleAggregate;
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

namespace Dywq.Web.Application.Commands.Expert
{
    public class EditExpertTypeCommand : IRequest<Result>
    {
        [Range(0, int.MaxValue, ErrorMessage = "id错误")]
        public string Id { get; set; }


        [Required(ErrorMessage = "名字不能为空")]
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }


        [RegularExpression("^[0-9]+$", ErrorMessage = "排序值错误")]
        public string Sort { get; set; } = "0";
    }



    public class EditExpertTypeCommandHandler : BaseRequestHandler<EditExpertTypeCommand, Result>
    {
        IBaseRepository<Dywq.Domain.Expert.ExpertType> _expertTypeRepository;

        public EditExpertTypeCommandHandler(
            ICapPublisher capPublisher,
            ILogger<EditExpertTypeCommandHandler> logger,
             IBaseRepository<Dywq.Domain.Expert.ExpertType> expertTypeRepository
            ) : base(capPublisher, logger)
        {
            _expertTypeRepository = expertTypeRepository;
        }

        public override async Task<Result> Handle(EditExpertTypeCommand request, CancellationToken cancellationToken)
        {
            var id = string.IsNullOrWhiteSpace(request.Id) ? 0 : Convert.ToInt32(request.Id);

            var sort = Convert.ToInt32(request.Sort);

            if (id <= 0) //新增
            {
                var item = new Domain.Expert.ExpertType()
                {
                    Sort = sort,
                    Name = request.Name
                };
                await _expertTypeRepository.AddAsync(item);
            }
            else
            {
                //修改
                var item = await _expertTypeRepository.Set().FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return Result.Failure($"id={request.Id}内容不存在");
                }

                item.Sort = sort;
                item.Name = request.Name;
                await _expertTypeRepository.UpdateAsync(item);
            }

            return Result.Success();

        }
    }
}
