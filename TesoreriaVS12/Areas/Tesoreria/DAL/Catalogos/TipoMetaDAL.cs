using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class TipoMetaDAL : DALGeneric, IRepository<Ca_TipoMeta>
    {
        public IEnumerable<Ca_TipoMeta> Get(System.Linq.Expressions.Expression<Func<Ca_TipoMeta, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_TipoMeta;
            return this.Db.Ca_TipoMeta.Where(filter);
        }

        public Ca_TipoMeta GetByID(System.Linq.Expressions.Expression<Func<Ca_TipoMeta, bool>> filter = null)
        {
            return this.Db.Ca_TipoMeta.SingleOrDefault(filter);
        }

        public void Insert(Ca_TipoMeta entity)
        {
            this.Db.Ca_TipoMeta.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_TipoMeta, bool>> filter = null)
        {
            this.Db.Ca_TipoMeta.Remove(this.GetByID(filter));
        }

        public void Update(Ca_TipoMeta entity)
        {
            this.Db.Ca_TipoMeta.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



