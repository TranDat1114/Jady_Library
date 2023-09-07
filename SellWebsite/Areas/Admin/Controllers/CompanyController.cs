using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SellWebsite.DataAccess.Reponsitory.IReponsitory;
using SellWebsite.Models.Models;
using SellWebsite.Models.ViewModels.Customer;
using SellWebsite.Utility;

namespace SellWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Boss}")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertBookVM UpsertBookVM { get; set; }

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {

            DetailCompanyVM detailCompanyVM = new()
            {
                ListProductInCompany = new(),
                CompanyId = id,
                CompanyName = _unitOfWork.Company.Get(filter: p => p.CompanyId == id).Name
            };

            if (id == 0)
            {
                detailCompanyVM.ListProductInCompany = _unitOfWork.ProductInCompany.GetAll(includes: p => p.Product).ToList();
            }
            else
            {
                detailCompanyVM.ListProductInCompany = _unitOfWork.ProductInCompany.GetAll(includes: p => p.Product, filter: p => p.CompanyId == id).ToList();
            }
            return View(detailCompanyVM);
        }


        public IActionResult Upsert(int? id)
        {
            var company = new Company();
            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                company = _unitOfWork.Company.Get(p => p.CompanyId == id);
            }
            return View(company);
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {

            if (ModelState.IsValid)
            {

                if (company.CompanyId == 0)
                {
                    _unitOfWork.Company.Add(company);
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                }
                _unitOfWork.Save();
                TempData["success"] = "Category update successfully";

                return RedirectToAction(nameof(Index));

            }
            return View(company);

        }

        public IActionResult UpsertBook(int? id, int companyId)
        {
            UpsertBookVM = new()
            {
                ProductInCompany = new()
                {
                    CompanyId = companyId,
                },
                ListProduct = _unitOfWork.Product.GetAll().ToList(),
            };
            if (id == null || id == 0)
            {
                //return View(UpsertBookVM);
            }
            else
            {
                UpsertBookVM.ProductInCompany = _unitOfWork.ProductInCompany.Get(p => p.ProductInCompanyId == id);
            }
            return View(UpsertBookVM);
        }


        [HttpPost]
        public IActionResult UpsertBook(ProductInCompany productInCompany)
        {
            if (ModelState.IsValid)
            {
                if (productInCompany.ProductInCompanyId == 0)
                {
                    var productInCom = _unitOfWork.ProductInCompany.Get(filter: p => p.ProductId == productInCompany.ProductId && p.CompanyId == productInCompany.CompanyId);
                    if (productInCom != null)
                    {
                        productInCom.Quantity = productInCompany.Quantity;
                    }
                    else
                    {
                        _unitOfWork.ProductInCompany.Add(productInCompany);
                    }

                }
                else
                {
                    _unitOfWork.ProductInCompany.Update(productInCompany);
                }
                _unitOfWork.Save();
                TempData["success"] = "Successfully";

                return Redirect($"Details?id={productInCompany.CompanyId}");

            }
            return View(productInCompany);

        }

        #region API Call
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = companies });
        }

        //[Route("/admin/product/delete/{id:int?}")]
        public IActionResult Delete(int? id)
        {
            var company = _unitOfWork.Company.Get(u => u.CompanyId == id);

            if (company == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(company!);


            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
        public IActionResult DeleteBook(int? id)
        {
            var company = _unitOfWork.ProductInCompany.Get(u => u.ProductInCompanyId == id);

            if (company == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.ProductInCompany.Remove(company!);


            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
