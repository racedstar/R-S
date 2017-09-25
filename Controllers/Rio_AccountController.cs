using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RioManager.Models;

namespace RioManager.Controllers
{
    public class Rio_AccountController : Controller
    {
        private Entities db = new Entities();

        #region 系統產生
        // GET: Rio_Account
        public ActionResult Index()
        {
            return View(db.Rio_Account.ToList());
        }

        // GET: Rio_Account/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rio_Account rio_Account = db.Rio_Account.Find(id);
            if (rio_Account == null)
            {
                return HttpNotFound();
            }
            return View(rio_Account);
        }

        // GET: Rio_Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rio_Account/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SN,ID,Password,Name,Account_Content,IsEnable")] Rio_Account rio_Account)
        {
            if (ModelState.IsValid)
            {
                string ID = string.Empty;
                if (HttpContext.Session["ID"] != null)
                {
                    ID = HttpContext.Session["ID"].ToString();
                }
                DateTime dt = DateTime.Now;

                rio_Account.Password = App_Code.Coding.Encrypt(rio_Account.Password);
                rio_Account.PicSN = 0;

                rio_Account.CreateID = ID;
                rio_Account.CreateName = ID;
                rio_Account.ModifyID = ID;
                rio_Account.ModifyName = ID;
                rio_Account.CreateDate = dt;
                rio_Account.ModifyDate = dt;

                rio_Account.IsDelete = false;
                

                db.Rio_Account.Add(rio_Account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rio_Account);
        }

        // GET: Rio_Account/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rio_Account rio_Account = db.Rio_Account.Find(id);
            if (rio_Account == null)
            {
                return HttpNotFound();
            }
            return View(rio_Account);
        }

        // POST: Rio_Account/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SN,ID,Password,Name,Account_Content,PicSN,CreateID,CreateDate,ModifyID,ModifyDate,IsEnable,IsDelete")] Rio_Account rio_Account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rio_Account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rio_Account);
        }

        // GET: Rio_Account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rio_Account rio_Account = db.Rio_Account.Find(id);
            if (rio_Account == null)
            {
                return HttpNotFound();
            }
            return View(rio_Account);
        }

        // POST: Rio_Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rio_Account rio_Account = db.Rio_Account.Find(id);
            db.Rio_Account.Remove(rio_Account);
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
        #endregion

        //[HttpPost, ActionName("Login")]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(string ID, string Password)
        {
            if (HttpContext.Session["UserID"] != null)
            {
                return RedirectToAction("../Home/Index");
            }

            if(Request.HttpMethod == "POST")
            {
                var LoginCheck = new AccountModel().LoginCheck(ID, Password);
                if (ModelState.IsValid && LoginCheck)
                {
                    HttpContext.Session["UserID"] = ID;
                    HttpContext.Session["IsLogin"] = "true";
                    return RedirectToAction("../Home/Index");
                }
                ModelState.AddModelError("Password", "帳號密碼錯誤，請重新輸入");
            }
            return View();
        }        

        public void LoingChecked()
        {
            if (System.Web.HttpContext.Current.Session["UserID"] == null || System.Web.HttpContext.Current.Session["IsLogin"] == null)
            {
                System.Web.HttpContext.Current.Response.Redirect("~/Rio_Account/Login");
            }
        }

    }
}
