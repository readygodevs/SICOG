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

    public class ProcesoDAL : DALGeneric, IRepository<Ca_Proyecto>
    {

        public IEnumerable<Ca_Proyecto> Get(System.Linq.Expressions.Expression<Func<Ca_Proyecto, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_Proyecto;
            return this.Db.Ca_Proyecto.Where(filter);
        }

        public Ca_Proyecto GetByID(System.Linq.Expressions.Expression<Func<Ca_Proyecto, bool>> filter = null)
        {
            return this.Db.Ca_Proyecto.SingleOrDefault(filter);
        }

        public void Insert(Ca_Proyecto entity)
        {
            this.Db.Ca_Proyecto.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_Proyecto, bool>> filter = null)
        {
            this.Db.Ca_Proyecto.Remove(this.GetByID(filter));
        }

        public void Update(Ca_Proyecto entity)
        {
            this.Db.Ca_Proyecto.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



