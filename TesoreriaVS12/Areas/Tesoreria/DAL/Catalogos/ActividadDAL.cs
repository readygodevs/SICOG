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

    public class ActividadDAL : DALGeneric, IRepository<Ca_ActividadesInst>
    {

        public IEnumerable<Ca_ActividadesInst> Get(System.Linq.Expressions.Expression<Func<Ca_ActividadesInst, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_ActividadesInst;
            return this.Db.Ca_ActividadesInst.Where(filter);
        }

        public Ca_ActividadesInst GetByID(System.Linq.Expressions.Expression<Func<Ca_ActividadesInst, bool>> filter = null)
        {
            return this.Db.Ca_ActividadesInst.SingleOrDefault(filter);
        }

        public void Insert(Ca_ActividadesInst entity)
        {
            this.Db.Ca_ActividadesInst.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_ActividadesInst, bool>> filter = null)
        {
            this.Db.Ca_ActividadesInst.Remove(this.GetByID(filter));
        }

        public void Update(Ca_ActividadesInst entity)
        {
            this.Db.Ca_ActividadesInst.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



