using BookStore.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models.Models;
using BookStore.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore.Models.ViewModels;

namespace BookStore.Areas.Admin.Controllers;

public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;


    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Index()
    {
        List<Product> productList = _unitOfWork.Product.GetAll().ToList();
        return View(productList);
    }

    public IActionResult Upsert(int? productId)
    {
        IEnumerable<SelectListItem> categoryList=_unitOfWork.Category.GetAll().Select(c=>new SelectListItem
        {
            Text=c.Name,
            Value=c.Id.ToString(),
        });
        //ViewBag.CategoryList = categoryList;
        //ViewData["CategoryList"]=categoryList;
        ProductViewModel productViewModel = new ProductViewModel()
        {
            CategoryList = categoryList,
            Product = new Product()
        };

        if(productId == null || productId==0)
        {
            //Create
            return View(productViewModel);
        }
        else
        {
            //Edit
            productViewModel.Product = _unitOfWork.Product.Get(p => p.Id == productId);
            return View(productViewModel);
        }
        
    }
    [HttpPost]
    public IActionResult Upsert(ProductViewModel productViewModel, IFormFile file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if(file!=null)
            {
                string fileName=Guid.NewGuid().ToString();
                string productPath=Path.Combine(wwwRootPath, @"Images\Product");

                if(!string.IsNullOrEmpty(productViewModel.Product.ImageUrl))
                {
                    var oldImagePath=Path.Combine(wwwRootPath,productViewModel.Product.ImageUrl.Trim('\\'));
                    if(System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using(var fileStream=new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                productViewModel.Product.ImageUrl = @"Images\Product\" + fileName;
            }

            if(productViewModel.Product.Id==0)
            {
                _unitOfWork.Product.Add(productViewModel.Product);
            }
            else
            {
                _unitOfWork.Product.Update(productViewModel.Product);
            }
            
            _unitOfWork.Save();
            //TempData["success"] = "Product created sucessfully!";
            return RedirectToAction("Index", "Product");
        }
        else
        {
            productViewModel.CategoryList= _unitOfWork.Category.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });
        }
        return View(productViewModel);
    }

    public IActionResult Delete(int? productId)
    {
        if (productId == null || productId == 0)
        {
            return NotFound();
        }
        Product? product = _unitOfWork.Product.Get(c => c.Id == productId);

        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }
    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? productId)
    {
        Product? product = _unitOfWork.Product.Get(c => c.Id == productId);
        if (product == null)
        {
            return NotFound();
        }

        _unitOfWork.Product.Delete(product);
        _unitOfWork.Save();
        TempData["success"] = "Product deleted sucessfully!";
        return RedirectToAction("Index", "Product");

    }
}
