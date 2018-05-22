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

    public class ClasificaPolizaDAL : DALGeneric, IRepository<Ca_ClasificaPolizas>
    {
        public IEnumerable<Ca_ClasificaPolizas> Get(System.Linq.Expressions.Expression<Func<Ca_ClasificaPolizas, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_ClasificaPolizas;
            return this.Db.Ca_ClasificaPolizas.Where(filter);
        }

        public Ca_ClasificaPolizas GetByID(System.Linq.Expressions.Expression<Func<Ca_ClasificaPolizas, bool>> filter = null)
        {
            return this.Db.Ca_ClasificaPolizas.SingleOrDefault(filter);
        }

        public void Insert(Ca_ClasificaPolizas entity)
        {
            this.Db.Ca_ClasificaPolizas.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_ClasificaPolizas, bool>> filter = null)
        {
            this.Db.Ca_ClasificaPolizas.Remove(this.GetByID(filter));
        }

        public void Update(Ca_ClasificaPolizas entity)
        {
            this.Db.Ca_ClasificaPolizas.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



