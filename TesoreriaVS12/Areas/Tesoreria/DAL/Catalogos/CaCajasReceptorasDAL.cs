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
    public class CaCajasReceptorasDAL : DALGeneric, IRepository<Ca_CajasReceptoras>
    {
        public IEnumerable<Ca_CajasReceptoras> Get(System.Linq.Expressions.Expression<Func<Ca_CajasReceptoras, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_CajasReceptoras;
            return this.Db.Ca_CajasReceptoras.Where(filter);
        }

        public Ca_CajasReceptoras GetByID(System.Linq.Expressions.Expression<Func<Ca_CajasReceptoras, bool>> filter = null)
        {
            return this.Db.Ca_CajasReceptoras.SingleOrDefault(filter);
        }

        public void Insert(Ca_CajasReceptoras entity)
        {
            this.Db.Ca_CajasReceptoras.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_CajasReceptoras, bool>> filter = null)
        {
            this.Db.Ca_CajasReceptoras.Remove(this.GetByID(filter));
        }

        public void Update(Ca_CajasReceptoras entity)
        {
            this.Db.Ca_CajasReceptoras.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}