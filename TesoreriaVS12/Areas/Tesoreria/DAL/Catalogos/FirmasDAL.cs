using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{
    public class FirmasDAL : DALGeneric, IRepository<TA_Firmas>
    {
        public IEnumerable<TA_Firmas> Get(System.Linq.Expressions.Expression<Func<TA_Firmas, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.TA_Firmas;
            return this.Db.TA_Firmas.Where(filter);
        }

        public TA_Firmas GetByID(System.Linq.Expressions.Expression<Func<TA_Firmas, bool>> filter = null)
        {
            return this.Db.TA_Firmas.SingleOrDefault(filter);
        }

        public void Insert(TA_Firmas entity)
        {
            this.Db.TA_Firmas.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<TA_Firmas, bool>> filter = null)
        {
            this.Db.TA_Firmas.Remove(this.GetByID(filter));
        }

        public void Update(TA_Firmas entity)
        {
            this.Db.TA_Firmas.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}