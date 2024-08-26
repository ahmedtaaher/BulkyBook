using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Services;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly AppDbContext Context;
        public CategoryRepository(AppDbContext db) : base(db)
        {
            Context = db;
        }
        public void Update(Category NewCategory)
        {
            Context.Categories.Update(NewCategory);
        }
    }
}
