using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class DeCompromisosHDAL : DALGeneric, IRepository<De_Compromisos_H>
    {

        public IEnumerable<De_Compromisos_H> Get(System.Linq.Expressions.Expression<Func<De_Compromisos_H, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.De_Compromisos_H;
            return this.Db.De_Compromisos_H.Where(filter);
        }

        public De_Compromisos_H GetByID(System.Linq.Expressions.Expression<Func<De_Compromisos_H, bool>> filter = null)
        {
            return this.Db.De_Compromisos_H.SingleOrDefault(filter);
        }

        public void Insert(De_Compromisos_H entity)
        {
            this.Db.De_Compromisos_H.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<De_Compromisos_H, bool>> filter = null)
        {
            this.Db.De_Compromisos_H.Remove(this.GetByID(filter));
        }

        public void Update(De_Compromisos_H entity)
        {
            this.Db.De_Compromisos_H.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



