using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Delivery.Models;

namespace Delivery.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/PersonalSetting
        public ActionResult PersonalSetting()
        {         
            if (Session["LoginId"].ToString() == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int userID = int.Parse(Session["LoginId"].ToString());
            Users user = db.Users.Find(userID);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public JsonResult AddressGet()
        {
            if (Session["LoginId"].ToString() == null)
            {
                return Json("FAIL", JsonRequestBehavior.AllowGet);
            }
            int userID = int.Parse(Session["LoginId"].ToString());
            Users user = db.Users.Find(userID);
            //ViewBag.ReceiverLocationID = new SelectList(db.Locations, "ID", "PlaceName", orders.ReceiverLocationID);
            var list= db.Addresses.Include(a => a.Address).Where(a => a.UserID==userID);
            List<string> resultList = new List<string>();
            if (list.Count() > 0)
            {
                foreach (var item in list)
                {
                    resultList.Add(item.Address.PlaceName);
                }
            }
            JsonResult result = new JsonResult();
            result.Data = resultList;
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Save(string[] addresses,string phone )
        {
            if (Session["LoginId"].ToString()!=null)
            {
                int userID = int.Parse(Session["LoginId"].ToString());
                Users user = db.Users.Find(userID);
                user.PhoneNumber = phone;
                db.Entry(user).State = EntityState.Modified;
                var addr = db.Addresses.Where(a => a.UserID == userID);
                foreach(var item in addr)
                {
                    db.Addresses.Remove(item);
                }
                for (int i = 0; i < addresses.Length; i++) {
                    var addrStr = addresses[i];
                    var loc = db.Locations.FirstOrDefault(l => l.PlaceName == addrStr && l.Remark == null);
                    var location = new Locations();
                    if (loc == null)
                    {                     
                        location.PlaceName = addresses[i];
                        db.Locations.Add(location);
                        db.SaveChanges();
                    }
                    var address = new Addresses();
                    address.UserID = userID;
                    if (loc == null)
                    {
                        address.AddressesID = location.ID;
                    }
                    else { address.AddressesID = loc.ID; }
                    db.Addresses.Add(address);
                    db.SaveChanges();
                }
                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
            }
            return Json("FAIL", JsonRequestBehavior.AllowGet);
        }

        // GET: Users/Details/5
        public ActionResult Details()
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Users users = db.Users.Find(id);
            //if (users == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Account,Password,Name,Sex,Grade,Degree,PhoneNumber")] Users users)
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
        public ActionResult Edit(int? id)
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
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Account,Password,Name,Sex,Grade,Degree,PhoneNumber")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
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
        public ActionResult DeleteConfirmed(int id)
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
