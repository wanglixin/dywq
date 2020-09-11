using DotNetCore.CAP;
using Dywq.Domain.UserAggregate;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Dto.User;
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
    public class GetUserCommand : IRequest<UserDTO>
    {
        [Range(1, int.MaxValue, ErrorMessage = "用户id错误")]
        public int Id { get; set; }
    }



    public class GetUserCommandHandler : IRequestHandler<GetUserCommand, UserDTO>
    {
        readonly ICapPublisher _capPublisher;
        readonly ILogger<GetUserCommandHandler> _logger;
        readonly IBaseRepository<User> _userRepository;


        public GetUserCommandHandler(
             ICapPublisher capPublisher,
            ILogger<GetUserCommandHandler> logger,
            IBaseRepository<User> userRepository
            )
        {
            _capPublisher = capPublisher;
            _logger = logger;
            _userRepository = userRepository;



        }

        public async Task<UserDTO> Handle(GetUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Set().FirstOrDefaultAsync(x => x.Id == request.Id);
            if (user == null) return null;
            return new UserDTO()
            {
                Id = user.Id,
                CreatedTime = user.CreatedTime,
                UserName = user.UserName,
                Type = user.Type
            };


        }
    }
}
