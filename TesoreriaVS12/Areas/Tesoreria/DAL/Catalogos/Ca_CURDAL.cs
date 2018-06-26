using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{
    public class Ca_CURDAL : DALGeneric, IRepository<CA_CUR>
    {
        public IEnumerable<CA_CUR> Get(System.Linq.Expressions.Expression<Func<CA_CUR, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.CA_CUR;
            return this.Db.CA_CUR.Where(filter);
        }

        public CA_CUR GetByID(System.Linq.Expressions.Expression<Func<CA_CUR, bool>> filter = null)
        {
            return this.Db.CA_CUR.SingleOrDefault(filter);
        }

        public CA_CUR GetByID2(string id_conc,string niv4,string niv5,string niv6,
            string niv7,string niv8,string niv9,string niv10,string niv11,string niv12)
        {
            List<CA_CUR> registros_cur = this.Db.CA_CUR.Where(x => x.Id_Concepto == id_conc &&
              x.nivel4 == niv4 &&
              x.nivel5 == niv5 &&
              x.nivel6 == niv6 &&
              x.nivel7 == niv7 &&
              x.nivel8 == niv8 &&
              x.nivel9 == niv9 &&
              x.nivel10 == niv10 &&
              x.nivel11 == niv11 &&
              x.Nivel12 == niv12).ToList();
            if (registros_cur.Count == 0)
            {
                return new CA_CUR();
            }
            else
            {
                return registros_cur[0];
            }
            
        }

        public void Insert(CA_CUR entity)
        {
            this.Db.CA_CUR.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<CA_CUR, bool>> filter = null)
        {
            this.Db.CA_CUR.Remove(this.GetByID(filter));
        }

        public void Update(CA_CUR entity)
        {
            this.Db.CA_CUR.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}