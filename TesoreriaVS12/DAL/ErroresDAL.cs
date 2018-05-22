using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{
    public class ErroresDAL : GenericDAL, IRepository<Ca_MensajesError>
    {

        public IEnumerable<Ca_MensajesError> Get(System.Linq.Expressions.Expression<Func<Ca_MensajesError, bool>> filter = null)
        {
            if (filter == null)
                return this.db.Ca_MensajesError;
            return this.db.Ca_MensajesError.Where(filter);
        }

        public Ca_MensajesError GetByID(System.Linq.Expressions.Expression<Func<Ca_MensajesError, bool>> filter = null)
        {
            return this.db.Ca_MensajesError.FirstOrDefault(filter);
        }

        public void Insert(Ca_MensajesError entity)
        {
            this.db.Ca_MensajesError.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_MensajesError, bool>> filter = null)
        {
            this.db.Ca_MensajesError.Remove(this.GetByID(filter));
        }

        public void Update(Ca_MensajesError entity)
        {
            this.db.Ca_MensajesError.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}