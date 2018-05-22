using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Detalles
{

    public class DeBeneficiariosGirosDAL : DALGeneric, IRepository<De_Beneficiarios_Giros>
    {

        public IEnumerable<De_Beneficiarios_Giros> Get(System.Linq.Expressions.Expression<Func<De_Beneficiarios_Giros, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.De_Beneficiarios_Giros;
            return this.Db.De_Beneficiarios_Giros.Where(filter);
        }

        public De_Beneficiarios_Giros GetByID(System.Linq.Expressions.Expression<Func<De_Beneficiarios_Giros, bool>> filter = null)
        {
            return this.Db.De_Beneficiarios_Giros.SingleOrDefault(filter);
        }

        public void Insert(De_Beneficiarios_Giros entity)
        {
            this.Db.De_Beneficiarios_Giros.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<De_Beneficiarios_Giros, bool>> filter = null)
        {
            this.Db.De_Beneficiarios_Giros.Remove(this.GetByID(filter));
        }

        public void Update(De_Beneficiarios_Giros entity)
        {
            this.Db.De_Beneficiarios_Giros.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}