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

    public class TipoContrarecibosDAL : DALGeneric, IRepository<Ca_TipoContrarecibos>
    {

        public IEnumerable<Ca_TipoContrarecibos> Get(System.Linq.Expressions.Expression<Func<Ca_TipoContrarecibos, bool>> filter = null)
        {
            if (filter == null)
                return Db.Ca_TipoContrarecibos;
            return Db.Ca_TipoContrarecibos.Where(filter);
        }

        public Ca_TipoContrarecibos GetByID(System.Linq.Expressions.Expression<Func<Ca_TipoContrarecibos, bool>> filter = null)
        {
            return this.Db.Ca_TipoContrarecibos.SingleOrDefault(filter);
        }

        public void Insert(Ca_TipoContrarecibos entity)
        {
            this.Db.Ca_TipoContrarecibos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_TipoContrarecibos, bool>> filter = null)
        {
            this.Db.Ca_TipoContrarecibos.Remove(this.GetByID(filter));
        }

        public void Update(Ca_TipoContrarecibos entity)
        {
            this.Db.Ca_TipoContrarecibos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



