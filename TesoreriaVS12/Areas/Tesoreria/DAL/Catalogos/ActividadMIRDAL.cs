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

    public class ActividadMIRDAL : DALGeneric, IRepository<Ca_Actividad>
    {
        public IEnumerable<Ca_Actividad> Get(System.Linq.Expressions.Expression<Func<Ca_Actividad, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_Actividad;
            return this.Db.Ca_Actividad.Where(filter);
        }

        public Ca_Actividad GetByID(System.Linq.Expressions.Expression<Func<Ca_Actividad, bool>> filter = null)
        {
             return this.Db.Ca_Actividad.SingleOrDefault(filter);
        }

        public void Insert(Ca_Actividad entity)
        {
            this.Db.Ca_Actividad.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_Actividad, bool>> filter = null)
        {
            this.Db.Ca_Actividad.Remove(this.GetByID(filter));
        }

        public void Update(Ca_Actividad entity)
        {
            this.Db.Ca_Actividad.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



