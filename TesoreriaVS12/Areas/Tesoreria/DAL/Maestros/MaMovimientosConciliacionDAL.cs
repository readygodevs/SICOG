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
    public class MaMovimientosConciliacionDAL : DALGeneric, IRepository<Ma_MovimientosConciliacion>
    {
        public IEnumerable<Ma_MovimientosConciliacion> Get(System.Linq.Expressions.Expression<Func<Ma_MovimientosConciliacion, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ma_MovimientosConciliacion;
            return this.Db.Ma_MovimientosConciliacion.Where(filter);
        }

        public Ma_MovimientosConciliacion GetByID(System.Linq.Expressions.Expression<Func<Ma_MovimientosConciliacion, bool>> filter = null)
        {
            return this.Db.Ma_MovimientosConciliacion.SingleOrDefault(filter);
        }

        public void Insert(Ma_MovimientosConciliacion entity)
        {
            this.Db.Ma_MovimientosConciliacion.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ma_MovimientosConciliacion, bool>> filter = null)
        {
            this.Db.Ma_MovimientosConciliacion.Remove(this.GetByID(filter));
        }
        public void DeleteAll(List<Ma_MovimientosConciliacion> entity)
        {
            foreach (Ma_MovimientosConciliacion item in entity)
            {
                this.Db.Ma_MovimientosConciliacion.Remove(item);
            }
        }
        public void Update(Ma_MovimientosConciliacion entity)
        {
            this.Db.Ma_MovimientosConciliacion.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}