using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class FuenteDAL : DALGeneric, IRepository<Ca_FuentesFin>
    {
        public IEnumerable<Ca_FuentesFin> Get(System.Linq.Expressions.Expression<Func<Ca_FuentesFin, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_FuentesFin;
            return this.Db.Ca_FuentesFin.Where(filter);
        }

        public Ca_FuentesFin GetByID(System.Linq.Expressions.Expression<Func<Ca_FuentesFin, bool>> filter = null)
        {
            return this.Db.Ca_FuentesFin.SingleOrDefault(filter);
        }

        public void Insert(Ca_FuentesFin entity)
        {
            this.Db.Ca_FuentesFin.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_FuentesFin, bool>> filter = null)
        {
            this.Db.Ca_FuentesFin.Remove(this.GetByID(filter));
        }

        public void Update(Ca_FuentesFin entity)
        {
            this.Db.Ca_FuentesFin.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



