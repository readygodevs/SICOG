using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Detalles
{
    public class DeBancoDAL : DALGeneric, IRepository<DE_Bancos>
    {
        public IEnumerable<DE_Bancos> Get(System.Linq.Expressions.Expression<Func<DE_Bancos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.DE_Bancos;
            return this.Db.DE_Bancos.Where(filter);
        }

        public DE_Bancos GetByID(System.Linq.Expressions.Expression<Func<DE_Bancos, bool>> filter = null)
        {
            return this.Db.DE_Bancos.SingleOrDefault(filter);
        }

        public void Insert(DE_Bancos entity)
        {
            this.Db.DE_Bancos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<DE_Bancos, bool>> filter = null)
        {
            this.Db.DE_Bancos.Remove(this.GetByID(filter));
        }

        public void Update(DE_Bancos entity)
        {
            this.Db.DE_Bancos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }
        public void DeleteAll(List<DE_Bancos> entity)
        {
            foreach (DE_Bancos item in entity)
            {
                this.Db.DE_Bancos.Remove(item);
            }
        }
        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}