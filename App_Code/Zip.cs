using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace RioManager.App_Code
{
    class Zip
    {
        public static string AddZip(string dirPath, string fileName, string savePath)
        {
            string zipPath = string.Empty;

            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            //try
            //{
            //    ZipFile
            //}
            //catch(Exception ex)
            //{

            //}

            return zipPath;
        }
    }
}
