using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.Models;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class AmpliacionesReduccionesBL
    {
        protected MaTrasnferenciasDAL MaTranferenciasDAL { get; set; }
        protected MaTransferenciasIngDAL MaTransferenciasIngDAL { get; set; }
        protected CierreMensualDAL cierreMensualDAL { get; set; }
        protected DeTransferenciaDAL DeTransferenciasDAL { get; set; }
        protected ProceduresDAL proceduresDAL { get; set; }
        protected Llenado llenar { get; set; }
        public AmpliacionesReduccionesBL()
        {
            if (MaTranferenciasDAL == null) MaTranferenciasDAL = new MaTrasnferenciasDAL();
            if (MaTransferenciasIngDAL == null) MaTransferenciasIngDAL = new MaTransferenciasIngDAL();
            if (cierreMensualDAL == null) cierreMensualDAL = new CierreMensualDAL();
            if (DeTransferenciasDAL == null) DeTransferenciasDAL = new DeTransferenciaDAL();
            if (proceduresDAL == null) proceduresDAL = new ProceduresDAL();
            if (llenar == null) llenar = new Llenado();
        }

        public byte GetUltimoMesCerrado()
        {
            byte total = 0;
            if (cierreMensualDAL.Get(x => x.Contable == true).Count() > 0)
                total = cierreMensualDAL.Get(x => x.Contable == true).Last().Id_Mes;
            return total;
        }
        public int ValidarSinAfectar()
        {
            return MaTranferenciasDAL.Get(x => x.Id_Estatus == 1 && x.Id_PptoModificado == true).Count();
        }
        public int ValidarSinAfectarIng()
        {
            return MaTransferenciasIngDAL.Get(x => x.Id_Estatus == 1 && x.Id_PptoModificado == true).Count();
        }
        public int ValidarSinTransferencia()
        {
            return MaTranferenciasDAL.Get(x => x.Id_Estatus == 1 && x.Id_PptoModificado == false).Count();
        }
        public int ValidarSinAfectarArrastre()
        {
            return MaTranferenciasDAL.Get(x => x.Id_Estatus == 1 && x.Id_PptoModificado == null).Count();
        }

        public DateTime GetFechaMayor(DateTime Fecha1)
        {
            DateTime FechaMes = new DateTime(DateTime.Now.Year, GetUltimoMesCerrado() + 1, 1);
            if (Fecha1 > FechaMes)
                return Fecha1;
            else
                return FechaMes;
        }
        public byte ObtenerTipoMaTransferencia(Int32 id_Transfecencia)
        {
            return MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == id_Transfecencia).Id_TipoT.Value;
 
        }
        public byte ObtenerTipoMaTransferenciaIng(Int32 id_Transfecencia)
        {
            return MaTransferenciasIngDAL.GetByID(x => x.Folio == id_Transfecencia).Id_TipoT.Value;

        }
    }
}