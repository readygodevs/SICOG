using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Detalles
{
    public class DeArchivosContraRecibosDAL : DALGeneric, IRepository<DE_ContrarecibosArchivos>
    {
        public IEnumerable<DE_ContrarecibosArchivos> Get(System.Linq.Expressions.Expression<Func<DE_ContrarecibosArchivos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.DE_ContrarecibosArchivos;
            return this.Db.DE_ContrarecibosArchivos.Where(filter);
        }

        public DE_ContrarecibosArchivos GetByID(System.Linq.Expressions.Expression<Func<DE_ContrarecibosArchivos, bool>> filter = null)
        {
            return this.Db.DE_ContrarecibosArchivos.SingleOrDefault(filter);
        }

        public void Insert(DE_ContrarecibosArchivos entity)
        {
            this.Db.DE_ContrarecibosArchivos.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<DE_ContrarecibosArchivos, bool>> filter = null)
        {
            this.Db.DE_ContrarecibosArchivos.Remove(this.GetByID(filter));
        }

        public void Update(DE_ContrarecibosArchivos entity)
        {
            this.Db.DE_ContrarecibosArchivos.Attach(entity);
            this.Db.Entry(entity).State = EntityState.Modified;
        }
        public void DeleteAll(List<DE_ContrarecibosArchivos> entity)
        {
            foreach (DE_ContrarecibosArchivos item in entity)
            {
                this.Db.DE_ContrarecibosArchivos.Remove(item);
            }
        }
        public void Save()
        {
            this.Db.SaveChanges();
        }
    }
}