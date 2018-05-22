using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class DePolizasDAL : IRepository<De_Polizas>
    {

        private BD_TesoreriaEntities db { get; set; }
        public DePolizasDAL()
        {
            if (db == null) db = new BD_TesoreriaEntities();
        }

        public IEnumerable<De_Polizas> Get(System.Linq.Expressions.Expression<Func<De_Polizas, bool>> filter = null)
        {
            if (filter == null)
                return this.db.De_Polizas;
            return this.db.De_Polizas.Where(filter);
        }

        public De_Polizas GetByID(System.Linq.Expressions.Expression<Func<De_Polizas, bool>> filter = null)
        {
            return this.db.De_Polizas.SingleOrDefault(filter);
        }

        public void Insert(De_Polizas entity)
        {
            this.db.De_Polizas.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<De_Polizas, bool>> filter = null)
        {
            this.db.De_Polizas.Remove(this.GetByID(filter));
        }

        public void Update(De_Polizas entity)
        {
            this.db.De_Polizas.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}



