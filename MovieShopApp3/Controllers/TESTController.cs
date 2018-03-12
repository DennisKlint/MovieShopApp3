using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieShopApp3.Models;
namespace MovieShopApp3.Controllers
{
    public class TESTController : Controller
    {
        private dbMSA3Entities db = new dbMSA3Entities();
        // GET: TEST
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Index(string Prefix)
        {
           
            //Note : you can bind same list from database  
          List<Products> ObjList = new List<Products>();
            List<Products> hitlist = new List<Products>();
            {
                ObjList = db.Products.ToList();

            };
            //Searching records from list using LINQ query  
            var productlist = (from N in ObjList
                               where N.ProductName.StartsWith(Prefix)
                               select N.ProductName);
            //hitlist = productlist;
            return Json(productlist, JsonRequestBehavior.AllowGet);
        }
    }
}