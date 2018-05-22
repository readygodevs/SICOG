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
    public class DeCompromisosBL
    {
        private CierreMensualDAL DALCierreMensual { get; set; }
        private DeCompromisosDAL DALDeCompromisos { get; set; }
        private TipoCompromisosDAL DALTipoCompromisos { get; set; }
        private BeneficiariosCuentasDAL DALBeneficiarioCuenta { get; set; }
        private MaCompromisosDAL DALMaCompromisos { get; set; }
        protected DeDisponibilidadDAL DALDeDisponiblidad { get; set; }
        

        public DeCompromisosBL()
        {
            if (DALCierreMensual == null) DALCierreMensual = new CierreMensualDAL();
            if (DALDeCompromisos == null) DALDeCompromisos = new DeCompromisosDAL();
            if (DALTipoCompromisos == null) DALTipoCompromisos = new TipoCompromisosDAL();
            if (DALBeneficiarioCuenta == null) DALBeneficiarioCuenta = new BeneficiariosCuentasDAL();
            if (DALMaCompromisos == null) DALMaCompromisos = new MaCompromisosDAL();
            if (DALDeDisponiblidad == null) DALDeDisponiblidad = new DeDisponibilidadDAL();
        }

        public bool hasRows(short tipoCompromiso, int folioCompromiso)
        {
            return DALDeCompromisos.Get(reg => reg.Id_TipoCompromiso == tipoCompromiso && reg.Id_FolioCompromiso == folioCompromiso).Count() > 0 ? true : false;
        }

        public bool isSaldado(short tipoCompromiso, int folioCompromiso)
        {
            decimal? cargos = DALDeCompromisos.Get(reg => reg.Id_TipoCompromiso == tipoCompromiso && reg.Id_FolioCompromiso == folioCompromiso && reg.Id_Movimiento == Diccionarios.ValorMovimientos.CARGO).Sum(reg => reg.Importe);
            decimal? abonos = DALDeCompromisos.Get(reg => reg.Id_TipoCompromiso == tipoCompromiso && reg.Id_FolioCompromiso == folioCompromiso && reg.Id_Movimiento == Diccionarios.ValorMovimientos.ABONO).Sum(reg => reg.Importe);
            if (cargos == abonos && cargos > 0)
                return true;
            return false;
        }

        public int getNextId(short tipoCompromiso, int folioCompromiso)
        {
            try
            {
                return DALDeCompromisos.Get(reg => reg.Id_TipoCompromiso == tipoCompromiso && reg.Id_FolioCompromiso == folioCompromiso).Max(max => max.Id_Registro) + 1;
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

        public List<object> createBotonera(Ma_CompromisosModel model, bool? CanSaldar)
        {
            if (!String.IsNullOrEmpty(model.Poliza_Comprometido))
                return new List<object>() { "bSalir" };

            List<object> botonera = new List<object>();

            if (model.Id_MesPO_Comprometido == null && model.Estatus != Diccionarios.ValorEstatus.CANCELADO && (model.De_Compromisos.Count == 0 || hasRows(model.Id_TipoCompromiso, model.Id_FolioCompromiso)))
                this.AddBoton(ref botonera, "bNuevo");

            if (hasRows(model.Id_TipoCompromiso, model.Id_FolioCompromiso) && model.Id_MesPO_Comprometido == null)
            {
                this.AddBoton(ref botonera, "bEditar");
                this.AddBoton(ref botonera, "bEliminar");
            }            

            decimal? cargos = model.De_Compromisos.Where(reg => reg.Id_Movimiento == Diccionarios.ValorMovimientos.CARGO).Sum(reg => reg.Importe);
            decimal? abonos = model.De_Compromisos.Where(reg => reg.Id_Movimiento == Diccionarios.ValorMovimientos.ABONO).Sum(reg => reg.Importe);
            if ((CanSaldar.HasValue && CanSaldar.Value) || cargos != abonos)
                this.AddBoton(ref botonera, "bSaldar");
            if (cargos == abonos && cargos > 0 || model.Estatus == Diccionarios.ValorEstatus.CANCELADO || !hasRows(model.Id_TipoCompromiso, model.Id_FolioCompromiso))
                this.AddBoton(ref botonera, "bSalir");

            return botonera;
        }

        private void AddBoton(ref List<object> botonera, string btn)
        {
            botonera.Add(btn);
        }
    }
}