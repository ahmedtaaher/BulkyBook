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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly AppDbContext Context;
        public CompanyRepository(AppDbContext db) : base(db)
        {
            Context = db;
        }
        public void Update(Company NewCompany)
        {
            Context.Companies.Update(NewCompany);
        }
    }
}
