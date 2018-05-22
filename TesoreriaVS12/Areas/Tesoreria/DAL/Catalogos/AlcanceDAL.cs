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

    public class AlcanceDAL : DALGeneric, IRepository<Ca_AlcanceGeo>
    {
        
        public IEnumerable<Ca_AlcanceGeo> Get(System.Linq.Expressions.Expression<Func<Ca_AlcanceGeo, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_AlcanceGeo;
            return this.Db.Ca_AlcanceGeo.Where(filter);
        }

        public Ca_AlcanceGeo GetByID(System.Linq.Expressions.Expression<Func<Ca_AlcanceGeo, bool>> filter = null)
        {
            return this.Db.Ca_AlcanceGeo.SingleOrDefault(filter);
        }

        public void Insert(Ca_AlcanceGeo entity)
        {
            this.Db.Ca_AlcanceGeo.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_AlcanceGeo, bool>> filter = null)
        {
            this.Db.Ca_AlcanceGeo.Remove(this.GetByID(filter));
        }

        public void Update(Ca_AlcanceGeo entity)
        {
            this.Db.Ca_AlcanceGeo.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



