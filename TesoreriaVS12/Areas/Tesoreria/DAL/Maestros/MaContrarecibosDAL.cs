using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Maestros
{

    public class MaContrarecibosDAL : DALGeneric, IRepository<Ma_Contrarecibos>
    {
        public IEnumerable<Ma_Contrarecibos> Get(System.Linq.Expressions.Expression<Func<Ma_Contrarecibos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ma_Contrarecibos;
            return this.Db.Ma_Contrarecibos.Where(filter);
        }

        public Ma_Contrarecibos GetByID(System.Linq.Expressions.Expression<Func<Ma_Contrarecibos, bool>> filter = null)
        {
            return this.Db.Ma_Contrarecibos.SingleOrDefault(filter);
        }

        public void Insert(Ma_Contrarecibos entity)
        {
            this.Db.Ma_Contrarecibos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ma_Contrarecibos, bool>> filter = null)
        {
            this.Db.Ma_Contrarecibos.Remove(this.GetByID(filter));
        }

        public void Update(Ma_Contrarecibos entity)
        {
            this.Db.Ma_Contrarecibos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



