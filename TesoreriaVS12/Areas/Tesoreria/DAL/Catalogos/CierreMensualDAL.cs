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

    public class CierreMensualDAL : DALGeneric, IRepository<Ca_CierreMensual>
    {
        public IEnumerable<Ca_CierreMensual> Get(System.Linq.Expressions.Expression<Func<Ca_CierreMensual, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_CierreMensual;
            return this.Db.Ca_CierreMensual.Where(filter);
        }

        public Ca_CierreMensual GetByID(System.Linq.Expressions.Expression<Func<Ca_CierreMensual, bool>> filter = null)
        {
            return this.Db.Ca_CierreMensual.SingleOrDefault(filter);
        }

        public void Insert(Ca_CierreMensual entity)
        {
            this.Db.Ca_CierreMensual.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_CierreMensual, bool>> filter = null)
        {
            this.Db.Ca_CierreMensual.Remove(this.GetByID(filter));
        }

        public void Update(Ca_CierreMensual entity)
        {
            this.Db.Ca_CierreMensual.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



