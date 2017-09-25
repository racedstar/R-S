using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RioManager.Models
{
    public class PicModel
    {
        private Entities db = new Entities();
        public List<Rio_Pic> getAllPic()
        {
            var data = (from o in db.Rio_Pic
                        where o.IsDelete == false
                        select o).ToList();
            return data;
        }
    }
}