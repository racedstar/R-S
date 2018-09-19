using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RioManager.Models;

namespace RioManager.Controllers
{
    public class ToolsController : Controller
    {

        public ActionResult DownloadZip(string type, string[] SN)
        {
            string zipPath = Server.MapPath("~/Upload//zip//" + type);
            string downloadUrl = string.Empty;
            List<string> zipFileNamePath = new List<string>();

            if (type == "img")
            {
                List<Rio_Pic> Pic = new PicModel().getZipPic(SN);
                foreach (var item in Pic)
                {
                    zipFileNamePath.Add(Server.MapPath(item.PicPath) + item.PicName);
                }
            }
            else if (type == "doc")
            {
                List<Rio_Doc> Doc = new DocModel().getZipDoc(SN);
                foreach (var item in Doc)
                {
                    zipFileNamePath.Add(Server.MapPath(item.DocPath) + item.DocName);
                }
            }

            downloadUrl = App_Code.Zip.AddZip(zipFileNamePath, zipPath);

            return Content("/Upload/zip/" + type + "/" + downloadUrl);
        }

        #region Upload Model
        public ActionResult fmUpload(string t)
        {
            string UploadType = t;
            string Extension = string.Empty;

            if (UploadType.Equals("img"))
            {
                Extension = RioManager.App_Code.WebConfig.UploadPicType;
            }
            else if (UploadType.Equals("Doc"))
            {
                Extension = RioManager.App_Code.WebConfig.UploadDocumentType;
            }
            else if (UploadType.Equals("Compression"))
            {
                Extension = RioManager.App_Code.WebConfig.UploadCompressionType;
            }

            ViewBag.UploadType = Extension;

            return View();
        }

        [HttpPost]
        public ActionResult fmUpload(string upLoadType, string count)
        {
            int userSN = 0;
            int.TryParse(Session["UserSN"].ToString(), out userSN);

            string ID = Session["UserID"].ToString();

            SaveData(userSN, ID, upLoadType);

            if (!count.Equals("0"))
            {                
                setDBNotice(userSN, upLoadType, count);
            }

            return Content("Upload Scuuess");
        }
        private string CheckName(string tempName, string fileName, string savePath, int checkNameNum)//檢查名稱是否重複，若是名稱重複則在後面加1
        {
            if (System.IO.File.Exists(savePath + tempName))
            {
                int ArrayLength = fileName.Split('.').Length;
                tempName = fileName.Split('.')[ArrayLength - 2] + "_" + checkNameNum.ToString() + "." + fileName.Split('.')[ArrayLength - 1];
                tempName = CheckName(tempName, fileName, savePath, checkNameNum + 1);
            }
            return tempName;
        }

        private void SaveData(int userSN, string ID, string upLoadType)//存檔
        {
            int fileCount = Request.Files.Count;
            if (fileCount > 0)
            {
                string savePath = Server.MapPath(("~/Upload/" + ID + "/" + upLoadType + "/"));
                string fileName = string.Empty;
                HttpPostedFileBase UploadFile = Request.Files[0];

                fileName = UploadFile.FileName;

                if (!System.IO.Directory.Exists(savePath))
                    System.IO.Directory.CreateDirectory(savePath);

                fileName = CheckName(fileName, fileName, savePath, 0);
                UploadFile.SaveAs(savePath + fileName);

                if (upLoadType.Equals("img"))
                {
                    App_Code.ImageTools.Scaling(fileName, savePath);
                    setDBimg(ID, fileName, upLoadType);
                }
                else if (upLoadType.Equals("Doc"))
                {
                    setDBDoc(ID, fileName, upLoadType);
                }
                else if (upLoadType.Equals("Compression"))
                {
                    setDBCompression(ID, userSN, fileName, upLoadType);
                }
            }
        }

        private void setDBimg(string ID, string fileName, string upLoadType)
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

        private void setDBDoc(string ID, string fileName, string upLoadType)
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

        private void setDBCompression(string ID, int userSN, string fileName, string upLoadType)
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

        private void setDBNotice(int userSN, string upLoadType, string sumCount)
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
        #endregion

    }
}