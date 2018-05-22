using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{
    public class VWPermisosDAL : GenericDAL
    {

        public IEnumerable<VW_Permisos> Get(System.Linq.Expressions.Expression<Func<VW_Permisos, bool>> filter = null)
        {
            if (filter == null)
                return this.db.VW_Permisos;
            return this.db.VW_Permisos.Where(filter);
        }

        public VW_Permisos GetByID(System.Linq.Expressions.Expression<Func<VW_Permisos, bool>> filter = null)
        {
            return this.db.VW_Permisos.FirstOrDefault(filter);
        }
    }

    public class vwPermisos2DAL : GenericDAL
    {

        public IEnumerable<vw_Permisos2> Get(System.Linq.Expressions.Expression<Func<vw_Permisos2, bool>> filter = null)
        {
            if (filter == null)
                return this.db.vw_Permisos2;
            return this.db.vw_Permisos2.Where(filter);
        }

        public vw_Permisos2 GetByID(System.Linq.Expressions.Expression<Func<vw_Permisos2, bool>> filter = null)
        {
            return this.db.vw_Permisos2.FirstOrDefault(filter);
        }
    }
}