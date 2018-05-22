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

    public class FuenteIngDAL : DALGeneric, IRepository<Ca_FuentesFin_Ing>
    {
        public IEnumerable<Ca_FuentesFin_Ing> Get(System.Linq.Expressions.Expression<Func<Ca_FuentesFin_Ing, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_FuentesFin_Ing;
            return this.Db.Ca_FuentesFin_Ing.Where(filter);
        }

        public Ca_FuentesFin_Ing GetByID(System.Linq.Expressions.Expression<Func<Ca_FuentesFin_Ing, bool>> filter = null)
        {
            return this.Db.Ca_FuentesFin_Ing.SingleOrDefault(filter);
        }

        public void Insert(Ca_FuentesFin_Ing entity)
        {
            this.Db.Ca_FuentesFin_Ing.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_FuentesFin_Ing, bool>> filter = null)
        {
            this.Db.Ca_FuentesFin_Ing.Remove(this.GetByID(filter));
        }

        public void Update(Ca_FuentesFin_Ing entity)
        {
            this.Db.Ca_FuentesFin_Ing.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



