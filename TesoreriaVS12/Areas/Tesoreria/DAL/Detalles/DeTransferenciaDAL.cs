using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class DeTransferenciaDAL : DALGeneric, IRepository<De_Transferencia>
    {

        public IEnumerable<De_Transferencia> Get(System.Linq.Expressions.Expression<Func<De_Transferencia, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.De_Transferencia;
            return this.Db.De_Transferencia.Where(filter);
        }

        public De_Transferencia GetByID(System.Linq.Expressions.Expression<Func<De_Transferencia, bool>> filter = null)
        {
            return this.Db.De_Transferencia.SingleOrDefault(filter);
        }

        public void Insert(De_Transferencia entity)
        {
            this.Db.De_Transferencia.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<De_Transferencia, bool>> filter = null)
        {
            this.Db.De_Transferencia.Remove(this.GetByID(filter));
        }
        public void DeleteAll(List<De_Transferencia> entity)
        {
            foreach (De_Transferencia item in entity)
            {
                this.Db.De_Transferencia.Remove(item);
            }
        }
        public void Update(De_Transferencia entity)
        {
            this.Db.De_Transferencia.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



