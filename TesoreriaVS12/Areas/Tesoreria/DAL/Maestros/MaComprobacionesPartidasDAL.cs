using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;
using System.Data.Objects;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Maestros
{

    public class MaComprobacionesPartidasDAL : DALGeneric, IRepository<Ma_Comprobaciones_Partida>
    {
        public IEnumerable<Ma_Comprobaciones_Partida> Get(System.Linq.Expressions.Expression<Func<Ma_Comprobaciones_Partida, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.Ma_Comprobaciones_Partida;
            return this.Db.Ma_Comprobaciones_Partida.Where(filter);
        }

        public Ma_Comprobaciones_Partida GetByID(System.Linq.Expressions.Expression<Func<Ma_Comprobaciones_Partida, bool>> filter = null)
        {
            return this.Db.Ma_Comprobaciones_Partida.SingleOrDefault(filter);
        }

        public void Insert(Ma_Comprobaciones_Partida entity)
        {
            this.Db.Ma_Comprobaciones_Partida.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Ma_Comprobaciones_Partida, bool>> filter = null)
        {
            this.Db.Ma_Comprobaciones_Partida.Remove(this.GetByID(filter));
        }

        public void Update(Ma_Comprobaciones_Partida entity)
        {
            this.Db.Ma_Comprobaciones_Partida.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}



