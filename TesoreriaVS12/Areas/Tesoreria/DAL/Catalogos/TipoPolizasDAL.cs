using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class TipoPolizasDAL : DALGeneric, IRepository<Ca_TipoPolizas>
    {

        public IEnumerable<Ca_TipoPolizas> Get(System.Linq.Expressions.Expression<Func<Ca_TipoPolizas, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_TipoPolizas;
            return this.Db.Ca_TipoPolizas.Where(filter);
        }

        public Ca_TipoPolizas GetByID(System.Linq.Expressions.Expression<Func<Ca_TipoPolizas, bool>> filter = null)
        {
            return this.Db.Ca_TipoPolizas.SingleOrDefault(filter);
        }

        public void Insert(Ca_TipoPolizas entity)
        {
            this.Db.Ca_TipoPolizas.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_TipoPolizas, bool>> filter = null)
        {
            this.Db.Ca_TipoPolizas.Remove(this.GetByID(filter));
        }

        public void Update(Ca_TipoPolizas entity)
        {
            this.Db.Ca_TipoPolizas.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



