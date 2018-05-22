using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{
    public class BdTesoreriasDAL : IRepository<VW_Bases>
    {
        private ControlGeneralContainer db { get; set; }
        public BdTesoreriasDAL()
        {
            if (db == null) db = new ControlGeneralContainer();
        }

        public IEnumerable<VW_Bases> Get(System.Linq.Expressions.Expression<Func<VW_Bases, bool>> filter = null)
        {
            if (filter == null)
                return this.db.VW_Bases;
            return this.db.VW_Bases.Where(filter);
        }

        public VW_Bases GetByID(System.Linq.Expressions.Expression<Func<VW_Bases, bool>> filter = null)
        {
            return this.db.VW_Bases.FirstOrDefault(filter);
        }

        public void Insert(VW_Bases entity)
        {
            this.db.VW_Bases.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<VW_Bases, bool>> filter = null)
        {
            this.db.VW_Bases.Remove(this.GetByID(filter));
        }

        public void Update(VW_Bases entity)
        {
            this.db.VW_Bases.Attach(entity);
            this.db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}