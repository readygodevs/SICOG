using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.DAL.Vistas;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using TesoreriaVS12.Filters;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    [AuthorizeLogin]
    public class ReportesPDFController : Controller
    {
        protected AccionDAL accion { get; set; }
        protected ActividadMIRDAL actividad { get; set; }
        protected ActividadDAL actividadins { get; set; }
        protected AlcanceDAL alcancegeo { get; set; }
        protected AreasDAL areas { get; set; }
        protected BancosDAL bancos { get; set; }
        protected BancosRHDAL bancosrh { get; set; }
        protected BeneficiariosDAL beneficiarios { get; set; }
        protected BeneficiariosCuentasDAL beneficiarioscuentas { get; set; }
        protected CallesDAL calles { get; set; }
        protected CausasCancelacionDAL causascancelacion { get; set; }
        protected CierreBancoDAL cierrebanco { get; set; }
        protected CierreMensualDAL cierremensual { get; set; }
        protected ClasificaBeneficiariosDAL clasificacionbeneficiario { get; set; }
        protected ClasificaPolizaDAL clasificacionpolizas { get; set; }
        protected ClasProgramaticaDAL clasprogramatica { get; set; }
        protected ColoniasDAL colonias { get; set; }
        protected ConceptosIngresosDAL conceptoingresos { get; set; }
        protected CuentasDAL cuentas { get; set; }
        protected CuentasBancariasDAL cuentasbancarias { get; set; }
        protected DeBancoDAL debanco { get; set; }
        protected DeBancoChequeDAL debancocheque { get; set; }
        protected EstadosDAL estados { get; set; }
        protected FuenteDAL fuentesfin { get; set; }
        protected FuncionDAL funciones { get; set; }
        protected ImpuestosDeduccionDAL impuestodeduccion { get; set; }
        protected LocalidadesDAL localidades { get; set; }
        protected MunicipiosDAL municipios { get; set; }
        protected ObjetoGDAL objetogasto { get; set; }
        protected PaisesDAL paises { get; set; }
        protected ParametrosDAL parametros { get; set; }
        protected PercepDeducDAL percepdeduc { get; set; }
        protected ProgramaDAL programas { get; set; }
        protected ProcesoDAL proyecto { get; set; }
        protected TipoBeneficiariosDAL tipobeneficiario { get; set; }
        protected TipoCompromisosDAL tipocompromiso { get; set; }
        protected TipoContrarecibosDAL tipocontrarecibo { get; set; }
        protected TipoDoctosDAL tipodoctos { get; set; }
        protected TipoFormatoChequesDAL tipoformatocheques { get; set; }
        protected TipoGastosDAL tipogasto { get; set; }
        protected TipoImpuestosDAL tipoimpuesto { get; set; }
        protected TipoMetaDAL tipometa { get; set; }
        protected TipoMovBancariosDAL tipomovbancarios { get; set; }
        protected TipoPagosDAL tipopagos { get; set; }
        protected TipoPolizasDAL tipopolizas { get; set; }
        protected TipoTrasferenciasEgDAL tipotransferenciaseg { get; set; }
        protected TipoTransferenciasIngDAL tipotrasnferenciasing { get; set; }
        protected DePolizasDAL depolizas { get; set; }
        protected MaPolizasDAL mapolizas { get; set; }
        protected GirosDAL giros { get; set; }
        protected DeBeneficiariosGirosDAL debeneficiariosgiros { get; set; }
        protected DeBeneficiariosContactosDAL debeneficiarioscontactos { get; set; }
        protected PersonasDAL personas { get; set; }
        private ConvertHtmlToString GenerarPDF { get; set; }
        public ReportesPDFController()
        {
            if (accion == null) accion = new AccionDAL();
            if (actividad == null) actividad = new ActividadMIRDAL();
            if (actividadins == null) actividadins = new ActividadDAL();
            if (alcancegeo == null) alcancegeo = new AlcanceDAL();
            if (areas == null) areas = new AreasDAL();
            if (bancos == null) bancos = new BancosDAL();
            if (bancosrh == null) bancosrh = new BancosRHDAL();
            if (beneficiarios == null) beneficiarios = new BeneficiariosDAL();
            if (beneficiarioscuentas == null) beneficiarioscuentas = new BeneficiariosCuentasDAL();
            if (calles == null) calles = new CallesDAL();
            if (causascancelacion == null) causascancelacion = new CausasCancelacionDAL();
            if (cierrebanco == null) cierrebanco = new CierreBancoDAL();
            if (cierremensual == null) cierremensual = new CierreMensualDAL();
            if (clasificacionbeneficiario == null) clasificacionbeneficiario = new ClasificaBeneficiariosDAL();
            if (clasificacionpolizas == null) clasificacionpolizas = new ClasificaPolizaDAL();
            if (clasprogramatica == null) clasprogramatica = new ClasProgramaticaDAL();
            if (colonias == null) colonias = new ColoniasDAL();
            if (conceptoingresos == null) conceptoingresos = new ConceptosIngresosDAL();
            if (cuentas == null) cuentas = new CuentasDAL();
            if (cuentasbancarias == null) cuentasbancarias = new CuentasBancariasDAL();
            if (debanco == null) debanco = new DeBancoDAL();
            if (debancocheque == null) debancocheque = new DeBancoChequeDAL();
            if (estados == null) estados = new EstadosDAL();
            if (fuentesfin == null) fuentesfin = new FuenteDAL();
            if (funciones == null) funciones = new FuncionDAL();
            if (impuestodeduccion == null) impuestodeduccion = new ImpuestosDeduccionDAL();
            if (localidades == null) localidades = new LocalidadesDAL();
            if (municipios == null) municipios = new MunicipiosDAL();
            if (objetogasto == null) objetogasto = new ObjetoGDAL();
            if (paises == null) paises = new PaisesDAL();
            if (parametros == null) parametros = new ParametrosDAL();
            if (percepdeduc == null) percepdeduc = new PercepDeducDAL();
            if (programas == null) programas = new ProgramaDAL();
            if (proyecto == null) proyecto = new ProcesoDAL();
            if (tipobeneficiario == null) tipobeneficiario = new TipoBeneficiariosDAL();
            if (tipocompromiso == null) tipocompromiso = new TipoCompromisosDAL();
            if (tipocontrarecibo == null) tipocontrarecibo = new TipoContrarecibosDAL();
            if (tipodoctos == null) tipodoctos = new TipoDoctosDAL();
            if (tipoformatocheques == null) tipoformatocheques = new TipoFormatoChequesDAL();
            if (tipogasto == null) tipogasto = new TipoGastosDAL();
            if (tipoimpuesto == null) tipoimpuesto = new TipoImpuestosDAL();
            if (tipometa == null) tipometa = new TipoMetaDAL();
            if (tipomovbancarios == null) tipomovbancarios = new TipoMovBancariosDAL();
            if (tipopagos == null) tipopagos = new TipoPagosDAL();
            if (tipopolizas == null) tipopolizas = new TipoPolizasDAL();
            if (tipotransferenciaseg == null) tipotransferenciaseg = new TipoTrasferenciasEgDAL();
            if (tipotrasnferenciasing == null) tipotrasnferenciasing = new TipoTransferenciasIngDAL();
            if (mapolizas == null) mapolizas = new MaPolizasDAL();
            if (depolizas == null) depolizas = new DePolizasDAL();
            if (giros == null) giros = new GirosDAL();
            if (debeneficiarioscontactos == null) debeneficiarioscontactos = new DeBeneficiariosContactosDAL();
            if (personas == null) personas = new PersonasDAL();
            if (debeneficiariosgiros == null) debeneficiariosgiros = new DeBeneficiariosGirosDAL();
            if (GenerarPDF == null)
            {
                GenerarPDF = new ConvertHtmlToString();
                GenerarPDF.TituloSistema = "Sistema";
            } 
        }

        #region Presupuestos
        public ActionResult Areas()
        {
            UsuarioLogueado Usuario = Session["appUsuario"] as UsuarioLogueado;
            List<Ca_AreasModel> ListaAreas = new List<Ca_AreasModel>();
            areas.Get().ToList().ForEach(x => { ListaAreas.Add(ModelFactory.getModel<Ca_AreasModel>(x, new Ca_AreasModel())); });
            GenerarPDF.NombreCompleto = Usuario.NombreCompleto;
            byte[] PDF = GenerarPDF.GenerarPDF("Areas", ListaAreas, this.ControllerContext);
            return File(PDF, "application/pdf");
        }

        public ActionResult Acciones()
        {
            List<Ca_AccionesModel> accLst = new List<Ca_AccionesModel>();
            IEnumerable<Ca_Acciones> entities = accion.Get();
            foreach (Ca_Acciones item in entities)
            {
                Ca_AccionesModel model = ModelFactory.getModel<Ca_AccionesModel>(item, new Ca_AccionesModel());
                model.CA_Proyecto = ModelFactory.getModel<Ca_ProyectoModel>(proyecto.GetByID(x => x.Id_Proceso == model.Id_Proceso), new Ca_ProyectoModel());
                model.CA_Actividad = ModelFactory.getModel<Ca_ActividadModel>(actividad.GetByID(x => x.Id_ActividadMIR2 == model.Id_ActividadMIR2 && x.Id_Proceso == model.Id_Proceso), new Ca_ActividadModel());
                accLst.Add(model);
            }
            byte[] PDF = GenerarPDF.GenerarPDF("Acciones", accLst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }

        public ActionResult Actividades()
        {
            List<Ca_ActividadModel> Lst = new List<Ca_ActividadModel>();
            IEnumerable<Ca_Actividad> entities = actividad.Get();
            foreach (Ca_Actividad item in entities)
            {
                Ca_ActividadModel model = ModelFactory.getModel<Ca_ActividadModel>(item, new Ca_ActividadModel());
                model.CA_Proyecto = ModelFactory.getModel<Ca_ProyectoModel>(proyecto.GetByID(x => x.Id_Proceso == model.Id_Proceso), new Ca_ProyectoModel());
                Lst.Add(model);
            }
            byte[] PDF = GenerarPDF.GenerarPDF("Actividades", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }

        public ActionResult AlcanceGeo()
        {
            List<Ca_AlcanceGeoModel> Lst = new List<Ca_AlcanceGeoModel>();
            alcancegeo.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_AlcanceGeoModel>(x, new Ca_AlcanceGeoModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("AlcanceGeo", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult ClasProgramatica()
        {
            List<Ca_ClasProgramaticaModel> Lst = new List<Ca_ClasProgramaticaModel>();
            clasprogramatica.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_ClasProgramaticaModel>(x, new Ca_ClasProgramaticaModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("ClasProgramatica", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult Compromisos()
        {
            List<Ca_ActividadesInstModel> actividadesLst = new List<Ca_ActividadesInstModel>();
            actividadins.Get().ToList().ForEach(x => { actividadesLst.Add(ModelFactory.getModel<Ca_ActividadesInstModel>(x, new Ca_ActividadesInstModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("Compromisos", actividadesLst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult FuentesFinanciamiento()
        {
            List<Ca_FuentesFinModel> Lst = new List<Ca_FuentesFinModel>();
            fuentesfin.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_FuentesFinModel>(x, new Ca_FuentesFinModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("FuentesFinanciamiento", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult Funciones()
        {
            List<Ca_FuncionesModel> funcionesLst = new List<Ca_FuncionesModel>();
            funciones.Get().ToList().ForEach(x => { funcionesLst.Add(ModelFactory.getModel<Ca_FuncionesModel>(x, new Ca_FuncionesModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("Funciones", funcionesLst,this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult Programas()
        {
            List<Ca_ProgramasModel> Lst = new List<Ca_ProgramasModel>();
            programas.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_ProgramasModel>(x, new Ca_ProgramasModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("Programas", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult Proyectos()
        {
            List<Ca_ProyectoModel> Lst = new List<Ca_ProyectoModel>();
            proyecto.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_ProyectoModel>(x, new Ca_ProyectoModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("Proyectos", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult TipoMeta()
        {
            List<Ca_TipoMetaModel> Lst = new List<Ca_TipoMetaModel>();
            tipometa.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoMetaModel>(x, new Ca_TipoMetaModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("TipoMeta", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }

        public ActionResult TipoGasto()
        {
            List<Ca_TipoGastosModel> Lst = new List<Ca_TipoGastosModel>();
            tipogasto.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoGastosModel>(x, new Ca_TipoGastosModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("TipoGasto", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult ObjetoGasto()
        {
            List<Ca_ObjetoGastoModel> Lst = new List<Ca_ObjetoGastoModel>();
            objetogasto.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_ObjetoGastoModel>(x, new Ca_ObjetoGastoModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("ObjetoGasto", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }

        #endregion
        #region sinNombre

        public ActionResult ContraRecibo()
        {
            return View();
        }
        public ActionResult PolizaDeCheque()
        {
            List<Ca_ObjetoGastoModel> Lst = new List<Ca_ObjetoGastoModel>();
            byte[] PDF = GenerarPDF.GenerarPDF_ChequePoliza("PolizaDeCheque", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult reciboIndividual() {
            return View();
        }
        public ActionResult reporteGeneral() {
            return View();
        }
        #endregion

        #region Contabilidad
        public ActionResult ClasificacionPolizas()
        {
            List<Ca_ClasificaPolizasModel> Lst = new List<Ca_ClasificaPolizasModel>();
            clasificacionpolizas.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_ClasificaPolizasModel>(x, new Ca_ClasificaPolizasModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("ClasificacionPolizas", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult Cuentas()
        {
            IEnumerable<CA_Cuentas> Lst = cuentas.Get().OrderBy(x => x.Id_CuentaFormato);
            byte[] PDF = GenerarPDF.GenerarPDF("Cuentas", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        #endregion
        #region Ingresos
        public ActionResult CajasReceptoras()
        {
            List<Ca_CajasReceptorasModel> Lst = new List<Ca_CajasReceptorasModel>();
            new CaCajasReceptorasDAL().Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_CajasReceptorasModel>(x, new Ca_CajasReceptorasModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("CajasReceptoras", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }

        public ActionResult CentrosRecaudadores()
        {
            List<Ca_CentroRecaudadorModel> Lst = new List<Ca_CentroRecaudadorModel>();
            new CentroRecaudadorDAL().Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_CentroRecaudadorModel>(x, new Ca_CentroRecaudadorModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("CentrosRecaudadores", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }

        public ActionResult FuentesFinIng()
        {
            List<Ca_FuentesFin_IngModel> Lst = new List<Ca_FuentesFin_IngModel>();
            new FuenteIngDAL().Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_FuentesFin_IngModel>(x, new Ca_FuentesFin_IngModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("FuentesFinIng", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult ConceptoIngresos()
        {
            List<Ca_ConceptosIngresosModel> Lst = new List<Ca_ConceptosIngresosModel>();
            new ConceptosIngresosDAL().Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_ConceptosIngresosModel>(x, new Ca_ConceptosIngresosModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("ConceptoIngresos", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }

        public ActionResult CUR()
        {
            List<CA_CURModel> Lst = new List<CA_CURModel>();
            new Ca_CURDAL().Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<CA_CURModel>(x, new CA_CURModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("CUR", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult Contribuyentes()
        {
            List<Ca_PersonasModel> Lst = new List<Ca_PersonasModel>();
            
            new PersonasDAL().Get().ToList().ForEach(x => { Lst.Add(new Llenado().Llenado_CaPersonas(x.IdPersona)); });
            byte[] PDF = GenerarPDF.GenerarPDF("Contribuyentes", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        #endregion
        #region Egresos
        public ActionResult TiposCompromiso()
        {
            List<Ca_TipoCompromisosModel> Lst = new List<Ca_TipoCompromisosModel>();
            tipocompromiso.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoCompromisosModel>(x, new Ca_TipoCompromisosModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("TiposCompromiso", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult TiposContrarecibo()
        {
            List<Ca_TipoContrarecibosModel> Lst = new List<Ca_TipoContrarecibosModel>();
            tipocontrarecibo.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoContrarecibosModel>(x, new Ca_TipoContrarecibosModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("TiposContrarecibo", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult TipoBeneficiario()
        {
            List<Ca_TipoBeneficiariosModel> Lst = new List<Ca_TipoBeneficiariosModel>();
            tipobeneficiario.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoBeneficiariosModel>(x, new Ca_TipoBeneficiariosModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("TipoBeneficiario", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }

        public ActionResult ClasificacionBeneficiario()
        {
            List<Ca_ClasificaBeneficiariosModel> Lst = new List<Ca_ClasificaBeneficiariosModel>();
            clasificacionbeneficiario.Get().ToList().ForEach(x =>
            {
                Ca_ClasificaBeneficiariosModel temp = ModelFactory.getModel<Ca_ClasificaBeneficiariosModel>(x, new Ca_ClasificaBeneficiariosModel());
                temp.Ca_Cuentas = ModelFactory.getModel<Ca_CuentasModel>(cuentas.GetByID(reg => reg.Id_Cuenta == temp.Id_Cuenta), new Ca_CuentasModel());
                Lst.Add(temp);
            });
            byte[] PDF = GenerarPDF.GenerarPDF("ClasificacionBeneficiario", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult TipoPago()
        {
            List<Ca_TipoPagosModel> Lst = new List<Ca_TipoPagosModel>();
            tipopagos.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoPagosModel>(x, new Ca_TipoPagosModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("TipoPago", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult Estados()
        {
            List<Ca_EstadosModel> Lst = new List<Ca_EstadosModel>();
            estados.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_EstadosModel>(x, new Ca_EstadosModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("Estados", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }

        public ActionResult Municipios()
        {
            List<Ca_MunicipiosModel> Lst = new List<Ca_MunicipiosModel>();
            IEnumerable<Ca_Municipios> entities = municipios.Get();
            foreach (Ca_Municipios item in entities)
            {
                Ca_MunicipiosModel model = ModelFactory.getModel<Ca_MunicipiosModel>(item, new Ca_MunicipiosModel());
                model.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                Lst.Add(model);
            }
            byte[] PDF = GenerarPDF.GenerarPDF("Municipios", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult Localidades()
        {
            List<Ca_LocalidadesModel> Lst = new List<Ca_LocalidadesModel>();
            IEnumerable<Ca_Localidades> entities = localidades.Get();
            foreach (Ca_Localidades item in entities)
            {
                Ca_LocalidadesModel model = ModelFactory.getModel<Ca_LocalidadesModel>(item, new Ca_LocalidadesModel());
                model.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                model.CA_Municipio = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado), new Ca_MunicipiosModel());
                Lst.Add(model);
            }
            byte[] PDF = GenerarPDF.GenerarPDF("Localidades", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult Colonias()
        {
            List<Ca_ColoniasModel> Lst = new List<Ca_ColoniasModel>();
            IEnumerable<Ca_Colonias> entities = colonias.Get();
            foreach (Ca_Colonias item in entities)
            {
                try
                {
                    Ca_ColoniasModel model = ModelFactory.getModel<Ca_ColoniasModel>(item, new Ca_ColoniasModel());
                    model.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                    model.CA_Municipio = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado), new Ca_MunicipiosModel());
                    model.CA_Localidad = ModelFactory.getModel<Ca_LocalidadesModel>(localidades.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado && x.Id_Localidad == model.Id_Localidad), new Ca_LocalidadesModel());
                    Lst.Add(model);
                }
                catch (Exception ex)
                {

                }

            }
            byte[] PDF = GenerarPDF.GenerarPDF("Colonias", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult Calles()
        {
            List<Ca_CallesModel> Lst = new List<Ca_CallesModel>();
            IEnumerable<Ca_Calles> entities = calles.Get();
            foreach (Ca_Calles item in entities)
            {
                try
                {
                    Ca_CallesModel model = ModelFactory.getModel<Ca_CallesModel>(item, new Ca_CallesModel());
                    model.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                    model.CA_Municipio = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado), new Ca_MunicipiosModel());
                    model.CA_Localidad = ModelFactory.getModel<Ca_LocalidadesModel>(localidades.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado && x.Id_Localidad == model.Id_Localidad), new Ca_LocalidadesModel());
                    Lst.Add(model);
                }
                catch (Exception ex)
                {

                }

            }
            byte[] PDF = GenerarPDF.GenerarPDF("Calles", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }

        public ActionResult Bancos()
        {
            List<Ca_BancosModel> dataModel = new List<Ca_BancosModel>();
            bancos.Get().ToList().ForEach(item =>
            {
                Ca_BancosModel temp = ModelFactory.getModel<Ca_BancosModel>(item, new Ca_BancosModel());
                temp.Ca_Cuentas = ModelFactory.getModel<Ca_CuentasModel>(cuentas.GetByID(x => x.Id_Cuenta == temp.Id_Cuenta), new Ca_CuentasModel());
                dataModel.Add(temp);
            });
            byte[] PDF = GenerarPDF.GenerarPDF("Bancos",dataModel, this.ControllerContext);
            return File(PDF, "application/pdf");
        }

        public ActionResult CuentasBancarias()
        {
            List<Ca_CuentasBancariasModel> Lst = new List<Ca_CuentasBancariasModel>();
            IEnumerable<Ca_CuentasBancarias> entities = cuentasbancarias.Get();
            foreach (Ca_CuentasBancarias item in entities)
            {
                Ca_CuentasBancariasModel model = ModelFactory.getModel<Ca_CuentasBancariasModel>(item, new Ca_CuentasBancariasModel());
                model.Ca_Bancos = ModelFactory.getModel<Ca_BancosModel>(bancos.GetByID(x => x.Id_Banco == model.Id_Banco), new Ca_BancosModel());
                model.Ca_Cuentas = ModelFactory.getModel<Ca_CuentasModel>(cuentas.GetByID(x => x.Id_Cuenta == model.Id_Cuenta), new Ca_CuentasModel());
                model.Ca_Fuentes = ModelFactory.getModel<Ca_FuentesFinModel>(fuentesfin.GetByID(x => x.Id_Fuente == model.Id_Fuente), new Ca_FuentesFinModel());
                Lst.Add(model);
            }
            byte[] PDF = GenerarPDF.GenerarPDF("CuentasBancarias", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult ImpuestosDeduccion()
        {
            List<Ca_Impuestos_Deduccion> Lst = new ImpuestosDeduccionDAL().Get().ToList();
            byte[] PDF = GenerarPDF.GenerarPDF("ImpuestosDeduccion", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }

        public ActionResult TipoDocumentos()
        {
            List<Ca_TipoDoctosModel> Lst = new List<Ca_TipoDoctosModel>();
            tipodoctos.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoDoctosModel>(x, new Ca_TipoDoctosModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("TipoDocumentos", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }

        public ActionResult DiasInhabiles()
        {
            List<CA_InHabilModel> Lst = new List<CA_InHabilModel>();
            new DiasInhabilesDAL().Get().ToList().ForEach(x =>
            {
                CA_InHabilModel model = (ModelFactory.getModel<CA_InHabilModel>(x, new CA_InHabilModel()));
                model.MesLetra = Diccionarios.Meses[model.Mes.Value];
                Lst.Add(model);
            });
            byte[] PDF = GenerarPDF.GenerarPDF("DiasInhabiles", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        public ActionResult Beneficiarios()
        {
            List<VW_Beneficiarios> entities = new VWBeneficiariosDAL().Get().OrderBy(x=>x.Nombre).ToList();
            byte[] PDF = GenerarPDF.GenerarPDF("Beneficiarios", entities, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        #endregion

        #region Conciliación
        public ActionResult TiposMovBancarios()
        {
            List<Ca_TipoMovBancariosModel> Lst = new List<Ca_TipoMovBancariosModel>();
            tipomovbancarios.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoMovBancariosModel>(x, new Ca_TipoMovBancariosModel())); });
            byte[] PDF = GenerarPDF.GenerarPDF("TiposMovBancarios", Lst, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        #endregion
    }
}
