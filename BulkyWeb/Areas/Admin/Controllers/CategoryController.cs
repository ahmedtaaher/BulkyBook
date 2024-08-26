using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> CategoryModel = unitOfWork.category.GetAll().ToList();
            return View(CategoryModel);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category Newcategory)
        {
            if (Newcategory.Name == Newcategory.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The displayorder can't exactly match the name");
            }
            if (ModelState.IsValid)
            {
                unitOfWork.category.Create(Newcategory);
                unitOfWork.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View(Newcategory);
        }
        public IActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category categoryFound = unitOfWork.category.Get(c => c.Id == id);
            if (categoryFound == null)
            {
                return NotFound();
            }
            return View(categoryFound);
        }
        [HttpPost]
        public IActionResult Edit(Category Newcategory)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.category.Update(Newcategory);
                unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(Newcategory);
        }
        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category categoryFound = unitOfWork.category.Get(z => z.Id == id);
            if (categoryFound == null)
            {
                return NotFound();
            }
            return View(categoryFound);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            Category Found = unitOfWork.category.Get(z => z.Id == id);
            if (Found == null)
            {
                return NotFound();
            }
            unitOfWork.category.Delete(Found);
            unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
