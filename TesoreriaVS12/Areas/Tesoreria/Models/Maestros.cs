using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.BL;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Areas.Tesoreria.Models
{
    public class Ma_CompromisosModel
    {
        public const short ORDEN_COMPRA = 1;
        public const short ORDEN_SERVICIO = 2;
        public const short ORDEN_COMPRA_NOMINA = 5;

        public const int COMPROMETIDO = 1;
        public const int PROGRAMADO = 2;
        public const int PAGADO = 3;
        public const int CANCELADO = 4;
        public const int AUTORIZACION = 5;
        public const int RECIBIDO = 6;

        public Ma_CompromisosModel()
        {
            TipoCompromisosDAL DALTipoCompromisos = new TipoCompromisosDAL();
            this.ListaEstatus = new SelectList(Diccionarios.EstatusCompromisos, "Key", "Value");
            this.ListaId_TipoCompromiso = new SelectList(DALTipoCompromisos.Get(), "Id_TipoCompromiso", "Descripcion");
        }

        public Ma_CompromisosModel(short tipoCompromiso)
        {
            Llenado llenar = new Llenado();
            TipoCompromisosDAL DALTipoCompromisos = new TipoCompromisosDAL();
            this.ListaEstatus = new SelectList(Diccionarios.EstatusCompromisos, "Key", "Value", COMPROMETIDO);
            this.ListaId_TipoCompromiso = new SelectList(DALTipoCompromisos.Get().OrderBy(x=>x.Descripcion), "Id_TipoCompromiso", "Descripcion", tipoCompromiso);
            this.ListaId_ClasificaBeneficiario = llenar.LLenado_ClasificacionBeneficiario(this.Id_Cuenta_Beneficiario, this.Id_Beneficiario);

            //Campos
            this.PagarseEn = llenar.LLenado_CaTipoCompromisos(tipoCompromiso).Pagarse;
            this.Fecha = DateTime.Now;
            this.Fecha_Orden = DateTime.Now;
            this.Fecha_Requisicion = DateTime.Now;
            this.Fecha_Autorizo = null;
            this.Adquisicion = false;
            this.regresoDetalle = false;
            this.Descripcion_Estatus = Diccionarios.EstatusCompromisos[COMPROMETIDO];
        }

        [Display(Name = "Tipo de Compromiso")]
        [Required(ErrorMessage = "*")]
        public short Id_TipoCompromiso { get; set; }
        [Display(Name = "Folio")]
        public int Id_FolioCompromiso { get; set; }
        [Display(Name = "Orden")]
        [Required(ErrorMessage = "*")]
        public Nullable<int> Id_FolioOrden { get; set; }
        [Display(Name = "Fecha Concurso")]
        [Required(ErrorMessage = "*")]
        public Nullable<System.DateTime> Fecha { get; set; }
        [Display(Name = "Usuario de Fincamiento")]
        [Required(ErrorMessage = "*")]
        public string Usuario_Orden { get; set; }
        [Display(Name = "Fecha Fincamiento")]
        [Required(ErrorMessage = "*")]
        public Nullable<DateTime> Fecha_Orden { get; set; }
        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "*")]
        public Nullable<int> Id_Beneficiario { get; set; }
        [Display(Name = "Clasificación de Beneficiario")]
        [Required(ErrorMessage = "*")]
        public Nullable<byte> Id_ClasificaBeneficiario { get; set; }
        public string Beneficiario { get; set; }
        public string Id_Cuenta_Beneficiario { get; set; }
        public Nullable<short> Id_Anio_Requisicion { get; set; }
        [Display(Name = "No. Requisición")]
        [Required(ErrorMessage = "*")]
        public Nullable<int> No_Requisicion { get; set; }
        [Display(Name = "Fecha Requisición")]
        [Required(ErrorMessage = "*")]
        public Nullable<System.DateTime> Fecha_Requisicion { get; set; }
        [Display(Name = "Centro Gestor Solicitante")]
        [Required(ErrorMessage = "*")]
        public string Id_Area_Solicitud { get; set; }
        public string Area_Solicitud { get; set; }
        [Display(Name = "Usuario que Autoriza")]
        public string Usuario_Autorizo { get; set; }
        [Display(Name = "Fecha Autoriza")]
        public System.DateTime? Fecha_Autorizo { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }
        [Display(Name = "Centro Gestor Entrega")]
        public string Id_Area_Entrega { get; set; }
        public string Area_Entrega { get; set; }
        [Display(Name = "Usuario que Cancela")]
        public string Usuario_Cancela { get; set; }
        [Display(Name = "Fecha Cancela")]
        public Nullable<System.DateTime> Fecha_Cancela { get; set; }
        [Display(Name = "Usuario que Recibe en Centro Gestor")]
        public string Usuario_Recibe_Area { get; set; }
        [Display(Name = "Fecha de Recibido")]
        public Nullable<System.DateTime> Fecha_Recibe_Area { get; set; }
        [Display(Name = "Observaciones de Quien Recibió")]
        [DataType(DataType.MultilineText)]
        public string Observa_Recibio { get; set; }
        public Nullable<decimal> Cargos { get; set; }
        public Nullable<decimal> Abonos { get; set; }
        [Display(Name = "Tipo Cuenta por Liquidar")]
        public Nullable<byte> Id_TipoCR { get; set; }
        [Display(Name = "Cuenta por Liquidar")]
        public Nullable<int> Id_FolioCR { get; set; }
        public Nullable<byte> Id_TipoPoliza { get; set; }
        [Display(Name = "Póliza de Devengado")]
        public Nullable<byte> Id_MesPoliza { get; set; }
        public Nullable<int> Id_FolioPoliza { get; set; }
        [Display(Name = "Cancelación de Póliza de Devengado")]
        public Nullable<byte> Id_MesPoliza_C { get; set; }
        public Nullable<int> Id_FolioPoliza_C { get; set; }
        public Nullable<byte> Id_Proyecto { get; set; }
        [Required(ErrorMessage = "*")]
        public Nullable<short> Estatus { get; set; }
        public string Descripcion_Estatus { get; set; }
        [Display(Name = "Póliza Orden Comprometido")]
        public Nullable<byte> Id_MesPO_Comprometido { get; set; }
        public Nullable<int> Id_FolioPO_Comprometido { get; set; }
        [Display(Name = "Cancelación de Póliza Orden Comprometido")]
        public Nullable<byte> Id_MesPO_Comprometido_C { get; set; }
        public Nullable<int> Id_FolioPO_Comprometido_C { get; set; }
        [Display(Name = "Póliza Orden Devengado")]
        public Nullable<byte> Id_MesPO_Devengado { get; set; }
        public Nullable<int> Id_FolioPO_Devengado { get; set; }
        [Display(Name = "Cancelación de Póliza Orden Devengado")]
        public Nullable<byte> Id_MesPO_Devengado_C { get; set; }
        public Nullable<int> Id_FolioPO_Devengado_C { get; set; }
        public Nullable<bool> Historial { get; set; }
        public string TipoNomina { get; set; }
        public Nullable<byte> No_Quincena { get; set; }
        public Nullable<byte> Mes_Quincena { get; set; }
        public string No_Nomina { get; set; }
        [Display(Name = "Fecha Devengado")]
        public Nullable<System.DateTime> Fecha_Devengado { get; set; }
        public Nullable<System.DateTime> Fecha_CancelaCompro { get; set; }
        public Nullable<System.DateTime> Fecha_CancelaDevengo { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        [Display(Name = "Orden de Compra")]
        public Nullable<int> No_Adquisicion { get; set; }
        [Display(Name = "Fecha Orden de Compra")]
        public Nullable<System.DateTime> Fecha_Adquisicion { get; set; }

        public Ca_TipoCompromisosModel Ca_TipoCompromisos { get; set; }
        public Ca_AreasModel Ca_Areas_Solicitud { get; set; }
        public Ca_AreasModel Ca_Areas_Entrega { get; set; }
        public Ca_CuentasModel Ca_Cuentas_Beneficiario { get; set; }
        public List<De_CompromisosModel> De_Compromisos { get; set; }
        public Ca_BeneficiariosModel Ca_Beneficiarios { get; set; }
        public Ma_ContrarecibosModel Ma_Contrarecibos { get; set; }
        public Ma_PolizasModel Ma_Polizas { get; set; }
        public ReturnMaster Regreso { get; set; }

        /*Campos mios*/
        /*Listas*/
        public SelectList ListaId_TipoCompromiso { get; set; }
        [Display(Name = "Estatus")]
        public SelectList ListaEstatus { get; set; }
        public SelectList ListaId_ClasificaBeneficiario { get; set; }

        [Display(Name = "Póliza Orden Comprometido")]
        public string Poliza_Comprometido { get; set; }
        [Display(Name = "Cancelación de Póliza Orden Comprometido")]
        public string Poliza_Comprometido_C { get; set; }
        [Display(Name = "Póliza Orden Devengado")]
        public string Poliza_Devengado { get; set; }
        [Display(Name = "Cancelación de Póliza Orden Devengado")]
        public string Poliza_Devengado_C { get; set; }
        [Display(Name = "Póliza de Devengado")]
        public string Poliza_Fincamiento { get; set; }
        [Display(Name = "Cancelación de Póliza de Devengado")]
        public string Poliza_Fincamiento_C { get; set; }
        [Display(Name = "Cuenta por Liquidar")]
        public string ContraRecibo { get; set; }

        /*campos*/
        [Display(Name = "Pagarse en")]
        public string PagarseEn { get; set; }
        public bool? Adquisicion { get; set; }
        public bool regresoDetalle { get; set; }
        public List<Object> Botonera { get; set; }
        public string argsReturn { get; set; }
    }

    public class CompromisosBusqueda
    {
        public Nullable<decimal> ImporteDesde { get; set; }
        public Nullable<decimal> ImporteHasta { get; set; }
        public Nullable<DateTime> FechaDesdeOrden { get; set; }
        public Nullable<DateTime> FechaHastaOrden { get; set; }
        public Nullable<DateTime> FechaDesdeDevengado { get; set; }
        public Nullable<DateTime> FechaHastaDevengado { get; set; }
        public Nullable<Boolean> BusquedaAdquisiciones { get; set; }
        public Nullable<short> Orden { get; set; }
        public Nullable<short> Id_TipoCompromiso { get; set; }
        public Nullable<short> Estatus { get; set; }
        public Nullable<short> Id_Beneficiario { get; set; }
        public SelectList ListaId_TipoCompromiso { get; set; }
        public SelectList ListaEstatus { get; set; }
        public Nullable<short> TipoFecha { get; set; }
        public CompromisosBusqueda()
        {
            TipoCompromisosDAL DALTipoCompromisos = new TipoCompromisosDAL();
            this.ListaEstatus = new SelectList(Diccionarios.EstatusCompromisos, "Key", "Value");
            this.ListaId_TipoCompromiso = new SelectList(DALTipoCompromisos.Get(), "Id_TipoCompromiso", "Descripcion");
        }
    }

    public class BusquedaAnaliticoIngresos
    {
        public int IdContribuyente { get; set; }
        [Display(Name = "Desde")]
        public Nullable<DateTime> FechaDesdeRecibo { get; set; }
        [Display(Name = "Hasta")]
        public Nullable<DateTime> FechaHastaRecibo { get; set; }
        [Display(Name = "Desde")]
        public Nullable<DateTime> FechaDesdeRecaudacion { get; set; }
        [Display(Name = "Hasta")]
        public Nullable<DateTime> FechaHastaRecaudacion { get; set; }
        [Display(Name = "Nombre de Contribuyente")]
        public String NombreContribuyente {get;set;}
        [Display(Name = "Desde")]
        public Nullable<decimal> ImporteDesde { get; set; }
        [Display(Name = "Hasta")]
        public Nullable<decimal> ImporteHasta { get; set; }
        public Nullable<int> Estatus { get; set;}
        [Display(Name = "Cuenta Bancaria")]
        public int CuentaBancaria { get; set;}
        public int Orden { get; set; }
        public SelectList ListaEstatus { get; set; }
        public BusquedaAnaliticoIngresos()
        {
            this.ListaEstatus = new SelectList(Diccionarios.EstatusRecibos,"Key","Value");
        }
    }
    public class BusquedaCorteCaja
    {
        [Display(Name = "Desde")]
        public Nullable<DateTime> FechaDesdeRecaudacion { get; set; }
        [Display(Name = "Hasta")]
        public Nullable<DateTime> FechaHastaRecaudacion { get; set; }
        [Display(Name = "Caja Receptora")]
        public int CajaReceptora { get; set; }
        public int Estatus { get; set; }
        public int Orden { get; set; }
        public SelectList EstatusRecibo { get; set; }
        public BusquedaCorteCaja()
        {
            this.EstatusRecibo = new SelectList(Diccionarios.EstatusRecibos, "Key", "Value");
        }
    }

    public class RecibeCompromiso
    {
        public short Id_TipoCompromiso_R { get; set; }
        [Display(Name = "Folio del compromiso")]
        public int Id_FolioCompromiso_R { get; set; }
        public Nullable<System.DateTime> Fecha_Orden_R { get; set; }
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "*")]
        public Nullable<System.DateTime> Fecha_Devengado_R { get; set; }
        public Nullable<System.DateTime> Fecha_Recibe_Area_R { get; set; }
        [Display(Name = "Observaciones de Quien Recibió")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "*")]
        public string Observa_Recibio_R { get; set; }

        public Control_Fechas cfechas { get; set; }
    }

    public class CancelaCompromiso
    {
        public short Id_TipoCompromiso_C { get; set; }
        [Display(Name = "Folio del Compromiso")]
        public int Id_FolioCompromiso_C { get; set; }
        [Display(Name = "Usuario que Cancela")]
        [Required(ErrorMessage = "*")]
        public string Usuario_Cancela_C { get; set; }
        [Display(Name = "Fecha Cancela")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)] 
        public Nullable<System.DateTime> Fecha_Cancela_C { get; set; }
        [Display(Name = "Cuenta del Beneficiario")]
        public string Cuenta_Beneficiario_C { get; set; }

        public Control_Fechas cfechas { get; set; }
    }

    public class Ma_ContrarecibosModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Tipo de Cuenta por Liquidar")]
        public byte Id_TipoCR { get; set; }
        [Display(Name="Tipo Cuenta por Liquidar")]
        public string TipoCRstr {get; set;}
        [Display(Name = "Folio")]
        public int Id_FolioCR { get; set; }
        [Display(Name = "Fecha de Cuenta por Liquidar")]
        public Nullable<System.DateTime> FechaCR { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        [Display(Name = "Fecha de Vencimiento")]
        public Nullable<System.DateTime> FechaVen { get; set; }
        [Display(Name = "Beneficiario")]
        public Nullable<int> Id_Beneficiario { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Usuario del Sistema")]
        public string Usu_CR { get; set; }
        [Display(Name = "C.R. Impreso")]
        public Nullable<bool> Impreso_CR { get; set; }
        [Display(Name = "Cuenta Bancaria")]
        public Nullable<short> Id_CtaBancaria { get; set; }
        [Display(Name = "Cheque")]
        public Nullable<int> No_Cheque { get; set; }
        [Display(Name = "Fecha de Pago")]
        public Nullable<System.DateTime> Fecha_Pago { get; set; }
        public string Usu_Pago { get; set; }
        [Display(Name = "Estatus")]
        public Nullable<byte> Id_EstatusCR { get; set; }
        [Display(Name = "Importe")]
        public Nullable<decimal> Cargos { get; set; }
        public Nullable<decimal> Abonos { get; set; }
        [Display(Name = "Cheque Impreso")]
        public Nullable<bool> Impreso_CH { get; set; }
        [Display(Name = "Tipo de Compromiso")]
        public Nullable<short> Id_TipoCompromiso { get; set; }
        public string Id_CuentaFR { get; set; }
        public string Id_CuentaBeneficiario { get; set; }
        public Nullable<System.DateTime> FechaCierreGC { get; set; }
        public Nullable<byte> Estatus_GC { get; set; }
        public Nullable<int> Id_FolioGC { get; set; }
        [Display(Name = "Comp.")]
        public int FolioCompromiso { get; set; }
        [Display(Name = "Estauts")]
        public string DEscripcionEstatus { get; set; }
        [Display(Name = "Ejercicio anterior")]
        public Nullable<bool> Ejercicio_Anterior { get; set; }
        public Nullable<bool> AmpliacionFR { get; set; }
        public Nullable<byte> Id_Proyecto { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public Control_Fechas cFechas { get; set; }

        public Nullable<bool> Cerrado { get; set; }
        public string Spei { get; set; }
        public short StateEdit { get; set; }
        public short StateCancel { get; set; }
        public string NombreCompleto { get; set; }
        public string Nombre { get; set; }
        public List<Object> Botonera { get; set; }

        [Display(Name = "PolizaCR")]
        public string PolizaCR { get; set; }
        public Nullable<int> Id_FolioPolizaCR { get; set; }
        public Nullable<byte> Id_MesPolizaCR { get; set; }

        [Display(Name = "PolizaCR_C")]
        public string PolizaCR_C { get; set; }
        public Nullable<int> Id_FolioPolizaCR_C { get; set; }
        public Nullable<byte> Id_MesPolizaCR_C { get; set; }
        [Display(Name = "Fecha")]
        public Nullable<System.DateTime> FechaCancelacionCR { get; set; }

        [Display(Name = "Poliza de Orden Ejercido")]
        public string Poliza_Ejercido { get; set; }
        public Nullable<int> Id_FolioPO_Ejercido { get; set; }
        public Nullable<byte> Id_MesPO_Ejercido { get; set; }

        [Display(Name = "Cancelación de Poliza Orden Ejercido")]
        public string Poliza_Ejercido_C { get; set; }
        public Nullable<byte> Id_MesPO_Ejercido_C { get; set; }
        public Nullable<int> Id_FolioPO_Ejercido_C { get; set; }
        [Display(Name = "Fecha")]
        public Nullable<System.DateTime> Fecha_CancelaEjercido { get; set; }

        [Display(Name = "Poliza Orden Pagado")]
        public string Poliza_Pagado { get; set; }
        public Nullable<byte> Id_MesPO_Pagado { get; set; }
        public Nullable<int> Id_FolioPO_Pagado { get; set; }

        [Display(Name = "Cancelación de Poliza Orden Pagado")]
        public string Poliza_Pagado_C { get; set; }
        public Nullable<byte> Id_MesPO_Pagado_C { get; set; }
        public Nullable<int> Id_FolioPO_Pagado_C { get; set; }
        [Display(Name = "Fecha")]
        public Nullable<System.DateTime> Fecha_CancelaPagado { get; set; }

        [Display(Name = "Poliza Cheque")]
        public string PolizaCheque { get; set; }
        public Nullable<int> Id_FolioPolizaCH { get; set; }
        public Nullable<byte> Id_MesPolizaCH { get; set; }

        [Display(Name = "Cancelación de Poliza Cheque")]
        public string PolizaCheque_C { get; set; }
        public Nullable<int> Id_FolioPolizaCH_C { get; set; }
        public Nullable<byte> Id_MesPolizaCH_C { get; set; }
        [Display(Name = "Fecha")]
        public Nullable<System.DateTime> FechaCancelacion_CH { get; set; }

        [Display(Name = "Fuente de Financiamiento")]
        public string Id_Fuente { get; set; }
        public Nullable<int> IdPersona_ENP { get; set; }

        public Ca_PersonasModel Ca_Persona { get; set; }
        public Ca_BeneficiariosModel Ca_Beneficiarios { get; set; }
        public Ca_PersonasModel Ca_Personas { get; set; }
        public Ca_TipoContrarecibosModel Ca_TipoContrarecibos { get; set; }
        public List<De_ContrarecibosModel> De_Contrarecibos { get; set; }
        public Ca_ClasificaBeneficiariosModel Ca_ClasificaBeneficiarios { get; set; }
        public CA_UsuariosModel CA_Usuarios_UsuCR { get; set; }
        public Ca_CuentasBancariasModel Ca_CuentasBancarias { get; set; }
        public Ca_TipoCompromisosModel Ca_TipoCompromisos { get; set; }
        public Ca_CuentasModel Ca_Cuentas_FR { get; set; }
        public Ca_BeneficiariosCuentasModel Ca_BeneficiariosCuentas_FR { get; set; }
        public Ca_FuentesFinModel Ca_FuentesFin { get; set; }
        public List<De_FacturasModel> De_Documentos { get; set; }
        public SelectList ListaId_TipoCompromiso { get;set; }
        public Nullable<int> Id_FolioCompromiso { get; set; }
        public Nullable<int> No_Requisicion { get; set; }
        public Nullable<int> No_Adquisicion { get; set; }
        public Nullable<byte> Id_TipoMovimiento_AH { get; set; }
        public string Id_Cuenta_AH { get; set; }
        public Nullable<decimal> Importe_AH { get; set; }
        public Nullable<decimal> Importe_CH { get; set; }
        //Incio nuevas cuentas
        public string Id_Cuenta_AH2 { get; set; }
        public Nullable<decimal> Importe_AH2 { get; set; }
        public Nullable<decimal> Importe_CH2 { get; set; }
        public string Descripcion2 { get; set; }


        public string Id_Cuenta_AH3 { get; set; }
        public Nullable<decimal> Importe_AH3 { get; set; }
        public Nullable<decimal> Importe_CH3 { get; set; }
        public string Descripcion3 { get; set; }


        public string Id_Cuenta_AH4 { get; set; }
        public Nullable<decimal> Importe_AH4 { get; set; }
        public Nullable<decimal> Importe_CH4 { get; set; }
        public string Descripcion4 { get; set; }

        //Fin nuevas cuentas
        public String TipoMovmientoAH { get; set; }
        public Ma_ContrarecibosModel(Byte TipoCR)
        {
            TipoContrarecibosDAL tipoContra = new TipoContrarecibosDAL();
            TipoCompromisosDAL tipoCompromisos = new TipoCompromisosDAL();
            this.FechaCR = DateTime.Now;
            this.Cargos = 0;
            this.Id_FolioCR = 0;
            this.Impreso_CR = false;
            this.Impreso_CH = false;
            this.Ejercicio_Anterior = false;
            this.Id_TipoCR = TipoCR;
            this.Ca_TipoContrarecibos = ModelFactory.getModel<Ca_TipoContrarecibosModel>(tipoContra.GetByID(x => x.Id_TipoCR == TipoCR), new Ca_TipoContrarecibosModel());
            this.ListaId_TipoCompromiso = new SelectList(tipoCompromisos.Get(), "Id_TipoCompromiso", "Descripcion");
            //this.Ca_TipoCompromisos = ModelFactory.getModel<Ca_TipoCompromisosModel>(tipoCompromisos.GetByID(x => x.Id_TipoCompromiso == TipoCR), new Ca_TipoCompromisosModel());
        }

        public Ma_ContrarecibosModel()
        {
            TipoContrarecibosDAL tipoContra = new TipoContrarecibosDAL();
            TipoCompromisosDAL tipoCompromisos = new TipoCompromisosDAL();
            this.FechaCR = DateTime.Now;
            this.Cargos = 0;
            this.Id_FolioCR = 0;
            this.Impreso_CR = false;
            this.Impreso_CH = false;
            this.Ejercicio_Anterior = false;
            //this.Id_TipoCR = TipoCR;
            //this.Ca_TipoContrarecibos = ModelFactory.getModel<Ca_TipoContrarecibosModel>(tipoContra.GetByID(x => x.Id_TipoCR == TipoCR), new Ca_TipoContrarecibosModel());
            //this.Ca_TipoCompromisos = ModelFactory.getModel<Ca_TipoCompromisosModel>(tipoCompromisos.GetByID(x => x.Id_TipoCompromiso == TipoCR), new Ca_TipoCompromisosModel());
        }

        public Ma_ContrarecibosModel(byte Tipo, int Folio)
        {
            this.Id_TipoCR = Tipo;
            this.Id_FolioCR = Folio;
            this.De_Contrarecibos = new List<De_ContrarecibosModel>();
        }
    }

    public partial class Ma_PolizasModel
    {
        [Display(Name = "Tipo Póliza")]
        [Required(ErrorMessage = "*")]
        [Range(3,3)]
        public byte Id_TipoPoliza { get; set; }
        [Display(Name = "Folio Póliza")]
        public int Id_FolioPoliza { get; set; }
        [Display(Name = "Mes")]
        [Required(ErrorMessage = "*")]
        public byte Id_MesPoliza { get; set; }
        public Nullable<byte> Id_Proyecto { get; set; }
        public System.DateTime Fecha { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        [Display(Name = "Banco")]
        public Nullable<short> Id_CtaBancaria { get; set; }
        public Nullable<int> No_Cheque { get; set; }
        public Nullable<int> Id_Beneficiario { get; set; }
        public Nullable<byte> Id_TipoCR { get; set; }
        public Nullable<int> Id_FolioCR { get; set; }
        [Display(Name = "Importe")]
        public Nullable<decimal> Cargos { get; set; }
        public Nullable<decimal> Abonos { get; set; }
        public Nullable<byte> Estatus { get; set; }
        [Display(Name = "Clasificación")]
        [Required(ErrorMessage = "*")]
        public Nullable<byte> Id_ClasPoliza { get; set; }
        [Display(Name = "Subclasificación")]
        [Required(ErrorMessage = "*")]
        public Nullable<byte> Id_SubClasificaPol { get; set; }

        public Control_Fechas cFechas { get; set; }

        [Display(Name = "Póliza de Orden Comprometido")]
        public String Poliza_Comprometido { get; set; }
        [Display(Name = "Póliza de Orden Devengado")]
        public String Poliza_Devengado { get; set; }
        [Display(Name = "Póliza de Orden Ejercido")]
        public String Poliza_Ejercido { get; set; }
        [Display(Name = "Póliza de Orden Pagado")]
        public String Poliza_Pagado { get; set; }
        [Display(Name = "Póliza de Orden Devengado")]
        public String Poliza_DevengadoIng { get; set; }
        [Display(Name = "Póliza de Orden Recaudado")]
        public String Poliza_Recaudado { get; set; }

        [Display(Name = "Cancelación de Póliza Orden Comprometido")]
        public String Poliza_ComprometidoC { get; set; }
        [Display(Name = "Cancelación de Póliza Orden Devengado")]
        public String Poliza_DevengadoC { get; set; }
        [Display(Name = "Cancelación de Póliza Orden Ejercido")]
        public String Poliza_EjercidoC { get; set; }
        [Display(Name = "Cancelación de Póliza Orden Pagado")]
        public String Poliza_PagadoC { get; set; }
        [Display(Name = "Cancelación de Póliza Orden Devengado")]
        public String Poliza_DevengadoIngC { get; set; }
        [Display(Name = "Cancelación de Póliza Orden Recaudado")]
        public String Poliza_RecaudadoC { get; set; }
        [Display(Name = "Cancelación de Póliza Diario")]
        public String Poliza_DiarioC { get; set; }
        public SelectList ListaId_ClasPoliza { get; set; }
        public SelectList ListaId_SubClasificaPol { get; set; }
        public String TipoPoliza { get; set; }


        public String Id_Fuente { get; set; }
        public short? Id_Banco { get; set; }
        public String Banco { get; set; }
        public String CtaBancaria { get; set; }


        public string Recibi_Donativo { get; set; }
        public Nullable<int> Id_Folio_Donativo { get; set; }
        public string RFC_Donativo { get; set; }
        public Nullable<short> Tipo_Donativo { get; set; }
        public string Facturas { get; set; }
        public bool? MesCerrado { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio { get; set; }
        public Nullable<short> Id_TipoCompromiso { get; set; }
        public Nullable<int> Id_FolioCompromiso { get; set; }
        public Ca_BeneficiariosModel Ca_Beneficiarios { get; set; }
        public Ca_ClasificaPolizasModel Ca_ClasificaPolizas { get; set; }
        public Ca_CuentasBancariasModel Ca_CuentasBancarias { get; set; }
        public Ca_TipoPolizasModel Ca_TipoPolizas { get; set; }
        public Ca_TipoContrarecibosModel Ca_TipoContrarecibo { get; set; }
        public List<De_Comprobaciones> De_Comprobaciones { get; set; }
        public List<De_Comprobaciones> De_Comprobaciones1 { get; set; }
        public List<De_PolizasModel> De_Polizas { get; set; }
        public List<Ma_CompromisosModel> Ma_Compromisos { get; set; }
        public Ma_ContrarecibosModel Ma_Contrarecibos { get; set; }
        public De_ReferenciasPolizasModel De_ReferenciasPolizas { get; set; }
        public List<Object> Botonera { get; set; }


        public bool ValidaAutomatica(int tipo, byte Clasi, byte Sub)
        {
            ClasificaPolizaDAL DAL = new ClasificaPolizaDAL();
            Ca_ClasificaPolizas ma = DAL.GetByID(x => x.Id_TipoPoliza == tipo && x.Id_ClasificaPoliza == Clasi && x.Id_SubClasificaPoliza == Sub);
            return ma.Automatica;
        }
        public int ObtenerFolio(Int16 tipo, Int16 mes)
        {
            MaPolizasDAL DAL = new MaPolizasDAL();
            BD_TesoreriaEntities bd = new BD_TesoreriaEntities();

            int registros = DAL.Get(x => x.Id_TipoPoliza == tipo && x.Id_MesPoliza == mes).Count();
            if (registros != 0)
            {
                Int32 le = (from m in bd.Ma_Polizas
                            where m.Id_TipoPoliza == tipo && m.Id_MesPoliza == mes
                            select m.Id_FolioPoliza).Max();
                return (le);
            }
            else
            {
                return (0);
            }
        }
    }


    #region Presupuesto Egresos
    public class MA_PresupuestoEgModel
    {
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Centro Gestor")]
        public string Id_Area { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Función")]
        public string Id_Funcion { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Compromiso")]
        public string Id_Actividad { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Clasificación Programática")]
        public string Id_ClasificacionP { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Programa Presupuestario")]
        public string Id_Programa { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Proyecto/Proceso")]
        public string Id_Proceso { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Tipo Meta")]
        //[ValidacionRemota("ValidarProceso", "Presupuestos", "Tesoreria", AdditionalFields = "Id_Proceso,Id_TipoMeta", ErrorMessage = "*")]
        //[ValidarProcesoAttribute("La meta debe pertenecer al proyecto", "Id_Proceso")]
        public string Id_TipoMeta { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [ValidacionRemota("ValidarActividadMIR", "Presupuestos", "Tesoreria", AdditionalFields = "Id_ActividadMIR, Id_Proceso", ErrorMessage = "*")]
        [Display(Name = "Actividad MIR")]
        public string Id_ActividadMIR { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [ValidacionRemota("ValidarAccion", "Presupuestos", "Tesoreria", AdditionalFields = "Id_Accion,Id_ActividadMIR, Id_Proceso", ErrorMessage = "*")]
        [Display(Name = "Acción u Obra")]
        public string Id_Accion { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Dimensión Geográfica")]
        public string Id_Alcance { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Tipo de Gasto")]
        public string Id_TipoG { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Fuente de Financiamiento")]
        public string Id_Fuente { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Año de Financiamiento")]
        public string AnioFin { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Objeto de Gasto")]
        public string Id_ObjetoG { get; set; }
        public string FolioPoliza { get; set; }
        public string Fecha_String { get; set; }
        [Display(Name = "Clave Presupuestaria")]
        public string Id_ClavePresupuesto { get; set; }
        [Display(Name = "Enero")]
        public Nullable<decimal> Aprobado01 { get; set; }
        [Display(Name = "Febrero")]
        public Nullable<decimal> Aprobado02 { get; set; }
        [Display(Name = "Marzo")]
        public Nullable<decimal> Aprobado03 { get; set; }
        [Display(Name = "Abril")]
        public Nullable<decimal> Aprobado04 { get; set; }
        [Display(Name = "Mayo")]
        public Nullable<decimal> Aprobado05 { get; set; }
        [Display(Name = "Junio")]
        public Nullable<decimal> Aprobado06 { get; set; }
        [Display(Name = "Julio")]
        public Nullable<decimal> Aprobado07 { get; set; }
        [Display(Name = "Agosto")]
        public Nullable<decimal> Aprobado08 { get; set; }
        [Display(Name = "Septiembre")]
        public Nullable<decimal> Aprobado09 { get; set; }
        [Display(Name = "Octubre")]
        public Nullable<decimal> Aprobado10 { get; set; }
        [Display(Name = "Noviembre")]
        public Nullable<decimal> Aprobado11 { get; set; }
        [Display(Name = "Diciembre")]
        public Nullable<decimal> Aprobado12 { get; set; }
        //[Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        //[ValidarTotal("*", "Id_Total")]
        public Nullable<decimal> Total { get; set; }
        public Nullable<byte> Id_Proyecto { get; set; }
        public Nullable<byte> Id_MesPO_Aprobado { get; set; }
        public Nullable<int> Id_FolioPO_Aprobado { get; set; }
        public Nullable<byte> Id_MesPO_Aprobado_C { get; set; }
        public Nullable<int> Id_FolioPO_Aprobado_C { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        public Nullable<System.DateTime> Fecha_Aprobado { get; set; }
        public Nullable<System.DateTime> Fecha_CancelaAprobado { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_act { get; set; }
        public string Id_ClavePresupuestoEgFormato {get;set;}

        public Ca_AreasModel Ca_Areas { get; set; }
        public Ca_FuncionesModel Ca_Funciones { get; set; }
        public Ca_ActividadesInstModel Ca_ActividadInst { get; set; }
        public Ca_ClasProgramaticaModel Ca_ClasProgramatica { get; set; }
        public Ca_ProgramasModel Ca_Programas { get; set; }
        public Ca_ProyectoModel Ca_Proyecto { get; set; }
        public Ca_TipoMetaModel Ca_TipoMeta { get; set; }
        public Ca_ActividadModel Ca_Actividad { get; set; }
        public Ca_AccionesModel Ca_Acciones { get; set; }
        public Ca_AlcanceGeoModel Ca_AlcanceGeo { get; set; }
        public Ca_TipoGastosModel Ca_TipoGastos { get; set; }
        public Ca_FuentesFinModel Ca_FuentesFin { get; set; }
        public Ca_FuentesFin_Ing Ca_FuentesFin_Ing { get; set; }
        public Ca_ObjetoGastoModel Ca_ObjetoGasto { get; set; }
        public Ca_CentroRecaudadorModel Ca_CentroRecaudador { get; set; }
        public Ca_ConceptosIngresosModel Ca_ConceptoIngresos { get; set; }

        public MA_PresupuestoEgModel()
        {

        }

    }
    public class ValidacionRemota : RemoteAttribute
    {
        public ValidacionRemota(string action, string controller, string area)
            : base(action, controller, area)
        {
            this.RouteData["area"] = area;
        }
    }
    public class ValidarProcesoAttribute : ValidationAttribute
    {
        public string[] PropertyNames { get; private set; }
        private const string errorMsg = "";

        public ValidarProcesoAttribute(string error, params string[] propertyNames)
            : base(() => errorMsg)
        {
            this.PropertyNames = propertyNames;
            this.ErrorMessage = error;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var modelo = (MA_PresupuestoEgModel)validationContext.ObjectInstance;
            string idProceso = modelo.Id_Proceso;
            string idMeta = modelo.Id_TipoMeta;
            /*La validación de la base de datos iría aquí*/
            MaPresupuestoEgDAL pDal = new MaPresupuestoEgDAL();
            List<MA_PresupuestoEg> presupuesto = pDal.Get(x => x.Id_Proceso == idProceso && x.Id_TipoMeta == idMeta).ToList();
            if (presupuesto == null && presupuesto.Count == 0)
            {
                return new ValidationResult(null);
            }
            return ValidationResult.Success;
        }
    }

    public class ValidarTotal : ValidationAttribute
    {
        public string[] PropertyNames { get; private set; }
        private const string errorMsg = "";

        public ValidarTotal(string error, params string[] propertyNames)
            : base(() => errorMsg)
        {
            this.PropertyNames = propertyNames;
            this.ErrorMessage = error;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var modelo = (MA_PresupuestoEgModel)validationContext.ObjectInstance;
            decimal Total = (modelo.Total.HasValue ? modelo.Total.Value : 0);
            if (Total == 0)
            {
                return new ValidationResult(null);
            }
            return ValidationResult.Success;
        }
    }
    public class ModalPresupuestoModel
    {
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Centro Gestor")]
        public string Id_AreaModal { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Función")]
        public string Id_FuncionModal { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Compromiso")]
        public string Id_ActividadModal { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Clasificación Programática")]
        public string Id_ClasificacionPModal { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Programa Presupuestario")]
        public string Id_ProgramaModal { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Proyecto/Proceso")]
        public string Id_ProcesoModal { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Tipo Meta")]
        public string Id_TipoMetaModal { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Actividad MIR")]
        public string Id_ActividadMIRModal { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Acción u Obra")]
        public string Id_AccionModal { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Dimensión Geográfica")]
        public string Id_AlcanceModal { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Tipo de Gasto")]
        public string Id_TipoGModal { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Fuente de Financiamiento")]
        public string Id_FuenteModal { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Año de Financiamiento")]
        public string AnioFinModal { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Objeto de Gasto")]
        public string Id_ObjetoGModal { get; set; }
    }

    #endregion
    #region Presupuesto Ingresos
    public partial class Ma_PresupuestoIngModel
    {
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [ValidacionRemota("ValidarId_Area", "PresupuestosIngresos", "Tesoreria", AdditionalFields = "Id_CentroRecaudador", ErrorMessage = "*")]
        [Display(Name = "Centro Recaudador")]
        public string Id_CentroRecaudador { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Dimensión Geográfica")]
        [ValidacionRemota("ValidarDimenGeo", "PresupuestosIngresos", "Tesoreria", AdditionalFields = "Id_Alcance", ErrorMessage = "*")]
        public string Id_Alcance { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Fuente de Financiamiento")]
        [ValidacionRemota("ValidarFuenteFin", "PresupuestosIngresos", "Tesoreria", AdditionalFields = "Id_Fuente", ErrorMessage = "*")]
        public string Id_Fuente { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Año de Financiamiento")]
        public string AnioFin { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [ValidacionRemota("ValidarCRI", "PresupuestosIngresos", "Tesoreria", AdditionalFields = "Id_Concepto", ErrorMessage = "*")]
        //[Display(Name = "Rubro de Ingreso")]
        [Display(Name = "CRI")]
        public string Id_Concepto { get; set; }
        [Display(Name = "Clave Presupuestaria")]
        public string Id_ClavePresupuesto { get; set; }
        [Display(Name = "Enero")]
        public Nullable<decimal> Estimado01 { get; set; }
        [Display(Name = "Febrero")]
        public Nullable<decimal> Estimado02 { get; set; }
        [Display(Name = "Marzo")]
        public Nullable<decimal> Estimado03 { get; set; }
        [Display(Name = "Abril")]
        public Nullable<decimal> Estimado04 { get; set; }
        [Display(Name = "Mayo")]
        public Nullable<decimal> Estimado05 { get; set; }
        [Display(Name = "Junio")]
        public Nullable<decimal> Estimado06 { get; set; }
        [Display(Name = "Julio")]
        public Nullable<decimal> Estimado07 { get; set; }
        [Display(Name = "Agosto")]
        public Nullable<decimal> Estimado08 { get; set; }
        [Display(Name = "Septiembre")]
        public Nullable<decimal> Estimado09 { get; set; }
        [Display(Name = "Octubre")]
        public Nullable<decimal> Estimado10 { get; set; }
        [Display(Name = "Noviembre")]
        public Nullable<decimal> Estimado11 { get; set; }
        [Display(Name = "Diciembre")]
        public Nullable<decimal> Estimado12 { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        public Nullable<decimal> Total { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_act { get; set; }
        public Nullable<byte> Id_MesPO_Estimado { get; set; }
        public Nullable<int> Id_FolioPO_Estimado { get; set; }
        public Nullable<byte> Id_MesPO_Estimado_C { get; set; }
        public Nullable<int> Id_FolioPO_Estimado_C { get; set; }
        public string FolioPoliza { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        public Nullable<System.DateTime> Fecha_Estimado { get; set; }
        public Nullable<System.DateTime> Fecha_CancelaEstimado { get; set; }
        public Ca_CentroRecaudadorModel Ca_CentroRecaudador { get; set; }
        public Ca_FuentesFin_IngModel Ca_FuentesFin_Ing { get; set; }
        public Ca_AlcanceGeoModel Ca_AlcanceGeo { get; set; }
        public Ca_ConceptosIngresosModel Ca_ConceptosIngresos { get; set; }
        public string Id_ClavePresupuestoFormato { get; set; }
    }

    public class ModalPresupuestoIngModel
    {
        [Display(Name = "Centro Recaudador")]
        public string Id_CentroRecaudadorModal { get; set; }
        [Display(Name = "Dimensión Geográfica")]
        public string Id_AlcanceModal { get; set; }
        [Display(Name = "Fuente de Financiamiento")]
        public string Id_FuenteModal { get; set; }
        [Display(Name = "Año de Financiamiento")]
        public string AnioFinModal { get; set; }
        [Display(Name = "CRI")]
        public string Id_ConceptoModal { get; set; }
        public string Id_ClavePresupuestoModal { get; set; }
    }
    #endregion
    public class Ma_NominaModel
    {
        public string No_Control { get; set; }
        public string Tipo_Nomina { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        public string No_Nomina { get; set; }
        public Nullable<DateTime> Fecha { get; set; }
        public string CURP { get; set; }
        public string Nombre { get; set; }
        public string Apellido_Pat { get; set; }
        public string Apellido_Mat { get; set; }
        public Nullable<int> Id_Banco_RH { get; set; }
        public string No_Tarjeta { get; set; }
        public string Tipo_Pago { get; set; }
        public Nullable<decimal> Neto { get; set; }
        public string Percepciones { get; set; }
        public string Deducciones { get; set; }
        public Nullable<bool> Pagado { get; set; }
        public Nullable<int> Id_FolioPolE { get; set; }
        public Nullable<byte> Id_MesPolE { get; set; }
        public Nullable<short> Id_Banco { get; set; }
        public Nullable<int> Id_ctaBancaria { get; set; }
        public Nullable<int> No_Cheque { get; set; }
        public Nullable<bool> Asignado { get; set; }
        public Nullable<byte> Quincena { get; set; }
    }
    public partial class Ma_TransferenciasModel
    {
        [Display(Name = "Folio")]
        [Required(ErrorMessage = "*")]
        public int Id_Transferencia { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "*")]
        public string Descrip { get; set; }
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "*")]
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<byte> Id_Mes { get; set; }
        public Nullable<byte> Id_Mes_Origen { get; set; }
        public Nullable<short> Id_Usuario { get; set; }
        public Nullable<decimal> Importe_AMP { get; set; }
        public Nullable<decimal> Importe_RED { get; set; }
        public string Importe_AMP_f { get; set; }
        public string Importe_RED_f { get; set; }
        [Display(Name = "Tipo transferencia")]
        public Nullable<byte> Id_TipoT { get; set; }
        [Display(Name = "Estatus")]
        public Nullable<byte> Id_Estatus { get; set; }
        public Nullable<System.DateTime> Fecha_Afecta { get; set; }
        public string DescripAjuste { get; set; }
        public Nullable<decimal> Importe_AmpAjuste { get; set; }
        public Nullable<decimal> Importe_RedAjuste { get; set; }
        public Nullable<bool> Funcion { get; set; }
        public string Id_Funcion { get; set; }
        public string Id_Area { get; set; }
        public Nullable<System.DateTime> Fecha_Reporte { get; set; }
        public string Id_Clasifica { get; set; }
        public string Id_OGInicial { get; set; }
        public string Id_OGFinal { get; set; }
        public Nullable<bool> Id_PptoModificado { get; set; }
        public Nullable<byte> Id_MesPO_Modificado { get; set; }
        public Nullable<int> Id_FolioPO_Modificado { get; set; }
        public Nullable<byte> Id_MesPO_Modificado_Cancela { get; set; }
        public Nullable<int> Id_FolioPO_Modificado_Cancela { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public Nullable<decimal> Importe_Requerido { get; set; }
        public Nullable<decimal> Importe_A_Cubrir { get; set; }
        public Nullable<bool> Alterado { get; set; }
        public SelectList ListaEstatus { get; set; }
        public SelectList ListaMeses { get; set; }
        public string PolizaOrdenModificado { get; set; }
        public string PolizaOrdenCancelado { get; set; }
        public Ca_AreasModel Ca_Areas { get; set; }

        public List<De_TransferenciaModel> De_Transferencias { get; set; }

        public Ma_TransferenciasModel()
        {
            CierreMensualDAL CierreMensualDAL = new CierreMensualDAL();
            this.ListaEstatus = new SelectList(Diccionarios.EstatusTransferencia, "Key", "Value");
            List<Ca_CierreMensual> meses = CierreMensualDAL.Get(x => x.Contable == false).ToList();
            List<Ca_CierreMensualModel> models = new List<Ca_CierreMensualModel>();
            foreach (Ca_CierreMensual item in meses)
            {
                Ca_CierreMensualModel temp = new Ca_CierreMensualModel();
                temp.Id_Mes = item.Id_Mes;
                temp.desc = Diccionarios.Meses[item.Id_Mes];
                models.Add(temp);
            }
            this.ListaMeses = new SelectList(models, "Id_Mes", "desc");
        }
    }
    public class Ma_ComprobacionesModel
    {
        public int Id_FolioGC { get; set; }
        public Nullable<decimal> Importe_Original { get; set; }
        public Nullable<decimal> Importe_Reintegro { get; set; }
        public Nullable<decimal> Importe_Sobrante { get; set; }
        public string Id_Cuenta_CtaXPagar { get; set; }
        public Nullable<byte> Id_MesPO_Comprometido { get; set; }
        public Nullable<int> Id_FolioPO_Comprometido { get; set; }
        public Nullable<byte> Id_MesPO_Comprometido_C { get; set; }
        public Nullable<int> Id_FolioPO_Comprometido_C { get; set; }
        public Nullable<byte> Id_MesPO_Devengado { get; set; }
        public Nullable<int> Id_FolioPO_Devengado { get; set; }
        public Nullable<byte> Id_MesPO_Devengado_C { get; set; }
        public Nullable<int> Id_FolioPO_Devengado_C { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public List<De_ComprobacionesModel> De_Comprobaciones { get; set; }

        [Display(Name = "Póliza Orden Comprometido")]
        public string Poliza_Comprometido { get; set; }
        [Display(Name = "Póliza Orden Comprometido")]
        public string Poliza_Comprometido_C { get; set; }
        [Display(Name = "Póliza Orden Devengado")]
        public string Poliza_Devengado { get; set; }
        [Display(Name = "Póliza Orden Devengado")]
        public string Poliza_Devengado_C { get; set; }

        public Ma_ComprobacionesModel()
        {
            De_Comprobaciones = new List<De_ComprobacionesModel>();
        }
    }

    public class Ma_ContrarecibosFGModel : Ma_ComprobacionesModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Tipo de Cuenta por Liquidar")]
        public byte Id_TipoCR { get; set; }
        [Display(Name = "Folio")]
        public int Id_FolioCR { get; set; }
        [Display(Name = "Fecha de Cuenta por Liquidar")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> FechaCR { get; set; }
        [Display(Name = "Tipo de Compromiso")]
        [Required(ErrorMessage = "*")]
        public Nullable<short> Id_TipoCompromiso { get; set; }
        [Display(Name = "Estatus")]
        [Required(ErrorMessage = "*")]
        public Nullable<byte> Id_EstatusCR { get; set; }
        public string EstatusCR { get; set; }
        public string EstatusGCDesc { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        [Display(Name = "Fecha de Vencimiento")]
        [Required(ErrorMessage = "*")]
        public Nullable<System.DateTime> FechaVen { get; set; }
        [Display(Name = "Estatus GC")]
        [Required(ErrorMessage = "*")]
        public Nullable<byte> Estatus_GC { get; set; }
        [Display(Name = "C.R. Impreso")]
        public Nullable<bool> Impreso_CR { get; set; }
        [Display(Name = "Aplicación Contable")]
        [Required(ErrorMessage = "*")]
        public string Id_CuentaFR { get; set; }
        [Display(Name = "Fuente de Financiamiento")]
        [Required(ErrorMessage = "*")]
        public string Id_Fuente { get; set; }
        [Display(Name = "Importe")]
        [Required(ErrorMessage = "*")]
        public Nullable<decimal> Cargos { get; set; }
        public Nullable<decimal> Abonos { get; set; }
        [Display(Name = "Usuario del Sistema")]
        [Required(ErrorMessage = "*")]
        public string Usu_CR { get; set; }
        public string Usuario_CR { get; set; }


        [Display(Name = "Cheque Impreso")]
        public Nullable<bool> Impreso_CH { get; set; }
        [Display(Name = "Cheque")]
        public Nullable<int> No_Cheque { get; set; }
        [Display(Name = "Fecha de Pago")]
        public Nullable<System.DateTime> Fecha_Pago { get; set; }
        [Display(Name = "Usuario Pago")]
        public string Usu_Pago { get; set; }
        [Display(Name = "SPEI")]
        public string Spei { get; set; }
        [Display(Name = "Fecha de Cierre")]
        public Nullable<System.DateTime> FechaCierreGC { get; set; }
        public Nullable<int> Id_FolioGC { get; set; }
        public Nullable<bool> Ejercicio_Anterior { get; set; }
        public Nullable<bool> AmpliacionFR { get; set; }
        public Nullable<byte> Id_Proyecto { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public Nullable<bool> Cerrado { get; set; }

        [Display(Name = "Original")]
        public Nullable<decimal> Original { get; set; }
        [Display(Name = "Reintegrado/Comprobado")]
        public Nullable<decimal> TotalReintegro { get; set; }
        [Display(Name = "Por Reintegrar/Comprobar")]
        public Nullable<decimal> Reintegros { get; set; }
        [Display(Name = "Excedido")]
        public Nullable<decimal> Sobrantes { get; set; }
        public Nullable<bool> SeReintegra { get; set; }

        public List<Object> Botonera { get; set; }

        [Display(Name = "Póliza CxL")]
        public string PolizaCR { get; set; }
        public Nullable<int> Id_FolioPolizaCR { get; set; }
        public Nullable<byte> Id_MesPolizaCR { get; set; }

        [Display(Name = "Póliza CxL_C")]
        public string PolizaCR_C { get; set; }
        public Nullable<int> Id_FolioPolizaCR_C { get; set; }
        public Nullable<byte> Id_MesPolizaCR_C { get; set; }
        [Display(Name = "Fecha")]
        public Nullable<System.DateTime> FechaCancelacionCR { get; set; }

        [Display(Name = "Póliza de Orden Ejercido")]
        public string Poliza_Ejercido { get; set; }
        public Nullable<int> Id_FolioPO_Ejercido { get; set; }
        public Nullable<byte> Id_MesPO_Ejercido { get; set; }

        [Display(Name = "Cancelación de Póliza Orden Ejercido")]
        public string Poliza_Ejercido_C { get; set; }
        public Nullable<byte> Id_MesPO_Ejercido_C { get; set; }
        public Nullable<int> Id_FolioPO_Ejercido_C { get; set; }
        [Display(Name = "Fecha")]
        public Nullable<System.DateTime> Fecha_CancelaEjercido { get; set; }

        [Display(Name = "Póliza Orden Pagado")]
        public string Poliza_Pagado { get; set; }
        public Nullable<byte> Id_MesPO_Pagado { get; set; }
        public Nullable<int> Id_FolioPO_Pagado { get; set; }

        [Display(Name = "Cancelación de Póliza Orden Pagado")]
        public string Poliza_Pagado_C { get; set; }
        public Nullable<byte> Id_MesPO_Pagado_C { get; set; }
        public Nullable<int> Id_FolioPO_Pagado_C { get; set; }
        [Display(Name = "Fecha")]
        public Nullable<System.DateTime> Fecha_CancelaPagado { get; set; }

        [Display(Name = "Póliza del Pago")]
        public string PolizaCheque { get; set; }
        public Nullable<int> Id_FolioPolizaCH { get; set; }
        public Nullable<byte> Id_MesPolizaCH { get; set; }

        [Display(Name = "Cancelación de Póliza del Pago")]
        public string PolizaCheque_C { get; set; }
        public Nullable<int> Id_FolioPolizaCH_C { get; set; }
        public Nullable<byte> Id_MesPolizaCH_C { get; set; }
        [Display(Name = "Fecha")]
        public Nullable<System.DateTime> FechaCancelacion_CH { get; set; }

        public Ca_BeneficiariosModel Ca_Beneficiarios { get; set; }
        public Ca_TipoContrarecibosModel Ca_TipoContrarecibos { get; set; }
        public List<De_ContrarecibosModel> De_Contrarecibos { get; set; }
        public Ca_ClasificaBeneficiariosModel Ca_ClasificaBeneficiarios { get; set; }
        public CA_UsuariosModel CA_Usuarios_UsuCR { get; set; }
        public Ca_CuentasBancariasModel Ca_CuentasBancarias { get; set; }
        public Ca_TipoCompromisosModel Ca_TipoCompromisos { get; set; }
        public Ca_CuentasModel Ca_Cuentas_FR { get; set; }
        public Ca_BeneficiariosCuentasModel Ca_BeneficiariosCuentas_FR { get; set; }
        public Ca_FuentesFinModel Ca_FuentesFin { get; set; }

        //FG
        public short? No_Comprobacion { get; set; }
        public DateTime? Fecha_Comprobacion { get; set; }
        public SelectList ListaId_TipoCompromiso { get; set; }

        public Ma_ContrarecibosFGModel(Byte TipoCR)
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            TipoContrarecibosDAL tipoContra = new TipoContrarecibosDAL();
            TipoCompromisosDAL tipoCompromisos = new TipoCompromisosDAL();
            Fondos_GastosBL bl = new Fondos_GastosBL();
            this.FechaCR = DateTime.Now;
            this.FechaVen = DateTime.Now;
            this.Cargos = 0;
            this.Id_FolioCR = 0;
            this.Impreso_CR = false;
            this.Impreso_CH = false;
            this.Ejercicio_Anterior = false;
            this.Id_TipoCR = TipoCR;
            this.Id_EstatusCR = 1;
            this.Estatus_GC = 1;
            this.Ca_TipoContrarecibos = new Llenado().LLenado_CaTipoContrarecibos(TipoCR);
            List<object> btn = new List<object>();
            bl.AddBoton(ref btn, "bNuevo");
            bl.AddBoton(ref btn, "bBuscar");
            bl.AddBoton(ref btn, "bSalir");
            this.Botonera = btn;
            this.Ca_Cuentas_FR = new Ca_CuentasModel();
            this.Id_EstatusCR = 1;
            this.Fecha_Pago = null;
            this.EstatusCR = Diccionarios.Estatus_CR.SingleOrDefault(x=> x.Key == this.Id_EstatusCR).Value;
            this.EstatusGCDesc = Diccionarios.Estatus_GC[this.Estatus_GC.Value];
            this.ListaId_TipoCompromiso = new SelectList(tipoCompromisos.Get(), "Id_TipoCompromiso", "Descripcion");
            //this.ListaId_Banco = new SelectList(new BancosDAL().Get(), "Id_Banco", "Descripcion");
            //this.ListaId_CtaBancaria = new SelectList(new CuentasBancariasDAL().Get(x => x.Id_Banco == null), "Id_CtaBancaria", "Descripcion");
            this.Usu_CR = appUsuario.NombreCompleto;
        }

        public Ma_ContrarecibosFGModel()
        {
            TipoContrarecibosDAL tipoContra = new TipoContrarecibosDAL();
            TipoCompromisosDAL tipoCompromisos = new TipoCompromisosDAL();
            this.FechaCR = DateTime.Now;
            this.FechaVen = DateTime.Now;
            this.Cargos = 0;
            this.Id_FolioCR = 0;
            this.Impreso_CR = false;
            this.Impreso_CH = false;
            this.Ejercicio_Anterior = false;
            this.Id_EstatusCR = 1;
            this.Fecha_Pago = null;
            this.EstatusCR = Diccionarios.Estatus_CR.SingleOrDefault(x => x.Key == this.Id_EstatusCR).Value;
            this.ListaId_TipoCompromiso = new SelectList(tipoCompromisos.Get(), "Id_TipoCompromiso", "Descripcion");
        }
    }

    public class Ma_ContrarecibosCPModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Tipo de Cuenta por Liquidar")]
        public byte Id_TipoCR { get; set; }
        [Display(Name = "Folio")]
        public int Id_FolioCR { get; set; }
        [Display(Name = "Tipo de Compromiso")]
        [Required(ErrorMessage = "*")]
        public Nullable<short> Id_TipoCompromiso { get; set; }
        [Display(Name = "Fecha de Cuenta por Liquidar")]
        [Required(ErrorMessage = "*")]
        public Nullable<System.DateTime> FechaCR { get; set; }
        [Display(Name = "Estatus")]
        [Required(ErrorMessage = "*")]
        public Nullable<byte> Id_EstatusCR { get; set; }
        public string Descripcion_EstatusCR { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        [Display(Name = "Fecha de Vencimiento")]
        public Nullable<System.DateTime> FechaVen { get; set; }
        [Display(Name = "Aplicación Contable")]
        [Required(ErrorMessage = "*")]
        public string Id_CuentaFR { get; set; }
        [Display(Name = "Fuente de Finanaciamiento")]
        [Required(ErrorMessage = "*")]
        public string Id_Fuente { get; set; }        
        [Display(Name = "Importe")]
        [Required(ErrorMessage = "*")]
        public Nullable<decimal> Cargos { get; set; }
        public Nullable<decimal> Abonos { get; set; }
        [Display(Name = "Usuario del Sistema")]
        [Required(ErrorMessage = "*")]
        public string Usu_CR { get; set; }
        [Display(Name = "CxL Impreso")]
        public Nullable<bool> Impreso_CR { get; set; }

        [Display(Name = "Cheque Impreso")]
        public Nullable<bool> Impreso_CH { get; set; }
        [Display(Name = "Cheque")]
        public Nullable<int> No_Cheque { get; set; }
        [Display(Name = "Fecha de Pago")]
        public Nullable<System.DateTime> Fecha_Pago { get; set; }
        [Display(Name = "Usuario Pago")]
        public string Usu_Pago { get; set; }
        [Display(Name = "SPEI")]
        public string Spei { get; set; }
        [Display(Name = "Fecha de Cierre")]
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public List<Object> Botonera { get; set; }
        [Display(Name = "Saldos acumulados en el mes de")]
        public Nullable<int> Id_Mes { get; set; } 
        public SelectList ListaId_Mes { get; set; }
        [Display(Name = "Saldo")]
        public string CargoMes { get; set; }
        [Display(Name = "Abonos")]
        public string AbonoMes { get; set; }

        [Display(Name = "Póliza del Pago")]
        public string PolizaCheque { get; set; }
        public Nullable<int> Id_FolioPolizaCH { get; set; }
        public Nullable<byte> Id_MesPolizaCH { get; set; }

        [Display(Name = "Cancelación de Póliza del Pago")]
        public string PolizaCheque_C { get; set; }
        public Nullable<int> Id_FolioPolizaCH_C { get; set; }
        public Nullable<byte> Id_MesPolizaCH_C { get; set; }
        [Display(Name = "Fecha")]
        public Nullable<System.DateTime> FechaCancelacion_CH { get; set; }
        public Nullable<int> IdPersona_ENP { get; set; }

        public Ca_PersonasModel Ca_Persona { get; set; }
        public Ca_BeneficiariosModel Ca_Beneficiarios { get; set; }
        public Ca_TipoContrarecibosModel Ca_TipoContrarecibos { get; set; }
        public List<De_ContrarecibosModel> De_Contrarecibos { get; set; }
        public Ca_ClasificaBeneficiariosModel Ca_ClasificaBeneficiarios { get; set; }
        public CA_UsuariosModel CA_Usuarios_UsuCR { get; set; }
        public Ca_CuentasBancariasModel Ca_CuentasBancarias { get; set; }
        public Ca_TipoCompromisosModel Ca_TipoCompromisos { get; set; }
        public Ca_CuentasModel Ca_Cuentas_FR { get; set; }
        public Ca_BeneficiariosCuentasModel Ca_BeneficiariosCuentas_FR { get; set; }
        public Ca_FuentesFinModel Ca_FuentesFin { get; set; }

        public Control_Fechas cfechas { get; set; }
        public SelectList ListaId_TipoCompromiso { get; set; }

        public Ma_ContrarecibosCPModel(Byte TipoCR)
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            TipoContrarecibosDAL tipoContra = new TipoContrarecibosDAL();
            TipoCompromisosDAL tipoCompromisos = new TipoCompromisosDAL();
            this.FechaCR = DateTime.Now;
            this.Cargos = 0;
            this.Id_FolioCR = 0;
            this.Impreso_CH = false;
            this.Id_TipoCR = TipoCR;
            this.Id_EstatusCR = Diccionarios.Valores_Estatus_CR.Programado;
            this.Descripcion_EstatusCR = Diccionarios.Estatus_CR[Diccionarios.Valores_Estatus_CR.Programado];
            this.Ca_TipoContrarecibos = new Llenado().LLenado_CaTipoContrarecibos(TipoCR);
            this.Ca_Cuentas_FR = new Ca_CuentasModel();
            this.Ca_Persona = new Ca_PersonasModel();
            //this.ListaId_CtaBancaria = new SelectList(new CuentasBancariasDAL().Get(x => x.Id_Banco == null), "Id_CtaBancaria", "Descripcion");
            this.Usu_CR = appUsuario.NombreCompleto;
            this.Impreso_CR = false;
            this.ListaId_Mes = new SelectList(Diccionarios.Meses, "Key", "Value");
            this.Botonera = new List<object>() { "bNuevo", "bBuscar", "bSalir" };
            this.ListaId_TipoCompromiso = new SelectList(tipoCompromisos.Get(), "Id_TipoCompromiso", "Descripcion");
        }

        public Ma_ContrarecibosCPModel()
        {
        }
    }

    public class CancelaCR_CPModel
    {
        [Display(Name = "Tipo de Cuenta por Liquidar")]
        public byte Id_TipoCR_C { get; set; }
        [Display(Name = "Folio")]
        public int Id_FolioCR_C { get; set; }
        [Display(Name = "Fecha de Cuenta por Liquidar")]
        [Required(ErrorMessage = "*")]
        public Nullable<System.DateTime> FechaCR_C { get; set; }

        public Control_Fechas cfechas { get; set; }
    }
    public partial class Ma_TransferenciasIngModel
    {
        public int Folio { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public string Descrip { get; set; }
        public Nullable<byte> Id_Mes { get; set; }
        public Nullable<byte> Id_Mes_Origen { get; set; }
        public Nullable<short> Id_Usuario { get; set; }
        public Nullable<decimal> Importe_AMP { get; set; }
        public Nullable<decimal> Importe_RED { get; set; }
        public Nullable<byte> Id_TipoT { get; set; }
        public Nullable<byte> Id_Estatus { get; set; }
        public Nullable<System.DateTime> Fecha_Afecta { get; set; }
        public string DescripAjuste { get; set; }
        public Nullable<decimal> Importe_AmpAjuste { get; set; }
        public Nullable<decimal> Importe_RedAjuste { get; set; }
        public string Id_Area { get; set; }
        public Nullable<System.DateTime> Fecha_Reporte { get; set; }
        public string Id_Clasifica { get; set; }
        public string Id_CRIInicial { get; set; }
        public string Id_CRIFinal { get; set; }
        public Nullable<bool> Id_PptoModificado { get; set; }
        public Nullable<byte> Id_MesPO_Modificado { get; set; }
        public Nullable<int> Id_FolioPO_Modificado { get; set; }
        public Nullable<byte> Id_MesPO_Modificado_Cancela { get; set; }
        public Nullable<int> Id_FolioPO_Modificado_Cancela { get; set; }
        public Nullable<decimal> Importe_Requerido { get; set; }
        public Nullable<decimal> Importe_A_Cubrir { get; set; }
        public Nullable<bool> Alterado { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public SelectList ListaEstatus { get; set; }
        public SelectList ListaMeses { get; set; }
        public string PolizaOrdenModificado { get; set; }
        public string PolizaOrdenCancelado { get; set; }
        public Ca_AreasModel Ca_Areas { get; set; }
        public List<De_TransferenciaIngModel> De_Transferencias { get; set; }

        public Ma_TransferenciasIngModel()
        {
            CierreMensualDAL CierreMensualDAL = new CierreMensualDAL();
            this.ListaEstatus = new SelectList(Diccionarios.EstatusTransferencia, "Key", "Value");
            List<Ca_CierreMensual> meses = CierreMensualDAL.Get(x => x.Contable == false).ToList();
            List<Ca_CierreMensualModel> models = new List<Ca_CierreMensualModel>();
            foreach (Ca_CierreMensual item in meses)
            {
                Ca_CierreMensualModel temp = new Ca_CierreMensualModel();
                temp.Id_Mes = item.Id_Mes;
                temp.desc = Diccionarios.Meses[item.Id_Mes];
                models.Add(temp);
            }
            this.ListaMeses = new SelectList(models, "Id_Mes", "desc");
        }
    }

    public class Ma_ReciboIngresosModel
    {
        public Ma_ReciboIngresosModel()
        {
            this.De_ReciboIngresos = new List<De_ReciboIngresosModel>();
            this.Botonera = new List<object>();
        }
        public int Folio { get; set; }
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "*")]
        public DateTime Fecha { get; set; }
        [Display(Name = "Caja Receptora")]
        [Required(ErrorMessage = "*")]
        public byte Id_CajaR { get; set; }
        [Display(Name = "Contribuyente")]
        [Required(ErrorMessage = "*")]
        [Range(1, int.MaxValue, ErrorMessage = "*")]
        public int IdContribuyente { get; set; }
        public string Observaciones { get; set; }
        public Nullable<decimal> Importe { get; set; }
        public Nullable<byte> IdTipoPDevengado { get; set; }
        public Nullable<int> IdFolioPDevengado { get; set; }
        public Nullable<byte> IdMesPDevengado { get; set; }
        public Nullable<byte> IdTipoPRecaudado { get; set; }
        public Nullable<int> IdFolioPRecaudado { get; set; }
        public Nullable<byte> IdMesPRecaudado { get; set; }
        public Nullable<byte> IdTipoPDevengadoC { get; set; }
        public Nullable<int> IdFolioPDevengadoC { get; set; }
        public Nullable<byte> IdMesPDevengadoC { get; set; }
        public Nullable<byte> IdTipoPRecaudadoC { get; set; }
        public Nullable<int> IdFolioPRecaudadoC { get; set; }
        public Nullable<byte> IdMesPRecaudadoC { get; set; }
        public Nullable<bool> Impreso { get; set; }
        [Display(Name="Estatus")]
        public Nullable<byte> IdEstatus { get; set; }
        [Display(Name = "Fecha de Recaudación")]
        public Nullable<System.DateTime> FechaRecaudacion { get; set; }
        [Display(Name = "Fecha de Cancelación")]
        public Nullable<System.DateTime> FechaCancelacion { get; set; }
        [Display(Name = "Banco")]
        [Required(ErrorMessage = "*")]
        public Nullable<short> Id_Banco { get; set; }
        [Display(Name = "Cuenta Bancaria")]
        [Required(ErrorMessage = "*")]
        public Nullable<short> Id_CtaBancaria { get; set; }
        public Nullable<short> uAct { get; set; }
        public Nullable<System.DateTime> fAct { get; set; }
        [Display(Name="Usuario de Captura")]
        public string Usuario_Captura { get; set; }
        [Display(Name = "Usuario de Recaudación")]
        public string Usuario_Recaudacion { get; set; }
        [Display(Name = "Usuario de Cancelación")]
        public string Usuario_Cancelacion { get; set; }
        public Nullable<byte> Id_TipoPIngresos { get; set; }
        public Nullable<int> Id_FolioPIngresos { get; set; }
        public Nullable<byte> Id_MesPIngresos { get; set; }
        public Nullable<byte> Id_TipoPDiario { get; set; }
        public Nullable<int> Id_FolioPDiario { get; set; }
        public Nullable<byte> Id_MesPDiario { get; set; }
        public Nullable<byte> Id_TipoPIngresosC { get; set; }
        public Nullable<int> Id_FolioPIngresosC { get; set; }
        public Nullable<byte> Id_MesPIngresosC { get; set; }
        public Nullable<byte> Id_TipoPDiarioC { get; set; }
        public Nullable<int> Id_FolioPDiarioC { get; set; }
        public Nullable<byte> Id_MesPDiarioC { get; set; }
        [Display(Name="Domicilio del Contribuyente")]
        public string DomicilioContribuyente { get; set; }
        public string EstatusDescipcion { get; set; }
        [Display(Name = "Nombre del Contribuyente")]
        public string NombreContribuyente { get; set; }
        /*Poilzas*/
        [Display(Name = "Póliza de Orden Devengado")]
        public String Poliza_Devengado { get; set; }
        [Display(Name = "Póliza de Orden Recaudado")]
        public String Poliza_Recaudado { get; set; }
        [Display(Name = "Cancelación de Póliza de Orden Devengado")]
        public String Poliza_DevengadoC { get; set; }
        [Display(Name = "Cancelación de Póliza de Orden Recaudado")]
        public String Poliza_RecaudadoC { get; set; }

        [Display(Name = "Póliza de Ingresos")]
        public String Poliza_Ingresos { get; set; }
        [Display(Name = "Cancelación de Póliza Ingresos")]
        public String Poliza_IngresosC { get; set; }

        [Display(Name = "Póliza de Diario")]
        public String Poliza_Diario { get; set; }
        
        [Display(Name = "Cancelación de Póliza de Diario")]
        public String Poliza_DiarioC { get; set; }


        public Ca_CajasReceptorasModel Ca_CajasReceptoras { get; set; }
        public Ca_CuentasBancariasModel Ca_CuentasBancarias { get; set; }
        public List<De_ReciboIngresosModel> De_ReciboIngresos { get; set; }
        public Ma_PolizasModel Ma_Polizas { get; set; }
        public Ma_PolizasModel Ma_Polizas1 { get; set; }
        public SelectList ListaCa_CajasReceptoras { get; set; }
        public SelectList ListaId_Banco { get; set; }
        public SelectList ListaId_CtaBancaria { get; set; }
        public List<object> Botonera { get; set; }
        public Control_Fechas cfechas { get; set; }
    }

    public class ReturnMaster
    {
        public string Accion { get; set; }
        public string Controlador { get; set; }
        public string Area { get; set; }
        public List<Campos> Parametros { get; set; }
    }

    public class Campos
    {
        public Campos() { }
        public Campos(string id, string valor)
        {
            this.Id = id;
            this.Valor = valor;
        }
        public string Id { get; set; }
        public string Valor { get; set; }
    }

    public class AnaliticoPolizas
    {
        public Nullable<decimal> ImporteDesde { get; set; }
        public Nullable<decimal> ImporteHasta { get; set; }
        public Nullable<DateTime> FechaDesde { get; set; }
        public Nullable<DateTime> FechaHasta { get; set; }
        [Display(Name = "Mes Inicio")]
        public Nullable<short> MesInicio { get; set; }
        [Display(Name = "Mes Fin")]
        public Nullable<short> MesFin { get; set; }
        [Display(Name = "Tipo Póliza")]
        public Nullable<short> Id_TipoPoliza { get; set; }
        [Display(Name = "Tipo CR")]
        public Nullable<short> Id_TipoCR { get; set; }
        [Display(Name = "Beneficiario")]
        public Nullable<short> Id_Beneficiario { get; set; }
        [Display(Name = "Cuenta Bancaria")]
        public Nullable<Int16> Id_CtaBancaria { get; set; }
        [Display(Name = "Cheque Inicial")]
        public Nullable<short> ChequeInicial { get; set; }
        [Display(Name = "Cheque Final")]
        public Nullable<short> ChequeFinal { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public SelectList ListaId_TipoPoliza { get; set; }
        public SelectList ListaId_TipoCR { get; set; }
        public SelectList ListaMeses { get; set; }
        public SelectList ListaCuentaBancaria { get; set; }
        public AnaliticoPolizas()
        {
            TipoPolizasDAL polizas = new TipoPolizasDAL();
            TipoContrarecibosDAL tipos = new TipoContrarecibosDAL();
            this.ListaId_TipoCR = new SelectList(tipos.Get(), "Id_TipoCR", "Descripcion");
            this.ListaId_TipoPoliza = new SelectList(polizas.Get(), "Id_TipoPoliza", "Descripcion");
            this.ListaMeses = new SelectList(Diccionarios.Meses, "Key", "Value");
            CuentasBancariasDAL dal= new CuentasBancariasDAL();
            this.ListaCuentaBancaria = new SelectList(dal.Get(), "Id_CtaBancaria", "Descripcion");
        }
    }
    public class Ma_MovimientosConciliacionModel
    {
        public short Id_CtaBancaria { get; set; }
        public string NoCuenta { get; set; }
        public short Id_Mes { get; set; }
        public short No_Registro { get; set; }
        public string Id_CuentaContable { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Tipo Movimiento")]
        public Nullable<short> Id_TipoMovimientoBancario { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Folio Movimiento")]
        public Nullable<short> Id_FolioMovimienotBancario { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "No. Cheque")]
        public Nullable<int> No_Cheque { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Referencia")]
        public string Referencia { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Movimiento")]
        public Nullable<short> Id_Movimiento { get; set; }
        [Required(ErrorMessage = "*")]
        public Nullable<System.DateTime> Fecha { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Importe")]
        public Nullable<decimal> Importe { get; set; }
        public Nullable<short> Estatus { get; set; }
        public string EstadoCuenta { get; set; }
        public Nullable<short> Origen { get; set; }
        public Nullable<short> Id_TipoMovimiento_Original { get; set; }
        public Nullable<short> Id_FolioMovimiento_Original { get; set; }
        public Nullable<byte> Id_TipoPoliza { get; set; }
        public Nullable<int> Id_FolioPoliza { get; set; }
        public Nullable<byte> Id_MesPoliza { get; set; }
        public Nullable<short> No_RegistroPoliza { get; set; }
        public Nullable<bool> Cancelado { get; set; }
        public Nullable<bool> Capturado { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        [Display(Name = "Cuenta Bancaria")]
        [Required(ErrorMessage = "*")]
        public string Id_TipoMovimiento { get; set; }

        public DateTime? fechaInicial { get; set; }
        public DateTime? fechaFin { get; set; }

        public SelectList ListaTipoMovimiento { get; set; }
        public SelectList ListaFolioMovimiento { get; set; }
        public SelectList ListaMovimiento { get; set; }

        public Ma_MovimientosConciliacionModel()
        {
            TipoMovBancariosDAL tipos = new TipoMovBancariosDAL();
            this.ListaFolioMovimiento = new SelectList(tipos.Get(), "Id_FolioMovB", "Descripcion");
            this.ListaTipoMovimiento = new SelectList(Diccionarios.TipoMovimientoBancario, "Key", "Value");
            this.ListaMovimiento = new SelectList(Diccionarios.Movimiento, "Key", "Value");
        }
    }
    public class ImporteContrarecibosModel
    {
        [Required(ErrorMessage = "*")]
        public string Cuenta { get; set; }
        public string CuentaDescripcion { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal Importe { get; set; }

        public string Cuenta2 { get; set; }
        public string CuentaDescripcion2 { get; set; }
        [Remote("Cuenta2", "Contrarecibos", AdditionalFields = "Cuenta2,Importe2", ErrorMessage = "*")]

        public decimal Importe2 { get; set; }


        public string Cuenta3 { get; set; }
        public string CuentaDescripcion3 { get; set; }
        [Remote("Cuenta3", "Contrarecibos", AdditionalFields = "Cuenta3,Importe3", ErrorMessage = "*")]

        public decimal Importe3 { get; set; }


        public string Cuenta4 { get; set; }
        public string CuentaDescripcion4 { get; set; }
        [Remote("Cuenta4", "Contrarecibos", AdditionalFields = "Cuenta4,Importe4", ErrorMessage = "*")]

        public decimal Importe4 { get; set; }



        public int IdTipoCR { get; set; }
        public int FolioCR { get; set; }
        public byte? TipoMovimiento { get; set; }
        public byte? TipoMovimiento2 { get; set; }
        public byte? TipoMovimiento3 { get; set; }
    }
}