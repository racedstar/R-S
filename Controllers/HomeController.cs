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
            Vw_Account rioAccount = new AccountModel().getVwAccountByID(userID);
            int.TryParse(rioAccount.SN.ToString(), out SN);

            ViewBag.userSetting = new UserIndexSettingMode().getVwUserIndexSettingBySN(SN);
            ViewBag.indexAccountCover = rioAccount.PicPath + rioAccount.PicName;
            ViewBag.albumCount = new AlbumModel().getUsertVwAlbumListByID(userID).Count;
            ViewBag.picCount = new PicModel().getUserPicByID(userID).Count;
            ViewBag.docCount = new DocModel().getDocListByID(userID).Count;

            return View();
        }


    }
}