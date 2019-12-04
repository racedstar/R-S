using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace RioManager.Models
{
    public class BaseEntity<Entities, Entity>
        where Entities : System.Data.Linq.DataContext, new()
        where Entity : class
    {
        public static IEnumerable<Entity> Insert(params Entity[] Record)
        {
            //防呆null不執行
            if (Record == null)
            {
                return null;
            }

            //防呆長度等於0
            if (Record.Length == 0)
            {
                return Record;
            }

            IEnumerable<Entity> returnvalue = default(IEnumerable<Entity>);

            using (Entities db = new Entities())
            {
                try
                {
                    var queryentity = db.GetTable<Entity>();

                    returnvalue = Record.AsEnumerable();
                    queryentity.InsertAllOnSubmit(returnvalue);

                    db.SubmitChanges();

                }
                catch (NotSupportedException)
                {
                    throw;
                }
                catch (ObjectDisposedException)
                {
                    throw;
                }
                catch (InvalidOperationException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (db != null)
                    {
                        db.Dispose();
                    }
                }
            }

            return returnvalue;
        }
    }
}