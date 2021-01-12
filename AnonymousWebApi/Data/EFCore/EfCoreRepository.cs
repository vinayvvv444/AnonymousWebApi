using AnonymousWebApi.Data.Contracts;
using AnonymousWebApi.Data.DomainModel.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.EFCore
{
    public abstract class EfCoreRepository<TEntity, TContext> : BaseRepository, IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly TContext context;
        public EfCoreRepository(IConfiguration configuration, TContext context) : base(configuration)
        {
            this.context = context;
        }
        public async Task<TEntity> Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public TEntity AddSync(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Delete(Guid id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
            if (entity == null)
            {
                return entity;
            }

            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return entity;
        }

        public async Task<TEntity> Get(Guid id)
        {
            return await context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
        }

        public async Task<TEntity> Get(int id)
        {
            return await context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await context.Set<TEntity>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public async Task<List<TEntity>> GetAllFromSql(string tableName)
        {
            return await context.Set<TEntity>().FromSqlRaw($"select * from {tableName}").ToListAsync().ConfigureAwait(false);
        }

    }
}
