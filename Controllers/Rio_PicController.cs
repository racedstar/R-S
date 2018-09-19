using PagedList;
using System.Collections.Generic;
using RioManager.Models;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Helpers;


namespace RioManager.Controllers
{
    public class Rio_PicController : Controller
    {
        private Entities db = new Entities();

        private int pageSize = 20;

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
                db.Entry(rio_Pic).State = System.Data.Entity.EntityState.Modified;
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
        #endregion

        public ActionResult RioPicView(int? page)
        {
            string ID = string.Empty;
            string title = "PicView";
            string mode = "V";
            bool isUser = false;

            if (Request.QueryString.Get("m") != null)
            {
                if (Request.QueryString.Get("m").Equals("E"))
                {
                    title = "PicEdit";
                    mode = "E";
                }
            }
            ViewBag.title = title;

            ClassNameModel cn = ClassNameModel.getClassName("pic");
            ViewBag.className = cn;

            #region getData
            List<Rio_Pic> data = new List<Rio_Pic>();
            if (Session["UserID"] != null)
            {
                ID = Session["UserID"].ToString();
                data = new PicModel().getUserAllPicByID(ID);
            }

            if (Request.QueryString.Get("vid") != null)
            {
                ID = Request.QueryString.Get("vid");
                data = new PicModel().getUserPicEnableByID(ID);                
            }

            if (Session["UserID"] != null && Request.QueryString.Get("vid") != null)
            {
                if (Session["UserID"].ToString().Equals(Request.QueryString.Get("vid")))
                {
                    ID = Session["UserID"].ToString();
                    isUser = true;
                    data = new PicModel().getUserAllPicByID(ID);
                }
            }
            else
            {
                return RedirectToAction("Login", "Rio_Account", null);
            }

            var pageNumeber = page ?? 1;
            var pageData = data.ToPagedList(pageNumeber, pageSize);
            ViewBag.vid = ID;
            ViewBag.mode = mode;
            ViewBag.isUser = isUser;
            #endregion

            return View(pageData);
        }

        public ActionResult ReScaling()//重製縮圖
        {
            if(Session["UserID"] != null)
            { 
                string ID = Session["UserID"].ToString();
                if (new PicModel().getUserAllPicByID(ID) != null)
                { 
                    string Message = App_Code.ImageTools.ReScaling(new PicModel().getUserAllPicByID(ID));
                    Response.Write(App_Code.JS.Alert(Message));
                }
            }
            return RedirectToAction("RioPicView", "Rio_Pic", new {m="E"});
        }
                
        public ActionResult deleteFile(string[] SN)
        {
            deletePic(SN);

            return Content("Delete Success");
        }

        public ActionResult fileEnable(string[] SN)
        {
            changeEnablePic(SN);

            return Content("Enable Change Successs");
        }

        private void deletePic(string[] SNArray)
        {
            foreach (var data in SNArray)
            {
                int SN = 0;
                int.TryParse(data.ToString(), out SN);

                //刪除實體檔案
                Rio_Pic Pic = db.Rio_Pic.Find(SN);
                if (System.IO.File.Exists(Server.MapPath(Pic.PicPath + "\\" + Pic.PicName)))
                {
                    System.IO.File.Delete(Server.MapPath(Pic.PicPath + "\\" + Pic.PicName));
                }

                //刪除實體檔案縮圖
                if (System.IO.File.Exists(Server.MapPath(Pic.PicPath + "\\Scaling\\" + Pic.PicName)))
                {
                    System.IO.File.Delete(Server.MapPath(Pic.PicPath + "\\Scaling\\" + Pic.PicName));
                }

                //資料庫更新刪除標記           
                new PicModel().Delete(Pic);
            }
        }

        private void changeEnablePic(string[] SNArray)
        {
            foreach (var data in SNArray)
            {
                int SN = 0;
                int.TryParse(data.ToString(), out SN);


                Rio_Pic Pic = db.Rio_Pic.Find(SN);
                if (Pic.IsEnable == true)
                {
                    Pic.IsEnable = false;
                }
                else
                {
                    Pic.IsEnable = true;
                }
                new PicModel().Update(Pic);
            }
        }
    }
}
