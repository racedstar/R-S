using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace RioManager.Models
{
    public class UserTrackModel
    {
        private Entities db = new Entities();
        public void Insert(Rio_UserTrack data)
        {
            db.Rio_UserTrack.Add(data);
            db.SaveChanges();
        }

        public void Delete(int SN)
        {
            var data = (from o in db.Rio_UserTrack
                        where o.SN == SN
                        select o).SingleOrDefault();
            db.Rio_UserTrack.Remove(data);
            db.SaveChanges();
        }

        public Vw_UserTrack getUserTrackBySN(int userSN, int clientSN)
        {
            var data = (from o in db.Vw_UserTrack
                        where clientSN == o.TrackAccountSN && userSN == o.AccountSN
                        select o).SingleOrDefault();
            return data;
        }

        public List<Vw_UserTrack> getUserTrackListBySN(int accountSN)
        {
            var data = (from o in db.Vw_UserTrack
                        where accountSN == o.AccountSN
                        select o).ToList();
            return data;
        }

        public List<Vw_UserTrack> getTrackerListBySN(int accountSN)
        {
            var data = (from o in db.Vw_UserTrack
                        where o.TrackAccountSN == accountSN
                        select o).ToList();
            return data;
        }
    }
}