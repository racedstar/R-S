using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RioManager.Models;

namespace RioManager.Views.Tools
{
    /// <summary>
    /// zip 的摘要描述
    /// </summary>
    public class Addzip : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (HttpContext.Current.Request.QueryString.Get("t") != null && HttpContext.Current.Request.Form.Get("picSN") !=null)
            {
                string type = HttpContext.Current.Request.QueryString.Get("t").ToString();
                string[] SN = HttpContext.Current.Request.Form.Get("SN").ToString().Split(',');
                string zipPath = HttpContext.Current.Server.MapPath("~/Upload//zip//" + type);
                string downloadUrl = string.Empty;
                List<Rio_Pic> Pic = new PicModel().getZipPic(SN);
                string[] zipFileNamePath = new string[Pic.Count];                

                for (int i = 0; i < Pic.Count; i++)
                {
                    zipFileNamePath[i] = HttpContext.Current.Server.MapPath(Pic[i].PicPath) + Pic[i].PicName;                    
                }
                
                downloadUrl = App_Code.Zip.AddZip(zipFileNamePath, zipPath);

                context.Response.Write("/Upload/zip/img/" + downloadUrl);
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