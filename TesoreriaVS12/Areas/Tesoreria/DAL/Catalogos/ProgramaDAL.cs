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

    public class ProgramaDAL : DALGeneric, IRepository<CA_Programas>
    {       
        public IEnumerable<CA_Programas> Get(System.Linq.Expressions.Expression<Func<CA_Programas, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.CA_Programas;
            return this.Db.CA_Programas.Where(filter);
        }

        public CA_Programas GetByID(System.Linq.Expressions.Expression<Func<CA_Programas, bool>> filter = null)
        {
            return this.Db.CA_Programas.SingleOrDefault(filter);
        }

        public void Insert(CA_Programas entity)
        {
            this.Db.CA_Programas.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<CA_Programas, bool>> filter = null)
        {
            this.Db.CA_Programas.Remove(this.GetByID(filter));
        }

        public void Update(CA_Programas entity)
        {
            this.Db.CA_Programas.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



