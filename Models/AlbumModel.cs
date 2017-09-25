using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RioManager.Models
{
    public class AlbumModel
    {
        private Entities db = new Entities();
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

        public List<Rio_Pic> getUpdateNotJoinPic(int aSN)
        {
            var data = (from o in db.Rio_Pic
                        where !(from a in db.Vw_AlbumJoinPic
                                where a.AlbumSN == aSN && a.PicIsEnable == true && a.PicIsDelete == false
                                select a.PicSN).Contains(o.SN)
                                && o.IsDelete == false
                        select o).ToList();
            return data;
        } //編輯相簿未與相簿關聯的圖片

        public List<Vw_AlbumJoinPic> getUpdateJoinPic(int aSN)
        {
            var data = (from o in db.Vw_AlbumJoinPic
                        where o.AlbumSN == aSN && o.PicIsEnable == true && o.PicIsDelete == false
                        select o).ToList();

            return data;
        } //編輯相簿已與相簿關聯的圖片

        public void joinAlbum(Rio_AlbumJoinPic joined)
        {
            db.Rio_AlbumJoinPic.Add(joined);
            db.SaveChanges();
        } //寫入圖片與相簿關聯

        public void deleteJoinAlbum(int AlbumSN)
        {
            var data = (from o in db.Rio_AlbumJoinPic
                        where o.AlbumSN == AlbumSN
                        select o);
            foreach (var item in data)
            {
                db.Rio_AlbumJoinPic.Remove(item);
            }
            db.SaveChanges();
        } //刪除圖片與相簿關聯
    }
}