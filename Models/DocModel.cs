using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RioManager.Models
{
    public class DocModel
    {
        private Entities db = new Entities();
        public List<Rio_Doc> getAllDoc()
        {
            var data = (from o in db.Rio_Doc
                        where o.IsDelete == false
                        select o).ToList();

            return data;
        }
    }
}