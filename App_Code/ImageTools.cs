using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using RioManager.Models;

namespace RioManager.App_Code
{
    public class ImageTools
    {
        public static string Scaling(string imgName, string Path)
        {
            string Message = string.Empty;
            System.Drawing.Image img = System.Drawing.Image.FromFile(Path + "/" + imgName);
            int fixWidth = 0;
            int fixHeight = 0;

            if (img.Width > img.Height)
            {
                fixWidth = 336;
                fixHeight = 226;
            }
            else if (img.Height > img.Width)
            {
                fixWidth = 140;
                fixHeight = 226;
            }
            else if (img.Width == img.Height)
            {
                fixWidth = 229;
                fixHeight = 229;
            }

            Bitmap imgOuput = new Bitmap(img, fixWidth, fixHeight);
            if (!Directory.Exists((Path + "/Scaling/")))
                Directory.CreateDirectory(Path + "/Scaling/");

            try
            {
                imgOuput.Save(string.Concat(Path + "/Scaling/", imgName));
                Message += imgName + " = Scaling Clear<br />";
            }
            catch
            {
                Message += imgName + " = Scaling ERROR<br />";
            }
            finally
            {
                //釋放記憶體與圖檔
                imgOuput.Dispose();
                img.Dispose();
            }

            return Message;
        }

        public static string ReScaling(List<Rio_Pic> PicList)
        {
            string Message = string.Empty;
            foreach (var item in PicList)
            {
                Message += Scaling(item.PicName, HttpContext.Current.Server.MapPath(item.PicPath));
            }

            return Message;
        }

    }
}