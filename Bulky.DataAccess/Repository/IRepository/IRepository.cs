using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeproperties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeproperties = null, bool tracked = false);
        void Create(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entity);
    }
}
