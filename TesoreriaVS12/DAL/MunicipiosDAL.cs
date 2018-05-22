using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{

    public class MunicipiosDAL : GenericDAL, IRepository<Ca_Municipios>
    {

        
        public IEnumerable<Ca_Municipios> Get(System.Linq.Expressions.Expression<Func<Ca_Municipios, bool>> filter = null)
        {
            if (filter == null)
                return this.db.Ca_Municipios;
            return this.db.Ca_Municipios.Where(filter);
        }

        public Ca_Municipios GetByID(System.Linq.Expressions.Expression<Func<Ca_Municipios, bool>> filter = null)
        {
            return this.db.Ca_Municipios.SingleOrDefault(filter);
        }

        public void Insert(Ca_Municipios entity)
        {
            this.db.Ca_Municipios.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_Municipios, bool>> filter = null)
        {
            this.db.Ca_Municipios.Remove(this.GetByID(filter));
        }

        public void Update(Ca_Municipios entity)
        {
            this.db.Ca_Municipios.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}



