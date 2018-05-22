using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class DeCompromisosHBL
    {
        private DeCompromisosHDAL DALCompromisosH { get; set; }
        public DeCompromisosHBL()
        {
            if (DALCompromisosH == null) DALCompromisosH = new DeCompromisosHDAL();
        }

        public int getNextId(short tipoCompromiso, int folioCompromiso, int idRegistro)
        {
            try
            {
                return DALCompromisosH.Get(reg => reg.Id_TipoCompromiso == tipoCompromiso && reg.Id_FolioCompromiso == folioCompromiso && reg.Id_Registro == idRegistro).Max(max => max.Id_Historial) + 1;
            }
            catch (Exception)
            {
                return 1;
            }
        }
    }
}