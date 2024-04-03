using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Areas.Admin.Controllers;

public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        List<Company> companiesList = _unitOfWork.Company.GetAll().ToList();
        return View(companiesList);
    }

    public IActionResult Upsert(int? id)
    {
        Company company=new Company();
        if(id==null || id==0)
        {
            return View(company);
        }
        else
        {
            company=_unitOfWork.Company.Get(p=>p.Id==id);
            return View(company);
        }
    }
    [HttpPost]
    public IActionResult Upsert(Company company)
    {
        if (ModelState.IsValid)
        {
            TempData["success"] = "Company created succesfully";


            if (company.Id == 0)
            {
                _unitOfWork.Company.Add(company);
            }
            else
            {
                _unitOfWork.Company.Update(company);
            }

            _unitOfWork.Save();
            return RedirectToAction("Index", "Company");
        }
        return View(company);
    }


        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companiesList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = companiesList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var company = _unitOfWork.Company.Get(p => p.Id == id);
            if (company == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            else
            {
            
                _unitOfWork.Company.Delete(company);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Deleted successfully" });
            }
        }
    #endregion
}


