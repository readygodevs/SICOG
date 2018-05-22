using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Maestros
{
    public class MaTransferenciasIngDAL : DALGeneric, IRepository<Ma_TransferenciasIng>
    {
        public IEnumerable<Ma_TransferenciasIng> Get(System.Linq.Expressions.Expression<Func<Ma_TransferenciasIng, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ma_TransferenciasIng;
            return this.Db.Ma_TransferenciasIng.Where(filter);
        }

        public Ma_TransferenciasIng GetByID(System.Linq.Expressions.Expression<Func<Ma_TransferenciasIng, bool>> filter = null)
        {
            return this.Db.Ma_TransferenciasIng.SingleOrDefault(filter);
        }

        public void Insert(Ma_TransferenciasIng entity)
        {
            this.Db.Ma_TransferenciasIng.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ma_TransferenciasIng, bool>> filter = null)
        {
            this.Db.Ma_TransferenciasIng.Remove(this.GetByID(filter));
        }

        public void Update(Ma_TransferenciasIng entity)
        {
            this.Db.Ma_TransferenciasIng.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }
        public Ma_TransferenciasIng GetFisrt(System.Linq.Expressions.Expression<Func<Ma_TransferenciasIng, bool>> filter = null)
        {
            return this.Db.Ma_TransferenciasIng.FirstOrDefault(filter);
        }
        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}