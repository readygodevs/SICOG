using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class DeContrarecibosDAL : IRepository<De_Contrarecibos>
    {

        private BD_TesoreriaEntities db { get; set; }
        public DeContrarecibosDAL()
        {
            if (db == null) db = new BD_TesoreriaEntities();
        }

        public IEnumerable<De_Contrarecibos> Get(System.Linq.Expressions.Expression<Func<De_Contrarecibos, bool>> filter = null)
        {
            if (filter == null)
                return this.db.De_Contrarecibos;
            return this.db.De_Contrarecibos.Where(filter);
        }

        public De_Contrarecibos GetByID(System.Linq.Expressions.Expression<Func<De_Contrarecibos, bool>> filter = null)
        {
            return this.db.De_Contrarecibos.SingleOrDefault(filter);
        }

        public void Insert(De_Contrarecibos entity)
        {
            this.db.De_Contrarecibos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<De_Contrarecibos, bool>> filter = null)
        {
            this.db.De_Contrarecibos.Remove(this.GetByID(filter));
        }

        public void Update(De_Contrarecibos entity)
        {
            this.db.De_Contrarecibos.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}



