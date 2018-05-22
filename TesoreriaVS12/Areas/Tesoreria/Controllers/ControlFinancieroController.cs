using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Utils;
using TesoreriaVS12.Models;
using TesoreriaVS12.Filters;
using System.Web.Script.Serialization;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.DAL;
using TesoreriaVS12.Areas.Tesoreria.BL;
using System.Configuration;
using System.IO;
using TesoreriaVS12.Areas.Tesoreria.DAL.Vistas;

namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    [AuthorizeLogin]
    public class ControlFinancieroController : Controller
    {
        bool exito = false;
        public string errorMensaje = "";
        private string ruta = "/ArchivosConciliacion";

        protected BancosDAL DALBancos { get; set; }
        protected CierreBancoDAL DALCierreBanco { get; set; }
        protected CuentasBancariasDAL DALCuentasBancarias { get; set; }
        protected ParametrosDAL DALParametros { get; set; }
        protected ProceduresDAL DALProcedures { get; set; }
        protected VW_ConciliacionDAL DALVConciliacion { get; set; }
        protected TipoMovBancariosDAL DALTipoMovBancarios { get; set; }
        protected ControlFinancieroBL BLControlFinanciero { get; set; }
        protected MaEstadosCuentaDAL DALEstadosCuenta { get; set; }

        protected MaMovimientosConciliacionDAL DALMaMovimientosConciliacion { get; set; }
        

        public ControlFinancieroController()
        {
            if (DALBancos == null) DALBancos = new BancosDAL();
            if (DALCierreBanco == null) DALCierreBanco = new CierreBancoDAL();
            if (DALParametros == null) DALParametros = new ParametrosDAL();
            if (DALProcedures == null) DALProcedures = new ProceduresDAL();
            if (DALVConciliacion == null) DALVConciliacion = new VW_ConciliacionDAL();
            if (DALTipoMovBancarios == null) DALTipoMovBancarios = new TipoMovBancariosDAL();
            if (DALCuentasBancarias == null) DALCuentasBancarias = new CuentasBancariasDAL();
            if (DALEstadosCuenta == null) DALEstadosCuenta = new MaEstadosCuentaDAL();
            if (DALMaMovimientosConciliacion == null) DALMaMovimientosConciliacion = new MaMovimientosConciliacionDAL();
            if (BLControlFinanciero == null) BLControlFinanciero = new ControlFinancieroBL();
        }

        //
        // GET: /Tesoreria/ControlFinanciero/

        //Funcion para regresar todos los registros a su estado inicial
        public ActionResult V_Reset()
        {
            ResetControlFinanciero model = new ResetControlFinanciero();
            model.borrado = true;
            return View(model);
        }
        //Funcion para actualizar los movimientos bancarios 
        public ActionResult V_ActulizaMovi()
        {
            ResetControlFinanciero model = new ResetControlFinanciero();
            model.borrado = false;
            return View(model);
        }
        [HttpPost]
        public JsonResult V_Reset(int idCtaBancaria,bool borrado)
        {
            try
            {
                CA_CierreBanco cierre = DALCierreBanco.Get(x => x.Cerrado_Conciliado.Value == false && x.Id_CtaBancaria == idCtaBancaria).OrderBy(x => x.Id_Mes).FirstOrDefault();
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                DALProcedures.Pa_ConciliaAct(Convert.ToByte(cierre.Id_Mes), idCtaBancaria, appUsuario.IdUsuario, borrado);
                return Json(new { Exito = true, Mensaje = "Operación realizada con éxito." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        [HttpPost]
        public JsonResult ValidaMes(int? idCtaBancaria)
        {
            if (idCtaBancaria.HasValue && idCtaBancaria > 0)
            {
                if (DALCierreBanco.Get(x => x.Cerrado_Conciliado.Value == false && x.Id_CtaBancaria == idCtaBancaria).Count() > 0)
                {
                    CA_CierreBanco cierre = DALCierreBanco.Get(x => x.Cerrado_Conciliado.Value == false && x.Id_CtaBancaria == idCtaBancaria).OrderBy(x => x.Id_Mes).FirstOrDefault();
                    if (cierre != null)
                        return Json(new { Exito = true, Mes = Diccionarios.Meses[cierre.Id_Mes] });
                    else
                        return Json(new { Exito = false, Mensaje = "Registro no encontrado" });
                }
                else
                    return Json(new { Exito = false, Mensaje = "No hay meses por cerrar" });
            }
            else
                return Json(new { Exito = false, Mensaje = "Registro no encontrado" });
            

        }

        public ActionResult V_ConciliarMes()
        {
            return View(new ResetControlFinanciero());
        }
        [HttpPost]
        public JsonResult V_ConciliarMes(int idCtaBancaria)
        {
            try
            {
                CA_CierreBanco cierre = DALCierreBanco.Get(x => x.Cerrado_Conciliado.Value == false && x.Id_CtaBancaria == idCtaBancaria).OrderBy(x => x.Id_Mes).FirstOrDefault();
                cierre.Cerrado_Conciliado = true;
                DALCierreBanco.Update(cierre);
                DALCierreBanco.Save();
                return Json(new { Exito = true, Mensaje = "Mes cerrado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        #region Conciliacion
        public ActionResult V_ConciliarBancaria()
        {
            return View(new ResetControlFinanciero());
        }
        [HttpPost]
        public ActionResult TablaConciliacion(int idCtaBancaria,Int16 idMes, Int32? estatus)
        {
            CA_CierreBanco cierre = DALCierreBanco.Get(x => x.Cerrado_Conciliado.Value == false && x.Id_CtaBancaria == idCtaBancaria).OrderBy(x => x.Id_Mes).FirstOrDefault();
            if (cierre.Id_Mes == idMes)
                ViewBag.acciones = true;
            else
                ViewBag.acciones = false;
            if(estatus.HasValue)
            {
                string cadena = Diccionarios.EstatusMovimientoBancario[estatus.Value];
                return View(DALVConciliacion.Get(x => x.Id_CtaBancaria == idCtaBancaria && x.Id_Mes == idMes && x.Estatus == cadena).ToList());
            }
            else
                return View(DALVConciliacion.Get(x => x.Id_CtaBancaria == idCtaBancaria && x.Id_Mes == idMes).ToList());
        }
        [HttpPost]
        public JsonResult getSaldo(int idCtaBancaria, Int16 idMes)
        {
            try
            {
                return Json(new { Exito = true, Saldo = DALCierreBanco.GetByID(x => x.Id_CtaBancaria == idCtaBancaria && x.Id_Mes==idMes).Saldo_EdoCta });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        public ActionResult AgregarConcilia(Int16 idCtaBancaria)
        {
            CA_CierreBanco cierre = DALCierreBanco.Get(x => x.Cerrado_Conciliado.Value == false && x.Id_CtaBancaria == idCtaBancaria).OrderBy(x => x.Id_Mes).FirstOrDefault();
            Ca_CuentasBancarias cuenta = DALCuentasBancarias.GetByID(x => x.Id_CtaBancaria == idCtaBancaria);
            Ma_MovimientosConciliacionModel model = new Ma_MovimientosConciliacionModel();
            model.Id_CtaBancaria = idCtaBancaria;
            model.NoCuenta = cuenta.Id_Cuenta;
            model.Id_CuentaContable = cuenta.Id_Cuenta;
            model.Id_Mes = cierre.Id_Mes;
            model.fechaInicial = new DateTime(DateTime.Now.Year, cierre.Id_Mes, 1);
            model.fechaFin = new DateTime(DateTime.Now.Year, cierre.Id_Mes+1, 1).AddDays(-1);
            return View(model);
        }
        [HttpPost]
        public ActionResult AgregarConcilia(Ma_MovimientosConciliacionModel model)
        {
            try
            {
                //Ca_TipoMovBancarios tipo = DALTipoMovBancarios.GetByID(x => x.Id_MovBancario == model.Id_TipoMovimiento);
                Ma_MovimientosConciliacion entity = EntityFactory.getEntity<Ma_MovimientosConciliacion>(model,new Ma_MovimientosConciliacion());
                //entity.Id_TipoMovimientoBancario = tipo.Id_TipoMovB;
                //entity.Id_FolioMovimienotBancario = tipo.Id_FolioMovB;
                entity.Id_FolioMovimiento_Original = entity.Id_FolioMovimienotBancario;
                entity.Id_TipoMovimiento_Original = entity.Id_TipoMovimientoBancario;
                entity.Estatus = 1;
                entity.Origen = 3;
                entity.Capturado = true;
                if (DALMaMovimientosConciliacion.Get(x => x.Id_Mes == entity.Id_Mes && x.Id_CtaBancaria == entity.Id_CtaBancaria && x.NoCuenta == entity.NoCuenta).Count() > 0)
                    entity.No_Registro = Convert.ToInt16(DALMaMovimientosConciliacion.Get(x => x.Id_Mes == entity.Id_Mes && x.Id_CtaBancaria == entity.Id_CtaBancaria && x.NoCuenta == entity.NoCuenta).Max(x => x.No_Registro) + 1);
                else
                    entity.No_Registro = 1;
                DALMaMovimientosConciliacion.Insert(entity);
                DALMaMovimientosConciliacion.Save();
                VW_Conciliacion vistaEntity=DALVConciliacion.GetById(x => x.Id_Mes == entity.Id_Mes && x.Id_CtaBancaria == entity.Id_CtaBancaria && x.No_Registro == entity.No_Registro);
                VW_ConciliacionModel vistaModel = ModelFactory.getModel<VW_ConciliacionModel>(vistaEntity, new VW_ConciliacionModel());
                vistaModel.Abonos_formato = String.Format("{0:N}", vistaModel.Abonos);
                vistaModel.Cargos_formato = String.Format("{0:N}", vistaModel.Cargos);
                return Json(new { Exito = true, Mensaje = "Registro guardado correctamente.", Registro = vistaModel });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
            
        }
        public ActionResult eliminarConciliacion(Int32 NoRegistro, Int32 idCtaBancaria, Int16 idMes)
        {
            try
            {
                Ma_MovimientosConciliacion entity = DALMaMovimientosConciliacion.GetByID(x => x.No_Registro == NoRegistro && x.Id_CtaBancaria == idCtaBancaria && x.Id_Mes == idMes);
                if (entity.Fecha_Conciliacion.HasValue)
                {
                    CA_CierreBanco cierre = DALCierreBanco.Get(x => x.Cerrado_Conciliado.Value == false && x.Id_CtaBancaria == idCtaBancaria).OrderBy(x => x.Id_Mes).FirstOrDefault();
                    CA_Parametros parametro = DALParametros.GetByID(x => x.Nombre == "Ejercicio");
                    if (cierre.Id_Mes == entity.Fecha_Conciliacion.Value.Month && Convert.ToInt32(parametro.Valor) == entity.Fecha_Conciliacion.Value.Year)
                    {
                        DALMaMovimientosConciliacion.Delete(x => x.No_Registro == NoRegistro && x.Id_CtaBancaria == idCtaBancaria && x.Id_Mes == idMes);
                        DALMaMovimientosConciliacion.Save();
                        return Json(new { Exito = true, Mensaje = "Registro eliminado correctamente." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { Exito = false, Mensaje = "El mes del registro que desea eliminar ya fue cerrado." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DALMaMovimientosConciliacion.Delete(x => x.No_Registro == NoRegistro && x.Id_CtaBancaria == idCtaBancaria && x.Id_Mes == idMes);
                    DALMaMovimientosConciliacion.Save();
                    return Json(new { Exito = true, Mensaje = "Registro eliminado correctamente." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult verPoliza(Int32 NoRegistro, Int32 idCtaBancaria, Int16 idMes)
        {
                Ma_MovimientosConciliacion entity = DALMaMovimientosConciliacion.GetByID(x => x.No_Registro == NoRegistro && x.Id_CtaBancaria == idCtaBancaria && x.Id_Mes == idMes);
                if(entity.Id_FolioPoliza.HasValue && entity.Id_TipoPoliza.HasValue && entity.Id_MesPoliza.HasValue)
                    return Json(new { Exito = true, Registro = entity }, JsonRequestBehavior.AllowGet);
                return Json(new { Exito = false, Registro = entity }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult ValidarMenu(Int32 NoRegistro, Int32 idCtaBancaria, Int16 idMes)
        {
            try
            {
                Ma_MovimientosConciliacion entity = DALMaMovimientosConciliacion.GetByID(x => x.No_Registro == NoRegistro && x.Id_CtaBancaria == idCtaBancaria && x.Id_Mes == idMes);
                CA_CierreBanco cierre = DALCierreBanco.Get(x => x.Cerrado_Conciliado.Value == false && x.Id_CtaBancaria == idCtaBancaria).OrderBy(x => x.Id_Mes).FirstOrDefault();
                List<Ca_TipoMovBancarios> entities = new List<Ca_TipoMovBancarios>();
                bool menu = false;
                string mensaje = "none";
                if (entity.Id_Mes == cierre.Id_Mes)
                {
                    //opcion 1
                    if (entity.Estatus == 1)
                    {
                        menu = true;
                        mensaje = "border";
                        Ca_TipoMovBancarios temp = new Ca_TipoMovBancarios();
                        temp.Id_TipoMovB = 2;
                        temp.Id_FolioMovB = 0;
                        temp.Descripcion = "CONCILIADO CORRECTO";
                        entities.Add(temp);
                        DALTipoMovBancarios.Get(x => x.Id_TipoMovB == 3).ToList().ForEach(x => { entities.Add(x); });
                        
                    }
                    //opcion 2
                    if (entity.Estatus == 2 && (entity.Origen==2 || entity.Origen==3))
                    {
                        menu = true;
                        entities = DALTipoMovBancarios.Get(x => x.Id_TipoMovB == 3).ToList();
                    }
                    //opcion 3
                    if ((entity.Estatus == 2 || entity.Estatus == 3) && entity.Origen == 1)
                    {
                        menu = true;
                        Ca_TipoMovBancarios temp = new Ca_TipoMovBancarios();
                        temp.Id_TipoMovB = 1;
                        temp.Id_FolioMovB = 0;
                        temp.Descripcion = "POR REVISAR";
                        entities.Add(temp);
                    }
                    //opcion 4
                    if (entity.Estatus == 3 && (entity.Origen == 2 || entity.Origen == 3))
                    {
                        menu = true;
                        Ca_TipoMovBancarios temp = new Ca_TipoMovBancarios();
                        temp.Id_TipoMovB = 2;
                        temp.Id_FolioMovB = 0;
                        temp.Descripcion = "CONCILIADO CORRECTO";
                        entities.Add(temp);
                    }
                }
                return Json(new { Exito = true,Menu=menu, Mensaje = mensaje,Elementos=entities }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult changeStatus(Int16 Id_Tipo, Int16 Id_Folio, Int32 NoRegistro, Int32 idCtaBancaria, Int16 idMes)
        {
            try
            {
                Ma_MovimientosConciliacion entity = DALMaMovimientosConciliacion.GetByID(x => x.No_Registro == NoRegistro && x.Id_CtaBancaria == idCtaBancaria && x.Id_Mes == idMes);
                if (Id_Tipo == 1)
                {
                    entity.Estatus = 1;
                    entity.Fecha_Conciliacion = null;
                    entity.Id_FolioMovimienotBancario = entity.Id_FolioMovimiento_Original;
                    entity.Id_TipoMovimientoBancario = entity.Id_TipoMovimiento_Original;
                }
                if (Id_Tipo == 2 )
                {
                    entity.Estatus = 2;
                    entity.Fecha_Conciliacion = DateTime.Now;
                }
                if(Id_Tipo==3)
                {
                    entity.Id_FolioMovimienotBancario = Id_Folio;
                    entity.Id_TipoMovimientoBancario = Id_Tipo;
                    entity.Estatus = 3;
                    entity.Fecha_Conciliacion = null;
                }
                DALMaMovimientosConciliacion.Update(entity);
                DALMaMovimientosConciliacion.Save();
                VW_Conciliacion vistaEntity = DALVConciliacion.GetById(x => x.Id_Mes == entity.Id_Mes && x.Id_CtaBancaria == entity.Id_CtaBancaria && x.No_Registro == entity.No_Registro);
                VW_ConciliacionModel vistaModel = ModelFactory.getModel<VW_ConciliacionModel>(vistaEntity, new VW_ConciliacionModel());
                vistaModel.Abonos_formato = String.Format("{0:N}", vistaModel.Abonos);
                vistaModel.Cargos_formato = String.Format("{0:N}", vistaModel.Cargos);
                return Json(new { Exito = true, Mensaje = "Todo bien", Registro = vistaModel }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult saveSaldo(ResetControlFinanciero model)
        {
            try
            {
                CA_CierreBanco entity =DALCierreBanco.GetByID(x=>x.Id_CtaBancaria==model.IdCtaBancaria && x.Id_Mes==model.IdMes);
                if (!entity.Cerrado_Conciliado.Value)
               {
                   entity.Saldo_EdoCta = model.Saldo;
                   DALCierreBanco.Update(entity);
                   DALCierreBanco.Save();
                   return Json(new { Exito = true, Mensaje = "Todo bien" }, JsonRequestBehavior.AllowGet);
               }
               return Json(new { Exito = false, Mensaje = "El mes se encuentra cerrado" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult conciliarCheques(ResetControlFinanciero model)
        {
            try
            {
               CA_CierreBanco entity =DALCierreBanco.GetByID(x=>x.Id_CtaBancaria==model.IdCtaBancaria && x.Id_Mes==model.IdMes);
               if (!entity.Cerrado_Conciliado.Value)
               {
                    List<Ma_MovimientosConciliacion> lista = DALMaMovimientosConciliacion.Get(x=>x.Id_CtaBancaria==model.IdCtaBancaria && x.Id_Mes== model.IdMes && x.Id_TipoMovimientoBancario==2 && x.Id_FolioMovimienotBancario==1 && x.Estatus==1 && x.Origen==1).ToList();
                    foreach (Ma_MovimientosConciliacion item in lista)
                    {
                        item.Id_FolioMovimienotBancario = 1;
                        item.Id_TipoMovimientoBancario = 3;
                        item.Estatus = 3;
                        DALMaMovimientosConciliacion.Update(item);
                        DALMaMovimientosConciliacion.Save();
                    }
                    return Json(new { Exito = true, Mensaje = "todo bien" }, JsonRequestBehavior.AllowGet);
               }
               return Json(new { Exito = false, Mensaje = "El mes se encuentra cerrado" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ValidarMes(Int32 idCtaBancaria,Int16 idMes)
        {
            try
            {
                CA_CierreBanco entity = DALCierreBanco.GetByID(x => x.Id_CtaBancaria == idCtaBancaria && x.Id_Mes == idMes);
                if (!entity.Cerrado_Conciliado.Value)
                {
                    return Json(new { Exito = true, Mensaje = "Todo bien" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Exito = false, Mensaje = "El mes se encuentra cerrado" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Conciliación Automática
        public ActionResult ArvhicoEstadosCuenta()
        {
            var error = Session["error"];
            Session.Remove("error");
            var exito = Session["Exito"];
            Session.Remove("Exito");
            List<Ma_EstadosCuenta> lista = (List<Ma_EstadosCuenta>)Session["listaConciliacion"];
            Session.Remove("listaConciliacion");
            if (exito != null)
                ViewBag.Exito = exito;
            if (error != null)
            {
                if (!String.IsNullOrEmpty(error.ToString()))
                    ModelState.AddModelError("", error.ToString());
            }
            if (lista != null && lista.Count() > 0)
                return View(lista.OrderByDescending(x => x.Fecha));

            return View(new List<Ma_EstadosCuenta>());
        }
        [HttpPost]
        public ActionResult ArvhicoEstadosCuenta(FormCollection form,Int16 banco,Int16 mes)
        {
            try
            {
                Session.Add("error", "");
                Session.Add("exito", false);
                List<Ma_EstadosCuenta> listaConciliacion = new List<Ma_EstadosCuenta>();
                if (Request.Files["archivoExcel"].ContentLength > 0)
                {
                    Uri url = new Uri(ConfigurationManager.AppSettings.Get("urlFtp"));
                    string usuario = ConfigurationManager.AppSettings.Get("ftpUser");
                    string password = ConfigurationManager.AppSettings.Get("ftpPass");
                    Ftp ftp = new Ftp(url, usuario, password);
                    var file = Request.Files["archivoExcel"];
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    if (extension.ToLower() == ".txt")
                    {
                        ftp.ExisteDirectorio(ruta);
                        string nombreArchivo = ruta + "/" + file.FileName;
                        if (!ftp.UploadFTP(file.InputStream, nombreArchivo))
                            throw new ArgumentException("No es posible conectar con el servidor");
                        Byte[] archivo;
                        archivo = ftp.DownloadFTPtoByte(nombreArchivo);
                        string stringArchivo = System.Text.Encoding.UTF8.GetString(archivo);//convertir array de bytes a string
                        string[] elementos = stringArchivo.Split('\n');//seperar cada elemento del archivp
                        BLControlFinanciero.ValidarCuentas(elementos, mes);//validar si ningun mes esta cerrado
                        if(DALBancos.GetByID(x=>x.Id_Banco==banco).Descripcion.ToUpper().Contains("SANTANDER"))
                            listaConciliacion = BLControlFinanciero.ProcesarSantander(elementos,mes);
                        else
                            throw new ArgumentException(String.Format("No existe layaout registrado para el banco {0}.", DALBancos.GetByID(x => x.Id_Banco == banco).Descripcion));
                        ftp.EliminarArchivo(nombreArchivo);
                    }
                    else
                        throw new ArgumentException("El archivo no es un documento txt.");
                }
                Session.Add("listaConciliacion", listaConciliacion);
                return RedirectToAction("ArvhicoEstadosCuenta");
            }
            catch (Exception ex)
            {
                var sesionError = Session["ErrorSQL"];
                Session.Remove("ErrorSQL");
                string error = "";
                if (sesionError != null)
                {
                    error = new Errores(ex.HResult, ex.Message).Mensaje;
                }
                else
                {
                    switch (ex.HResult)
                    {
                        default:
                            error = String.Format(ex.Message);
                            break;
                    }
                }

                Session["error"] = String.Format("Ocurrió un error: {0}", error);
                return RedirectToAction("ArvhicoEstadosCuenta");
            }
        }
        [HttpPost]
        public ActionResult CleanEstadosCuenta(Int16 banco, Int16 mes)
        {
            try
            {
                BLControlFinanciero.CleanEstadosCuenta(banco,mes);
                return Json(new { Exito = true, Mensaje = "Operación realizada con éxito." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        public ActionResult ConciliacionAutomatica()
        {
            return View(new BusquedaConciliacionAuto());
        }
        [HttpPost]
        public ActionResult TablaConciliacionAuto(BusquedaConciliacionAuto busqueda)
        {
            return View(DALEstadosCuenta.Get(x=>x.Id_CuentaBancaria==busqueda.IdCtaBancaria && x.IdMes==busqueda.IdMes).ToList());
        }
        
        [HttpPost]
        public ActionResult StartConciliacion(BusquedaConciliacionAuto busqueda)
        {
            try
            {
                BLControlFinanciero.startConciliacion(busqueda);
                return Json(new { Exito = true, Mensaje = "Conciliación realizada con éxito." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult StartConciliacionDepositos(BusquedaConciliacionAuto busqueda)
        {
            try
            {
                BLControlFinanciero.startConciliacionDepositos(busqueda);
                return Json(new { Exito = true, Mensaje = "Conciliación realizada con éxito." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult RevertConciliacion(BusquedaConciliacionAuto busqueda)
        {
            try
            {
                BLControlFinanciero.revertConciliacion(busqueda);
                return Json(new { Exito = true, Mensaje = "Operación realizada con éxito." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = ex.Message });
            }
        }
        
        
        #endregion

        #region Consulta y reportes
        public ActionResult V_AnalisisMovimientosBancarios(Byte? origen)
        {
            if (origen.HasValue)
                ViewBag.Origen = 1;
            else
                ViewBag.Origen = 0;
            return View(new ResetControlFinanciero());
        }
        public ActionResult ReporteMovimientosBancarios(ResetControlFinanciero busqueda)
        {
            List<VW_Conciliacion> entities = DALVConciliacion.Get(x => (busqueda.IdCtaBancaria > 0 ? x.Id_CtaBancaria == busqueda.IdCtaBancaria : x.Id_CtaBancaria != null) &&
                                                                (busqueda.IdMes.HasValue ? x.Id_Mes==busqueda.IdMes : x.Id_Mes != null) &&
                                                                (busqueda.TipoMovimiento.HasValue ? x.Id_TipoMovB == busqueda.TipoMovimiento : x.Id_TipoMovB != null) &&
                                                                (busqueda.FolioMovimiento.HasValue ? x.Id_FolioMovB == busqueda.FolioMovimiento : x.Id_FolioMovB != null) &&
                                                                (busqueda.Desde.HasValue ? x.Cargos >= busqueda.Desde || x.Abonos >=busqueda.Desde : x.Cargos == null || x.Abonos ==null) &&
                                                                (busqueda.Hasta.HasValue ? x.Cargos <= busqueda.Hasta || x.Abonos <= busqueda.Hasta : x.Cargos == null || x.Abonos == null)).ToList();

            if (this.HttpContext.Request.IsAjaxRequest())
            {
                ViewBag.Imprimir = false;
                return View(entities);
            }
            else
            {
                ConvertHtmlToString pdf = new ConvertHtmlToString();
                ViewBag.Imprimir = true;
                return File(pdf.GenerarPDF("ReporteMovimientosBancarios", entities, this.ControllerContext), "application/pdf");
            }
        }
        public ActionResult V_AnalisisEstadosCuenta()
        {
            return View(new BusquedaConciliacionAuto());
        }
        public ActionResult ReporteEstadosCuenta(BusquedaConciliacionAuto busqueda)
        {
            List<Ma_EstadosCuenta> entities = DALEstadosCuenta.Get(x=>(busqueda.IdCtaBancaria.HasValue ? x.Id_CuentaBancaria==busqueda.IdCtaBancaria : x.IdRegistro != null) &&
                                                                    (busqueda.IdMes.HasValue ? x.Fecha.Value.Month==busqueda.IdMes : x.IdRegistro != null) &&
                                                                    (busqueda.Estatus.HasValue ? x.Conciliado==busqueda.Estatus : x.IdRegistro != null) &&
                                                                    (busqueda.Desde.HasValue ? x.Cargos >= busqueda.Desde || x.Abonos >= busqueda.Desde : x.Cargos == null || x.Abonos == null) &&
                                                                (busqueda.Hasta.HasValue ? x.Cargos <= busqueda.Hasta || x.Abonos <= busqueda.Hasta : x.Cargos == null || x.Abonos == null)).ToList();

            if (this.HttpContext.Request.IsAjaxRequest())
            {
                ViewBag.Imprimir = false;
                return View(entities);
            }
            else
            {
                ConvertHtmlToString pdf = new ConvertHtmlToString();
                ViewBag.Imprimir = true;
                return File(pdf.GenerarPDF("ReporteEstadosCuenta", entities, this.ControllerContext), "application/pdf");
            }
        }
        public ActionResult V_AnalisisSaldosBancarios()
        {
            return View(new ResetControlFinanciero());
        }


        public ActionResult ReporteSaldosBancarios(Int32? IdCtaBancaria, Int16? IdMes)
        {
            List<CA_CierreBanco> entities = DALCierreBanco.Get(x => (IdCtaBancaria.HasValue ? x.Id_CtaBancaria == IdCtaBancaria : x.Id_CtaBancaria != null) &&
                                                                (IdMes.HasValue ? x.Id_Mes == IdMes : x.Id_Mes != null)).ToList();
            List<Ca_CierreBancoModel> models = new List<Ca_CierreBancoModel>();
            foreach(CA_CierreBanco item in entities)
            {
                Ca_CierreBancoModel temp = ModelFactory.getModel<Ca_CierreBancoModel>(item, new Ca_CierreBancoModel());
                temp.Ca_CuentasBancarias = new Llenado().LLenado_CaCuentasBancarias(item.Id_CtaBancaria);
                models.Add(temp);
            }
            if (this.HttpContext.Request.IsAjaxRequest())
            {
                ViewBag.Imprimir = false;
                return View(models);
            }
            else
            {
                ConvertHtmlToString pdf = new ConvertHtmlToString();
                ViewBag.Imprimir = true;
                return File(pdf.GenerarPDF("ReporteSaldosBancarios", models, this.ControllerContext), "application/pdf");
            }
        }
        public ActionResult V_AnalisisConciliacionBancaria()
        {
            return View(new ResetControlFinanciero());
        }


        public ActionResult ReporteConciliacionBancaria(Int32? IdCtaBancaria, Int16? IdMes)
        {
            ReporteConciliacion model = new ReporteConciliacion();
            model.ListaSaldos = DALProcedures.PA_ConciliacionReporte(IdCtaBancaria, IdMes);
            CA_CierreBanco cierre = DALCierreBanco.GetByID(x => x.Id_CtaBancaria == IdCtaBancaria && x.Id_Mes == IdMes);
            if (!cierre.Saldo_EdoCta.HasValue)
                cierre.Saldo_EdoCta = 0;
            CA_CierreBanco cierreAnterior= new CA_CierreBanco();
            decimal abonos = 0, cargos = 0;
            foreach (tblRepConciliacion item in model.ListaSaldos.Where(x => x.Id_TipoMovimientoBancario == 3))
            {
                if (DALTipoMovBancarios.GetByID(x => x.Id_FolioMovB == item.Id_FolioMovimienotBancario && x.Id_TipoMovB == item.Id_TipoMovimientoBancario).TipoMov == 1)
                {
                    if (item.Total.HasValue)
                        abonos += item.Total.Value;
                    model.ListaAbonos.Add(item);
                }
                else
                {
                    if (item.Total.HasValue)
                        cargos += item.Total.Value;
                    model.ListaCargos.Add(item);
                }
                        
            }
            if (!cierre.Cerrado_Conciliado.Value)
            {
                cierre.Abonos_Conciliacion = abonos;
                cierre.Cargos_Conciliacion = cargos;
                if (IdMes > 1)
                {
                    cierreAnterior = DALCierreBanco.GetByID(x => x.Id_CtaBancaria == IdCtaBancaria && x.Id_Mes == IdMes - 1);
                    if (!cierreAnterior.Saldo_EdoCta.HasValue)
                        cierreAnterior.Saldo_EdoCta = 0;
                    //Guardar saldo incial 
                    cierre.Saldo_Inicial = cierreAnterior.Saldo_EdoCta + cierreAnterior.Abonos_Conciliacion - cierreAnterior.Cargos_Conciliacion;
                }
                else
                    cierre.Saldo_Inicial = 0;//Guardar saldo incial cuando el mes es enero
                
                //Guardar Retiros= la suma de los mas depositos(tipo 1)
                cierre.Retiros = model.ListaSaldos.Where(x => x.Total != null && x.Id_TipoMovimientoBancario == 1).Sum(x => x.Total);
                //Guardar Depositos= la suma de los mas retiros(tipo 2)
                cierre.Depositos = model.ListaSaldos.Where(x => x.Total != null && x.Id_TipoMovimientoBancario == 2).Sum(x => x.Total);
                DALCierreBanco.Update(cierre);
                DALCierreBanco.Save();
            }
            model.SaldoEstadoCuenta = cierre.Saldo_EdoCta;
            model.SaldoMesAnterior = cierre.Saldo_Inicial;
            model.Cargos = cargos;
            model.Abonos = abonos;
            if (this.HttpContext.Request.IsAjaxRequest())
            {
                ViewBag.Imprimir = false;
                return View(model);
            }
            else
            {
                ConvertHtmlToString pdf = new ConvertHtmlToString();
                ViewBag.Imprimir = true;
                return File(pdf.GenerarPDF("ReporteConciliacionBancaria", model, this.ControllerContext), "application/pdf");
            }
        }
        #endregion
    }
    public static class EnumerableEx
    {
        public static IEnumerable<string> SplitBy(this string str, int chunkLength)
        {
            if (String.IsNullOrEmpty(str)) throw new ArgumentException();
            if (chunkLength < 1) throw new ArgumentException();

            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength);
            }
        }
    }
}
