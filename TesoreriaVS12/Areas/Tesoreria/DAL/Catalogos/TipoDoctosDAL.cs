using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class TipoDoctosDAL : DALGeneric, IRepository<Ca_TipoDoctos>
    {
        public IEnumerable<Ca_TipoDoctos> Get(System.Linq.Expressions.Expression<Func<Ca_TipoDoctos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_TipoDoctos;
            return this.Db.Ca_TipoDoctos.Where(filter);
        }

        public Ca_TipoDoctos GetByID(System.Linq.Expressions.Expression<Func<Ca_TipoDoctos, bool>> filter = null)
        {
            return this.Db.Ca_TipoDoctos.SingleOrDefault(filter);
        }

        public void Insert(Ca_TipoDoctos entity)
        {
            this.Db.Ca_TipoDoctos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_TipoDoctos, bool>> filter = null)
        {
            this.Db.Ca_TipoDoctos.Remove(this.GetByID(filter));
        }

        public void Update(Ca_TipoDoctos entity)
        {
            this.Db.Ca_TipoDoctos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



