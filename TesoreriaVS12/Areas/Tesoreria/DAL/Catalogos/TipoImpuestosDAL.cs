using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class TipoImpuestosDAL : DALGeneric, IRepository<Ca_TipoImpuestos>
    {

        public IEnumerable<Ca_TipoImpuestos> Get(System.Linq.Expressions.Expression<Func<Ca_TipoImpuestos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_TipoImpuestos;
            return this.Db.Ca_TipoImpuestos.Where(filter);
        }

        public Ca_TipoImpuestos GetByID(System.Linq.Expressions.Expression<Func<Ca_TipoImpuestos, bool>> filter = null)
        {
            return this.Db.Ca_TipoImpuestos.SingleOrDefault(filter);
        }

        public void Insert(Ca_TipoImpuestos entity)
        {
            this.Db.Ca_TipoImpuestos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_TipoImpuestos, bool>> filter = null)
        {
            this.Db.Ca_TipoImpuestos.Remove(this.GetByID(filter));
        }

        public void Update(Ca_TipoImpuestos entity)
        {
            this.Db.Ca_TipoImpuestos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



