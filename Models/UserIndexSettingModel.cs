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
        private Entities db = new Entities();
        #region CU
        public void Insert(Rio_UserIndexSetting UserIndexSetting)
        {
            db.Rio_UserIndexSetting.Add(UserIndexSetting);
            db.SaveChanges();
        }

        public void Update(Rio_UserIndexSetting UserIndexSetting)
        {
            db.Entry(UserIndexSetting).State = EntityState.Modified;
            db.SaveChanges();
        }

        #endregion

        public Rio_UserIndexSetting getUserIndexSettingBySN(int SN)
        {
            var data = (from o in db.Rio_UserIndexSetting
                        where SN == o.AccountSN
                        select o).FirstOrDefault();
            return data;
        }

        public Vw_UserIndexSetting getVwUserIndexSettingBySN(int SN)
        {
            var data = (from o in db.Vw_UserIndexSetting
                        where SN == o.AccountSN
                        select o).FirstOrDefault();
            return data;
        }

    }
}