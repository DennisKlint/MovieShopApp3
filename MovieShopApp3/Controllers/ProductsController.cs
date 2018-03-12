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
    public class ProductsController : Controller
    {
        private dbMSA3Entities db = new dbMSA3Entities();

        // GET: Products
        public ActionResult Index()
        {
           
            var products = db.Products.Include(p => p.ProductType);
            return View(products.ToList());
        }
       
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.ProductTypeID = new SelectList(db.ProductType, "ProductTypeID", "ProductTypeName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,MovieDescription,Price,NrInStore,Rating,ProductTypeID")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductTypeID = new SelectList(db.ProductType, "ProductTypeID", "ProductTypeName", products.ProductTypeID);
            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductTypeID = new SelectList(db.ProductType, "ProductTypeID", "ProductTypeName", products.ProductTypeID);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,MovieDescription,Price,NrInStore,Rating,ProductTypeID")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductTypeID = new SelectList(db.ProductType, "ProductTypeID", "ProductTypeName", products.ProductTypeID);
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
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


        public ActionResult Purchase(int id)
        {
            if (Session["CartList"] == null)
            {
                List<int> cartList = new List<int>();
                Session["CartList"] = cartList;
            }
            //string sessionId = this.Session.SessionID;
            //Session["sessionsId"] = sessionId;
            

            var cartlist = (List<int>)Session["CartList"];
            cartlist.Add(id);
           Session["CartList"] = cartlist;
          
            return RedirectToAction("Index");

        }

        public ActionResult ShopingCartDetails()
        {
            dbMSA3Entities cont = new dbMSA3Entities();
            List<Products> plist = new List<Products>();
            var cartlist = (List<int>)Session["CartList"];
            foreach (int itm in cartlist)
            {
                Products obj = new Products();
                obj = cont.Products.Single(x => x.ProductID == itm);

                plist.Add(obj);
            }
          
            return View(plist);
        }
        public ActionResult DeleteFromBasket(int id)
        {

            var cartlist = (List<int>)Session["CartList"];
            cartlist.Remove (id);
            Session["CartList"] = cartlist;
            
            return RedirectToAction("ShopingCartDetails");

        }

        public ActionResult ShopingCart()
        {
            dbMSA3Entities cont = new dbMSA3Entities();
            List<Products> plist = new List<Products>();
            List<ProductCategoriesViewModel> productCategoryList = new List<ProductCategoriesViewModel>();

            //Contains a list of ID's, we'll use this to find the correct products
            var cartlist = (List<int>)Session["CartList"];
            foreach (int itm in cartlist)
            {
                List<Categories> categories = new List<Categories>();

                //Find and save the product linked to the ID
                Products obj = new Products();
                obj = cont.Products.Single(x => x.ProductID == itm);

                //find each category "linked" to the product, and add it to our category list
                foreach (var prodCat in cont.ProdCat.Where(x => x.ProductID == obj.ProductID))
                {

                    //This query will find the category 
                    IEnumerable<Categories> query = from c in cont.Categories where c.CategoryID == prodCat.CategoryID select c;

                    //for each category, add it to a temporary list, so we can include it into our final list
                    foreach (Categories category in query)
                    {
                        categories.Add(category);
                    }


                }
                productCategoryList.Add(new ProductCategoriesViewModel(obj.ProductID, obj.ProductName, obj.Price, obj.ProductTypeID, categories));

                //cont.Database.SqlQuery<Products> ( "SELECT * FROM Products WHERE ProductID =@p0 ,'" + itm + "'");
                plist.Add(obj);
            }

            //var products = db.Products.Include(p => p.ProductType);
            return View(productCategoryList);
        }

    }
    }

