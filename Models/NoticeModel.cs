using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RioManager.Models
{
    public class NoticeModel
    {
        private Entities db = new Entities();

        public void Insert(Rio_Notice data)
        {
            db.Rio_Notice.Add(data);
            db.SaveChanges();
        }

        public void UpdateRead(int SN) 
        {
            var data = (from o in db.Rio_Notice
                        where o.SN == SN
                        select o).SingleOrDefault();
            data.IsRead = true;

            db.Entry(data).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        //View
        public List<Vw_Notice> getNoticeListByTrackSN(int SN)
        {
            var data = (from o in db.Vw_Notice
                        where o.TrackSN == SN
                        select o).OrderByDescending(o => o.SN).ToList();
            return data;
        }

        public void updateNotReadNoticeByTrackSN(int SN)
        {
            var data = (from o in db.Vw_Notice
                        where o.TrackSN == SN && o.IsRead ==false
                        select o.SN).ToList();

            foreach(var item in data)
            {
                UpdateRead(item);
            }
        }

        public int getNotReadNoticeCountByTrackSN(int SN)
        {
            var data = (from o in db.Vw_Notice
                        where o.TrackSN == SN && o.IsRead == false
                        select o.SN).Count();

            return data;
        }
    }
}