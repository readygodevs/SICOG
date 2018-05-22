using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class TipoTransferenciasIngDAL : DALGeneric, IRepository<Ca_TipoTransferenciasIng>
    {

        public IEnumerable<Ca_TipoTransferenciasIng> Get(System.Linq.Expressions.Expression<Func<Ca_TipoTransferenciasIng, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_TipoTransferenciasIng;
            return this.Db.Ca_TipoTransferenciasIng.Where(filter);
        }

        public Ca_TipoTransferenciasIng GetByID(System.Linq.Expressions.Expression<Func<Ca_TipoTransferenciasIng, bool>> filter = null)
        {
            return this.Db.Ca_TipoTransferenciasIng.SingleOrDefault(filter);
        }

        public void Insert(Ca_TipoTransferenciasIng entity)
        {
            this.Db.Ca_TipoTransferenciasIng.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_TipoTransferenciasIng, bool>> filter = null)
        {
            this.Db.Ca_TipoTransferenciasIng.Remove(this.GetByID(filter));
        }

        public void Update(Ca_TipoTransferenciasIng entity)
        {
            this.Db.Ca_TipoTransferenciasIng.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



