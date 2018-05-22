using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{
    public class UsuariosDAL : IRepository<CA_Usuarios>
    {

        private ControlGeneralContainer db { get; set; }
        public UsuariosDAL()
        {
            if (db == null) db = new ControlGeneralContainer();
        }
        public IEnumerable<CA_Usuarios> Get(System.Linq.Expressions.Expression<Func<CA_Usuarios, bool>> filter = null)
        {
            if (filter == null)
                return this.db.CA_Usuarios;
            return this.db.CA_Usuarios.Where(filter);
        }

        public CA_Usuarios GetByID(System.Linq.Expressions.Expression<Func<CA_Usuarios, bool>> filter = null)
        {
            return this.db.CA_Usuarios.FirstOrDefault(filter);
        }

        public void Insert(CA_Usuarios entity)
        {
            this.db.CA_Usuarios.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<CA_Usuarios, bool>> filter = null)
        {
            this.db.CA_Usuarios.Remove(this.GetByID(filter));
        }

        public void Update(CA_Usuarios entity)
        {
            this.db.CA_Usuarios.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;   
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}