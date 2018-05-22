using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Detalles
{
    public class DeDisponibilidadDAL : DALGeneric, IRepository<DE_Disponibilidad>
    {                 
        public IEnumerable<DE_Disponibilidad> Get(System.Linq.Expressions.Expression<Func<DE_Disponibilidad, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.DE_Disponibilidad;
            return this.Db.DE_Disponibilidad.Where(filter);
        }

        public DE_Disponibilidad GetByID(System.Linq.Expressions.Expression<Func<DE_Disponibilidad, bool>> filter = null)
        {
            return this.Db.DE_Disponibilidad.SingleOrDefault(filter);
        }

        public void Insert(DE_Disponibilidad entity)
        {
            this.Db.DE_Disponibilidad.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<DE_Disponibilidad, bool>> filter = null)
        {
            this.Db.DE_Disponibilidad.Remove(this.GetByID(filter));
        }

        public void Update(DE_Disponibilidad entity)
        {
            this.Db.DE_Disponibilidad.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }

    }
}