using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Detalles
{
    public class DeBeneficiariosContactosDAL : DALGeneric, IRepository<DE_BeneficiarioContactos>
    {
        public IEnumerable<DE_BeneficiarioContactos> Get(System.Linq.Expressions.Expression<Func<DE_BeneficiarioContactos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.DE_BeneficiarioContactos;
            return this.Db.DE_BeneficiarioContactos.Where(filter);
        }

        public DE_BeneficiarioContactos GetByID(System.Linq.Expressions.Expression<Func<DE_BeneficiarioContactos, bool>> filter = null)
        {
            return this.Db.DE_BeneficiarioContactos.SingleOrDefault(filter);
        }

        public void Insert(DE_BeneficiarioContactos entity)
        {
            this.Db.DE_BeneficiarioContactos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<DE_BeneficiarioContactos, bool>> filter = null)
        {
            this.Db.DE_BeneficiarioContactos.Remove(this.GetByID(filter));
        }

        public void Update(DE_BeneficiarioContactos entity)
        {
            this.Db.DE_BeneficiarioContactos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}