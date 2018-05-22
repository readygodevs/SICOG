using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Vistas;
using TesoreriaVS12.Areas.Tesoreria.Models;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class AccionesBL
    {
        private AccionDAL DALAccion;
        public AccionesBL()
        {
            if (DALAccion == null) DALAccion = new AccionDAL();
        }

        public short nextIdAccion(string idProceso, string idActividadMIR)
        {
            try
            {
                return Convert.ToInt16(DALAccion.Get(x => x.Id_Proceso == idProceso && x.Id_ActividadMIR2 == idActividadMIR ).Max(x => x.Id_Accion) + 1);
            }
            catch (Exception)
            {
                return 1;
            }
        }
    }
}