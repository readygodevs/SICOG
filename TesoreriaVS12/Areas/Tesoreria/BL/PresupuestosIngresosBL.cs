using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class PresupuestosIngresosBL
    {
        private MaPresupuestoIngDAL DALPresupuestos;

        public PresupuestosIngresosBL()
        {
            DALPresupuestos = new MaPresupuestoIngDAL();
        }

    }
}