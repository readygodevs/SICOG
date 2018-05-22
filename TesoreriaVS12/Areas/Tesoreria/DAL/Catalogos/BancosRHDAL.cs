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

    public class BancosRHDAL : DALGeneric, IRepository<Ca_BancosRH>
    {

        public IEnumerable<Ca_BancosRH> Get(System.Linq.Expressions.Expression<Func<Ca_BancosRH, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_BancosRH;
            return this.Db.Ca_BancosRH.Where(filter);
        }

        public Ca_BancosRH GetByID(System.Linq.Expressions.Expression<Func<Ca_BancosRH, bool>> filter = null)
        {
            return this.Db.Ca_BancosRH.SingleOrDefault(filter);
        }

        public void Insert(Ca_BancosRH entity)
        {
            this.Db.Ca_BancosRH.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_BancosRH, bool>> filter = null)
        {
            this.Db.Ca_BancosRH.Remove(this.GetByID(filter));
        }

        public void Update(Ca_BancosRH entity)
        {
            this.Db.Ca_BancosRH.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



