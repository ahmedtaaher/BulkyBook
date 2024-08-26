using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> ProductModel = unitOfWork.product.GetAll(includeproperties:"Category").ToList();
            return View(ProductModel);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM ProductVM = new()
            {
                CategoryList = unitOfWork.category.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                return View(ProductVM);
            }
            else
            {
                ProductVM.Product = unitOfWork.product.Get(z => z.Id == id);
                return View(ProductVM);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwrootpath = webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productpath = Path.Combine(wwwrootpath, @"images\product");
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var olditempath = Path.Combine(wwwrootpath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(olditempath))
                        {
                            System.IO.File.Delete(olditempath);
                        }
                    }
                    using (var filestream = new FileStream(Path.Combine(productpath, filename), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + filename;
                }
                if (productVM.Product.Id == 0)
                {
                    unitOfWork.product.Create(productVM.Product);
                }
                else
                {
                    unitOfWork.product.Update(productVM.Product);
                }
                unitOfWork.Save();
                TempData["success"] = "Product Created/Updated Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = unitOfWork.category.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
                return View(productVM);
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> ProductModel = unitOfWork.product.GetAll().ToList();
            return Json(new { data = ProductModel });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var producttobedeleted = unitOfWork.product.Get(z => z.Id == id);
            if (producttobedeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var olditempath = Path.Combine(webHostEnvironment.WebRootPath, producttobedeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(olditempath))
            {
                System.IO.File.Delete(olditempath);
            }
            unitOfWork.product.Delete(producttobedeleted);
            unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion

    }
}
