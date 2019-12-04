using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RioManager.Models
{
    public class AccountModel
    {       
        public static void Insert(Rio_Account rio_Account)
        {
            using (Entities db = new Entities())
            {
                db.Rio_Account.Add(rio_Account);
                db.SaveChanges();
            }
        }

        public static void Update(Rio_Account rio_Account)
        {
            using (Entities db = new Entities())
            {
                db.Entry(rio_Account).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static Rio_Account getAccountByID(string ID)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Rio_Account
                            where o.ID == ID
                            select o).FirstOrDefault();
                return data;
            }
        }

        public static Vw_Account getVwAccount(int SN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_Account
                            where o.SN == SN && o.IsEnable == true && o.IsDelete == false
                            select o).FirstOrDefault();
                return data;
            }
        }

        public static Vw_Account getVwAccountByFBEmail(string email)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_Account
                            where o.fbEmail == email
                            select o).FirstOrDefault();
                return data;
            }
        }

        public static Vw_Account getVwAccountByID(string ID)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_Account
                            where o.ID == ID
                            select o).FirstOrDefault();
                return data;
            }
        }

        public static List<Vw_Account> getAccountList()
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_Account
                            where o.IsEnable == true && o.IsDelete == false
                            select o).ToList();
                return data;
            }
        }

        public static int LoginCheck(string ID, string Password)
        {
            using(Entities db = new Entities())
            { 
                Password = App_Code.Coding.stringToSHA512(Password);
                var data = (from o in db.Rio_Account
                            where o.ID == ID && o.Password == Password
                            select o.SN).FirstOrDefault();

                return data;
            }
        }
    }
}