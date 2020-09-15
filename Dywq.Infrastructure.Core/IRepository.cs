using Dywq.Domain;
using Dywq.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dywq.Infrastructure.Core
{
    public interface IRepository<TEntity> where TEntity : Entity, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        TEntity Update(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        bool Remove(Entity entity);
        Task<bool> RemoveAsync(Entity entity);

        Task<bool> AnyAsync(Func<TEntity, bool> predicate);
        bool Any(Func<TEntity, bool> predicate);

        Task<bool> AnyAsync();
        bool Any();
        Task<TEntity> GetAsync(Func<TEntity, bool> predicate);
        TEntity Get(Func<TEntity, bool> predicate);

        DbSet<TEntity> Set();


        Task<IEnumerable<TEntity>> BatchAddAsync(IEnumerable<TEntity> entities);

        Task<int> CountAsync(Func<TEntity, bool> predicate);
        Task<int> CountAsync();

        // Task<bool> ExecuteAsync(string sql);

        Task<IEnumerable<T>> SqlQueryAsync<T>(string sql, params object[] parameters) where T : class, new();

        Task<int> SqlCountAsync(string sql, params object[] parameters);


        Task<PageData<T>> GetPageDataAsync<T>(string fields = "", string where = "", int pageIndex = 1, int pageSize = 10, string order = "id desc") where T : class, new();

    }


    public interface IRepository<TEntity, TKey> : IRepository<TEntity> where TEntity : Entity<TKey>, IAggregateRoot
    {
        bool Delete(TKey id);
        Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default);
        TEntity Get(TKey id);
        Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);
    }
}
