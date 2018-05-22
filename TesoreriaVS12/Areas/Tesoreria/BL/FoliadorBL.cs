using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class ContrarecibosBL
    {
        private MaContrarecibosDAL _dalMaContra;
        private TipoContrarecibosDAL _dalFoliadorMaContra;
        private CierreMensualDAL dalCierreMes { get; set; }
        private MaCompromisosDAL dalCompromisos { get; set; }
        private DePolizasDAL dalDePolizas { get; set; }
        protected DeFacturasDAL dalFacturas { get; set; }

        protected TipoContrarecibosDAL dalFoliadorContra
        {
            get { return _dalFoliadorMaContra; }
            set { _dalFoliadorMaContra = value; }
        }

        protected MaContrarecibosDAL dalMaContra
        {
            get { return _dalMaContra; }
            set { _dalMaContra = value; }
        }

        public ContrarecibosBL()
        {
            if (dalFoliadorContra == null) dalFoliadorContra = new TipoContrarecibosDAL();
            if (dalMaContra == null) dalMaContra = new MaContrarecibosDAL();
            if (dalCierreMes == null) dalCierreMes = new CierreMensualDAL();
            if (dalCompromisos == null) dalCompromisos = new MaCompromisosDAL();
            if (dalDePolizas == null) dalDePolizas = new DePolizasDAL();
            if (dalFacturas == null) dalFacturas = new DeFacturasDAL();
            
        }
        public Int32? getMaContrarecibos(Byte TipoCR)
        {
            try
            {
                return dalFoliadorContra.GetByID(x => x.Id_TipoCR == TipoCR).UltimoCR + 1;
            }
            catch (Exception ex)
            {
                new Errores(ex.HResult, ex.Message);
                return -1;
            }
        }

        public bool setMaContrarecibos(Int16 TipoCR)
        {
            try
            {
                UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
                Ca_TipoContrarecibos c = dalFoliadorContra.GetByID(x => x.Id_TipoCR == TipoCR);
                c.UltimoCR++;
                c.Fecha_Act = DateTime.Now;
                c.Usu_Act = (Int16)appUsuario.IdUsuario;
                dalFoliadorContra.Update(c);
                dalFoliadorContra.Save();
                return true;
            }
            catch (Exception ex)
            {
                new Errores(ex.HResult, ex.Message);
                return false;
            }
        }

        public short StateEdit(Ma_ContrarecibosModel dataModal)
        {
            if (dataModal.Id_EstatusCR == 1 && dataModal.Cargos == 0)
                return 1;
            if (dalCierreMes.GetByID(x => x.Id_Mes == dataModal.FechaCR.Value.Month).Contable.Value)
                return -1;
            if (dataModal.Impreso_CR == true && dataModal.Id_EstatusCR != 1)
                return 2;
            if (dataModal.Cargos > 0 || dataModal.Impreso_CR == true)
                return 3;
            if (dataModal.Id_TipoCR == 2)
            {
                if (dataModal.Impreso_CR == true || dataModal.Id_EstatusCR != 1)
                    return 2;
                if (dalCompromisos.Get(x => x.Id_TipoCR == dataModal.Id_TipoCR && x.Id_FolioCR == dataModal.Id_FolioCR).Count() > 0 || dataModal.Impreso_CR == true)
                    return 3;
            }
            return -1;
        }

        public short StateCancel(Ma_ContrarecibosModel dataModal)
        {
            if (dalCierreMes.GetByID(x => x.Id_Mes == dataModal.FechaCR.Value.Month).Contable.Value)
                return -1;
            if (dataModal.Id_EstatusCR == 1 && dataModal.No_Cheque == null)
                return 1;
            return -1;
        }

        /// <summary>
        /// Nex id De Polizas
        /// </summary>
        /// <param name="TipoPoliza"></param>
        /// <param name="FolioPoliza"></param>
        /// <param name="IdMesPoliza"></param>
        /// <returns></returns>
        public short getNextIdDePolizas(short TipoPoliza, int FolioPoliza, byte IdMesPoliza)
        {
            try
            {
                return Convert.ToInt16(dalDePolizas.Get(reg => reg.Id_TipoPoliza == TipoPoliza && reg.Id_FolioPoliza == FolioPoliza && reg.Id_MesPoliza == IdMesPoliza).Max(max => max.Id_Registro) + 1);
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public bool exist(short TipoCR, int FolioCR)
        {
            return dalMaContra.Get(reg => reg.Id_TipoCompromiso == TipoCR && reg.Id_FolioCR == FolioCR).Count() > 0 ? true : false;
        }

        public List<object> createBotoneraDocumentos(Ma_Contrarecibos dataModel)
        {
            List<object> botonera = new List<object>();
            if (dataModel.Id_TipoCR == Diccionarios.TiposCR.ContraRecibos)
            {
                //solo cuando esta cancelado no se muestra el boton nuevo se quito-> dataModel.Id_EstatusCR != Diccionarios.Valores_Estatus_CR.Pagado 
                if (dataModel.Id_EstatusCR != Diccionarios.Valores_Estatus_CR.Cancelado)
                    this.AddBoton(ref botonera, "bNuevo");
            }
            else
                this.AddBoton(ref botonera, "bNuevo");
            this.AddBoton(ref botonera, "bUploadFiles");
            this.AddBoton(ref botonera, "bSalir");
            return botonera;
        }

        public List<object> createBotonera(Ma_ContrarecibosModel dataModel)
        {
            List<object> botonera = new List<object>() { "bNuevo", "bBuscar" };

            if (this.StateEdit(dataModel) != -1)
                this.AddBoton(ref botonera, "bEditar");
            if (!(dataModel.Cargos == 0 || dataModel.Cargos == null))
            {
                this.AddBoton(ref botonera, "bDetalles");
                if(dataModel.Id_EstatusCR != Diccionarios.Valores_Estatus_CR.Cancelado)
                    this.AddBoton(ref botonera, "bDocumentos");
            }
            if(dataModel.Id_FolioCR > 0 || (dataModel.Id_EstatusCR != Diccionarios.Valores_Estatus_CR.Cancelado && dataModel.FolioCompromiso != 0))
                this.AddBoton(ref botonera, "bCompromisos");
            if (this.StateCancel(dataModel) != -1)
                this.AddBoton(ref botonera, "bCancelarGral");
            bool tieneFacturas = dalFacturas.Get(x => x.Id_FolioCR == dataModel.Id_FolioCR && x.Id_TipoCR == dataModel.Id_TipoCR).Count() > 0;
            switch (dataModel.Id_TipoCR)
            {
                case 8:
                case 9:
                case 10:
                case 11:
                    if(dataModel.Id_TipoCompromiso.HasValue && dataModel.Id_TipoCompromiso.Value > 0  && !dataModel.Impreso_CH.Value)
                        this.AddBoton(ref botonera, "bAgregarImporte");
                    if(tieneFacturas && dataModel.Importe_AH.HasValue && dataModel.Importe_AH.Value > 0 )
                        this.AddBoton(ref botonera, "bReportes");
                    break;
                default:
                    if (tieneFacturas)
                        this.AddBoton(ref botonera, "bReportes");
                    break;
            }
            this.AddBoton(ref botonera, "bSalir");
            return botonera;
        }

        public List<object> createBotoneraCP(Ma_ContrarecibosCPModel dataModel)
        {
            List<object> botonera = new List<object>() { "bNuevo" };            
            //Guardado reporte cancelar y editar buscar e==1
            if (dataModel.Id_EstatusCR != Diccionarios.Valores_Estatus_CR.Cancelado)
                this.AddBoton(ref botonera, "bEditar");
            this.AddBoton(ref botonera, "bBuscar");
            if (dataModel.Id_EstatusCR != Diccionarios.Valores_Estatus_CR.Cancelado)
                this.AddBoton(ref botonera, "bImprimir");
            if (dataModel.Id_EstatusCR == Diccionarios.Valores_Estatus_CR.Programado || dataModel.Id_EstatusCR == Diccionarios.Valores_Estatus_CR.Pagado)
                this.AddBoton(ref botonera, "bCancelarGral");


            this.AddBoton(ref botonera, "bSalir");
            return botonera;
        }
        public List<object> createBotoneraNP(Ma_ContrarecibosCPModel dataModel)
        {
            List<object> botonera = new List<object>() { "bNuevo" };
            //Guardado reporte cancelar y editar buscar e==1
            if (dataModel.Id_EstatusCR != Diccionarios.Valores_Estatus_CR.Cancelado)
                this.AddBoton(ref botonera, "bEditar");
            this.AddBoton(ref botonera, "bBuscar");
            if (dataModel.Id_EstatusCR != Diccionarios.Valores_Estatus_CR.Cancelado && new DeContrarecibosDAL().Get(x => x.Id_FolioCR == dataModel.Id_FolioCR && x.Id_TipoCR == dataModel.Id_TipoCR).Count() > 0)
                this.AddBoton(ref botonera, "bImprimir");
            if (dataModel.Id_EstatusCR == Diccionarios.Valores_Estatus_CR.Programado || dataModel.Id_EstatusCR == Diccionarios.Valores_Estatus_CR.Pagado)
                this.AddBoton(ref botonera, "bCancelarGral");


            this.AddBoton(ref botonera, "bSalir");
            return botonera;
        }

        private void AddBoton(ref List<object> botonera, string btn)
        {
            botonera.Add(btn);
        }

        public bool existFolioFactura(De_FacturasModel dataModel)
        {
            if (dalFacturas.Get(x => x.No_docto == dataModel.No_docto).Count() > 0)
                return true;
            return false;
        }

    }
}