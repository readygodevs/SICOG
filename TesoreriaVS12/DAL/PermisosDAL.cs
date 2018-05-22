using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{
    public sealed class PermisosDAL : GenericDAL, IRepository<DE_Permisos>
    {
        public IEnumerable<DE_Permisos> Get(System.Linq.Expressions.Expression<Func<DE_Permisos, bool>> filter = null)
        {
            if (filter == null)
                return this.db.DE_Permisos;
            return this.db.DE_Permisos.Where(filter);
        }

        public DE_Permisos GetByID(System.Linq.Expressions.Expression<Func<DE_Permisos, bool>> filter = null)
        {
            return this.db.DE_Permisos.FirstOrDefault(filter);
        }

        public void Insert(DE_Permisos entity)
        {
            this.db.DE_Permisos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<DE_Permisos, bool>> filter = null)
        {
            this.db.DE_Permisos.Remove(this.GetByID(filter));
        }

        public void Update(DE_Permisos entity)
        {
            this.db.DE_Permisos.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}