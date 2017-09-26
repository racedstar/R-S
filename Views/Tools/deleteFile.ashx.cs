using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using RioManager.Models;

namespace RioManager.Views.Tools
{
    /// <summary>
    /// deleteFile 的摘要描述
    /// </summary>
    public class deleteFile : IHttpHandler
    {
        private Entities db = new Entities();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if(HttpContext.Current.Request.QueryString.Get("t") != null)
            { 
                switch (HttpContext.Current.Request.QueryString.Get("t").ToString())
                {
                    case "img":
                        string[] picSN = HttpContext.Current.Request.Form["SN"].ToString().Split(',');
                        deletePic(picSN);
                        break;
                    case "doc":
                        string[] docSN = HttpContext.Current.Request.Form["SN"].ToString().Split(',');
                        deleteDoc(docSN);
                        break;
                }


            }
        }
        private void deletePic(string[] SNArray)
        {
            foreach (var data in SNArray)
            {
                int SN = 0;
                int.TryParse(data.ToString(), out SN);

                //刪除實體檔案
                Rio_Pic Pic = db.Rio_Pic.Find(SN);
                if (File.Exists(HttpContext.Current.Server.MapPath(Pic.PicPath + "\\" + Pic.PicName)))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(Pic.PicPath + "\\" + Pic.PicName));
                }

                //刪除實體檔案縮圖
                if (File.Exists(HttpContext.Current.Server.MapPath(Pic.PicPath + "\\Scaling\\" + Pic.PicName)))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(Pic.PicPath + "\\Scaling\\" + Pic.PicName));
                }

                //資料庫更新刪除標記           
                Pic.IsDelete = true;
                db.SaveChanges();
            }
        }
        private void deleteDoc(string[] SNArray)
        {
            foreach (var data in SNArray)
            {
                int SN = 0;
                int.TryParse(data.ToString(), out SN);

                //刪除實體檔案
                Rio_Doc Doc = db.Rio_Doc.Find(SN);
                if (File.Exists(HttpContext.Current.Server.MapPath(Doc.DocPath + "\\" + Doc.DocName)))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(Doc.DocPath + "\\" + Doc.DocName));
                }

                //刪除實體檔案縮圖
                if (File.Exists(HttpContext.Current.Server.MapPath(Doc.DocPath + "\\Scaling\\" + Doc.DocName)))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(Doc.DocPath + "\\Scaling\\" + Doc.DocName));
                }

                //資料庫更新刪除標記           
                Doc.IsDelete = true;
                db.SaveChanges();
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