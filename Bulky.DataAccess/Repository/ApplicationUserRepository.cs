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
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly AppDbContext Context;
        public ApplicationUserRepository(AppDbContext db) : base(db)
        {
            Context = db;
        }

    }
}
