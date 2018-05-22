using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{
    public class Ca_CURDAL : DALGeneric, IRepository<CA_CUR>
    {
        public IEnumerable<CA_CUR> Get(System.Linq.Expressions.Expression<Func<CA_CUR, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.CA_CUR;
            return this.Db.CA_CUR.Where(filter);
        }

        public CA_CUR GetByID(System.Linq.Expressions.Expression<Func<CA_CUR, bool>> filter = null)
        {
            return this.Db.CA_CUR.SingleOrDefault(filter);
        }

        public void Insert(CA_CUR entity)
        {
            this.Db.CA_CUR.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<CA_CUR, bool>> filter = null)
        {
            this.Db.CA_CUR.Remove(this.GetByID(filter));
        }

        public void Update(CA_CUR entity)
        {
            this.Db.CA_CUR.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}