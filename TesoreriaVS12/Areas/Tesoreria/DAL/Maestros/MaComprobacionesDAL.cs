using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;
using System.Data.Objects;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Maestros
{

    public class MaComprobacionesDAL : DALGeneric, IRepository<Ma_Comprobaciones>
    {
        public IEnumerable<Ma_Comprobaciones> Get(System.Linq.Expressions.Expression<Func<Ma_Comprobaciones, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ma_Comprobaciones;
            return this.Db.Ma_Comprobaciones.Where(filter);
        }

        public Ma_Comprobaciones GetByID(System.Linq.Expressions.Expression<Func<Ma_Comprobaciones, bool>> filter = null)
        {
            return this.Db.Ma_Comprobaciones.SingleOrDefault(filter);
        }

        public void Insert(Ma_Comprobaciones entity)
        {
            this.Db.Ma_Comprobaciones.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ma_Comprobaciones, bool>> filter = null)
        {
            this.Db.Ma_Comprobaciones.Remove(this.GetByID(filter));
        }

        public void Update(Ma_Comprobaciones entity)
        {
            this.Db.Ma_Comprobaciones.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



