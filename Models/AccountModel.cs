using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RioManager.Models;


namespace RioManager.Models
{
    public class AccountModel : Controller
    {
        private Entities db = new Entities();

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
    }
}