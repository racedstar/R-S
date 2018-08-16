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
        int userSN = 0;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if(context.Session["UserID"] !=null)
            { 
                ID = context.Session["UserID"].ToString();
                int.TryParse(context.Session["UserSN"].ToString(), out userSN);
                string upLoadType = string.Empty;
                upLoadType = context.Request.QueryString["t"].ToString();
                SaveData(upLoadType);

                if (!HttpContext.Current.Request.QueryString.Get("count").Equals("0"))
                {                                        
                    string sumCount = HttpContext.Current.Request.QueryString.Get("count");
                    setDBNotice(userSN, upLoadType, sumCount);
                }
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
                tempName = fileName.Split('.')[ArrayLength - 2] + "_" + checkNameNum.ToString() + "." + fileName.Split('.')[ArrayLength - 1];
                tempName = CheckName(tempName, fileName, savePath, checkNameNum + 1);
            }
            return tempName;
        }
        private void SaveData(string upLoadType)//存檔
        {
            int fileCount = HttpContext.Current.Request.Files.Count;
            if (fileCount > 0)
            {
                string savePath = HttpContext.Current.Server.MapPath(("~/Upload/" + ID + "/" + upLoadType + "/"));
                string fileName = string.Empty;
                HttpPostedFile UploadFile = HttpContext.Current.Request.Files[0];

                fileName = UploadFile.FileName;

                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                fileName = CheckName(fileName, fileName, savePath, 0);
                UploadFile.SaveAs(savePath + fileName);

                if (upLoadType.Equals("img"))
                {
                    App_Code.ImageTools.Scaling(fileName, savePath);
                    setDBimg(fileName, upLoadType);
                }
                else if (upLoadType.Equals("Doc"))
                {
                    setDBDoc(fileName, upLoadType);
                }
                else if (upLoadType.Equals("Compression"))
                {
                    setDBCompression(fileName, upLoadType);
                }
            }
        }
        private void setDBimg(string fileName, string upLoadType)
        {
            Rio_Pic Pic = new Rio_Pic();
            Pic.PicName = fileName;
            Pic.PicPath = "/Upload/" + ID + "/" + upLoadType + "/";
            Pic.PicContent = string.Empty;
            Pic.HitCount = 0;
            Pic.CreateID = ID;
            Pic.CreateName = ID;
            Pic.ModifyID = ID;
            Pic.ModifyName = ID;

            DateTime dt = DateTime.Now;
            Pic.CreateDate = dt;
            Pic.ModifyDate = dt;

            Pic.IsEnable = true;
            Pic.IsDelete = false;
            new PicModel().Insert(Pic);
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
            Doc.CreateDate = dt;
            Doc.ModifyDate = dt;
            new Rio_DocController().Create(Doc);
        }

        private void setDBCompression(string fileName, string upLoadType)
        {
            Rio_Compression CF = new Rio_Compression();
            CF.Name = fileName;
            CF.Path = "/Upload/" + ID + "/" + upLoadType + "/";
            CF.Extension = fileName.Split('.')[fileName.Split('.').Length - 1];
            CF.CreateSN = userSN;
            CF.CreateID = ID;
            CF.CreateName = ID;
            CF.CreateDate = DateTime.Now;
            CF.IsEnable = true;
            CF.IsDelete = false;

            new CompressionModel().Insert(CF);            
        }

        private void setDBNotice(int userSN,string upLoadType, string sumCount)
        {
            List<Vw_UserTrack> userTrackList = new UserTrackModel().getTrackerListBySN(userSN);
            string type = string.Empty;

            switch (upLoadType)
            {
                case "img":
                    type = " 張圖片";
                    break;
                case "Doc":
                    type = " 件文件";
                    break;
                case "Compression":
                    type = "個壓縮檔";
                    break;
            }

            foreach (var item in userTrackList)
            {
                Rio_Notice notice = new Rio_Notice();
                notice.AccountSN = userSN;
                notice.TrackSN = item.AccountSN;
                notice.NoticeContent = "已上傳 " + sumCount + type;
                notice.CreateDate = DateTime.Now;

                new NoticeModel().Insert(notice);
            }
        }
    }
}