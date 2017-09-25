using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace RioManager.App_Code
{
    public class WebConfig
    {
        #region 限制上傳圖檔類型
        private static string _UploadPicType = string.Empty;
        public static string UploadPicType
        {
            get
            {
                _UploadPicType = ConfigurationManager.AppSettings["UploadPicType"];
                return _UploadPicType;
            }
            set { WebConfig._UploadPicType = value; }
        }
        #endregion
        #region 限制上傳文件類型
        private static string _UploadDocumentType = string.Empty;
        public static string UploadDocumentType
        {
            get
            {
                _UploadDocumentType = ConfigurationManager.AppSettings["UploadDocumentType"];
                return _UploadDocumentType;
            }
            set { WebConfig._UploadDocumentType = value; }
        }
        #endregion

        #region AES加密
        private static string _key = string.Empty;
        public static string key
        {
            get
            {
                _key = ConfigurationManager.AppSettings["key"];
                return _key;
            }
            set { WebConfig._key = value; }
        }
        private static string _iv = string.Empty;
        public static string iv
        {
            get
            {
                _iv = ConfigurationManager.AppSettings["iv"];
                return _iv;
            }
            set { WebConfig._iv = value; }
        }
        #endregion
    }
}