using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Maestros
{

    public class MaPresupuestoIngDAL : DALGeneric, IRepository<Ma_PresupuestoIng>
    {
        public IEnumerable<Ma_PresupuestoIng> Get(System.Linq.Expressions.Expression<Func<Ma_PresupuestoIng, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ma_PresupuestoIng;
            return this.Db.Ma_PresupuestoIng.Where(filter);
        }

        public Ma_PresupuestoIng GetByID(System.Linq.Expressions.Expression<Func<Ma_PresupuestoIng, bool>> filter = null)
        {
            return this.Db.Ma_PresupuestoIng.SingleOrDefault(filter);
        }

        public void Insert(Ma_PresupuestoIng entity)
        {
            this.Db.Ma_PresupuestoIng.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ma_PresupuestoIng, bool>> filter = null)
        {
            this.Db.Ma_PresupuestoIng.Remove(this.GetByID(filter));
        }

        public void Update(Ma_PresupuestoIng entity)
        {
            this.Db.Ma_PresupuestoIng.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



