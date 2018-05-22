using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.Models;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class NominaBL
    {
        DeNominaDAL nDal = new DeNominaDAL();
        public short GetNextID(string No_Nomina, string No_Control, String Tipo_Nomina)
        {
            try
            {
               // max = Convert.ToByte((from reg in bd.De_Nomina where reg.No_Nomina == No_Nomina && reg.No_Control == No_Control && reg.Tipo_Nomina == Tipo_Nomina select reg.IdRegistro).Max());
                return Convert.ToInt16(nDal.Get(x=>x.No_Nomina == No_Nomina && x.No_Control == No_Control && x.Tipo_Nomina == Tipo_Nomina).Max(x=>x.IdRegistro)+1);
            }
            catch (Exception)
            {
                return 1;
            }
        }
    }
}