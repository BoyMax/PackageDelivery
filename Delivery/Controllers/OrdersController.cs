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
            var orders = db.Orders.Include(o => o.PickLocation).Include(o => o.Receiver).Include(o => o.ReceiverLocation).Include(o => o.Reward).Include(o => o.Sender).Where(o => o.Status == "等待接收"|| o.Status =="待选择");
            return View(orders.ToList());
        }


        // GET: Orders
        public ActionResult Index()
        {
            var userId = int.Parse(Session["LoginId"].ToString());
            var orders = db.Orders.Include(o => o.PickLocation).Include(o => o.Receiver).Include(o => o.ReceiverLocation).Include(o => o.Reward).Include(o => o.Sender).Where(o => o.SenderID == userId);
            return View(orders.ToList());
        }


        public ActionResult ReceiveIndex()
        {
            var userId = int.Parse(Session["LoginId"].ToString());
            var competitor = db.OrderCompetitors.Where(oc => oc.UserID == userId);
            var orders = db.Orders.Include(o => o.PickLocation).Include(o => o.Receiver).Include(o => o.ReceiverLocation).Include(o => o.Reward).Include(o => o.Sender).Where(o => db.OrderCompetitors.Where(oc => (oc.UserID == userId)).Select(oc => oc.OrderID).Contains(o.ID) && o.Status=="待选择" || o.ReceiverID==userId);
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
        public JsonResult Create(string PlaceName, string Remark, string pack,string rewardType,string money,string rewardRmk)
        //([Bind(Include = "SenderID,PlaceName,Remark,ExpressCompany,Description")] OrderCreateViewModel orderView)
        {

            if (ModelState.IsValid)
            {
                var loc = db.Locations.FirstOrDefault(u => u.PlaceName == PlaceName && u.Remark==Remark);
                int locID;
                if (loc == null)
                {
                    Locations location = new Locations();
                    //location.PlaceName = orderView.PlaceName;
                    //location.Remark = orderView.Remark;
                    location.PlaceName = PlaceName;
                    location.Remark = Remark;
                    db.Locations.Add(location);
                    db.SaveChanges();
                    locID = location.ID;
                }
                else
                {
                    locID = loc.ID;
                }
                int loginID = int.Parse(Session["LoginId"].ToString();
                
                var addr = db.Addresses.FirstOrDefault(a => a.AddressesID == locID && a.UserID == loginID);
                if (addr == null)
                {
                    Addresses address = new Addresses();
                    address.UserID = loginID;
                    address.AddressesID = locID;
                    db.Addresses.Add(address);
                    db.SaveChanges();
                }
                Rewards reward = new Rewards();
                if (rewardType.Equals("0"))
                {
                    reward.Type = "现金";
                    reward.Money = int.Parse(money);
                }
                else
                {
                    reward.Type = "物品";
                    reward.Remark = rewardRmk;
                }
                db.Rewards.Add(reward);
                db.SaveChanges();

                Orders order = new Orders();
                order.PickLocationID = locID;
                order.Status = "等待接收";
                order.SenderID = loginID;
                order.RewardID = reward.ID;
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
            ViewBag.PickLocationID = new SelectList(db.Addresses.Include(a => a.Address).Where(a => a.UserID == orders.SenderID), "AddressesID", "Address.PlaceName", orders.PickLocationID);
            //ViewBag.ReceiverID = new SelectList(db.Users, "ID", "Account", orders.ReceiverID);
            //ViewBag.ReceiverLocationID = new SelectList(db.Locations, "ID", "PlaceName", orders.ReceiverLocationID);
            ViewBag.RewardID = new SelectList(db.Rewards, "ID", "Type", orders.RewardID);
            //ViewBag.SenderID = new SelectList(db.Users, "ID", "Account", orders.SenderID);
            Rewards reward = db.Rewards.Find(orders.RewardID);
            ViewBag.RewardType = reward.Type;
            ViewBag.RewardRemark = reward.Remark;
            ViewBag.Money = reward.Money;
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Edit(string ID, string location, string type, string money, string remark)
           // [Bind(Include = "ID,PickLocationID,Reward.Type,Reward.Money,Reward.Remark")] Orders orders)
        {
            Orders o = new Orders();
            if (ModelState.IsValid)
            {
                //db.Entry(orders).State = EntityState.Modified; //该方法直接根据orders的Model修改，导致senderId等值均为null

                o = db.Orders.Find(int.Parse(ID));
                o.PickLocationID = int.Parse(location);
                o.PublishTime = DateTime.Now;
                Rewards r=db.Rewards.Find(o.RewardID);
                r.Type = type;
                r.Money = int.Parse(money);
                r.Remark = remark;
               
                db.Entry(r).State = EntityState.Modified;
                db.Entry(o).State = EntityState.Modified;               
            }
            ViewBag.PickLocationID = new SelectList(db.Locations, "ID", "PlaceName", int.Parse(location));
            //ViewBag.ReceiverID = new SelectList(db.Users, "ID", "Account", orders.ReceiverID);
            //ViewBag.ReceiverLocationID = new SelectList(db.Locations, "ID", "PlaceName", orders.ReceiverLocationID);
            ViewBag.RewardID = new SelectList(db.Rewards, "ID", "Type", o.RewardID);
            //ViewBag.SenderID = new SelectList(db.Users, "ID", "Account", orders.SenderID);
            var reward = db.Rewards.Find(o.RewardID);
            ViewBag.RewardType = type;
            ViewBag.RewardRemark = remark;
            ViewBag.Money = money;
            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
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


        [HttpPost]
        public JsonResult DeleteOrder(string id)
        {
            int ID = int.Parse(id);
            Orders order = db.Orders.Find(ID);
            order.Status = "已删除";           
            db.Entry(order).State = EntityState.Modified;
            int result=db.SaveChanges();
            if (result == 0)
            {
                return Json("FAIL", JsonRequestBehavior.AllowGet);
            }
            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetInfo()
        {
            var res = new JsonResult();
            int userid = int.Parse(Session["LoginId"].ToString());
            Users user = db.Users.Find(userid);
            var person = new { name = user.Name, phone = user.PhoneNumber };
            res.Data = person;//返回单个对象；  
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。  
            return res;
        }

        [HttpPost]
        public JsonResult Apply(string address, string orderId)
        {
            var res = new JsonResult();
            int userid = int.Parse(Session["LoginId"].ToString());
            int locId;
            var oid = int.Parse(orderId);
            var orderCompetitor = db.OrderCompetitors.FirstOrDefault(u => u.OrderID == oid && u.UserID == userid);
            if (orderCompetitor == null) {
                var loc = db.Locations.FirstOrDefault(u => u.PlaceName == address);
                if (loc == null)
                {
                    Locations location = new Locations();
                    location.PlaceName = address;
                    db.Locations.Add(location);
                    db.SaveChanges();
                    locId = location.ID;
                    //Addresses myAddress = new Addresses();
                    //myAddress.UserID = userid;
                    //myAddress.AddressesID = location.ID;
                }
                else
                {
                    //Addresses myAddress = new Addresses();
                    //myAddress.UserID = userid;
                    //myAddress.AddressesID = loc.ID;  
                    locId = loc.ID;
                }
                Orders order = db.Orders.Find(oid);
                if (order.Status == "等待接收")
                {
                    order.Status = "待选择";
                    db.Entry(order).State = EntityState.Modified;
                }
                
                OrderCompetitors oc = new OrderCompetitors();
                oc.OrderID = oid;
                oc.UserID = userid;
                oc.LocationID = locId;
                db.OrderCompetitors.Add(oc);
                int result=db.SaveChanges();
                if (result != 0)
                {
                    res.Data = "SUCCESS";
                }
                else
                {
                    res.Data = "FAIL";
                }
            }
            else
            {
                res.Data = "您已申请！请等待发件人选择!";
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。  
            return res;
        }

        [HttpPost]
        public JsonResult GetCompetitors(string orderId)
        {
            var res = new JsonResult();
            int oid = int.Parse(orderId);
            var oc = db.OrderCompetitors.FirstOrDefault(c => c.OrderID == oid);
            if (oc==null)
            {
                res.Data = "FAIL";
            }
            else
            {
                var competitors = db.OrderCompetitors.Include(o => o.User).Where(o => o.OrderID == oid);
                /*页面显示多个代收人信息....明天完成哟~*/               
                ViewBag.competitors=competitors.ToList();
                var list = competitors.ToList();
                JArray json = new JArray();
                for(int i=0;i<list.Count;i++)
                {
                    JObject obj = new JObject();
                    obj.Add("id", list[i].UserID);
                    obj.Add("name", list[i].User.Name);
                    obj.Add("account", list[i].User.Account);
                    obj.Add("phone", list[i].User.PhoneNumber);
                    obj.Add("address", list[i].Location.PlaceName);
                    //obj.Add("remark", list[i].Location.Remark);
                    json.Add(obj);
                }
                res.Data = json.ToString();
                //return Json(list);
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。  
            return res;
        }

        [HttpPost]
        public JsonResult ConfirmReceivers(string userId,string orderId)
        {
            int uid = int.Parse(userId);
            int oid = int.Parse(orderId);
            var competitor = db.OrderCompetitors.FirstOrDefault(oc => oc.OrderID == oid && oc.UserID == uid);
            Orders order = db.Orders.Find(oid);
            order.ReceiverID = uid;
            order.ReceiverLocationID = competitor.LocationID;
            order.Status = "接收中";
            db.Entry(order).State = EntityState.Modified;
            int result = db.SaveChanges();
            if (result == 0)
            {
                return Json("FAIL", JsonRequestBehavior.AllowGet);
            }
            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AcceptedOrder(string id)
        {
            int oid = int.Parse(id);
            Orders order = db.Orders.Find(oid);
            order.Status = "已代收";
            db.Entry(order).State = EntityState.Modified;
            int result = db.SaveChanges();
            if (result == 0)
            {
                return Json("FAIL", JsonRequestBehavior.AllowGet);
            }
            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConfirmOrder(string id)
        {
            int oid = int.Parse(id);
            Orders order = db.Orders.Find(oid);
            order.Status = "订单完成";
            order.EndTime= DateTime.Now; 
            db.Entry(order).State = EntityState.Modified;
            int result = db.SaveChanges();
            if (result == 0)
            {
                return Json("FAIL", JsonRequestBehavior.AllowGet);
            }
            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getRcvrName(string id)
        {
            int oid = int.Parse(id);
            Orders order = db.Orders.Find(oid);
            string name = order.Receiver.Name;    
            if(name==null)
            {
                name = " ";
            }       
            return Json(name, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public JsonResult commentOrder(string id,string mark,string remark)
        {
            int oid = int.Parse(id);
            Orders order = db.Orders.Find(oid);
            string name = order.Receiver.Name;
            order.Mark = int.Parse(mark);
            order.Comment = remark;
            db.Entry(order).State = EntityState.Modified;
            int result=db.SaveChanges();
            if (result == 0)
            {
                return Json("FAIL", JsonRequestBehavior.AllowGet);
            }
            return Json("SUCCESS", JsonRequestBehavior.AllowGet);

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
