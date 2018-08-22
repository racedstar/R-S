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

        public static ClassNameModel getPictureClassName()
        {
            ClassNameModel cn = new ClassNameModel();
            cn.TopBar = "topBar";
            cn.SysContainer = "systemContainer";
            cn.DataClass = "picViewDivNew";

            return cn;
        }
    }

    public enum pictureClassName
    {
        picViewDiv,
        picViewDivNew
    }

    public enum albumClassName
    {

    }

    public enum docClassName
    {

    }

    public enum CompressionClassName
    {

    }

    public enum notice
    {

    }
}