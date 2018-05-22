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

    public class PercepDeducDAL : DALGeneric, IRepository<Ca_Percep_Deduc>
    {

        public IEnumerable<Ca_Percep_Deduc> Get(System.Linq.Expressions.Expression<Func<Ca_Percep_Deduc, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_Percep_Deduc;
            return this.Db.Ca_Percep_Deduc.Where(filter);
        }

        public Ca_Percep_Deduc GetByID(System.Linq.Expressions.Expression<Func<Ca_Percep_Deduc, bool>> filter = null)
        {
            return this.Db.Ca_Percep_Deduc.SingleOrDefault(filter);
        }

        public void Insert(Ca_Percep_Deduc entity)
        {
            this.Db.Ca_Percep_Deduc.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_Percep_Deduc, bool>> filter = null)
        {
            this.Db.Ca_Percep_Deduc.Remove(this.GetByID(filter));
        }

        public void Update(Ca_Percep_Deduc entity)
        {
            this.Db.Ca_Percep_Deduc.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



