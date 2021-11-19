using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PracticeAPI.Models;
using PracticeAPI.Repositories;

namespace PracticeAPI.Services
{
    public class ParentService<TEntity> : iService<TEntity> where TEntity : class, iParent, iEntity
    {
        public readonly iUnitOfWork _unitOfWork;
        private readonly GenericRepository<TEntity> repo;
        private readonly iService<ChildA> childAService;
        private readonly iService<ChildB> childBService;

        public ParentService(iUnitOfWork UnitOfWork, iService<ChildA> ChildAService, iService<ChildB> ChildBService)
        {
            _unitOfWork = UnitOfWork;
            childAService = ChildAService;
            childBService = ChildBService;

            if (typeof(TEntity) == typeof(ParentA))
                repo = _unitOfWork.ParentARepo as GenericRepository<TEntity>;
            else if (typeof(TEntity) == typeof(ParentB))
                repo = _unitOfWork.ParentBRepo as GenericRepository<TEntity>;
        }

        public Task<ICollection<TEntity>> GetAll()
        {
            return repo.GetAll();
        }

        public List<TEntity> GetAllWithData(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            return repo.GetAllWithData(filter, orderBy,
                includeProperties: p => p.Include(p => p.Childs).ThenInclude(c => c.Things))
                .ToList();
        }

        public Task<TEntity> GetByID(Guid Id)
        {
            return repo.GetByID(Id,
                includeProperties: p => p.Include(p => p.Childs).ThenInclude(c => c.Things));
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
                repo.Update(entity);
                if (typeof(TEntity) == typeof(ParentA))
                    await childAService.UpdateAsChilds<ParentA>(entity, e => e.Childs);
                else if (typeof(TEntity) == typeof(ParentB))
                    await childBService.UpdateAsChilds<ParentB>(entity, e => e.Childs);
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

        public Task UpdateAsChilds<TEntity>(iEntity entity, Expression<Func<TEntity, object>> entities) where TEntity : class, iEntity
        {
            throw new NotImplementedException();
        }
    }
}