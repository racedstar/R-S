using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.Validation;


namespace RioManager.Models
{
    public class PicModel
    {
        private Entities db = new Entities();

        #region Rio_Pic CUD
        //新增圖片
        public static void Insert(Rio_Pic pic)
        {
            using(Entities db = new Entities())
            {
                db.Rio_Pic.Add(pic);
                db.SaveChanges();
            }
        }

        //更新圖片
        public static void Update(Rio_Pic pic)
        {
            using (Entities db = new Entities())
            {
                db.Entry(pic).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }            
        }

        //刪除圖片
        public static void Delete(Rio_Pic pic)
        {
            using (Entities db = new Entities())
            {
                db.Entry(pic).State = System.Data.Entity.EntityState.Modified;
                pic.IsDelete = true;
                db.SaveChanges();

                AlbumJoinPicModel.deleteJoinAlbumByPic(pic.SN);//刪除相簿與圖片關聯            
                List<Rio_Album> albumList = AlbumModel.getAlbumByFrontCoverSN(pic.SN);//取得所有使用該圖片當封面的相簿
                foreach (var item in albumList)
                {
                    int FronCoverSN = AlbumJoinPicModel.getJoinAlbumFirst(item.SN);//取得該相簿的第一個圖片當作封面
                    Rio_Album album = db.Rio_Album.Find(item.SN);
                    album.FrontCoverSN = FronCoverSN;
                    AlbumModel.Update(album);//更新相簿封面
                }
            }
        }
        #endregion

        //取得使用者所有的圖片
        public static List<Rio_Pic> getUserAllPicByID(string ID)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Rio_Pic
                            where o.CreateID == ID && o.IsDelete == false
                            select o).OrderByDescending(o => o.CreateDate).ToList();
                return data;
            }
        }        

        //取得只有啟用的圖片
        public static List<Rio_Pic> getUserPicEnableByID(string ID)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Rio_Pic
                            where o.IsEnable == true && o.IsDelete == false && o.CreateID == ID
                            select o).OrderByDescending(o => o.CreateDate).ToList();
                return data;
            }
        }

        //取得首頁圖片(前五張)
        public static List<Rio_Pic> getPreViewPicListByID(string ID)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Rio_Pic
                            where o.CreateID == ID && o.IsEnable == true && o.IsDelete == false
                            select o).OrderByDescending(o => o.CreateDate).ToList();
                data = data.Take(4).ToList();
                return data;
            }
        }

        //取得即將被壓縮的圖片
        public List<Rio_Pic> getZipPic(string[] SN)
        {
            using (Entities db = new Entities())
            {
                var data = (from o in db.Rio_Pic
                            where SN.Contains(o.SN.ToString())
                            select o).ToList();
                return data;
            }
        }
    }
}