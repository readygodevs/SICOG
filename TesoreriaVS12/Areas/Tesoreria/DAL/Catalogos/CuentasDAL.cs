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

    public class CuentasDAL : DALGeneric, IRepository<CA_Cuentas>
    {

        public IEnumerable<CA_Cuentas> Get(System.Linq.Expressions.Expression<Func<CA_Cuentas, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.CA_Cuentas;
            return this.Db.CA_Cuentas.Where(filter);
        }

        public CA_Cuentas GetByID(System.Linq.Expressions.Expression<Func<CA_Cuentas, bool>> filter = null)
        {
            return this.Db.CA_Cuentas.SingleOrDefault(filter);
        }

        public void Insert(CA_Cuentas entity)
        {
            this.Db.CA_Cuentas.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<CA_Cuentas, bool>> filter = null)
        {
            this.Db.CA_Cuentas.Remove(this.GetByID(filter));
        }

        public void Update(CA_Cuentas entity)
        {
            this.Db.CA_Cuentas.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



