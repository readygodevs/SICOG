using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{
    public sealed class GruposDAL : GenericDAL, IRepository<CA_Grupos>
    {
        public IEnumerable<CA_Grupos> Get(System.Linq.Expressions.Expression<Func<CA_Grupos, bool>> filter = null)
        {
            if (filter == null)
                return this.db.CA_Grupos;
            return this.db.CA_Grupos.Where(filter);
        }

        public CA_Grupos GetByID(System.Linq.Expressions.Expression<Func<CA_Grupos, bool>> filter = null)
        {
            return this.db.CA_Grupos.FirstOrDefault(filter);
        }

        public void Insert(CA_Grupos entity)
        {
            this.db.CA_Grupos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<CA_Grupos, bool>> filter = null)
        {
            this.db.CA_Grupos.Remove(this.GetByID(filter));
        }

        public void Update(CA_Grupos entity)
        {
            this.db.CA_Grupos.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}