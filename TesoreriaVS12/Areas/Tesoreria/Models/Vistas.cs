using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesoreriaVS12.Areas.Tesoreria.Models
{
    public class vBancosImpresionChequesModel
    {
        public short Id_CtaBancaria { get; set; }
        public string NombreCuentaBancaria { get; set; }
        public string NoCuenta { get; set; }
        public string NombreBanco { get; set; }
    }

    public class vListaChequesImpresionModel
    {
        public string NombreCompleto { get; set; }
        public Nullable<System.DateTime> FechaVen { get; set; }
        public int No_Cheque { get; set; }
        public Nullable<decimal> Importe { get; set; }
        public Nullable<short> Id_CtaBancaria { get; set; }
        public string NoCuenta { get; set; }
        public string ConceptoPago { get; set; }
        public string Importe_Letra { get; set; }
        public Nullable<System.DateTime> Fecha_Pago { get; set; }
        public Nullable<System.DateTime> FechaCR { get; set; }
        public Nullable<System.DateTime> Fecha_Ejercido { get; set; }
    }

    public class vFechaAsignacionPPyPDModel
    {
        public long id { get; set; }
        public string Fecha { get; set; }
    }
    public class vFechasAsignacionFRyGCModel
    {
        public long id { get; set; }
        public string Fecha { get; set; }
        public Nullable<short> Id_CtaBancaria { get; set; }
    }

    public class VW_DetalleReciboIngModel
    {
        public int Folio { get; set; }
        public byte IdRegistro { get; set; }
        public string Id_ClavePresupuestoIng { get; set; }
        public string Id_CentroRecaudador { get; set; }
        public string Id_Fuente { get; set; }
        public string AnioFin { get; set; }
        public string Id_Alcance { get; set; }
        public string Id_Concepto { get; set; }
        public string CRI { get; set; }
        public string IdCur { get; set; }
        public Nullable<byte> IdEstatus { get; set; }
        public string DescripcionEstatus { get; set; }
        public string CUR { get; set; }
        public Nullable<System.DateTime> FechaRecaudacion { get; set; }
        public Nullable<decimal> Importe { get; set; }
        public string Id_Cuenta { get; set; }
        public Nullable<byte> Id_Movimiento { get; set; }
        public int IdContribuyente { get; set; }
        public string NombreContribuyente { get; set; }
        public string CuentaBancaria { get; set; }
        public System.DateTime Fecha { get; set; }
        
    }
    public class ReporteCorteCaja
    {
        public string CUR { get; set; }
        public string Id_Concepto { get; set; }
        public string CRI { get; set; }
        public Nullable<byte> Id_Estatus { get; set; }
        public string DescripcionEstatus { get; set; }
        public List<VW_DetalleReciboIngModel> detalleCUR { get; set; }
        public ReporteCorteCaja()
        {
            this.detalleCUR = new List<VW_DetalleReciboIngModel>();
        }
    }
    public class DetalleCUR
    {
        public String CUR { get; set; }
        public DateTime? Fecha { get; set; }
        public int Recibo { get; set; }
        public Nullable<decimal> Importe { get; set; }

    }

    public class VW_ContrarecibosModel
    {
        public byte Id_TipoCR { get; set; }
        public int Id_FolioCR { get; set; }
        public Nullable<System.DateTime> FechaCR { get; set; }
        public string Descripcion { get; set; }
        public Nullable<System.DateTime> FechaVen { get; set; }
        public Nullable<int> Id_Beneficiario { get; set; }
        public Nullable<int> Id_FolioPolizaCR { get; set; }
        public Nullable<byte> Id_MesPolizaCR { get; set; }
        public Nullable<int> Id_FolioPolizaCR_C { get; set; }
        public Nullable<byte> Id_MesPolizaCR_C { get; set; }
        public Nullable<System.DateTime> FechaCancelacionCR { get; set; }
        public string Usu_CR { get; set; }
        public Nullable<bool> Impreso_CR { get; set; }
        public Nullable<int> Id_FolioPolizaCH { get; set; }
        public Nullable<byte> Id_MesPolizaCH { get; set; }
        public Nullable<short> Id_CtaBancaria { get; set; }
        public Nullable<int> No_Cheque { get; set; }
        public Nullable<System.DateTime> Fecha_Pago { get; set; }
        public string Usu_Pago { get; set; }
        public Nullable<byte> Id_EstatusCR { get; set; }
        public Nullable<decimal> Cargos { get; set; }
        public Nullable<decimal> Abonos { get; set; }
        public Nullable<bool> Impreso_CH { get; set; }
        public Nullable<short> Id_TipoCompromiso { get; set; }
        public Nullable<int> Id_FolioPolizaCH_C { get; set; }
        public Nullable<byte> Id_MesPolizaCH_C { get; set; }
        public Nullable<System.DateTime> FechaCancelacion_CH { get; set; }
        public string Id_CuentaFR { get; set; }
        public string Id_CuentaBeneficiario { get; set; }
        public Nullable<System.DateTime> FechaCierreGC { get; set; }
        public Nullable<byte> Estatus_GC { get; set; }
        public Nullable<int> Id_FolioGC { get; set; }
        public Nullable<bool> Ejercicio_Anterior { get; set; }
        public Nullable<bool> AmpliacionFR { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public Nullable<byte> Id_MesPO_Ejercido { get; set; }
        public Nullable<int> Id_FolioPO_Ejercido { get; set; }
        public Nullable<byte> Id_MesPO_Ejercido_C { get; set; }
        public Nullable<int> Id_FolioPO_Ejercido_C { get; set; }
        public Nullable<byte> Id_MesPO_Pagado { get; set; }
        public Nullable<int> Id_FolioPO_Pagado { get; set; }
        public Nullable<byte> Id_MesPO_Pagado_C { get; set; }
        public Nullable<int> Id_FolioPO_Pagado_C { get; set; }
        public Nullable<System.DateTime> Fecha_CancelaEjercido { get; set; }
        public Nullable<System.DateTime> Fecha_CancelaPagado { get; set; }
        public Nullable<bool> Cerrado { get; set; }
        public string Spei { get; set; }
        public string Nombre { get; set; }
        public int FolioCompromiso { get; set; }
        public string PolizaCR { get; set; }
        public string PolizaCR_C { get; set; }
        public string Poliza_Ejercido { get; set; }
        public string Poliza_Ejercido_C { get; set; }
        public string Poliza_Pagado { get; set; }
        public string Poliza_Pagado_C { get; set; }
        public string PolizaCheque { get; set; }
        public string PolizaCheque_C { get; set; }
        public string DEscripcionEstatus { get; set; }

        public Ca_TipoContrarecibosModel Ca_TipoContrarecibos { get; set; }
        public Ca_TipoCompromisosModel Ca_TipoCompromisos { get; set; }
        public Ca_CuentasBancariasModel Ca_CuentasBancarias { get; set; }
    }
    public class VW_ConciliacionModel
    {
        public string Tipo { get; set; }
        public string Movimiento { get; set; }
        public Nullable<int> No_Cheque { get; set; }
        public Nullable<decimal> Cargos { get; set; }
        public string Cargos_formato { get; set; }
        public Nullable<decimal> Abonos { get; set; }
        public string Abonos_formato { get; set; }
        public string Estatus { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public string Referencia { get; set; }
        public short Id_CtaBancaria { get; set; }
        public short Id_Mes { get; set; }
        public string NoCuenta { get; set; }
        public short No_Registro { get; set; }
    }
    public class VW_ProvedoresUsadosModel
    {
        public int IdPersona { get; set; }
        public Nullable<int> IdRepresentanteLegal { get; set; }
        public string Nombre { get; set; }
        public string CURP { get; set; }
        public string RFC { get; set; }
        public Nullable<byte> IdEstado { get; set; }
        public string Estado { get; set; }
        public Nullable<short> IdMunicipio { get; set; }
        public string Municipio { get; set; }
        public Nullable<short> IdLocalidad { get; set; }
        public string Localidad { get; set; }
        public Nullable<short> IdColonia { get; set; }
        public string Colonia { get; set; }
        public Nullable<short> IdCalle { get; set; }
        public string Calle { get; set; }
        public string NumeroExt { get; set; }
        public string NumeroInt { get; set; }
        public string CP { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public byte Id_TipoCR { get; set; }
        public string TipoCR { get; set; }
        public int Id_FolioCR { get; set; }
        public int Id_Factura { get; set; }
        public int Id_Proveedor { get; set; }
        public Nullable<byte> Id_TipoDocto { get; set; }
        public string TipoDocto { get; set; }
        public string No_docto { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<decimal> IVA { get; set; }
        public Nullable<decimal> Ret_ISR { get; set; }
        public Nullable<decimal> Ret_IVA { get; set; }
        public Nullable<decimal> Ret_Obra { get; set; }
        public Nullable<decimal> Ret_Otras { get; set; }
        public Nullable<byte> Id_Deduccion { get; set; }
        public Nullable<decimal> Otros { get; set; }
        public Nullable<byte> Id_Impuesto { get; set; }
        public Nullable<decimal> TOTAL { get; set; }
        public Nullable<int> No_Cheque { get; set; }
        public Nullable<short> Id_CtaBancaria { get; set; }
        public Nullable<int> Id_Prove_ant { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionDeduccion { get; set; }
        public string DescripcionImpuesto { get; set; }
        public Nullable<byte> Id_TipoBeneficiario { get; set; }
        public string TipoBeneficiario { get; set; }
        public Nullable<byte> Id_ClasBeneficiario { get; set; }
        public string ClasificaBeneficiario { get; set; }
    }
}