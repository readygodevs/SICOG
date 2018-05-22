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

    public class CuentasBancariasDAL : DALGeneric, IRepository<Ca_CuentasBancarias>
    {

        public IEnumerable<Ca_CuentasBancarias> Get(System.Linq.Expressions.Expression<Func<Ca_CuentasBancarias, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_CuentasBancarias;
            return this.Db.Ca_CuentasBancarias.Where(filter);
        }

        public Ca_CuentasBancarias GetByID(System.Linq.Expressions.Expression<Func<Ca_CuentasBancarias, bool>> filter = null)
        {
            return this.Db.Ca_CuentasBancarias.SingleOrDefault(filter);
        }

        public void Insert(Ca_CuentasBancarias entity)
        {
            this.Db.Ca_CuentasBancarias.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_CuentasBancarias, bool>> filter = null)
        {
            this.Db.Ca_CuentasBancarias.Remove(this.GetByID(filter));
        }

        public void Update(Ca_CuentasBancarias entity)
        {
            this.Db.Ca_CuentasBancarias.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



