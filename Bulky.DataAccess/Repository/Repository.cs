using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext Context;
        internal DbSet<T> dbset;
        public Repository(AppDbContext db)
        {
            Context = db;
            dbset = Context.Set<T>();
            Context.Products.Include(c => c.Category).Include(d => d.CategoryId);
        }
        public void Create(T entity)
        {
            dbset.Add(entity);
        }
        public void Delete(T entity)
        {
            dbset.Remove(entity);
        }
        public void DeleteRange(IEnumerable<T> entity)
        {
            dbset.RemoveRange(entity);
        }
        public T Get(Expression<Func<T, bool>> filter, string? includeproperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbset;

            }
            else
            {
                query = dbset.AsNoTracking();
            }
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeproperties))
            {
                foreach (var includeProp in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeproperties = null)
        {
            IQueryable<T> query = dbset;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            if(!string.IsNullOrEmpty(includeproperties))
            {
                foreach(var includeprop in includeproperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                }
            }
            return query.ToList();
        }
    }
}
