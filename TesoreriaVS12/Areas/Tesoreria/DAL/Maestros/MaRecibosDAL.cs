using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Maestros
{
    public class MaRecibosDAL : DALGeneric, IRepository<Ma_ReciboIngresos>
    {
        public IEnumerable<Ma_ReciboIngresos> Get(System.Linq.Expressions.Expression<Func<Ma_ReciboIngresos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ma_ReciboIngresos;
            return this.Db.Ma_ReciboIngresos.Where(filter);
        }

        public Ma_ReciboIngresos GetByID(System.Linq.Expressions.Expression<Func<Ma_ReciboIngresos, bool>> filter = null)
        {
            return this.Db.Ma_ReciboIngresos.SingleOrDefault(filter);
        }

        public void Insert(Ma_ReciboIngresos entity)
        {
            this.Db.Ma_ReciboIngresos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ma_ReciboIngresos, bool>> filter = null)
        {
            this.Db.Ma_ReciboIngresos.Remove(this.GetByID(filter));
        }

        public void Update(Ma_ReciboIngresos entity)
        {
            this.Db.Ma_ReciboIngresos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }
        public Ma_ReciboIngresos GetFisrt(System.Linq.Expressions.Expression<Func<Ma_ReciboIngresos, bool>> filter = null)
        {
            return this.Db.Ma_ReciboIngresos.FirstOrDefault(filter);
        }
        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}