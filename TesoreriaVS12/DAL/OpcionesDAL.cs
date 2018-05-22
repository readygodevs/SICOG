using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{
    public sealed class OpcionesDAL : GenericDAL, IRepository<CA_Opciones>
    {
        public IEnumerable<CA_Opciones> Get(System.Linq.Expressions.Expression<Func<CA_Opciones, bool>> filter = null)
        {
            if (filter == null)
                return this.db.CA_Opciones;
            return this.db.CA_Opciones.Where(filter);
        }

        public CA_Opciones GetByID(System.Linq.Expressions.Expression<Func<CA_Opciones, bool>> filter = null)
        {
            return this.db.CA_Opciones.FirstOrDefault(filter);
        }

        public void Insert(CA_Opciones entity)
        {
            this.db.CA_Opciones.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<CA_Opciones, bool>> filter = null)
        {
            this.db.CA_Opciones.Remove(this.GetByID(filter));
        }

        public void Update(CA_Opciones entity)
        {
            this.db.CA_Opciones.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}