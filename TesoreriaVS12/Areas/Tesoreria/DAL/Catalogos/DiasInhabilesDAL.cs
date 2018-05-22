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
    public class DiasInhabilesDAL : DALGeneric, IRepository<CA_InHabil>
    {

        public IEnumerable<CA_InHabil> Get(System.Linq.Expressions.Expression<Func<CA_InHabil, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.CA_InHabil;
            return this.Db.CA_InHabil.Where(filter);
        }

        public CA_InHabil GetByID(System.Linq.Expressions.Expression<Func<CA_InHabil, bool>> filter = null)
        {
            return this.Db.CA_InHabil.SingleOrDefault(filter);
        }

        public void Insert(CA_InHabil entity)
        {
            this.Db.CA_InHabil.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<CA_InHabil, bool>> filter = null)
        {
            this.Db.CA_InHabil.Remove(this.GetByID(filter));
        }

        public void Update(CA_InHabil entity)
        {
            this.Db.CA_InHabil.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}