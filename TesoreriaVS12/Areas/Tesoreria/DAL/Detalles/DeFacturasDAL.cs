using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Detalles
{
    public class DeFacturasDAL : DALGeneric, IRepository<De_CR_Facturas>
    {
        public IEnumerable<De_CR_Facturas> Get(System.Linq.Expressions.Expression<Func<De_CR_Facturas, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.De_CR_Facturas;
            return this.Db.De_CR_Facturas.Where(filter);
        }

        public De_CR_Facturas GetByID(System.Linq.Expressions.Expression<Func<De_CR_Facturas, bool>> filter = null)
        {
            return this.Db.De_CR_Facturas.SingleOrDefault(filter);
        }

        public void Insert(De_CR_Facturas entity)
        {
            this.Db.De_CR_Facturas.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<De_CR_Facturas, bool>> filter = null)
        {
            this.Db.De_CR_Facturas.Remove(this.GetByID(filter));
        }

        public void Update(De_CR_Facturas entity)
        {
            this.Db.De_CR_Facturas.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}