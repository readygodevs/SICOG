using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.BL;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using TesoreriaVS12.Filters;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;
namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    [AuthorizeLogin]
    public class IngresosController : Controller
    {
        //
        // GET: /Tesoreria/Ingresos/
        protected MaTransferenciasIngDAL MaTransferenciasIngDAL { get; set; }
        protected MaPolizasDAL MaPolizasDAL { get; set; }
        protected CierreMensualDAL cierreMensualDAL { get; set; }
        protected DeDisponibilidadDAL DeDisponibilidadDAL { get; set; }
        protected DeTransferenciaIngDAL DeTransferenciaIngDAL { get; set; }
        protected ProceduresDAL proceduresDAL { get; set; }
        protected DeEvolucionDAL DALEvolucion { get; set; }
        protected MaPresupuestoIngDAL DALMaPresupustoIng { get; set; }
        protected AmpliacionesReduccionesBL AmpliacionesReduccionesBL { get; set; }
        protected Llenado llenar { get; set; }
        private MaRecibosDAL _MaRecibosDAL { get; set; }
        private DeRecibosDAL _DeRecibosDAL { get; set; }
        private Listas _listas { get; set; }
        private PersonasDAL personasDAL { get; set; }
        private RecibosBL reciboBL { get; set; }
        private ProceduresDAL _StoredProcedure { get; set; }
        protected ConvertHtmlToString reports { get; set; }
        private CompromisosBL compromisosBL { get; set; }
        public IngresosController()
        {
            if (MaPolizasDAL == null) MaPolizasDAL = new MaPolizasDAL();
            if (MaTransferenciasIngDAL == null) MaTransferenciasIngDAL = new MaTransferenciasIngDAL();
            if (cierreMensualDAL == null) cierreMensualDAL = new CierreMensualDAL();
            if (DeDisponibilidadDAL == null) DeDisponibilidadDAL = new DeDisponibilidadDAL();
            if (DeTransferenciaIngDAL == null) DeTransferenciaIngDAL = new DeTransferenciaIngDAL();
            if (proceduresDAL == null) proceduresDAL = new ProceduresDAL();
            if (DALEvolucion == null) DALEvolucion = new DeEvolucionDAL();
            if (AmpliacionesReduccionesBL == null) AmpliacionesReduccionesBL = new AmpliacionesReduccionesBL();
            if (llenar == null) llenar = new Llenado();
            if (DALMaPresupustoIng == null) DALMaPresupustoIng = new MaPresupuestoIngDAL();
            if (_MaRecibosDAL == null) _MaRecibosDAL = new MaRecibosDAL();
            if (_DeRecibosDAL == null) _DeRecibosDAL = new DeRecibosDAL();
            if (_listas == null) _listas = new Listas();
            if (personasDAL == null) personasDAL = new PersonasDAL();
            if (reciboBL == null) reciboBL = new RecibosBL();
            if (_StoredProcedure == null) _StoredProcedure = new ProceduresDAL();
            if (reports == null) reports = new ConvertHtmlToString();
            if (compromisosBL == null) compromisosBL = new CompromisosBL();
        }
        public ActionResult Index()
        {
            return View();
        }
        #region AmpliacionesyReduccionesIng
        public ActionResult V_AmpliacionesReduccionesIng()
        {
            ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, AmpliacionesReduccionesBL.GetUltimoMesCerrado() + 1, 1).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            ViewBag.post = false;
            return View(new Ma_TransferenciasIngModel());
        }
        [HttpPost]
        public ActionResult V_AmpliacionesReduccionesIng(Int32 folio)
        {
            ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, AmpliacionesReduccionesBL.GetUltimoMesCerrado() + 1, 1).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            ViewBag.post = true;
            Ma_TransferenciasIngModel datamodel = llenar.LLenado_MaTransferenciasIng(folio);
            datamodel.PolizaOrdenModificado = StringID.Polizas(datamodel.Id_FolioPO_Modificado, datamodel.Id_MesPO_Modificado);
            datamodel.PolizaOrdenCancelado = StringID.Polizas(datamodel.Id_FolioPO_Modificado_Cancela, datamodel.Id_MesPO_Modificado_Cancela);
            return View(datamodel);
        }
        [HttpPost]
        public JsonResult ValidarExistencia()
        {
            string seleccion = "";
            if (AmpliacionesReduccionesBL.ValidarSinAfectarIng() > 0)
            {
                Ma_TransferenciasIng model = MaTransferenciasIngDAL.GetFisrt(x => x.Id_Estatus == 1 && x.Id_PptoModificado == true);
                if (model.Id_TipoT == 1)
                    seleccion = " Ampliación";
                else
                    seleccion = "Reducción";
                return Json(new { Exito = false, Mensaje = "No se puede agregar porque la " + seleccion + " No." + model.Folio + "no esta afectada" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Exito = true, Mensaje = "Todo bien" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult V_BuscarAmpliacionReduccion(byte idPpto)
        {
            ViewBag.idPpto = idPpto;
            return View();
        }

        public ActionResult V_TablaBusquedaAmpliacionReduccion(Int32? folioBusqueda, string DescripciónBusqueda, byte idPpto)
        {
            //idPpto 1=ampliacionReduccion; 2=transferencias,3 arrastre de saldos
            //IEnumerable<Ma_Transferencias> entities = MaTransferenciasIngDAL.Get(x => folioBusqueda > 0 ? x.Folio == folioBusqueda : x.Folio != null && !String.IsNullOrEmpty(Desc) ? x.Descrip == Desc : x.Descrip != null);
            List<Ma_TransferenciasIngModel> Lst = new List<Ma_TransferenciasIngModel>();
            List<Ma_TransferenciasIng> lista = new List<Ma_TransferenciasIng>();
            if (idPpto == 1)
            {
                lista = MaTransferenciasIngDAL.Get(x => folioBusqueda > 0 ? x.Folio == folioBusqueda : x.Folio != null && !String.IsNullOrEmpty(DescripciónBusqueda) ? x.Descrip.Contains(DescripciónBusqueda) : x.Descrip != null && x.Id_PptoModificado == true).ToList();
                foreach (Ma_TransferenciasIng item in lista)
                {
                    if (item.Id_PptoModificado == true)
                    {
                        Lst.Add((ModelFactory.getModel<Ma_TransferenciasIngModel>(item, new Ma_TransferenciasIngModel())));
                    }
                }
            }
            if (idPpto == 2)
            {
                lista = MaTransferenciasIngDAL.Get(x => folioBusqueda > 0 ? x.Folio == folioBusqueda : x.Folio != null && !String.IsNullOrEmpty(DescripciónBusqueda) ? x.Descrip.Contains(DescripciónBusqueda) : x.Descrip != null && x.Id_PptoModificado == false).ToList();
                foreach (Ma_TransferenciasIng item in lista)
                {
                    if (item.Id_PptoModificado == false)
                    {
                        Lst.Add((ModelFactory.getModel<Ma_TransferenciasIngModel>(item, new Ma_TransferenciasIngModel())));
                    }
                }
            }
            if (idPpto == 3)
            {
                lista = MaTransferenciasIngDAL.Get(x => folioBusqueda > 0 ? x.Folio == folioBusqueda : x.Folio != null && !String.IsNullOrEmpty(DescripciónBusqueda) ? x.Descrip.Contains(DescripciónBusqueda) : x.Descrip != null && x.Id_PptoModificado == null).ToList();
                foreach (Ma_TransferenciasIng item in lista)
                {
                    if (item.Id_PptoModificado == null)
                    {
                        Lst.Add((ModelFactory.getModel<Ma_TransferenciasIngModel>(item, new Ma_TransferenciasIngModel())));
                    }
                }
            }
            return View(Lst);
        }
        [HttpPost]
        public JsonResult SeleccionarTransferencia(Int32 IdTransferencia)
        {
            try
            {
                Ma_TransferenciasIng model = MaTransferenciasIngDAL.GetByID(x => x.Folio == IdTransferencia);
                if (model != null)
                {
                    int afecta = 0;
                    if (DeTransferenciaIngDAL.Get(x => x.Folio == IdTransferencia).Count() > 0 && model.Id_Estatus == 1)
                        afecta = 1;
                    else
                        afecta = 0;
                    Ma_TransferenciasIngModel datamodel = ModelFactory.getModel<Ma_TransferenciasIngModel>(model, new Ma_TransferenciasIngModel());
                    Ma_TransferenciasIngModel maModel = llenar.LLenado_MaTransferenciasIng(IdTransferencia);
                    maModel.PolizaOrdenModificado = StringID.Polizas(datamodel.Id_FolioPO_Modificado, datamodel.Id_MesPO_Modificado);
                    maModel.PolizaOrdenCancelado = StringID.Polizas(datamodel.Id_FolioPO_Modificado_Cancela, datamodel.Id_MesPO_Modificado_Cancela);
                    return Json(new { Exito = true, Mensaje = "OK", Registro = maModel, Afectar = afecta });
                }

                return Json(new { Exito = false, Mensaje = "Registro no encontrado." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }

        }
        [HttpPost]
        public JsonResult GuardarAmpRed(Ma_TransferenciasIng modelo)
        {
            try
            {
                Ca_CierreMensual cierre = cierreMensualDAL.GetByID(x => x.Id_Mes == modelo.Fecha.Value.Month);
                if (!cierre.Contable.Value)
                {
                    if (modelo.Folio == 0)
                    {
                        UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                        modelo.Usu_Act = (short)appUsuario.IdUsuario;
                        modelo.Fecha_Act = DateTime.Now;
                        if (modelo.Id_TipoT == 1)
                            modelo.Importe_AMP = 0;
                        else
                            modelo.Importe_RED = 0;
                        modelo.Id_PptoModificado = true;
                        if (MaTransferenciasIngDAL.Get().Count() == 0)
                            modelo.Folio = 1;
                        else
                            modelo.Folio = MaTransferenciasIngDAL.Get().Max(x => x.Folio) + 1;
                        modelo.Id_Estatus = 1;
                        MaTransferenciasIngDAL.Insert(modelo);
                        MaTransferenciasIngDAL.Save();
                        return Json(new { Exito = true, Mensaje = "Registro guardado correctamente.", Registro = modelo });
                    }
                    else
                    {
                        Ma_TransferenciasIng modeloupdate = MaTransferenciasIngDAL.GetByID(x => x.Folio == modelo.Folio);
                        modeloupdate.Descrip = modelo.Descrip;
                        modeloupdate.Fecha = modelo.Fecha;
                        MaTransferenciasIngDAL.Update(modeloupdate);
                        MaTransferenciasIngDAL.Save();
                        return Json(new { Exito = true, Mensaje = "Registro guardado correctamente.", Registro = modeloupdate });
                    }

                }
                return Json(new { Exito = false, Mensaje = "El mes ya esta cerrado." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }

        }
        [HttpPost]
        public JsonResult EliminarTransferencia(Int32 IdTransferencia, DateTime FechaCancelacion)
        {
            try
            {
                Ma_TransferenciasIng model = MaTransferenciasIngDAL.GetByID(x => x.Folio == IdTransferencia);
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                if (model.Id_Estatus == 1)
                {
                    model.Id_Estatus = 3;
                    MaTransferenciasIngDAL.Update(model);
                    MaTransferenciasIngDAL.Save();
                }
                else
                {

                    model.Id_Estatus = 3;
                    int[] resultado = new int[] { };
                    if (model.Id_TipoT == 1)
                        resultado = proceduresDAL.Pa_Genera_PolizaOrden_Modificado_Amp_Cancela_Ing(IdTransferencia, FechaCancelacion, (short)appUsuario.IdUsuario);
                    else
                        resultado = proceduresDAL.Pa_Genera_PolizaOrden_Modificado_Red_Cancela_Ing(IdTransferencia, FechaCancelacion, (short)appUsuario.IdUsuario);
                    model.Id_MesPO_Modificado_Cancela = Convert.ToByte(resultado[0]);
                    model.Id_FolioPO_Modificado_Cancela = resultado[1]; ;
                    MaTransferenciasIngDAL.Update(model);
                    MaTransferenciasIngDAL.Save();
                    Ma_TransferenciasModel datamodel = ModelFactory.getModel<Ma_TransferenciasModel>(model, new Ma_TransferenciasModel());
                    datamodel.PolizaOrdenModificado = StringID.Polizas(datamodel.Id_FolioPO_Modificado, datamodel.Id_MesPO_Modificado);
                    datamodel.PolizaOrdenCancelado = StringID.Polizas(datamodel.Id_FolioPO_Modificado_Cancela, datamodel.Id_MesPO_Modificado_Cancela);
                    return Json(new { Exito = true, Mensaje = "Registro eliminado correctamente.", Registro = datamodel });

                }
                return Json(new { Exito = true, Mensaje = "Registro no encontrado." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }

        }
        [HttpPost]
        public JsonResult ValidarEliminar(Int32 IdTransferencia)
        {
            try
            {
                Ma_TransferenciasIng model = MaTransferenciasIngDAL.GetByID(x => x.Folio == IdTransferencia);
                if (model.Id_Estatus == 1)
                {

                }
                else
                {
                    if (!proceduresDAL.PA_VerificaCierreTransferencia(IdTransferencia))
                    {
                        if (model.Id_Estatus == 2)//SI YA FUE AFECTADA
                        {
                            if (MaPolizasDAL.Get(x => x.Id_FolioPoliza == model.Id_FolioPO_Modificado && x.Id_MesPoliza == model.Id_MesPO_Modificado && x.Id_TipoPoliza == 4).Count() > 0)//VALIDAR SI SE GENERARON POLIZAS MODIFICACION
                                return Json(new { Exito = true, Mensaje = "No hubo error." });
                            else
                                return Json(new { Exito = false, Mensaje = "No se generaron polizas de modificación." });
                        }
                        return Json(new { Exito = true, Mensaje = "No hubo error." });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = "No se puede eliminar, hay detalles con meses cerrados." });
                }
                return Json(new { Exito = true, Mensaje = "Registro no encontrado." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }

        }
        public ActionResult V_ModalEliminar(Int32 Folio)
        {
            Ma_TransferenciasIng model = MaTransferenciasIngDAL.GetByID(x => x.Folio == Folio);
            DateTime fecha = new DateTime();
            if (model.Id_Estatus == 1)
                fecha = model.Fecha.Value;
            else
                fecha = model.Fecha_Afecta.Value;
            ViewBag.FechaInicio = fecha.ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            ViewBag.id = model.Folio;
            return View();
        }
        [HttpPost]
        public JsonResult ValidarAfectacion(Int32 IdTransferencia)
        {
            try
            {
                if (DeTransferenciaIngDAL.Get(x => x.Folio == IdTransferencia).Count() > 0)
                {
                    if (!proceduresDAL.PA_VerificaCierreTransferencia(IdTransferencia))
                    {
                        return Json(new { Exito = true, Mensaje = "No hubo error." });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = "No se puede afectar, hay detalles con meses cerrados." });
                }
                else
                    return Json(new { Exito = false, Mensaje = "No se puede afectar porque no tiene movimientos." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }

        }
        public ActionResult V_ModalAfectacion(Int32 IdTransferencia, byte IdTipo)
        {
            Ma_TransferenciasIng model = MaTransferenciasIngDAL.GetByID(x => x.Folio == IdTransferencia);
            if (AmpliacionesReduccionesBL.ObtenerTipoMaTransferenciaIng(IdTransferencia) == 1)
                ViewBag.Tipo = "Ampliación";
            else
                ViewBag.Tipo = "Reducción";
            ViewBag.Trans = IdTipo;
            ViewBag.Importe = DeTransferenciaIngDAL.Get(x => x.Folio == IdTransferencia && x.Id_Movimiento == 1).Sum(x => x.Importe).Value.ToString("N");
            ViewBag.FechaInicio = AmpliacionesReduccionesBL.GetFechaMayor(model.Fecha.Value).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            ViewBag.total = DeTransferenciaIngDAL.Get(x => x.Folio == IdTransferencia).Sum(x => x.Importe);
            ViewBag.id = model.Folio;
            return View();
        }
        public ActionResult V_DetalleDeTransferencia(Int32 IdTransferencia, byte origen)
        {
            ViewBag.origen = origen;
            Ma_TransferenciasIngModel dataModal = llenar.LLenado_MaTransferenciasIng(IdTransferencia);
            return View(dataModal);
        }
        [HttpPost]
        public JsonResult GetMesesCerrados()
        {
            IEnumerable<Ca_CierreMensual> meses = cierreMensualDAL.Get(x => x.Contable == true);
            return Json(new { Exito = true, Meses = meses });
        }
        [HttpPost]
        public JsonResult Afectar(Int32 IdTransferencia, DateTime FechaAfectacion)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                Ca_CierreMensual cierre = cierreMensualDAL.GetByID(x => x.Id_Mes == FechaAfectacion.Month);
                if (!cierre.Contable.Value)
                {
                    Ma_TransferenciasIng model = MaTransferenciasIngDAL.GetByID(x => x.Folio == IdTransferencia);
                    int[] resultado = new int[] { };
                    if (model.Id_TipoT == 1)
                        resultado = proceduresDAL.Pa_Genera_PolizaOrden_Modificado_Amp_Ing(IdTransferencia, FechaAfectacion, (short)appUsuario.IdUsuario);
                    else
                        resultado = proceduresDAL.Pa_Genera_PolizaOrden_Modificado_Red_Ing(IdTransferencia, FechaAfectacion, (short)appUsuario.IdUsuario);
                    model.Fecha_Afecta = FechaAfectacion;
                    model.Id_Estatus = 2;
                    model.Id_MesPO_Modificado = Convert.ToByte(resultado[0]);
                    model.Id_FolioPO_Modificado = resultado[1];
                    MaTransferenciasIngDAL.Update(model);
                    MaTransferenciasIngDAL.Save();
                    Ma_TransferenciasIngModel datamodel = ModelFactory.getModel<Ma_TransferenciasIngModel>(model, new Ma_TransferenciasIngModel());
                    datamodel.PolizaOrdenModificado = StringID.Polizas(datamodel.Id_FolioPO_Modificado, datamodel.Id_MesPO_Modificado);
                    datamodel.PolizaOrdenCancelado = StringID.Polizas(datamodel.Id_FolioPO_Modificado_Cancela, datamodel.Id_MesPO_Modificado_Cancela);
                    return Json(new { Exito = true, Mensaje = "Afectado correctamente.", Registro = datamodel });

                }
                return Json(new { Exito = false, Mensaje = "No se puede afectar porque el mes esta cerrado." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        public ActionResult V_TablaDetallesTransferencia(Int32 Folio)
        {
            ViewBag.Estatus = MaTransferenciasIngDAL.GetByID(x => x.Folio == Folio).Id_Estatus;
            ViewBag.ppto = MaTransferenciasIngDAL.GetByID(x => x.Folio == Folio).Id_PptoModificado;
            List<De_TransferenciaIngModel> Lst = new List<De_TransferenciaIngModel>();
            DeTransferenciaIngDAL.Get(x => x.Folio == Folio).ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<De_TransferenciaIngModel>(x, new De_TransferenciaIngModel())); });
            return View(Lst);
        }
        [HttpPost]
        public ActionResult GuardarDeTransferencia(De_TransferenciaIngModel dataModel, De_TransferenciaIng dataTransferencia, De_ClavePresupuestal clave)
        {
            try
            {
                dataTransferencia.Id_ClavePresupuestoIng = StringID.IdClavePresupuestoIngreso(dataModel.Id_CentroRecaudador, dataModel.Id_Fuente, dataModel.AnioFin, dataModel.Id_Alcance, dataModel.Id_Concepto);
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                dataTransferencia.Usu_Act = (short)appUsuario.IdUsuario;
                dataTransferencia.Fecha_Act = DateTime.Now;
                if (DeTransferenciaIngDAL.Get(x => x.Folio == dataTransferencia.Folio).Count() > 0)
                    dataTransferencia.IdRegistro = Convert.ToByte(DeTransferenciaIngDAL.Get(x => x.Folio == dataTransferencia.Folio).Max(x => x.IdRegistro) + 1);
                else
                    dataTransferencia.IdRegistro = 1;
                DeTransferenciaIngDAL.Insert(dataTransferencia);
                DeTransferenciaIngDAL.Save();
                return Json(new { Exito = true, Mensaje = "Guardado correctamente.", Registro = dataTransferencia });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        [HttpPost]
        public JsonResult getDetalleTransferencia(Int32 Folio, Int32 Id_Consecutivo)
        {
            try
            {
                De_TransferenciaIngModel dataModal = llenar.LLenado_DeTransferenciasIng(Folio, Id_Consecutivo);
                return Json(new { Exito = true, Mensaje = "OK", Registro = dataModal });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje, Registro = new De_PolizasModel() });
            }
        }
        [HttpPost]
        public JsonResult EliminarDeTransferencia(Int32 Folio, Int32 Id_Consecutivo)
        {
            try
            {
                DeTransferenciaIngDAL.Delete(x => x.Folio == Folio && x.IdRegistro == Id_Consecutivo);
                DeTransferenciaIngDAL.Save();
                return Json(new { Exito = true, Mensaje = "Eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje, Registro = new De_PolizasModel() });
            }
        }
        #endregion

        [HttpGet]
        public ActionResult Evolucion()
        {
            DE_EvolucionModel model = new DE_EvolucionModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult tblEvolucion(DE_EvolucionModel dataModel)
        {
            List<DE_Evolucion> lista = DALEvolucion.Get(x => (!String.IsNullOrEmpty(dataModel.Id_CentroRecaudador) ? x.Id_CentroRecaudador == dataModel.Id_CentroRecaudador : x.Id_CentroRecaudador != null) &&
                (!String.IsNullOrEmpty(dataModel.Id_Fuente) ? x.Id_Fuente == dataModel.Id_Fuente : x.Id_CentroRecaudador != null) &&
                (!String.IsNullOrEmpty(dataModel.AnioFin) ? x.AnioFin == dataModel.AnioFin : x.Id_CentroRecaudador != null) &&
                (!String.IsNullOrEmpty(dataModel.Id_Concepto) ? x.Id_Concepto == dataModel.Id_Concepto : x.Id_CentroRecaudador != null)).ToList();
            List<EvolucionIngresos> listaEvolucion = new List<EvolucionIngresos>();
            for (int i = 1; i < 13; i++)
            {
                EvolucionIngresos temp = new EvolucionIngresos();
                temp.Estimado = lista.Where(x => x.Mes == i).Sum(x => x.Estimado).Value;
                temp.Ampliaciones = lista.Where(x => x.Mes == i).Sum(x => x.Ampliaciones).Value;
                temp.Reducciones = lista.Where(x => x.Mes == i).Sum(x => x.Reducciones).Value;
                temp.Modificado = lista.Where(x => x.Mes == i).Sum(x => x.Modificado).Value;
                temp.Devengado = lista.Where(x => x.Mes == i).Sum(x => x.Devengado).Value;
                temp.Recaudado = lista.Where(x => x.Mes == i).Sum(x => x.Recaudado).Value;
                temp.PorEjecutar = lista.Where(x => x.Mes == i).Sum(x => x.PorEjecutar).Value;
                temp.Mes = Diccionarios.Meses[i];
                listaEvolucion.Add(temp);
            }
            return View(listaEvolucion);
        }

        #region ReciboIngresos
        public ActionResult ReciboIngresos()
        {
            Ma_ReciboIngresosModel dataModal = new Ma_ReciboIngresosModel();
            dataModal.ListaCa_CajasReceptoras = _listas.ListaCajasReceptoras();
            dataModal.ListaId_Banco = _listas.ListaBancos();
            dataModal.ListaId_CtaBancaria = _listas.ListaCtaBancarias(null);
            return View(dataModal);
        }

        [HttpPost]
        public ActionResult ReciboIngresos(int Folio)
        {
            Ma_ReciboIngresosModel dataModal = llenar.LLenado_MaRecibosIngresos(Folio);
            dataModal.Botonera = reciboBL.createBotonera(dataModal);
            dataModal.ListaCa_CajasReceptoras = _listas.ListaCajasReceptoras();
            return View(dataModal);
        }

        public ActionResult newReciboIngresos()
        {
            Ma_ReciboIngresosModel dataModel = new Ma_ReciboIngresosModel();
            UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
            dataModel.ListaCa_CajasReceptoras = _listas.ListaCajasReceptoras();
            dataModel.Usuario_Captura = appUsuario.NombreCompleto;
            dataModel.cfechas = new Control_Fechas();
            dataModel.Fecha = DateTime.Now;
            dataModel.IdEstatus = Diccionarios.Valores_EstatusRecibos.REGISTRADO;
            dataModel.EstatusDescipcion = Diccionarios.EstatusRecibos[Diccionarios.Valores_EstatusRecibos.REGISTRADO];
            dataModel.cfechas = new Control_Fechas();
            dataModel.ListaId_Banco = _listas.ListaBancos();
            return Json(new { Registro = dataModel, fMax = dataModel.cfechas.Fecha_Max.ToShortDateString(), fMin = dataModel.cfechas.Fecha_Min.ToShortDateString() });
        }

        [HttpPost]
        public JsonResult searchContribuyentes(string name)
        {
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            var Contribuyentes = personasDAL.Get(x => x.Nombre.Contains(name) || x.ApellidoMaterno.Contains(name) || x.ApellidoPaterno.Contains(name) || x.RazonSocial.Contains(name));
            foreach (var item in Contribuyentes)
            {
                if (item.PersonaFisica.HasValue && item.PersonaFisica.Value == true)
                    dataModel.Add(String.Format("{0}-{1}", String.Format("{0} {1} {2}", item.Nombre, item.ApellidoPaterno, item.ApellidoMaterno), item.IdPersona));
                else
                    dataModel.Add(String.Format("{0}-{1}", item.RazonSocial, item.IdPersona));
                dataIds.Add(item.IdPersona.ToString());
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public JsonResult getContribuyenteData(int IdPersona)
        {
            Ca_PersonasModel persona = llenar.Llenado_CaPersonas(IdPersona);
            if (!persona.Resultado)
            {
                return Json(new { Exito = false, Mensaje = "El contribuyente no tiene datos de domicilio. Revisar.", persona });

            }
            else
            {
                persona.Domicilio = String.Format("{0} {1} {2}, C.P: {3} {4}, {5},{6}", persona.Ca_CallesModel.Descripcion, persona.NumeroExt, persona.NumeroInt, persona.Colonia, persona.CP, persona.Ca_MunicipiosModel.Descripcion, persona.Ca_LocalidadesModel.Descripcion, persona.Ca_EstadosModel.Descripcion);
                return Json(new { Exito = true, Mensaje = " OK", persona });

            }
        }

        [HttpPost]
        public ActionResult GuardarRecibo(Ma_ReciboIngresos dataModel, Ma_ReciboIngresosModel dataModelo)
        {
            try
            {
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                dataModel.uAct = (short)appUsuario.IdUsuario;
                dataModel.fAct = DateTime.Now;
                bool nuevo = false;
                if (dataModel.Folio == 0)
                {
                    dataModel.Folio = reciboBL.nextFolio();
                    _MaRecibosDAL.Insert(dataModel);
                    nuevo = true;
                }
                else
                    _MaRecibosDAL.Update(dataModel);
                _MaRecibosDAL.Save();
                dataModel.Usuario_Captura = appUsuario.NombreCompleto;
                if (nuevo)
                {
                    if (reciboBL.setNextFolio())
                    {
                        dataModelo.Botonera = reciboBL.createBotonera(dataModelo);
                        dataModelo.Folio = dataModel.Folio;
                        return Json(new { Exito = true, Mensaje = "OK", Registro = dataModelo });
                    }
                    else
                        return Json(new { Exito = true, Mensaje = "No se guardo el ultimo Folio", Registro = dataModel });
                }
                else
                {
                    dataModelo.Botonera = reciboBL.createBotonera(dataModelo);
                    dataModelo.Folio = dataModel.Folio;
                    return Json(new { Exito = true, Mensaje = "OK", Registro = dataModelo });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpGet]
        public ActionResult BuscarRecibo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult tblResultadosRecibos(int? Folio, string NombreContribuyente)
        {
            try
            {
                IEnumerable<Ma_ReciboIngresos> recibos;
                if (Folio.HasValue)
                    recibos = _MaRecibosDAL.Get(x => x.Folio == Folio);
                else
                {
                    List<int> personas = personasDAL.Get(x => x.Nombre.Contains(NombreContribuyente) || x.ApellidoMaterno.Contains(NombreContribuyente) || x.ApellidoPaterno.Contains(NombreContribuyente) || x.RazonSocial.Contains(NombreContribuyente)).GroupBy(x => x.IdPersona).Select(x => x.Key).ToList();
                    recibos = _MaRecibosDAL.Get(r => personas.Contains(r.IdContribuyente));
                }
                return View(recibos);
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult getRecibo(int IdRecibo)
        {
            try
            {
                Ma_ReciboIngresosModel dataModel = llenar.LLenado_MaRecibosIngresos(IdRecibo);
                dataModel.Botonera = reciboBL.createBotonera(dataModel);
                dataModel.cfechas = new Control_Fechas();
                return Json(new { Exito = true, Mensaje = "OK", Registro = dataModel, fMax = dataModel.cfechas.Fecha_Max.ToShortDateString(), fMin = dataModel.cfechas.Fecha_Min.ToShortDateString() });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult DetallesReciboIngresos(int IdRecibo)
        {
            Ma_ReciboIngresosModel dataModel = llenar.LLenado_MaRecibosIngresos(IdRecibo);
            dataModel.Botonera = reciboBL.createDeBotonera(_MaRecibosDAL.GetByID(x => x.Folio == IdRecibo));
            return View(dataModel);
        }

        [HttpPost]
        public ActionResult TablaDetallesRecibos(int Folio)
        {
            List<De_ReciboIngresosModel> deRecibos = new List<De_ReciboIngresosModel>();
            _DeRecibosDAL.Get(x => x.Folio == Folio).ToList().ForEach(x => deRecibos.Add(llenar.LLenado_DeRecibos(x.Folio, x.IdRegistro)));
            return View(deRecibos);
        }

        [HttpPost]
        public ActionResult GuardarDetalleRecibo(De_ReciboIngresosModel dataModelo, De_ReciboIngresos dataEntity)
        {
            try
            {
                UsuarioLogueado appUSuario = Logueo.GetUsrLogueado();
                dataEntity.uAct = (short)appUSuario.IdUsuario;
                dataEntity.fAct = DateTime.Now;
                if (!String.IsNullOrEmpty(dataModelo.Id_CentroRecaudador))
                    dataEntity.Id_ClavePresupuestoIng = StringID.IdClavePresupuestoIngreso(dataModelo.Id_CentroRecaudador, dataModelo.Id_Fuente, dataModelo.AnioFin, dataModelo.Id_Alcance, dataModelo.Id_Concepto);
                if (dataEntity.IdRegistro == 0)
                    _DeRecibosDAL.Insert(dataEntity);
                else
                    _DeRecibosDAL.Update(dataEntity);
                _DeRecibosDAL.Save();
                dataModelo.Id_ClavePresupuestoIng = dataEntity.Id_ClavePresupuestoIng;
                dataModelo.Ca_Cuentas = llenar.LLenado_CaCuentas(dataModelo.Id_Cuenta);
                dataModelo.Botonera = reciboBL.createDeBotonera(_MaRecibosDAL.GetByID(x => x.Folio == dataModelo.Folio));
                return Json(new { Exito = true, Mensaje = "OK", Registro = dataModelo });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }


        [HttpPost]
        public ActionResult getDetalleRecibo(int Folio, int IdRegistro)
        {
            try
            {
                De_ReciboIngresosModel dataModel = llenar.LLenado_DeRecibos(Folio, IdRegistro);
                dataModel.Botonera = reciboBL.createDeBotonera(_MaRecibosDAL.GetByID(x => x.Folio == Folio));
                return Json(new { Exito = true, Mensaje = "OK", Registro = dataModel });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult EliminarDeRecibo(int Folio, int Registro)
        {
            try
            {
                _DeRecibosDAL.Delete(x => x.Folio == Folio && x.IdRegistro == Registro);
                _DeRecibosDAL.Save();
                return Json(new { Exito = true, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        public ActionResult CancelarRecibo(String FechaCancelacion)
        {
            try
            {
                Control_Fechas fechas = new Control_Fechas();
                DateTime fminima = Convert.ToDateTime(FechaCancelacion);
                if (fminima > fechas.Fecha_Min)
                    fechas.Fecha_Min = fminima;
                //fechas.Fecha_Min = Convert.ToDateTime(FechaCancelacion);
                return View(fechas);
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult CancelarRecibo(int Folio, DateTime FechaCancelacion)
        {
            try
            {
                Ma_ReciboIngresos recibo = _MaRecibosDAL.GetByID(x => x.Folio == Folio);
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                if (compromisosBL.isClosed(FechaCancelacion))
                    return Json(new { Exito = false, Mensaje = "Ese mes está cerrado" });
                byte Mes = 0; int folio = 0;
                if (recibo.IdFolioPDevengado != null)
                {
                    _StoredProcedure.Pa_Genera_PolizaOrden_DevengadoIng_Cancela(recibo.Folio, FechaCancelacion, (short)appUsuario.IdUsuario, ref Mes, ref folio);
                    recibo.IdFolioPDevengadoC = folio;
                    recibo.IdMesPDevengadoC = Mes;
                    recibo.IdTipoPDevengadoC = 4;
                    //_StoredProcedure.PA_Genera_Polizas_DiarioIngresos_Cancela(recibo.Folio, (short)appUsuario.IdUsuario, FechaCancelacion, ref Mes, ref folio);
                    //recibo.Id_MesPDiarioC = Mes;
                    //recibo.Id_FolioPDiarioC = folio;
                    //recibo.Id_TipoPDiarioC = 3;
                }
                if (recibo.IdFolioPRecaudado != null && recibo.Id_FolioPIngresos != null)
                {
                    _StoredProcedure.Pa_Genera_PolizaOrden_Recaudado_Cancela(recibo.Folio, FechaCancelacion, (short)appUsuario.IdUsuario, ref Mes, ref folio);
                    recibo.IdFolioPRecaudadoC = folio;
                    recibo.IdMesPRecaudadoC = Mes;
                    recibo.IdTipoPRecaudadoC = 4;
                    _StoredProcedure.PA_Genera_Polizas_CANCELA_ReciboIng(recibo.Folio, (short)appUsuario.IdUsuario, FechaCancelacion, ref Mes, ref folio);
                    recibo.Id_MesPDiarioC = Mes;
                    recibo.Id_FolioPDiarioC = folio;
                    recibo.Id_TipoPDiarioC = 3;
                }
                recibo.IdEstatus = Diccionarios.Valores_EstatusRecibos.CANCELADO;
                recibo.fAct = DateTime.Now;
                recibo.uAct = (short)appUsuario.IdUsuario;
                recibo.FechaCancelacion = FechaCancelacion;
                recibo.Usuario_Cancelacion = appUsuario.NombreCompleto;
                _MaRecibosDAL.Update(recibo);
                _MaRecibosDAL.Save();
                Ma_ReciboIngresosModel modelo = llenar.LLenado_MaRecibosIngresos(Folio);
                modelo.Botonera = reciboBL.createBotonera(modelo);
                return Json(new { Exito = true, Mensaje = "OK", Registro = modelo });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }


        public ActionResult ImprimirRecibo(String FechaRecaudacion)
        {
            try
            {
                Control_Fechas fechas = new Control_Fechas();
                DateTime fminima = Convert.ToDateTime(FechaRecaudacion);
                if (fminima > fechas.Fecha_Min)
                    fechas.Fecha_Min = fminima;
                //fechas.Fecha_Min = Convert.ToDateTime(FechaRecaudacion);
                return View(fechas);
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult ImprimirRecibo(int Folio, DateTime FechaRecaudacion)
        {
            try
            {
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                if (compromisosBL.isClosed(FechaRecaudacion))
                    return Json(new { Exito = false, Mensaje = "Ese mes está cerrado" });
                Ma_ReciboIngresos recibo = _MaRecibosDAL.GetByID(x => x.Folio == Folio);
                byte Mes = 0; int folio = 0;
                _StoredProcedure.Pa_Genera_PolizaOrden_DevengadoIng(recibo.Folio, FechaRecaudacion, (short)appUsuario.IdUsuario, ref Mes, ref folio);
                recibo.IdFolioPDevengado = folio;
                recibo.IdMesPDevengado = Mes;
                recibo.IdTipoPDevengado = 4;
                /*_StoredProcedure.Pa_Genera_PolizaOrden_Recaudado(recibo.Folio, FechaRecaudacion, (short)appUsuario.IdUsuario, ref Mes, ref folio);
                recibo.IdFolioPRecaudado = folio;
                recibo.IdMesPRecaudado = Mes;
                recibo.IdTipoPRecaudado = 4;
                _StoredProcedure.PA_Genera_PolizaIngresos(recibo.Folio, (short)appUsuario.IdUsuario,FechaRecaudacion, ref Mes, ref folio);
                recibo.Id_FolioPIngresos = folio;
                recibo.Id_MesPIngresos = Mes;
                recibo.Id_TipoPIngresos = 1;
                _StoredProcedure.PA_Genera_Polizas_DiarioIngresos(recibo.Folio, (short)appUsuario.IdUsuario,FechaRecaudacion, ref Mes, ref folio);
                recibo.Id_MesPDiario = Mes;
                recibo.Id_FolioPDiario = folio;
                recibo.Id_TipoPDiario = 3;*/
                recibo.IdEstatus = Diccionarios.Valores_EstatusRecibos.DEVENGADO;
                recibo.Impreso = true;
                //recibo.FechaRecaudacion = FechaRecaudacion;
                recibo.Importe = recibo.De_ReciboIngresos.Where(x => x.Id_Movimiento == Diccionarios.ValorMovimientos.ABONO).Sum(s => s.Importe);
                //recibo.Usuario_Recaudacion = appUsuario.NombreCompleto;
                recibo.fAct = DateTime.Now;
                recibo.uAct = (short)appUsuario.IdUsuario;
                _MaRecibosDAL.Update(recibo);
                _MaRecibosDAL.Save();
                Ma_ReciboIngresosModel dataModel = llenar.LLenado_MaRecibosIngresos(Folio);
                dataModel.Botonera = reciboBL.createBotonera(dataModel);
                return Json(new { Exito = true, Mensaje = "OK", Registro = dataModel });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult RecaudarRecibo(int Folio, DateTime FechaRecaudacion)
        {
            try
            {
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                if (compromisosBL.isClosed(FechaRecaudacion))
                    return Json(new { Exito = false, Mensaje = "Ese mes está cerrado" });
                Ma_ReciboIngresos recibo = _MaRecibosDAL.GetByID(x => x.Folio == Folio);
                byte Mes = 0; int folio = 0;
                _StoredProcedure.Pa_Genera_PolizaOrden_DevengadoIng(recibo.Folio, FechaRecaudacion, (short)appUsuario.IdUsuario, ref Mes, ref folio);
                /*recibo.IdFolioPDevengado = folio;
                recibo.IdMesPDevengado = Mes;
                recibo.IdTipoPDevengado = 4;*/
                _StoredProcedure.Pa_Genera_PolizaOrden_Recaudado(recibo.Folio, FechaRecaudacion, (short)appUsuario.IdUsuario, ref Mes, ref folio);
                recibo.IdFolioPRecaudado = folio;
                recibo.IdMesPRecaudado = Mes;
                recibo.IdTipoPRecaudado = 4;
                _StoredProcedure.PA_Genera_PolizaIngresos(recibo.Folio, (short)appUsuario.IdUsuario, FechaRecaudacion, ref Mes, ref folio);
                recibo.Id_FolioPIngresos = folio;
                recibo.Id_MesPIngresos = Mes;
                recibo.Id_TipoPIngresos = 1;
                _StoredProcedure.PA_Genera_Polizas_DiarioIngresos(recibo.Folio, (short)appUsuario.IdUsuario, FechaRecaudacion, ref Mes, ref folio);
                recibo.Id_MesPDiario = Mes;
                recibo.Id_FolioPDiario = folio;
                recibo.Id_TipoPDiario = 3;
                recibo.IdEstatus = Diccionarios.Valores_EstatusRecibos.RECAUDADO;
                recibo.FechaRecaudacion = FechaRecaudacion;
                recibo.Importe = recibo.De_ReciboIngresos.Where(x => x.Id_Movimiento == Diccionarios.ValorMovimientos.ABONO).Sum(s => s.Importe);
                recibo.Usuario_Recaudacion = appUsuario.NombreCompleto;
                recibo.fAct = DateTime.Now;
                recibo.uAct = (short)appUsuario.IdUsuario;
                _MaRecibosDAL.Update(recibo);
                _MaRecibosDAL.Save();
                Ma_ReciboIngresosModel dataModel = llenar.LLenado_MaRecibosIngresos(Folio);
                dataModel.Botonera = reciboBL.createBotonera(dataModel);
                return Json(new { Exito = true, Mensaje = "OK", Registro = dataModel });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        public ActionResult ReporteReciboIngresos(int Folio)
        {
            Ma_ReciboIngresosModel recibo = llenar.LLenado_MaRecibosIngresos(Folio);
            ConvertHtmlToString pdf = new ConvertHtmlToString();
            return File(pdf.GenerarPDF_Blanco("ReporteReciboIngresos", recibo, this.ControllerContext), "application/pdf");
        }

        #endregion
        #region Analítico de Ingresos
        public ActionResult AnalisisIngresos()
        {
            return View(new BusquedaAnaliticoIngresos());
        }
        [HttpPost]
        public ActionResult BuscarIngresos(BusquedaAnaliticoIngresos busqueda)
        {
            return View(new IngresosBL().BusquedaReciboIngresos(busqueda));
        }
        public ActionResult ReporteIngresos(BusquedaAnaliticoIngresos busqueda)
        {
            List<VW_DetalleReciboIngModel> lista = new IngresosBL().BusquedaReciboIngresos(busqueda);
            return File(reports.GenerarPDF("ReporteIngresos", lista, this.ControllerContext), "Application/PDF");
        }
        #endregion
        #region Corte de Caja
        public ActionResult CorteCaja()
        {
            return View(new BusquedaCorteCaja());
        }
        [HttpPost]
        public ActionResult BuscarCorteCaja(BusquedaCorteCaja busqueda)
        {
            return View(new IngresosBL().BusquedaCorteCaja(busqueda));
        }
        [HttpGet]
        public ActionResult ReporteCorteCaja(BusquedaCorteCaja busqueda, String FechaDesdeRecaudacion, String FechaHastaRecaudacion)
        {
            if (!String.IsNullOrEmpty(FechaDesdeRecaudacion))
                busqueda.FechaDesdeRecaudacion = Convert.ToDateTime(FechaDesdeRecaudacion);
            if (!String.IsNullOrEmpty(FechaHastaRecaudacion))
                busqueda.FechaHastaRecaudacion = Convert.ToDateTime(FechaHastaRecaudacion);
            ViewBag.FechaInicio = busqueda.FechaDesdeRecaudacion.HasValue ? busqueda.FechaDesdeRecaudacion.Value.ToShortDateString() : "";
            ViewBag.FechaFin = busqueda.FechaHastaRecaudacion.HasValue ? busqueda.FechaHastaRecaudacion.Value.ToShortDateString() : "";
            ViewBag.CajaReceptora = busqueda.CajaReceptora > 0 ? new CaCajasReceptorasDAL().GetByID(x => x.Id_CajaR == busqueda.CajaReceptora).Descripcion : "";
            List<VW_DetalleReciboIngModel> lista = new IngresosBL().BusquedaCorteCaja(busqueda);
            List<ReporteCorteCaja> reporte = new IngresosBL().ReporteCaja(lista);
            return File(reports.GenerarPDF("ReporteCorteCaja", reporte, this.ControllerContext), "Application/PDF");
        }

        #endregion
    }
}
