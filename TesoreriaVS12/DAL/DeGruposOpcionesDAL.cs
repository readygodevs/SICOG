using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{
    public sealed class DeGruposOpcionesDAL : GenericDAL, IRepository<DE_GruposOpciones>
    {
        public IEnumerable<DE_GruposOpciones> Get(System.Linq.Expressions.Expression<Func<DE_GruposOpciones, bool>> filter = null)
        {
            if (filter == null)
                return this.db.DE_GruposOpciones;
            return this.db.DE_GruposOpciones.Where(filter);
        }

        public DE_GruposOpciones GetByID(System.Linq.Expressions.Expression<Func<DE_GruposOpciones, bool>> filter = null)
        {
            return this.db.DE_GruposOpciones.FirstOrDefault(filter);
        }

        public void Insert(DE_GruposOpciones entity)
        {
            this.db.DE_GruposOpciones.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<DE_GruposOpciones, bool>> filter = null)
        {
            this.db.DE_GruposOpciones.Remove(this.GetByID(filter));
        }

        public void Update(DE_GruposOpciones entity)
        {
            this.db.DE_GruposOpciones.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}