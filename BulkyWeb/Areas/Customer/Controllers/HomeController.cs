using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart,
                    unitOfWork.shoppingcart.GetAll(u => u.ApplicationUserId == claim.Value).Count());
            }

            IEnumerable<Product> productlist = unitOfWork.product.GetAll(includeproperties:"Category");
            return View(productlist);
        }
        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = unitOfWork.product.Get(x => x.Id == productId, includeproperties: "Category"),
                Count = 1,
                ProductId = productId
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingcart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingcart.ApplicationUserId = userId;
            ShoppingCart cartFromDb = unitOfWork.shoppingcart.Get(u => u.ApplicationUserId == userId &&
            u.ProductId == shoppingcart.ProductId);

            if (cartFromDb != null)
            {
                cartFromDb.Count += shoppingcart.Count;
                unitOfWork.shoppingcart.Update(cartFromDb);
                unitOfWork.Save();
            }
            else
            {
                //add cart record
                unitOfWork.shoppingcart.Create(shoppingcart);
                unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                unitOfWork.shoppingcart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            TempData["success"] = "Cart updated successfully";

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
