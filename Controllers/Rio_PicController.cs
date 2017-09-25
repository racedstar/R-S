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
    public class Rio_PicController : Controller
    {
        private Entities db = new Entities();

        // GET: Rio_Pic
        #region 系統產生
        public ActionResult Index()
        {
            return View(db.Rio_Pic.ToList());
        }

        // GET: Rio_Pic/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rio_Pic rio_Pic = db.Rio_Pic.Find(id);
            if (rio_Pic == null)
            {
                return HttpNotFound();
            }
            return View(rio_Pic);
        }

        // GET: Rio_Pic/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rio_Pic/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SN,PicName,PicContent,HitCount,PicPath,CreateID,CreateDate,ModifyID,ModifyDate,IsEnable,IsDelete")] Rio_Pic rio_Pic)
        {
            if (ModelState.IsValid)
            {
                db.Rio_Pic.Add(rio_Pic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rio_Pic);
        }

        public void Insert(Rio_Pic Pic)
        {
            db.Rio_Pic.Add(Pic);
            db.SaveChanges();
        }

        // GET: Rio_Pic/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rio_Pic rio_Pic = db.Rio_Pic.Find(id);
            if (rio_Pic == null)
            {
                return HttpNotFound();
            }
            return View(rio_Pic);
        }

        // POST: Rio_Pic/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SN,PicName,PicContent,HitCount,PicPath,CreateID,CreateDate,ModifyID,ModifyDate,IsEnable,IsDelete")] Rio_Pic rio_Pic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rio_Pic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rio_Pic);
        }

        // GET: Rio_Pic/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rio_Pic rio_Pic = db.Rio_Pic.Find(id);
            if (rio_Pic == null)
            {
                return HttpNotFound();
            }
            return View(rio_Pic);
        }

        // POST: Rio_Pic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rio_Pic rio_Pic = db.Rio_Pic.Find(id);
            db.Rio_Pic.Remove(rio_Pic);
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
        public ActionResult RioPicView()
        {
            var data = new PicModel().getAllPic();

            return View(data);
        }
        #endregion

        public ActionResult ReScaling()//重製縮圖
        {
            string Message = App_Code.ImageTools.ReScaling(new PicModel().getAllPic());
            Response.Write(App_Code.JS.Alert(Message));

            return RedirectToAction("RioPicView", "Rio_Pic", new {m="E"});
        }
        
    }
}
