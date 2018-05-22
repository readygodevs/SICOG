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

    public class TipoBeneficiariosDAL : DALGeneric, IRepository<Ca_TipoBeneficiarios>
    {
        public IEnumerable<Ca_TipoBeneficiarios> Get(System.Linq.Expressions.Expression<Func<Ca_TipoBeneficiarios, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_TipoBeneficiarios;
            return this.Db.Ca_TipoBeneficiarios.Where(filter);
        }

        public Ca_TipoBeneficiarios GetByID(System.Linq.Expressions.Expression<Func<Ca_TipoBeneficiarios, bool>> filter = null)
        {
            return this.Db.Ca_TipoBeneficiarios.SingleOrDefault(filter);
        }

        public void Insert(Ca_TipoBeneficiarios entity)
        {
            this.Db.Ca_TipoBeneficiarios.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_TipoBeneficiarios, bool>> filter = null)
        {
            this.Db.Ca_TipoBeneficiarios.Remove(this.GetByID(filter));
        }

        public void Update(Ca_TipoBeneficiarios entity)
        {
            this.Db.Ca_TipoBeneficiarios.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



