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
        private Entities db = new Entities();

        #region AlbumCUD
        public void Insert(Rio_Album album)
        {
            db.Rio_Album.Add(album);
            db.SaveChanges();
        }

        public void Update(Rio_Album album)
        {
            db.Entry(album).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(Rio_Album album)
        {
            db.Entry(album).State = EntityState.Modified;
            db.SaveChanges();
        }
        #endregion

        public Rio_Album getAlbum(int SN)
        {
            var data = (from o in db.Rio_Album
                        where o.SN == SN && o.IsDelete == false
                        select o).FirstOrDefault();

            return data;
        }

        public Vw_Album getVwAlbum(int AlbumSN)
        {
            var data = (from o in db.Vw_Album
                        where o.SN == AlbumSN && o.IsDelete == false
                        select o).SingleOrDefault();
            return data;
        }

        public List<Vw_Album> getAllVwAlbumList()
        {
            var data = (from o in db.Vw_Album
                        where o.IsDelete == false
                        select o).ToList();

            return data;
        }

        public List<Vw_Album> getUsertVwAlbumListByID(string ID)
        {
            var data = (from o in db.Vw_Album
                        where o.IsDelete == false && o.CreateID == ID
                        select o).ToList();
            return data;
        }
        
        //刪除圖片後，取得所有有使用該圖片的當封面的相簿
        public List<Rio_Album> getAlbumByFrontCoverSN(int picSN)
        {
            var data = (from o in db.Rio_Album
                        where o.FrontCoverSN == picSN
                        select o).ToList();
            return data;
        }

        public List<Vw_Album> getUserVwAlbumList()
        {
            var data = (from o in db.Vw_Album
                        where o.IsEnable == true && o.IsDelete == false
                        select o).ToList();
            return data;
        }
    }

        
}