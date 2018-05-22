using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class DeComprobacionesDAL : IRepository<De_Comprobaciones>
    {

        private BD_TesoreriaEntities db { get; set; }
        public DeComprobacionesDAL()
        {
            if (db == null) db = new BD_TesoreriaEntities();
        }

        public IEnumerable<De_Comprobaciones> Get(System.Linq.Expressions.Expression<Func<De_Comprobaciones, bool>> filter = null)
        {
            if (filter == null)
                return this.db.De_Comprobaciones;
            return this.db.De_Comprobaciones.Where(filter);
        }

        public De_Comprobaciones GetByID(System.Linq.Expressions.Expression<Func<De_Comprobaciones, bool>> filter = null)
        {
            return this.db.De_Comprobaciones.SingleOrDefault(filter);
        }

        public void Insert(De_Comprobaciones entity)
        {
            this.db.De_Comprobaciones.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<De_Comprobaciones, bool>> filter = null)
        {
            this.db.De_Comprobaciones.Remove(this.GetByID(filter));
        }

        public void Update(De_Comprobaciones entity)
        {
            this.db.De_Comprobaciones.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}



