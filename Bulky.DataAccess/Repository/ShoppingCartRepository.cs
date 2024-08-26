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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly AppDbContext Context;
        public ShoppingCartRepository(AppDbContext db) : base(db)
        {
            Context = db;
        }
        public void Update(ShoppingCart NewShoppingCart)
        {
            Context.ShoppingCarts.Update(NewShoppingCart);
        }
    }
}
