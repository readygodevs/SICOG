using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{
    public sealed class DeAccionesPermisosDAL : GenericDAL, IRepository<DE_AccionesPermisos>
    {
        public IEnumerable<DE_AccionesPermisos> Get(System.Linq.Expressions.Expression<Func<DE_AccionesPermisos, bool>> filter = null)
        {
            if (filter == null)
                return this.db.DE_AccionesPermisos;
            return this.db.DE_AccionesPermisos.Where(filter);
        }

        public DE_AccionesPermisos GetByID(System.Linq.Expressions.Expression<Func<DE_AccionesPermisos, bool>> filter = null)
        {
            return this.db.DE_AccionesPermisos.FirstOrDefault(filter);
        }

        public void Insert(DE_AccionesPermisos entity)
        {
            this.db.DE_AccionesPermisos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<DE_AccionesPermisos, bool>> filter = null)
        {
            this.db.DE_AccionesPermisos.Remove(this.GetByID(filter));
        }

        public void Update(DE_AccionesPermisos entity)
        {
            this.db.DE_AccionesPermisos.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}