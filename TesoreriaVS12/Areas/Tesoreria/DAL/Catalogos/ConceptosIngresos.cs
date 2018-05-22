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

    public class ConceptosIngresosDAL : DALGeneric, IRepository<Ca_ConceptosIngresos>
    {

        public IEnumerable<Ca_ConceptosIngresos> Get(System.Linq.Expressions.Expression<Func<Ca_ConceptosIngresos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_ConceptosIngresos;
            return this.Db.Ca_ConceptosIngresos.Where(filter);
        }

        public Ca_ConceptosIngresos GetByID(System.Linq.Expressions.Expression<Func<Ca_ConceptosIngresos, bool>> filter = null)
        {
            return this.Db.Ca_ConceptosIngresos.SingleOrDefault(filter);
        }

        public void Insert(Ca_ConceptosIngresos entity)
        {
            this.Db.Ca_ConceptosIngresos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_ConceptosIngresos, bool>> filter = null)
        {
            this.Db.Ca_ConceptosIngresos.Remove(this.GetByID(filter));
        }

        public void Update(Ca_ConceptosIngresos entity)
        {
            this.Db.Ca_ConceptosIngresos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



