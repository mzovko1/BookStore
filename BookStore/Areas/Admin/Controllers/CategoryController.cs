using BookStore.Models.Models;
using Microsoft.AspNetCore.Mvc;
using BookStore.DataAccess.Data;
using BookStore.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using BookStore.Utility;

namespace BookStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Role.Role_Admin)]
public class CategoryController : Controller
{
    //private readonly ApplicationDbContext _context;
    //private readonly ICategoryRepository _categoryRepository;

    private readonly IUnitOfWork _unitOfWork;

    //public CategoryController(ICategoryRepository categoryRepository, ApplicationDbContext context)
    //{
    //    _context = context;
    //    _categoryRepository = categoryRepository;
    //}

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        List<Category> categoryList = _unitOfWork.Category.GetAll().ToList();
        return View(categoryList);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Category category)
    {
        if (category.Name.Length > 10)
        {
            ModelState.AddModelError("Name", "The name can not be longer than 10 caharacters.");
        }
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", "The display order can't be the same as the name");
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Add(category);
            _unitOfWork.Save();
            TempData["success"] = "Category created sucessfully!";
            return RedirectToAction("Index", "Category");
        }
        return View();
    }
    public IActionResult Edit(int? categoryId)
    {
        if (categoryId == null || categoryId == 0)
        {
            return NotFound();
        }
        Category? category = _unitOfWork.Category.Get(c => c.Id == categoryId);
        //Category? category1 = _context.Categories.Find(categoryId);
        //Category? category2 = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }
    [HttpPost]
    public IActionResult Edit(Category category)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(category);
            _unitOfWork.Save();
            TempData["success"] = "Category edited sucessfully!";
            return RedirectToAction("Index", "Category");
        }
        return View();
    }

    public IActionResult Delete(int? categoryId)
    {
        if (categoryId == null || categoryId == 0)
        {
            return NotFound();
        }
        Category? category = _unitOfWork.Category.Get(c => c.Id == categoryId);

        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }
    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? categoryId)
    {
        Category? category = _unitOfWork.Category.Get(c => c.Id == categoryId);
        if (category == null)
        {
            return NotFound();
        }

        _unitOfWork.Category.Delete(category);
        _unitOfWork.Save();
        TempData["success"] = "Category deleted sucessfully!";
        return RedirectToAction("Index", "Category");

    }

}
