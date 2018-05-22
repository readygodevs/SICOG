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

    public class ClasificaBeneficiariosDAL : DALGeneric, IRepository<Ca_ClasificaBeneficiarios>
    {
        public IEnumerable<Ca_ClasificaBeneficiarios> Get(System.Linq.Expressions.Expression<Func<Ca_ClasificaBeneficiarios, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_ClasificaBeneficiarios;
            return this.Db.Ca_ClasificaBeneficiarios.Where(filter);
        }

        public Ca_ClasificaBeneficiarios GetByID(System.Linq.Expressions.Expression<Func<Ca_ClasificaBeneficiarios, bool>> filter = null)
        {
            return this.Db.Ca_ClasificaBeneficiarios.SingleOrDefault(filter);
        }

        public void Insert(Ca_ClasificaBeneficiarios entity)
        {
            this.Db.Ca_ClasificaBeneficiarios.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_ClasificaBeneficiarios, bool>> filter = null)
        {
            this.Db.Ca_ClasificaBeneficiarios.Remove(this.GetByID(filter));
        }

        public void Update(Ca_ClasificaBeneficiarios entity)
        {
            this.Db.Ca_ClasificaBeneficiarios.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



