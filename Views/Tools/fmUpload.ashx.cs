using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.IO;
using RioManager.Models;
using RioManager.Controllers;

namespace RioManager.Views.Tools
{
    /// <summary>
    /// fmUpload 的摘要描述
    /// </summary>
    public class fmUpload : IHttpHandler, IRequiresSessionState
    {
        string ID = string.Empty;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if(context.Session["UserID"] !=null)
            { 
                ID = context.Session["UserID"].ToString();
                string UploadType = string.Empty;
                UploadType = context.Request.QueryString["t"].ToString();
                SaveData(UploadType);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private string CheckName(string tempName, string fileName, string savePath, int checkNameNum)//檢查名稱是否重複，若是名稱重複則在後面加1
        {
            if (File.Exists(savePath + tempName))
            {
                int ArrayLength = fileName.Split('.').Length;
                tempName = fileName.Split('.')[ArrayLength - 2] + "(" + checkNameNum.ToString() + ")." + fileName.Split('.')[ArrayLength - 1];
                tempName = CheckName(tempName, fileName, savePath, checkNameNum + 1);
            }
            return tempName;
        }
        private void SaveData(string UploadType)//存檔
        {
            int fileCount = HttpContext.Current.Request.Files.Count;
            if (fileCount > 0)
            {
                string savePath = HttpContext.Current.Server.MapPath(("~/Upload/" + ID + "/" + UploadType + "/"));
                string fileName = string.Empty;
                HttpPostedFile UploadFile = HttpContext.Current.Request.Files[0];

                fileName = UploadFile.FileName;

                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                fileName = CheckName(fileName, fileName, savePath, 0);
                UploadFile.SaveAs(savePath + fileName);

                if (UploadType.Equals("img"))
                {
                    App_Code.ImageTools.Scaling(fileName, savePath);
                    setDBimg(fileName, UploadType);
                }
                else if (UploadType.Equals("Doc"))
                {
                    setDBDoc(fileName, UploadType);
                }
            }
        }
        private void setDBimg(string fileName, string UploadType)
        {
            Rio_Pic Pic = new Rio_Pic();
            Pic.PicName = fileName;
            Pic.PicPath = "/Upload/" + ID + "/" + UploadType + "/";
            Pic.PicContent = string.Empty;
            Pic.HitCount = 0;
            Pic.CreateID = ID;
            Pic.CreateName = ID;
            Pic.ModifyID = ID;
            Pic.ModifyName = ID;

            DateTime dt = DateTime.Now; ;
            DateTime.TryParse(dt.ToString("yyyy-MM-dd"), out dt);
            Pic.CreateDate = dt;
            Pic.ModifyDate = dt;

            Pic.IsEnable = true;
            Pic.IsDelete = false;
            new Rio_PicController().Insert(Pic);
        }

        private void setDBDoc(string fileName,string upLoadType)
        {            
            Rio_Doc Doc = new Rio_Doc();
            Doc.DocName = fileName;
            Doc.DocContent = string.Empty;
            Doc.DocPath = "/Upload/" + ID + "/" + upLoadType + "/";
            Doc.HitCount = 0;
            Doc.Extension = fileName.Split('.')[fileName.Split('.').Length - 1];
            Doc.IsEnable = true;
            Doc.IsDelete = false;
            Doc.CreateID = ID;
            Doc.CreateName = ID;
            Doc.ModifyId = ID;
            Doc.ModifyName = ID;

            DateTime dt = DateTime.Now; ;
            DateTime.TryParse(dt.ToString("yyyy-MM-dd"), out dt);
            Doc.CreateDate = dt;
            Doc.ModifyDate = dt;
            new Rio_DocController().Create(Doc);
        }
    }
}