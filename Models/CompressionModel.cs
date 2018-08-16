using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RioManager.Models
{
    public class CompressionModel
    {
        private Entities db = new Entities();

        public void Insert(Rio_Compression data)
        {
            db.Rio_Compression.Add(data);
            db.SaveChanges();
        }

        public void Update(Rio_Compression data)
        {
            db.Entry(data).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }


        public List<Rio_Compression> getCompressionByUserSN(int SN)
        {
            var data = (from o in db.Rio_Compression
                        where o.CreateSN == SN && o.IsDelete == false
                        select o).ToList();
            return data;
        }

        public List<Rio_Compression> getCompressionClientByUserSN(int SN)
        {
            var data = (from o in db.Rio_Compression
                        where o.CreateSN == SN && o.IsEnable == true && o.IsDelete == false
                        select o).ToList();
            return data;
        }
    }
}