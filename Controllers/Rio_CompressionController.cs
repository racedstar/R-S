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
        // GET: Rio_Compression
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CompressionView(int? page)
        {
            int pageSize = 20;
            int userSN = 0;
            string ID = string.Empty;
            List<Rio_Compression> data = new List<Rio_Compression>();

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
                }
            }
            else
            {
                return RedirectToAction("Login", "Rio_Account", null);
            }

            var pageNumeber = page ?? 1;
            var pageData = data.ToPagedList(pageNumeber, pageSize);            

            return View(pageData);
        }
    }
}