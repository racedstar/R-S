using System.Collections.Generic;
using System.Data;
using System.Linq;



namespace RioManager.Models
{
    public class AccountModel
    {
        private Entities db = new Entities();
        
        public void Insert(Rio_Account rio_Account)
        {            
                db.Rio_Account.Add(rio_Account);
                db.SaveChanges();

        }

        public void Update(Rio_Account rio_Account)
        {
            db.Entry(rio_Account).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public Rio_Account getAccountByID(string ID)
        {
            var data = (from o in db.Rio_Account
                        where o.ID == ID
                        select o).FirstOrDefault();
            return data;
        }

        public Vw_Account getVwAccount(int SN)
        {
            var data = (from o in db.Vw_Account
                        where o.SN == SN && o.IsEnable == true && o.IsDelete == false
                        select o).FirstOrDefault();
            return data;
        }

        public Vw_Account getVwAccountByFBEmail(string email)
        {
            var data = (from o in db.Vw_Account
                        where o.fbEmail == email
                        select o).FirstOrDefault();
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

        public int LoginCheck(string ID, string Password)
        {
            Password = App_Code.Coding.stringToSHA512(Password);
            var data = (from o in db.Rio_Account
                        where o.ID == ID && o.Password == Password
                        select o.SN).FirstOrDefault();

            return data;
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}