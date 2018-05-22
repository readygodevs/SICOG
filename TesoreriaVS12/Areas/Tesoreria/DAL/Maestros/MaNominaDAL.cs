using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Maestros
{
    public class MaNominaDAL : DALGeneric, IRepository<Ma_Nomina>
    {
        public IEnumerable<Ma_Nomina> Get(System.Linq.Expressions.Expression<Func<Ma_Nomina, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ma_Nomina;
            return this.Db.Ma_Nomina.Where(filter);
        }

        public Ma_Nomina GetByID(System.Linq.Expressions.Expression<Func<Ma_Nomina, bool>> filter = null)
        {
            return this.Db.Ma_Nomina.SingleOrDefault(filter);
        }

        public void Insert(Ma_Nomina entity)
        {
            this.Db.Ma_Nomina.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ma_Nomina, bool>> filter = null)
        {
            this.Db.Ma_Nomina.Remove(this.GetByID(filter));
        }

        public void Update(Ma_Nomina entity)
        {
            this.Db.Ma_Nomina.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}