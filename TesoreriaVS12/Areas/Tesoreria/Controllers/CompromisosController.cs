using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.BL;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Areas.Tesoreria.DAL.Vistas;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Filters;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;
using System.Web.Script.Serialization;

namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    [AuthorizeLogin]
    public class CompromisosController : Controller
    {
        private MaCompromisosDAL DALMaCompromisos { get; set; }
        private TipoCompromisosDAL DALTipoCompromisos { get; set; }
        private ClasificaBeneficiariosDAL DALClasificaBeneficiarios { get; set; }
        private Llenado llenar { get; set; }
        private CompromisosBL BLCompromisos { get; set; }
        private DeCompromisosBL BLDeCompromisos { get; set; }
        private ParametrosDAL DALParametros { get; set; }
        private DeCompromisosDAL DALDeCompromisos { get; set; }
        private MaPolizasDAL DALMaPolizas { get; set; }
        private ProceduresDAL DALProcedures { get; set; }
        private CuentasDAL DALCuentas { get; set; }
                protected BeneficiariosDAL beneficiarioDal { get; set; }
        public CompromisosController()
        {
            if (DALMaCompromisos == null) DALMaCompromisos = new MaCompromisosDAL();
            if (DALTipoCompromisos == null) DALTipoCompromisos = new TipoCompromisosDAL();
            if (DALClasificaBeneficiarios == null) DALClasificaBeneficiarios = new ClasificaBeneficiariosDAL();
            if (llenar == null) llenar = new Llenado();
            if (BLCompromisos == null) BLCompromisos = new CompromisosBL();
            if (BLDeCompromisos == null) BLDeCompromisos = new DeCompromisosBL();
            if (DALParametros == null) DALParametros = new ParametrosDAL();
            if (DALDeCompromisos == null) DALDeCompromisos = new DeCompromisosDAL();
            if (DALMaPolizas == null) DALMaPolizas = new MaPolizasDAL();
            if (DALProcedures == null) DALProcedures = new ProceduresDAL();
            if (DALCuentas == null) DALCuentas = new CuentasDAL();
            if(beneficiarioDal == null) beneficiarioDal = new BeneficiariosDAL();
        }
        //
        // GET: /Tesoreria/Compromisos/
        public ActionResult Index()
        {
            BusquedaCompromisos model = new BusquedaCompromisos();            

            model.ListEstatus = new SelectList(Diccionarios.EstatusCompromisos, "Key", "Value");
            model.ListTipoCompromiso = new SelectList(DALTipoCompromisos.Get(), "Id_TipoCompromiso", "Descripcion");            
            return View(model);
        }

        public ActionResult BuscarCompromisos(BusquedaCompromisos dataModel)
        {
            List<Ma_CompromisosModel> models = new List<Ma_CompromisosModel>();
            List<Ma_Compromisos> entities = new List<Ma_Compromisos>();

            DALMaCompromisos.Get(reg => (dataModel.Folio > 0 ? reg.Id_FolioCompromiso == dataModel.Folio : reg.Id_FolioCompromiso > 0) &&
                (dataModel.Estatus > 0 ? reg.Estatus == dataModel.Estatus : reg.Id_FolioCompromiso > 0) &&
                (dataModel.NoRequisicion > 0 ? reg.No_Requisicion == dataModel.NoRequisicion : reg.Id_FolioCompromiso > 0) &&
                (dataModel.TipoCompromiso > 0 ? reg.Id_TipoCompromiso == dataModel.TipoCompromiso : reg.Id_FolioCompromiso > 0) &&
                (dataModel.IdBeneficiario > 0 ? reg.Id_Beneficiario == dataModel.IdBeneficiario : reg.Id_FolioCompromiso > 0) &&
                (dataModel.Fecha_Fincamiento.HasValue ? reg.Fecha_Orden == dataModel.Fecha_Fincamiento : reg.Id_FolioCompromiso > 0) &&
                (dataModel.isAdquisiciones ? reg.Adquisicion == dataModel.isAdquisiciones : reg.Id_FolioCompromiso > 0) &&
                (dataModel.NoOrdenCompra > 0 ? reg.No_Adquisicion == dataModel.NoOrdenCompra :reg.Id_FolioCompromiso > 0)
                ).ToList().ForEach(item => { if (!entities.Contains(item)) entities.Add(item); });
            foreach (var item in entities)
            {
                Ma_CompromisosModel model = new Ma_CompromisosModel();
                model = llenar.LLenado_MaCompromisos(item.Id_TipoCompromiso, item.Id_FolioCompromiso);
                model.Beneficiario = model.Ca_Beneficiarios.NombreCompleto;
                models.Add(model);
            }

            return View(models);
        }

        [HttpPost]
        public ActionResult getNuevoCompromiso() 
        {
            bool canCreate = false;
            CA_Parametros entity = DALParametros.GetByID(reg => reg.Nombre.Equals("CapturarCompromisos"));
            if (entity != null)
                canCreate = Boolean.Parse(entity.Valor);

            if(!canCreate)
                return Json(new { Data = "", Error = true, Message = "No se puede agregar compromiso, porque solo se permite la generación de compromisos desde Adquisiciones." });
            
            Ma_CompromisosModel model = new Ma_CompromisosModel(Ma_CompromisosModel.ORDEN_COMPRA);                        
            model.Usuario_Orden = Logueo.GetUsrLogueado().NombreCompleto;
            model.Botonera = new List<object>() { "bNuevo", "bBuscar", "bSalir" };
            ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, BLCompromisos.getUltimoMesCerrado() + 1, 1).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            model.Fecha = DateTime.Now;
            model.Fecha_Orden = DateTime.Now;
            model.Fecha_Requisicion = DateTime.Now;
            model.Fecha_Adquisicion = DateTime.Now;
            model.Fecha_Autorizo = null;
            return Json(new { Data = model, Error = false, Message = "" });
        }

        [HttpGet]
        public ActionResult OrdenCompra()
        {            
            Ma_CompromisosModel model = new Ma_CompromisosModel(Ma_CompromisosModel.ORDEN_COMPRA);
            model.Usuario_Orden = Logueo.GetUsrLogueado().NombreCompleto;
            model.PagarseEn = "";
            model.Botonera = new List<object>() { "bNuevo", "bBuscar", "bSalir" };
            ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, BLCompromisos.getUltimoMesCerrado() + 1, 1).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            model.Fecha = DateTime.Now;
            model.Fecha_Orden = DateTime.Now;
            model.Fecha_Requisicion = DateTime.Now;
            model.Fecha_Autorizo = null;
            return View(model);
        }

        [HttpPost]
        public ActionResult OrdenCompra(short TipoCompromiso, int FolioCompromiso, string args,bool? Regreso = null)
        {
            Ma_CompromisosModel model = new Ma_CompromisosModel();
            model = llenar.LLenado_MaCompromisos(TipoCompromiso, FolioCompromiso);
            model.ListaId_TipoCompromiso = new SelectList(DALTipoCompromisos.Get(), "Id_TipoCompromiso", "Descripcion", model.Id_TipoCompromiso);

            if (model.Id_Beneficiario != null)
                model.Beneficiario = model.Ca_Beneficiarios.NombreCompleto;
            if (model.Estatus != null)
                model.Descripcion_Estatus = Diccionarios.EstatusCompromisos[model.Estatus.Value];
            if (model.Ca_Areas_Solicitud != null)
                model.Area_Solicitud = model.Ca_Areas_Solicitud.Descripcion;
            if (model.Ca_Areas_Entrega != null)
                model.Area_Entrega = model.Ca_Areas_Entrega.Descripcion;         

            if (Regreso.HasValue && Regreso.Value)            
                model.regresoDetalle = true;

            /*Setear botonera*/
            model.Botonera = BLCompromisos.createBotonera(TipoCompromiso, FolioCompromiso, model.Estatus.Value, model.Adquisicion);
            if(!String.IsNullOrEmpty(args))
            {
                model.Regreso = new JavaScriptSerializer().Deserialize<ReturnMaster>(args);
                model.argsReturn = args;
            }
            return View(model);
        }

        [HttpPost]       
        public ActionResult OrdenCompraJson(short TipoCompromiso, int FolioCompromiso)
       {            
            try
            {
                Ma_CompromisosModel model = new Ma_CompromisosModel();
                model = llenar.LLenado_MaCompromisos(TipoCompromiso, FolioCompromiso);                
                //model.Usuario_Orden = Logueo.GetUsrLogueado().NombreCompleto;
                model.ListaId_TipoCompromiso = new SelectList(DALTipoCompromisos.Get(), "Id_TipoCompromiso", "Descripcion", model.Id_TipoCompromiso);

                if(model.Estatus != null)
                    model.Descripcion_Estatus = Diccionarios.EstatusCompromisos[model.Estatus.Value];                
                if (model.Ca_Areas_Solicitud != null)
                    model.Area_Solicitud = model.Ca_Areas_Solicitud.Descripcion;
                if (model.Ca_Areas_Entrega != null)
                    model.Area_Entrega = model.Ca_Areas_Entrega.Descripcion;                

                /*Setear botonera*/
                model.Botonera = BLCompromisos.createBotonera(TipoCompromiso, FolioCompromiso, model.Estatus.Value, model.Adquisicion);

                return Json(new { Exito = true, Data = model, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }            
        }

        [HttpPost]
        [ActionName("OrdenCompraGuardar")]
        public ActionResult OrdenCompra(Ma_CompromisosModel dataModel)
        {            
            try
            {     
                Ma_Compromisos entity = DALMaCompromisos.GetByID(reg => reg.Id_TipoCompromiso == dataModel.Id_TipoCompromiso && reg.Id_FolioCompromiso == dataModel.Id_FolioCompromiso);
                if (entity == null)
                {
                    //Nuevo
                    dataModel.Id_Cuenta_Beneficiario = BLCompromisos.getCuentaBeneficiario(dataModel.Id_Beneficiario.Value, dataModel.Id_ClasificaBeneficiario.Value);
                    dataModel.Historial = false;
                    dataModel.Usuario_Act = short.Parse(Logueo.GetUsrLogueado().IdUsuario.ToString());
                    dataModel.Fecha_Act = DateTime.Now;
                    dataModel.Fecha = dataModel.Fecha_Orden;
                    dataModel.Estatus = Diccionarios.ValorEstatus.COMPROMETIDO;
                    entity = EntityFactory.getEntity<Ma_Compromisos>(dataModel, new Ma_Compromisos());
                    if (BLCompromisos.isClosed(entity.Fecha_Orden.Value))
                        return Json(new { Exito = false, Mensaje = "El mes se encuentra cerrado." });

                    entity.Id_FolioCompromiso = BLCompromisos.getNextId(entity.Id_TipoCompromiso);
                    DALMaCompromisos.Insert(entity);
                    DALMaCompromisos.Save();
                    BLCompromisos.updateNextId(entity.Id_TipoCompromiso);
                }
                else
                {
                    //Editar
                    dataModel.Id_Cuenta_Beneficiario = BLCompromisos.getCuentaBeneficiario(dataModel.Id_Beneficiario.Value, dataModel.Id_ClasificaBeneficiario.Value);
                    dataModel.Usuario_Act = short.Parse(Logueo.GetUsrLogueado().IdUsuario.ToString());
                    dataModel.Fecha_Act = DateTime.Now;
                    entity = EntityFactory.getEntity<Ma_Compromisos>(dataModel, entity);
                    if (!BLCompromisos.isEditable(entity.Fecha_Orden.Value, entity.Estatus.Value, entity.Adquisicion.Value))
                        return Json(new { Exito = false, Mensaje = "No se puede editar este registro." });

                    DALMaCompromisos.Update(entity);
                    DALMaCompromisos.Save();
                }
                /*Setear botonera*/

                return Json(new { Exito = true, TipoCompromiso = entity.Id_TipoCompromiso, FolioCompromiso = entity.Id_FolioCompromiso, Botonera = BLCompromisos.createBotonera(dataModel.Id_TipoCompromiso, entity.Id_FolioCompromiso, dataModel.Estatus.Value), Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }    
        }

        [HttpPost]
        public ActionResult OrdenCompraEditar(Ma_CompromisosModel dataModel)
        {
            try
            {
                Ma_Compromisos entity = DALMaCompromisos.GetByID(reg => reg.Id_TipoCompromiso == dataModel.Id_TipoCompromiso && reg.Id_FolioCompromiso == dataModel.Id_FolioCompromiso);
                entity = EntityFactory.getEntity<Ma_Compromisos>(dataModel, entity);
                if(!BLCompromisos.isEditable(entity.Fecha_Orden.Value, entity.Estatus.Value, entity.Adquisicion.Value))
                    return Json(new { Exito = false, Mensaje = "OK" });
                
                DALMaCompromisos.Update(entity);
                DALMaCompromisos.Save();
                return Json(new { Exito = true, TipoCompromiso = entity.Id_TipoCompromiso, FolioCompromiso = entity.Id_FolioCompromiso, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult getPagarseEn(short? tipo) 
        {
            try
            {                
                if(tipo==null)
                    return Json(new { Exito = true, Data = "", Mensaje = "OK" });
                return Json(new { Exito = true, Data = DALTipoCompromisos.GetByID(reg => reg.Id_TipoCompromiso == tipo).Pagarse, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpGet]
        public ActionResult Recibir(short tcompromiso, int fcompromiso)
        {
            Ma_Compromisos entity = DALMaCompromisos.GetByID(reg => reg.Id_TipoCompromiso == tcompromiso && reg.Id_FolioCompromiso == fcompromiso);
            RecibeCompromiso model = new RecibeCompromiso();
            model.Id_TipoCompromiso_R = tcompromiso;
            model.Id_FolioCompromiso_R = fcompromiso;
            model.Fecha_Orden_R = entity.Fecha_Orden;
            if (BLCompromisos.isClosed(entity.Fecha_Orden.Value))
                return Json(new { Exito = false, Mensaje = "El mes se encuentra cerrado." });

            model.cfechas = new Control_Fechas();
            model.cfechas.Fecha_Min = model.Fecha_Orden_R.Value;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Recibir(RecibeCompromiso dataModel)
        {
            try
            {
                Ma_Compromisos entity = DALMaCompromisos.GetByID(reg => reg.Id_TipoCompromiso == dataModel.Id_TipoCompromiso_R && reg.Id_FolioCompromiso == dataModel.Id_FolioCompromiso_R);                
                entity.Estatus = Diccionarios.ValorEstatus.RECIBIDO;
                entity.Usuario_Act = (short)Logueo.GetUsrLogueado().IdUsuario;
                entity.Fecha_Act = DateTime.Now;
                entity.Usuario_Recibe_Area = Logueo.GetUsrLogueado().NombreCompleto;
                entity.Fecha_Recibe_Area = dataModel.Fecha_Devengado_R;
                entity.Fecha_Devengado = dataModel.Fecha_Devengado_R;
                entity.Observa_Recibio = dataModel.Observa_Recibio_R;
                if (BLCompromisos.isClosed(dataModel.Fecha_Devengado_R.Value))
                    return Json(new { Exito = false, Mensaje = "El mes se encuentra cerrado." });

                DALMaCompromisos.Update(entity);
                DALMaCompromisos.Save();

                string[] poliza = DALProcedures.Pa_Genera_PolizaOrden_Devengado(dataModel.Id_TipoCompromiso_R, dataModel.Id_FolioCompromiso_R, short.Parse(Logueo.GetUsrLogueado().IdUsuario.ToString()));
                entity.Id_MesPO_Devengado = byte.Parse(poliza[0]);
                entity.Id_FolioPO_Devengado = int.Parse(poliza[1]);                
                poliza = new string[2];
                poliza = DALProcedures.PA_Genera_Poliza_Diario_CR(dataModel.Id_TipoCompromiso_R, dataModel.Id_FolioCompromiso_R, short.Parse(Logueo.GetUsrLogueado().IdUsuario.ToString()));
                entity.Id_TipoPoliza = Ca_TipoPolizasModel.DIARIO;
                entity.Id_MesPoliza = byte.Parse(poliza[0]);
                entity.Id_FolioPoliza = int.Parse(poliza[1]);                
                
                if (BLCompromisos.isClosed(dataModel.Fecha_Devengado_R.Value))
                    return Json(new { Exito = false, Mensaje = "El mes se encuentra cerrado."  });
                
                DALMaCompromisos.Update(entity);    
                DALMaCompromisos.Save();

                return Json(new { Exito = true, Data = llenar.LLenado_MaCompromisos(dataModel.Id_TipoCompromiso_R, dataModel.Id_FolioCompromiso_R), Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpGet]
        public ActionResult Cancelar(short tcompromiso, int fcompromiso)
        {            
            Ma_Compromisos entity = DALMaCompromisos.GetByID(reg => reg.Id_TipoCompromiso == tcompromiso && reg.Id_FolioCompromiso == fcompromiso);
            CancelaCompromiso model = new CancelaCompromiso();
            model.Id_TipoCompromiso_C = entity.Id_TipoCompromiso;
            model.Id_FolioCompromiso_C = entity.Id_FolioCompromiso;
            model.Usuario_Cancela_C = Logueo.GetUsrLogueado().NombreCompleto;
            model.Cuenta_Beneficiario_C = DALCuentas.GetByID(reg => reg.Id_Cuenta == entity.Id_Cuenta_Beneficiario).Id_CuentaFormato;
            model.cfechas = new Control_Fechas();
            model.cfechas.Fecha_Min = entity.Fecha_Orden.Value;
            if(entity.Id_MesPO_Devengado != null)
                model.cfechas.Fecha_Min = entity.Fecha_Devengado.Value;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancelar(CancelaCompromiso dataModel)
        {
            try
            {
                Ma_Compromisos entity = DALMaCompromisos.GetByID(reg => reg.Id_TipoCompromiso == dataModel.Id_TipoCompromiso_C && reg.Id_FolioCompromiso == dataModel.Id_FolioCompromiso_C);                
                entity.Usuario_Cancela = dataModel.Usuario_Cancela_C;
                entity.Fecha_Cancela = DateTime.Now;
                entity.Estatus = Diccionarios.ValorEstatus.CANCELADO;
                
                List<De_Compromisos> detalis = DALDeCompromisos.Get(reg => reg.Id_TipoCompromiso == dataModel.Id_TipoCompromiso_C && reg.Id_FolioCompromiso == dataModel.Id_FolioCompromiso_C).ToList();
                foreach (De_Compromisos item in detalis)
                {
                    item.AfectaCompro = false;
                    item.Usuario_Act = (short)Logueo.GetUsrLogueado().IdUsuario;
                    item.Fecha_Act = DateTime.Now;
                    DALDeCompromisos.Update(item);
                }
                byte? mnulo = null;
                int? fnulo = null;
                if (entity.Id_MesPO_Comprometido != null)
                {
                    byte mes = 0;
                    int fpoliza = 0;
                    DALProcedures.Pa_Genera_PolizaOrden_Comprometido_Cancela(dataModel.Id_TipoCompromiso_C, dataModel.Id_FolioCompromiso_C, dataModel.Fecha_Cancela_C, short.Parse(Logueo.GetUsrLogueado().IdUsuario.ToString()), ref mes, ref fpoliza);
                    entity.Id_MesPO_Comprometido_C = mes == 0 ? mnulo : mes;
                    entity.Id_FolioPO_Comprometido_C = fpoliza == 0 ? fnulo : fpoliza;   
                }

                if (entity.Id_MesPO_Devengado != null)
                {
                    byte mes = 0;
                    int fpoliza = 0;
                    DALProcedures.Pa_Genera_PolizaOrden_Devengado_Cancela(dataModel.Id_TipoCompromiso_C, dataModel.Id_FolioCompromiso_C, dataModel.Fecha_Cancela_C, short.Parse(Logueo.GetUsrLogueado().IdUsuario.ToString()), ref mes, ref fpoliza);
                    entity.Id_MesPO_Devengado_C = mes == 0 ? mnulo : mes;
                    entity.Id_FolioPO_Devengado_C = fpoliza == 0 ? fnulo : fpoliza;
                    mes = 0;
                    fpoliza = 0;
                    DALProcedures.Pa_Cancelacion_Poliza_Diario_Devengo(dataModel.Id_TipoCompromiso_C, dataModel.Id_FolioCompromiso_C, dataModel.Fecha_Cancela_C, short.Parse(Logueo.GetUsrLogueado().IdUsuario.ToString()), ref mes, ref fpoliza);
                    entity.Id_MesPoliza_C = mes == 0 ? mnulo : mes;
                    entity.Id_FolioPoliza_C = fpoliza == 0 ? fnulo : fpoliza;
                }

                if (!BLCompromisos.isClosed(dataModel.Fecha_Cancela_C.Value))
                {
                    DALDeCompromisos.Save();
                    DALMaCompromisos.Save();
                }

                return Json(new { Exito = true, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult DetalleCompromiso(short TipoCompromiso, int FolioCompromiso, string args, bool? CanSaldar)
        {
            Ma_CompromisosModel model = new Ma_CompromisosModel();
            model = llenar.LLenado_MaCompromisos(TipoCompromiso, FolioCompromiso);
            List<De_Compromisos> entities = DALDeCompromisos.Get(reg=> reg.Id_TipoCompromiso == TipoCompromiso && reg.Id_FolioCompromiso == FolioCompromiso).ToList();
            
            model.De_Compromisos = model.De_Compromisos.OrderBy(reg => reg.Id_Registro).ToList();

            /*Setear botonera*/
            model.Botonera = BLDeCompromisos.createBotonera(model, CanSaldar);
            model.argsReturn = args;
            return View(model);
        }

        [HttpPost]
        [ActionName("DetalleCompromisoGuardar")]
        [ValidateAntiForgeryToken]
        public ActionResult DetalleCompromiso(De_CompromisosModel dataModel)
        {
            try
            {
                De_Compromisos entity = DALDeCompromisos.GetByID(reg => reg.Id_TipoCompromiso == dataModel.Id_TipoCompromiso && reg.Id_FolioCompromiso == dataModel.Id_FolioCompromiso && reg.Id_Registro == dataModel.Id_Registro);
                if (entity != null)
                {
                    //Editar
                    entity = EntityFactory.getEntity<De_Compromisos>(dataModel, entity);
                    entity.Id_ClavePresupuesto = StringID.IdClavePresupuesto(dataModel.Id_Area, dataModel.Id_Funcion, dataModel.Id_Actividad, dataModel.Id_ClasificacionP, dataModel.Id_Programa, dataModel.Id_Proceso, dataModel.Id_TipoMeta, dataModel.Id_ActividadMIR, dataModel.Id_Accion, dataModel.Id_Alcance, dataModel.Id_TipoG, dataModel.Id_Fuente, dataModel.AnioFin, dataModel.Id_ObjetoG);
                    entity.Disponibilidad = BLDeCompromisos.hasDisponibilidad(entity.Id_ClavePresupuesto, entity.Importe.Value, dataModel.Fecha_Orden);
                    if (entity.Disponibilidad.Value == false && Convert.ToBoolean(DALParametros.GetByID(reg => reg.Nombre == "Sin_Disponibilidad").Valor) == false)
                        return Json(new { Exito = false, Mensaje = "Esta Clave Presupuestal no cuenta con disponibilidad." });

                    entity.AfectaCompro = true;
                    if (!entity.Disponibilidad.Value)
                    {
                        //false -- Autorizado
                        entity.AfectaCompro = false;
                        Ma_Compromisos master = DALMaCompromisos.GetByID(reg => reg.Id_TipoCompromiso == entity.Id_TipoCompromiso && reg.Id_FolioCompromiso == entity.Id_FolioCompromiso);
                        master.Estatus = Diccionarios.ValorEstatus.AUTORIZACION;
                        DALMaCompromisos.Update(master);
                        DALMaCompromisos.Save();
                    }
                    
                    DALDeCompromisos.Update(entity);
                    DALDeCompromisos.Save();
                    return Json(new { Exito = true, Mensaje = "OK" });
                }
                entity = EntityFactory.getEntity<De_Compromisos>(dataModel,new De_Compromisos());
                entity.Id_ClavePresupuesto = StringID.IdClavePresupuesto(dataModel.Id_Area, dataModel.Id_Funcion, dataModel.Id_Actividad, dataModel.Id_ClasificacionP, dataModel.Id_Programa, dataModel.Id_Proceso, dataModel.Id_TipoMeta, dataModel.Id_ActividadMIR, dataModel.Id_Accion, dataModel.Id_Alcance, dataModel.Id_TipoG, dataModel.Id_Fuente, dataModel.AnioFin, dataModel.Id_ObjetoG);
                entity.Disponibilidad = BLDeCompromisos.hasDisponibilidad(entity.Id_ClavePresupuesto, entity.Importe.Value, dataModel.Fecha_Orden);
                
                if (entity.Disponibilidad.Value == false && Convert.ToBoolean(DALParametros.GetByID(reg => reg.Nombre == "Sin_Disponibilidad").Valor) == false)
                    return Json(new { Exito = false, Mensaje = "Esta Clave Presupuestal no cuenta con disponibilidad." });

                //if (dataModel.Id_Fuente)

                entity.AfectaCompro = true;
                if (!entity.Disponibilidad.Value)
                {                     
                    //false -- Autorizado
                    entity.AfectaCompro = false;
                    Ma_Compromisos master = DALMaCompromisos.GetByID(reg => reg.Id_TipoCompromiso == entity.Id_TipoCompromiso && reg.Id_FolioCompromiso == entity.Id_FolioCompromiso);
                    master.Estatus = Diccionarios.ValorEstatus.AUTORIZACION;
                    DALMaCompromisos.Update(master);
                    DALMaCompromisos.Save();
                    List<De_Compromisos> details = DALDeCompromisos.Get(reg => reg.Id_TipoCompromiso == entity.Id_TipoCompromiso && reg.Id_FolioCompromiso == entity.Id_FolioCompromiso).ToList();
                    details.ForEach(item => { item.AfectaCompro = false; DALDeCompromisos.Update(item); });
                    DALDeCompromisos.Save();
                }
                entity.Id_Registro = (short)BLDeCompromisos.getNextId(dataModel.Id_TipoCompromiso, dataModel.Id_FolioCompromiso);
                DALDeCompromisos.Insert(entity);
                DALDeCompromisos.Save();
                return Json(new { Exito = true, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public JsonResult getNuevoDetalleCompromiso(short TipoCompromiso, int FolioCompromiso)
        {
            Ma_CompromisosModel master = llenar.LLenado_MaCompromisos(TipoCompromiso, FolioCompromiso);
            De_CompromisosModel model = new De_CompromisosModel();
            return Json(new { Data = master, Botonera = BLDeCompromisos.createBotonera(master, false), Error = false, Message = "" });
        }

        [HttpPost]
        public JsonResult getDetalleCompromiso(short TipoCompromiso, int FolioCompromiso, short Registro)
        {
            Ma_CompromisosModel master = llenar.LLenado_MaCompromisos(TipoCompromiso, FolioCompromiso);
            De_CompromisosModel model = llenar.LLenado_DeCompromisos(TipoCompromiso, FolioCompromiso, Registro);
            return Json(new { Data = model, Botonera = BLDeCompromisos.createBotonera(master, false), Error = false, Message = "" });            
        }

        [HttpPost]
        public JsonResult deleteDetalleCompromiso(short TipoCompromiso, int FolioCompromiso, short Registro)
        {
            try
            {
                List<De_Compromisos> entities = DALDeCompromisos.Get(reg => reg.Id_TipoCompromiso == TipoCompromiso && reg.Id_FolioCompromiso == FolioCompromiso).ToList();
                De_Compromisos entity = entities.FirstOrDefault(reg => reg.Id_TipoCompromiso == TipoCompromiso && reg.Id_FolioCompromiso == FolioCompromiso && reg.Id_Registro == Registro);
                if(entity.Id_Movimiento == Diccionarios.ValorMovimientos.CARGO)
                    if (entities.Where(reg => reg.Id_Movimiento == Diccionarios.ValorMovimientos.CARGO).Count() == entities.Where(reg => reg.Id_Movimiento == Diccionarios.ValorMovimientos.ABONO).Count())
                        DALDeCompromisos.Delete(reg => reg.Id_TipoCompromiso == TipoCompromiso && reg.Id_FolioCompromiso == FolioCompromiso && reg.Id_Movimiento == Diccionarios.ValorMovimientos.ABONO);

                DALDeCompromisos.Delete(reg => reg.Id_TipoCompromiso == TipoCompromiso && reg.Id_FolioCompromiso == FolioCompromiso && reg.Id_Registro == Registro);
                DALDeCompromisos.Save();
                return Json(new { Exito = true, CanSaldar = BLDeCompromisos.hasRows(TipoCompromiso, FolioCompromiso) ? true : false, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }


        [HttpPost]
        public JsonResult getDisponibilidad(string cve, decimal importe, DateTime? fecha)
        {
            return Json(new { Data = new { Disponibilidad = BLDeCompromisos.hasDisponibilidad(cve, importe, fecha), SinDisponibilidad = Convert.ToBoolean(DALParametros.GetByID(reg => reg.Nombre == "Sin_Disponibilidad").Valor) }, Error = false, Message = "" });
        }

        [HttpPost]
        public ActionResult saldarMovimientos(short TipoCompromiso, int FolioCompromiso)
        {
            try
            {
                De_Compromisos deleteAbono = DALDeCompromisos.GetByID(reg => reg.Id_TipoCompromiso == TipoCompromiso && reg.Id_FolioCompromiso == FolioCompromiso && reg.Id_Movimiento == Diccionarios.ValorMovimientos.ABONO);
                if (deleteAbono != null)
                {
                    DALDeCompromisos.Delete(reg => reg.Id_TipoCompromiso == deleteAbono.Id_TipoCompromiso && reg.Id_FolioCompromiso == deleteAbono.Id_FolioCompromiso && reg.Id_Registro == deleteAbono.Id_Registro);
                    DALDeCompromisos.Save();
                }

                De_Compromisos entity = new De_Compromisos();
                entity.Id_Movimiento = Diccionarios.ValorMovimientos.ABONO;
                entity.Importe = DALDeCompromisos.Get(reg => reg.Id_TipoCompromiso == TipoCompromiso && reg.Id_FolioCompromiso == FolioCompromiso && reg.Id_Movimiento == Diccionarios.ValorMovimientos.CARGO).Sum(reg => reg.Importe);
                entity.Id_Cuenta = DALMaCompromisos.GetByID(reg => reg.Id_TipoCompromiso == TipoCompromiso && reg.Id_FolioCompromiso == FolioCompromiso).Id_Cuenta_Beneficiario;
                entity.Id_TipoCompromiso = TipoCompromiso;
                entity.Id_FolioCompromiso = FolioCompromiso;
                entity.Id_Registro = (short)BLDeCompromisos.getNextId(entity.Id_TipoCompromiso, entity.Id_FolioCompromiso);
                DALDeCompromisos.Insert(entity);
                DALDeCompromisos.Save();
                return Json(new { Exito = true, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult generarPolizaComprometido(short TipoCompromiso, int FolioCompromiso)
        {
            try
            {                
                Ma_Compromisos master = DALMaCompromisos.GetByID(reg => reg.Id_TipoCompromiso == TipoCompromiso && reg.Id_FolioCompromiso == FolioCompromiso);

                if (master.Estatus == Diccionarios.ValorEstatus.COMPROMETIDO && master.Cargos > 0 && (master.Abonos == master.Cargos) && master.Id_FolioPO_Comprometido == null)
                {
                    string[] poliza = DALProcedures.Pa_Genera_PolizaOrden_Comprometido(TipoCompromiso, FolioCompromiso, short.Parse(Logueo.GetUsrLogueado().IdUsuario.ToString()));
                    master.Id_MesPO_Comprometido = byte.Parse(poliza[0]);
                    master.Id_FolioPO_Comprometido = int.Parse(poliza[1]);
                    DALMaCompromisos.Update(master);
                }
                DALMaCompromisos.Save();
                return Json(new { Exito = true, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        public ActionResult Polizas()
        {
            return View();
        }

        #region Compromisos de Nomina

        public ActionResult CompromisoNomina()
        {
            Control_Fechas fechas = new Control_Fechas();
            ViewBag.Fecha_Min = fechas.Fecha_Min;
            ViewBag.Fecha_Max = fechas.Fecha_Max;
            return View(new  Ma_CompromisosModel());
        }


        #endregion



        [HttpPost]
        public JsonResult searchBeneficiario(string Descripcion)
        {
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            int i = 0;
            string s = "108";
            bool result = int.TryParse(s, out i);
            List<VW_BeneficiariosModel> entity = new List<VW_BeneficiariosModel>();
            List<VW_Beneficiarios> entities  = new VWBeneficiariosDAL().Get().OrderBy(x => x.Nombre).ToList();
            foreach (var item in entities)
            {
                VW_BeneficiariosModel enty = new VW_BeneficiariosModel();

                enty = ModelFactory.getModel<VW_BeneficiariosModel>(item, new VW_BeneficiariosModel());
                enty.IdBeneficiario =  enty.Id_Beneficiario.ToString();
                entity.Add(enty);
                     
            }

            if (Descripcion != "-")
            {


                entity = entity.Where(x => x.Nombre.Contains(Descripcion) || x.IdBeneficiario.Contains(Descripcion)).OrderBy(x => x.Nombre).ToList();
 
            }
            else
            {
                entity = entity.OrderBy(x => x.Nombre).ToList();

            }

            foreach (var item in entity)
            {
                 dataModel.Add(String.Format("{1}-{0}",  item.Nombre, item.Id_Beneficiario));
                 
                dataIds.Add(item.Id_Beneficiario.ToString());

            }
             return Json(new { Data = dataModel, Ids = dataIds });


        }


    }
}
