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
    public class DeContrarecibosBL
    {
        private CierreMensualDAL DALCierreMensual { get; set; }
        private DeContrarecibosDAL DALDeContrarecibos { get; set; }
        private TipoCompromisosDAL DALTipoCompromisos { get; set; }
        private BeneficiariosCuentasDAL DALBeneficiarioCuenta { get; set; }
        private MaContrarecibosDAL DALMaContrarecibos { get; set; }
        protected DeDisponibilidadDAL DALDeDisponiblidad { get; set; }


        public DeContrarecibosBL()
        {
            if (DALCierreMensual == null) DALCierreMensual = new CierreMensualDAL();
            if (DALDeContrarecibos == null) DALDeContrarecibos = new DeContrarecibosDAL();
            if (DALTipoCompromisos == null) DALTipoCompromisos = new TipoCompromisosDAL();
            if (DALBeneficiarioCuenta == null) DALBeneficiarioCuenta = new BeneficiariosCuentasDAL();
            if (DALMaContrarecibos == null) DALMaContrarecibos = new MaContrarecibosDAL();
            if (DALDeDisponiblidad == null) DALDeDisponiblidad = new DeDisponibilidadDAL();
        }

        public bool hasRows(short tipoCR, int folioCR)
        {
            return DALDeContrarecibos.Get(reg => reg.Id_TipoCR == tipoCR && reg.Id_FolioCR == folioCR).Count() > 0 ? true : false;
        }

        public bool isSaldado(short tipoCR, int folioCR)
        {
            decimal? cargos = DALDeContrarecibos.Get(reg => reg.Id_TipoCR == tipoCR && reg.Id_FolioCR == folioCR && reg.Id_Movimiento == Diccionarios.ValorMovimientos.CARGO).Sum(reg => reg.Importe);
            decimal? abonos = DALDeContrarecibos.Get(reg => reg.Id_TipoCR == tipoCR && reg.Id_FolioCR == folioCR && reg.Id_Movimiento == Diccionarios.ValorMovimientos.ABONO).Sum(reg => reg.Importe);
            if (cargos == abonos && cargos > 0)
                return true;
            return false;
        }

        public int getNextId(short tipoCR, int folioCR)
        {
            try
            {
                return DALDeContrarecibos.Get(reg => reg.Id_TipoCR == tipoCR && reg.Id_FolioCR == folioCR).Max(max => max.Id_Registro) + 1;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public bool hasDisponibilidad(string cve_presupuesto, decimal importe, DateTime? Fecha_Orden)
        {
            byte mes = Fecha_Orden.HasValue ? Convert.ToByte(Fecha_Orden.Value.Month) : Convert.ToByte("0");
            DE_Disponibilidad entity = DALDeDisponiblidad.GetByID(reg => reg.Id_ClavePresupuesto == cve_presupuesto && reg.Mes == mes);
            return entity.Disponible.HasValue ? entity.Disponible.Value > importe : false;
        }

        public List<object> createBotonera(byte Tipo, int Folio)
        {
            return new List<object>() { "bNuevo", "bBuscar", "bSalir" };
        }
        
        private void AddBoton(ref List<object> botonera, string btn)
        {
            botonera.Add(btn);
        }
    }
}