using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using PracticeAPI.Models;
using PracticeAPI.Repositories;

namespace PracticeAPI.Services
{
    public interface iService<T> where T : class
    {
        List<T> GetAllWithData(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
        );
        Task<ICollection<T>> GetAll();
        Task<T> GetByID(Guid Id);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task UpdateAsChilds<TEntity>(iEntity entity, Expression<Func<TEntity, object>> entities) where TEntity : class, iEntity;
        Task Delete(Guid Id);
    }
}