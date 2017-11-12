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
    public class AccountModel : Controller
    {
        private Entities db = new Entities();
        
        public void Insert(Rio_Account rio_Account)
        {            
                db.Rio_Account.Add(rio_Account);
                db.SaveChanges();

        }

        public void Update(Rio_Account rio_Account)
        {
            db.Entry(rio_Account).State = EntityState.Modified;
            db.SaveChanges();
        }

        public Rio_Account getAccountByID(string ID)
        {
            var data = (from o in db.Rio_Account
                        where o.ID == ID
                        select o).FirstOrDefault();
            return data;
        }

        public Vw_Account getAccount(int SN)
        {
            var data = (from o in db.Vw_Account
                        where o.SN == SN && o.IsEnable == true && o.IsDelete == false
                        select o).FirstOrDefault();
            //ViewBag.picPath = data.PicPath + data.PicName;
            return data;
        }

        public Vw_Account getVwAccountByID(string ID)
        {            
                var data = (from o in db.Vw_Account
                            where o.ID == ID
                            select o).FirstOrDefault();
                return data;                        
        }

        public List<Vw_Account> getAccountList()
        {
            var data = (from o in db.Vw_Account
                        where o.IsEnable == true && o.IsDelete == false
                        select o).ToList();
            return data;
        }

        public bool LoginCheck(string ID, string Password)
        {
            Password = App_Code.Coding.Encrypt(Password);
            var data = (from o in db.Rio_Account
                        where o.ID == ID && o.Password == Password
                        select o).FirstOrDefault();

            if (data != null)
            {
                return true;
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}