using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RioManager.Models;
using System.Data.Entity.Validation;
using System.Data.Linq;



namespace RioManager.Models
{
    public class FBLoginModel
    {                
        public static void Insert(Rio_FBLogin rio_FBLogin)
        {
            using (Entities db = new Entities())
            {
                db.Rio_FBLogin.Add(rio_FBLogin);
                db.SaveChanges();
            }
        }

        public static void Update(Rio_FBLogin rio_FBLogin)
        {
            using (Entities db = new Entities())
            {
                db.Entry(rio_FBLogin).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static Rio_FBLogin getFBInfo(string email)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Rio_FBLogin
                            where email == o.Email
                            select o).FirstOrDefault();
                return data;
            }
        }
    }
}