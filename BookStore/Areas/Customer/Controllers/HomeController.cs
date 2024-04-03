using BookStore.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;

namespace BookStore.Areas.Customer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(productList);
        }

        public IActionResult Details(int? productId)
        {
            Product product = _unitOfWork.Product.Get(p => p.Id == productId, includeProperties: "Category");
            return View(product);
        }
    }
}
