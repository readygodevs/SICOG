using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{
    public class PerfilesDAL : IRepository<CA_Perfiles>
    {

        private ControlGeneralContainer db { get; set; }
        public PerfilesDAL() {
            if (db == null) db = new ControlGeneralContainer();
        }

        public IEnumerable<CA_Perfiles> Get(System.Linq.Expressions.Expression<Func<CA_Perfiles, bool>> filter = null)
        {
            if (filter == null)
                return this.db.CA_Perfiles;
            return this.db.CA_Perfiles.Where(filter);
        }

        public CA_Perfiles GetByID(System.Linq.Expressions.Expression<Func<CA_Perfiles, bool>> filter = null)
        {
            return this.db.CA_Perfiles.FirstOrDefault(filter);
        }

        public void Insert(CA_Perfiles entity)
        {
            this.db.CA_Perfiles.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<CA_Perfiles, bool>> filter = null)
        {
            this.db.CA_Perfiles.Remove(this.GetByID(filter));
        }

        public void Update(CA_Perfiles entity)
        {
            this.db.CA_Perfiles.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}