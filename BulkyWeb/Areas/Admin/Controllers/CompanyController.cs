using Bulky.DataAccess.Repository;
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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Company> CompanyModel = unitOfWork.company.GetAll().ToList();
            return View(CompanyModel);
        }
        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                return View(new Company());
            }
            else
            {
                Company company = unitOfWork.company.Get(z => z.Id == id);
                return View(company);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {

                if (CompanyObj.Id == 0)
                {
                    unitOfWork.company.Create(CompanyObj);
                }
                else
                {
                    unitOfWork.company.Update(CompanyObj);
                }

                unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {

                return View(CompanyObj);
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> CompanyModel = unitOfWork.company.GetAll().ToList();
            return Json(new { data = CompanyModel });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = unitOfWork.company.Get(u => u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            unitOfWork.company.Delete(CompanyToBeDeleted);
            unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion

    }
}
