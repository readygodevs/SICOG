using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class ImpuestoDeduccionBL
    {
        private ImpuestosDeduccionDAL DALImpuestosDeduccion;

        public ImpuestoDeduccionBL()
        {
            DALImpuestosDeduccion = new ImpuestosDeduccionDAL();
        }

        public int GetId_ImpDed(int Id_Tipo_ImpDed)
        {
            try
            {
                return DALImpuestosDeduccion.Get(reg => reg.Id_Tipo_ImpDed == Id_Tipo_ImpDed).Max(max => max.Id_ImpDed) + 1;
            }
            catch (Exception)
            {
                return 1;
            }
        }

    }
}