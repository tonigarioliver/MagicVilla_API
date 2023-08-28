using MagicVilla_VillaApi.Core.IRepository;
using MagicVilla_VillaApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace MagicVilla_VillaApi.Core.Repository
{
    public class Generic<T>:IGeneric<T> where T : class
    {
        protected AppDbContext _appDbContext;
        protected DbSet<T> _dbSet;
        protected ILogger _logger;

        public Generic(AppDbContext appDbContext,ILogger logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _dbSet = _appDbContext.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                return entity;
            }catch (Exception ex)
            {
                _logger.LogError(ex, "Error Adding entity", entity);
                return null;
            }
        }

        public async Task<bool> Delete(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error Removing entity", entity);
                return false;
            }
        }

        public async Task<IList> GetAllAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> query =_dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> query = _dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                return entity;
            }catch (Exception ex)
            {
                _logger.LogError(ex, "Error Updating entity", entity);
                return null;
            }
        }
    }
}
