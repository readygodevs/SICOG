using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Maestros
{

    public class MaTrasnferenciasDAL : DALGeneric, IRepository<Ma_Transferencias>
    {
        public IEnumerable<Ma_Transferencias> Get(System.Linq.Expressions.Expression<Func<Ma_Transferencias, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ma_Transferencias;
            return this.Db.Ma_Transferencias.Where(filter);
        }

        public Ma_Transferencias GetByID(System.Linq.Expressions.Expression<Func<Ma_Transferencias, bool>> filter = null)
        {
            return this.Db.Ma_Transferencias.SingleOrDefault(filter);
        }

        public void Insert(Ma_Transferencias entity)
        {
            this.Db.Ma_Transferencias.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ma_Transferencias, bool>> filter = null)
        {
            this.Db.Ma_Transferencias.Remove(this.GetByID(filter));
        }

        public void Update(Ma_Transferencias entity)
        {
            this.Db.Ma_Transferencias.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }
        public Ma_Transferencias GetFisrt(System.Linq.Expressions.Expression<Func<Ma_Transferencias, bool>> filter = null)
        {
            return this.Db.Ma_Transferencias.FirstOrDefault(filter);
        }
        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



