using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class PresupuestosEgresosBL
    {
        private MaPresupuestoEgDAL DALPresupuestos;

        public PresupuestosEgresosBL()
        {
            DALPresupuestos = new MaPresupuestoEgDAL();
        }

    }
}