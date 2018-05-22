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

    public class ObjetoGDAL : DALGeneric, IRepository<CA_ObjetoGasto>
    {

        public IEnumerable<CA_ObjetoGasto> Get(System.Linq.Expressions.Expression<Func<CA_ObjetoGasto, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.CA_ObjetoGasto;
            return this.Db.CA_ObjetoGasto.Where(filter);
        }

        public CA_ObjetoGasto GetByID(System.Linq.Expressions.Expression<Func<CA_ObjetoGasto, bool>> filter = null)
        {
            return this.Db.CA_ObjetoGasto.SingleOrDefault(filter);
        }

        public void Insert(CA_ObjetoGasto entity)
        {
            this.Db.CA_ObjetoGasto.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<CA_ObjetoGasto, bool>> filter = null)
        {
            this.Db.CA_ObjetoGasto.Remove(this.GetByID(filter));
        }

        public void Update(CA_ObjetoGasto entity)
        {
            this.Db.CA_ObjetoGasto.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



