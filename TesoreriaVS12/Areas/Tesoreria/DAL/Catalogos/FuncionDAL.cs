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

    public class FuncionDAL : DALGeneric, IRepository<Ca_Funciones>
    {

        public IEnumerable<Ca_Funciones> Get(System.Linq.Expressions.Expression<Func<Ca_Funciones, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_Funciones;
            return this.Db.Ca_Funciones.Where(filter);
        }

        public Ca_Funciones GetByID(System.Linq.Expressions.Expression<Func<Ca_Funciones, bool>> filter = null)
        {
            return this.Db.Ca_Funciones.SingleOrDefault(filter);
        }

        public void Insert(Ca_Funciones entity)
        {
            this.Db.Ca_Funciones.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_Funciones, bool>> filter = null)
        {
            this.Db.Ca_Funciones.Remove(this.GetByID(filter));
        }

        public void Update(Ca_Funciones entity)
        {
            this.Db.Ca_Funciones.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



