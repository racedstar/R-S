using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using RioManager.Models;
using RioManager.Controllers;


namespace RioManager.Views.Tools
{
    /// <summary>
    /// createAlbum 的摘要描述
    /// </summary>
    public class albumSystem : IHttpHandler, IRequiresSessionState
    {
        private Entities db = new Entities();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (HttpContext.Current.Request.QueryString.Get("State") != null && HttpContext.Current.Request.Form["imgSN"] != null && HttpContext.Current.Request.Form["frontCover"] != null && HttpContext.Current.Request.Form["Title"] != null && context.Session["UserID"] != null)
            {
                #region 變數宣告
                string[] picSN = HttpContext.Current.Request.Form["imgSN"].ToString().Split(',');
                int frontCoverSN = 0;
                int.TryParse(HttpContext.Current.Request.Form["frontCover"].ToString(), out frontCoverSN);
                string Title = string.Empty;
                Title = HttpContext.Current.Request.Form["Title"].ToString();
                string userID = context.Session["UserID"].ToString();
                int AlbumSN = 0;
                bool IsEnable = false;
                if (HttpContext.Current.Request.Form["isEnable"] != null)
                    bool.TryParse(HttpContext.Current.Request.Form["isEnable"].ToString(), out IsEnable);


                #endregion
                #region 判斷路線
                if (HttpContext.Current.Request.QueryString.Get("State") == "Create")
                { 
                    AlbumSN = AddAlbum(frontCoverSN, Title, IsEnable, userID);
                    joinAlbum(picSN, AlbumSN);
                }
                if(HttpContext.Current.Request.QueryString.Get("State") == "Update" && HttpContext.Current.Request.QueryString.Get("as") != null)
                {
                    int.TryParse(HttpContext.Current.Request.QueryString.Get("as").ToString(), out AlbumSN);
                    
                    updateAlbum(AlbumSN,frontCoverSN, Title, IsEnable, userID);
                    new AlbumModel().deleteJoinAlbum(AlbumSN);
                    joinAlbum(picSN, AlbumSN);
                }
                #endregion
            }
            else if(HttpContext.Current.Request.QueryString.Get("State") != null && HttpContext.Current.Request.QueryString.Get("State").ToString().Equals("delete"))
            {
                string[] deleteAlbumSN = HttpContext.Current.Request.Form["aSN"].ToString().Split(',');
                deleteAlbum(deleteAlbumSN);
            }
        }

        private int AddAlbum(int frontCoverSN,string Title,bool IsEnable,string userID)
        {
            Rio_Album Album = new Rio_Album();
            Album.FrontCoverSN = frontCoverSN;
            Album.AlbumName = Title;
            Album.AlbumContent = string.Empty;
            Album.Sort = 0;
            Album.HitCount = 0;
            Album.IsEnable = IsEnable;
            Album.IsDelete = false;

            DateTime timeNow = DateTime.Now;
            Album.CreateID = userID;
            Album.CreateName = userID;
            Album.CreateDate = timeNow;
            Album.ModifyID = userID;
            Album.ModifyName = userID;
            Album.ModifyDate = timeNow;                     

            new Rio_AlbumController().Create(Album);
            
            return Album.SN;
        } //新增相簿

        private void updateAlbum(int SN,int frontCoverSN, string Title,bool IsEnable, string userID)
        {
            Rio_Album Album = new AlbumModel().getAlbum(SN);
            Album.FrontCoverSN = frontCoverSN;
            Album.AlbumName = Title;
            Album.IsEnable = IsEnable;

            DateTime timeNow = DateTime.Now;
            Album.ModifyID = userID;
            Album.ModifyName = userID;
            Album.ModifyDate = timeNow;

            new Rio_AlbumController().Edit(Album);
        }//編輯相簿

        private void joinAlbum(string[] picSN,int AlbumSN)
        {
            int sort = 0;
            foreach(var item in picSN)
            {
                Rio_AlbumJoinPic joined = new Rio_AlbumJoinPic();
                int SN = 0;
                int.TryParse(item.ToString(), out SN);
                sort += 1;

                joined.AlbumSN = AlbumSN;
                joined.PicSN = SN;
                joined.Sort = sort;
                new AlbumModel().joinAlbum(joined);
            }
        }//寫入相簿與圖片關聯

        private void deleteAlbum(string[] deleteAlbumSN)//刪除相簿
        {
            int SN = 0;
            foreach (var item in deleteAlbumSN)
            {
                int.TryParse(item.ToString(), out SN);
                Rio_Album Album = new AlbumModel().getAlbum(SN);
                Album.IsDelete = true;
                new Rio_AlbumController().Edit(Album);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}