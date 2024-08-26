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
    public class ProductRepository : Repository<Product> ,IProductRepository
    {
        private readonly AppDbContext Context;
        public ProductRepository(AppDbContext db) : base(db)
        {
            Context = db;
        }

        public void Update(Product newproduct)
        {
            Product oldproduct = Context.Products.FirstOrDefault(c => c.Id == newproduct.Id);
            if (oldproduct != null)
            {
                oldproduct.Title = newproduct.Title;
                oldproduct.Description = newproduct.Description;
                oldproduct.Author = newproduct.Author;
                oldproduct.ISBN = newproduct.ISBN;
                oldproduct.ListPrice = newproduct.ListPrice;
                oldproduct.Price = newproduct.Price;
                oldproduct.Price50 = newproduct.Price50;
                oldproduct.Price100 = newproduct.Price100;
                oldproduct.CategoryId = newproduct.CategoryId;
                if (newproduct.ImageUrl != null)
                {
                    oldproduct.ImageUrl = newproduct.ImageUrl;
                }
            }
        }
    }
}
