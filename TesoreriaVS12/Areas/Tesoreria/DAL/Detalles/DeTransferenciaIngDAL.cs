using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Detalles
{
    public class DeTransferenciaIngDAL : DALGeneric, IRepository<De_TransferenciaIng>
    {

        public IEnumerable<De_TransferenciaIng> Get(System.Linq.Expressions.Expression<Func<De_TransferenciaIng, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.De_TransferenciaIng;
            return this.Db.De_TransferenciaIng.Where(filter);
        }

        public De_TransferenciaIng GetByID(System.Linq.Expressions.Expression<Func<De_TransferenciaIng, bool>> filter = null)
        {
            return this.Db.De_TransferenciaIng.SingleOrDefault(filter);
        }

        public void Insert(De_TransferenciaIng entity)
        {
            this.Db.De_TransferenciaIng.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<De_TransferenciaIng, bool>> filter = null)
        {
            this.Db.De_TransferenciaIng.Remove(this.GetByID(filter));
        }
        public void DeleteAll(List<De_TransferenciaIng> entity)
        {
            foreach (De_TransferenciaIng item in entity)
            {
                this.Db.De_TransferenciaIng.Remove(item);
            }
        }
        public void Update(De_TransferenciaIng entity)
        {
            this.Db.De_TransferenciaIng.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}