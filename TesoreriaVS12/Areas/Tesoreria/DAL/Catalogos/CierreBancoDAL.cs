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

    public class CierreBancoDAL : DALGeneric, IRepository<CA_CierreBanco>
    {
        public IEnumerable<CA_CierreBanco> Get(System.Linq.Expressions.Expression<Func<CA_CierreBanco, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.CA_CierreBanco;
            return this.Db.CA_CierreBanco.Where(filter);
        }

        public CA_CierreBanco GetByID(System.Linq.Expressions.Expression<Func<CA_CierreBanco, bool>> filter = null)
        {
            return this.Db.CA_CierreBanco.SingleOrDefault(filter);
        }

        public void Insert(CA_CierreBanco entity)
        {
            this.Db.CA_CierreBanco.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<CA_CierreBanco, bool>> filter = null)
        {
            this.Db.CA_CierreBanco.Remove(this.GetByID(filter));
        }

        public void Update(CA_CierreBanco entity)
        {
            this.Db.CA_CierreBanco.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }
        public void DeleteAll(List<CA_CierreBanco> entity)
        {
            foreach (CA_CierreBanco item in entity)
            {
                this.Db.CA_CierreBanco.Remove(item);
            }
        }
        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



