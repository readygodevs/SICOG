using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Maestros
{
    public class MaEstadosCuentaDAL: DALGeneric, IRepository<Ma_EstadosCuenta>
    {
        public IEnumerable<Ma_EstadosCuenta> Get(System.Linq.Expressions.Expression<Func<Ma_EstadosCuenta, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ma_EstadosCuenta;
            return this.Db.Ma_EstadosCuenta.Where(filter);
        }

        public Ma_EstadosCuenta GetByID(System.Linq.Expressions.Expression<Func<Ma_EstadosCuenta, bool>> filter = null)
        {
            return this.Db.Ma_EstadosCuenta.SingleOrDefault(filter);
        }

        public void Insert(Ma_EstadosCuenta entity)
        {
            this.Db.Ma_EstadosCuenta.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ma_EstadosCuenta, bool>> filter = null)
        {
            this.Db.Ma_EstadosCuenta.Remove(this.GetByID(filter));
        }

        public void DeleteAll(List<Ma_EstadosCuenta> entity)
        {
            foreach (Ma_EstadosCuenta item in entity)
            {
                this.Db.Ma_EstadosCuenta.Remove(item);
            }
        }

        public void Update(Ma_EstadosCuenta entity)
        {
            this.Db.Ma_EstadosCuenta.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }
        public Ma_EstadosCuenta GetFisrt(System.Linq.Expressions.Expression<Func<Ma_EstadosCuenta, bool>> filter = null)
        {
            return this.Db.Ma_EstadosCuenta.FirstOrDefault(filter);
        }
        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}
