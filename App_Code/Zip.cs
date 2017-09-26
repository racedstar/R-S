using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ionic.Zip;

namespace RioManager.App_Code
{
    class Zip
    {
        /// <summary>
        /// 文件與圖片系統使用
        /// </summary>
        /// <param name="zipFilePathName"></param>被壓縮檔名(字串陣列)        
        /// <param name="savePath"></param>壓縮檔存檔目錄
        public static string AddZip(string[] zipFileNamePath, string savePath)
        {            
            string zipName = Guid.NewGuid().ToString("N") + ".zip";
            string zipPath = savePath + "\\" + zipName;

            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            try
            {
                using (ZipFile zip = new ZipFile(zipName))
                {
                    foreach(var item in zipFileNamePath)
                    {
                        zip.AddFile(item, "");
                    }
                    zip.Save(zipPath);
                }
            }
            catch (Exception ex)
            {

            }

            return zipName;
        }

        public static string AddAlbumZip(string dirPath, string fileName, string savePath)
        {
            string zipName = string.Empty;



            return string.Empty;
        }
    }
}
