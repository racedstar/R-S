using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RioManager.Models;

namespace RioManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            int SN = 0;
            string userID = string.Empty;
            if (Request.QueryString.Get("vid") != null)
            {
                userID = Request.QueryString.Get("vid").ToString();                
            }
            SN = new AccountModel().getAccountByID(userID).SN;

            ViewBag.userSetting = new UserIndexSettingMode().getVwUserIndexSettingBySN(SN);

            return View();
        }


    }
}