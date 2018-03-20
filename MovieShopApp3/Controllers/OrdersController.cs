using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieShopApp3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MovieShopApp3.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private dbMSA3Entities db = new dbMSA3Entities();

        // GET: Orders
        public ActionResult Index()
        {
            //Establish a connection so we can find the logged in user
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (User.IsInRole("Admin")) { }
            else
            {
                var userid = User.Identity.GetUserId();
                var query = from order in db.Orders where order.UserID == userid select order;

                var orderProductsList = new List<OrderAndProductsModel>();
                foreach (var order in query)
                {
                    var query2 = from prodOrder in db.ProdOrder where prodOrder.OrderID == order.OrderID select prodOrder;

                    var prodList = new List<Products>();

                    foreach (var prodOrder in query2)
                    {
                        var product = new Products();
                        product = db.Products.Single(x => x.ProductID == prodOrder.ProductID);
                        prodList.Add(product);
                        //Need to get the products, and orders, into a list of OrderAndProductsModel
                    }
                    orderProductsList.Add(new OrderAndProductsModel()
                    {
                        OrderID = order.OrderID,
                        UserID = order.UserID,
                        OrderDateTime = order.OrderDateTime,
                        OrderSent = order.OrderSent,
                        OrderSentDate = order.OrderSentDate,
                        Product = new List<Products>(prodList)
                    });
                }
                return View(orderProductsList);
            }

            var orders = db.Orders.Include(o => o.Users);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,UserID,OrderSent,OrderSentDate,OrderDateTime")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(orders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", orders.UserID);
            return View(orders);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", orders.UserID);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,UserID,OrderSent,OrderSentDate,OrderDateTime")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", orders.UserID);
            return View(orders);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orders orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
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

        public ActionResult MakeOrder()
        {
            dbMSA3Entities cont = new dbMSA3Entities();
            var productsUser = new ProductsAndUserModel();

            //Contains a list of ID's, we'll use this to find the correct products
            var cartlist = (List<int>)Session["CartList"];
            foreach (int itm in cartlist)
            {
                //Find and save the product linked to the ID
                Products obj = new Products();
                obj = cont.Products.Single(x => x.ProductID == itm);

                productsUser.Products.Add(obj);
            }

            //Establish a connection so we can find the logged in user
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //Save the user to a variable, so we can extract the needed attributes
            var user = UserManager.FindById(User.Identity.GetUserId());

            productsUser.Address = user.Adress;
            productsUser.City = user.City;
            productsUser.ZipCode = user.ZipCode;

            return View(productsUser);
        }

        public ActionResult Ordered()
        {

            //Establish a connection so we can find the logged in user
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //Save the user to a variable, so we can extract the needed attributes
            var user = UserManager.FindById(User.Identity.GetUserId());

            var order = new Orders() {
                UserID = user.Id,
                OrderDateTime = System.DateTime.Now.ToString()
            };
            db.Orders.Add(order);
            db.SaveChanges();

            var orderId = order.OrderID;

            var cartlist = (List<int>)Session["CartList"];
            foreach (int itm in cartlist)
            {
                db.ProdOrder.Add(new ProdOrder() { ProductID = itm, OrderID = orderId });
            }
            db.SaveChanges();


            return View();
        }
    }
}
