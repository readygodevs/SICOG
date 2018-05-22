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

    public class AccionDAL : DALGeneric, IRepository<Ca_Acciones>
    {

        public IEnumerable<Ca_Acciones> Get(System.Linq.Expressions.Expression<Func<Ca_Acciones, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_Acciones;
            return this.Db.Ca_Acciones.Where(filter);
        }

        public Ca_Acciones GetByID(System.Linq.Expressions.Expression<Func<Ca_Acciones, bool>> filter = null)
        {
            return this.Db.Ca_Acciones.SingleOrDefault(filter);
        }

        public void Insert(Ca_Acciones entity)
        {
            this.Db.Ca_Acciones.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_Acciones, bool>> filter = null)
        {
            this.Db.Ca_Acciones.Remove(this.GetByID(filter));
        }

        public void Update(Ca_Acciones entity)
        {
            this.Db.Ca_Acciones.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



