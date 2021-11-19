using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PracticeAPI.Data;
using PracticeAPI.Models;

namespace PracticeAPI.Repositories
{
    public class UtilityRepository
    {
        internal ApiContext _context;

        public UtilityRepository(ApiContext context)
        {
            this._context = context;
        }

        public EntityEntry GetEntryAsync(iEntity entity){
            return _context.Entry(entity);
        }

        public async Task<T> GetTracked<T>(Guid id) where T : class, iEntity
        {
            return await _context.FindAsync<T>(id);
        }
    }
}