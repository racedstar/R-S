using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RioManager.Models;
using PagedList;

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
                db.Entry(rio_Album).State = System.Data.Entity.EntityState.Modified;
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

        public ActionResult RioAlbumView(int? page)
        {
            string ID = string.Empty;
            bool isUser = false;
            List<Vw_Album> data = new List<Vw_Album>();

            if (Session["UserID"] != null)
            {
                ID = Session["UserID"].ToString();
                data = new AlbumModel().getUserAllVwAlbumList(ID);
            }

            if (Request.QueryString.Get("vid") != null)
            {
                ID = Request.QueryString.Get("vid").ToString();
                data = new AlbumModel().getUsertVwAlbumEnableListByID(ID);
            }

            if (Session["UserID"] != null && Request.QueryString.Get("vid") != null)
            {
                if (Session["UserID"].ToString().Equals(Request.QueryString.Get("vid")))
                {
                    ID = Session["UserID"].ToString();
                    data = new AlbumModel().getUserAllVwAlbumList(ID);
                    isUser = true;
                }
            }
            else
            {
                return RedirectToAction("Login", "Rio_Account", null);
            }

            ViewBag.vid = ID;
            ViewBag.isUser = isUser;

            var pageNumber = page ?? 1;
            var pageData = data.ToPagedList(pageNumber, 20);
            ViewBag.className = ClassNameModel.getClassName("albumView");

            return View(pageData);
        }

        public ActionResult RioAlbumContent(int? SN, int? page)
        {
            int aSN = SN ?? 0;                                                           
            int pageNumber = page ?? 1;
            string userID = string.Empty;
            bool isUser = false;
            Vw_Album va = new Vw_Album();            

            if (aSN == 0 || Request.QueryString.Get("vid") == null)
            {
                return RedirectToAction("Login", "Rio_Account", null);
            }

            if (Session["UserID"] != null)
            {
                userID = Session["UserID"].ToString();
            }           

            if (Request.QueryString.Get("vid").Equals(userID))
            {
                isUser = true;
            }

            va = new AlbumModel().getVwAlbum(aSN);

            if (isUser && va.IsEnable == false)
            {
                return RedirectToAction("RioAlbumView", "Rio_Album", new { vid = Request.QueryString.Get("vid") });
            }

            
            ViewBag.aSN = aSN;
            ViewBag.VwAlbum = va;
            ViewBag.isUser = isUser;
            ViewBag.vid = Request.QueryString.Get("vid");
            ViewBag.className = ClassNameModel.getClassName("albumContent");
            ViewBag.getJoinPic = new AlbumJoinPicModel().getUpdateJoinPic(aSN).ToPagedList(pageNumber, 20);                            
            return View(db.Rio_Album.ToList());
        }

        public ActionResult CreateAlbum(int? s, int? aSN)
        {
            string ID = string.Empty;
            int status = s ?? 0;
            int albumSN = aSN ?? 0;
            if (Session["UserID"] != null)
            {
                ID = Session["UserID"].ToString();
            }
            if (status == 0)//建立相簿時取得圖片
            { 
                ViewBag.getNotJoinPic = new PicModel().getUserPicEnableByID(ID);
            }

            if (status == 1 && albumSN != 0)//編輯相簿時取得圖片
            {                    
                ViewBag.VwAlbum = new AlbumModel().getVwAlbum(albumSN);//相簿資料
                ViewBag.getJoinPic = new AlbumJoinPicModel().getUpdateJoinPic(albumSN);//已加入相簿的圖片
                ViewBag.getNotJoinPic = new AlbumJoinPicModel().getUpdateNotJoinPic(albumSN, ID).OrderByDescending(o => o.CreateDate);//未加入相簿的圖片
            }

            ViewBag.aSN = albumSN;            
            return View();
        }

        public ActionResult DeleteAlbum(int? SN)
        {
            int aSN = SN ?? 0;
            string vid = string.Empty;
            if (Request.QueryString.Get("vid") != null)
            {
                vid = Request.QueryString.Get("vid").ToString();
            }
            if (aSN != 0)
            {
                Rio_Album Album  = new AlbumModel().getAlbum(aSN);
                Album.IsDelete = true;
                new AlbumModel().Delete(Album);
            }
            return Redirect("RioAlbumView?vid=" + vid);
        }

        public ActionResult ZipAlbum()
        {
            if (Request.QueryString.Get("SN") != null)
            {
                int SN = 0;
                int.TryParse(Request.QueryString.Get("SN").ToString(), out SN);
                string ziPath = Server.MapPath("~/Upload//zip//Album");
                string zipName = string.Empty;
                List<Vw_AlbumJoinPic> Album = new RioManager.Models.AlbumJoinPicModel().getUpdateJoinPic(SN);
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
