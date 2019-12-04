using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RioManager.Models
{
    public static class NoticeModel
    {        
        public static void Insert(Rio_Notice data)
        {
            using (Entities db = new Entities())
            {
                db.Rio_Notice.Add(data);
                db.SaveChanges();
            }
        }

        public static void UpdateRead(int SN) 
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Rio_Notice
                            where o.SN == SN
                            select o).SingleOrDefault();
                data.IsRead = true;

                db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        //View
        public static List<Vw_Notice> getNoticeListByTrackSN(int SN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_Notice
                            where o.TrackSN == SN
                            select o).OrderByDescending(o => o.SN).ToList();
                return data;
            }
        }

        public static void updateNotReadNoticeByTrackSN(int SN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_Notice
                            where o.TrackSN == SN && o.IsRead == false
                            select o.SN).ToList();

                foreach (var item in data)
                {
                    UpdateRead(item);
                }
            }
        }

        public static int getNotReadNoticeCountByTrackSN(int SN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_Notice
                            where o.TrackSN == SN && o.IsRead == false
                            select o.SN).Count();

                return data;
            }
        }
    }
}