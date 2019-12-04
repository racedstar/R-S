using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RioManager.Models
{
    public class CompressionModel
    {        

        public static void Insert(Rio_Compression data)
        {
            using (Entities db = new Entities())
            {
                db.Rio_Compression.Add(data);
                db.SaveChanges();
            }
        }

        public static void Update(Rio_Compression data)
        {
            using (Entities db = new Entities())
            {
                db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }


        public static List<Rio_Compression> getCompressionByUserSN(int SN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Rio_Compression
                            where o.CreateSN == SN && o.IsDelete == false
                            select o).OrderBy(o => o.CreateDate).ToList();
                return data;
            }
        }

        public static List<Rio_Compression> getCompressionClientByUserSN(int SN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Rio_Compression
                            where o.CreateSN == SN && o.IsEnable == true && o.IsDelete == false
                            select o).ToList();
                return data;
            }
        }
    }
}