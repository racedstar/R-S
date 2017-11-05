using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RioManager.Models;

namespace RioManager.Views.Tools
{
    /// <summary>
    /// fileEnable 的摘要描述
    /// </summary>
    public class fileEnable : IHttpHandler
    {
        private Entities db = new Entities();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (HttpContext.Current.Request.QueryString.Get("t") != null)
            {
                switch (HttpContext.Current.Request.QueryString.Get("t").ToString())
                {
                    case "img":
                        string[] picSN = HttpContext.Current.Request.Form["SN"].ToString().Split(',');
                        changeEnablePic(picSN);
                        break;
                    case "doc":
                        string[] docSN = HttpContext.Current.Request.Form["SN"].ToString().Split(',');
                        changeEnableDoc(docSN);
                        break;
                }
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
        private void changeEnableDoc(string[] SNArray)
        {
            foreach (var data in SNArray)
            {
                int SN = 0;
                int.TryParse(data.ToString(), out SN);
                
                Rio_Doc Doc = db.Rio_Doc.Find(SN);
                if (Doc.IsEnable == true)
                {
                    Doc.IsEnable = false;
                }
                else
                {
                    Doc.IsEnable = true;
                }
                new DocModel().Update(Doc);
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