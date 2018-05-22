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

    public class BancosDAL : DALGeneric, IRepository<Ca_Bancos>
    {
        public IEnumerable<Ca_Bancos> Get(System.Linq.Expressions.Expression<Func<Ca_Bancos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_Bancos;
            return this.Db.Ca_Bancos.Where(filter);
        }

        public Ca_Bancos GetByID(System.Linq.Expressions.Expression<Func<Ca_Bancos, bool>> filter = null)
        {
            return this.Db.Ca_Bancos.SingleOrDefault(filter);
        }

        public void Insert(Ca_Bancos entity)
        {
            this.Db.Ca_Bancos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_Bancos, bool>> filter = null)
        {
            this.Db.Ca_Bancos.Remove(this.GetByID(filter));
        }

        public void Update(Ca_Bancos entity)
        {
            this.Db.Ca_Bancos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



