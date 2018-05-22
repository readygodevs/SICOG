using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{
    public sealed class DeOpcionesGruposDAL : GenericDAL, IRepository<DE_OpcionesGrupos>
    {
        public IEnumerable<DE_OpcionesGrupos> Get(System.Linq.Expressions.Expression<Func<DE_OpcionesGrupos, bool>> filter = null)
        {
            if (filter == null)
                return this.db.DE_OpcionesGrupos;
            return this.db.DE_OpcionesGrupos.Where(filter);
        }

        public DE_OpcionesGrupos GetByID(System.Linq.Expressions.Expression<Func<DE_OpcionesGrupos, bool>> filter = null)
        {
            return this.db.DE_OpcionesGrupos.FirstOrDefault(filter);
        }

        public void Insert(DE_OpcionesGrupos entity)
        {
            this.db.DE_OpcionesGrupos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<DE_OpcionesGrupos, bool>> filter = null)
        {
            this.db.DE_OpcionesGrupos.Remove(this.GetByID(filter));
        }

        public void Update(DE_OpcionesGrupos entity)
        {
            this.db.DE_OpcionesGrupos.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}