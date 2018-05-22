using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class TipoTrasferenciasEgDAL : DALGeneric, IRepository<Ca_TipoTransferenciasEg>
    {
        public IEnumerable<Ca_TipoTransferenciasEg> Get(System.Linq.Expressions.Expression<Func<Ca_TipoTransferenciasEg, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_TipoTransferenciasEg;
            return this.Db.Ca_TipoTransferenciasEg.Where(filter);
        }

        public Ca_TipoTransferenciasEg GetByID(System.Linq.Expressions.Expression<Func<Ca_TipoTransferenciasEg, bool>> filter = null)
        {
            return this.Db.Ca_TipoTransferenciasEg.SingleOrDefault(filter);
        }

        public void Insert(Ca_TipoTransferenciasEg entity)
        {
            this.Db.Ca_TipoTransferenciasEg.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_TipoTransferenciasEg, bool>> filter = null)
        {
            this.Db.Ca_TipoTransferenciasEg.Remove(this.GetByID(filter));
        }

        public void Update(Ca_TipoTransferenciasEg entity)
        {
            this.Db.Ca_TipoTransferenciasEg.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



