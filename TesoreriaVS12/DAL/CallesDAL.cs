using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{

    public class CallesDAL : GenericDAL, IRepository<Ca_Calles>
    {
        public IEnumerable<Ca_Calles> Get(System.Linq.Expressions.Expression<Func<Ca_Calles, bool>> filter = null)
        {
            if (filter == null)
                return this.db.Ca_Calles;
            return this.db.Ca_Calles.Where(filter);
        }

        public Ca_Calles GetByID(System.Linq.Expressions.Expression<Func<Ca_Calles, bool>> filter = null)
        {
            return this.db.Ca_Calles.SingleOrDefault(filter);
        }

        public void Insert(Ca_Calles entity)
        {
            this.db.Ca_Calles.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_Calles, bool>> filter = null)
        {
            this.db.Ca_Calles.Remove(this.GetByID(filter));
        }

        public void Update(Ca_Calles entity)
        {
            this.db.Ca_Calles.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}



