using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.Models;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class CuentasBL
    {
        private CuentasDAL DALCuentas { get; set; }

        public CuentasBL()
        {
            if (DALCuentas == null) DALCuentas = new CuentasDAL();
        }

        public object getCargosAbonos(string Id_Cuenta, int mes)
        {
            CA_Cuentas cuenta = DALCuentas.GetByID(reg => reg.Id_Cuenta == Id_Cuenta);
            switch (mes)
            {
                case 1:
                    return new { Cargo = cuenta.cargo01, Abono = cuenta.abono01 };                    
                case 2:
                    return new { Cargo = cuenta.cargo02, Abono = cuenta.abono02 };
                case 3:
                    return new { Cargo = cuenta.cargo03, Abono = cuenta.abono03 };
                case 4:
                    return new { Cargo = cuenta.cargo04, Abono = cuenta.abono04 };
                case 5:
                    return new { Cargo = cuenta.cargo05, Abono = cuenta.abono05 };
                case 6:
                    return new { Cargo = cuenta.cargo06, Abono = cuenta.abono06 };
                case 7:
                    return new { Cargo = cuenta.cargo07, Abono = cuenta.abono07 };
                case 8:
                    return new { Cargo = cuenta.cargo08, Abono = cuenta.abono08 };
                case 9:
                    return new { Cargo = cuenta.cargo09, Abono = cuenta.abono09 };
                case 10:
                    return new { Cargo = cuenta.cargo10, Abono = cuenta.abono10 };
                case 11:
                    return new { Cargo = cuenta.cargo11, Abono = cuenta.abono11 };
                case 12:
                    return new { Cargo = cuenta.cargo12, Abono = cuenta.abono12 };
            }
            return null;
        }
    }
}