using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SellWebsite.DataAccess.Reponsitory.IReponsitory;
using SellWebsite.Models.Models;
using SellWebsite.Models.ViewModels.Customer;

namespace SellWebsite.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class YourOrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public YourOrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public YourOrderVM YourOrderVM { get; set; }
        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity!;

            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            YourOrderVM = new()
            {
                ListOrderHeader = new List<OrderHeader>()
            };

            YourOrderVM.ListOrderHeader = _unitOfWork.OrderHeader.GetAll(filter: p => p.ApplicationUserId == userId,
                includes: p => p.ApplicationUser
                ).Include(p => p.Company).ToList();

            return View(YourOrderVM);
        }


        public IActionResult Details(int idOrder)
        {
            List<OrderDetail> orderDetails = _unitOfWork.OrderDetail.GetAll(filter: p => p.OrderHeaderId == idOrder, includes: p => p.Product).ToList();
            return View(orderDetails);
        }

        #region API Call
        [HttpGet]
        public IActionResult GetAll()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity!;

            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            List<OrderHeader> orderHeaders = _unitOfWork.OrderHeader.GetAll(includes: p => p.ApplicationUser, filter: p => p.ApplicationUserId == userId).ToList();

            return Json(new { data = orderHeaders });
        }

        [HttpGet]
        public IActionResult GetOrderDetail(int? idOrder)
        {
            List<OrderDetail> orderDetails = _unitOfWork.OrderDetail.GetAll(filter: p => p.OrderHeaderId == idOrder, includes: p => p.Product).ToList();
            return Json(new { data = orderDetails });
        }

        public IActionResult Extend(int? idOrder)
        {
            var orderInfor = _unitOfWork.OrderHeader.Get(u => u.OrderHeaderId == idOrder);
            if (orderInfor.OrderTime.AddDays(14) < orderInfor.ReturnTime)
            {
                TempData["error"] = $"Your extend request failed cause limit time extend";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                orderInfor.ReturnTime = orderInfor.ReturnTime.AddDays(7);
            }

            _unitOfWork.Save();
            TempData["success"] = $"Your extend request successfully";

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
