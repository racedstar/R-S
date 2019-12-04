using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RioManager.Models;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace RioManager.Models
{
    public class UserIndexSettingMode
    {        
        #region CU
        public static void Insert(Rio_UserIndexSetting UserIndexSetting)
        {
            using (Entities db = new Entities())
            {
                db.Rio_UserIndexSetting.Add(UserIndexSetting);
                db.SaveChanges();
            }                
        }

        public static void Update(Rio_UserIndexSetting UserIndexSetting)
        {
            using (Entities db = new Entities())
            {
                db.Entry(UserIndexSetting).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        #endregion

        public static Rio_UserIndexSetting getUserIndexSettingBySN(int SN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Rio_UserIndexSetting
                            where SN == o.AccountSN
                            select o).FirstOrDefault();
                return data;
            }
        }

        public static Vw_UserIndexSetting getVwUserIndexSettingBySN(int SN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_UserIndexSetting
                            where SN == o.AccountSN
                            select o).FirstOrDefault();
                return data;
            }
        }

    }
}