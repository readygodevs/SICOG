using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{
    public sealed class DeGrupoPerfilDAL : GenericDAL, IRepository<De_GrupoPerfil>
    {
        public IEnumerable<De_GrupoPerfil> Get(System.Linq.Expressions.Expression<Func<De_GrupoPerfil, bool>> filter = null)
        {
            if (filter == null)
                return this.db.De_GrupoPerfil;
            return this.db.De_GrupoPerfil.Where(filter);
        }

        public De_GrupoPerfil GetByID(System.Linq.Expressions.Expression<Func<De_GrupoPerfil, bool>> filter = null)
        {
            return this.db.De_GrupoPerfil.FirstOrDefault(filter);
        }

        public void Insert(De_GrupoPerfil entity)
        {
            this.db.De_GrupoPerfil.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<De_GrupoPerfil, bool>> filter = null)
        {
            this.db.De_GrupoPerfil.Remove(this.GetByID(filter));
        }

        public void Update(De_GrupoPerfil entity)
        {
            this.db.De_GrupoPerfil.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}