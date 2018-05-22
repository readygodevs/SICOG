using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Maestros
{

    public class MaPresupuestoEgDAL : DALGeneric, IRepository<MA_PresupuestoEg>
    {
        public IEnumerable<MA_PresupuestoEg> Get(System.Linq.Expressions.Expression<Func<MA_PresupuestoEg, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.MA_PresupuestoEg;
            return this.Db.MA_PresupuestoEg.Where(filter);
        }

        public MA_PresupuestoEg GetByID(System.Linq.Expressions.Expression<Func<MA_PresupuestoEg, bool>> filter = null)
        {
            return this.Db.MA_PresupuestoEg.SingleOrDefault(filter);
        }

        public void Insert(MA_PresupuestoEg entity)
        {
            this.Db.MA_PresupuestoEg.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<MA_PresupuestoEg, bool>> filter = null)
        {
            this.Db.MA_PresupuestoEg.Remove(this.GetByID(filter));
        }

        public void Update(MA_PresupuestoEg entity)
        {
            this.Db.MA_PresupuestoEg.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



