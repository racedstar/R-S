using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RioManager.Models
{
    public class AlbumJoinPicModel
    {
        private Entities db = new Entities();
        //編輯相簿未與相簿關聯的圖片
        public List<Rio_Pic> getUpdateNotJoinPic(int aSN, string ID)
        {
            var data = (from o in db.Rio_Pic
                        where !(from a in db.Vw_AlbumJoinPic
                                where a.AlbumSN == aSN && a.PicIsEnable == true && a.PicIsDelete == false
                                select a.PicSN).Contains(o.SN)
                                && o.CreateID == ID
                                && o.IsDelete == false
                        select o).ToList();
            return data;
        }

        //編輯相簿已與相簿關聯的圖片
        public List<Vw_AlbumJoinPic> getUpdateJoinPic(int aSN )
        {
            var data = (from o in db.Vw_AlbumJoinPic
                        where o.AlbumSN == aSN && o.PicIsEnable == true && o.PicIsDelete == false
                        select o).OrderBy(o => o.JoinSort).ToList();

            return data;
        }

        //寫入圖片與相簿關聯
        public void joinAlbum(Rio_AlbumJoinPic joined)
        {
            db.Rio_AlbumJoinPic.Add(joined);
            db.SaveChanges();
        }

        //刪除圖片與相簿關聯
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
        }

        //封面被刪除後取得相簿第一個圖片當作封面
        public int getJoinAlbumFirst(int AlbumSN)
        {
            var data = (from o in db.Vw_AlbumJoinPic
                        where o.AlbumSN == AlbumSN
                        select o.PicSN).FirstOrDefault();
            int picSN = 0;
            int.TryParse(data.ToString(),out picSN);
            return picSN;
        }

        //圖片刪除後，連帶相簿關聯也該刪除
        public void deleteJoinAlbumByPic(int PicSN)
        {
            var data = (from o in db.Rio_AlbumJoinPic
                        where o.PicSN == PicSN
                        select o);
            foreach (var item in data)
            {
                db.Rio_AlbumJoinPic.Remove(item);
            }
            db.SaveChanges();
        }

    }
}