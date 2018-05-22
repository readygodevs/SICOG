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

    public class BeneficiariosCuentasDAL : DALGeneric, IRepository<CA_BeneficiariosCuentas>
    {

        public IEnumerable<CA_BeneficiariosCuentas> Get(System.Linq.Expressions.Expression<Func<CA_BeneficiariosCuentas, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.CA_BeneficiariosCuentas;
            return this.Db.CA_BeneficiariosCuentas.Where(filter);
        }

        public CA_BeneficiariosCuentas GetByID(System.Linq.Expressions.Expression<Func<CA_BeneficiariosCuentas, bool>> filter = null)
        {
            return this.Db.CA_BeneficiariosCuentas.SingleOrDefault(filter);
        }

        public void Insert(CA_BeneficiariosCuentas entity)
        {
            this.Db.CA_BeneficiariosCuentas.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<CA_BeneficiariosCuentas, bool>> filter = null)
        {
            this.Db.CA_BeneficiariosCuentas.Remove(this.GetByID(filter));
        }

        public void Update(CA_BeneficiariosCuentas entity)
        {
            this.Db.CA_BeneficiariosCuentas.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



