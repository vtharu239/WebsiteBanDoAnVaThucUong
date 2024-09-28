﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanDoAnVaThucUong.Models.EF;
using WebsiteBanDoAnVaThucUong.Models;

namespace WebsiteBanDoAnVaThucUong.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
            var items = db.ProductCategories;
            return View(items);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.Alias = WebsiteBanDoAnVaThucUong.Models.Common.Filter.FilterChar(model.Title);

                var catepro = db.ProductCategories.FirstOrDefault(c => c.Title == model.Title);
                if (catepro != null)
                {
                    ModelState.AddModelError(string.Empty, "Đã tồn tại danh mục, hãy đặt tên khác");
                    return View();
                }
                if (ModelState.IsValid)
                {
                    db.ProductCategories.Add(model);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");

            }

            return View();
        }
        public ActionResult Edit(int id)
        {
            var item = db.ProductCategories.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.Alias = WebsiteBanDoAnVaThucUong.Models.Common.Filter.FilterChar(model.Title);
                db.ProductCategories.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}