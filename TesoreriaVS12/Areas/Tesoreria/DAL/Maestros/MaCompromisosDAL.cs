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

    public class MaCompromisosDAL : DALGeneric, IRepository<Ma_Compromisos>
    {
        public IEnumerable<Ma_Compromisos> Get(System.Linq.Expressions.Expression<Func<Ma_Compromisos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ma_Compromisos;
            return this.Db.Ma_Compromisos.Where(filter);
        }

        public Ma_Compromisos GetByID(System.Linq.Expressions.Expression<Func<Ma_Compromisos, bool>> filter = null)
        {
            return this.Db.Ma_Compromisos.SingleOrDefault(filter);
        }

        public void Insert(Ma_Compromisos entity)
        {
            this.Db.Ma_Compromisos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ma_Compromisos, bool>> filter = null)
        {
            this.Db.Ma_Compromisos.Remove(this.GetByID(filter));
        }

        public void Update(Ma_Compromisos entity)
        {
            this.Db.Ma_Compromisos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }        

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



