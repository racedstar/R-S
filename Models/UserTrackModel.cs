using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace RioManager.Models
{
    public class UserTrackModel
    {        
        public static void Insert(Rio_UserTrack data)
        {
            using (Entities db = new Entities())
            {
                db.Rio_UserTrack.Add(data);
                db.SaveChanges();
            }
        }

        public static void Delete(int SN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Rio_UserTrack
                            where o.SN == SN
                            select o).SingleOrDefault();
                db.Rio_UserTrack.Remove(data);
                db.SaveChanges();
            }
        }

        public static Vw_UserTrack getUserTrackBySN(int userSN, int clientSN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_UserTrack
                            where clientSN == o.TrackAccountSN && userSN == o.AccountSN
                            select o).SingleOrDefault();
                return data;
            }
        }

        public static List<Vw_UserTrack> getUserTrackListBySN(int accountSN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_UserTrack
                            where accountSN == o.AccountSN
                            select o).ToList();
                return data;
            }
        }

        public static List<Vw_UserTrack> getTrackerListBySN(int accountSN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_UserTrack
                            where o.TrackAccountSN == accountSN
                            select o).ToList();
                return data;
            }
        }
    }
}