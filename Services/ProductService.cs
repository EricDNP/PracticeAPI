using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PracticeAPI.Models;
using PracticeAPI.Repositories;

namespace PracticeAPI.Services
{
    public class ProductService : iService<Product>
    {
        private readonly iUnitOfWork _unitOfWork;

        public ProductService(iUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        public Task<ICollection<Product>> GetAll()
        {
            return _unitOfWork.ProductRepo.GetAll();
        }

        public List<Product> GetAllWithData(
            Expression<Func<Product, bool>> filter = null,
            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null
        )
        {
            return _unitOfWork.ProductRepo.GetAllWithData(filter, orderBy).ToList();
        }

        public Task<Product> GetByID(Guid Id)
        {
            return _unitOfWork.ProductRepo.GetByID(Id);
        }

        public async Task<Product> Create(Product entity)
        {
            await _unitOfWork.ProductRepo.Create(entity);
            await _unitOfWork.CommitChangesAsync();
            return entity;
        }

        public async Task<Product> Update(Product entity)
        {
            if(await _unitOfWork.ProductRepo.Exist(entity.Id))
                _unitOfWork.ProductRepo.Update(entity);
            else
                return null;                
            return await _unitOfWork.ProductRepo.GetByID(entity.Id);
        }

        public async Task Delete(Guid Id)
        {
            await _unitOfWork.ProductRepo.Delete(Id);
            await _unitOfWork.CommitChangesAsync();
        }

        public async Task<bool> Exist(Guid id)
        {
            return await _unitOfWork.ProductRepo.Exist(id);
        }

        public Task UpdateAsChilds<TEntity>(iEntity entity, Expression<Func<TEntity, object>> entities) where TEntity : class, iEntity
        {
            throw new NotImplementedException();
        }
    }
}