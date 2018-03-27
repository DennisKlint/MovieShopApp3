using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieShopApp3.Models;

namespace MovieShopApp3.Controllers
{
    public class ProdCatsController : Controller
    {
        private dbMSA3Entities db = new dbMSA3Entities();

        // GET: ProdCats
        public ActionResult Index()
        {
            var prodCat = db.ProdCat.Include(p => p.Categories).Include(p => p.Products);
            return View(prodCat.ToList());
        }

        // GET: ProdCats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProdCat prodCat = db.ProdCat.Find(id);
            if (prodCat == null)
            {
                return HttpNotFound();
            }
            return View(prodCat);
        }

        // GET: ProdCats/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            return View();
        }

        // POST: ProdCats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProdCatID,ProductID,CategoryID")] ProdCat prodCat)
        {
            if (ModelState.IsValid)
            {
                db.ProdCat.Add(prodCat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", prodCat.CategoryID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", prodCat.ProductID);
            return View(prodCat);
        }

        // GET: ProdCats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProdCat prodCat = db.ProdCat.Find(id);
            if (prodCat == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", prodCat.CategoryID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", prodCat.ProductID);
            return View(prodCat);
        }

        // POST: ProdCats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProdCatID,ProductID,CategoryID")] ProdCat prodCat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prodCat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", prodCat.CategoryID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", prodCat.ProductID);
            return View(prodCat);
        }

        // GET: ProdCats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProdCat prodCat = db.ProdCat.Find(id);
            if (prodCat == null)
            {
                return HttpNotFound();
            }
            return View(prodCat);
        }

        // POST: ProdCats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProdCat prodCat = db.ProdCat.Find(id);
            db.ProdCat.Remove(prodCat);
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
