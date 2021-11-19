using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using PracticeAPI.Data;
using PracticeAPI.Models;

namespace PracticeAPI.Repositories
{
    public class GenericRepository<TEntity> : iRepository<TEntity> where TEntity : class, iEntity
    {
        internal ApiContext _context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(ApiContext context)
        {
            this._context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public async Task<ICollection<TEntity>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public virtual IEnumerable<TEntity> GetAllWithData(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (includeProperties != null)
                query = includeProperties(query);

            if (orderBy != null)
                return orderBy(query).ToList();
            else
                return query.ToList();
        }

        public async Task<TEntity> GetByID(Guid id,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (includeProperties != null)
                query = includeProperties(query);

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Create(TEntity entity)
        {
            await _context.AddAsync<TEntity>(entity);
        }

        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task UpdateWithChilds<T>(T entity, params Expression<Func<T, object>>[] navigations) where T : class, iEntity
        {
            var dbEntity = await _context.FindAsync<T>(entity.Id);

            var dbEntry = _context.Entry(dbEntity);
            dbEntry.CurrentValues.SetValues(entity);

            foreach (var property in navigations)
            {
                var propertyName = property.GetPropertyAccess().Name;
                var dbItemsEntry = dbEntry.Collection(propertyName);
                var accessor = dbItemsEntry.Metadata.GetCollectionAccessor();

                await dbItemsEntry.LoadAsync();
                var dbItemsMap = (dbItemsEntry.CurrentValue as IEnumerable<iEntity>)
                    .ToDictionary(e => e.Id);

                var items = accessor.GetOrCreate(entity, true) as IEnumerable<iEntity>;

                foreach (var item in items)
                {
                    if (!dbItemsMap.TryGetValue(item.Id, out var oldItem))
                        accessor.Add(dbEntity, item, true);
                    else
                    {
                        _context.Entry(oldItem).CurrentValues.SetValues(item);
                        dbItemsMap.Remove(item.Id);
                    }
                }

                foreach (var oldItem in dbItemsMap.Values)
                    _context.Remove(oldItem);
            }
        }

        public async Task Delete(Guid id)
        {
            var entity = await dbSet.FindAsync(id);
            Remove(entity);
        }

        public void Remove(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);
            dbSet.Remove(entity);
        }

        public async Task<bool> Exist(Guid id)
        {
            return await dbSet.AnyAsync(e => e.Id == id);
        }
    }
}