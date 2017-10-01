﻿using System;
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
    public class Rio_AlbumController : Controller
    {
        private Entities db = new Entities();

        // GET: Rio_Album
        #region 系統產生
        public ActionResult Index()
        {
            return View(db.Rio_Album.ToList());
        }

        // GET: Rio_Album/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rio_Album rio_Album = db.Rio_Album.Find(id);
            if (rio_Album == null)
            {
                return HttpNotFound();
            }
            return View(rio_Album);
        }

        // GET: Rio_Album/Create
        public ActionResult Create()
        {
            return View();
        }
        

        // POST: Rio_Album/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SN,FrontCoverSN,Title,Album_Countent,Sort,HitCount,CreateID,CreateDate,ModifyID,ModifyDate,IsEnable,IsDelete")] Rio_Album rio_Album)
        {
            if (ModelState.IsValid)
            {
                db.Rio_Album.Add(rio_Album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rio_Album);
        }

        // GET: Rio_Album/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rio_Album rio_Album = db.Rio_Album.Find(id);
            if (rio_Album == null)
            {
                return HttpNotFound();
            }
            return View(rio_Album);
        }

        // POST: Rio_Album/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SN,FrontCoverSN,Title,Album_Countent,Sort,HitCount,CreateID,CreateDate,ModifyID,ModifyDate,IsEnable,IsDelete")] Rio_Album rio_Album)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rio_Album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rio_Album);
        }

        // GET: Rio_Album/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rio_Album rio_Album = db.Rio_Album.Find(id);
            if (rio_Album == null)
            {
                return HttpNotFound();
            }
            return View(rio_Album);
        }

        // POST: Rio_Album/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rio_Album rio_Album = db.Rio_Album.Find(id);
            db.Rio_Album.Remove(rio_Album);
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

        public ActionResult RioAlbumView()
        {
            var data = (from o in db.Vw_Album
                        where o.IsDelete == false
                        select o).ToList();
            ViewBag.VwAlbum = data;
            return View(db.Rio_Album.ToList());
        }

        public ActionResult RioAlbumContent()
        {
            if (Request.QueryString.Get("as") != null)
            {
                if (Request.QueryString.Get("as") != null)
                {
                    int aSN = 0;
                    int.TryParse(Request.QueryString.Get("as").ToString(), out aSN);

                    ViewBag.VwAlbum = new AlbumModel().getVwAlbum(aSN);
                    ViewBag.getJoinPic = new AlbumModel().getUpdateJoinPic(aSN);                    
                }
            }
            return View(db.Rio_Album.ToList());
        }

        public ActionResult CreateAlbum()
        {
            if(Request.QueryString.Get("s") != null)
            { 
                if(Request.QueryString.Get("s").ToString() == "0")
                    ViewBag.getNotJoinPic = new PicModel().getAllPic();

                if(Request.QueryString.Get("s").ToString() == "1" && Request.QueryString.Get("as") != null)
                {
                    int aSN = 0;
                    int.TryParse(Request.QueryString.Get("as").ToString(), out aSN);

                    ViewBag.VwAlbum = new AlbumModel().getVwAlbum(aSN);
                    ViewBag.getJoinPic = new AlbumModel().getUpdateJoinPic(aSN);
                    ViewBag.getNotJoinPic = new AlbumModel().getUpdateNotJoinPic(aSN);
                }
            }
            return View(db.Rio_Album.ToList());
        }        

        public ActionResult ZipAlbum()
        {
            if (Request.QueryString.Get("SN") != null)
            {
                int SN = 0;
                int.TryParse(Request.QueryString.Get("SN").ToString(), out SN);
                string ziPath = Server.MapPath("~/Upload//zip//Album");
                string zipName = string.Empty;
                List<Vw_AlbumJoinPic> Album = new RioManager.Models.AlbumModel().getUpdateJoinPic(SN);
                List<string> picName  = new List<string>();

                foreach (var item in Album)
                {
                    picName.Add(Server.MapPath(item.PicPath) + item.PicName);
                }

                zipName = App_Code.Zip.AddZip(picName, ziPath);

                return File("/Upload/zip/Album/" + zipName , "zip" ,Album[0].AlbumName + ".zip");
            }
            return Redirect("RioAlbumView");
        }
    }
}