using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.Validation;

namespace RioManager.Models
{
    public class DocModel
    {
        private Entities db = new Entities();
        #region Rio_Doc CUD
        public void Insert(Rio_Doc doc)
        {
            db.Rio_Doc.Add(doc);
            db.SaveChanges();
        }

        public void Update(Rio_Doc doc)
        {
            db.Entry(doc).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(Rio_Doc doc)
        {
            db.Entry(doc).State = EntityState.Modified;
            doc.IsDelete = true;
            db.SaveChanges();
        }
        #endregion

        public List<Rio_Doc> getUserAllDocByID(string ID)
        {
            var data = (from o in db.Rio_Doc
                        where o.CreateID == ID && o.IsDelete == false
                        select o).ToList();

            return data;
        }

        public List<Rio_Doc> getUserDocEnableListByID(string ID)
        {
            var data = (from o in db.Rio_Doc
                        where  o.CreateID == ID && o.IsEnable == true && o.IsDelete == false
                        select o).OrderByDescending(o => o.CreateDate).ToList();
            return data;
        }
        

        public List<Rio_Doc> getPreviewDocListByID(string ID)
        {
            var data = (from o in db.Rio_Doc
                        where o.CreateID == ID && o.IsEnable == true && o.IsDelete == false
                        select o).OrderByDescending(o => o.CreateDate).ToList();
            data = data.Take(4).ToList();
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