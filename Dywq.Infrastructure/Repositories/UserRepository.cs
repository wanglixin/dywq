using Dywq.Domain.UserAggregate;
using Dywq.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Infrastructure.Repositories
{
    public class UserRepository : Repository<User, int, DomainContext>, IUserRepository
    {
        public UserRepository(DomainContext context) : base(context)
        {
        }
    }

    public interface IUserRepository : IRepository<User, int>
    { }
}
