using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.Areas.Tesoreria.Models
{
    public class De_CompromisosModel : De_ClavePresupuestal
    {
        public short Id_TipoCompromiso { get; set; }
        public int Id_FolioCompromiso { get; set; }
        public short Id_Registro { get; set; }
        public string Id_Cuenta { get; set; }
        public string Id_ClavePresupuesto { get; set; }
        public Nullable<decimal> Importe { get; set; }
        public Nullable<byte> Id_Movimiento { get; set; }
        public Nullable<bool> Disponibilidad { get; set; }
        public Nullable<bool> AfectaCompro { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public MA_PresupuestoEgModel Ma_PresupuestoEg { get; set; }
        public Ca_CuentasModel Ca_Cuentas { get; set; }

        /*Campos Mios*/
        public Nullable<System.DateTime> Fecha_Orden { get; set; }
    }

    public class De_ContrarecibosModel : De_ClavePresupuestal
    {
        [Display(Name="Tipo de Cuenta por Liquidar")]
        public byte Id_TipoCR { get; set; }
        [Display(Name = "Folio de Cuenta por Liquidar")]
        public int Id_FolioCR { get; set; }
        [Display(Name = "Consecutivo")]
        public short Id_Registro { get; set; }
        public Nullable<short> No_Comprobacion { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public string Id_ClavePresupuesto { get; set; }
        public string Id_Cuenta { get; set; }
        public Nullable<decimal> Importe { get; set; }
        [Display(Name="Movimiento")]
        public Nullable<byte> Id_Movimiento { get; set; }
        public Nullable<byte> Id_MesPoliza { get; set; }
        public Nullable<int> Id_FolioPoliza { get; set; }
        public Nullable<bool> Disponibilidad { get; set; }
        public Nullable<int> Id_FolioPoliza_Cierre { get; set; }
        public Nullable<short> Id_MesPoliza_Cierre { get; set; }
        public Nullable<short> Id_TipoCR_Cierre { get; set; }
        public Nullable<int> Id_FolioCR_Cierre { get; set; }
        public Nullable<bool> Cerrado { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public String TipoContrarecibo { get; set; }
        public String TipoMovimiento { get; set; }
        public MA_PresupuestoEgModel Ma_PresupuestoEg { get; set; }
        public Ca_CuentasModel Ca_Cuentas { get; set; }
        public List<Object> Botonera { get; set; }


        //public decimal? Reintegrado { get; set; }
        //public decimal? Excedido { get; set; }
        public De_ContrarecibosModel()
        {
            this.Ca_Cuentas = new Ca_CuentasModel();
        }

        public De_ContrarecibosModel(byte TipoCR, int FolioCR)
        {
            this.Id_TipoCR = TipoCR;
            this.Id_FolioCR = FolioCR;
        }
    }

    public class De_PolizasModel : De_ClavePresupuestal
    {
        public byte Id_TipoPoliza { get; set; }
        public int Id_FolioPoliza { get; set; }
        public byte Id_MesPoliza { get; set; }
        public short Id_Registro { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        [Display(Name="Clave presupuestaria")]
        public string Id_ClavePresupuesto { get; set; }
        public string Id_ClavePresupuestoIng { get; set; }
        public string DescripcionMP { get; set; }
        public Nullable<byte> Id_Movimiento { get; set; }
        [Display(Name = "Cuenta")]
        public string Id_Cuenta { get; set; }
        public Nullable<decimal> Importe { get; set; }
        public Nullable<byte> Estatus { get; set; }
        public Nullable<byte> Id_TipoMovB { get; set; }
        public Nullable<byte> Id_FolioMovB { get; set; }
        public Nullable<bool> TransferidoC { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public List<object> Botonera { get; set; }
        public Ca_TipoMovBancariosModel Ca_TipoMovBancarios { get; set; }
        public MA_PresupuestoEgModel Ma_PresupuestoEg { get; set; }
        public Ma_PresupuestoIngModel Ma_PresupuestoIng { get; set; }
        public Ma_PolizasModel Ma_Polizas { get; set; }
        public Ca_CuentasModel Ca_Cuentas { get; set; }

        public short ObtenerFolio(Int16 Id_Tipo, Int16 Id_Folio, Int16 Id_Mes)
        {
            BD_TesoreriaEntities bd = new BD_TesoreriaEntities();
            DePolizasDAL dal = new DePolizasDAL();
            IEnumerable<De_Polizas> list = dal.Get(x=>x.Id_TipoPoliza==Id_Tipo && x.Id_FolioPoliza==Id_Folio && x.Id_MesPoliza==Id_Mes);
            if (list != null && list.Count() > 0)
            {
                short le = (from m in bd.De_Polizas
                            where m.Id_MesPoliza == Id_Mes && m.Id_FolioPoliza==Id_Folio && m.Id_TipoPoliza==Id_Tipo
                            select m.Id_Registro).Max();
                le++;
                return (le);
            }
            else
                return (1);
            
        }

        public De_PolizasModel()
        {
            this.Ca_Cuentas = new Ca_CuentasModel();
        }
    }

    public class De_DisponibilidadModel
    {
        public string Id_ClavePresupuesto { get; set; }
        public byte Mes { get; set; }
        public Nullable<decimal> Aprobado { get; set; }
        public Nullable<decimal> Ampliaciones1 { get; set; }
        public Nullable<decimal> Reducciones1 { get; set; }
        public Nullable<decimal> Ampliaciones { get; set; }
        public Nullable<decimal> Reducciones { get; set; }
        public Nullable<decimal> Vigente { get; set; }
        public Nullable<decimal> PreComprometido { get; set; }
        public Nullable<decimal> Comprometido { get; set; }
        public Nullable<decimal> Devengado { get; set; }
        public Nullable<decimal> Ejercido { get; set; }
        public Nullable<decimal> Pagado { get; set; }
        public Nullable<decimal> Disponible { get; set; }
        public string Id_Area { get; set; }
        public string Id_Funcion { get; set; }
        public string Id_Actividad { get; set; }
        public string Id_ClasificacionP { get; set; }
        public string Id_Programa { get; set; }
        public string Id_Proceso { get; set; }
        public string Id_TipoMeta { get; set; }
        public string Id_ActividadMIR { get; set; }
        public string Id_Accion { get; set; }
        public string Id_Alcance { get; set; }
        public string Id_TipoG { get; set; }
        public string Id_Fuente { get; set; }
        public string AnioFin { get; set; }
        public string Id_ObjetoG { get; set; }
        public string importeFormato { get; set; }

        public Ca_ObjetoGastoModel Ca_ObjetoGasto { get; set; }
    }

    public class De_FacturasModel
    {
        public byte Id_TipoCR { get; set; }
        public int Id_FolioCR { get; set; }
        [Required(ErrorMessage = "*")]
        public int Id_Proveedor { get; set; }
        public byte No_Comprobacion { get; set; }
        public int Id_Factura { get; set; }
        public Nullable<int> Id_ProveedorGC { get; set; }
        [Required(ErrorMessage="*")]
        public Nullable<byte> Id_TipoDocto { get; set; }

        [Required(ErrorMessage="*")]
        [StringLength(12,MinimumLength=12, ErrorMessage="**")]
        public string No_docto { get; set; }
        [Required(ErrorMessage = "*")]
        public Nullable<System.DateTime> Fecha { get; set; }
        [Required(ErrorMessage = "*")]
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<decimal> IVA { get; set; }
        public Nullable<decimal> Ret_ISR { get; set; }
        public Nullable<decimal> Ret_IVA { get; set; }
        public Nullable<decimal> Ret_Obra { get; set; }
        public Nullable<decimal> Ret_Otras { get; set; }
        public Nullable<byte> Id_Deduccion { get; set; }
        public Nullable<decimal> Otros { get; set; }
        public Nullable<byte> Id_Impuesto { get; set; }
        [Required(ErrorMessage="*")]
        [DataType(DataType.Text)]
        public Nullable<decimal> TOTAL { get; set; }
        public Nullable<int> No_Cheque { get; set; }
        public Nullable<short> Id_CtaBancaria { get; set; }
        public Nullable<int> Id_Prove_ant { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public String BeneficiarioName { get; set; }
        public String ProveedorName { get; set; }
        public String TipoDocumento { get; set; }
        public List<object> Botonera { get; set; }
        public SelectList ListaId_TipoDocto { get; set; }

        public SelectList ListaId_Impuesto { get; set; }
        public SelectList ListaId_Deduccion { get; set; }

        public Ma_ContrarecibosModel Ma_Contrarecibos { get; set; }

        public De_FacturasModel()
        {
            this.Botonera = new List<object>();
        }
    }
    public partial class DE_Banco_ChequeModel
    {
        [Display(Name = "Cuenta Bancaria")]
        [Required(ErrorMessage = "*")]
        public short Id_CtaBancaria { get; set; }
         [Display(Name = "Asignación")]
        public short Id_Asignacion { get; set; }
        [Display(Name = "Cheque Inicial")]
        [Required(ErrorMessage = "*")]
        public Nullable<int> Cheque_Ini { get; set; }
        [Display(Name = "Cheque Final")]
        [Required(ErrorMessage = "*")]
        public Nullable<int> Cheque_Fin { get; set; }
         [Display(Name = "Fecha Asignación")]
        public Nullable<System.DateTime> Fecha_Asigna { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_CuentasBancariasModel Ca_CuentasBancarias { get; set; }
        public DE_Banco_ChequeModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class De_ClavePresupuestal
    {
        [Display(Name = "Centro Gestor")]
        [Required(ErrorMessage="*")]
        //[ValidacionRemota("ValidarArea", "FocusOut", "Tesoreria", AdditionalFields = "Id_Area", ErrorMessage = "*")]
        public string Id_Area { get; set; }
        [Display(Name = "Función")]
        [Required(ErrorMessage = "*")]
        //[ValidacionRemota("ValidarFuncion", "FocusOut", "Tesoreria", AdditionalFields = "Id_Funcion", ErrorMessage = "*")]
        public string Id_Funcion { get; set; }
        [Display(Name = "Compromiso")]
        [Required(ErrorMessage = "*")]
        //[ValidacionRemota("ValidarActividad", "FocusOut", "Tesoreria", AdditionalFields = "Id_Actividad", ErrorMessage = "*")]
        public string Id_Actividad { get; set; }
        [Display(Name = "Clasificación Programática")]
        [Required(ErrorMessage = "*")]
        //[ValidacionRemota("ValidarClafificacion", "FocusOut", "Tesoreria", AdditionalFields = "Id_ClasificacionP", ErrorMessage = "*")]
        public string Id_ClasificacionP { get; set; }
        //[ValidacionRemota("ValidarPrograma", "FocusOut", "Tesoreria", AdditionalFields = "Id_Programa", ErrorMessage = "*")]
        [Display(Name = "Programa Presupuestario")]
        [Required(ErrorMessage = "*")]
        public string Id_Programa { get; set; }
        //[ValidacionRemota("ValidarProceso", "FocusOut", "Tesoreria", AdditionalFields = "Id_Programa", ErrorMessage = "*")]
        //[ValidarProcesoAttribute("El proceso debe pertenecer al programa", "Id_Programa")]
        [Display(Name = "Proyecto/Proceso")]
        [Required(ErrorMessage = "*")]
        public string Id_Proceso { get; set; }
        [Display(Name = "Tipo Meta")]
        //[ValidacionRemota("ValidarMeta", "FocusOut", "Tesoreria", AdditionalFields = "Id_Programa,Id_Proceso", ErrorMessage = "*")]
        //[ValidarProcesoAttribute("La meta debe pertenecer al proyecto", "Id_Proceso")]
        public string Id_TipoMeta { get; set; }
        [Display(Name = "Actividad MIR")]
        [Required(ErrorMessage = "*")]
        //[ValidacionRemota("ValidarMIR", "FocusOut", "Tesoreria", AdditionalFields = "Id_Programa,Id_Proceso,Id_TipoMeta", ErrorMessage = "*")]
        //[ValidarProcesoAttribute("La meta debe pertenecer al proyecto", "Id_TipoMeta")]
        public string Id_ActividadMIR { get; set; }
        [Display(Name = "Acción u Obra")]
        [Required(ErrorMessage = "*")]
        //[ValidacionRemota("ValidarAccion", "FocusOut", "Tesoreria", AdditionalFields = "Id_Programa,Id_Proceso,Id_TipoMeta,Id_ActividadMIR", ErrorMessage = "*")]
        //[ValidarProcesoAttribute("La acción debe pertenecer a la actividad MIR", "Id_Accion")]
        public string Id_Accion { get; set; }
        [Display(Name = "Dimensión Geográfica")]
        [Required(ErrorMessage = "*")]
        //[ValidacionRemota("ValidarAlcance", "FocusOut", "Tesoreria", AdditionalFields = "Id_Alcance", ErrorMessage = "*")]
        public string Id_Alcance { get; set; }
        [Display(Name = "Tipo de Gasto")]
        [Required(ErrorMessage = "*")]
        //[ValidacionRemota("ValidarTipoG", "FocusOut", "Tesoreria", AdditionalFields = "Id_TipoG", ErrorMessage = "*")]
        public string Id_TipoG { get; set; }
        [Display(Name = "Fuente de Financiamiento")]
        [Required(ErrorMessage = "*")]
        //[ValidacionRemota("ValidarFuente", "FocusOut", "Tesoreria", AdditionalFields = "Id_Fuente", ErrorMessage = "*")]
        public string Id_Fuente { get; set; }
        [Display(Name = "Año de Financiamiento")]
        [Required(ErrorMessage = "*")]
        public string AnioFin { get; set; }
        [Display(Name = "Objeto de Gasto")]
        [Required(ErrorMessage = "*")]
        //[ValidacionRemota("ValidarObjetoG", "FocusOut", "Tesoreria", AdditionalFields = "Id_ObjetoG", ErrorMessage = "*")]
        public string Id_ObjetoG { get; set; }
        /*Campos CRI*/
        [Display(Name = "Centro Recaudador")]
        [Required(ErrorMessage = "*")]
        public string Id_CentroRecaudador { get; set; }
        [Display(Name = "CRI")]
        [Required(ErrorMessage = "*")]
        public string Id_Concepto { get; set; }

        [Display(Name = "CUR")]
        [Required(ErrorMessage = "*")]
        public string IdCur { get; set; }

        public string Id_Fuente_Filtro { get; set; }

        public string Id_Actual { get; set; }

        public CA_CURModel Ca_Cur { get; set; }
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
        public Ca_FuentesFin_IngModel Ca_FuentesFin_Ing { get; set; }
        public Ca_ObjetoGastoModel Ca_ObjetoGasto { get; set; }
        public Ca_CentroRecaudadorModel Ca_CentroRecaudador { get; set; }
        public Ca_ConceptosIngresosModel Ca_ConceptoIngresos { get; set; }
    }

    public class De_Beneficiarios_GirosModel
    {
        public int Id_Beneficiario { get; set; }
        public short Id_GiroComercial { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_BeneficiariosModel Ca_BeneficiariosModel { get; set; }
        public Ca_GirosModel Ca_GirosModel { get; set; }
    }

    public class De_BeneficiarioContactosModel
    {
        public int Id_Beneficiario { get; set; }
        public short IdContacto { get; set; }
        public string Nombre { get; set; }
        public string CURP { get; set; }
        public string Tipo { get; set; }

        public short Id_Calculate(int IdBeneficiario)
        {
            try
            {
                DeBeneficiariosContactosDAL beneficiarios = new DeBeneficiariosContactosDAL();
                short c = beneficiarios.Get(x=> x.Id_Beneficiario == IdBeneficiario).Max(x=> x.IdContacto);
                c++;
                return c;
            }
            catch
            {
                return 1;
            }
        }
    }

    public partial class DE_BancosModel
    {
        [Display(Name = "Cuenta")]
        public short Id_CtaBancaria { get; set; }
        [Display(Name = "No Cheque")]
        public int No_Cheque { get; set; }
        [Display(Name = "Tipo poliza")]
        public Nullable<byte> Id_TipoPol { get; set; }
        [Display(Name = "Folio poliza")]
        public Nullable<int> Id_FolioPol { get; set; }
        [Display(Name = "Mes poliza")]
        public Nullable<byte> Id_MesPol { get; set; }
        [Display(Name = "Beneficiario")]
        public Nullable<int> Id_Beneficiario { get; set; }
        public Nullable<decimal> Importe { get; set; }
        [Display(Name = "Estatus")]
        public Nullable<short> Id_Estatus { get; set; }
        [Display(Name = "Observación")]
        public string Observa { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<bool> Exportado { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public string Estatus { get; set; }
    }
    public partial class De_TransferenciaModel : De_ClavePresupuestal
    {
        public int Id_Transferencia { get; set; }
        public short Id_Consecutivo { get; set; }
        public Nullable<short> Id_Movimiento { get; set; }
        public string Id_ClavePresupuesto { get; set; }
        public Nullable<decimal> Importe { get; set; }
        public Nullable<decimal> Pre01 { get; set; }
        public Nullable<decimal> Pre02 { get; set; }
        public Nullable<decimal> Pre03 { get; set; }
        public Nullable<decimal> Pre04 { get; set; }
        public Nullable<decimal> Pre05 { get; set; }
        public Nullable<decimal> Pre06 { get; set; }
        public Nullable<decimal> Pre07 { get; set; }
        public Nullable<decimal> Pre08 { get; set; }
        public Nullable<decimal> Pre09 { get; set; }
        public Nullable<decimal> Pre10 { get; set; }
        public Nullable<decimal> Pre11 { get; set; }
        public Nullable<decimal> Pre12 { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public MA_PresupuestoEgModel Ma_PresupuestoEg { get; set; }
    }
    
    public class De_ReferenciasPolizasModel
    {
         public byte IdTipoPoliza { get; set; }
        public int IdFolioPoliza { get; set; }
        public byte IdMesPoliza { get; set; }
        public Nullable<byte> IdTipoPCom { get; set; }
        public Nullable<int> IdFolioPCom { get; set; }
        public Nullable<byte> IdMesPCom { get; set; }
        public Nullable<byte> IdTipoPComC { get; set; }
        public Nullable<int> IdFolioPComC { get; set; }
        public Nullable<byte> IdMesPComC { get; set; }
        public Nullable<byte> IdTipoPDev { get; set; }
        public Nullable<int> IdFolioPDev { get; set; }
        public Nullable<byte> IdMesPDev { get; set; }
        public Nullable<byte> IdTipoPDevC { get; set; }
        public Nullable<int> IdFolioPDevC { get; set; }
        public Nullable<byte> IdMesPDevC { get; set; }
        public Nullable<byte> IdTipoPEje { get; set; }
        public Nullable<int> IdFolioPEje { get; set; }
        public Nullable<byte> IdMesPEje { get; set; }
        public Nullable<byte> IdTipoPEjeC { get; set; }
        public Nullable<int> IdFolioPEjeC { get; set; }
        public Nullable<byte> IdMesPEjeC { get; set; }
        public Nullable<byte> IdTipoPPag { get; set; }
        public Nullable<int> IdFolioPPag { get; set; }
        public Nullable<byte> IdMesPPag { get; set; }
        public Nullable<byte> IdTipoPPagC { get; set; }
        public Nullable<int> IdFolioPPagC { get; set; }
        public Nullable<byte> IdMesPPagC { get; set; }
        public Nullable<byte> IdTipoPDevI { get; set; }
        public Nullable<int> IdFolioPDevI { get; set; }
        public Nullable<byte> IdMesPDevI { get; set; }
        public Nullable<byte> IdTipoPDevIC { get; set; }
        public Nullable<int> IdFolioPDevIC { get; set; }
        public Nullable<byte> IdMesPDevIC { get; set; }
        public Nullable<byte> IdTipoPRec { get; set; }
        public Nullable<int> IdFolioPRec { get; set; }
        public Nullable<byte> IdMesPRec { get; set; }
        public Nullable<byte> IdTipoPRecC { get; set; }
        public Nullable<int> IdFolioPRecC { get; set; }
        public Nullable<byte> IdMesPRecC { get; set; }
        public Nullable<byte> IdTipoPolizaDC { get; set; }
        public Nullable<int> IdFolioPolizaDC { get; set; }
        public Nullable<byte> IdMesPolizaDC { get; set; }
    }

    public partial class DE_EvolucionModel : De_ClavePresupuestal
    {
        [Display(Name="Clave presupuestaria")]
        public string Id_ClavePresupuesto { get; set; }
        [Display(Name = "Mes")]
        public byte Mes { get; set; }
        public string Mes_Desc { get; set; }
        //public string Id_CentroRecaudador { get; set; }
        //public string Id_Fuente { get; set; }
        //public string AnioFin { get; set; }
        //public string Id_Alcance { get; set; }
        //public string Id_Concepto { get; set; }
        [Display(Name = "Estimado")]
        public Nullable<decimal> Estimado { get; set; }
        [Display(Name = "Ampliaciones")]
        public Nullable<decimal> Ampliaciones { get; set; }
        [Display(Name = "Reducciones")]
        public Nullable<decimal> Reducciones { get; set; }
        [Display(Name = "Modificado")]
        public Nullable<decimal> Modificado { get; set; }
        [Display(Name = "Devengado")]
        public Nullable<decimal> Devengado { get; set; }
        [Display(Name = "Recaudado")]
        public Nullable<decimal> Recaudado { get; set; }
        [Display(Name = "Por ejecutar")]
        public Nullable<decimal> PorEjecutar { get; set; }
    }

    public partial class De_TransferenciaIngModel : De_ClavePresupuestal
    {
        public int Folio { get; set; }
        public byte IdRegistro { get; set; }
        public Nullable<short> Id_Movimiento { get; set; }
        public string Id_ClavePresupuestoIng { get; set; }
        public Nullable<decimal> Importe { get; set; }
        public Nullable<decimal> Est01 { get; set; }
        public Nullable<decimal> Est02 { get; set; }
        public Nullable<decimal> Est03 { get; set; }
        public Nullable<decimal> Est04 { get; set; }
        public Nullable<decimal> Est05 { get; set; }
        public Nullable<decimal> Est06 { get; set; }
        public Nullable<decimal> Est07 { get; set; }
        public Nullable<decimal> Est08 { get; set; }
        public Nullable<decimal> Est09 { get; set; }
        public Nullable<decimal> Est10 { get; set; }
        public Nullable<decimal> Est11 { get; set; }
        public Nullable<decimal> Est12 { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public Ma_PresupuestoIngModel Ma_PresupuestoIng { get; set; }       
    }

    
    public class De_ReciboIngresosModel : De_ClavePresupuestal
    {
        public int Folio { get; set; }
        public byte IdRegistro { get; set; }
        public string Id_ClavePresupuestoIng { get; set; }
        [Required(ErrorMessage="*")]
        public Nullable<decimal> Importe { get; set; }
        public string Id_Cuenta { get; set; }
        public Nullable<byte> Id_Movimiento { get; set; }
        public Nullable<short> uAct { get; set; }
        public Nullable<System.DateTime> fAct { get; set; }
        public Ca_CuentasModel Ca_Cuentas { get; set; }
        public List<object> Botonera { get; set; }
    }

    public class De_ComprobacionesModel
    {
        public int Id_FolioGC { get; set; }
        public short Id_Consecutivo { get; set; }
        public Nullable<decimal> Importe { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<byte> Id_TipoPoliza { get; set; }
        public Nullable<byte> Id_MesPoliza { get; set; }
        public Nullable<int> Id_FolioPoliza { get; set; }
        public Nullable<int> Id_ReciboIng { get; set; }
        public Nullable<System.DateTime> Fecha_ReciboIng { get; set; }
        public Nullable<short> Id_CuentaBancaria { get; set; }
        public string Id_Cuenta { get; set; }
        public Nullable<System.DateTime> Fecha_C { get; set; }
        public Nullable<byte> Id_TipoPoliza_C { get; set; }
        public Nullable<byte> Id_MesPoliza_C { get; set; }
        public Nullable<int> Id_FolioPoliza_C { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_CuentasBancariasModel Ca_CuentasBancarias { get; set; }
        public Ca_CuentasModel Ca_Cuentas { get; set; }
    }

    public class DE_ContrarecibosArchivosModel
    {
        [Display(Name="Id Archivo")]
        public int IdArchivo { get; set; }
        [Display(Name="Tipo Cuenta por Liquidar")]
        public byte Id_TipoCR { get; set; }
        [Display(Name = "Folio Cuenta por Liquidar")]
        public int Id_FolioCR { get; set; }
        [Display(Name = "Nombre Archivo")]
        public string Nombre { get; set; }
        public string NombreSistema { get; set; }
        public System.DateTime Fecha { get; set; }
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }
    }
}