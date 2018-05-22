using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class TipoGastosDAL : DALGeneric, IRepository<Ca_TipoGastos>
    {
        public IEnumerable<Ca_TipoGastos> Get(System.Linq.Expressions.Expression<Func<Ca_TipoGastos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_TipoGastos;
            return this.Db.Ca_TipoGastos.Where(filter);
        }

        public Ca_TipoGastos GetByID(System.Linq.Expressions.Expression<Func<Ca_TipoGastos, bool>> filter = null)
        {
            return this.Db.Ca_TipoGastos.SingleOrDefault(filter);
        }

        public void Insert(Ca_TipoGastos entity)
        {
            this.Db.Ca_TipoGastos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_TipoGastos, bool>> filter = null)
        {
            this.Db.Ca_TipoGastos.Remove(this.GetByID(filter));
        }

        public void Update(Ca_TipoGastos entity)
        {
            this.Db.Ca_TipoGastos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



