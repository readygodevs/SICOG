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

    public class ImpuestosDeduccionDAL : DALGeneric, IRepository<Ca_Impuestos_Deduccion>
    {

        public IEnumerable<Ca_Impuestos_Deduccion> Get(System.Linq.Expressions.Expression<Func<Ca_Impuestos_Deduccion, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_Impuestos_Deduccion;
            return this.Db.Ca_Impuestos_Deduccion.Where(filter);
        }

        public Ca_Impuestos_Deduccion GetByID(System.Linq.Expressions.Expression<Func<Ca_Impuestos_Deduccion, bool>> filter = null)
        {
            return this.Db.Ca_Impuestos_Deduccion.SingleOrDefault(filter);
        }

        public void Insert(Ca_Impuestos_Deduccion entity)
        {
            this.Db.Ca_Impuestos_Deduccion.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_Impuestos_Deduccion, bool>> filter = null)
        {
            this.Db.Ca_Impuestos_Deduccion.Remove(this.GetByID(filter));
        }

        public void Update(Ca_Impuestos_Deduccion entity)
        {
            this.Db.Ca_Impuestos_Deduccion.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



