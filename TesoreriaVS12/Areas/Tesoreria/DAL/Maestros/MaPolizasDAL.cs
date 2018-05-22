using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Maestros
{

    public class MaPolizasDAL : DALGeneric, IRepository<Ma_Polizas>
    {
        public IEnumerable<Ma_Polizas> Get(System.Linq.Expressions.Expression<Func<Ma_Polizas, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ma_Polizas;
            return this.Db.Ma_Polizas.Where(filter);
        }

        public Ma_Polizas GetByID(System.Linq.Expressions.Expression<Func<Ma_Polizas, bool>> filter = null)
        {
            return this.Db.Ma_Polizas.SingleOrDefault(filter);
        }

        public void Insert(Ma_Polizas entity)
        {
            this.Db.Ma_Polizas.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ma_Polizas, bool>> filter = null)
        {
            this.Db.Ma_Polizas.Remove(this.GetByID(filter));
        }

        public void Update(Ma_Polizas entity)
        {
            this.Db.Ma_Polizas.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



