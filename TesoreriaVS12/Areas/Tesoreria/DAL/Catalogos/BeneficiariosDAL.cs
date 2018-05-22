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

    public class BeneficiariosDAL : DALGeneric, IRepository<Ca_Beneficiarios>
    {

        public IEnumerable<Ca_Beneficiarios> Get(System.Linq.Expressions.Expression<Func<Ca_Beneficiarios, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_Beneficiarios;
            return this.Db.Ca_Beneficiarios.Where(filter);
        }

        public Ca_Beneficiarios GetByID(System.Linq.Expressions.Expression<Func<Ca_Beneficiarios, bool>> filter = null)
        {
            return this.Db.Ca_Beneficiarios.SingleOrDefault(filter);
        }

        public void Insert(Ca_Beneficiarios entity)
        {
            this.Db.Ca_Beneficiarios.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_Beneficiarios, bool>> filter = null)
        {
            this.Db.Ca_Beneficiarios.Remove(this.GetByID(filter));
        }

        public void Update(Ca_Beneficiarios entity)
        {
            this.Db.Ca_Beneficiarios.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



