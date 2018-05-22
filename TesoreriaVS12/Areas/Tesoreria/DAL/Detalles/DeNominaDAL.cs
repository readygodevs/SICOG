using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class DeNominaDAL :DALGeneric,IRepository<De_Nomina>
    {

        public IEnumerable<De_Nomina> Get(System.Linq.Expressions.Expression<Func<De_Nomina, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.De_Nomina;
            return this.Db.De_Nomina.Where(filter);
        }

        public De_Nomina GetByID(System.Linq.Expressions.Expression<Func<De_Nomina, bool>> filter = null)
        {
            return this.Db.De_Nomina.SingleOrDefault(filter);
        }

        public void Insert(De_Nomina entity)
        {
            this.Db.De_Nomina.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<De_Nomina, bool>> filter = null)
        {
            this.Db.De_Nomina.Remove(this.GetByID(filter));
        }

        public void Update(De_Nomina entity)
        {
            this.Db.De_Nomina.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }

    }
}



