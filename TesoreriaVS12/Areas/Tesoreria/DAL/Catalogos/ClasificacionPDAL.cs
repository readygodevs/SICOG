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

    public class ClasificacionPDAL : DALGeneric, IRepository<Ca_ClasProgramatica>
    {

        public IEnumerable<Ca_ClasProgramatica> Get(System.Linq.Expressions.Expression<Func<Ca_ClasProgramatica, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_ClasProgramatica;
            return this.Db.Ca_ClasProgramatica.Where(filter);
        }

        public Ca_ClasProgramatica GetByID(System.Linq.Expressions.Expression<Func<Ca_ClasProgramatica, bool>> filter = null)
        {
            return this.Db.Ca_ClasProgramatica.SingleOrDefault(filter);
        }

        public void Insert(Ca_ClasProgramatica entity)
        {
            this.Db.Ca_ClasProgramatica.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_ClasProgramatica, bool>> filter = null)
        {
            this.Db.Ca_ClasProgramatica.Remove(this.GetByID(filter));
        }

        public void Update(Ca_ClasProgramatica entity)
        {
            this.Db.Ca_ClasProgramatica.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



