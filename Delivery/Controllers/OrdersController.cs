using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Delivery.Models;
using Newtonsoft.Json;  
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Json;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;

namespace Delivery.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Json数组对象序列化转换
        public static Object JsonToObj(String json, Type t)
        {
            try
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(t);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
                {
                    return serializer.ReadObject(ms);
                }
            }
            catch
            {
                return null;
            }
        }

        public ActionResult Announcement()
        {
            return View();
        }


        // GET: Orders
        public ActionResult Index()
        {

            var orders = db.Orders.Include(o => o.PickLocation).Include(o => o.Receiver).Include(o => o.ReceiverLocation).Include(o => o.Reward).Include(o => o.Sender);
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
            string userID=Request["UserID"];
            ViewData["userID"] = userID;
            //ViewBag.PickLocationID = new SelectList(db.Locations, "ID", "PlaceName");
            //ViewBag.ReceiverID = new SelectList(db.Users, "ID", "Account");
            //ViewBag.ReceiverLocationID = new SelectList(db.Locations, "ID", "PlaceName");
            //ViewBag.RewardID = new SelectList(db.Rewards, "ID", "Type");
            //ViewBag.SenderID = new SelectList(db.Users, "ID", "Account");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public JsonResult Create(string SenderID, string PlaceName, string Remark, string pack)
        //([Bind(Include = "SenderID,PlaceName,Remark,ExpressCompany,Description")] OrderCreateViewModel orderView)
        {

            if (ModelState.IsValid)
            {
                Locations location = new Locations();
                //location.PlaceName = orderView.PlaceName;
                //location.Remark = orderView.Remark;
                location.PlaceName = PlaceName;
                location.Remark = Remark;
                db.Locations.Add(location);
                db.SaveChanges();

                Orders order = new Orders();
                order.PickLocationID = location.ID;
                order.Status = "等待接收";
                order.SenderID = int.Parse(SenderID);
                try
                {
                    db.Orders.Add(order);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    throw new Exception(ex.Message);
                }
                /*
                catch (OptimisticConcurrencyException)//处理并发
                {
                    db.Refresh(RefreshMode.StoreWins, db.Orders);
                    db.SaveChanges();
                }*/
                
                JObject obj = (JObject)JsonConvert.DeserializeObject(pack);
                JArray list = (JArray)JsonConvert.DeserializeObject(obj["pack"].ToString());
                for (int i = 0; i < list.Count; i++)
                {
                    Packages package = new Packages();
                    package.ExpressCompany = list[i]["express"].ToString();//.ExpressCompany;
                    package.Description = list[i]["describe"].ToString();//.Description;
                    package.OrderID = order.ID;
                    db.Packages.Add(package);
                }
                db.SaveChanges();
                return Json("SUCCESS",JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }

            //ViewBag.PickLocationID = new SelectList(db.Locations, "ID", "PlaceName", orders.PickLocationID);
            //ViewBag.ReceiverID = new SelectList(db.Users, "ID", "Account", orders.ReceiverID);
            //ViewBag.ReceiverLocationID = new SelectList(db.Locations, "ID", "PlaceName", orders.ReceiverLocationID);
            //ViewBag.RewardID = new SelectList(db.Rewards, "ID", "Type", orders.RewardID);
            //ViewBag.SenderID = new SelectList(db.Users, "ID", "Account", orders.SenderID);
            //return View(orderView);
            return Json("FAIL", JsonRequestBehavior.AllowGet);
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
            ViewBag.PickLocationID = new SelectList(db.Locations, "ID", "PlaceName", orders.PickLocationID);
            ViewBag.ReceiverID = new SelectList(db.Users, "ID", "Account", orders.ReceiverID);
            ViewBag.ReceiverLocationID = new SelectList(db.Locations, "ID", "PlaceName", orders.ReceiverLocationID);
            ViewBag.RewardID = new SelectList(db.Rewards, "ID", "Type", orders.RewardID);
            ViewBag.SenderID = new SelectList(db.Users, "ID", "Account", orders.SenderID);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SenderID,ReceiverID,PickLocationID,ReceiverLocationID,RewardID,Status,Mark,Comment,PublishTime,EndTime")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PickLocationID = new SelectList(db.Locations, "ID", "PlaceName", orders.PickLocationID);
            ViewBag.ReceiverID = new SelectList(db.Users, "ID", "Account", orders.ReceiverID);
            ViewBag.ReceiverLocationID = new SelectList(db.Locations, "ID", "PlaceName", orders.ReceiverLocationID);
            ViewBag.RewardID = new SelectList(db.Rewards, "ID", "Type", orders.RewardID);
            ViewBag.SenderID = new SelectList(db.Users, "ID", "Account", orders.SenderID);
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
    }
}
