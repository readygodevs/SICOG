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

    public class CentroRecaudadorDAL : DALGeneric, IRepository<Ca_CentroRecaudador>
    {
        public IEnumerable<Ca_CentroRecaudador> Get(System.Linq.Expressions.Expression<Func<Ca_CentroRecaudador, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ca_CentroRecaudador;
            return this.Db.Ca_CentroRecaudador.Where(filter);
        }

        public Ca_CentroRecaudador GetByID(System.Linq.Expressions.Expression<Func<Ca_CentroRecaudador, bool>> filter = null)
        {
            return this.Db.Ca_CentroRecaudador.SingleOrDefault(filter);
        }

        public void Insert(Ca_CentroRecaudador entity)
        {
            this.Db.Ca_CentroRecaudador.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ca_CentroRecaudador, bool>> filter = null)
        {
            this.Db.Ca_CentroRecaudador.Remove(this.GetByID(filter));
        }

        public void Update(Ca_CentroRecaudador entity)
        {
            this.Db.Ca_CentroRecaudador.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }
        public void DeleteAll(List<Ca_CentroRecaudador> entity)
        {
            foreach (Ca_CentroRecaudador item in entity)
            {
                this.Db.Ca_CentroRecaudador.Remove(item);
            }
        }
        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



