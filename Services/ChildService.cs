using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PracticeAPI.Models;
using PracticeAPI.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace PracticeAPI.Services
{
    public class ChildService<TEntity> : iService<TEntity> where TEntity : class, iEntity
    {
        private readonly iUnitOfWork _unitOfWork;
        private readonly GenericRepository<TEntity> repo;
        private readonly UtilityRepository utilityRepo;

        public ChildService(iUnitOfWork unitOfWork, UtilityRepository UtilityRepository)
        {
            _unitOfWork = unitOfWork;
            utilityRepo = UtilityRepository;

            if (typeof(TEntity) == typeof(ChildA))
                repo = _unitOfWork.ChildARepo as GenericRepository<TEntity>;
            else if (typeof(TEntity) == typeof(ChildB))
                repo = _unitOfWork.ChildBRepo as GenericRepository<TEntity>;
        }

        public Task<ICollection<TEntity>> GetAll()
        {
            return repo.GetAll();
        }

        public List<TEntity> GetAllWithData(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            return repo.GetAllWithData(filter, orderBy).ToList();
        }

        public Task<TEntity> GetByID(Guid Id)
        {
            return repo.GetByID(Id);
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            await repo.Create(entity);
            await _unitOfWork.CommitChangesAsync();
            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            if (await repo.Exist(entity.Id))
            {
                if (typeof(TEntity) == typeof(ChildA))
                {
                    Expression<Func<ParentA, object>>[] includes = {e => e.Childs};
                    await repo.UpdateWithChilds<ChildA>(EntityConverter.ConvertEntity<ChildA>(entity), c => c.Things);
                }
                else if (typeof(TEntity) == typeof(ChildB))
                    await repo.UpdateWithChilds<ChildB>(EntityConverter.ConvertEntity<ChildB>(entity), c => c.Things);
                await _unitOfWork.CommitChangesAsync();
            }
            else
                return null;
            return await repo.GetByID(entity.Id);
        }

        public async Task Delete(Guid Id)
        {
            await repo.Delete(Id);
            await _unitOfWork.CommitChangesAsync();
        }

        public async Task<bool> Exist(Guid id)
        {
            return await repo.Exist(id);
        }

        public async Task UpdateAsChilds<T>(iEntity parent, Expression<Func<T, object>> entities) where T : class, iEntity
        {
            iEntity existingParent = await utilityRepo.GetTracked<T>(parent.Id);
            var dbEntry = utilityRepo.GetEntryAsync(existingParent);
            var propertyName = entities.GetPropertyAccess().Name;
            var dbItemsEntry = dbEntry.Collection(propertyName);
            var accessor = dbItemsEntry.Metadata.GetCollectionAccessor();

            await dbItemsEntry.LoadAsync();
            var dbItemsMap = (dbItemsEntry.CurrentValue as IEnumerable<iEntity>)
                .ToDictionary(e => e.Id);

            var items = accessor.GetOrCreate(parent, true) as IEnumerable<iEntity>;

            foreach (TEntity item in items)
            {
                if (!dbItemsMap.TryGetValue(item.Id, out var oldItem))
                    accessor.Add(existingParent, item, true);
                else
                {
                    await Update(item);
                    dbItemsMap.Remove(item.Id);
                }
            }

            foreach (var oldItem in dbItemsMap.Values)
                await Delete(oldItem.Id);
        }
    }
}