using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanDoAnVaThucUong.Models;
using Microsoft.AspNet.Identity;
using WebsiteBanDoAnVaThucUong.Models.EF;

namespace WebsiteBanDoAnVaThucUong.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index()
        {
            var items = db.Products.ToList();

            return View(items);
        }

        public ActionResult Detail(string alias, int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                // Update the product's view count
                db.Products.Attach(item);
                item.ViewCount += 1;
                db.Entry(item).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();
                // Save product to user's view history
                var userId = User.Identity.GetUserId(); // Assuming you're using Identity or any other user identification
                if (userId != null) // If the user is logged in
                {
                    var existingHistory = db.ProductViewHistory
                        .FirstOrDefault(h => h.ProductId == id && h.UserId == userId);
                    if (existingHistory == null) // Avoid duplicate entries in the history
                    {
                        var historyEntry = new ProductViewHistory
                        {
                            ProductId = id,
                            UserId = userId,
                            ViewedAt = DateTime.Now
                        };
                        db.ProductViewHistory.Add(historyEntry);
                        db.SaveChanges();
                    }
                }
                else
                {
                    // For guest users, store viewed product IDs in session
                    List<int> viewedProducts = Session["ViewHistory"] as List<int> ?? new List<int>();
                    if (!viewedProducts.Contains(id))
                    {
                        viewedProducts.Add(id);
                        Session["ViewHistory"] = viewedProducts;
                    }
                }
            }
            // Get the count of reviews
            var countReview = db.Reviews.Where(x => x.ProductId == id).Count();
            ViewBag.CountReview = countReview;
            // Return the product details view
            return View(item);
        }
        public ActionResult ViewHistory()
        {
            var userId = User.Identity.GetUserId();
            var history = db.ProductViewHistory
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.ViewedAt)
                .Take(5) // 5 hui
                .Select(h => h.Product) // Get the products
                .ToList();
            return View(history);
        }

        public ActionResult ProductCategory(string alias, int id)
        {
            var items = db.Products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = db.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }

            ViewBag.CateId = id;
            return View(items);
        }

        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.Where(x => x.IsActive).Take(12).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_ProductSales()
        {
            var products = db.Products.Where(x => x.IsActive).ToList();
            var filteredProducts = products.Where(product =>
               db.OrderDetails.Where(od => od.ProductId == product.Id).Sum(od => (int?)od.Quantity) > 10).ToList();
            return PartialView(filteredProducts);
        }

        [HttpGet]
        public ActionResult Search(string searchString)
        {
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(x => x.Alias.Contains(searchString) || x.Title.Contains(searchString));
            }
            return View(items.ToList());
        }

        // Sản phẩm liên quan
        public ActionResult RelatedProducts(int productId)
        {
            var currentProduct = db.Products.Find(productId);
            if (currentProduct == null)
            {
                return HttpNotFound();
            }

            // Chia tiêu đề sản phẩm hiện tại thành các từ khóa
            var keywords = currentProduct.Title.Split(' ');

            // Tìm các sản phẩm có tiêu đề chứa các từ khóa này (loại bỏ sản phẩm hiện tại)
            var relatedProducts = db.Products
                                    .Where(p => p.Id != productId && keywords.Any(kw => p.Title.Contains(kw)))
                                    .Take(5)  // Giới hạn số lượng sản phẩm liên quan
                                    .ToList();

            return PartialView("_RelatedProducts", relatedProducts);
        }

    }
}