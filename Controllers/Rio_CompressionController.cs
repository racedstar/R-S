using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RioManager.Models;
using System.Web.Mvc;
using PagedList;

namespace RioManager.Controllers
{
    public class Rio_CompressionController : Controller
    {
        private Entities db = new Entities();
        // GET: Rio_Compression
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CompressionView(int? page)
        {
            int pageSize = 20;
            int userSN = 0;
            bool isUser = false;
            string ID = string.Empty;
            string mode = "V";
            string title = "CompressionView";
            List<Rio_Compression> data = new List<Rio_Compression>();

            if(Request.QueryString.Get("m") != null)
            {
                if (Request.QueryString.Get("m").Equals("E"))
                {
                    title = "CompressionEdit";
                    mode = "E";
                }

            }

            if (Session["UserID"] != null)
            {
                int.TryParse(Session["UserSN"].ToString(), out userSN);
                data = new CompressionModel().getCompressionByUserSN(userSN);
            }

            if (Request.QueryString.Get("vid") != null)
            {
                ID = Request.QueryString.Get("vid");
                userSN = new AccountModel().getAccountByID(ID).SN;
                data = new CompressionModel().getCompressionClientByUserSN(userSN);
            }

            if (Request.QueryString.Get("vid") != null && Session["UserID"] != null)
            {
                if (Request.QueryString.Get("vid") == Session["UserID"].ToString())
                {
                    int.TryParse(Session["UserSN"].ToString(), out userSN);
                    data = new CompressionModel().getCompressionByUserSN(userSN);
                    isUser = true;
                }
            }
            else
            {
                return RedirectToAction("Login", "Rio_Account", null);
            }

            ViewBag.vid = ID;
            ViewBag.mode = mode;
            ViewBag.title = title;
            ViewBag.isUser = isUser;
            ViewBag.className = ClassNameModel.getClassName("compression");

            var pageNumeber = page ?? 1;
            var pageData = data.ToPagedList(pageNumeber, pageSize);            

            return View(pageData);
        }

        public ActionResult fileEnable(string[] SN)
        {
            changeEnableCompression(SN);

            return Content("Enable Change Success");
        }

        private void deleteCompression(string[] SNArray)
        {
            foreach (var data in SNArray)
            {
                int SN = 0;
                int.TryParse(data.ToString(), out SN);

                //刪除實體檔案
                Rio_Compression CF = db.Rio_Compression.Find(SN);
                if (System.IO.File.Exists(Server.MapPath(CF.Path + "\\" + CF.Name)))
                {
                    System.IO.File.Delete(Server.MapPath(CF.Path + "\\" + CF.Name));
                }

                //資料庫更新刪除標記           
                CF.IsDelete = true;
                db.SaveChanges();
            }
        }

        private void changeEnableCompression(string[] SNArray)
        {
            foreach (var data in SNArray)
            {
                int SN = 0;
                int.TryParse(data.ToString(), out SN);

                Rio_Compression CF = db.Rio_Compression.Find(SN);
                if (CF.IsEnable == true)
                {
                    CF.IsEnable = false;
                }
                else
                {
                    CF.IsEnable = true;
                }
                new CompressionModel().Update(CF);
            }
        }
    }
}