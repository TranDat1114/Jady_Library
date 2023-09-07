using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using SellWebsite.DataAccess.Reponsitory.IReponsitory;
using SellWebsite.Models.Models;
using SellWebsite.Models.ViewModels.Customer;
using SellWebsite.Utility;

namespace SellWebsite.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;


        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public IActionResult Index()
        {
            HttpContext.Session.Remove(SD.SessionShopingCarts);

            var claimIdentity = (ClaimsIdentity)User.Identity!;
            var sessionShopCart = HttpContext.Session.GetString(SD.SessionShopingCarts);
            var listCarts = new List<ShoppingCart>();

            ShoppingCartVM = new()
            {
                OrderHeader = new OrderHeader(),
                Companies = _unitOfWork.Company.GetAll().ToList(),
            };

            if (sessionShopCart != null)
            {
                listCarts = JsonConvert.DeserializeObject<List<ShoppingCart>>(HttpContext.Session.GetString(SD.SessionShopingCarts));
            }
            if (claimIdentity.Name != null)
            {
                var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                if (listCarts.Count > 0)
                {
                    var listNewCart = new List<ShoppingCart>();
                    foreach (var item in listCarts)
                    {
                        var list = _unitOfWork.ShoppingCart.GetAll(p => p.ApplicationUserId == userId, x => x.Product).ToList();
                        if (!list.Any(p => p.ProductId == item.ProductId))
                        {
                            item.ApplicationUserId = userId;
                            item.Product = null;
                            listNewCart.Add(item);
                        }
                        else
                        {
                            //Logic khi cần tăng thêm số lượng sản phẩm theo session vào database
                        }
                    }
                    if (listNewCart.Count > 0)
                    {
                        _unitOfWork.ShoppingCart.AddRange(listNewCart);
                        _unitOfWork.Save();

                        //Xong rồi thì nhớ xóa session tránh việc lặp lại việc tăng sản phẩm trong giỏ mỗi khi gọi tới index
                        HttpContext.Session.Remove(SD.SessionShopingCarts);
                    }
                }

                listCarts = _unitOfWork.ShoppingCart.GetAll(p => p.ApplicationUserId == userId, x => x.Product).ToList();

                ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
                ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
                ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber!;
                ShoppingCartVM.OrderHeader.Email = ShoppingCartVM.OrderHeader.ApplicationUser.Email;
            }

            ShoppingCartVM.Carts = listCarts!;

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Index")]
        [Authorize]
        public async Task<IActionResult> IndexPOST()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity!;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            ShoppingCartVM.Carts = _unitOfWork.ShoppingCart.GetAll(p => p.ApplicationUserId == userId, x => x.Product).ToList();

            if (ShoppingCartVM.Carts.Count != 0)
            {
                var dictionKeyError = new Dictionary<string, string>();
                foreach (var cart in ShoppingCartVM.Carts)
                {
                    var products = _unitOfWork.ProductInCompany.Get(includes: p => p.Product, filter: p => p.CompanyId == ShoppingCartVM.OrderHeader.CompanyId && p.ProductId == cart.ProductId);
                    if (products != null)
                    {
                        if (products.Quantity - cart.Quantity < 0)
                        {
                            dictionKeyError.Add(products.Product.Title, $"{products.Quantity}");
                        }
                    }
                }
                if (dictionKeyError.Count() == 0)
                {
                    ShoppingCartVM.OrderHeader.OrderTime = DateTime.Now;
                    ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

                    var userData = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
                    if (string.IsNullOrEmpty(userData.PhoneNumber))
                    {
                        userData.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
                    }
                    if (string.IsNullOrEmpty(userData.Email))
                    {
                        userData.Email = ShoppingCartVM.OrderHeader.ApplicationUser.Email;
                    }

                    ShoppingCartVM.OrderHeader.ApplicationUser = userData;
                    ShoppingCartVM.OrderHeader.IsReturn = false;
                    //
                    ShoppingCartVM.OrderHeader.ReturnTime = DateTime.Now.AddDays(7);
                    ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;

                    ShoppingCartVM.OrderHeader.Company = _unitOfWork.Company.Get(filter: p => p.CompanyId == ShoppingCartVM.OrderHeader.CompanyId);

                    _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
                    _unitOfWork.Save();

                    foreach (var cart in ShoppingCartVM.Carts)
                    {
                        var orderDetail = new OrderDetail()
                        {
                            ProductId = cart.ProductId,
                            OrderHeaderId = ShoppingCartVM.OrderHeader.OrderHeaderId,
                            Count = cart.Quantity,

                        };
                        _unitOfWork.OrderDetail.Add(orderDetail);
                        var productInC = _unitOfWork.ProductInCompany.Get(includes: p => p.Product, filter: p => p.CompanyId == ShoppingCartVM.OrderHeader.CompanyId && p.ProductId == cart.ProductId);
                        productInC.Quantity -= cart.Quantity;
                    }
                    _unitOfWork.Save();

                    await OrderConfirmationAsync(ShoppingCartVM.OrderHeader.OrderHeaderId);
                }
                else
                {
                    string ShowError = "";
                    foreach (var item in dictionKeyError)
                    {
                        ShowError += $"{item.Key} Only {item.Value} . left <br/>";
                    }
                    TempData["error"] = ShowError;
                }
            }
            else
            {
                TempData["error"] = $"0 item in cart";
            }
            return RedirectToAction(nameof(Index));

        }
        [Authorize]
        public async Task<IActionResult> OrderConfirmationAsync(int id)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity!;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            try
            {
                var listCarts = _unitOfWork.ShoppingCart.GetAll(p => p.ApplicationUserId == userId, x => x.Product).ToList();

                //_unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved);


                //_unitOfWork.OrderHeader.UpdateStatus(id, SD.PaymentRejected, SD.PaymentRejected);


                //Mua hàng xong thì clear cái giỏ hàng đi
                _unitOfWork.ShoppingCart.RemoveRange(listCarts);
                _unitOfWork.Save();
                //Clear cả cái session mới thêm vào nữa :_) bug mò cả ngày mới ra
                HttpContext.Session.Remove(SD.SessionCart);

                TempData["Success"] = $"Your order is success";
            }
            catch (Exception)
            {
                ///
                throw;
            }

            OrderConfirmationVM orderConfirmationVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(p => p.OrderHeaderId == id),
            };

            return View(orderConfirmationVM);
        }




        #region Cart Options
        public IActionResult Remove(int cartId, int productId)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity!;

            var sessionShopCart = HttpContext.Session.GetString(SD.SessionShopingCarts);

            var listCarts = new List<ShoppingCart>();

            if (sessionShopCart != null)
            {
                listCarts = JsonConvert.DeserializeObject<List<ShoppingCart>>(HttpContext.Session.GetString(SD.SessionShopingCarts));
            }

            if (claimIdentity.Name != null)
            {
                var productInCart = _unitOfWork.ShoppingCart.Get(p => p.CartId == cartId);
                _unitOfWork.ShoppingCart.Remove(productInCart);
                _unitOfWork.Save();

                listCarts.Remove(listCarts.FirstOrDefault(p => p.CartId == cartId));
            }
            else
            {
                listCarts.Remove(listCarts.FirstOrDefault(p => p.ProductId == productId && p.ApplicationUserId == "0"));
            }

            HttpContext.Session.SetString(SD.SessionShopingCarts, JsonConvert.SerializeObject(listCarts));

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId, int productId)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity!;

            var sessionShopCart = HttpContext.Session.GetString(SD.SessionShopingCarts);

            var listCarts = new List<ShoppingCart>();

            if (sessionShopCart != null)
            {
                listCarts = JsonConvert.DeserializeObject<List<ShoppingCart>>(HttpContext.Session.GetString(SD.SessionShopingCarts));
            }

            if (claimIdentity.Name != null)
            {
                var productInCart = _unitOfWork.ShoppingCart.Get(p => p.CartId == cartId);
                if (productInCart.Quantity <= 1)
                {
                    //Session cho số lượng sản phẩm trong cart thành 0 nếu sản phẩm bị xóa hoặc số lượng = 0
                    _unitOfWork.ShoppingCart.Remove(productInCart);

                    listCarts.Remove(listCarts.FirstOrDefault(p => p.CartId == cartId));

                }
                else
                {
                    productInCart.Quantity -= 1;
                    if (listCarts.Count != 0)
                    {
                        listCarts.FirstOrDefault(p => p.CartId == cartId).Quantity -= 1;
                    }

                }
                _unitOfWork.Save();
            }
            else
            {
                //Vì không có sản phẩm nên không cần kiểm tra số lượng trong session
                if (listCarts.FirstOrDefault(p => p.ProductId == productId && p.ApplicationUserId == "0").Quantity <= 1)
                {
                    listCarts.Remove(listCarts.FirstOrDefault(p => p.ProductId == productId && p.ApplicationUserId == "0"));
                }
                else
                {
                    listCarts.FirstOrDefault(p => p.ProductId == productId && p.ApplicationUserId == "0").Quantity -= 1;
                }

            }
            HttpContext.Session.SetString(SD.SessionShopingCarts, JsonConvert.SerializeObject(listCarts));

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Plus(int cartId, int productId)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity!;

            var sessionShopCart = HttpContext.Session.GetString(SD.SessionShopingCarts);

            var listCarts = new List<ShoppingCart>();

            if (sessionShopCart != null)
            {
                listCarts = JsonConvert.DeserializeObject<List<ShoppingCart>>(HttpContext.Session.GetString(SD.SessionShopingCarts));
            }

            if (claimIdentity.Name != null)
            {
                var productInCart = _unitOfWork.ShoppingCart.Get(p => p.CartId == cartId);
                if (productInCart.Quantity >= 999)
                {

                }
                else
                {
                    productInCart.Quantity += 1;
                    if (listCarts.Count != 0)
                    {
                        listCarts.FirstOrDefault(p => p.ProductId == productId && p.ApplicationUserId == "0").Quantity += 1;
                    }
                }
                _unitOfWork.Save();
            }
            else
            {
                if (listCarts.FirstOrDefault(p => p.ProductId == productId && p.ApplicationUserId == "0").Quantity >= 999)
                {

                }
                else
                {
                    listCarts.FirstOrDefault(p => p.ProductId == productId && p.ApplicationUserId == "0").Quantity += 1;
                }
            }

            HttpContext.Session.SetString(SD.SessionShopingCarts, JsonConvert.SerializeObject(listCarts));

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }

}
