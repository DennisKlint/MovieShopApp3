using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieShopApp3.Models;
namespace MovieShopApp3.Controllers
{
   
    public class CategoriesController : Controller
    {
        private dbMSA3Entities db = new dbMSA3Entities();
        // GET: Categories
        public ActionResult Index()
        {
            var cat = db.Categories;

            return PartialView("PartialViewCategories", cat.ToList());

        }
        //public ActionResult PartialViewCategories()
        //{
        //    var cat = db.Categories;

        //    return PartialView("PartialViewCategories",cat.ToList());
           
        //}
        public ActionResult showByCategory(int id)
        {
            Session["selectedCat"] = id;
            List <Products> productlist = new List<Products>();
            productlist = db.Products.SqlQuery("Select Products.* from Products,ProdCat where Products.ProductID = ProdCat.ProductID AND ProdCat.CategoryID ='"  + id + "'").ToList();


            return View("../Products/PartialView_Products",productlist);

        }
        
        // GET: Categories/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Categories/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Categories/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
           return  showByCategory(Convert.ToInt32(Session["selectedCat"]));
            //return Redirect("~/Products/Index");
            //return RedirectToAction("PartialViewCategories");
        }
    }
}
