using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanDoAnVaThucUong.Models;
using WebsiteBanDoAnVaThucUong.Models.EF;

namespace WebsiteBanDoAnVaThucUong.Controllers
{
    public class StoreProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StoreProducts
        public ActionResult Index()
        {
            var storeProducts = db.StoreProducts.Include(s => s.Product).Include(s => s.Store);
            return View(storeProducts.ToList());
        }

        // GET: StoreProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreProduct storeProduct = db.StoreProducts.Find(id);
            if (storeProduct == null)
            {
                return HttpNotFound();
            }
            return View(storeProduct);
        }

        // GET: StoreProducts/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title");
            ViewBag.StoreId = new SelectList(db.Stores, "Id", "Alias");
            return View();
        }

        // POST: StoreProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StoreId,ProductId,StockCount,SellCount")] StoreProduct storeProduct)
        {
            if (ModelState.IsValid)
            {
                db.StoreProducts.Add(storeProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", storeProduct.ProductId);
            ViewBag.StoreId = new SelectList(db.Stores, "Id", "Alias", storeProduct.StoreId);
            return View(storeProduct);
        }

        // GET: StoreProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreProduct storeProduct = db.StoreProducts.Find(id);
            if (storeProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", storeProduct.ProductId);
            ViewBag.StoreId = new SelectList(db.Stores, "Id", "Alias", storeProduct.StoreId);
            return View(storeProduct);
        }

        // POST: StoreProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StoreId,ProductId,StockCount,SellCount")] StoreProduct storeProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storeProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", storeProduct.ProductId);
            ViewBag.StoreId = new SelectList(db.Stores, "Id", "Alias", storeProduct.StoreId);
            return View(storeProduct);
        }

        // GET: StoreProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreProduct storeProduct = db.StoreProducts.Find(id);
            if (storeProduct == null)
            {
                return HttpNotFound();
            }
            return View(storeProduct);
        }

        // POST: StoreProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StoreProduct storeProduct = db.StoreProducts.Find(id);
            db.StoreProducts.Remove(storeProduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
