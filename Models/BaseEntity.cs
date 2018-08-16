using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Data.Linq;

namespace RioManager.Models
{
    [Serializable]
    public class BaseEntity<Entity>        
        where Entity : class
    {
        
        public static IEnumerable<Entity> Insert(params Entity[] Record)
        {
            Entities db = new Entities();

            if (Record == null)
            {
                return null;
            }            
            //防呆長度等於0
            if (Record.Length == 0)
            {
                return Record;
            }

            IEnumerable<Entity> returnValue = default(IEnumerable<Entity>);

            
            
                try
                {                    
                    //db
                    //returnValue = Record.AsEnumerable();
                    //queryentity.InsertAllOnSubmit(returnValue);

                    //db.SubmitChanges();
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
            return returnValue;
        }
    }
}