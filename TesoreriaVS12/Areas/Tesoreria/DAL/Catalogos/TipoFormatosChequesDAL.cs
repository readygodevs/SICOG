using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class TipoFormatoChequesDAL : DALGeneric, IRepository<Ca_TipoFormatosCheques>
    {
        
        public IEnumerable<Ca_TipoFormatosCheques> Get(System.Linq.Expressions.Expression<Func<Ca_TipoFormatosCheques, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_TipoFormatosCheques;
            return this.Db.Ca_TipoFormatosCheques.Where(filter);
        }

        public Ca_TipoFormatosCheques GetByID(System.Linq.Expressions.Expression<Func<Ca_TipoFormatosCheques, bool>> filter = null)
        {
            return this.Db.Ca_TipoFormatosCheques.SingleOrDefault(filter);
        }

        public void Insert(Ca_TipoFormatosCheques entity)
        {
            this.Db.Ca_TipoFormatosCheques.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_TipoFormatosCheques, bool>> filter = null)
        {
            this.Db.Ca_TipoFormatosCheques.Remove(this.GetByID(filter));
        }

        public void Update(Ca_TipoFormatosCheques entity)
        {
            this.Db.Ca_TipoFormatosCheques.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



