using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;
using System.Data.SqlClient;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class AreasDAL : DALGeneric, IRepository<Ca_Areas>
    {

        public IEnumerable<Ca_Areas> Get(System.Linq.Expressions.Expression<Func<Ca_Areas, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_Areas;
            return this.Db.Ca_Areas.Where(filter);
        }

        public Ca_Areas GetByID(System.Linq.Expressions.Expression<Func<Ca_Areas, bool>> filter = null)
        {
            return this.Db.Ca_Areas.SingleOrDefault(filter);
        }

        public void Insert(Ca_Areas entity)
        {
            this.Db.Ca_Areas.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_Areas, bool>> filter = null)
        {
            this.Db.Ca_Areas.Remove(this.GetByID(filter));
        }

        public void Update(Ca_Areas entity)
        {
            this.Db.Ca_Areas.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



