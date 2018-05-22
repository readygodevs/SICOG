using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class TipoPagosDAL : DALGeneric, IRepository<Ca_TipoPagos>
    {
        public IEnumerable<Ca_TipoPagos> Get(System.Linq.Expressions.Expression<Func<Ca_TipoPagos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_TipoPagos;
            return this.Db.Ca_TipoPagos.Where(filter);
        }

        public Ca_TipoPagos GetByID(System.Linq.Expressions.Expression<Func<Ca_TipoPagos, bool>> filter = null)
        {
            return this.Db.Ca_TipoPagos.SingleOrDefault(filter);
        }

        public void Insert(Ca_TipoPagos entity)
        {
            this.Db.Ca_TipoPagos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_TipoPagos, bool>> filter = null)
        {
            this.Db.Ca_TipoPagos.Remove(this.GetByID(filter));
        }

        public void Update(Ca_TipoPagos entity)
        {
            this.Db.Ca_TipoPagos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



