using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class TipoMovBancariosDAL : DALGeneric, IRepository<Ca_TipoMovBancarios>
    {
        
        public IEnumerable<Ca_TipoMovBancarios> Get(System.Linq.Expressions.Expression<Func<Ca_TipoMovBancarios, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_TipoMovBancarios;
            return this.Db.Ca_TipoMovBancarios.Where(filter);
        }

        public Ca_TipoMovBancarios GetByID(System.Linq.Expressions.Expression<Func<Ca_TipoMovBancarios, bool>> filter = null)
        {
            return this.Db.Ca_TipoMovBancarios.SingleOrDefault(filter);
        }

        public void Insert(Ca_TipoMovBancarios entity)
        {
            this.Db.Ca_TipoMovBancarios.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_TipoMovBancarios, bool>> filter = null)
        {
            this.Db.Ca_TipoMovBancarios.Remove(this.GetByID(filter));
        }

        public void Update(Ca_TipoMovBancarios entity)
        {
            this.Db.Ca_TipoMovBancarios.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



