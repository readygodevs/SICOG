using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Detalles
{
    public class DeBancoChequeDAL : DALGeneric, IRepository<DE_Banco_Cheque>
    {
        public IEnumerable<DE_Banco_Cheque> Get(System.Linq.Expressions.Expression<Func<DE_Banco_Cheque, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.DE_Banco_Cheque;
            return this.Db.DE_Banco_Cheque.Where(filter);
        }

        public DE_Banco_Cheque GetByID(System.Linq.Expressions.Expression<Func<DE_Banco_Cheque, bool>> filter = null)
        {
            return this.Db.DE_Banco_Cheque.SingleOrDefault(filter);
        }

        public void Insert(DE_Banco_Cheque entity)
        {
            this.Db.DE_Banco_Cheque.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<DE_Banco_Cheque, bool>> filter = null)
        {
            this.Db.DE_Banco_Cheque.Remove(this.GetByID(filter));
        }

        public void Update(DE_Banco_Cheque entity)
        {
            this.Db.DE_Banco_Cheque.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }
        public void DeleteAll(List<DE_Banco_Cheque> entity)
        {
            foreach(DE_Banco_Cheque item in entity)
            {
                this.Db.DE_Banco_Cheque.Remove(item);
            }
        }
        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}