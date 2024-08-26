using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext Context;
        public ICategoryRepository category { get; private set; }
        public IProductRepository product { get; private set; }
        public ICompanyRepository company { get; private set; }
        public IShoppingCartRepository shoppingcart { get; private set; }
        public IApplicationUserRepository applicationuser { get; private set; }
        public IOrderHeaderRepository orderheader { get; }
        public IOrderDetailRepository orderdetail { get; }

        public UnitOfWork(AppDbContext db)
        {
            Context = db;
            category = new CategoryRepository(Context);
            product = new ProductRepository(Context);
            company = new CompanyRepository(Context);
            shoppingcart = new ShoppingCartRepository(Context);
            applicationuser = new ApplicationUserRepository(Context);
            orderheader = new OrderHeaderRepository(Context);
            orderdetail = new OrderDetailRepository(Context);
        }
        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
