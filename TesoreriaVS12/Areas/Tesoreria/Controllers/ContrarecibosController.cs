using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TesoreriaVS12.Areas.Tesoreria.BL;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.DAL.Vistas;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Filters;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    [AuthorizeLogin]
    public class ContrarecibosController : Controller
    {
        private MaContrarecibosDAL _dalContra;
        private TipoCompromisosDAL _dalTipoCompromisos;
        private TipoContrarecibosDAL _dalTipoContrarecibos;
        private MaCompromisosDAL _dalCompromisos;
        private DeCompromisosDAL _dalDeCompromisos;
        private DeContrarecibosDAL _dalDeContra;
        private ProceduresDAL _StoreDal { get; set; }
        private ContrarecibosBL repo { get; set; }
        private Fondos_GastosBL _dalFondosGastosBL;
        private CuentasDAL DALCuentas { get; set; }
        private CuentasBL BLCuentas { get; set; }
        private ContrarecibosBL BLContra { get; set; }
        private DeContrarecibosBL BLDeContra { get; set; }
        private Llenado _llenar { get; set; }
        private DeFacturasDAL dalFacturas { get; set; }
        private Fondos_GastosDetallesBL BLDetalleFG { get; set; }
        private DeCompromisosBL BLDeCompromisos { get; set; }
        private ParametrosDAL DALParametros { get; set; }
        private BancosDAL dalBancos { get; set; }
        private ImpuestosDeduccionDAL dalImpuestos { get; set; }
        private CompromisosBL BlCompromisos;
        private CuentasBancariasDAL DALCuentasBancarias { get; set; }
        private ContraRecibosDAL dalVWContraRecibo { get; set; }
        private DeArchivosContraRecibosDAL dalArchivos { get; set; }
        private VWBeneficiariosDAL vwbeneficiarios { get; set; }

        public DeContrarecibosDAL dalDeContra
        {
            get { return _dalDeContra; }
            set { _dalDeContra = value; }
        }
        public DeCompromisosDAL dalDeCompromisos
        {
            get { return _dalDeCompromisos; }
            set { _dalDeCompromisos = value; }
        }

        public MaCompromisosDAL dalCompromisos
        {
            get { return _dalCompromisos; }
            set { _dalCompromisos = value; }
        }

        public TipoContrarecibosDAL dalTipoContrarecibos
        {
            get { return _dalTipoContrarecibos; }
            set { _dalTipoContrarecibos = value; }
        }

        public TipoCompromisosDAL dalTipoCompromisos
        {
            get { return _dalTipoCompromisos; }
            set { _dalTipoCompromisos = value; }
        }
        protected MaContrarecibosDAL dalContra
        {
            get { return _dalContra; }
            set { _dalContra = value; }
        }

        public ContrarecibosController()
        {
            if (dalContra == null) dalContra = new MaContrarecibosDAL();
            if (dalTipoCompromisos == null) dalTipoCompromisos = new TipoCompromisosDAL();
            if (dalTipoContrarecibos == null) dalTipoContrarecibos = new TipoContrarecibosDAL();
            if (dalCompromisos == null) dalCompromisos = new MaCompromisosDAL();
            if (dalDeCompromisos == null) dalDeCompromisos = new DeCompromisosDAL();
            if (dalDeContra == null) dalDeContra = new DeContrarecibosDAL();
            if (dalFacturas == null) dalFacturas = new DeFacturasDAL();
            if (_llenar == null) _llenar = new Llenado();
            if (repo == null) repo = new ContrarecibosBL();
            if (_StoreDal == null) _StoreDal = new ProceduresDAL();
            if (_dalFondosGastosBL == null) _dalFondosGastosBL = new Fondos_GastosBL();
            if (DALCuentas == null) DALCuentas = new CuentasDAL();
            if (BLCuentas == null) BLCuentas = new CuentasBL();
            if (BLContra == null) BLContra = new ContrarecibosBL();
            if (BLDeContra == null) BLDeContra = new DeContrarecibosBL();
            if (BLDetalleFG == null) BLDetalleFG = new Fondos_GastosDetallesBL();
            if (BLDeCompromisos == null) BLDeCompromisos = new DeCompromisosBL();
            if (DALParametros == null) DALParametros = new ParametrosDAL();
            if (dalBancos == null) dalBancos = new BancosDAL();
            if (dalImpuestos == null) dalImpuestos = new ImpuestosDeduccionDAL();
            if (BlCompromisos == null) BlCompromisos = new CompromisosBL();
            if (DALCuentasBancarias == null) DALCuentasBancarias = new CuentasBancariasDAL();
            if (dalVWContraRecibo == null) dalVWContraRecibo = new ContraRecibosDAL();
            if (dalArchivos == null) dalArchivos = new DeArchivosContraRecibosDAL();
            if (vwbeneficiarios == null) vwbeneficiarios = new VWBeneficiariosDAL();
        }

        #region Contra Recibos

        public ActionResult Index()
        {
            Ma_ContrarecibosModel dataModal = new Ma_ContrarecibosModel(Diccionarios.TiposCR.ContraRecibos);
            dataModal.Botonera = repo.createBotonera(dataModal);
            dataModal.DEscripcionEstatus = Diccionarios.Estatus_CR[Diccionarios.Valores_Estatus_CR.Programado];
            dataModal.cFechas = new Control_Fechas();
            return View(dataModal);
        }

        [HttpPost]
        public ActionResult Index(byte? TipoCR, int? FolioCR)
        {
            Ma_ContrarecibosModel dataModal = new Ma_ContrarecibosModel(Diccionarios.TiposCR.ContraRecibos);
            if (FolioCR.HasValue)
            {
                dataModal = _llenar.LLenado_MaContrarecibos(TipoCR.Value, FolioCR.Value);
                dataModal.Botonera = repo.createBotonera(dataModal);
                dataModal.DEscripcionEstatus = Diccionarios.Estatus_CR[(short)dataModal.Id_EstatusCR];
                dataModal.StateCancel = repo.StateCancel(dataModal);
                dataModal.StateEdit = repo.StateEdit(dataModal);
                dataModal.cFechas = new Control_Fechas();
                //dataModal.Botonera = new List<object>() { "bNuevo","bBuscar","bCompromisos" };//, "bCancelar", "bBuscar", "bDetalles", "bRecibido", "bSalir" };
                //if (dataModal.De_Contrarecibos.Count > 0)
                //    dataModal.Botonera.AddRange(new List<object>() {"bDetalles","bReportes"});
                //if (dalFacturas.Get(x => x.Id_FolioCR == dataModal.Id_FolioCR && x.Id_TipoCR == dataModal.Id_TipoCR).Count() > 0)
                //    dataModal.Botonera.Add("bDocumentos");
                //if (repo.StateEdit(dataModal) != -1)
                //    dataModal.Botonera.Add("bEditar");
                //if (repo.StateCancel(dataModal) != -1) 
                //    dataModal.Botonera.Add("bCancelar");
                //dataModal.Botonera.Add("bSalir");
            }
            return View(dataModal);
        }
        [HttpPost]
        public ActionResult CancelacionActivos(byte? TipoCR, int? FolioCR)
        {
            Ma_ContrarecibosModel dataModal = new Ma_ContrarecibosModel(Diccionarios.TiposCR.CancelacionActivos);
            if (FolioCR.HasValue)
            {
                dataModal = _llenar.LLenado_MaContrarecibos(TipoCR.Value, FolioCR.Value);
                dataModal.Botonera = repo.createBotonera(dataModal);
                dataModal.DEscripcionEstatus = Diccionarios.Estatus_CR[(short)dataModal.Id_EstatusCR];
                dataModal.StateCancel = repo.StateCancel(dataModal);
                dataModal.StateEdit = repo.StateEdit(dataModal);
                dataModal.cFechas = new Control_Fechas();
            }
            return View("Arrendamientos", dataModal);
        }
        public ActionResult CancelacionActivos()
        {
            Ma_ContrarecibosModel dataModal = new Ma_ContrarecibosModel(Diccionarios.TiposCR.CancelacionActivos);
            dataModal.Botonera = repo.createBotonera(dataModal);
            dataModal.DEscripcionEstatus = Diccionarios.Estatus_CR[Diccionarios.Valores_Estatus_CR.Programado];
            dataModal.cFechas = new Control_Fechas();
            return View("Arrendamientos", dataModal);
        }
        [HttpPost]
        public ActionResult Honorarios(byte? TipoCR, int? FolioCR)
        {
            Ma_ContrarecibosModel dataModal = new Ma_ContrarecibosModel(Diccionarios.TiposCR.Honorarios);
            if (FolioCR.HasValue)
            {
                dataModal = _llenar.LLenado_MaContrarecibos(TipoCR.Value, FolioCR.Value);
                dataModal.Botonera = repo.createBotonera(dataModal);
                dataModal.DEscripcionEstatus = Diccionarios.Estatus_CR[(short)dataModal.Id_EstatusCR];
                dataModal.StateCancel = repo.StateCancel(dataModal);
                dataModal.StateEdit = repo.StateEdit(dataModal);
                dataModal.cFechas = new Control_Fechas();
            }
            return View("Arrendamientos", dataModal);
        }

        public ActionResult Honorarios()
        {
            Ma_ContrarecibosModel dataModal = new Ma_ContrarecibosModel(Diccionarios.TiposCR.Honorarios);
            dataModal.Botonera = repo.createBotonera(dataModal);
            dataModal.DEscripcionEstatus = Diccionarios.Estatus_CR[Diccionarios.Valores_Estatus_CR.Programado];
            dataModal.cFechas = new Control_Fechas();
            return View("Arrendamientos", dataModal);
        }
        [HttpPost]
        public ActionResult HonorariosAsimilables(byte? TipoCR, int? FolioCR)
        {
            Ma_ContrarecibosModel dataModal = new Ma_ContrarecibosModel(Diccionarios.TiposCR.Honorarios);
            if (FolioCR.HasValue)
            {
                dataModal = _llenar.LLenado_MaContrarecibos(TipoCR.Value, FolioCR.Value);
                dataModal.Botonera = repo.createBotonera(dataModal);
                dataModal.DEscripcionEstatus = Diccionarios.Estatus_CR[(short)dataModal.Id_EstatusCR];
                dataModal.StateCancel = repo.StateCancel(dataModal);
                dataModal.StateEdit = repo.StateEdit(dataModal);
                dataModal.cFechas = new Control_Fechas();
            }
            return View("Arrendamientos", dataModal);
        }

        public ActionResult HonorariosAsimilables()
        {
            Ma_ContrarecibosModel dataModal = new Ma_ContrarecibosModel(Diccionarios.TiposCR.HonorariosAsimilables);
            dataModal.Botonera = repo.createBotonera(dataModal);
            dataModal.DEscripcionEstatus = Diccionarios.Estatus_CR[Diccionarios.Valores_Estatus_CR.Programado];
            dataModal.cFechas = new Control_Fechas();


            return View("Arrendamientos", dataModal);
        }
        [HttpPost]
        public ActionResult Arrendamientos(byte? TipoCR, int? FolioCR)
        {
            Ma_ContrarecibosModel dataModal = new Ma_ContrarecibosModel(Diccionarios.TiposCR.Arrendamientos);
            if (FolioCR.HasValue)
            {
                dataModal = _llenar.LLenado_MaContrarecibos(TipoCR.Value, FolioCR.Value);
                dataModal.Botonera = repo.createBotonera(dataModal);
                dataModal.DEscripcionEstatus = Diccionarios.Estatus_CR[(short)dataModal.Id_EstatusCR];
                dataModal.StateCancel = repo.StateCancel(dataModal);
                dataModal.StateEdit = repo.StateEdit(dataModal);
                dataModal.cFechas = new Control_Fechas();
            }
            return View(dataModal);
        }
        public ActionResult Arrendamientos()
        {
            Ma_ContrarecibosModel dataModal = new Ma_ContrarecibosModel(Diccionarios.TiposCR.Arrendamientos);
            dataModal.Botonera = repo.createBotonera(dataModal);
            dataModal.DEscripcionEstatus = Diccionarios.Estatus_CR[Diccionarios.Valores_Estatus_CR.Programado];
            dataModal.cFechas = new Control_Fechas();
            return View(dataModal);
        }
        [HttpGet]
        public ActionResult AgregarImporte(int FolioCR, int TipoCR)
        {
            ViewBag.Id_TipoCR = TipoCR;
            ViewBag.FolioCR = FolioCR;
            ImporteContrarecibosModel importe = new ImporteContrarecibosModel();
            Ma_Contrarecibos contrarecibo = dalContra.GetByID(x => x.Id_FolioCR == FolioCR && x.Id_TipoCR == TipoCR);
            importe.Cuenta = contrarecibo.Id_Cuenta_AH;
            importe.Importe = contrarecibo.Importe_AH.HasValue ? contrarecibo.Importe_AH.Value : 0;
            importe.TipoMovimiento = Convert.ToByte(contrarecibo.Id_TipoMovimiento_AH.HasValue ? contrarecibo.Id_TipoMovimiento_AH.Value : 0);
            importe.CuentaDescripcion = !String.IsNullOrEmpty(importe.Cuenta) ? DALCuentas.GetByID(x => x.Id_Cuenta == importe.Cuenta).Descripcion : String.Empty;

            importe.Cuenta2 = contrarecibo.Id_Cuenta_AH2;
            importe.Importe2 = contrarecibo.Importe_AH2.HasValue ? contrarecibo.Importe_AH2.Value : 0;
            importe.CuentaDescripcion2 = !String.IsNullOrEmpty(importe.Cuenta) ? DALCuentas.GetByID(x => x.Id_Cuenta == importe.Cuenta2).Descripcion : String.Empty;


            importe.Cuenta3 = contrarecibo.Id_Cuenta_AH3;
            importe.Importe3 = contrarecibo.Importe_AH3.HasValue ? contrarecibo.Importe_AH3.Value : 0;
            importe.CuentaDescripcion3 = !String.IsNullOrEmpty(importe.Cuenta3) ? DALCuentas.GetByID(x => x.Id_Cuenta == importe.Cuenta3).Descripcion : String.Empty;


            importe.Cuenta4 = contrarecibo.Id_Cuenta_AH4;
            importe.Importe4 = contrarecibo.Importe_AH4.HasValue ? contrarecibo.Importe_AH4.Value : 0;
            importe.CuentaDescripcion4 = !String.IsNullOrEmpty(importe.Cuenta4) ? DALCuentas.GetByID(x => x.Id_Cuenta == importe.Cuenta).Descripcion : String.Empty;
            return View(importe);
        }
        [HttpPost]
        public ActionResult AgregarImporte(ImporteContrarecibosModel model)
        {
            try
            {
                Ma_Contrarecibos contrarecibo = dalContra.GetByID(x => x.Id_TipoCR == model.IdTipoCR && x.Id_FolioCR == model.FolioCR);
                decimal? importeCh = 0;
                decimal importe = 0;
                importe = model.Importe + model.Importe2 + model.Importe3 + model.Importe4;

                if (!string.IsNullOrEmpty(model.Cuenta2))
                {
                    if (model.Importe2 <= 0)
                        return Json(new { Exito = false, Mensaje = String.Format("El importe de la cuenta 2 no debe estar vacio.") });


                }
                if (!string.IsNullOrEmpty(model.Cuenta3))
                {
                    if (model.Importe3 <= 0)
                        return Json(new { Exito = false, Mensaje = String.Format("El importe de la cuenta 3 no debe estar vacio.") });


                }
                if (!string.IsNullOrEmpty(model.Cuenta4))
                {
                    if (model.Importe4 <= 0)
                        return Json(new { Exito = false, Mensaje = String.Format("El importe de la cuenta 4 no debe estar vacio.") });


                }
                if (model.TipoMovimiento.HasValue) //Es un honorario asimilable (es el único que traerá valor en TipoMovimiento
                {
                    if (model.TipoMovimiento.Value == 2)
                    {
                        if (!(contrarecibo.Cargos > importe))
                            return Json(new { Exito = false, Mensaje = String.Format("Los cargos {0} del contrarecibo debe ser mayor que el importe del cheque {1}.", contrarecibo.Cargos, importe) });
                    }
                    else//Si es cargo el importe del cheque debe ser mayor que la suma de los detalles
                    {
                        if (!(importe > contrarecibo.Cargos))
                            return Json(new { Exito = false, Mensaje = String.Format("El importe del cheque {0} debe ser mayor que los cargos del contrarecibo {1}.", importe, contrarecibo.Cargos) });
                    }
                    importeCh = model.TipoMovimiento == 1 ? contrarecibo.Cargos + importe : contrarecibo.Cargos - importe;
                }
                else
                {
                    if (importe > contrarecibo.Cargos)
                        return Json(new { Exito = false, Mensaje = String.Format("El importe {0:C2} no puede ser mayor que los cargos {1:C2}", importe, contrarecibo.Cargos) });
                }
                contrarecibo.Id_Cuenta_AH = model.Cuenta;
                contrarecibo.Importe_AH = model.Importe;
                contrarecibo.Importe_CH = importeCh;

                //

                contrarecibo.Id_Cuenta_AH2 = model.Cuenta2;
                contrarecibo.Importe_AH2 = model.Importe2;

                contrarecibo.Id_Cuenta_AH3 = model.Cuenta3;
                contrarecibo.Importe_AH3 = model.Importe3;

                contrarecibo.Id_Cuenta_AH4 = model.Cuenta4;
                contrarecibo.Importe_AH4 = model.Importe4;

                //
                contrarecibo.Id_TipoMovimiento_AH = model.TipoMovimiento;
                contrarecibo.Id_TipoMovimiento_AH2 = model.TipoMovimiento;
                contrarecibo.Id_TipoMovimiento_AH3 = model.TipoMovimiento;
                contrarecibo.Id_TipoMovimiento_AH4 = model.TipoMovimiento;

                dalContra.Update(contrarecibo);
                dalContra.Save();
                Ma_ContrarecibosModel modelContrarecibo = new Ma_ContrarecibosModel((byte)model.IdTipoCR);
                modelContrarecibo = _llenar.LLenado_MaContrarecibos((byte)model.IdTipoCR, model.FolioCR);
                modelContrarecibo.Botonera = repo.createBotonera(modelContrarecibo);
                modelContrarecibo.DEscripcionEstatus = Diccionarios.Estatus_CR[(short)modelContrarecibo.Id_EstatusCR];
                modelContrarecibo.StateCancel = repo.StateCancel(modelContrarecibo);
                modelContrarecibo.StateEdit = repo.StateEdit(modelContrarecibo);
                modelContrarecibo.cFechas = new Control_Fechas();

                modelContrarecibo.Ca_Cuentas_FR.Descripcion2 = modelContrarecibo.Descripcion2 = !String.IsNullOrEmpty(modelContrarecibo.Id_Cuenta_AH2) ? DALCuentas.GetByID(x => x.Id_Cuenta == modelContrarecibo.Id_Cuenta_AH2).Descripcion : String.Empty;
                modelContrarecibo.Ca_Cuentas_FR.Descripcion3 = modelContrarecibo.Descripcion3 = !String.IsNullOrEmpty(modelContrarecibo.Id_Cuenta_AH3) ? DALCuentas.GetByID(x => x.Id_Cuenta == modelContrarecibo.Id_Cuenta_AH3).Descripcion : String.Empty;
                modelContrarecibo.Ca_Cuentas_FR.Descripcion4 = modelContrarecibo.Descripcion4 = !String.IsNullOrEmpty(modelContrarecibo.Id_Cuenta_AH4) ? DALCuentas.GetByID(x => x.Id_Cuenta == modelContrarecibo.Id_Cuenta_AH4).Descripcion : String.Empty;
                //   modelContrarecibo.Descripcion4 = !String.IsNullOrEmpty(modelContrarecibo.Id_Cuenta_AH2) ? DALCuentas.GetByID(x => x.Id_Cuenta == modelContrarecibo.Id_Cuenta_AH2).Descripcion : String.Empty;

                return Json(new { Exito = true, Mensaje = "Datos agregados con éxito", Registro = modelContrarecibo });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }


        public ActionResult BuscarContrarecibo(byte Tipo)
        {
            return View(new BusquedaContrarecibos(Tipo));
        }

        [HttpPost]
        public JsonResult GetContrarecibo(Int32 IdFolio, Byte IdTipoCR)
        {
            try
            {
                Llenado llenar = new Llenado();
                Ma_ContrarecibosModel dataModal = llenar.LLenado_MaContrarecibos(IdTipoCR, IdFolio);
                Int32 hasDocuments = dalFacturas.Get(x => x.Id_FolioCR == dataModal.Id_FolioCR && x.Id_TipoCR == dataModal.Id_TipoCR).Count();
                dataModal.Botonera = repo.createBotonera(dataModal);
                dataModal.DEscripcionEstatus = Diccionarios.Estatus_CR[(short)dataModal.Id_EstatusCR];
                if (IdTipoCR == Diccionarios.TiposCR.ContraRecibos || IdTipoCR == Diccionarios.TiposCR.Arrendamientos || IdTipoCR == Diccionarios.TiposCR.Honorarios || IdTipoCR == Diccionarios.TiposCR.CancelacionActivos || IdTipoCR == Diccionarios.TiposCR.HonorariosAsimilables)
                {
                    dataModal.StateEdit = repo.StateEdit(dataModal);
                    dataModal.StateCancel = repo.StateCancel(dataModal);
                    return Json(new { Exito = true, Mensaje = "OK", Registro = dataModal });
                }
                if (IdTipoCR == Diccionarios.TiposCR.GastosComprobar || IdTipoCR == Diccionarios.TiposCR.FondosRevolventes)
                {
                    Ma_ContrarecibosFGModel ma = Llenado_FG(IdTipoCR, IdFolio);
                    List<object> Botones = new List<object>();
                    _dalFondosGastosBL.CreateBotonera(ref Botones, IdTipoCR, IdFolio);
                    return Json(new { Exito = true, Mensaje = "OK", Registro = ma, Botonera = Botones });
                }
                if (IdTipoCR == Diccionarios.TiposCR.CancelacionPasivos || IdTipoCR == Diccionarios.TiposCR.AnticiposPrestamos)
                {
                    Ma_ContrarecibosCPModel model = ModelFactory.getModel<Ma_ContrarecibosCPModel>(dataModal, new Ma_ContrarecibosCPModel());
                    model.Botonera = repo.createBotoneraCP(model);
                    model.Descripcion_EstatusCR = Diccionarios.Estatus_CR[(short)model.Id_EstatusCR];
                    return Json(new { Exito = true, Mensaje = "OK", Registro = model });
                }
                //if (IdTipoCR == Diccionarios.TiposCR.EgresosNoPresupuestales)
                //{
                //    Ma_ContrarecibosCPModel model = ModelFactory.getModel<Ma_ContrarecibosCPModel>(dataModal, new Ma_ContrarecibosCPModel());
                //    model.Botonera = repo.createBotoneraCP(model);
                //    model.Descripcion_EstatusCR = Diccionarios.Estatus_CR[(short)model.Id_EstatusCR];
                //    return Json(new { Exito = true, Mensaje = "OK", Registro = model });
                //}
                if (IdTipoCR == Diccionarios.TiposCR.EgresosNoPresupuestales)
                {
                    Ma_ContrarecibosCPModel model = ModelFactory.getModel<Ma_ContrarecibosCPModel>(dataModal, new Ma_ContrarecibosCPModel());
                    model.Botonera = repo.createBotoneraNP(model);
                    model.Descripcion_EstatusCR = Diccionarios.Estatus_CR[(short)model.Id_EstatusCR];
                    return Json(new { Exito = true, Mensaje = "OK", Registro = model });
                }

                return Json(new { Exito = true });

            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }


        public ActionResult tblContrarecibos(BusquedaContrarecibos filtros)
        {
            List<Ma_ContrarecibosModel> dataModal = new List<Ma_ContrarecibosModel>();
            IEnumerable<VW_Contrarecibos> tbl = dalVWContraRecibo.Get(x => (
                (filtros.FolioContrarecibo.HasValue ? x.Id_FolioCR == filtros.FolioContrarecibo.Value : x.Id_FolioCR != null) &&
                (filtros.NoCheque.HasValue ? x.No_Cheque == filtros.NoCheque : x.Id_FolioCR != null) &&
                (x.Id_TipoCR == filtros.TipoCR) && (!string.IsNullOrEmpty(filtros.NombreBeneficiario) ? x.Nombre.Contains(filtros.NombreBeneficiario) : x.Id_FolioCR != null)
               ) && (filtros.NoCompromiso.HasValue ? x.Id_FolioCompromiso == filtros.NoCompromiso : x.Id_FolioCR != null) && (filtros.NoRequisicion.HasValue ? x.No_Requisicion == filtros.NoRequisicion : x.Id_FolioCR != null)
               && (filtros.NoOrdenCompra.HasValue ? x.No_Adquisicion == filtros.NoOrdenCompra : x.Id_FolioCR != null));
            try
            {
                foreach (VW_Contrarecibos item in tbl)
                {
                    Ma_ContrarecibosModel aux = ModelFactory.getModel<Ma_ContrarecibosModel>(item, new Ma_ContrarecibosModel());
                    aux.TipoCRstr = dalTipoContrarecibos.GetByID(x => x.Id_TipoCR == aux.Id_TipoCR).Descripcion;
                    dataModal.Add(aux);
                }
            }
            catch (Exception ex)
            {

            }
            if (dataModal.Count > 0)
                ViewBag.TipoCRStr = dataModal.FirstOrDefault().TipoCRstr;
            else
                ViewBag.TipoCRStr = "";
            return View(dataModal);
        }

        [HttpPost]
        public ActionResult GuardarContrarecibo(Ma_ContrarecibosModel dataModel)
        {

            try
            {
                if (!BlCompromisos.isClosed(dataModel.FechaCR.Value))
                {
                    UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                    ContrarecibosBL foliador = new ContrarecibosBL();
                    if (dataModel.Id_FolioCR != 0)
                    {
                        dataModel.Fecha_Act = DateTime.Now;
                        dataModel.Usuario_Act = (Int16)appUsuario.IdUsuario;
                        //dataModel.Usu_CR = (Int16)appUsuario.IdUsuario;
                        dalContra.Update(EntityFactory.getEntity<Ma_Contrarecibos>(dataModel, new Ma_Contrarecibos()));
                        dalContra.Save();
                        dataModel.Botonera = repo.createBotonera(dataModel);
                        dataModel.StateEdit = repo.StateEdit(dataModel);
                        dataModel.StateCancel = repo.StateCancel(dataModel);
                        return Json(new { Exito = true, Mensaje = "OK", Registro = dataModel });
                    }
                    else
                    {
                        Object thisLock = new Object();
                        lock (thisLock)
                        {
                            dataModel.Fecha_Act = DateTime.Now;
                            dataModel.Usuario_Act = (Int16)appUsuario.IdUsuario;
                            dataModel.Id_EstatusCR = 1;
                            dataModel.Usu_CR = appUsuario.NombreCompleto;
                            dataModel.Id_FolioCR = (Int32)foliador.getMaContrarecibos(dataModel.Id_TipoCR);
                            dalContra.Insert(EntityFactory.getEntity<Ma_Contrarecibos>(dataModel, new Ma_Contrarecibos()));
                            dalContra.Save();
                            foliador.setMaContrarecibos(dataModel.Id_TipoCR);
                            dataModel.Botonera = repo.createBotonera(dataModel);
                            dataModel.StateEdit = repo.StateEdit(dataModel);
                            dataModel.StateCancel = repo.StateCancel(dataModel);
                            return Json(new { Exito = true, Mensaje = "OK", Registro = dataModel });
                        }
                    }
                }
                else
                    return Json(new { Exito = false, Mensaje = "El mes está cerrado, elija otra fecha." });

            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult BuscarCompromisos(Int32 FolioCR, Byte TipoCR, DateTime FContra)
        {
            ViewBag.TipoCr = TipoCR;
            ViewBag.FolioCr = FolioCR;
            Ma_Compromisos comp = dalCompromisos.GetByID(x => x.Id_TipoCR == TipoCR && x.Id_FolioCR == FolioCR);
            short idTipoCompromiso = 0;
            switch (TipoCR)
            {
                case Diccionarios.TiposCR.Arrendamientos:
                    idTipoCompromiso = 12;
                    break;
                case Diccionarios.TiposCR.CancelacionActivos:
                    idTipoCompromiso = 54;
                    break;
                case Diccionarios.TiposCR.Honorarios:
                    idTipoCompromiso = 29;
                    break;
                case Diccionarios.TiposCR.HonorariosAsimilables:
                    idTipoCompromiso = 59;
                    break;
            }
            if (comp == null)
            {
                //List<Ma_Compromisos> ComprimisosTbl = dalCompromisos.Get(x => x.Id_Beneficiario == IdBene && x.Id_TipoCR == null && x.Id_FolioCR == null && (x.Cargos != null || x.Cargos > 0) && x.Estatus == 6 && (x.Fecha_Devengado <= FContra && x.Fecha_Devengado != null) ).ToList();
                List<Ma_Compromisos> ComprimisosTbl = new List<Ma_Compromisos>();
                switch (TipoCR)
                {
                    case Diccionarios.TiposCR.Arrendamientos:
                    case Diccionarios.TiposCR.CancelacionActivos:
                    case Diccionarios.TiposCR.Honorarios:
                    case Diccionarios.TiposCR.HonorariosAsimilables:
                        ComprimisosTbl = dalCompromisos.Get(x => x.Id_TipoCompromiso == idTipoCompromiso && x.Id_TipoCR == null && x.Id_FolioCR == null && (x.Cargos != null || x.Cargos > 0) && x.Estatus == 6 && (x.Fecha_Devengado <= FContra && x.Fecha_Devengado != null)).ToList();
                        break;
                    default:
                        ComprimisosTbl = dalCompromisos.Get(x => x.Id_TipoCR == null && x.Id_FolioCR == null && (x.Cargos != null || x.Cargos > 0) && x.Estatus == 6 && (x.Fecha_Devengado <= FContra && x.Fecha_Devengado != null)).ToList();
                        break;
                }
                List<Ma_CompromisosModel> dataModel = new List<Ma_CompromisosModel>();
                if (ComprimisosTbl.Count() > 0)
                {
                    ComprimisosTbl.ForEach(f => { dataModel.Add(_llenar.LLenado_MaCompromisos(f.Id_TipoCompromiso, f.Id_FolioCompromiso)); });
                    return View(dataModel);
                }
                return Json(new { Exito = false, Mensaje = "No tiene compromisos" });
            }
            else
            {
                ReturnMaster regreso = new ReturnMaster();
                regreso.Accion = "Index";
                regreso.Controlador = "Contrarecibos";
                regreso.Parametros = new List<Campos>();
                regreso.Parametros.Add(new Campos("TipoCR", TipoCR.ToString()));
                regreso.Parametros.Add(new Campos("FolioCR", FolioCR.ToString()));
                return Json(new { Exito = true, Url = "frmCompromisos", Parametros = new { TipoCompromiso = comp.Id_TipoCompromiso, FolioCompromiso = comp.Id_FolioCompromiso, args = regreso } });
                //return RedirectToAction("OrdenCompra", "Compromisos", new { TipoCompromiso = comp.Id_TipoCompromiso, FolioCompromiso = comp.Id_FolioCompromiso });
            }
        }

        [HttpPost]
        public ActionResult GuardarComprimisoCR(Int32 FolioCompromiso, Byte TipoCR, Int32 FolioCR, Byte TipoCompromiso)
        {
            try
            {
                Ma_Compromisos compromiso = dalCompromisos.GetByID(x => x.Id_TipoCompromiso == TipoCompromiso && x.Id_FolioCompromiso == FolioCompromiso);
                compromiso.Id_FolioCR = FolioCR;
                compromiso.Id_TipoCR = TipoCR;
                compromiso.Estatus = Diccionarios.ValorEstatus.ASIGANADO_CR;
                Ma_Contrarecibos contra = dalContra.GetByID(x => x.Id_TipoCR == TipoCR && x.Id_FolioCR == FolioCR);
                contra.Cargos = compromiso.Cargos;
                contra.Abonos = compromiso.Abonos;
                contra.Id_Beneficiario = compromiso.Id_Beneficiario;
                contra.Id_TipoCompromiso = compromiso.Id_TipoCompromiso;
                contra.Id_FolioPolizaCR = compromiso.Id_FolioPoliza;
                contra.Id_MesPolizaCR = compromiso.Id_MesPoliza;
                contra.Id_Fuente = compromiso.De_Compromisos.Where(x => x.Id_Movimiento == Diccionarios.ValorMovimientos.CARGO).FirstOrDefault().Id_ClavePresupuesto.Substring(26, 4);
                //Ca_CuentasBancarias cuetnaBancaria = DALCuentasBancarias.GetByID(x => x.Id_Fuente == contra.Id_Fuente);
                //if (cuetnaBancaria != null)
                //    contra.Id_CtaBancaria = cuetnaBancaria.Id_CtaBancaria;
                contra.FechaVen = Convert.ToDateTime(_StoreDal.PA_FechaCompromiso((short)compromiso.Id_TipoCompromiso, contra.FechaCR.Value.ToShortDateString()));
                dalCompromisos.Update(compromiso);
                dalContra.Update(contra);
                dalCompromisos.Save();
                dalContra.Save();
                IEnumerable<De_Compromisos> deCompromisos = dalDeCompromisos.Get(x => x.Id_FolioCompromiso == FolioCompromiso && x.Id_TipoCompromiso == TipoCompromiso);
                //     short Registro = dalDeContra.Get().Max(x => x.Id_Registro) == null ? dalDeContra.Get().Max(x => x.Id_Registro) : 0;



                foreach (De_Compromisos item in deCompromisos)
                {
                    De_Contrarecibos aux = EntityFactory.getEntity<De_Contrarecibos>(item, new De_Contrarecibos());
                    aux.Id_TipoCR = TipoCR;
                    aux.Id_FolioCR = FolioCR;
                    //      aux.Id_Registro = 
                    if (dalDeContra.Get(x => x.Id_TipoCR == TipoCR && x.Id_FolioCR == FolioCR).Count() > 0)
                        aux.Id_Registro = Convert.ToInt16(dalDeContra.Get(x => x.Id_TipoCR == TipoCR && x.Id_FolioCR == FolioCR).Max(x => x.Id_Registro) + 1);
                    else
                        aux.Id_Registro = 1;

                    dalDeContra.Insert(aux);
                    dalDeContra.Save();
                }

                return Json(new { Exito = true, Mensaje = "OK", Registro = _llenar.LLenado_MaContrarecibos(contra.Id_TipoCR, contra.Id_FolioCR) });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public JsonResult NuevoContrarecibo()
        {
            Ma_ContrarecibosModel dataModel = new Ma_ContrarecibosModel(Diccionarios.TiposCR.ContraRecibos);
            dataModel.cFechas = new Control_Fechas();
            return Json(new { Registro = dataModel, fActual = DateTime.Now.ToShortDateString(), fMin = dataModel.cFechas.Fecha_Min.ToShortDateString(), fMax = dataModel.cFechas.Fecha_Max.ToShortDateString() });
        }
        [HttpPost]
        public JsonResult NuevoArrendamiento(byte idTipoCR)
        {
            //Diccionarios.TiposCR.Arrendamientos
            Ma_ContrarecibosModel dataModel = new Ma_ContrarecibosModel(idTipoCR);
            dataModel.cFechas = new Control_Fechas();
            return Json(new { Registro = dataModel, fActual = DateTime.Now.ToShortDateString(), fMin = dataModel.cFechas.Fecha_Min.ToShortDateString(), fMax = dataModel.cFechas.Fecha_Max.ToShortDateString() });
        }


        public ActionResult DetallesContrarecibos(Int32 FolioCR, Byte TipoCr, Byte? IdRegistro)
        {
            Llenado llenar = new Llenado();
            List<De_ContrarecibosModel> lstDeContra = new List<De_ContrarecibosModel>();
            dalDeContra.Get(x => x.Id_TipoCR == TipoCr && x.Id_FolioCR == FolioCR).ToList().ForEach(item => { lstDeContra.Add(llenar.LLenado_DeContrarecibos(item.Id_TipoCR, item.Id_FolioCR, item.Id_Registro)); });
            return View(lstDeContra);
        }

        public ActionResult GetDetalleContrarecibo(Int32 FolioCR, Byte TipoCR, Int16 IdRegistro)
        {
            Llenado llenar = new Llenado();
            De_ContrarecibosModel dataModel = llenar.LLenado_DeContrarecibos(TipoCR, FolioCR, IdRegistro);
            if (dataModel.Ma_PresupuestoEg == null)
                return PartialView("ParcialPresupuesto", new MA_PresupuestoEgModel());
            return PartialView("ParcialPresupuesto", dataModel.Ma_PresupuestoEg);
            //return Json(new { Registro = dataModel.Ma_PresupuestoEg, Detalle = dataModel });
        }

        [HttpPost]
        public ActionResult GetDetalleContrareciboJson(Int32 FolioCR, Byte TipoCR, Int16 IdRegistro)
        {
            try
            {
                Llenado llenar = new Llenado();
                De_ContrarecibosModel dataModel = llenar.LLenado_DeContrarecibos(TipoCR, FolioCR, IdRegistro);
                return Json(new { Exito = true, Mensaje = "OK", Registro = dataModel });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }


        public ActionResult Documentos(Byte tipo, Int32 folio)
        {
            De_CR_Facturas factura = dalFacturas.Get(x => x.Id_TipoCR == tipo && x.Id_FolioCR == folio).FirstOrDefault();
            De_FacturasModel dataModel = new De_FacturasModel();
            TipoDoctosDAL dalDoctos = new TipoDoctosDAL();
            if (factura != null)
            {
                dataModel = _llenar.LLenado_DeFacturas(factura.Id_FolioCR, factura.Id_TipoCR, factura.Id_Factura, factura.Id_Proveedor); //ModelFactory.getModel<De_FacturasModel😠factura,new De_FacturasModel());
                dataModel.ListaId_TipoDocto = new SelectList(dalDoctos.Get(), "Id_Tipodocto", "Descripcion", dataModel.Id_TipoDocto);
                dataModel.ListaId_Impuesto = new SelectList(dalImpuestos.Get(x => x.Id_Tipo_ImpDed == 1), "Id_ImpDed", "Descripcion", dataModel.Id_Impuesto);
                dataModel.ListaId_Deduccion = new SelectList(dalImpuestos.Get(x => x.Id_Tipo_ImpDed == 2), "Id_ImpDed", "Descripcion", dataModel.Id_Deduccion);
            }
            dataModel.ListaId_TipoDocto = new SelectList(dalDoctos.Get(), "Id_Tipodocto", "Descripcion");
            dataModel.ListaId_Impuesto = new SelectList(dalImpuestos.Get(x => x.Id_Tipo_ImpDed == 1), "Id_ImpDed", "Descripcion");
            dataModel.ListaId_Deduccion = new SelectList(dalImpuestos.Get(x => x.Id_Tipo_ImpDed == 2), "Id_ImpDed", "Descripcion");
            Ma_ContrarecibosModel contraRecibo = _llenar.LLenado_MaContrarecibos(tipo, folio);
            ViewBag.NombreCompleto = contraRecibo.Ca_Beneficiarios.NombreCompleto;
            ViewBag.TipoContraRecibo = contraRecibo.Ca_TipoContrarecibos.Descripcion;
            ViewBag.Folio = folio;
            ViewBag.Tipo = tipo;
            dataModel.Botonera = repo.createBotoneraDocumentos(ModelFactory.getModel<Ma_Contrarecibos>(contraRecibo, new Ma_Contrarecibos()));
            return View(dataModel);
        }

        public ActionResult tblDocumentos(Byte tipo, Int32 folio)
        {
            List<De_FacturasModel> facturas = new List<De_FacturasModel>();
            dalFacturas.Get(x => x.Id_TipoCR == tipo && x.Id_FolioCR == folio).ToList().ForEach(x => { facturas.Add(_llenar.LLenado_DeFacturas(x.Id_FolioCR, x.Id_TipoCR, x.Id_Factura, x.Id_Proveedor)); });
            return View(facturas);
        }

        [HttpPost]
        public ActionResult GuardarDocumentos(De_FacturasModel dataModel, FormCollection frm)
        {
            try
            {
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                if (!BLContra.existFolioFactura(dataModel))
                {
                    if (frm["Editado"] == "1")
                    {
                        De_CR_Facturas fact = dalFacturas.GetByID(x => x.Id_Factura == dataModel.Id_Factura && x.Id_FolioCR == dataModel.Id_FolioCR && x.Id_TipoCR == dataModel.Id_TipoCR && x.Id_Proveedor == dataModel.Id_Proveedor);
                        fact.Fecha_Act = DateTime.Now;
                        fact.Usu_Act = (short)appUsuario.IdUsuario;
                        UpdateModel<De_CR_Facturas>(fact);
                        dalFacturas.Update(fact);//EntityFactory.getEntity<De_CR_Facturas>(dataModel, new De_CR_Facturas()));
                        dalFacturas.Save();
                        return Json(new { Exito = true, Mensaje = "OK", Registro = dataModel, Editado = true });
                    }
                    else
                    {
                        dataModel.Fecha_Act = DateTime.Now;
                        dataModel.No_Comprobacion = 1;
                        dataModel.Usu_Act = (short)appUsuario.IdUsuario;
                        De_CR_Facturas entidad = EntityFactory.getEntity<De_CR_Facturas>(dataModel, new De_CR_Facturas());
                        dalFacturas.Insert(entidad);
                        dalFacturas.Save();
                        dataModel.Id_Factura = entidad.Id_Factura;
                        return Json(new { Exito = true, Mensaje = "OK", Registro = dataModel, Editado = false });
                    }
                }
                else
                    return Json(new { Exito = false, Mensaje = "El folio de esta factura ya ha sido utilizado anteriormente. Revise" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        public JsonResult EliminarDocumento(Int32 FolioCR, Byte TipoCR, Int32 Factura, Int32 Proveedor)
        {
            try
            {
                dalFacturas.Delete(x => x.Id_Factura == Factura && x.Id_FolioCR == FolioCR && x.Id_TipoCR == TipoCR && x.Id_Proveedor == Proveedor);
                dalFacturas.Save();
                return Json(new { Exito = true, Mensaje = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDocumento(Int32 FolioCR, Byte TipoCR, Int32 Factura, Int32 Proveedor)
        {
            try
            {
                De_FacturasModel factura = _llenar.LLenado_DeFacturas(FolioCR, TipoCR, Factura, Proveedor);
                factura.Botonera = repo.createBotoneraDocumentos(dalContra.GetByID(x => x.Id_FolioCR == FolioCR && x.Id_TipoCR == TipoCR));

                //De_FacturasModel factura = ModelFactory.getModel<De_FacturasModel>(dalFacturas.GetByID(x => x.Id_Factura == Factura && x.Id_FolioCR == FolioCR && x.Id_TipoCR == TipoCR && x.Id_Proveedor == Proveedor && x.No_Comprobacion == 1),new De_FacturasModel());
                return Json(new { Exito = true, Mensaje = "OK", Registro = factura });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult EditarDocumento(De_FacturasModel dataModel)
        {
            try
            {

                return Json(new { Exito = true, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        public ActionResult CancelarContrarecibo(Int32 FolioCR, Byte TipoCR)
        {
            return View(_llenar.LLenado_MaContrarecibos(TipoCR, FolioCR));
        }

        [HttpPost]
        public ActionResult CancelarContraRecibo(Int32 Id_FolioCR, Byte Id_TipoCR, Byte Opcion, DateTime Fecha)
        {
            try
            {
                Ma_Contrarecibos contrarecibo = dalContra.GetByID(x => x.Id_FolioCR == Id_FolioCR && x.Id_TipoCR == Id_TipoCR);
                Ma_Compromisos compromiso = dalCompromisos.GetByID(x => x.Id_FolioCR == Id_FolioCR && x.Id_TipoCR == Id_TipoCR);
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();

                byte mes = 0; int folio = 0;
                if (Opcion == 1)
                {
                    if (compromiso != null)
                    {
                        compromiso.Id_TipoCR = null;
                        compromiso.Id_FolioCR = null;
                        compromiso.Estatus = Diccionarios.ValorEstatus.RECIBIDO;
                    }
                    contrarecibo.Cargos = 0;
                    contrarecibo.Abonos = 0;
                    contrarecibo.Id_FolioPolizaCR = null;
                    contrarecibo.Id_MesPolizaCR = null;
                }
                else
                {
                    if (compromiso != null)
                    {
                        if (compromiso.Adquisicion == true)
                        {
                            RESTConsume ws = new RESTConsume();
                            string msnparametros = DALParametros.GetByID(x => x.Nombre == "MensajeADQ").Descripcion;
                            ResponseCancelacion responsews = ws.CancelaRequisicion(compromiso.No_Requisicion.Value, msnparametros);
                            if (responsews.error)
                                return Json(new { Exito = false, Mensaje = "Error al notificar a adquisiciones de la cancelacion" });
                        }
                        compromiso.Fecha_CancelaDevengo = Fecha;
                        compromiso.Fecha_CancelaCompro = Fecha;
                        _StoreDal.Pa_Genera_PolizaOrden_Devengado_Cancela(compromiso.Id_TipoCompromiso, compromiso.Id_FolioCompromiso, Fecha, (short)appUsuario.IdUsuario, ref mes, ref folio);
                        compromiso.Id_MesPO_Devengado_C = mes;
                        compromiso.Id_FolioPO_Devengado_C = folio;
                        _StoreDal.Pa_Cancelacion_Poliza_Diario_Devengo(compromiso.Id_TipoCompromiso, compromiso.Id_FolioCompromiso, Fecha, (short)appUsuario.IdUsuario, ref mes, ref folio);
                        compromiso.Id_MesPoliza_C = mes;
                        compromiso.Id_FolioPoliza_C = folio;
                        _StoreDal.Pa_Genera_PolizaOrden_Comprometido_Cancela(compromiso.Id_TipoCompromiso, compromiso.Id_FolioCompromiso, Fecha, (short)appUsuario.IdUsuario, ref mes, ref folio);
                        compromiso.Id_MesPO_Comprometido_C = mes;
                        compromiso.Id_FolioPO_Comprometido_C = folio;
                        compromiso.Estatus = Diccionarios.ValorEstatus.CANCELADO;
                    }
                }
                dalCompromisos.Save();

                contrarecibo.FechaCancelacionCR = Fecha;
                contrarecibo.Id_EstatusCR = Diccionarios.Valores_Estatus_CR.Cancelado;
                if (contrarecibo.Impreso_CR == true)
                {
                    _StoreDal.Pa_Genera_PolizaOrden_Ejercido_Compromiso_Cancela(contrarecibo.Id_TipoCR, contrarecibo.Id_FolioCR, Fecha, (short)appUsuario.IdUsuario, ref mes, ref folio);
                    contrarecibo.Id_FolioPO_Ejercido_C = folio;
                    contrarecibo.Id_MesPO_Ejercido_C = mes;
                }
                if (Opcion == 2 && compromiso != null)
                {
                    contrarecibo.Id_MesPolizaCR_C = compromiso.Id_MesPoliza_C;
                    contrarecibo.Id_FolioPolizaCR_C = compromiso.Id_FolioPoliza_C;
                }
                dalContra.Save();
                foreach (var item in dalFacturas.Get(x => x.Id_FolioCR == contrarecibo.Id_FolioCR && x.Id_TipoCR == contrarecibo.Id_TipoCR))
                {
                    dalFacturas.Delete(x => x.Id_TipoCR == item.Id_TipoCR && x.Id_Proveedor == item.Id_Proveedor && x.Id_FolioCR == item.Id_FolioCR && x.Id_Factura == item.Id_Factura);
                }

                dalFacturas.Save();
                return Json(new { Exito = true, Mensaje = "OK", Registro = _llenar.LLenado_MaContrarecibos(contrarecibo.Id_TipoCR, contrarecibo.Id_FolioCR) });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult ImprimirContraRecibo(Byte TipoCR, Int32 FolioCR)
        {
            try
            {
                byte mes = 0; int folio = 0;
                Ma_Contrarecibos contrarecibo = dalContra.GetByID(x => x.Id_FolioCR == FolioCR && x.Id_TipoCR == TipoCR);
                if (contrarecibo.Impreso_CR != true)
                {
                    UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                    contrarecibo.Impreso_CR = true;
                    contrarecibo.Usuario_Act = (short)appUsuario.IdUsuario;
                    contrarecibo.Fecha_Act = DateTime.Now;
                    switch (contrarecibo.Id_TipoCR)
                    {
                        case Diccionarios.TiposCR.ContraRecibos:
                        case Diccionarios.TiposCR.Honorarios:
                        case Diccionarios.TiposCR.CancelacionActivos:
                            _StoreDal.Pa_Genera_PoilizaOrden_Ejercido_Compromiso(TipoCR, FolioCR, (short)appUsuario.IdUsuario, ref mes, ref folio);
                            contrarecibo.Id_MesPO_Ejercido = mes;
                            contrarecibo.Id_FolioPO_Ejercido = folio;
                            break;
                    }
                    dalContra.Save();
                    Ma_ContrarecibosModel model = _llenar.LLenado_MaContrarecibos(TipoCR, FolioCR);
                    return Json(new { Exito = true, Registro = model, Mensaje = "OK" });
                }
                return Json(new { Exito = true, Mensaje = "OK" });

            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        public ActionResult Reporte_ContraRecibo(Byte TipoCR, Int32 FolioCR)
        {
            Ma_ContrarecibosModel dataModal = _llenar.LLenado_MaContrarecibos(TipoCR, FolioCR);
            dataModal.De_Documentos = new List<De_FacturasModel>();
            dalFacturas.Get(x => x.Id_FolioCR == FolioCR && x.Id_TipoCR == TipoCR).ToList().ForEach(x => { dataModal.De_Documentos.Add(_llenar.LLenado_DeFacturas(x.Id_FolioCR, x.Id_TipoCR, x.Id_Factura, x.Id_Proveedor)); });
            //dalFacturas.Get(x=> x.Id_FolioCR == FolioCR && x.Id_TipoCR == TipoCR).ToList().ForEach(x=> { dataModal.De_Documentos.Add(_llenar.LLenado_DeFacturas(x.Id_FolioCR,x.Id_TipoCR,x.Id_Factura,x.No_Comprobacion,x.Id_Proveedor)); }); //Esto lo hizo julio pero no se que pedo
            ConvertHtmlToString pdf = new ConvertHtmlToString();
            return File(pdf.GenerarPDF_Blanco("Reporte_ContraRecibo", dataModal, this.ControllerContext), "application/pdf");
        }

        [HttpPost]
        public ActionResult tblArchivos(byte TipoCR, int FolioCR)
        {
            List<DE_ContrarecibosArchivosModel> files = new List<DE_ContrarecibosArchivosModel>();
            dalArchivos.Get(x => x.Id_FolioCR == FolioCR && x.Id_TipoCR == TipoCR).ToList().ForEach(f => { files.Add(ModelFactory.getModel<DE_ContrarecibosArchivosModel>(f, new DE_ContrarecibosArchivosModel())); });
            return View(files);
        }

        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, byte fTipoCR, int fFolioCR)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    Uri url = new Uri(ConfigurationManager.AppSettings.Get("urlFtp"));
                    string usuario = ConfigurationManager.AppSettings.Get("ftpUser");
                    string password = ConfigurationManager.AppSettings.Get("ftpPass");
                    Ma_Contrarecibos contraRecibo = dalContra.GetByID(x => x.Id_FolioCR == fFolioCR && x.Id_TipoCR == fTipoCR);
                    if (contraRecibo != null)
                    {
                        Ftp ftp = new Ftp(url, usuario, password);
                        string extension = System.IO.Path.GetExtension(file.FileName);
                        string path = String.Format("{0}/{1}", "/Archivoscontabilidad", contraRecibo.Id_TipoCR);
                        ftp.ExisteDirectorio(path);
                        UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                        string nameFile = "";
                        if (string.IsNullOrEmpty(appUsuario.Conexion))
                            nameFile = string.Format("{0}/{1}-{2}-{3}_{4}.{5}", path, contraRecibo.Id_TipoCR, contraRecibo.Id_FolioCR, DateTime.Now.Year.ToString(), new Random().Next(), "pdf");
                        else
                            nameFile = string.Format("{0}/{1}-{2}-{3}_{4}.{5}", path, contraRecibo.Id_TipoCR, contraRecibo.Id_FolioCR, appUsuario.Conexion, new Random().Next(), "pdf");
                        bool upload = ftp.UploadFTP(file.InputStream, nameFile);
                        if (upload)
                        {
                            DE_ContrarecibosArchivos registro = new DE_ContrarecibosArchivos();
                            registro.Fecha = DateTime.Now;
                            registro.Id_FolioCR = contraRecibo.Id_FolioCR;
                            registro.Id_TipoCR = contraRecibo.Id_TipoCR;
                            registro.Nombre = file.FileName;
                            registro.NombreSistema = nameFile;
                            registro.Tipo = System.IO.Path.GetExtension(file.FileName).Split('.')[1].ToUpper();
                            registro.usu_act = (short)appUsuario.IdUsuario;
                            dalArchivos.Insert(registro);
                            dalArchivos.Save();
                            return Json(new { Exito = true, Mensaje = "OK" });
                        }
                        else
                            return Json(new { Exito = false, Mensaje = "Ocurrió un error al subir el archivo. Contacte al equipo de Desarrollo." });
                    }
                }



                return Json(new { Exito = true, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        public ActionResult GetPDF(int IdArchivo)
        {
            Uri url = new Uri(ConfigurationManager.AppSettings.Get("urlFtp"));
            string usuario = ConfigurationManager.AppSettings.Get("ftpUser");
            string password = ConfigurationManager.AppSettings.Get("ftpPass");
            Ftp ftp = new Ftp(url, usuario, password);
            DE_ContrarecibosArchivos files = dalArchivos.GetByID(x => x.IdArchivo == IdArchivo);
            return File(ftp.DownloadFTP(files.NombreSistema), "application/pdf");
        }

        [HttpPost]
        public ActionResult EliminarArchivo(int IdArchivo)
        {
            try
            {
                Uri url = new Uri(ConfigurationManager.AppSettings.Get("urlFtp"));
                string usuario = ConfigurationManager.AppSettings.Get("ftpUser");
                string password = ConfigurationManager.AppSettings.Get("ftpPass");
                Ftp ftp = new Ftp(url, usuario, password);
                DE_ContrarecibosArchivos files = dalArchivos.GetByID(x => x.IdArchivo == IdArchivo);
                if (ftp.EliminarArchivo(files.NombreSistema))
                {
                    dalArchivos.Delete(x => x.IdArchivo == IdArchivo);
                    dalArchivos.Save();
                }
                return Json(new { Exito = true, Mensaje = "El archivo se eleminó correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        #endregion

        #region Fondos Revolventes y Gastos a Comprobar
        public ActionResult FondosRevolventes()
        {
            return RedirectToAction("Gastos_Fondos", new { Tipo = 3, json = false });
        }

        public ActionResult GastosaComprobar()
        {
            return RedirectToAction("Gastos_Fondos", new { Tipo = 4, json = false });
        }

        public ActionResult Gastos_Fondos(byte Tipo, bool json)
        {
            if (json)
                return Json(new { data = new Ma_ContrarecibosFGModel(Tipo) }, JsonRequestBehavior.AllowGet);
            TempData["FechaUltima"] = new Control_Fechas().Fecha_Max;
            TempData["FechaInicio"] = new Control_Fechas().Fecha_Min;
            return View(new Ma_ContrarecibosFGModel(Tipo));
        }

        public Ma_ContrarecibosFGModel Llenado_FG(byte Tipo, int Folio)
        {
            Ma_ContrarecibosFGModel ma = EntityFactory.getEntity<Ma_ContrarecibosFGModel>(_llenar.LLenado_MaContrarecibos(Tipo, Folio), new Ma_ContrarecibosFGModel()); ;
            if (ma.Id_FolioGC.HasValue)
                ma = EntityFactory.getEntity<Ma_ContrarecibosFGModel>(_llenar.LLenado_MaComprobaciones(ma.Id_FolioGC.Value), ma);

            ma.Reintegros = _dalFondosGastosBL.Calcula_Reintegros(Tipo, Folio, ma.Id_FolioGC);
            ma.SeReintegra = _dalFondosGastosBL.hasReintegro(Tipo, Folio, ma.Id_FolioGC);
            decimal? resta = ma.Cargos - _dalFondosGastosBL.cargosDetails(Tipo, Folio);
            if (resta < 0)
            {
                ma.Sobrantes = -1 * resta;
                ma.Reintegros = null;
                ma.SeReintegra = null;
            }
            if (ma.Reintegros == 0) ma.Reintegros = null;
            if (ma.Sobrantes == 0) ma.Sobrantes = null;
            ma.Original = _dalFondosGastosBL.ImporteOriginal(Tipo, Folio);
            if (_dalFondosGastosBL.Calcula_TotalReintegros(ma.Id_TipoCR, ma.Id_FolioCR, ma.Id_FolioGC) > 0)
                ma.TotalReintegro = _dalFondosGastosBL.Calcula_TotalReintegros(ma.Id_TipoCR, ma.Id_FolioCR, ma.Id_FolioGC);
            ma.EstatusCR = Diccionarios.Estatus_CR.SingleOrDefault(x => x.Key == ma.Id_EstatusCR).Value;
            ma.EstatusGCDesc = Diccionarios.Estatus_GC[ma.Estatus_GC.Value];
            return ma;
        }

        [HttpPost]
        public ActionResult Gastos_Fondos(byte Tipo, int Folio)
        {
            string[] datos = Session["ComprobacionPartida"] as string[];
            if (datos != null)
                Session.Remove("ComprobacionPartida");
            Ma_ContrarecibosFGModel ma = Llenado_FG(Tipo, Folio);
            List<object> Botones = new List<object>();
            _dalFondosGastosBL.CreateBotonera(ref Botones, Tipo, Folio);
            ma.Botonera = Botones;
            return View(ma);
        }

        public ActionResult Gastos_FondosN(Ma_ContrarecibosFGModel modelo)
        {
            UsuarioLogueado appusuario = Session["appUsuario"] as UsuarioLogueado;
            try
            {
                if (BlCompromisos.isClosed(modelo.FechaCR.Value))
                    return Json(new { Exito = false, Mensaje = "El Mes de La Fecha esta cerado" });
                if (_dalFondosGastosBL.isAdd(modelo.Id_CuentaFR))
                {
                    Ma_Contrarecibos ma = EntityFactory.getEntity<Ma_Contrarecibos>(modelo, new Ma_Contrarecibos());
                    ma.Id_FolioCR = BLContra.getMaContrarecibos(modelo.Id_TipoCR).Value;
                    if (ma.FechaCR != null && ma.Id_TipoCompromiso != null)
                        ma.FechaVen = Convert.ToDateTime(_StoreDal.PA_FechaCompromiso((short)ma.Id_TipoCompromiso, ma.FechaCR.Value.ToShortDateString()));
                    ma.Abonos = modelo.Cargos;
                    ma.Usuario_Act = (short)appusuario.IdUsuario;
                    ma.Usu_CR = appusuario.NombreCompleto;
                    ma.Fecha_Act = DateTime.Now;
                    _dalContra.Insert(ma);
                    _dalContra.Save();
                    BLContra.setMaContrarecibos(modelo.Id_TipoCR);
                    Ma_ContrarecibosFGModel mafg = Llenado_FG(ma.Id_TipoCR, ma.Id_FolioCR);
                    List<Object> botonera = new List<object>();
                    _dalFondosGastosBL.CreateBotonera(ref botonera, ma.Id_TipoCR, ma.Id_FolioCR);
                    return Json(new { Exito = true, Registro = mafg, Botonera = botonera, Mesnaje = "Se guardo correctamente" });
                }
                else
                    return Json(new { Exito = false, Mensaje = "No se puede dar de alta porque la cuenta tiene saldo pendiente" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = ex.Message });
            }
        }

        public ActionResult Gastos_FondosE(Ma_ContrarecibosFGModel modelo)
        {
            UsuarioLogueado appusuario = Session["appUsuario"] as UsuarioLogueado;
            try
            {
                Ma_Contrarecibos ma = _dalContra.GetByID(x => x.Id_TipoCR == modelo.Id_TipoCR && x.Id_FolioCR == modelo.Id_FolioCR);
                if (ma.Id_FolioPO_Ejercido == null)
                {
                    if (BlCompromisos.isClosed(modelo.FechaCR.Value))
                        return Json(new { Exito = false, Mensaje = "El Mes de La Fecha esta cerado" });
                    ma = EntityFactory.getEntity<Ma_Contrarecibos>(modelo, ma);
                    ma.Usu_CR = appusuario.NombreCompleto;
                    ma.Abonos = modelo.Cargos;
                    _dalContra.Update(ma);
                }
                else
                {
                    if (!string.IsNullOrEmpty(modelo.Spei))
                        ma.Spei = modelo.Spei;
                    else
                        ma.Spei = null;
                }
                ma.Usuario_Act = (short)appusuario.IdUsuario;
                ma.Fecha_Act = DateTime.Now;
                _dalContra.Save();
                BLContra.setMaContrarecibos(modelo.Id_TipoCR);
                Ma_ContrarecibosFGModel mafg = Llenado_FG(ma.Id_TipoCR, ma.Id_FolioCR);
                List<Object> botonera = new List<object>();
                _dalFondosGastosBL.CreateBotonera(ref botonera, ma.Id_TipoCR, ma.Id_FolioCR);
                return Json(new { Exito = true, Registro = mafg, Botonera = botonera, Mesnaje = "Se guardo correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = ex.Data });
            }
        }

        [HttpPost]
        public ActionResult Gastos_FondosC(byte TipoCR, int FolioCR, DateTime FechaC)
        {
            try
            {
                if (BlCompromisos.isClosed(FechaC))
                    return Json(new { Exito = false, Mensaje = "El Mes de La Fecha esta cerado" });
                UsuarioLogueado appusuario = Session["appUsuario"] as UsuarioLogueado;
                _dalFondosGastosBL.CancelarCRFG(TipoCR, FolioCR, (short)appusuario.IdUsuario, FechaC);
                return Json(new { Exito = true });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = ex.Data });
            }
        }

        [HttpPost]
        public ActionResult CerrarComprobacion(byte TipoCR, int FolioCR, DateTime FechaC)
        {
            //0.- Convert.ToString(mesR.Value), 1.- Convert.ToString(folioR), 2.- Convert.ToString(folioPagado),  3.-Convert.ToString(folioEjercido),
            //4.- Convert.ToString(folioDevengado), 5.- Convert.ToString(folioCompromiso) };
            //MaRep.GuardarDatosSobrantesReintegros(TipoCR, FolioCR, FechaCerrado, null, null, null, null, null, null, null, null, MesPol, FolioPoC, MesPol, FolioPoD, MesPol, FolioPoE, MesPol, FolioPoP, appUsuario.DatosUsuario.Id_Usuario, FolioPoDiario); 
            if (BlCompromisos.isClosed(FechaC))
                return Json(new { Exito = false, Mensaje = "El Mes de La Fecha esta cerado" });
            UsuarioLogueado appusuario = Session["appUsuario"] as UsuarioLogueado;
            string[] resultado = _StoreDal.PA_Genera_Polizas_GC_FR(TipoCR, FolioCR, FechaC, (short)appusuario.IdUsuario);
            _dalFondosGastosBL.GuardarDatosSobrantesReintegros(TipoCR, FolioCR, FechaC, null, null, Convert.ToByte(resultado[0]),
                Convert.ToInt32(resultado[5]), Convert.ToInt32(resultado[4]), Convert.ToInt32(resultado[3]),
                Convert.ToInt32(resultado[2]), Convert.ToInt32(resultado[1]), (short)appusuario.IdUsuario
                );
            Ma_ContrarecibosFGModel ma = Llenado_FG(TipoCR, FolioCR);
            List<Object> botonera = new List<object>();
            _dalFondosGastosBL.CreateBotonera(ref botonera, ma.Id_TipoCR, ma.Id_FolioCR);
            return Json(new { Exito = true, Data = ma, Botones = botonera });
        }

        [HttpGet]
        public ActionResult Reintegros(int FolioGC)
        {
            TempData["Banco"] = new SelectList(dalBancos.Get(), "Id_Banco", "Descripcion");
            TempData["CtaBancaria"] = new SelectList(new Dictionary<int, string>(), "key", "value");
            return View(new ReintegrosModel(FolioGC));
        }

        [HttpPost]
        public ActionResult Reintegros(ReintegrosModel modelo, FormCollection collection)
        {
            try
            {

                UsuarioLogueado appusuario = Session["appUsuario"] as UsuarioLogueado;
                if (Convert.ToByte(collection["TipoReintegro"]) == 1)
                {
                    if (BlCompromisos.isClosed(modelo.FechaEfectivo))
                        return Json(new { Exito = false, Mensaje = "El Mes de La Fecha esta cerado" });
                    _dalFondosGastosBL.Pa_Reintegros(modelo.FolioGCEfectivo, modelo.CtaEfectivo, modelo.ImporteEfectivo, modelo.FechaEfectivo, (short)appusuario.IdUsuario, modelo.NoReciboEfectivo, null);
                }
                if (Convert.ToByte(collection["TipoReintegro"]) == 2)
                {
                    if (BlCompromisos.isClosed(modelo.FechaReintegro))
                        return Json(new { Exito = false, Mensaje = "El Mes de La Fecha esta cerado" });
                    _dalFondosGastosBL.Pa_Reintegros(modelo.FolioGCEfectivo, null, modelo.ImporteReintegro, modelo.FechaReintegro, (short)appusuario.IdUsuario, modelo.NoReciboReintegro, modelo.CtaBancariaReintegro);
                }
                return Json(new { Exito = true });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult GeneracionPolizas(byte Tipo, int Folio, short NoComporbacion)
        {
            UsuarioLogueado appusuario = Session["appUsuario"] as UsuarioLogueado;
            string[] datos = Session["ComprobacionPartida"] as string[];
            DateTime Fecha = Convert.ToDateTime(datos[0]);
            if (BlCompromisos.isClosed(Fecha))
                return Json(new { Exito = false, Mensaje = "El Mes de La Fecha esta cerado" });
            return Json(new { Exito = _dalFondosGastosBL.Pa_Reintegros2(Tipo, Folio, NoComporbacion, Fecha, (short)appusuario.IdUsuario) });
        }

        [HttpPost]
        public ActionResult CancelacionPolizaReintegro(byte Tipo, int Folio, DateTime FechaC, int Consecutivo)
        {
            if (BlCompromisos.isClosed(FechaC))
                return Json(new { Exito = false, Mensaje = "El Mes de La Fecha esta cerado" });
            UsuarioLogueado appusuario = Session["appUsuario"] as UsuarioLogueado;
            return Json(new { Exito = _dalFondosGastosBL.CancelacionPolizaReintegro(Tipo, Folio, FechaC, (short)appusuario.IdUsuario, Consecutivo) });
        }

        //[HttpPost]
        //public ActionResult Gastos_Fondos_Detalle(byte TipoCR, int FolioCR, bool json)
        //{

        //    if (json)
        //        return Json(new { Exito = true, Registro = new De_ContrarecibosModel(TipoCR,FolioCR)});

        //    Ma_ContrarecibosModel ma = _llenar.LLenado_MaContrarecibos(TipoCR, FolioCR);
        //    ma.Botonera = BLDeContra.createBotonera(TipoCR, FolioCR);
        //    return View(ma);
        //}

        [HttpPost]
        public ActionResult Gastos_Fondos_Detalle(byte TipoCR, int FolioCR, short? Registro, bool? json)
        {
            List<object> Botones = new List<object>();
            string[] datos = Session["ComprobacionPartida"] as string[];
            if (Registro.HasValue && json == null)
                return Json(new { Exito = true, data = _llenar.LLenado_DeContrarecibos(TipoCR, FolioCR, Registro.Value) });
            if (Registro.HasValue && json == true)
            {
                De_ContrarecibosModel de = new De_ContrarecibosModel(TipoCR, FolioCR);
                if (datos != null)
                {
                    BLDetalleFG.CreateBotoneraComprobacion(ref Botones, TipoCR, FolioCR, Convert.ToInt16(datos[1]));
                    de.No_Comprobacion = Convert.ToInt16(datos[1]);
                }
                else
                    BLDetalleFG.CreateBotonera(ref Botones, TipoCR, FolioCR);
                return Json(new { Exito = true, Registro = de, Botones = Botones });
            }
            Ma_ContrarecibosFGModel ma = Llenado_FG(TipoCR, FolioCR);
            if (datos != null)
            {
                ma.Fecha_Comprobacion = Convert.ToDateTime(datos[0]);
                ma.No_Comprobacion = Convert.ToInt16(datos[1]);
                BLDetalleFG.CreateBotoneraComprobacion(ref Botones, TipoCR, FolioCR, Convert.ToInt16(datos[1]));
            }
            else
                BLDetalleFG.CreateBotonera(ref Botones, TipoCR, FolioCR);
            ma.Botonera = Botones;
            return View(ma);
        }

        [HttpPost]
        public ActionResult ComprobacionPartida(byte TipoComprobacion, int FolioComprobacion, DateTime FechaComprobacion)
        {
            if (BlCompromisos.isClosed(FechaComprobacion))
                return Json(new { Exito = false, Mensaje = "El Mes de La Fecha esta cerado" });
            Session["ComprobacionPartida"] = new string[] { FechaComprobacion.ToString(), BLDetalleFG.getNoComprobacion(TipoComprobacion, FolioComprobacion).ToString() };
            return Json(new { Exito = true });
        }

        [HttpPost]
        public ActionResult ComprobacionPartidaDetalles(byte TipoComprobacion, int FolioComprobacion, DateTime FechaComprobacion, short NoComprobacion)
        {
            if (BlCompromisos.isClosed(FechaComprobacion))
                return Json(new { Exito = false, Mensaje = "El Mes de La Fecha esta cerado" });
            Session["ComprobacionPartida"] = new string[] { FechaComprobacion.ToString(), NoComprobacion.ToString() };
            return Json(new { Exito = true });
        }

        [HttpPost]
        public ActionResult VerificaNoComprobacion(byte Tipo, int Folio, short NoComprobacion)
        {
            return Json(new { Exito = _dalFondosGastosBL.VerificaNoComprobacion(Tipo, Folio, NoComprobacion) });
        }

        [HttpPost]
        public ActionResult Gastos_Fondos_DetalleN(De_ContrarecibosModel modelo)
        {
            try
            {
                De_Contrarecibos entity = _dalDeContra.GetByID(reg => reg.Id_TipoCR == modelo.Id_TipoCR && reg.Id_FolioCR == modelo.Id_FolioCR && reg.Id_Registro == modelo.Id_Registro);
                if (entity != null)
                {
                    //Editar
                    entity = EntityFactory.getEntity<De_Contrarecibos>(modelo, entity);
                    entity.Id_ClavePresupuesto = StringID.IdClavePresupuesto(modelo.Id_Area, modelo.Id_Funcion, modelo.Id_Actividad, modelo.Id_ClasificacionP, modelo.Id_Programa, modelo.Id_Proceso, modelo.Id_TipoMeta, modelo.Id_ActividadMIR, modelo.Id_Accion, modelo.Id_Alcance, modelo.Id_TipoG, modelo.Id_Fuente, modelo.AnioFin, modelo.Id_ObjetoG);
                    entity.Disponibilidad = BLDeCompromisos.hasDisponibilidad(entity.Id_ClavePresupuesto, entity.Importe.Value, modelo.Fecha);
                    if (entity.Disponibilidad.Value == false && Convert.ToBoolean(DALParametros.GetByID(reg => reg.Nombre == "Sin_Disponibilidad").Valor) == false)
                        return Json(new { Exito = false, Mensaje = "Esta clave presupuestal no cuenta con disponibilidad." });

                    if (!entity.Disponibilidad.Value && entity.No_Comprobacion == null)
                    {
                        Ma_Contrarecibos master = _dalContra.GetByID(x => x.Id_TipoCR == entity.Id_TipoCR && x.Id_FolioCR == entity.Id_FolioCR);
                        master.Id_EstatusCR = Diccionarios.Valores_Estatus_CR.Autorización;
                        _dalContra.Update(master);
                        _dalContra.Save();
                    }
                    _dalDeContra.Update(entity);
                    _dalDeContra.Save();
                    return Json(new { Exito = true, Mensaje = "OK", Registro = entity.Id_Registro });
                }
                entity = EntityFactory.getEntity<De_Contrarecibos>(modelo, new De_Contrarecibos());
                entity.Id_ClavePresupuesto = StringID.IdClavePresupuesto(modelo.Id_Area, modelo.Id_Funcion, modelo.Id_Actividad, modelo.Id_ClasificacionP, modelo.Id_Programa, modelo.Id_Proceso, modelo.Id_TipoMeta, modelo.Id_ActividadMIR, modelo.Id_Accion, modelo.Id_Alcance, modelo.Id_TipoG, modelo.Id_Fuente, modelo.AnioFin, modelo.Id_ObjetoG);
                entity.Disponibilidad = BLDeCompromisos.hasDisponibilidad(entity.Id_ClavePresupuesto, entity.Importe.Value, modelo.Fecha);
                if (entity.Disponibilidad.Value == false && Convert.ToBoolean(DALParametros.GetByID(reg => reg.Nombre == "Sin_Disponibilidad").Valor) == false)
                    return Json(new { Exito = false, Mensaje = "Esta clave presupuestal no cuenta con disponibilidad." });

                if (!entity.Disponibilidad.Value && entity.No_Comprobacion == null)
                {
                    Ma_Contrarecibos master = _dalContra.GetByID(x => x.Id_TipoCR == entity.Id_TipoCR && x.Id_FolioCR == entity.Id_FolioCR);
                    master.Id_EstatusCR = Diccionarios.Valores_Estatus_CR.Autorización;
                    _dalContra.Update(master);
                    _dalContra.Save();
                }

                entity.Id_Registro = (short)BLDeContra.getNextId(modelo.Id_TipoCR, modelo.Id_FolioCR);
                entity.Id_Movimiento = 1;
                _dalDeContra.Insert(entity);
                _dalDeContra.Save();
                return Json(new { Exito = true, Mensaje = "OK", Registro = entity.Id_Registro });
            }
            catch
            {
                return Json(new { Exito = false, Mensaje = "Ocurrio Un Error" });
            }
        }

        [HttpGet]
        public ActionResult SaldarMovimientos()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaldarMovimientos(byte TipoCR, int FolioCR, string CtaxPagar)
        {
            try
            {
                //Si CtaxPagar no es null y nocomprobacion es null es la primer comprobacion, se agregaran 2 registros el 1 con la cuenta del gc o fr y la 2da con la cuenta por pagar
                //si no es la primera comprobacion no comprobacion no es null y ctaxpagar es null y solamente se suman los cargos de los detalles
                Ma_Contrarecibos ma = _dalContra.GetByID(x => x.Id_TipoCR == TipoCR && x.Id_FolioCR == FolioCR);
                DateTime? Fecha = null;
                decimal? Importe = null;
                short? NoComprobacion = null;
                string[] reintegro = Session["ComprobacionPartida"] as string[];
                if (reintegro != null)
                {
                    Fecha = Convert.ToDateTime(reintegro[0]);
                    NoComprobacion = Convert.ToInt16(reintegro[1]);
                }
                else
                {
                    Fecha = ma.FechaCR;
                }
                Importe = _dalDeContra.Get(x => x.Id_TipoCR == TipoCR && x.Id_FolioCR == FolioCR && (NoComprobacion.HasValue ? x.No_Comprobacion == NoComprobacion : x.No_Comprobacion == null) && x.Id_Movimiento == Diccionarios.ValorMovimientos.CARGO).Sum(x => x.Importe);
                if (!NoComprobacion.HasValue && Importe > ma.Cargos)
                    Importe = ma.Cargos;
                //Se borran todos los abonos
                List<De_Contrarecibos> borrar = _dalDeContra.Get(x => x.Id_TipoCR == TipoCR && x.Id_FolioCR == FolioCR && x.Id_Movimiento == Diccionarios.ValorMovimientos.ABONO && (NoComprobacion.HasValue ? x.No_Comprobacion == NoComprobacion : x.No_Comprobacion == null)).ToList();
                if (borrar.Count() > 0)
                {
                    foreach (De_Contrarecibos item in borrar)
                    {
                        _dalDeContra.Delete(x => x.Id_TipoCR == item.Id_TipoCR && x.Id_FolioCR == item.Id_FolioCR && (NoComprobacion.HasValue ? x.No_Comprobacion == item.No_Comprobacion : x.No_Comprobacion == null) && x.Id_Movimiento == Diccionarios.ValorMovimientos.ABONO && x.Id_Registro == item.Id_Registro);
                    }
                    _dalDeContra.Save();
                }

                De_Contrarecibos de = new De_Contrarecibos();
                de.Id_TipoCR = TipoCR;
                de.Id_FolioCR = FolioCR;
                de.Id_Registro = (short)BLDeContra.getNextId(TipoCR, FolioCR);
                de.Fecha = Fecha;
                if (NoComprobacion.HasValue) de.No_Comprobacion = NoComprobacion; else de.No_Comprobacion = null;
                de.Importe = Importe;
                de.Id_Cuenta = ma.Id_CuentaFR;
                de.Id_Movimiento = Diccionarios.ValorMovimientos.ABONO;
                _dalDeContra.Insert(de);
                _dalDeContra.Save();



                decimal? excedido = _dalFondosGastosBL.ReintegrosSobrantes(TipoCR, FolioCR) * -1;
                if (excedido > 0 && !String.IsNullOrEmpty(CtaxPagar))
                {
                    de = new De_Contrarecibos();
                    de.Id_TipoCR = TipoCR;
                    de.Id_FolioCR = FolioCR;
                    de.Id_Registro = (short)BLDeContra.getNextId(TipoCR, FolioCR);
                    de.Fecha = Fecha;
                    if (NoComprobacion.HasValue) de.No_Comprobacion = NoComprobacion; else de.No_Comprobacion = null;
                    de.Importe = excedido;
                    de.Id_Cuenta = CtaxPagar;
                    de.Id_Movimiento = Diccionarios.ValorMovimientos.ABONO;
                    _dalDeContra.Insert(de);
                    _dalDeContra.Save();
                }

                return Json(new { Exito = true, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = ex.Message });
            }
        }

        public ActionResult DeleteDeContrarecibos(byte TipoCR, int FolioCR, short Registro, short? NoComprobacion)
        {
            try
            {
                if (_dalDeContra.Get(x => x.Id_TipoCR == TipoCR && x.Id_FolioCR == FolioCR && x.Id_Movimiento == Diccionarios.ValorMovimientos.CARGO && (NoComprobacion.HasValue ? x.No_Comprobacion == NoComprobacion : x.No_Comprobacion == null)).Count() > 1
                    || Diccionarios.ValorMovimientos.ABONO == _dalDeContra.GetByID(x => x.Id_TipoCR == TipoCR && x.Id_FolioCR == FolioCR && x.Id_Registro == Registro && (NoComprobacion.HasValue ? x.No_Comprobacion == NoComprobacion : x.No_Comprobacion == null)).Id_Movimiento)
                {
                    _dalDeContra.Delete(x => x.Id_TipoCR == TipoCR && x.Id_FolioCR == FolioCR && x.Id_Registro == Registro && (NoComprobacion.HasValue ? x.No_Comprobacion == NoComprobacion : x.No_Comprobacion == null));
                    _dalDeContra.Save();
                }
                else
                {
                    foreach (De_Contrarecibos item in _dalDeContra.Get(x => x.Id_TipoCR == TipoCR && x.Id_FolioCR == FolioCR && (NoComprobacion.HasValue ? x.No_Comprobacion == NoComprobacion : x.No_Comprobacion == null)))
                    {
                        _dalDeContra.Delete(x => x.Id_TipoCR == item.Id_TipoCR && x.Id_FolioCR == item.Id_FolioCR && x.Id_Registro == item.Id_Registro && (item.No_Comprobacion.HasValue ? x.No_Comprobacion == item.No_Comprobacion : x.No_Comprobacion == null));
                    }
                    _dalDeContra.Save();
                }
                return Json(new { Exito = true });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = ex.Message });
            }
        }

        #endregion

        #region Cancelacion de Pasivos

        [HttpGet]
        public ActionResult Cancelacion_Pasivos()
        {
            //Ma_Contrarecibos
            Ma_ContrarecibosCPModel model = new Ma_ContrarecibosCPModel(Diccionarios.TiposCR.CancelacionPasivos);
            model.cfechas = new Control_Fechas();
            return View(model);
        }

        [HttpGet]
        public ActionResult AnticiposPrestamos()
        {
            //Ma_Contrarecibos
            Ma_ContrarecibosCPModel model = new Ma_ContrarecibosCPModel(Diccionarios.TiposCR.AnticiposPrestamos);
            model.cfechas = new Control_Fechas();
            return View(model);
        }

        [HttpPost]
        public ActionResult getCancelacion_Pasivos(byte? TipoCr)
        {
            Ma_ContrarecibosCPModel model = new Ma_ContrarecibosCPModel(TipoCr.HasValue ? TipoCr.Value : Diccionarios.TiposCR.CancelacionPasivos);
            model.cfechas = new Control_Fechas();
            return Json(new { Exito = true, Data = model, Mensaje = "OK" });
        }

        [HttpPost]
        public ActionResult Cancelacion_Pasivos(short TipoCR, int FolioCR)
        {
            Ma_ContrarecibosCPModel model = new Ma_ContrarecibosCPModel(Diccionarios.TiposCR.CancelacionPasivos);
            model = ModelFactory.getModel<Ma_ContrarecibosCPModel>(dalContra.GetByID(reg => reg.Id_TipoCR == TipoCR && reg.Id_FolioCR == FolioCR), model);
            return View();
        }

        public ActionResult getCargosAbonos(string Id_Cuenta, int mes)
        {
            return Json(new { Exito = true, Data = BLCuentas.getCargosAbonos(Id_Cuenta, mes), Mensaje = "OK" });
        }

        [HttpPost]
        [ActionName("Cancelacion_PasivosGuardar")]
        public ActionResult Cancelacion_Pasivos(Ma_ContrarecibosCPModel dataModel)
        {
            try
            {
                Ma_Contrarecibos entity = new Ma_Contrarecibos();
                ContrarecibosBL foliador = new ContrarecibosBL();
                if (dataModel.Id_FolioCR == 0)
                {
                    //Nuevo
                    dataModel.Abonos = dataModel.Cargos;
                    if (dataModel.FechaCR != null && dataModel.Id_TipoCompromiso != null)
                        dataModel.FechaVen = Convert.ToDateTime(_StoreDal.PA_FechaCompromiso((short)dataModel.Id_TipoCompromiso, dataModel.FechaCR.Value.ToShortDateString()));
                    //dataModel.FechaVen = dataModel.FechaCR;
                    dataModel.Fecha_Act = DateTime.Now;
                    dataModel.Usuario_Act = (short)Logueo.GetUsrLogueado().IdUsuario;
                    dataModel.Id_FolioCR = foliador.getMaContrarecibos(dataModel.Id_TipoCR).Value;
                    entity = EntityFactory.getEntity<Ma_Contrarecibos>(dataModel, new Ma_Contrarecibos());
                    dalContra.Insert(entity);
                    dalContra.Save();
                    foliador.setMaContrarecibos(dataModel.Id_TipoCR);
                }
                else
                {
                    //Editar
                    if (dataModel.FechaCR != null && dataModel.Id_TipoCompromiso != null)
                        dataModel.FechaVen = Convert.ToDateTime(_StoreDal.PA_FechaCompromiso((short)dataModel.Id_TipoCompromiso, dataModel.FechaCR.Value.ToShortDateString()));
                    //dataModel.FechaVen = dataModel.FechaCR;
                    dataModel.Fecha_Act = DateTime.Now;
                    dataModel.Usuario_Act = (short)Logueo.GetUsrLogueado().IdUsuario;
                    entity = dalContra.GetByID(reg => reg.Id_TipoCR == dataModel.Id_TipoCR && reg.Id_FolioCR == dataModel.Id_FolioCR);
                    entity = EntityFactory.getEntity<Ma_Contrarecibos>(dataModel, entity);
                    dalContra.Update(entity);
                    dalContra.Save();
                }
                return Json(new { Exito = true, FechaVen = entity.FechaVen.Value.ToShortDateString(), FolioCR = entity.Id_FolioCR, Botonera = foliador.createBotoneraCP(dataModel), Data = dataModel, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpGet]
        public ActionResult CancelarCRCP(short TipoCR, int FolioCR)
        {
            Ma_ContrarecibosCPModel master = ModelFactory.getModel<Ma_ContrarecibosCPModel>(dalContra.GetByID(reg => reg.Id_TipoCR == TipoCR && reg.Id_FolioCR == FolioCR), new Ma_ContrarecibosCPModel(Diccionarios.TiposCR.CancelacionPasivos));
            CancelaCR_CPModel model = new CancelaCR_CPModel();
            model.FechaCR_C = master.FechaCR;
            model.cfechas = new Control_Fechas();
            model.cfechas.Fecha_Min = master.FechaCR.Value;
            model.Id_TipoCR_C = master.Id_TipoCR;
            model.Id_FolioCR_C = master.Id_FolioCR;
            return View(model);
        }

        [HttpPost]
        public ActionResult CancelarCRCP(CancelaCR_CPModel dataModel)
        {
            Ma_Contrarecibos master = dalContra.GetByID(reg => reg.Id_TipoCR == dataModel.Id_TipoCR_C && reg.Id_FolioCR == dataModel.Id_FolioCR_C);
            if (!(dataModel.FechaCR_C >= master.FechaCR))
                return Json(new { Exito = false, Data = dataModel, Mensaje = "La fecha de cancelación debe ser mayor o igual a la del contrarecibo" });
            try
            {
                if (master.Id_EstatusCR == Diccionarios.Valores_Estatus_CR.Programado)
                    master.Id_EstatusCR = Diccionarios.Valores_Estatus_CR.Cancelado;
                else if (master.Id_MesPolizaCH != null)
                {
                    string[] polizas = _StoreDal.PA_Genera_Poliza_Egresos_Cancela(master.Id_TipoCR, master.Id_FolioCR, master.Id_CtaBancaria, master.No_Cheque, master.Fecha_Pago, (short)Logueo.GetUsrLogueado().IdUsuario);
                    master.Id_MesPolizaCH_C = byte.Parse(polizas[0]);
                    master.Id_FolioPolizaCH_C = int.Parse(polizas[1]);
                    master.FechaCancelacion_CH = dataModel.FechaCR_C;
                    master.Id_EstatusCR = Diccionarios.Valores_Estatus_CR.Cancelado;
                }
                _dalContra.Update(master);
                _dalContra.Save();

                Ma_ContrarecibosModel model = _llenar.LLenado_MaContrarecibos(master.Id_TipoCR, master.Id_FolioCR);
                Ma_ContrarecibosCPModel mastermodel = ModelFactory.getModel<Ma_ContrarecibosCPModel>(model, new Ma_ContrarecibosCPModel());
                mastermodel.Botonera = repo.createBotoneraCP(mastermodel);
                mastermodel.Descripcion_EstatusCR = Diccionarios.Estatus_CR[(short)mastermodel.Id_EstatusCR];
                return Json(new { Exito = true, Data = mastermodel, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        #endregion

        #region Egresos No Presupuestales
        [HttpGet]
        public ActionResult V_EgresosNoPresupuestales()
        {
            //Ma_Contrarecibos
            Ma_ContrarecibosCPModel model = new Ma_ContrarecibosCPModel(Diccionarios.TiposCR.EgresosNoPresupuestales);
            model.cfechas = new Control_Fechas();
            ViewBag.Generos = new ParametrosDAL().GetByID(x => x.Nombre == "EgresosNoPptales").Valor;
            return View(model);
        }

        [HttpPost]
        public ActionResult getEgresosNoPresupuestales()
        {
            Ma_ContrarecibosCPModel model = new Ma_ContrarecibosCPModel(Diccionarios.TiposCR.EgresosNoPresupuestales);
            model.cfechas = new Control_Fechas();
            return Json(new { Exito = true, Data = model, Mensaje = "OK" });
        }

        //[HttpPost]
        //public ActionResult Cancelacion_Pasivos(short TipoCR, int FolioCR)
        //{
        //    Ma_ContrarecibosCPModel model = new Ma_ContrarecibosCPModel(Diccionarios.TiposCR.CancelacionPasivos);
        //    model = ModelFactory.getModel<Ma_ContrarecibosCPModel>(dalContra.GetByID(reg => reg.Id_TipoCR == TipoCR && reg.Id_FolioCR == FolioCR), model);
        //    return View();
        //}

        //public ActionResult getCargosAbonos(string Id_Cuenta, int mes)
        //{
        //    return Json(new { Exito = true, Data = BLCuentas.getCargosAbonos(Id_Cuenta, mes), Mensaje = "OK" });
        //}

        [HttpPost]
        [ActionName("EgresosNoPrepGuardar")]
        public ActionResult EgresosNoPresupuestales(Ma_ContrarecibosCPModel dataModel)
        {
            try
            {
                Ma_Contrarecibos entity = new Ma_Contrarecibos();
                ContrarecibosBL foliador = new ContrarecibosBL();
                if (dataModel.Id_FolioCR == 0)
                {
                    //Nuevo
                    dataModel.Abonos = dataModel.Cargos;
                    if (dataModel.FechaCR != null && dataModel.Id_TipoCompromiso != null)
                        dataModel.FechaVen = Convert.ToDateTime(_StoreDal.PA_FechaCompromiso((short)dataModel.Id_TipoCompromiso, dataModel.FechaCR.Value.ToShortDateString()));
                    //dataModel.FechaVen = dataModel.FechaCR;
                    dataModel.Fecha_Act = DateTime.Now;
                    dataModel.Usuario_Act = (short)Logueo.GetUsrLogueado().IdUsuario;
                    dataModel.Id_FolioCR = foliador.getMaContrarecibos(dataModel.Id_TipoCR).Value;
                    entity = EntityFactory.getEntity<Ma_Contrarecibos>(dataModel, new Ma_Contrarecibos());
                    dalContra.Insert(entity);
                    dalContra.Save();
                    foliador.setMaContrarecibos(dataModel.Id_TipoCR);
                }
                else
                {
                    //Editar
                    if (dataModel.FechaCR != null && dataModel.Id_TipoCompromiso != null)
                        dataModel.FechaVen = Convert.ToDateTime(_StoreDal.PA_FechaCompromiso((short)dataModel.Id_TipoCompromiso, dataModel.FechaCR.Value.ToShortDateString()));
                    //dataModel.FechaVen = dataModel.FechaCR;
                    dataModel.Fecha_Act = DateTime.Now;
                    dataModel.Usuario_Act = (short)Logueo.GetUsrLogueado().IdUsuario;
                    entity = dalContra.GetByID(reg => reg.Id_TipoCR == dataModel.Id_TipoCR && reg.Id_FolioCR == dataModel.Id_FolioCR);
                    entity = EntityFactory.getEntity<Ma_Contrarecibos>(dataModel, entity);
                    dalContra.Update(entity);
                    dalContra.Save();
                }
                return Json(new { Exito = true, FolioCR = entity.Id_FolioCR, Botonera = foliador.createBotoneraNP(dataModel), Data = dataModel, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        public ActionResult TablaSaldos(Int16? IdTipoCR, Int16? IdFolioCR)
        {
            List<De_Contrarecibos> lst = _dalDeContra.Get(x => x.Id_FolioCR == IdFolioCR && x.Id_TipoCR == IdTipoCR).ToList();
            List<De_ContrarecibosModel> models = new List<De_ContrarecibosModel>();
            foreach (De_Contrarecibos item in lst)
            {
                models.Add(_llenar.LLenado_DeContrarecibos(item.Id_TipoCR, item.Id_FolioCR, item.Id_Registro));
            }
            return View(models);
        }
        [HttpPost]
        public JsonResult SaveDetails(byte IdTipoCR, Int16 IdFolioCR, decimal Importe, string IdCuenta)
        {
            try
            {
                ContrarecibosBL foliador = new ContrarecibosBL();
                De_Contrarecibos model = new De_Contrarecibos();
                model.Id_TipoCR = IdTipoCR;
                model.Id_FolioCR = IdFolioCR;
                model.Importe = Importe;
                model.Id_Cuenta = IdCuenta;
                model.Id_Movimiento = 1;
                if (_dalDeContra.Get(x => x.Id_TipoCR == IdTipoCR && x.Id_FolioCR == IdFolioCR).Count() > 0)
                    model.Id_Registro = Convert.ToInt16(_dalDeContra.Get(x => x.Id_TipoCR == IdTipoCR && x.Id_FolioCR == IdFolioCR).Max(x => x.Id_Registro) + 1);
                else
                    model.Id_Registro = 1;
                _dalDeContra.Insert(model);
                _dalDeContra.Save();
                Ma_Contrarecibos contra = _dalContra.GetByID(x => x.Id_FolioCR == IdFolioCR && x.Id_TipoCR == IdTipoCR);
                contra.Cargos += Importe;
                contra.Abonos += Importe;
                _dalContra.Update(contra);
                _dalContra.Save();
                Ma_ContrarecibosCPModel dataModel = ModelFactory.getModel<Ma_ContrarecibosCPModel>(ModelFactory.getModel<Ma_ContrarecibosModel>(contra, new Ma_ContrarecibosModel()), new Ma_ContrarecibosCPModel());
                return Json(new { Exito = true, Registro = _llenar.LLenado_DeContrarecibos(model.Id_TipoCR, model.Id_FolioCR, model.Id_Registro), importe = String.Format("{0:N}", Importe), Botonera = foliador.createBotoneraNP(dataModel), Mensaje = "OK", Cargo = contra.Cargos });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        [HttpPost]
        public JsonResult DeleteDetails(byte IdTipoCR, Int16 IdFolioCR, Int16 IdRegistro)
        {
            try
            {
                ContrarecibosBL foliador = new ContrarecibosBL();
                _dalDeContra.Delete(x => x.Id_TipoCR == IdTipoCR && x.Id_FolioCR == IdFolioCR && x.Id_Registro == IdRegistro);
                _dalDeContra.Save();
                Ma_Contrarecibos contra = _dalContra.GetByID(x => x.Id_FolioCR == IdFolioCR && x.Id_TipoCR == IdTipoCR);
                Ma_ContrarecibosCPModel dataModel = ModelFactory.getModel<Ma_ContrarecibosCPModel>(ModelFactory.getModel<Ma_ContrarecibosModel>(contra, new Ma_ContrarecibosModel()), new Ma_ContrarecibosCPModel());
                return Json(new { Exito = true, Mensaje = "OK", Botonera = foliador.createBotoneraNP(dataModel) });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        //[HttpGet]
        //public ActionResult CancelarCRCP(short TipoCR, int FolioCR)
        //{
        //    Ma_ContrarecibosCPModel master = ModelFactory.getModel<Ma_ContrarecibosCPModel>(dalContra.GetByID(reg => reg.Id_TipoCR == TipoCR && reg.Id_FolioCR == FolioCR), new Ma_ContrarecibosCPModel(Diccionarios.TiposCR.CancelacionPasivos));
        //    CancelaCR_CPModel model = new CancelaCR_CPModel();
        //    model.FechaCR_C = master.FechaCR;
        //    model.cfechas = new Control_Fechas();
        //    model.cfechas.Fecha_Min = master.FechaCR.Value;
        //    model.Id_TipoCR_C = master.Id_TipoCR;
        //    model.Id_FolioCR_C = master.Id_FolioCR;
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult CancelarCRCP(CancelaCR_CPModel dataModel)
        //{
        //    Ma_Contrarecibos master = dalContra.GetByID(reg => reg.Id_TipoCR == dataModel.Id_TipoCR_C && reg.Id_FolioCR == dataModel.Id_FolioCR_C);
        //    if (!(dataModel.FechaCR_C >= master.FechaCR))
        //        return Json(new { Exito = false, Data = dataModel, Mensaje = "La fecha de cancelación debe ser mayor o igual a la del contrarecibo" });
        //    try
        //    {
        //        if (master.Id_EstatusCR == Diccionarios.Valores_Estatus_CR.Programado)
        //            master.Id_EstatusCR = Diccionarios.Valores_Estatus_CR.Cancelado;
        //        else if (master.Id_MesPolizaCH != null)
        //        {
        //            string[] polizas = _StoreDal.PA_Genera_Poliza_Egresos_Cancela(master.Id_TipoCR, master.Id_FolioCR, master.Id_CtaBancaria, master.No_Cheque, master.Fecha_Pago, (short)Logueo.GetUsrLogueado().IdUsuario);
        //            master.Id_MesPolizaCH_C = byte.Parse(polizas[0]);
        //            master.Id_FolioPolizaCH_C = int.Parse(polizas[1]);
        //            master.FechaCancelacion_CH = dataModel.FechaCR_C;
        //            master.Id_EstatusCR = Diccionarios.Valores_Estatus_CR.Cancelado;
        //        }
        //        _dalContra.Update(master);
        //        _dalContra.Save();

        //        Ma_ContrarecibosModel model = _llenar.LLenado_MaContrarecibos(master.Id_TipoCR, master.Id_FolioCR);
        //        Ma_ContrarecibosCPModel mastermodel = ModelFactory.getModel<Ma_ContrarecibosCPModel>(model, new Ma_ContrarecibosCPModel());
        //        mastermodel.Botonera = repo.createBotoneraCP(mastermodel);
        //        mastermodel.Descripcion_EstatusCR = Diccionarios.Estatus_CR[(short)mastermodel.Id_EstatusCR];
        //        return Json(new { Exito = true, Data = mastermodel, Mensaje = "OK" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
        //    }
        //}


        #endregion

        public JsonResult Cuenta2(String cuenta2, decimal importe2)
        {
            CA_Usuarios UsuariosExistente = new CA_Usuarios();

            if (string.IsNullOrEmpty(cuenta2))
            {
                return importe2 == 0 ? Json(true, JsonRequestBehavior.AllowGet)
                                     : Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return (importe2 > 0) ? Json(true, JsonRequestBehavior.AllowGet)
                                     : Json(false, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult Cuenta3(String cuenta3, decimal importe3)
        {
            CA_Usuarios UsuariosExistente = new CA_Usuarios();

            if (string.IsNullOrEmpty(cuenta3))
            {
                return importe3 == 0 ? Json(true, JsonRequestBehavior.AllowGet)
                                     : Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return (importe3 > 0) ? Json(true, JsonRequestBehavior.AllowGet)
                                     : Json(false, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult Cuenta4(String cuenta4, decimal importe4)
        {
            CA_Usuarios UsuariosExistente = new CA_Usuarios();

            if (string.IsNullOrEmpty(cuenta4))
            {
                return importe4 == 0 ? Json(true, JsonRequestBehavior.AllowGet)
                                     : Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return (importe4 > 0) ? Json(true, JsonRequestBehavior.AllowGet)
                                     : Json(false, JsonRequestBehavior.AllowGet);
            }


        }

    }

}
