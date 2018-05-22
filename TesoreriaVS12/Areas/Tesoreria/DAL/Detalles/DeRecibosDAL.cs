using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;


namespace TesoreriaVS12.Areas.Tesoreria.DAL.Detalles
{
    public class DeRecibosDAL : DALGeneric, IRepository<De_ReciboIngresos>
    {
        public IEnumerable<De_ReciboIngresos> Get(System.Linq.Expressions.Expression<Func<De_ReciboIngresos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.De_ReciboIngresos;
            return this.Db.De_ReciboIngresos.Where(filter);
        }

        public De_ReciboIngresos GetByID(System.Linq.Expressions.Expression<Func<De_ReciboIngresos, bool>> filter = null)
        {
            return this.Db.De_ReciboIngresos.SingleOrDefault(filter);
        }

        public void Insert(De_ReciboIngresos entity)
        {
            this.Db.De_ReciboIngresos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<De_ReciboIngresos, bool>> filter = null)
        {
            this.Db.De_ReciboIngresos.Remove(this.GetByID(filter));
        }
        public void DeleteAll(List<De_ReciboIngresos> entity)
        {
            foreach (De_ReciboIngresos item in entity)
            {
                this.Db.De_ReciboIngresos.Remove(item);
            }
        }
        public void Update(De_ReciboIngresos entity)
        {
            this.Db.De_ReciboIngresos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}