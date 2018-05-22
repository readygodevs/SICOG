using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Detalles
{
    public class DeEvolucionDAL : DALGeneric, IRepository<DE_Evolucion>
    {
        public IEnumerable<DE_Evolucion> Get(System.Linq.Expressions.Expression<Func<DE_Evolucion, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.DE_Evolucion;
            return this.Db.DE_Evolucion.Where(filter);
        }

        public DE_Evolucion GetByID(System.Linq.Expressions.Expression<Func<DE_Evolucion, bool>> filter = null)
        {
            return this.Db.DE_Evolucion.SingleOrDefault(filter);
        }

        public void Insert(DE_Evolucion entity)
        {
            this.Db.DE_Evolucion.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<DE_Evolucion, bool>> filter = null)
        {
            this.Db.DE_Evolucion.Remove(this.GetByID(filter));
        }

        public void Update(DE_Evolucion entity)
        {
            this.Db.DE_Evolucion.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }
        public void DeleteAll(List<DE_Evolucion> entity)
        {
            foreach (DE_Evolucion item in entity)
            {
                this.Db.DE_Evolucion.Remove(item);
            }
        }
        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}