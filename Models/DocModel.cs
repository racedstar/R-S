using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RioManager.Models
{
    public class DocModel
    {
        private Entities db = new Entities();
        public List<Rio_Doc> getAllDoc()
        {
            var data = (from o in db.Rio_Doc
                        where o.IsDelete == false
                        select o).ToList();

            return data;
        }

        public List<Rio_Doc> getZipDoc(string[] SN)//取得要被壓縮的檔案
        {
            var data = (from o in db.Rio_Doc
                        where SN.Contains(o.SN.ToString())
                        select o).ToList();
            return data;
        }
    }
}