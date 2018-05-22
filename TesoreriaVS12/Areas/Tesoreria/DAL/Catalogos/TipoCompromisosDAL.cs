using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class TipoCompromisosDAL : DALGeneric, IRepository<Ca_TipoCompromisos>
    {

        public IEnumerable<Ca_TipoCompromisos> Get(System.Linq.Expressions.Expression<Func<Ca_TipoCompromisos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_TipoCompromisos;
            return this.Db.Ca_TipoCompromisos.Where(filter);
        }

        public Ca_TipoCompromisos GetByID(System.Linq.Expressions.Expression<Func<Ca_TipoCompromisos, bool>> filter = null)
        {
            return this.Db.Ca_TipoCompromisos.SingleOrDefault(filter);
        }

        public void Insert(Ca_TipoCompromisos entity)
        {
            this.Db.Ca_TipoCompromisos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_TipoCompromisos, bool>> filter = null)
        {
            this.Db.Ca_TipoCompromisos.Remove(this.GetByID(filter));
        }

        public void Update(Ca_TipoCompromisos entity)
        {
            this.Db.Ca_TipoCompromisos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



