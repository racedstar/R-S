using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.Validation;

namespace RioManager.Models
{
    public class AlbumModel
    {        
        #region AlbumCUD
        public static void Insert(Rio_Album album)
        {
            using (Entities db = new Entities())
            {
                db.Rio_Album.Add(album);
                db.SaveChanges();
            }
        }

        public static void Update(Rio_Album album)
        {
            using (Entities db = new Entities())
            {
                db.Entry(album).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static void Delete(Rio_Album album)
        {
            using (Entities db = new Entities())
            {
                db.Entry(album).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
        #endregion

        public static Rio_Album getAlbum(int SN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Rio_Album
                            where o.SN == SN && o.IsDelete == false
                            select o).FirstOrDefault();

                return data;
            }
        }

        public static Vw_Album getVwAlbum(int AlbumSN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_Album
                            where o.SN == AlbumSN && o.IsDelete == false
                            select o).SingleOrDefault();
                return data;
            }
        }

        public static List<Vw_Album> getUserAllVwAlbumList(string ID)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_Album
                            where o.CreateID == ID && o.IsDelete == false
                            select o).OrderBy(o => o.CreateDate).ToList();

                return data;
            }
        }

        public static List<Vw_Album> getUsertVwAlbumEnableListByID(string ID)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_Album
                            where o.IsEnable == true && o.IsDelete == false && o.CreateID == ID
                            select o).OrderByDescending(o => o.CreateDate).ToList();
                return data;
            }
        }

        public static List<Vw_Album> getPreViewAlbumListByID(string ID)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_Album
                            where o.IsDelete == false && o.IsEnable == true && o.CreateID == ID
                            select o).OrderByDescending(o => o.CreateDate).ToList();
                data = data.Take(4).ToList();
                return data;
            }
        }
        
        //刪除圖片後，取得所有有使用該圖片的當封面的相簿
        public static List<Rio_Album> getAlbumByFrontCoverSN(int picSN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Rio_Album
                            where o.FrontCoverSN == picSN
                            select o).ToList();
                return data;
            }
        }

        public static List<Vw_Album> getUserVwAlbumList()
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Vw_Album
                            where o.IsEnable == true && o.IsDelete == false
                            select o).ToList();
                return data;
            }
        }
    }

        
}