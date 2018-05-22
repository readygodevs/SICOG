using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class DeCompromisosDAL : DALGeneric, IRepository<De_Compromisos>
    {

        public IEnumerable<De_Compromisos> Get(System.Linq.Expressions.Expression<Func<De_Compromisos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.De_Compromisos;
            return this.Db.De_Compromisos.Where(filter);
        }

        public De_Compromisos GetByID(System.Linq.Expressions.Expression<Func<De_Compromisos, bool>> filter = null)
        {
            return this.Db.De_Compromisos.SingleOrDefault(filter);
        }

        public void Insert(De_Compromisos entity)
        {
            this.Db.De_Compromisos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<De_Compromisos, bool>> filter = null)
        {
            this.Db.De_Compromisos.Remove(this.GetByID(filter));
        }

        public void Update(De_Compromisos entity)
        {
            this.Db.De_Compromisos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



