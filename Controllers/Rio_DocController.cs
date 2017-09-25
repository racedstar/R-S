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
    public class Rio_DocController : Controller
    {
        private Entities db = new Entities();

        // GET: Rio_Doc
        #region 系統產生
        public ActionResult Index()
        {
            return View(db.Rio_Doc.ToList());
        }

        // GET: Rio_Doc/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rio_Doc rio_Doc = db.Rio_Doc.Find(id);
            if (rio_Doc == null)
            {
                return HttpNotFound();
            }
            return View(rio_Doc);
        }

        // GET: Rio_Doc/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rio_Doc/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SN,DocName,DocContent,DocPath,HitCount,Extension,CreateID,CreateName,CreateDate,ModifyId,ModifyName,ModifyDate,IsEnable,IsDelete")] Rio_Doc rio_Doc)
        {
            if (ModelState.IsValid)
            {
                db.Rio_Doc.Add(rio_Doc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rio_Doc);
        }

        // GET: Rio_Doc/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rio_Doc rio_Doc = db.Rio_Doc.Find(id);
            if (rio_Doc == null)
            {
                return HttpNotFound();
            }
            return View(rio_Doc);
        }

        // POST: Rio_Doc/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SN,DocName,DocContent,DocPath,HitCount,Extension,CreateID,CreateName,CreateDate,ModifyId,ModifyName,ModifyDate,IsEnable,IsDelete")] Rio_Doc rio_Doc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rio_Doc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rio_Doc);
        }

        // GET: Rio_Doc/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rio_Doc rio_Doc = db.Rio_Doc.Find(id);
            if (rio_Doc == null)
            {
                return HttpNotFound();
            }
            return View(rio_Doc);
        }

        // POST: Rio_Doc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rio_Doc rio_Doc = db.Rio_Doc.Find(id);
            db.Rio_Doc.Remove(rio_Doc);
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

        public ActionResult RioDocView()
        {
            ViewBag.getAllDoc = new DocModel().getAllDoc();
            return View(db.Rio_Doc.ToList());
        }       
    }
}
