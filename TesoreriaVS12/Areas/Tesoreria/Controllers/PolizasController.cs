using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Utils;
using TesoreriaVS12.Models;
using TesoreriaVS12.Filters;
using TesoreriaVS12.Areas.Tesoreria.BL;

namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    [AuthorizeLogin]
    public class PolizasController : Controller
    {
        protected ClasificaPolizaDAL DALclasificacionpolizas { get; set; }
        protected MaPolizasDAL DALMaPolizas { get; set; }
        private ParametrosDAL DALParametros { get; set; }
        protected TipoPolizasDAL DALtipopolizas { get; set; }
        protected DePolizasDAL DALdepolizas { get; set; }
        protected CuentasDAL DALcuentas { get; set; }
        protected Listas repoListas { get; set; }
        protected Llenado llenar { get; set; }
        protected TipoMovBancariosDAL movBancDal { get; private set; }
        private ContrarecibosBL foliador { get; set; }
        private PolizasBL polizasBL { get; set; }
        private ProceduresDAL _store { get; set; }
        private DeCompromisosBL compromisosBL { get; set; }
        ClasificaPolizaDAL clasificacionpolizas { get; set; }
        protected ConvertHtmlToString reports { get; set; }

        public PolizasController()
        {
            if (DALMaPolizas == null) DALMaPolizas = new MaPolizasDAL();
            if (DALtipopolizas == null) DALtipopolizas = new TipoPolizasDAL();
            if (DALclasificacionpolizas == null) DALclasificacionpolizas = new ClasificaPolizaDAL();
            if (DALdepolizas == null) DALdepolizas = new DePolizasDAL();
            if (DALcuentas == null) DALcuentas = new CuentasDAL();
            if (repoListas == null) repoListas = new Listas();
            if (llenar == null) llenar = new Llenado();
            if (movBancDal == null) movBancDal = new TipoMovBancariosDAL();
            if (foliador == null) foliador = new ContrarecibosBL();
            if (polizasBL == null) polizasBL = new PolizasBL();
            if (_store == null) _store = new ProceduresDAL();
            if (compromisosBL == null) compromisosBL = new DeCompromisosBL();
            if (DALParametros == null) DALParametros = new ParametrosDAL();
            if (clasificacionpolizas == null) clasificacionpolizas = new ClasificaPolizaDAL();
            if (reports == null) reports = new ConvertHtmlToString();
        }
        //
        // GET: /Tesoreria/Polizas/

        [PermisosFilter]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult V_Polizas()
        {
            List<byte> listatipo = new List<byte>();
            new ParametrosDAL().GetByID(x=> x.Nombre == "Capturar_TipoPoliza").Valor.Split(',').ToList().ForEach(aux=> listatipo.Add(Convert.ToByte(aux)));
            ViewBag.tipo = DALtipopolizas.Get(x=> listatipo.Contains(x.Id_TipoPoliza));
            Ma_PolizasModel model = new Ma_PolizasModel();
            model.Fecha = DateTime.Now;
            model.Botonera = new List<object>() { "bNuevo", "bBuscar", "bSalir" };
            model.ListaId_ClasPoliza = new SelectList(clasificacionpolizas.Get(x => x.Id_SubClasificaPoliza == 0 && x.Id_ClasificaPoliza != 0), "Id_ClasificaPoliza", "Descripcion");
            model.ListaId_SubClasificaPol = new SelectList(clasificacionpolizas.Get(x => x.Id_SubClasificaPoliza != 0 && x.Id_ClasificaPoliza == model.Id_ClasPoliza), "Id_SubClasificaPoliza", "Descripcion");
            model.TipoPoliza = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult V_Polizas(byte TipoPoliza = 0, byte MesPoliza = 0, int FolioPoliza = 0, params string[] args)
        {
            List<byte> listatipo = new List<byte>();
            new ParametrosDAL().GetByID(x => x.Nombre == "Capturar_TipoPoliza").Valor.Split(',').ToList().ForEach(aux => listatipo.Add(Convert.ToByte(aux)));
            ViewBag.tipo = DALtipopolizas.Get(x => listatipo.Contains(x.Id_TipoPoliza));
            Ma_PolizasModel model = llenar.LLenado_MaPolizas(TipoPoliza, FolioPoliza, MesPoliza);
            model.Botonera = polizasBL.createBotonera(model);
            return View(model);
        }

        [HttpPost]
        public JsonResult GetClasi(Int16? IdTipo, bool Manuales)
        {
            return new JsonResult() { Data = repoListas.ListaClasi(IdTipo, Manuales) };
        }
        [HttpPost]
        public JsonResult GetSub(Int16? IdTipo, Int16? IdClasi)
        {
            return new JsonResult() { Data = repoListas.ListaSub(IdTipo,IdClasi) };
        }
        [HttpPost]
        public JsonResult ValidarClasificacion(Int16? IdTipo, Int16? IdClasi)
        {
            try
            {
                Ca_ClasificaPolizas poliza = DALclasificacionpolizas.GetByID(x => x.Id_TipoPoliza == IdTipo && x.Id_ClasificaPoliza == IdClasi && x.Id_SubClasificaPoliza == 0);
                if(poliza.Automatica)
                    return Json(new { Exito = false, Mensaje = "No se puede capturar Pólizas Automáticas" });
                else
                    return Json(new { Exito = true, Mensaje = "No Ocurrió un error" });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GuardarPoliza(Ma_Polizas modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            Ca_CierreMensualModel Model = new Ca_CierreMensualModel();
            Ma_PolizasModel PolizaModel = new Ma_PolizasModel();
            if (ModelState.IsValid)
            {
                try
                {
                    if (modelo.Id_FolioPoliza > 0)
                    {
                        DALMaPolizas.Update(modelo);
                        DALMaPolizas.Save();
                        return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = modelo });
                    }
                    else
                    {
                        if (!Model.validaMes(modelo.Id_MesPoliza))
                        {
                            if (!PolizaModel.ValidaAutomatica(modelo.Id_TipoPoliza, modelo.Id_ClasPoliza.Value, modelo.Id_SubClasificaPol.Value))
                            {
                                modelo.Descripcion = modelo.Descripcion.Trim();
                                modelo.Id_FolioPoliza = PolizaModel.ObtenerFolio(modelo.Id_TipoPoliza, modelo.Id_MesPoliza) + 1;
                                modelo.Usuario_Act = (Int16)appUsuario.IdUsuario;
                                modelo.Fecha_Act = DateTime.Now;
                                DALMaPolizas.Insert(modelo);
                                DALMaPolizas.Save();
                                return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = modelo });

                            }
                            else
                                return Json(new { Exito = false, Mensaje = " Error al guardar la póliza, su clasificación es automática." });
                        }
                        return Json(new { Exito = false, Mensaje = "Ocurrió un error" });
                    }
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        public ActionResult BuscarPoliza()
        {
            ViewBag.tipo = DALtipopolizas.Get();
            return View(new Ma_PolizasModel());
        }
        [HttpPost]
        public ActionResult TablaPoliza(Ma_Polizas modelo)
        {
             IEnumerable<Ma_Polizas> entities = DALMaPolizas.Get(x=> 
                (modelo.Id_TipoPoliza > 0 ? x.Id_TipoPoliza==modelo.Id_TipoPoliza : x.Id_TipoPoliza != null) &&
                (modelo.Id_ClasPoliza > 0 ? x.Id_ClasPoliza == modelo.Id_ClasPoliza : x.Id_ClasPoliza != null) &&
                (modelo.Id_SubClasificaPol != null ? x.Id_SubClasificaPol==modelo.Id_SubClasificaPol : x.Id_SubClasificaPol != null) &&
                (modelo.Id_FolioPoliza >0 ? x.Id_FolioPoliza == modelo.Id_FolioPoliza : x.Id_FolioPoliza != null) &&
                (modelo.Cargos != null ? x.Cargos == modelo.Cargos : x.Cargos != -1 || x.Cargos == null) &&
                (modelo.Id_MesPoliza > 0 ? x.Id_MesPoliza == modelo.Id_MesPoliza : x.Id_MesPoliza != null) &&
                (modelo.Fecha > DateTime.MinValue ? x.Fecha == modelo.Fecha : x.Fecha != null) &&
                (modelo.Descripcion != null ? x.Descripcion.Contains(modelo.Descripcion) : x.Descripcion != null)
                );
             List<Ma_PolizasModel>Lst = new List<Ma_PolizasModel>();
             foreach (Ma_Polizas item in entities)
             {
                 Ma_PolizasModel model = ModelFactory.getModel<Ma_PolizasModel>(item, new Ma_PolizasModel());
                 model.Ca_TipoPolizas = ModelFactory.getModel<Ca_TipoPolizasModel>(DALtipopolizas.GetByID(x => x.Id_TipoPoliza == model.Id_TipoPoliza), new Ca_TipoPolizasModel());
                 Lst.Add(model);
             }
             return View(Lst);
        }
        [HttpPost]
        public JsonResult SeleccionarPoliza(byte IdTipo, Int16 IdFolio, byte IdMes)
        {
            try
            {
                Ma_PolizasModel model = llenar.LLenado_MaPolizas(IdTipo, IdFolio, IdMes);
                model.Botonera = polizasBL.createBotonera(model);
                return Json(new { Exito = true, Mensaje = "No Ocurrió un error", Registro = model,Fecha=model.Fecha.ToString("dd/MM/yyyy").ToString() });
                    
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult TablaDetallesPoliza(Int16 IdTipo, Int16 IdFolio, Int16 IdMes)
        {
            try
            {
                IEnumerable<De_Polizas> entities = DALdepolizas.Get(x => x.Id_TipoPoliza == IdTipo && x.Id_FolioPoliza == IdFolio && x.Id_MesPoliza == IdMes);
                List<De_PolizasModel> Lst = new List<De_PolizasModel>();
                foreach (De_Polizas item in entities)
                {
                    De_PolizasModel model = ModelFactory.getModel<De_PolizasModel>(item, new De_PolizasModel());
                    model.Ca_Cuentas = ModelFactory.getModel<Ca_CuentasModel>(DALcuentas.GetByID(x => x.Id_Cuenta == model.Id_Cuenta), new Ca_CuentasModel());
                    Lst.Add(model);
                }
                return View(Lst);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DetalleDePolizas(Byte IdTipo, Int16 IdFolio, Byte IdMes)
        {
            Ma_PolizasModel dataModal = llenar.LLenado_MaPolizas(IdTipo, IdFolio, IdMes);
            dataModal.Botonera = polizasBL.createDeBotonera(dataModal);
            return View(dataModal);
        }

        [HttpPost]
        public JsonResult getDetalle(Byte IdTipo, Int16 IdFolio, Byte IdMes, Int32 IdRegistro)
        {
            try
            {
                De_PolizasModel dataModal = llenar.LLenado_DePolizas(IdTipo, IdFolio, IdMes, IdRegistro);
                Byte Tipo = 0;
                if (dataModal.Id_ClavePresupuesto != null && dataModal.Id_ClavePresupuestoIng == null)
                    Tipo = 2;
                else if (dataModal.Id_ClavePresupuesto == null && dataModal.Id_ClavePresupuestoIng != null)
                    Tipo = 1;
                else if (dataModal.Id_ClavePresupuesto == null && dataModal.Id_ClavePresupuestoIng == null)
                    Tipo = 3;
                return Json(new { Exito = true, Mensaje = "OK", Registro = dataModal, Tipo = Tipo });

            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje, Registro = new De_PolizasModel() });
            }
        }

        [HttpPost]
        public JsonResult ListTipoMovBancario(Byte TipoMov)
        {
            return new JsonResult() { Data = new SelectList(movBancDal.Get(x => x.Id_TipoMovB == TipoMov), "Id_FolioMovB", "Descripcion") };
        }

        [HttpPost]
        public ActionResult GuardarDePoliza(De_PolizasModel dataModel, De_Polizas dataPoliza, De_ClavePresupuestal clave)
        {
            try
            {
                if (dataModel.Id_ObjetoG != null)
                    dataPoliza.Id_ClavePresupuesto = StringID.IdClavePresupuesto(dataModel.Id_Area, dataModel.Id_Funcion, dataModel.Id_Actividad, dataModel.Id_ClasificacionP, dataModel.Id_Programa, dataModel.Id_Proceso, dataModel.Id_TipoMeta, dataModel.Id_ActividadMIR, dataModel.Id_Accion, dataModel.Id_Alcance, dataModel.Id_TipoG, dataModel.Id_Fuente, dataModel.AnioFin, dataModel.Id_ObjetoG);
                else if (dataModel.Id_Concepto != null)
                    dataPoliza.Id_ClavePresupuestoIng = StringID.IdClavePresupuestoIngreso(dataModel.Id_CentroRecaudador, dataModel.Id_Fuente, dataModel.AnioFin, dataModel.Id_Alcance, dataModel.Id_Concepto);

                if(!String.IsNullOrEmpty(dataPoliza.Id_ClavePresupuesto))
                {
                    switch(dataModel.Id_Movimiento)
                    {
                        case Diccionarios.ValorMovimientos.CARGO:
                            if(!compromisosBL.hasDisponibilidad(dataPoliza.Id_ClavePresupuesto,dataModel.Importe.Value,dataModel.Fecha) && Convert.ToBoolean(DALParametros.GetByID(reg => reg.Nombre == "Sin_Disponibilidad").Valor) == false)
                                return Json(new { Exito = false, hasDisponibilidad = false, Mensaje = "Esta clave presupuestal no tiene disponibilidad, ¿Aun asi deseas guardar?" });
                            break;
                        case Diccionarios.ValorMovimientos.ABONO:
                            decimal pagado = polizasBL.hasPagado(dataPoliza.Id_ClavePresupuesto, dataPoliza.Importe.Value, dataPoliza.Fecha);
                            if (pagado < 0)
                                return Json(new { Exito = false, hasPagado = false, Mensaje = String.Format("Esta clave presupuestaria no puede afectarse con un abono por este importe. Su columna de pagado tiene {0:C}",pagado + dataPoliza.Importe) });
                            break;
                    }
                }
                else if(!String.IsNullOrEmpty(dataPoliza.Id_ClavePresupuestoIng))
                {
                    dataPoliza.IdCur = dataModel.IdCur;
                    switch(dataModel.Id_Movimiento)
                    {
                        case Diccionarios.ValorMovimientos.CARGO:
                            decimal recaudado = polizasBL.hasRecaudado(dataPoliza.Id_ClavePresupuestoIng, dataPoliza.Importe.Value, dataPoliza.Fecha);
                            if (recaudado < 0)
                                return Json(new { Exito = false, hasPagado = false, Mensaje = String.Format("Esta clave presupuestaria no puede afectarse con un cargo por este importe. Su columna de recaudado tiene {0:C}",recaudado + dataPoliza.Importe) });
                            break;
                        case Diccionarios.ValorMovimientos.ABONO:
                            break;
                    }
                }

                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                dataPoliza.Usuario_Act = (short)appUsuario.IdUsuario;
                dataPoliza.Fecha_Act = DateTime.Now;
                dataPoliza.Fecha = DateTime.Now;
                if (dataPoliza.Id_Registro == 0)
                {
                    dataPoliza.Id_Registro = foliador.getNextIdDePolizas(dataPoliza.Id_TipoPoliza, dataPoliza.Id_FolioPoliza, dataPoliza.Id_MesPoliza);
                    DALdepolizas.Insert(dataPoliza);
                    DALdepolizas.Save();
                }
                else
                {
                    DALdepolizas.Update(dataPoliza);
                    DALdepolizas.Save();
                }
                De_PolizasModel r = llenar.LLenado_DePolizas(dataPoliza.Id_TipoPoliza,dataPoliza.Id_FolioPoliza,dataPoliza.Id_MesPoliza,dataPoliza.Id_Registro);
                return Json(new { Exito = true, Mensaje = "OK", Registro = r });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult EliminarDePolizas(Byte IdTipo, Int16 IdFolio, Byte IdMes, Int32 IdRegistro)
        {
            try
            {
                DALdepolizas.Delete(x => x.Id_TipoPoliza == IdTipo && x.Id_FolioPoliza == IdFolio && x.Id_MesPoliza == IdMes && x.Id_Registro == IdRegistro);
                DALdepolizas.Save();
                return Json(new { Exito = true, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult ValidarDePolizasSalir(byte IdTipo, int IdFolio, byte IdMes)
        {
            try
            {
                List<De_Polizas> detalles = DALdepolizas.Get(x=> x.Id_TipoPoliza == IdTipo && x.Id_FolioPoliza == IdFolio && x.Id_MesPoliza == IdMes).ToList();
                //if (detalles.Where(x => x.Id_ClavePresupuesto != null && x.Id_Movimiento == 1).Sum(x => x.Importe) != detalles.Where(x => x.Id_ClavePresupuesto != null && x.Id_Movimiento == 2).Sum(x => x.Importe))
                //    return Json(new { Exito = false, Mensaje = "La suma de cargos y abonos del COG no está cuadrada. No puede salir. Revise" });
                //if (detalles.Where(x => x.Id_ClavePresupuestoIng != null && x.Id_Movimiento == 1).Sum(x => x.Importe) != detalles.Where(x => x.Id_ClavePresupuestoIng != null && x.Id_Movimiento == 2).Sum(x => x.Importe))
                //    return Json(new { Exito = false, Mensaje = "La suma de cargos y abonos del CRI no está cuadrada. No puede salir. Revise" });
                //if (detalles.Where(x => x.Id_ClavePresupuestoIng == null && x.Id_ClavePresupuesto == null && x.Id_Movimiento == 1).Sum(x => x.Importe) != detalles.Where(x => x.Id_ClavePresupuestoIng == null && x.Id_ClavePresupuesto == null && x.Id_Movimiento == 2).Sum(x => x.Importe))
                //    return Json(new { Exito = false, Mensaje = "La suma de cargos y abonos de las cuentas de balance no está cuadrada. No puede salir. Revise" });
                if(detalles.Where(x=> x.Id_Movimiento == 1).Sum(x=>x.Importe) != detalles.Where(x=> x.Id_Movimiento == 2).Sum(x=>x.Importe))
                    return Json(new { Exito = false, Mensaje = "La suma de cargos y abonos no está cuadrada. No puede salir. Revise" });
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                if(!polizasBL.hasPolizas(IdTipo,IdFolio,IdMes))
                {
                    _store.PA_Genera_PolizasManuales(IdTipo, IdFolio, IdMes, (short)appUsuario.IdUsuario);
                }
                return Json(new { Exito = true, Mensaje = "Ok" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult NewPoliza()
        {
            Ma_PolizasModel dataModal = new Ma_PolizasModel();
            dataModal.cFechas = new Control_Fechas();
            dataModal.Fecha = DateTime.Now;
            dataModal.TipoPoliza = "DIARIO";
            dataModal.Id_TipoPoliza = 3;
            dataModal.ListaId_ClasPoliza = new SelectList(clasificacionpolizas.Get(x => x.Id_SubClasificaPoliza == 0 && x.Id_ClasificaPoliza != 0 && x.Id_TipoPoliza == 3 && x.Automatica == false), "Id_ClasificaPoliza", "Descripcion");
            return Json(new { Registro = dataModal, fMax = dataModal.cFechas.Fecha_Max.ToShortDateString(), fMin = dataModal.cFechas.Fecha_Min.ToShortDateString() });
        } 

        [HttpGet]
        public ActionResult CancelarPoliza(byte IdTipo, int IdFolio, byte IdMes)
        {
            Ma_PolizasModel dataModal = llenar.LLenado_MaPolizas(IdTipo, IdFolio, IdMes);
            dataModal.cFechas = new Control_Fechas();
            dataModal.cFechas.Fecha_Min = dataModal.Fecha;
            return View(dataModal);
        }

        [HttpPost]
        public ActionResult CancelarPoliza(Ma_Polizas dataModel, string Fecha_Cancela_C)
        {
            try
            {
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                Ma_Polizas poliza = DALMaPolizas.GetByID(x=> x.Id_FolioPoliza == dataModel.Id_FolioPoliza && x.Id_MesPoliza == dataModel.Id_MesPoliza && x.Id_TipoPoliza == dataModel.Id_TipoPoliza);
                poliza.FechaCancelacion = Convert.ToDateTime(Fecha_Cancela_C);
                DALMaPolizas.Update(poliza);
                DALMaPolizas.Save();
                _store.PA_Genera_PolizasManualesC(dataModel.Id_TipoPoliza, dataModel.Id_FolioPoliza, dataModel.Id_MesPoliza,(short)appUsuario.IdUsuario,Convert.ToDateTime(Fecha_Cancela_C));
                Ma_PolizasModel dataModal = llenar.LLenado_MaPolizas(dataModel.Id_TipoPoliza, dataModel.Id_FolioPoliza, dataModel.Id_MesPoliza);
                return Json(new { Exito = true, Mensaje = "OK", Registro = dataModal });
            }
            catch (Exception ex)
            {
                Ma_PolizasModel dataModal = llenar.LLenado_MaPolizas(dataModel.Id_TipoPoliza, dataModel.Id_FolioPoliza, dataModel.Id_MesPoliza);
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje, Registro = dataModal });
            }

        }

        

    }
}
