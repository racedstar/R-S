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
        public static string AddZip(List<string> zipFileNamePath, string savePath)
        {            
            string zipName = Guid.NewGuid().ToString("N") + ".zip";
            string zipPath = savePath + "\\" + zipName;

            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            try
            {
                using (ZipFile zip = new ZipFile(zipName, System.Text.Encoding.Default))//(壓縮檔名稱,編碼(若是檔案為中文沒有使用System.Text.Encoding.Default會無法壓縮中文檔名的檔案))
                {
                    foreach (var item in zipFileNamePath)
                    {
                        zip.AddFile(item, "");//AddFile(檔案名稱,壓縮檔內的儲存路徑)
                    }
                    zip.Save(zipPath);
                }
            }
            catch (Exception ex)
            {

            }

            return zipName;
        }
    }
}
