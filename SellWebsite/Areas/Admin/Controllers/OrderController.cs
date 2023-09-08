using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SellWebsite.DataAccess.Reponsitory.IReponsitory;
using SellWebsite.Models.Models;
using SellWebsite.Utility;

namespace SellWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Boss},{SD.Role_Employee}")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            List<OrderHeader> orderHeaders = _unitOfWork.OrderHeader.GetAll().ToList();
            return View(orderHeaders);
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
            List<OrderHeader> orderHeaders = _unitOfWork.OrderHeader.GetAll().ToList();
            return Json(new { data = orderHeaders });
        }

        [HttpGet]
        public IActionResult GetOrderDetail(int? idOrder)
        {
            List<OrderDetail> orderDetails = _unitOfWork.OrderDetail.GetAll(filter: p => p.OrderHeaderId == idOrder, includes: p => p.Product).ToList();
            return Json(new { data = orderDetails });
        }

        public IActionResult ApproveOderStatus(int idOrder)
        {
            _unitOfWork.OrderHeader.UpdateStatus(idOrder, SD.StatusApproved);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Update successful" }); ;
        }
        public IActionResult RejectedOderStatus(int idOrder)
        {
            _unitOfWork.OrderHeader.UpdateStatus(idOrder, SD.PaymentRejected);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Update successful" }); ;
        }
        public IActionResult ReturnedOderStatus(int idOrder)
        {
            _unitOfWork.OrderHeader.UpdateStatus(idOrder, SD.StatusReturned);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Update successful" }); ;
        }

        #endregion
    }
}
