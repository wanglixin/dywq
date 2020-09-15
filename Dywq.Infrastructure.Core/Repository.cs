using Microsoft.EntityFrameworkCore.Internal;
using Dywq.Domain;
using Dywq.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dywq.Infrastructure.Core
{
    public abstract class Repository<TEntity, TDbContext> : IRepository<TEntity> where TEntity : Entity, IAggregateRoot where TDbContext : EFContext
    {
        protected virtual TDbContext DbContext { get; set; }

        public Repository(TDbContext context)
        {

            this.DbContext = context;
        }
        public virtual IUnitOfWork UnitOfWork => DbContext;

        public virtual TEntity Add(TEntity entity)
        {
            return DbContext.Add(entity).Entity;
        }

        public virtual Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Add(entity));
        }

        public virtual TEntity Update(TEntity entity)
        {
            entity.UpdatedTime = DateTime.Now;
            return DbContext.Update(entity).Entity;
        }

        public virtual Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Update(entity));
        }

        public virtual bool Remove(Entity entity)
        {
            DbContext.Remove(entity);
            return true;
        }

        public virtual Task<bool> RemoveAsync(Entity entity)
        {
            return Task.FromResult(Remove(entity));
        }

        public virtual Task<bool> AnyAsync(Func<TEntity, bool> predicate)
        {
            return Task.FromResult(DbContext.Set<TEntity>().Any(predicate));
        }

        public virtual bool Any(Func<TEntity, bool> predicate)
        {
            return DbContext.Set<TEntity>().Any(predicate);
        }

        public virtual Task<bool> AnyAsync()
        {
            return Task.FromResult(DbContext.Set<TEntity>().Any());
        }

        public virtual bool Any()
        {
            return DbContext.Set<TEntity>().Any();
        }

        public virtual Task<TEntity> GetAsync(Func<TEntity, bool> predicate)
        {
            return Task.FromResult(DbContext.Set<TEntity>().Where(predicate).FirstOrDefault());
        }

        public virtual TEntity Get(Func<TEntity, bool> predicate)
        {
            return DbContext.Set<TEntity>().Where(predicate).FirstOrDefault();
        }

        public virtual DbSet<TEntity> Set()
        {
            return DbContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> BatchAddAsync(IEnumerable<TEntity> entities)
        {
            var ret = new List<TEntity>();
            foreach (var x in entities)
            {
                var _x = await AddAsync(x);
                ret.Add(_x);
            }
            return ret;
        }

        public Task<int> CountAsync(Func<TEntity, bool> predicate)
        {
            return DbContext.Set<TEntity>().CountAsync(x => predicate(x));
        }

        public Task<int> CountAsync()
        {
            return DbContext.Set<TEntity>().CountAsync();
        }

        //public async Task<IEnumerable<T>> GetListAsync<T>(string sql, params object[] parameters) where T : class, new()
        //{
        //    return await DbContext.Database.SqlQueryAsync<T>(sql, parameters);
        //}



        public async Task<IEnumerable<T>> SqlQueryAsync<T>(string sql, params object[] parameters) where T : class, new()
        {
            return await DbContext.Database.SqlQueryAsync<T>(sql, parameters);
        }

        public async Task<int> SqlCountAsync(string sql, params object[] parameters)
        {
            return await DbContext.Database.SqlCountAsync(sql, parameters);
        }

        public async Task<PageData<T>> GetPageDataAsync<T>(string fields, string where, int pageIndex = 1, int pageSize = 10, string order = "id desc") where T : class, new()
        {
            return await DbContext.Database.GetPageData<T>(fields, where, pageIndex, pageSize, order);
        }
    }


    public abstract class Repository<TEntity, TKey, TDbContext> : Repository<TEntity, TDbContext>, IRepository<TEntity, TKey> where TEntity : Entity<TKey>, IAggregateRoot where TDbContext : EFContext
    {
        public Repository(TDbContext context) : base(context)
        {
        }

        public virtual bool Delete(TKey id)
        {
            var entity = DbContext.Find<TEntity>(id);
            if (entity == null)
            {
                return false;
            }
            DbContext.Remove(entity);
            return true;
        }

        public virtual async Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await DbContext.FindAsync<TEntity>(id, cancellationToken);
            if (entity == null)
            {
                return false;
            }
            DbContext.Remove(entity);
            return true;
        }

        public virtual TEntity Get(TKey id)
        {
            return DbContext.Find<TEntity>(id);
        }

        public virtual async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await DbContext.FindAsync<TEntity>(id, cancellationToken);
        }
    }

}
