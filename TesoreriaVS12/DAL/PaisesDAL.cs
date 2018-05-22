using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{

    public class PaisesDAL : GenericDAL, IRepository<Ca_Paises>
    {

        public IEnumerable<Ca_Paises> Get(System.Linq.Expressions.Expression<Func<Ca_Paises, bool>> filter = null)
        {
            if (filter == null)
                return this.db.Ca_Paises;
            return this.db.Ca_Paises.Where(filter);
        }

        public Ca_Paises GetByID(System.Linq.Expressions.Expression<Func<Ca_Paises, bool>> filter = null)
        {
            return this.db.Ca_Paises.SingleOrDefault(filter);
        }

        public void Insert(Ca_Paises entity)
        {
            this.db.Ca_Paises.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_Paises, bool>> filter = null)
        {
            this.db.Ca_Paises.Remove(this.GetByID(filter));
        }

        public void Update(Ca_Paises entity)
        {
            this.db.Ca_Paises.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}



