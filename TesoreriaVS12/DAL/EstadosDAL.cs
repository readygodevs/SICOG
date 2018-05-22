using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{

    public class EstadosDAL : GenericDAL, IRepository<Ca_Estados>
    {
        public IEnumerable<Ca_Estados> Get(System.Linq.Expressions.Expression<Func<Ca_Estados, bool>> filter = null)
        {
            if (filter == null)
                return this.db.Ca_Estados;
            return this.db.Ca_Estados.Where(filter);
        }

        public Ca_Estados GetByID(System.Linq.Expressions.Expression<Func<Ca_Estados, bool>> filter = null)
        {
            return this.db.Ca_Estados.SingleOrDefault(filter);
        }

        public void Insert(Ca_Estados entity)
        {
            this.db.Ca_Estados.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_Estados, bool>> filter = null)
        {
            this.db.Ca_Estados.Remove(this.GetByID(filter));
        }

        public void Update(Ca_Estados entity)
        {
            this.db.Ca_Estados.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}



