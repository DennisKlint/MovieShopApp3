using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieShopApp3.Models;
using PagedList;

namespace MovieShopApp3.Controllers
{
    public class ProductsController : Controller
    {
        private dbMSA3Entities db = new dbMSA3Entities();
        private const int pageSize = 2;
        // GET: Products
        public ActionResult Index(int? page)
        {
            int pageNumber =0;
            if (page == null && Session["pageNumber"] == null)
            {
                pageNumber = (page ?? 1);
            }
            else if (page == null && !(Session["pageNumber"] == null))
            {
                pageNumber = Convert.ToInt32(Session["pageNumber"]);
            }
            else if(!(page == null))
            {
                pageNumber = (page ?? 1);
            }



            Session["pageNumber"] = pageNumber;

            var products = db.Products.Include(p => p.ProductType).ToList().ToPagedList(pageNumber, pageSize);
            if (!(Session["selectedCat"] == null))
            {
                return showByCategory(Convert.ToInt32(Session["selectedCat"]), pageNumber);
            }
            else
            {
                return View(products);
            }



        }

        public ActionResult showByCategory(int id, int? page)
        {


            int pageNumber = Convert.ToInt32(Session["pageNumber"]);


            Session["selectedCat"] = id;
            var products = db.Products.Include(p => p.ProductType).ToList().ToPagedList(pageNumber, pageSize);
            List<Products> productlist = new List<Products>();
            if (!(id == 99)) // show products for all categories
            //{
            //    var products = db.Products.Include(p => p.ProductType).ToList().ToPagedList(pageNumber, pageSize);
            //    //productlist = products.ToList();
            //}
            //else //show only products for selected categorie
            {
                products = db.Products.SqlQuery("Select Products.* from Products,ProdCat where Products.ProductID = ProdCat.ProductID AND ProdCat.CategoryID ='" + id + "'").ToList().ToPagedList(pageNumber, pageSize);

            }


            return View("Index", products);

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
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,MovieDescription,Price,NrInStore,Rating,ProductTypeID,Category")] FullProductAndCategoriesModel dataModel)
        {

            Products products = new Products()
            {
                ProductID = dataModel.ProductID,
                ProductName = dataModel.ProductName,
                MovieDescription = dataModel.MovieDescription,
                Price = dataModel.Price,
                NrInStore = dataModel.NrInStore,
                Rating = dataModel.Rating,
                ProductTypeID = dataModel.ProductTypeID
            };

            if (ModelState.IsValid)
            {

                List<Categories> categories = new List<Categories>(dataModel.Category);

                db.Products.Add(products);
                foreach (var cat in dataModel.Category)
                {
                    ProdCat prodCat = new ProdCat()
                    {
                        ProductID = products.ProductID,
                        CategoryID = cat.CategoryID
                    };

                    db.ProdCat.Add(prodCat);
                }
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
            var cartlist = (List<int>)Session["CartList"];
            cartlist.Add(id);
            Session["CartList"] = cartlist;


            int no = 0;
            if (!(cartlist == null))
            {
                no = cartlist.Count();
            }

            Session["noOfitems"] = no;
            if (!(Session["selectedCat"] == null))
            {
                return showByCategory(Convert.ToInt32(Session["selectedCat"]), Convert.ToInt32(Session["pageNumber"]));
            }
            else
            {
                return RedirectToAction("Index", Convert.ToInt32(Session["pageNumber"]));
            }

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
            cartlist.Remove(id);
            Session["CartList"] = cartlist;
            Session["noOfitems"] = Convert.ToInt32(Session["noOfitems"]) - 1;
            return RedirectToAction("ShopingCart");

        }

        public ActionResult ShopingCart()
        {
            dbMSA3Entities cont = new dbMSA3Entities();
            List<ProductCategoriesViewModel> productCategoryList = new List<ProductCategoriesViewModel>();
            if (Session["CartList"] == null)
            {
                List<int> cartList = new List<int>();
                Session["CartList"] = cartList;
            }
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

            }

            return View(productCategoryList);
        }
        public ActionResult PartialViewShopingBasket()
        {
            Session["noOfitems"] = null;
            var cartlist = (List<int>)Session["CartList"];
            int no = 0;
            if (!(cartlist == null))
            {
                no = cartlist.Count();
            }
            Session["noOfitems"] = no;
            return PartialView("PartialViewShopingBasket");
        }

    }

}