using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RioManager.Models;
using PagedList;

namespace RioManager.Controllers
{
    public class Rio_NoticeController : Controller
    {
        // GET: Rio_Notice
        public ActionResult Index(int? page)
        {
            if (Session["UserSN"] != null)
            {
                int userSN = 0;
                int.TryParse(Session["UserSN"].ToString(), out userSN);                
                List<Vw_Notice> noticeList = new NoticeModel().getNoticeListByTrackSN(userSN);

                new NoticeModel().updateNotReadNoticeByTrackSN(userSN);
                Session["notReadNoticeCount"] = "0";

                var pageNumeber = page ?? 1;
                var pageData = noticeList.ToPagedList(pageNumeber, 25);
            
                ViewBag.notice = noticeList.ToPagedList(pageNumeber, 25);
                return View(pageData);
            }
            else
            {
                return RedirectToAction("Login", "Rio_Account", null);
            }
        }
    }
}