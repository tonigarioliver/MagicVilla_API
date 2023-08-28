using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections;
using System.Linq.Expressions;

namespace MagicVilla_VillaApi.Core.IRepository
{
    public interface IGeneric<T>where T : class
    {
        public Task<IList> GetAllAsync(Expression<Func<T, bool>>? filter = null,bool tracked=true);
        public Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true);
        public Task<T> CreateAsync(T entity);
        public Task<T> Update(T entity);
        public Task<bool> Delete(T entity);
    }
}
