using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PracticeAPI.Models;
using PracticeAPI.Repositories;

namespace PracticeAPI.Services
{
    public class ClientService<TEntity> : iService<TEntity> where TEntity : Client, iEntity
    {
        private readonly iUnitOfWork _unitOfWork;
        private readonly GenericRepository<TEntity> repo;

        public ClientService(iUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;

            if(typeof(TEntity) == typeof(PublicClient))
                repo = _unitOfWork.PublicClientRepo as GenericRepository<TEntity>;
            else if(typeof(TEntity) == typeof(PrivateClient))
                repo = _unitOfWork.PrivateClientRepo as GenericRepository<TEntity>;
        }

        public Task<ICollection<TEntity>> GetAll()
        {
            return repo.GetAll();
        }

        public List<TEntity> GetAllWithData(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy
        )
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
            if(await repo.Exist(entity.Id))
                repo.Update(entity);
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

        public Task UpdateAsChilds<T>(iEntity entity, Expression<Func<T, object>> entities) where T : class, iEntity
        {
            throw new NotImplementedException();
        }
    }
}