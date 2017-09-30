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
            if (HttpContext.Current.Request.QueryString.Get("t") != null && HttpContext.Current.Request.Form.Get("SN") !=null)
            {
                string type = HttpContext.Current.Request.QueryString.Get("t").ToString();
                string[] SN = HttpContext.Current.Request.Form.Get("SN").ToString().Split(',');
                string zipPath = HttpContext.Current.Server.MapPath("~/Upload//zip//" + type);
                string downloadUrl = string.Empty;
                List<string> zipFileNamePath = new List<string>();                

                if (type == "img")
                {
                    List<Rio_Pic> Pic = new PicModel().getZipPic(SN);
                    foreach (var item in Pic)
                    {
                        zipFileNamePath.Add(HttpContext.Current.Server.MapPath(item.PicPath) + item.PicName);
                    }
                }
                else if (type == "doc")
                { 
                    List<Rio_Doc> Doc = new DocModel().getZipDoc(SN);                    
                    foreach(var item in Doc)
                    {
                        zipFileNamePath.Add(HttpContext.Current.Server.MapPath(item.DocPath) + item.DocName);
                    }
                }

                downloadUrl = App_Code.Zip.AddZip(zipFileNamePath, zipPath);

                context.Response.Write("/Upload/zip/" + type + "/" + downloadUrl);
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