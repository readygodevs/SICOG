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

    public class ParametrosDAL : DALGeneric, IRepository<CA_Parametros>
    {

        public IEnumerable<CA_Parametros> Get(System.Linq.Expressions.Expression<Func<CA_Parametros, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.CA_Parametros;
            return this.Db.CA_Parametros.Where(filter);
        }

        public CA_Parametros GetByID(System.Linq.Expressions.Expression<Func<CA_Parametros, bool>> filter = null)
        {
            return this.Db.CA_Parametros.SingleOrDefault(filter);
        }

        public void Insert(CA_Parametros entity)
        {
            this.Db.CA_Parametros.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<CA_Parametros, bool>> filter = null)
        {
            this.Db.CA_Parametros.Remove(this.GetByID(filter));
        }

        public void Update(CA_Parametros entity)
        {
            this.Db.CA_Parametros.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



