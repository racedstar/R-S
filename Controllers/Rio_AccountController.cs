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
            var data = db.Rio_Account.Where(o => o.IsDelete == false).ToList();
            return View(data);
        }

        // GET: Rio_Account/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int SN = 0;
            int.TryParse(id.ToString(), out SN);
            Vw_Account rio_Account = new AccountModel().getAccount(SN);
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
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(string ID,string Password, string Name,string AccountContent, bool IsEnable)
        {
            Rio_Account rio_Account = new Rio_Account();
            string createID = string.Empty;
            if (HttpContext.Session["UserID"] != null)
            {
                createID = HttpContext.Session["UserID"].ToString();
            }
            DateTime dt = DateTime.Now;

            rio_Account.ID = ID;
            rio_Account.Name = Name;
            rio_Account.Password = App_Code.Coding.Encrypt(Password);
            rio_Account.AccountContent = AccountContent;
            rio_Account.Email = string.Empty;
            rio_Account.PicSN = 0;

            rio_Account.CreateID = createID;
            rio_Account.CreateName = createID;
            rio_Account.ModifyID = createID;
            rio_Account.ModifyName = createID;
            rio_Account.CreateDate = dt;
            rio_Account.ModifyDate = dt;

            rio_Account.IsEnable = IsEnable;
            rio_Account.IsDelete = false;

            new AccountModel().Insert(rio_Account);

            return RedirectToAction("Index");
        }

        // GET: Rio_Account/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rio_Account rio_Account = db.Rio_Account.Find(id);
            rio_Account.Password = App_Code.Coding.Decrypt(rio_Account.Password);
            if (rio_Account == null)
            {
                return HttpNotFound();
            }
            return View(rio_Account);
        }

        // POST: Rio_Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int SN,string Name,string Password, string AccountContent,int PicSN,bool IsEnable)
        {
            if (ModelState.IsValid)
            {
                Rio_Account rio_Account = db.Rio_Account.Find(SN);
                string modifyID = string.Empty;                
                DateTime dt = DateTime.Now;

                if (HttpContext.Session["UserID"] != null)
                {
                    modifyID = HttpContext.Session["UserID"].ToString();
                }

                rio_Account.Name = Name;
                rio_Account.Password = App_Code.Coding.Encrypt(Password);
                rio_Account.AccountContent = AccountContent;
                rio_Account.PicSN = PicSN;

                rio_Account.ModifyID = modifyID;
                rio_Account.ModifyName = modifyID;
                rio_Account.ModifyDate = dt;

                rio_Account.IsEnable = IsEnable;

                new AccountModel().Update(rio_Account);

            }
            return RedirectToAction("Index");
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
            //db.Rio_Account.Remove(rio_Account);
            rio_Account.IsDelete = true;
            new AccountModel().Update(rio_Account);
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
