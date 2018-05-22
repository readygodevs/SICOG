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

    public class GirosDAL : DALGeneric, IRepository<CA_Giros>
    {

        public IEnumerable<CA_Giros> Get(System.Linq.Expressions.Expression<Func<CA_Giros, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.CA_Giros;
            return this.Db.CA_Giros.Where(filter);
        }

        public CA_Giros GetByID(System.Linq.Expressions.Expression<Func<CA_Giros, bool>> filter = null)
        {
            return this.Db.CA_Giros.SingleOrDefault(filter);
        }

        public void Insert(CA_Giros entity)
        {
            this.Db.CA_Giros.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<CA_Giros, bool>> filter = null)
        {
            this.Db.CA_Giros.Remove(this.GetByID(filter));
        }

        public void Update(CA_Giros entity)
        {
            this.Db.CA_Giros.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



