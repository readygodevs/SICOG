using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{

    public class PersonasDAL : GenericDAL, IRepository<CA_Personas>
    {
        public IEnumerable<CA_Personas> Get(System.Linq.Expressions.Expression<Func<CA_Personas, bool>> filter = null)
        {
            if (filter == null)
                return this.db.CA_Personas;
            return this.db.CA_Personas.Where(filter);
        }

        public CA_Personas GetByID(System.Linq.Expressions.Expression<Func<CA_Personas, bool>> filter = null)
        {
            return this.db.CA_Personas.SingleOrDefault(filter);
        }

        public void Insert(CA_Personas entity)
        {
            this.db.CA_Personas.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<CA_Personas, bool>> filter = null)
        {
            this.db.CA_Personas.Remove(this.GetByID(filter));
        }

        public void Update(CA_Personas entity)
        {
            this.db.CA_Personas.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}



