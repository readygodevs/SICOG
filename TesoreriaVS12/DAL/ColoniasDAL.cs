using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{

    public class ColoniasDAL : GenericDAL, IRepository<Ca_Colonias>
    {

        public IEnumerable<Ca_Colonias> Get(System.Linq.Expressions.Expression<Func<Ca_Colonias, bool>> filter = null)
        {
            if (filter == null)
                return this.db.Ca_Colonias;
            return this.db.Ca_Colonias.Where(filter);
        }

        public Ca_Colonias GetByID(System.Linq.Expressions.Expression<Func<Ca_Colonias, bool>> filter = null)
        {
            return this.db.Ca_Colonias.SingleOrDefault(filter);
        }

        public void Insert(Ca_Colonias entity)
        {
            this.db.Ca_Colonias.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_Colonias, bool>> filter = null)
        {
            this.db.Ca_Colonias.Remove(this.GetByID(filter));
        }

        public void Update(Ca_Colonias entity)
        {
            this.db.Ca_Colonias.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}



