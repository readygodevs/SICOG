using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{

    public class LocalidadesDAL : GenericDAL, IRepository<Ca_Localidades>
    {

        public IEnumerable<Ca_Localidades> Get(System.Linq.Expressions.Expression<Func<Ca_Localidades, bool>> filter = null)
        {
            if (filter == null)
                return this.db.Ca_Localidades;
            return this.db.Ca_Localidades.Where(filter);
        }

        public Ca_Localidades GetByID(System.Linq.Expressions.Expression<Func<Ca_Localidades, bool>> filter = null)
        {
            return this.db.Ca_Localidades.SingleOrDefault(filter);
        }

        public void Insert(Ca_Localidades entity)
        {
            this.db.Ca_Localidades.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_Localidades, bool>> filter = null)
        {
            this.db.Ca_Localidades.Remove(this.GetByID(filter));
        }

        public void Update(Ca_Localidades entity)
        {
            this.db.Ca_Localidades.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}



