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
        private Entities db = new Entities();
        
        public void Insert(Rio_FBLogin rio_FBLogin)
        {            
                db.Rio_FBLogin.Add(rio_FBLogin);
                db.SaveChanges();

        }

        public void Update(Rio_FBLogin rio_FBLogin)
        {
            db.Entry(rio_FBLogin).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public Rio_FBLogin getFBInfo(string email)
        {
            var data = (from o in db.Rio_FBLogin
                        where email == o.Email
                        select o).FirstOrDefault();
            return data;                         
        }
    }
}