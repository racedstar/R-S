using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RioManager.Models
{
    public class ClassNameModel
    {
        public string TopBar { get; set; }
        public string SysContainer { get; set; }
        public string DataClass { get; set; }

        public static ClassNameModel getClassName(string systemName)
        {
            ClassNameModel cn = new ClassNameModel();
            switch (systemName)
            {
                case "pic":
                    cn = getPictureClassName();
                    break;
                case "albumView":
                    cn = getAlbumViewClassName();
                    break;
                case "albumContent":
                    cn = getAlbumContentClassName();
                    break;
                case "doc":
                    cn = getDocumentClassName();
                    break;
                case "compression":
                    cn = getCompressionClassName();
                    break;
                default:

                    break;
            }

            return cn;
        }

        private static ClassNameModel getPictureClassName()
        {
            ClassNameModel cn = new ClassNameModel();
            cn.TopBar = "topBar";
            cn.SysContainer = "systemContainer";
            cn.DataClass = "picViewDiv";

            return cn;
        }

        private static ClassNameModel getDocumentClassName()
        {
            ClassNameModel cn = new ClassNameModel();
            cn.TopBar = "topBar";
            cn.SysContainer = "systemContainer";
            cn.DataClass = "docViewDiv";

            return cn;
        }

        private static ClassNameModel getAlbumViewClassName()
        {
            ClassNameModel cn = new ClassNameModel();
            cn.TopBar = "topBar";
            cn.SysContainer = "systemContainer";
            cn.DataClass = "albumDiv";

            return cn;
        }

        private static ClassNameModel getAlbumContentClassName()
        {
            ClassNameModel cn = new ClassNameModel();
            cn.TopBar = "topBar";
            cn.SysContainer = "systemContainer";
            cn.DataClass = "picViewDiv";

            return cn;
        }

        private static ClassNameModel getCompressionClassName()
        {
            ClassNameModel cn = new ClassNameModel();
            cn.TopBar = "topBar";
            cn.SysContainer = "systemContainer";
            cn.DataClass = "docViewDiv";

            return cn;
        }
    }

}