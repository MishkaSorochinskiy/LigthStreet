using Domain.Models;
using Domain.Root;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
        where TKey : struct
    {
        public DbContext databaseContext { get; }

        public Repository(DbContext context)
        {
            this.databaseContext = context;
        }

        public virtual Task<List<TEntity>> ToListAsync()
        {
            return databaseContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            var newEntity = await databaseContext.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            databaseContext.Set<TEntity>().Remove(entity);
        }

        public virtual async Task<TEntity> FindByIdAsync(TKey Id)
        {
           return await databaseContext.Set<TEntity>().FindAsync(Id);
        }

        public void Update(TEntity entity)
        {
            databaseContext.Set<TEntity>().Update(entity);
        }

    }
}
