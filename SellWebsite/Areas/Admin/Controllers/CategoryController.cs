﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SellWebsite.DataAccess.Reponsitory.IReponsitory;
using SellWebsite.Models.Models;
using SellWebsite.Models.ViewModels.Admin;
using SellWebsite.Utility;

namespace SellWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Boss},{SD.Role_Employee}")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            //Sử dụng API nên hiện không cần dùng nữa
            //var objCategories = _unitOfWork.Category.GetAll().ToList();
            //return View(objCategories);
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            var categoryVM = new CategoryVM()
            {
                Category = new Category(),
            };
            if (id == null || id == 0)
            {
                return View(categoryVM);
            }
            else
            {
                categoryVM.Category = _unitOfWork.Category.Get(p => p.CategoryId == id);
            }
            return View(categoryVM);
        }

        [HttpPost]
        public IActionResult Upsert(CategoryVM categoryVM)
        {
            //Custom Validation testing
            //if (category.NameEnglish != null && category.NameEnglish.ToLower() == "test")
            //{
            //    ModelState.AddModelError("Name", "Test is invalid value");
            //}

            if (ModelState.IsValid)
            {
                if (categoryVM.Category.CategoryId == 0)
                {
                    _unitOfWork.Category.Add(categoryVM.Category);
                }
                else
                {
                    _unitOfWork.Category.Update(categoryVM.Category);
                }
                _unitOfWork.Save();
                TempData["success"] = "Category update successfully";

                return RedirectToAction(nameof(Index));

            }
            return View(categoryVM);

        }

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var categoryVM = new CategoryVM()
        //    {
        //        Category = _unitOfWork.Category.Get(obj => obj.Id == id),
        //    };
        //    if (categoryVM.Category == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(categoryVM);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{
        //    var category = _unitOfWork.Category.Get(obj => obj.Id == id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Category.Remove(category);
        //    _unitOfWork.Save();
        //    TempData["Success"] = "Category deleted successfully";
        //    return RedirectToAction("Index");
        //}

        #region API Call
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();
            return Json(new { data = categories });
        }

        //[Route("/admin/product/delete/{id:int?}")]
        public IActionResult Delete(int? id)
        {
            var category = _unitOfWork.Category.Get(u => u.CategoryId == id);

            if (category == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Category.Remove(category!);

            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion

    }
}
