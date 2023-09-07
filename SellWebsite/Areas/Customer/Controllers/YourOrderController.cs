using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            YourOrderVM.ListOrderHeader = _unitOfWork.OrderHeader.GetAll(includes: p => p.ApplicationUser, filter: p => p.ApplicationUserId == userId).ToList();

            return View(YourOrderVM);
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
        #endregion
    }
}
