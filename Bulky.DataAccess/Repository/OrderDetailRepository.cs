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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly AppDbContext Context;
        public OrderDetailRepository(AppDbContext db) : base(db)
        {
            Context = db;
        }
        public void Update(OrderDetail NewOrderDetail)
        {
            Context.OrderDetails.Update(NewOrderDetail);
        }
    }
}
