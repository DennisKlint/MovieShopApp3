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
    public class UsersController : Controller
    {
        private dbMSA3Entities db = new dbMSA3Entities();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserName,Email,Adress,City,ZipCode")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(users);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "UserID,UserName,Email,Adress,City,ZipCode")] Users users)
        {
            //If email is changed, verify that it is not already in use 
            //Update own database then identity database 
            dbMSA3Entities cont = new dbMSA3Entities();
            List<Users> userlist = new List<Users>();
            ViewBag.ErrorMsg = "";
            userlist = db.Users.SqlQuery("Select * from Users where Users.Email ='" + users.Email + "' AND NOT (Users.UserID = '" + users.UserID + "')").ToList();
            if (userlist.Count  > 0)
            {
                ViewBag.ErrorMsg = "The Email is already in use";
                return View();
            }
            //Update own database table Users
            try
            {
                db.Users.SqlQuery("Update Users SET Email = '" + users.Email + "',UserName = '" + users.UserName + "',Adress = '" + users.Adress + "',City = '" + users.City + "',ZipCode = '" + users.ZipCode + "' WHERE Users.UserID = '" + users.UserID + "'");
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewBag.ErrorMsg = "Unable to update database. Errormessage:" + e.Message.ToString();
                return View();
            }
            //Update the identity database 
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: Users/Delete/5
        [Authorize(Roles ="Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
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
