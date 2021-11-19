using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using PracticeAPI.Models;

namespace PracticeAPI.Repositories
{
    public interface iRepository<T> where T : iEntity
    {
        Task<ICollection<T>> GetAll();
        IEnumerable<T> GetAllWithData(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties);
        Task<T> GetByID(Guid id, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties);
        Task Create(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<bool> Exist(Guid id);
    }
}