using Dywq.Domain.Abstractions;
using Dywq.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : Repository<TEntity, int, DomainContext>, IBaseRepository<TEntity> where TEntity : Entity<int>, IAggregateRoot
    {
        public BaseRepository(DomainContext context) : base(context)
        {
        }
    }

    public interface IBaseRepository<TEntity> : IRepository<TEntity, int> where TEntity : Entity<int>, IAggregateRoot
    { }
}
