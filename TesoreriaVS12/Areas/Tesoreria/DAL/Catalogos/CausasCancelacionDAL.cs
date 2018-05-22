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

    public class CausasCancelacionDAL : DALGeneric, IRepository<Ca_CausasCancelacion>
    {
        public IEnumerable<Ca_CausasCancelacion> Get(System.Linq.Expressions.Expression<Func<Ca_CausasCancelacion, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_CausasCancelacion;
            return this.Db.Ca_CausasCancelacion.Where(filter);
        }

        public Ca_CausasCancelacion GetByID(System.Linq.Expressions.Expression<Func<Ca_CausasCancelacion, bool>> filter = null)
        {
            return this.Db.Ca_CausasCancelacion.SingleOrDefault(filter);
        }

        public void Insert(Ca_CausasCancelacion entity)
        {
            this.Db.Ca_CausasCancelacion.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_CausasCancelacion, bool>> filter = null)
        {
            this.Db.Ca_CausasCancelacion.Remove(this.GetByID(filter));
        }

        public void Update(Ca_CausasCancelacion entity)
        {
            this.Db.Ca_CausasCancelacion.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



