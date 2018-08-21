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

            if (rioAccount != null)
            { 
                int.TryParse(rioAccount.SN.ToString(), out SN);                

                //使用者設定
                ViewBag.userSetting = new UserIndexSettingMode().getVwUserIndexSettingBySN(SN);
                ViewBag.indexAccountCover = rioAccount.PicPath + rioAccount.PicName;

                //系統相簿、圖片、文件總數
                ViewBag.albumCount = new AlbumModel().getUsertVwAlbumEnableListByID(userID).Where(o => o.IsEnable == true).Count();
                ViewBag.picCount = new PicModel().getUserPicEnableByID(userID).Count();
                ViewBag.docCount = new DocModel().getUserDocEnableListByID(userID).Count();

                //系統預覽
                ViewBag.preViewAlbum = new AlbumModel().getPreViewAlbumListByID(userID);
                ViewBag.preViewPic = new PicModel().getPreViewPicListByID(userID);
                ViewBag.preViewDoc = new DocModel().getPreviewDocListByID(userID);
                               
                if (Session["UserSN"] != null)
                {
                    int userSN = 0;
                    int.TryParse(Session["UserSN"].ToString(), out userSN);

                    //是否Track
                    ViewBag.userTrack = new UserTrackModel().getUserTrackBySN(userSN, SN);

                    //未閱讀通知數量
                    Session["notReadNoticeCount"] = new NoticeModel().getNotReadNoticeCountByTrackSN(userSN);
                }
            }
            return View();
        }


    }
}