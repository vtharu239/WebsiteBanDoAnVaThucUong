﻿using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanDoAnVaThucUong.Models.EF;
using WebsiteBanDoAnVaThucUong.Models;
using System.Web.Management;
using WebsiteBanDoAnVaThucUong.Models.Payments;

namespace WebsiteBanDoAnVaThucUong.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ShoppingCartController()
        {
        }

        public ShoppingCartController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: ShoppingCart
        [AllowAnonymous]
        public ActionResult Index()
        {

            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Partial_Item_ThanhToan()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        [AllowAnonymous]
        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult Partial_CheckOut()
        {
            var user = UserManager.FindByNameAsync(User.Identity.Name).Result;
            if (user != null)
            {
                ViewBag.User = user;
            }
            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req, string CouponCode)
        {

            var code = new { Success = false, Code = -1, Url = "" };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    Order order = new Order();
                    order.OrderStatus = 1;//chưa thanh toán, 2/đã thanh toán, 3/Hoàn thành, 4/hủy
                    order.ShippingStatus = 1; // Chờ xác nhận

                    // Apply promotions
                    var appliedPromotions = ApplyPromotions(cart);

                    foreach (var item in cart.Items)
                    {
                        var orderDetail = new OrderDetail
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.Price,
                            Subtotal = item.Quantity * item.Price,
                            DiscountAmount = item.DiscountAmount,
                            FinalAmount = item.TotalPrice
                        };
                        order.OrderDetails.Add(orderDetail);

                        // Add OrderDetailPromotions
                        if (appliedPromotions.TryGetValue(item.ProductId, out var promotions))
                        {
                            foreach (var promo in promotions)
                            {
                                orderDetail.OrderDetailPromotion.Add(new OrderDetailPromotion
                                {
                                    PromotionId = promo.PromotionId,
                                    DiscountAmount = promo.DiscountAmount
                                });
                            }
                        }

                        // Update product quantity
                        var product = db.Products.Find(item.ProductId);
                        if (product != null)
                        {
                            product.Quantity -= item.Quantity;
                        }
                    }

                    order.TotalQuantity = cart.GetTotalQuantity();
                    order.SubTotal = cart.GetSubTotal();
                    order.ProductDiscountTotal = cart.GetTotalDiscount();
                    order.TypePayment = req.TypePayment;
                    order.CreatedDate = DateTime.Now;
                    order.CreatedBy = req.CustomerName;
                    order.ModifiedDate = DateTime.Now;
                    order.ModifiedBy = User.Identity.GetUserId();

                    // Áp dụng mã giảm giá nếu có
                    var voucher = db.Vouchers.FirstOrDefault(v => v.Coupon == CouponCode && v.StartDate <= DateTime.Now && v.EndDate >= DateTime.Now);
                    if (voucher != null)
                    {
                        order.VoucherId = voucher.Id;
                        order.VoucherDiscount = order.SubTotal * voucher.Discount;
                    }
                    order.FinalAmount = order.SubTotal - order.ProductDiscountTotal - order.VoucherDiscount;

                    if (User.Identity.IsAuthenticated)
                        order.CustomerId = User.Identity.GetUserId();

                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);

                    //order.E = req.CustomerName;
                    db.Orders.Add(order);
                    db.SaveChanges();
                    //send mail cho khachs hang
                    var strSanPham = "";
                    var thanhtien = decimal.Zero;
                    var TongTien = decimal.Zero;
                    foreach (var sp in cart.Items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + sp.ProductName + "</td>";
                        strSanPham += "<td>" + sp.Quantity + "</td>";
                        strSanPham += "<td>" + WebsiteBanDoAnVaThucUong.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Price * sp.Quantity;
                    }
                    TongTien = thanhtien;
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", req.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", req.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", req.Address);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", WebsiteBanDoAnVaThucUong.Common.Common.FormatNumber(thanhtien, 0));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", WebsiteBanDoAnVaThucUong.Common.Common.FormatNumber(TongTien, 0));
                    WebsiteBanDoAnVaThucUong.Common.Common.SendMail("ShopOnline", "Đơn hàng #" + order.Code, contentCustomer.ToString(), req.Email);

                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", order.Code);
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", req.CustomerName);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", req.Phone);
                    contentAdmin = contentAdmin.Replace("{{Email}}", req.Email);
                    contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", req.Address);
                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", WebsiteBanDoAnVaThucUong.Common.Common.FormatNumber(thanhtien, 0));
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", WebsiteBanDoAnVaThucUong.Common.Common.FormatNumber(TongTien, 0));
                    WebsiteBanDoAnVaThucUong.Common.Common.SendMail("ShopOnline", "Đơn hàng mới #" + order.Code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);

                    cart.ClearCart();
                    code = new { Success = true, Code = req.TypePayment, Url = "" };
                    //var url = "";
                    if (req.TypePayment == 2)
                    {
                        var url = UrlPayment(req.TypePaymentVN, order.Code);
                        code = new { Success = true, Code = req.TypePayment, Url = url };
                    }

                    //code = new { Success = true, Code = 1, Url = url };
                    //return RedirectToAction("CheckOutSuccess");
                }
            }
            return Json(code);
        }

        private Dictionary<int, List<(int PromotionId, decimal DiscountAmount)>> ApplyPromotions(ShoppingCart cart)
        {
            var appliedPromotions = new Dictionary<int, List<(int PromotionId, decimal DiscountAmount)>>();
            var activePromotions = db.Promotions.Where(p => p.IsActive && p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now).ToList();

            foreach (var promotion in activePromotions)
            {
                switch (promotion.DiscountType)
                {
                    case 1: // Percentage discount
                        ApplyPercentageDiscount(cart, promotion, appliedPromotions);
                        break;
                    case 2: // Combo discount
                        ApplyComboDiscount(cart, promotion, appliedPromotions);
                        break;
                    case 3: // Buy X Get Y
                        ApplyBuyXGetY(cart, promotion, appliedPromotions);
                        break;
                }
            }

            return appliedPromotions;
        }

        private void ApplyPercentageDiscount(ShoppingCart cart, Promotion promotion, Dictionary<int, List<(int PromotionId, decimal DiscountAmount)>> appliedPromotions)
        {
            var eligibleProducts = db.PromotionProducts
                .Where(pp => pp.PromotionId == promotion.Id)
                .Select(pp => pp.ProductId)
                .ToList();

            foreach (var item in cart.Items.Where(i => eligibleProducts.Contains(i.ProductId)))
            {
                decimal discountAmount = item.Price * (decimal)promotion.DiscountValue * item.Quantity;
                item.DiscountAmount += discountAmount;
                item.TotalPrice -= discountAmount;

                if (!appliedPromotions.ContainsKey(item.ProductId))
                    appliedPromotions[item.ProductId] = new List<(int, decimal)>();
                appliedPromotions[item.ProductId].Add((promotion.Id, discountAmount));
            }
        }

        private void ApplyComboDiscount(ShoppingCart cart, Promotion promotion, Dictionary<int, List<(int PromotionId, decimal DiscountAmount)>> appliedPromotions)
        {
            var eligibleProducts = db.PromotionProducts
                .Where(pp => pp.PromotionId == promotion.Id)
                .Select(pp => pp.ProductId)
                .ToList();

            var comboItems = cart.Items.Where(i => eligibleProducts.Contains(i.ProductId)).ToList();

            if (comboItems.Count >= 3)
            {
                decimal comboTotal = comboItems.Sum(i => i.Price * i.Quantity);
                decimal discountAmount = comboTotal * (decimal)promotion.DiscountValue;

                // Distribute discount proportionally
                foreach (var item in comboItems)
                {
                    decimal itemDiscount = (item.Price * item.Quantity / comboTotal) * discountAmount;
                    item.DiscountAmount += itemDiscount;
                    item.TotalPrice -= itemDiscount;

                    if (!appliedPromotions.ContainsKey(item.ProductId))
                        appliedPromotions[item.ProductId] = new List<(int, decimal)>();
                    appliedPromotions[item.ProductId].Add((promotion.Id, itemDiscount));
                }
            }
        }

        private void ApplyBuyXGetY(ShoppingCart cart, Promotion promotion, Dictionary<int, List<(int PromotionId, decimal DiscountAmount)>> appliedPromotions)
        {
            var buyProducts = db.PromotionProducts
                .Where(pp => pp.PromotionId == promotion.Id && pp.IsBuyProduct)
                .Select(pp => pp.ProductId)
                .ToList();

            var getFreeProducts = db.PromotionProducts
                .Where(pp => pp.PromotionId == promotion.Id && !pp.IsBuyProduct)
                .Select(pp => pp.ProductId)
                .FirstOrDefault();

            var buyItemsCount = cart.Items.Where(i => buyProducts.Contains(i.ProductId)).Sum(i => i.Quantity);

            if (buyItemsCount >= 2 && getFreeProducts != null)
            {
                var freeProduct = db.Products.Find(getFreeProducts);
                if (freeProduct != null)
                {
                    var freeItem = new ShoppingCartItem
                    {
                        ProductId = freeProduct.Id,
                        ProductName = freeProduct.Title,
                        Quantity = 1,
                        Price = freeProduct.SalePrice,
                        TotalPrice = 0,
                        DiscountAmount = freeProduct.SalePrice
                    };
                    cart.Items.Add(freeItem);

                    if (!appliedPromotions.ContainsKey(freeProduct.Id))
                        appliedPromotions[freeProduct.Id] = new List<(int, decimal)>();
                    appliedPromotions[freeProduct.Id].Add((promotion.Id, freeProduct.SalePrice));
                }
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            var db = new ApplicationDbContext();
            var checkProduct = db.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Alias = checkProduct.Alias,
                    Quantity = quantity
                };
                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Price = checkProduct.SalePrice;
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = true, msg = "Thêm sản phẩm vào giỏ hàng thành công!", code = 1, Count = cart.Items.Count };

                // Kiểm tra điều kiện "Mua 2 tặng 1"
                var promotion = db.Promotions.FirstOrDefault(p => p.DiscountType == 3 && p.IsActive);
                if (promotion != null)
                {
                    var eligibleProducts = db.PromotionProducts
                        .Where(pp => pp.PromotionId == promotion.Id && pp.IsBuyProduct)
                        .Select(pp => pp.ProductId)
                        .ToList();

                    if (eligibleProducts.Contains(id))
                    {
                        var cartItems = cart.Items.Where(i => eligibleProducts.Contains(i.ProductId)).ToList();
                        if (cartItems.Sum(i => i.Quantity) >= 2)
                        {
                            var giftProduct = db.PromotionProducts
                                .FirstOrDefault(pp => pp.PromotionId == promotion.Id && !pp.IsBuyProduct);
                            if (giftProduct != null)
                            {
                                var giftItem = new ShoppingCartItem
                                {
                                    ProductId = giftProduct.ProductId,
                                    ProductName = giftProduct.Product.Title,
                                    Alias = giftProduct.Product.Alias,
                                    CategoryName = giftProduct.Product.ProductCategory.Title,
                                    Quantity = 1,
                                    Price = 0,
                                    OriginalPrice = giftProduct.Product.SalePrice,
                                    TotalPrice = 0,
                                    ProductImg = giftProduct.Product.ProductImage.FirstOrDefault(x => x.IsDefault)?.Image,
                                    IsGift = true
                                };
                                cart.AddToCart(giftItem, 1);
                            }
                        }
                    }
                }
            }
            return Json(code);
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult ApplyCoupon(string couponCode)
        {
            var voucher = db.Vouchers.FirstOrDefault(v => v.Coupon == couponCode && v.StartDate <= DateTime.Now && v.EndDate >= DateTime.Now);
            if (voucher != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                decimal originalTotal = cart.GetTotalPrice();
                decimal discountAmount = originalTotal * voucher.Discount;
                decimal newTotal = originalTotal - discountAmount;

                return Json(new { success = true, originalTotal = originalTotal, newTotal = newTotal, discountAmount = discountAmount });
            }
            return Json(new { success = false, message = "Mã giảm giá không hợp lệ hoặc đã hết hạn." });
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];

            if (cart != null)
            {

                var product = db.Products.Find(id);
                // Kiểm tra số lượng tồn kho
                if (product.Quantity < quantity)
                {

                    return Json(new { Success = false, msg = "Not enough stock available.", code = -1, Count = cart.Items.Count });
                }
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true, msg = "Cart updated successfully.", code = 1, Count = cart.Items.Count });
            }
            return Json(new { Success = false, msg = "Cart not found.", code = -1, Count = 0 });
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };

            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
                }
            }
            return Json(code);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

        [AllowAnonymous]
        public ActionResult VnpayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var itemOrder = db.Orders.FirstOrDefault(x => x.Code == orderCode);
                        if (itemOrder != null)
                        {
                            itemOrder.OrderStatus = 2;//đã thanh toán
                            db.Orders.Attach(itemOrder);
                            db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
            }
            //var a = UrlPayment(0, "DH3574");
            return View();
        }
        #region Thanh toán vnpay
        public string UrlPayment(int TypePaymentVN, string orderCode)
        {
            var urlPayment = "";
            var order = db.Orders.FirstOrDefault(x => x.Code == orderCode);
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)order.FinalAmount * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.Code);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.Code); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return urlPayment;
        }
        #endregion
    }
}