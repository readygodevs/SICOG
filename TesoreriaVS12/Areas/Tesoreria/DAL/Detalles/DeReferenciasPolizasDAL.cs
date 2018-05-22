using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Detalles
{
    public class DeReferenciasPolizasDAL : DALGeneric, IRepository<De_ReferenciasPolizas>
    {
        public IEnumerable<De_ReferenciasPolizas> Get(System.Linq.Expressions.Expression<Func<De_ReferenciasPolizas, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.De_ReferenciasPolizas;
            return this.Db.De_ReferenciasPolizas.Where(filter);
        }

        public De_ReferenciasPolizas GetByID(System.Linq.Expressions.Expression<Func<De_ReferenciasPolizas, bool>> filter = null)
        {
            return this.Db.De_ReferenciasPolizas.SingleOrDefault(filter);
        }

        public void Insert(De_ReferenciasPolizas entity)
        {
            this.Db.De_ReferenciasPolizas.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<De_ReferenciasPolizas, bool>> filter = null)
        {
            this.Db.De_ReferenciasPolizas.Remove(this.GetByID(filter));
        }

        public void Update(De_ReferenciasPolizas entity)
        {
            this.Db.De_ReferenciasPolizas.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }
        public void DeleteAll(List<De_ReferenciasPolizas> entity)
        {
            foreach (De_ReferenciasPolizas item in entity)
            {
                this.Db.De_ReferenciasPolizas.Remove(item);
            }
        }
        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}