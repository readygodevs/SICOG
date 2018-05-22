using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Transactions;
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
    public class EgresosController : Controller
    {
        MaCompromisosDAL cDal;
        MaContrarecibosDAL contrarecibosDal;
        protected CuentasBancariasDAL cuentasBancariasDAL { get; set; }
        protected DeBancoDAL debancoDAL { get; set; }
        protected DeBancoChequeDAL deBanchoChequeDAL { get; set; }
        protected Listas repoListas { get; set; }
        protected ProceduresDAL proceduresDAL { get; set; }
        protected Llenado llenar { get; set; }
        protected ConvertHtmlToString reports { get; set; }
        private ContraRecibosDAL vContrarecibosDAL { get; set; }
        protected VW_ProvedoresUsadosDAL vProveedoresUsadosDAL { get; set; }
        protected VW_ProvedoresUsadosAgrupadosDAL vProveedoresUsadosAgrupadosDAL { get; set; }
        protected CierreMensualDAL DALCierreMensual { get; set; }
        protected ParametrosDAL DALParametros { get; set; }

        public EgresosController()
        {
            if (cDal == null) cDal = new MaCompromisosDAL();
            if (repoListas == null) repoListas = new Listas();
            if (contrarecibosDal == null) contrarecibosDal = new MaContrarecibosDAL();
            if (debancoDAL == null) debancoDAL = new DeBancoDAL();
            if (deBanchoChequeDAL == null) deBanchoChequeDAL = new DeBancoChequeDAL();
            if (cuentasBancariasDAL == null) cuentasBancariasDAL = new CuentasBancariasDAL();
            if (proceduresDAL == null) proceduresDAL = new ProceduresDAL();
            if (llenar == null) llenar = new Llenado();
            if (reports == null) reports = new ConvertHtmlToString();
            if (vContrarecibosDAL == null) vContrarecibosDAL = new ContraRecibosDAL();
            if (vProveedoresUsadosDAL == null) vProveedoresUsadosDAL = new VW_ProvedoresUsadosDAL();
            if (vProveedoresUsadosAgrupadosDAL == null) vProveedoresUsadosAgrupadosDAL = new VW_ProvedoresUsadosAgrupadosDAL();
            if (DALCierreMensual == null) DALCierreMensual = new CierreMensualDAL();
            if (DALParametros == null) DALParametros = new ParametrosDAL();
        }

        public ActionResult Index()
        {
            return View();
        }


        #region Compromisos
        public ActionResult AutorizacionCompromisos()
        {
            List<Ma_Compromisos> compromiso = cDal.Get(x => x.Estatus == Diccionarios.ValorEstatus.AUTORIZACION).ToList();
            List<Ma_CompromisosModel> modelCompromiso = new List<Ma_CompromisosModel>();
            Llenado llenado = new Llenado();
            foreach (Ma_Compromisos m in compromiso)
            {
                modelCompromiso.Add(llenado.LLenado_MaCompromisos(m.Id_TipoCompromiso, m.Id_FolioCompromiso));
            }
            return View(modelCompromiso);
        }

        [HttpPost]
        public ActionResult ConsultarCompromisos(short TipoCompromiso, int FolioCompromiso)
        {
            ReturnMaster regreso = new ReturnMaster();
            regreso.Accion = "AutorizacionCompromisos";
            regreso.Controlador = "Egresos";
            regreso.Parametros = new List<Campos>();
            return Json(new { Exito = true, Parametros = new { TipoCompromiso = TipoCompromiso, FolioCompromiso = FolioCompromiso, args = regreso } });
        }

        [HttpPost]
        public ActionResult GetPartidasSinDisponibilidad(int idTipoCompromiso, int idFolioCompromiso)
        {
            DeCompromisosDAL cDal = new DeCompromisosDAL();
            List<De_Compromisos> listaCompromiso = cDal.Get(x => x.AfectaCompro == false && x.Id_Movimiento == Diccionarios.ValorMovimientos.CARGO && x.Disponibilidad == false && x.Id_TipoCompromiso == idTipoCompromiso && x.Id_FolioCompromiso == idFolioCompromiso).ToList();
            return View(listaCompromiso);
        }

        [HttpPost]
        public ActionResult AutorizarPartidas(short? idTipoCompromiso, int? idFolioCompromiso)
        {
            try
            {
                CierreMensualDAL cierreDal = new CierreMensualDAL();
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                short? idUsuario = (short?)appUsuario.IdUsuario;
                MaCompromisosDAL maDal = new MaCompromisosDAL();
                Ma_Compromisos compromiso = maDal.GetByID(x => x.Id_TipoCompromiso == idTipoCompromiso && x.Id_FolioCompromiso == idFolioCompromiso);
                UsuarioLogueado Usuario = Session["appUsuario"] as UsuarioLogueado;
                if (new CompromisosBL().isClosed(compromiso.Fecha_Orden.Value))
                    return Json(new { Exito = false, Mensaje = "Las partidas no se pueden autorizar, porque el mes se encuentra cerrado" });
                if (compromiso != null)
                {
                    new ProceduresDAL().Pa_AutorizarPartidasCompromiso(idTipoCompromiso.Value, idFolioCompromiso, idUsuario);
                    compromiso.Estatus = 1;
                    //Si el compromiso es de nómina 
                    if (compromiso.No_Nomina != null || !String.IsNullOrEmpty(compromiso.No_Nomina))
                    {
                        if (compromiso.Id_FolioPO_Comprometido.HasValue)
                            new ProceduresDAL().PA_ActualizaPoliza_Orden_Comprometido(compromiso.Id_TipoCompromiso, compromiso.Id_FolioCompromiso, (short)Usuario.IdUsuario);
                        else
                        {
                            String[] res = new ProceduresDAL().Pa_Genera_PolizaOrden_Comprometido(compromiso.Id_TipoCompromiso, compromiso.Id_FolioCompromiso, (short)Usuario.IdUsuario);
                            compromiso.Id_MesPO_Comprometido = Convert.ToByte(res[0]);
                            compromiso.Id_FolioPO_Comprometido = Convert.ToInt16(res[1]);
                            compromiso.Id_FolioPO_Comprometido = Convert.ToInt16(res[1]);
                            compromiso.Id_MesPO_Comprometido = Convert.ToByte(res[0]);
                            compromiso.Usuario_Autorizo = appUsuario.NombreCompleto;
                            compromiso.Fecha_Autorizo = DateTime.Now;
                            compromiso.Usuario_Act = (short)appUsuario.IdUsuario;
                            compromiso.Fecha_Act = DateTime.Now;
                        }
                        maDal.Update(compromiso);
                        maDal.Save();
                    }
                    else
                    {
                        String[] res = new ProceduresDAL().Pa_Genera_PolizaOrden_Comprometido(compromiso.Id_TipoCompromiso, compromiso.Id_FolioCompromiso, (short)Usuario.IdUsuario);
                        compromiso.Id_FolioPO_Comprometido = Convert.ToInt16(res[1]);
                        compromiso.Id_MesPO_Comprometido = Convert.ToByte(res[0]);
                        compromiso.Usuario_Autorizo = appUsuario.NombreCompleto;
                        compromiso.Fecha_Autorizo = DateTime.Now;
                        compromiso.Usuario_Act = (short)appUsuario.IdUsuario;
                        compromiso.Fecha_Act = DateTime.Now;
                        maDal.Update(compromiso);
                        maDal.Save();
                    }
                }
                return Json(new { Exito = true, Mensaje = "Partidas autorizadas correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult RechazarCompromiso(short? idTipoCompromiso, int? idFolioCompromiso)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                short? idUsuario = (short?)appUsuario.IdUsuario;
                new ProceduresDAL().PA_Cancelar_Compromiso(idTipoCompromiso, idFolioCompromiso, idUsuario);
                return Json(new { Exito = true, Mensaje = "Compromiso rechazado correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        #endregion

        #region Contrarecibos
        public ActionResult AutorizacionContrarecibos()
        {
            List<Ma_Contrarecibos> compromiso = contrarecibosDal.Get(x => x.Id_EstatusCR == Diccionarios.Valores_Estatus_CR.Autorización).ToList();
            List<Ma_ContrarecibosModel> modelContrarecibos = new List<Ma_ContrarecibosModel>();
            Llenado llenado = new Llenado();
            foreach (Ma_Contrarecibos m in compromiso)
            {
                modelContrarecibos.Add(llenado.LLenado_MaContrarecibos(m.Id_TipoCR, m.Id_FolioCR));
            }
            return View(modelContrarecibos);
        }

        [HttpPost]
        public ActionResult GetPartidasSinDisponibilidadCR(int idTipoCR, int idFolioCR)
        {
            DeContrarecibosDAL crDal = new DeContrarecibosDAL();
            List<De_Contrarecibos> listaContrarecibos = crDal.Get(x => x.Disponibilidad == false && x.Id_Movimiento == Diccionarios.ValorMovimientos.CARGO && x.Id_TipoCR == idTipoCR && x.Id_FolioCR == idFolioCR).ToList();
            return View(listaContrarecibos);
        }

        [HttpPost]
        public ActionResult AutorizarPartidasCR(short? idTipoCR, int? idFolioCR)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                BD_TesoreriaEntities bd = new BD_TesoreriaEntities();
                short? idUsuario = (short?)appUsuario.IdUsuario;
                MaContrarecibosDAL mDal = new MaContrarecibosDAL();
                Ma_Contrarecibos contrarecibo = mDal.GetByID(x => x.Id_TipoCR == idTipoCR && x.Id_FolioCR == idFolioCR);
                contrarecibo.Id_EstatusCR = Diccionarios.Valores_Estatus_CR.Pagado;
                contrarecibo.Usuario_Act = idUsuario;
                mDal.Save();
                return Json(new { Exito = true, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        [HttpPost]
        public JsonResult getFuente(Int32? idCta)
        {
            try
            {
                if (idCta.HasValue)
                {
                    Ca_CuentasBancarias model = cuentasBancariasDAL.GetByID(x => x.Id_CtaBancaria == idCta);
                    Ca_FuentesFin fuente = new FuenteDAL().GetByID(x => x.Id_Fuente == model.Id_Fuente);
                    return Json(new { Exito = true, Mensaje = String.Format("{0}-{1}", fuente.Id_Fuente, fuente.Descripcion) });
                }
                return Json(new { Exito = true, Mensaje = "" });

            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = "" });
            }
        }
        #endregion

        #region AsignacionCheques
        public ActionResult V_AsignacionCheques()
        {
            ViewBag.Titulo = "Asignación de cheques";
            ViewBag.Vista = 1;
            byte total = 0;
            if (DALCierreMensual.Get(x => x.Contable == true).Count() > 0)
                total = DALCierreMensual.Get(x => x.Contable == true).Last().Id_Mes;
            ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, total + 1, 1).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            return View();
        }
        public JsonResult ListaCheques(string Fecha, string FechaDesde, string FechaHasta, short? IdCta)
        {
            VW_ListaContraRecibosAsignacionChequesDAL dal = new VW_ListaContraRecibosAsignacionChequesDAL();
            if (FechaDesde != null && FechaDesde != "" && FechaDesde != "0")
            {
                DateTime DateInicio = Convert.ToDateTime(FechaDesde);
                DateTime DateFin = Convert.ToDateTime(FechaHasta);
                List<VW_ListaContraRecibosAsignacionCheques> Lista = dal.Get(x => x.FechaVen >= DateInicio && x.FechaVen <= DateFin &&
                    (IdCta.HasValue ? x.Id_CtaBancaria == IdCta.Value : x.Id_CtaBancaria != null)).ToList();
                return Json(new { Lista }, JsonRequestBehavior.AllowGet);

            }
            else if(IdCta.HasValue && IdCta>0)
            {
                List<VW_ListaContraRecibosAsignacionCheques> Lista = dal.Get(x =>
                    (IdCta.HasValue ? x.Id_CtaBancaria == IdCta.Value : x.Id_CtaBancaria != null)).ToList();
                return Json(new { Lista }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetFechaVenc(Int16 radio)
        {
            return new JsonResult() { Data = repoListas.ListaFechaVencimiento(radio) };
        }
        [HttpPost]
        public JsonResult AsignarCheque(Int16 IdCuentaBancaria)
        {
            try
            {

                int total = deBanchoChequeDAL.Get(c => c.Id_CtaBancaria == IdCuentaBancaria).Count();
                if (total > 0)
                {
                    Ca_CuentasBancarias foliador = cuentasBancariasDAL.GetByID(c => c.Id_CtaBancaria == IdCuentaBancaria);
                    return Json(new { Exito = true, Mensaje = "No Ocurrió un error", Foliador = foliador.TipoFoliador });
                }
                else
                    return Json(new { Exito = false, Mensaje = "Esta Cuenta Bancaria no tiene Chequera Asignada." });

            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ValidarCuentaManual(Int16 IdCuentaBancaria, Int16 numeroChecks, int totalImporte)
        {
            try
            {

                Ca_CuentasBancarias cuenta = cuentasBancariasDAL.GetByID(x => x.Id_CtaBancaria == IdCuentaBancaria);
                int inicio = cuenta.NoChequeIni.Value, final = cuenta.NoChequeFin.Value;
                int NumeroChequeLibres = debancoDAL.Get(x => x.Id_CtaBancaria == cuenta.Id_CtaBancaria && x.Id_Estatus == 1 && x.No_Cheque >= inicio && x.No_Cheque <= final).Count();
                if (numeroChecks <= NumeroChequeLibres)
                {
                    return ValidarSaldo(IdCuentaBancaria, totalImporte);
                }
                else
                    return Json(new { Exito = false, Mensaje = "La cuenta solo tiene " + NumeroChequeLibres + " cheques libres" });

            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ValidarSaldo(Int16 IdCuentaBancaria, int totalImporte)
        {
            try
            {
                if (totalImporte <= Convert.ToInt32(proceduresDAL.PA_Estado_Cuenta_Banco(IdCuentaBancaria.ToString())))
                    return Json(new { Exito = true, Importe = true, Mensaje = "Cuenta con suficientes cheques." }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Exito = true, Importe = false, Mensaje = "El importe excede el saldo total del banco. ¿Desea continuar? " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ModalNextChequeManual(Int16 IdCuentaBancaria)
        {
            Ca_CuentasBancarias cuenta = cuentasBancariasDAL.GetByID(x => x.Id_CtaBancaria == IdCuentaBancaria);
            int inicio = cuenta.NoChequeIni.Value, fin = cuenta.NoChequeFin.Value;
            for (int i = inicio; i <= fin; i++)
            {
                DE_Bancos temp = debancoDAL.GetByID(x => x.Id_CtaBancaria == IdCuentaBancaria && x.No_Cheque == i);
                if (temp.Id_Estatus == 1)
                {
                    ViewBag.NoCheque = temp.No_Cheque;
                    return View();
                }
            }
            return View();
        }
        public ActionResult ChequesDisponibles(Int16 IdCuentaBancaria, Int32 numero)
        {
            bool error;
            try
            {
                int Nocheque = debancoDAL.Get(x => x.Id_CtaBancaria == IdCuentaBancaria && x.Id_Estatus == 1).Min(x => x.No_Cheque);
                if (Nocheque > 0)
                {
                    if (numero > Nocheque)
                        return Json(new { Exito = false, Mensaje = "Solo cuenta con " + numero + " cheques disponibles en esta cuenta." });
                    error = true;
                }
                else
                    return Json(new { Exito = false, Mensaje = "No cuenta con cheques disponibles." });
            }
            catch
            {
                error = false;
            }
            return Json(new { Exito = error });
        }
        public ActionResult ModalNextChequeAutomatico(Int16 IdCuentaBancaria)
        {
            int Nocheque = debancoDAL.Get(x => x.Id_CtaBancaria == IdCuentaBancaria && x.Id_Estatus == 1).Min(x => x.No_Cheque);
            ViewBag.NoCheque = Nocheque;

            return View();
        }
        [HttpPost]
        public JsonResult GuardaAsignacionCheque(byte? Id_TipoCR, Int32? Id_FolioCR, short? IdCuentaBancaria, int NoCheque)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                short? idUsuario = (short?)appUsuario.IdUsuario;
                int Nocheque = debancoDAL.Get(x => x.Id_CtaBancaria == IdCuentaBancaria && x.Id_Estatus == 1).Min(x => x.No_Cheque);
                if (proceduresDAL.Pa_Asigna_Cheque(Id_TipoCR, Id_FolioCR, IdCuentaBancaria, Nocheque, idUsuario) == 0)
                    return Json(new { Exito = false, Mensaje = "Ocurrio un error al guardar el Cheque No: " + NoCheque });
                else
                    return Json(new { Exito = true, Mensaje = "ok", NoCheque = Nocheque });

            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GuardaAsignacionChequeAutomatico(byte? Id_TipoCR, Int32? Id_FolioCR, short? IdCuentaBancaria)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                short? idUsuario = (short?)appUsuario.IdUsuario;
                int Nocheque = debancoDAL.Get(x => x.Id_CtaBancaria == IdCuentaBancaria && x.Id_Estatus == 1).Min(x => x.No_Cheque);
                if (proceduresDAL.Pa_Asigna_Cheque(Id_TipoCR, Id_FolioCR, IdCuentaBancaria, Nocheque, idUsuario) == 0)
                    return Json(new { Exito = false, Mensaje = "Ocurrio un error al guardar el Cheque No: " + Nocheque });
                else
                    return Json(new { Exito = true, Mensaje = "ok", NoCheque = Nocheque });

            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region DesAsignacionCheques
        public ActionResult V_DesasigancionCheques()
        {
            return View();
        }
        [HttpPost]
        public ActionResult V_TablaCheques(Int32 IdCuenta)
        {
            return View();
        }
        public JsonResult ListaChequesDesasignacion(Int32? IdCuenta)
        {
            BD_TesoreriaEntities bd = new BD_TesoreriaEntities();
            if (IdCuenta > 0)
            {
                List<vListaChequesDesasignacion> Lista = bd.vListaChequesDesasignacion.Where(x => x.Id_CtaBancaria == IdCuenta).ToList();
                return Json(new { Lista }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<vListaChequesDesasignacion> Lista = bd.vListaChequesDesasignacion.ToList();
                return Json(JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DesasignaCheques(short IdCuenta, int? NoCheque, byte todos)
        {
            try
            {
                BD_TesoreriaEntities bd = new BD_TesoreriaEntities();
                if (todos == 1)
                {
                    proceduresDAL.PA_DesasignaciondeCheques(IdCuenta);
                    return Json(new { Exito = true, Mensaje = "Desasignación terminada" });
                }
                else
                {
                    proceduresDAL.PA_DesasignaciondeCheques_Individual(IdCuenta, NoCheque);
                    return Json(new { Exito = true, Mensaje = "Desasignación terminada" });
                }

            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region CancelacionFormatoCheques
        public ActionResult V_CancelacionFormatoCheques()
        {
            return View();
        }
        public JsonResult ListaDeBancos(Int32? IdCuenta)
        {
            if (IdCuenta > 0)
            {
                List<DE_BancosModel> ModelLst = new List<DE_BancosModel>();
                IEnumerable<DE_Bancos> entities = debancoDAL.Get(x => x.Id_CtaBancaria == IdCuenta && x.Id_Estatus == 1);
                foreach (DE_Bancos item in entities)
                {
                    DE_BancosModel model = ModelFactory.getModel<DE_BancosModel>(item, new DE_BancosModel());
                    if (model.Id_Estatus == 1)
                        model.Estatus = "Sin asignar";
                    if (model.Id_Estatus == 3)
                        model.Estatus = "Formato cancelado";
                    ModelLst.Add(model);
                }
                return Json(new { ModelLst }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult CancelaFormato(short IdCuenta, int? NoCheque, string Desc)
        {
            try
            {
                DE_Bancos banco = debancoDAL.GetByID(x => x.Id_CtaBancaria == IdCuenta && x.No_Cheque == NoCheque);
                banco.Id_Estatus = 3;
                banco.Observa = Desc;
                debancoDAL.Update(banco);
                debancoDAL.Save();
                return Json(new { Exito = true, Mensaje = "Uno por uno" });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult V_ModalDescripCancelacionFormato()
        {
            return View();
        }
        public ActionResult V_ModalBuscarCancelacionFormato()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DivTablaBusqueda(Int32? IdBanco, Int32? IdCuenta, byte? Estatus, Int32? inicio, Int32? fin)
        {
            IEnumerable<DE_Bancos> List = debancoDAL.Get(x => (IdCuenta > 0 ? x.Id_CtaBancaria == IdCuenta : x.Id_CtaBancaria != null) && (Estatus == 0 ? x.Id_Estatus != null : x.Id_Estatus == Estatus) && (inicio > 0 ? x.No_Cheque >= inicio : x.No_Cheque != null) && (fin > 0 ? x.No_Cheque <= fin : x.No_Cheque != null));
            List<DE_BancosModel> ListM = new List<DE_BancosModel>();
            foreach (DE_Bancos item in List)
            {
                DE_BancosModel model = ModelFactory.getModel<DE_BancosModel>(item, new DE_BancosModel());
                ListM.Add(model);
            }
            return View(ListM);
        }
        #endregion

        #region TransferenciaElectronica
        public ActionResult V_TranferenciaElectronica()
        {
            ViewBag.Titulo = "Transferencia Electrónica";
            ViewBag.Vista = 2;
            byte total = 0;
            if (DALCierreMensual.Get(x => x.Contable == true).Count() > 0)
                total = DALCierreMensual.Get(x => x.Contable == true).Last().Id_Mes;
            ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, total + 1, 1).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            return View("V_AsignacionCheques");
        }
        [HttpPost]
        public JsonResult ValidarMes(DateTime FechaPago)
        {
            try
            {
                Ca_CierreMensualModel Model = new Ca_CierreMensualModel();
                if (!Model.validaMes((short)FechaPago.Month))
                    return Json(new { Exito = true, Mensaje = "ok" });
                return Json(new { Exito = false, Mensaje = "El mes de la fecha de pago esta cerrado." });

            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GuardaAsignacionChequeAutomaticoTE(byte? Id_TipoCR, Int32? Id_FolioCR, short? IdCuentaBancaria, DateTime FechaPago)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                short? idUsuario = (short?)appUsuario.IdUsuario;
                //int Nocheque = debancoDAL.Get(x => x.Id_CtaBancaria == IdCuentaBancaria && x.Id_Estatus == 1).Min(x => x.No_Cheque);

                int Nocheque = proceduresDAL.Pa_Asigna_Cheque_TE(Id_TipoCR, Id_FolioCR, IdCuentaBancaria, idUsuario, FechaPago);
                if (Nocheque == 0)
                    return Json(new { Exito = true, Mensaje = "ok", NoCheque = Nocheque });
                else
                    return Json(new { Exito = false, Mensaje = "Ocurrió un Error al Guardar, favor de verificar los datos.", NoCheque = Nocheque });

            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region CancelacionCheque
        public ActionResult V_CancelacionCheque()
        {
            return View();
        }
        public JsonResult ListaChequesCancelacion(Int32? IdCuenta)
        {
            IEnumerable<Ma_Contrarecibos> Lista = contrarecibosDal.Get(x => x.Id_EstatusCR == 2);
            List<Ma_ContrarecibosModel> ListM = new List<Ma_ContrarecibosModel>();
            foreach (Ma_Contrarecibos item in Lista)
            {

                try
                {
                    Ma_ContrarecibosModel model = new Llenado().LLenado_MaContrarecibos(item.Id_TipoCR, item.Id_FolioCR);
                    model.NombreCompleto = model.Ca_Beneficiarios.NombreCompleto;
                    ListM.Add(model);
                }
                catch (Exception ex) { }

            }
            return Json(new { ListM }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult V_ModalFechaCancelacion(string Fecha)
        {
            AmpliacionesReduccionesBL bl = new AmpliacionesReduccionesBL();
            ViewBag.FechaInicio = bl.GetFechaMayor(Convert.ToDateTime(Fecha)).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            return View();
        }
        [HttpPost]
        public JsonResult CancelarCheque(byte? Id_TipoCR, Int32 Id_FolioCR, DateTime FechaC)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                short? idUsuario = (short?)appUsuario.IdUsuario;
                if (new CompromisosBL().isClosed(FechaC))
                    return Json(new { Exito = false, Mensaje = "La fecha se encuetra en un mes cerrado." });
                Ma_Compromisos compromiso = cDal.GetByID(x => x.Id_TipoCR == Id_TipoCR && x.Id_FolioCR == Id_FolioCR);
                Ma_Contrarecibos contrarecibo = contrarecibosDal.GetByID(x => x.Id_TipoCR == Id_TipoCR && x.Id_FolioCR == Id_FolioCR);
                if (FechaC < contrarecibo.Fecha_Pago)
                    return Json(new { Exito = false, Mensaje = "La fecha de cancelación es menor a la fecha de pago." });
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
                    byte id_mes_d = 0;
                    int id_folio_d = 0;
                    proceduresDAL.Pa_Genera_PolizaOrden_Devengado_Cancela(compromiso.Id_TipoCompromiso, compromiso.Id_FolioCompromiso, FechaC, idUsuario, ref id_mes_d, ref id_folio_d);

                    byte id_mes_c = 0;
                    int id_folio_c = 0;
                    proceduresDAL.Pa_Genera_PolizaOrden_Comprometido_Cancela(compromiso.Id_TipoCompromiso, compromiso.Id_FolioCompromiso, FechaC, idUsuario, ref id_mes_c, ref id_folio_c);

                    byte id_mes_diario = 0;
                    int id_folio_diario = 0;
                    proceduresDAL.Pa_Cancelacion_Poliza_Diario_Devengo(compromiso.Id_TipoCompromiso, compromiso.Id_FolioCompromiso, FechaC, idUsuario, ref id_mes_diario, ref id_folio_diario);

                    compromiso.Id_FolioPO_Devengado_C = id_folio_d;
                    compromiso.Id_MesPO_Devengado_C = id_mes_d;
                    compromiso.Id_FolioPO_Comprometido_C = id_folio_c;
                    compromiso.Id_MesPO_Comprometido_C = id_mes_c;
                    compromiso.Id_FolioPoliza_C = id_folio_diario;
                    compromiso.Id_MesPoliza_C = id_mes_diario;
                    compromiso.Fecha_CancelaCompro = FechaC;
                    compromiso.Fecha_CancelaDevengo = FechaC;
                    compromiso.Estatus = Diccionarios.ValorEstatus.CANCELADO;
                    compromiso.Usuario_Cancela = appUsuario.NombreCompleto;
                    compromiso.Fecha_Cancela = FechaC;

                    //contrarecibo
                    byte id_mes_ejercido_c = 0;
                    int id_folio_ejercido_c = 0;
                    proceduresDAL.Pa_Genera_PolizaOrden_Ejercido_Compromiso_Cancela(Id_TipoCR, Id_FolioCR, FechaC, idUsuario, ref id_mes_ejercido_c, ref id_folio_ejercido_c);

                    byte id_mes_pagado_c = 0;
                    int id_folio_pagado_c = 0;
                    proceduresDAL.Pa_Genera_PolizaOrden_Pagado_Cancela(Id_TipoCR, Id_FolioCR, FechaC, idUsuario, ref id_mes_pagado_c, ref id_folio_pagado_c);

                    string[] result = proceduresDAL.PA_Genera_Poliza_Egresos_Cancela(Id_TipoCR.Value, Id_FolioCR, contrarecibo.Id_CtaBancaria, contrarecibo.No_Cheque, FechaC, idUsuario.Value);

                    contrarecibo.Id_FolioPO_Ejercido_C = id_folio_ejercido_c;
                    contrarecibo.Id_MesPO_Ejercido_C = id_mes_ejercido_c;
                    contrarecibo.Id_FolioPO_Pagado_C = id_folio_pagado_c;
                    contrarecibo.Id_MesPO_Pagado_C = id_mes_pagado_c;
                    contrarecibo.Id_FolioPolizaCH_C = Convert.ToInt32(result[1]);
                    contrarecibo.Id_MesPolizaCH_C = Convert.ToByte(result[0]);
                    contrarecibo.Fecha_CancelaEjercido = FechaC;
                    contrarecibo.Fecha_CancelaPagado = FechaC;
                    contrarecibo.Id_MesPolizaCR_C = id_mes_diario;
                    contrarecibo.Id_FolioPolizaCR_C = id_folio_diario;
                    contrarecibo.FechaCancelacionCR = FechaC;
                    cDal.Save();
                    contrarecibosDal.Save();

                    return Json(new { Exito = true, Mensaje = "Cancelado correctamente." });

                }
                return Json(new { Exito = false, Mensaje = "No existe ligado ningun compromiso a esta Cuenta por Liquidar." });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Analisi de CR
        [HttpGet]
        public ActionResult AnalisisContraRecibos()
        {
            Modelos.ContraRecibosAnalisisModel model = new Modelos.ContraRecibosAnalisisModel();
            ViewBag.Ejercicio = new ParametrosDAL().GetByID(x => x.Descripcion == "Ejercicio contable").Valor;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult tblContraRecibos(Modelos.ContraRecibosAnalisisModel dataModel)
        {
            List<VW_ContrarecibosModel> models = new List<VW_ContrarecibosModel>();
            List<VW_Contrarecibos> entities =
            vContrarecibosDAL.Get(reg =>
                (dataModel.Id_TipoCR > 0 ? reg.Id_TipoCR == dataModel.Id_TipoCR : reg.Id_TipoCR != null) &&
                (dataModel.Id_TipoCompromiso.HasValue ? reg.Id_TipoCompromiso == dataModel.Id_TipoCompromiso.Value : reg.Id_TipoCR != null) &&
                (dataModel.Id_CtaBancaria.HasValue ? reg.Id_CtaBancaria == dataModel.Id_CtaBancaria : reg.Id_TipoCR != null) &&
                (dataModel.Id_Beneficiario.HasValue ? reg.Id_Beneficiario == dataModel.Id_Beneficiario.Value : reg.Id_TipoCR != null) &&
                (dataModel.Fecha_Pago_I.HasValue ? (reg.Fecha_Pago >= dataModel.Fecha_Pago_I.Value && reg.Fecha_Pago <= dataModel.Fecha_Pago_F) : reg.Id_TipoCR != null) &&
                (dataModel.Cargos_I.HasValue ? (reg.Cargos >= dataModel.Cargos_I.Value && reg.Cargos <= dataModel.Cargos_F.Value) : reg.Id_TipoCR != null) &&
                (dataModel.Id_EstatusCR.HasValue ? reg.Id_EstatusCR == dataModel.Id_EstatusCR.Value : reg.Id_TipoCR != null) &&
                (dataModel.Id_SituacionCheque.HasValue ?
                    (dataModel.Id_SituacionCheque.Value == 1 ? reg.Impreso_CH == null || reg.Impreso_CH == true || reg.Impreso_CH == false :
                     dataModel.Id_SituacionCheque.Value == 2 ? reg.Impreso_CH == false || reg.Impreso_CH == null :
                     dataModel.Id_SituacionCheque.Value == 3 ? reg.Impreso_CH == true : reg.Impreso_CH == null || reg.Impreso_CH == true || reg.Impreso_CH == false)
                     : reg.Impreso_CH == null || reg.Impreso_CH == true || reg.Impreso_CH == false) &&
                (dataModel.Id_FolioCR > 0 ? reg.Id_FolioCR == dataModel.Id_FolioCR : reg.Id_FolioCR > 0) &&
                (dataModel.No_Cheque.HasValue ? reg.No_Cheque == dataModel.No_Cheque.Value : reg.Id_TipoCR != null)
                ).ToList();

            //entities.ForEach(item => { models.Add(ModelFactory.getModel<Ma_ContrarecibosModel>(item, new Ma_ContrarecibosModel())); });
            entities.ForEach(item => { models.Add(llenar.LLenado_VistaMaContrarecibos(item.Id_TipoCR, item.Id_FolioCR)); });
            switch (dataModel.orden)
            {
                case 1:
                    models = models.OrderBy(reg => reg.Id_Beneficiario).ToList();
                    break;
                case 2:
                    models = models.OrderBy(reg => reg.Fecha_Pago).ToList();
                    break;
                case 3:
                    models = models.OrderBy(reg => reg.Id_TipoCR).ThenBy(reg => reg.Id_FolioCR).ToList();
                    break;
                case 4:
                    models = models.OrderBy(reg => reg.No_Cheque).ToList();
                    break;
                default:
                    break;

            }
            return View(models);
        }

        [HttpGet]
        public ActionResult rptContraRecibos(Modelos.ContraRecibosAnalisisModel dataModel)
        {
            /*
            List<Ma_ContrarecibosModel> models = new List<Ma_ContrarecibosModel>();
            List<Ma_Contrarecibos> entities =
            contrarecibosDal.Get(reg =>
                (dataModel.Id_TipoCR > 0 ? reg.Id_TipoCR == dataModel.Id_TipoCR : reg.Id_TipoCR != null) &&
                (dataModel.Id_TipoCompromiso.HasValue ? reg.Id_TipoCompromiso == dataModel.Id_TipoCompromiso.Value : reg.Id_TipoCR != null) &&
                (dataModel.Id_CtaBancaria.HasValue ? reg.Id_CtaBancaria == dataModel.Id_CtaBancaria : reg.Id_TipoCR != null) &&
                (dataModel.Id_Beneficiario.HasValue ? reg.Id_Beneficiario == dataModel.Id_Beneficiario.Value : reg.Id_TipoCR != null) &&
                (dataModel.Fecha_Pago_I.HasValue ? (reg.Fecha_Pago >= dataModel.Fecha_Pago_I.Value && reg.Fecha_Pago <= dataModel.Fecha_Pago_F) : reg.Id_TipoCR != null) &&
                (dataModel.Cargos_I.HasValue ? (reg.Cargos >= dataModel.Cargos_I.Value && reg.Cargos <= dataModel.Cargos_F.Value) : reg.Id_TipoCR != null) &&
                (dataModel.Id_EstatusCR.HasValue ? reg.Id_EstatusCR == dataModel.Id_EstatusCR.Value : reg.Id_TipoCR != null) &&
                (dataModel.Id_SituacionCheque.HasValue ?
                    (dataModel.Id_SituacionCheque.Value == 1 ? reg.Impreso_CH == null || reg.Impreso_CH == true || reg.Impreso_CH == false :
                     dataModel.Id_SituacionCheque.Value == 2 ? reg.Impreso_CH == false || reg.Impreso_CH == null :
                     dataModel.Id_SituacionCheque.Value == 3 ? reg.Impreso_CH == true : reg.Impreso_CH == null || reg.Impreso_CH == true || reg.Impreso_CH == false)
                     : reg.Impreso_CH == null || reg.Impreso_CH == true || reg.Impreso_CH == false) &&
                (dataModel.Id_FolioCR > 0 ? reg.Id_FolioCR == dataModel.Id_FolioCR : reg.Id_FolioCR > 0) &&
                (dataModel.No_Cheque.HasValue ? reg.No_Cheque == dataModel.No_Cheque.Value : reg.Id_TipoCR != null)
                ).ToList();

            //entities.ForEach(item => { models.Add(ModelFactory.getModel<Ma_ContrarecibosModel>(item, new Ma_ContrarecibosModel())); });
            entities.ForEach(item => { models.Add(llenar.LLenado_MaContrarecibos(item.Id_TipoCR, item.Id_FolioCR)); });
            switch (dataModel.orden)
            {
                case 1:
                    models = models.OrderBy(reg => reg.Id_Beneficiario).ToList();
                    break;
                case 2:
                    models = models.OrderBy(reg => reg.Fecha_Pago).ToList();
                    break;
                case 3:
                    models = models.OrderBy(reg => reg.Id_TipoCR).ThenBy(reg => reg.FolioCompromiso).ToList();
                    break;
                case 4:
                    models = models.OrderBy(reg => reg.No_Cheque).ToList();
                    break;
                default:
                    break;

            }
            return File(reports.GenerarPDF("rptContraRecibos", models, this.ControllerContext), "Application/PDF");
            //return View(models);*/

            List<VW_ContrarecibosModel> models = new List<VW_ContrarecibosModel>();
            List<VW_Contrarecibos> entities =
            vContrarecibosDAL.Get(reg =>
                (dataModel.Id_TipoCR > 0 ? reg.Id_TipoCR == dataModel.Id_TipoCR : reg.Id_TipoCR != null) &&
                (dataModel.Id_TipoCompromiso.HasValue ? reg.Id_TipoCompromiso == dataModel.Id_TipoCompromiso.Value : reg.Id_TipoCR != null) &&
                (dataModel.Id_CtaBancaria.HasValue ? reg.Id_CtaBancaria == dataModel.Id_CtaBancaria : reg.Id_TipoCR != null) &&
                (dataModel.Id_Beneficiario.HasValue ? reg.Id_Beneficiario == dataModel.Id_Beneficiario.Value : reg.Id_TipoCR != null) &&
                (dataModel.Fecha_Pago_I.HasValue ? (reg.Fecha_Pago >= dataModel.Fecha_Pago_I.Value && reg.Fecha_Pago <= dataModel.Fecha_Pago_F) : reg.Id_TipoCR != null) &&
                (dataModel.Cargos_I.HasValue ? (reg.Cargos >= dataModel.Cargos_I.Value && reg.Cargos <= dataModel.Cargos_F.Value) : reg.Id_TipoCR != null) &&
                (dataModel.Id_EstatusCR.HasValue ? reg.Id_EstatusCR == dataModel.Id_EstatusCR.Value : reg.Id_TipoCR != null) &&
                (dataModel.Id_SituacionCheque.HasValue ?
                    (dataModel.Id_SituacionCheque.Value == 1 ? reg.Impreso_CH == null || reg.Impreso_CH == true || reg.Impreso_CH == false :
                     dataModel.Id_SituacionCheque.Value == 2 ? reg.Impreso_CH == false || reg.Impreso_CH == null :
                     dataModel.Id_SituacionCheque.Value == 3 ? reg.Impreso_CH == true : reg.Impreso_CH == null || reg.Impreso_CH == true || reg.Impreso_CH == false)
                     : reg.Impreso_CH == null || reg.Impreso_CH == true || reg.Impreso_CH == false) &&
                (dataModel.Id_FolioCR > 0 ? reg.Id_FolioCR == dataModel.Id_FolioCR : reg.Id_FolioCR > 0) &&
                (dataModel.No_Cheque.HasValue ? reg.No_Cheque == dataModel.No_Cheque.Value : reg.Id_TipoCR != null)
                ).ToList();

            //entities.ForEach(item => { models.Add(ModelFactory.getModel<Ma_ContrarecibosModel>(item, new Ma_ContrarecibosModel())); });
            entities.ForEach(item => { models.Add(llenar.LLenado_VistaMaContrarecibos(item.Id_TipoCR, item.Id_FolioCR)); });
            switch (dataModel.orden)
            {
                case 1:
                    models = models.OrderBy(reg => reg.Id_Beneficiario).ToList();
                    break;
                case 2:
                    models = models.OrderBy(reg => reg.Fecha_Pago).ToList();
                    break;
                case 3:
                    models = models.OrderBy(reg => reg.Id_TipoCR).ThenBy(reg => reg.Id_FolioCR).ToList();
                    break;
                case 4:
                    models = models.OrderBy(reg => reg.No_Cheque).ToList();
                    break;
                default:
                    break;

            }
            return File(reports.GenerarPDF("rptContraRecibos", models, this.ControllerContext), "Application/PDF");
            //return View(models);
        }
        #endregion

        #region Analisis de Compromisos
        public ActionResult AnalisisCompromisos()
        {
            ViewBag.Ejercicio = new ParametrosDAL().GetByID(x => x.Descripcion == "Ejercicio contable").Valor;
            return View(new CompromisosBusqueda());
        }
        [HttpPost]
        public ActionResult BuscarCompromisos(CompromisosBusqueda Busqueda)
        {
            return View(new CompromisosBL().BuscarCompromisos(Busqueda));
        }
        public ActionResult ReporteCompromisos(CompromisosBusqueda Busqueda)
        {
            List<Ma_CompromisosModel> Compromisos = new CompromisosBL().BuscarCompromisos(Busqueda);
            return File(reports.GenerarPDF("ReporteCompromisos", Compromisos, this.ControllerContext), "Application/PDF");
        }
        #endregion

        #region ExportarCheques
        [HttpGet]
        public ActionResult ExportarCheques()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExportarCheques(String ListaCheques, int ClaveCuenta)
        {
            List<Cheques> Lista = new JavaScriptSerializer().Deserialize<List<Cheques>>(ListaCheques);
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            sheet.SetColumnWidth(0, 20 * 256);
            sheet.SetColumnWidth(1, 20 * 256);
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 20 * 256);
            sheet.SetColumnWidth(4, 20 * 256);
            sheet.SetColumnWidth(5, 20 * 256);
            sheet.SetColumnWidth(6, 20 * 256);
            var headerRow = sheet.CreateRow(0);
            IFont f1 = workbook.CreateFont();
            f1.Color = HSSFColor.Black.Index;
            f1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            headerRow.CreateCell(0).SetCellValue("Tipo");
            headerRow.GetCell(0).RichStringCellValue.ApplyFont(0, headerRow.GetCell(0).StringCellValue.Length, f1);
            headerRow.CreateCell(1).SetCellValue("Folio");
            headerRow.GetCell(1).RichStringCellValue.ApplyFont(0, headerRow.GetCell(1).StringCellValue.Length, f1);
            headerRow.CreateCell(2).SetCellValue("Fecha de Pago");
            headerRow.GetCell(2).RichStringCellValue.ApplyFont(0, headerRow.GetCell(2).StringCellValue.Length, f1);
            headerRow.CreateCell(3).SetCellValue("Beneficiario");
            headerRow.GetCell(3).RichStringCellValue.ApplyFont(0, headerRow.GetCell(3).StringCellValue.Length, f1);
            headerRow.CreateCell(4).SetCellValue("Descripción");
            headerRow.GetCell(4).RichStringCellValue.ApplyFont(0, headerRow.GetCell(4).StringCellValue.Length, f1);
            headerRow.CreateCell(5).SetCellValue("No. de Cheque");
            headerRow.GetCell(5).RichStringCellValue.ApplyFont(0, headerRow.GetCell(5).StringCellValue.Length, f1);
            headerRow.CreateCell(6).SetCellValue("Importe");
            headerRow.GetCell(6).RichStringCellValue.ApplyFont(0, headerRow.GetCell(6).StringCellValue.Length, f1);
            sheet.CreateFreezePane(0, 1, 0, 1);
            int rowNumber = 1;
            foreach (Cheques item in Lista)
            {
                Ma_ContrarecibosModel contrarecibo = new Llenado().LLenado_MaContrarecibos(Convert.ToByte(item.Id_TipoCR), item.Id_FolioCR);
                var row = sheet.CreateRow(rowNumber++);
                row.CreateCell(0).SetCellValue(contrarecibo.Id_TipoCR);
                row.CreateCell(1).SetCellValue(contrarecibo.Id_FolioCR);
                row.CreateCell(2).SetCellValue(contrarecibo.Fecha_Pago.HasValue ? contrarecibo.Fecha_Pago.Value.ToShortDateString() : "");
                row.CreateCell(3).SetCellValue(contrarecibo.Ca_Beneficiarios.NombreCompleto);
                row.CreateCell(4).SetCellValue(contrarecibo.Descripcion);
                row.CreateCell(5).SetCellValue(contrarecibo.No_Cheque.ToString());
                row.CreateCell(6).SetCellValue(contrarecibo.Cargos.ToString());
            }
            DateTime Fecha = DateTime.Now;
            String NombreArchivo = cuentasBancariasDAL.GetByID(x => x.Id_CtaBancaria == ClaveCuenta).Descripcion + "-" + Fecha.ToShortDateString() + "_" + Fecha.ToString("HH") + "_" + Fecha.ToString("mm");
            MemoryStream output = new MemoryStream();
            workbook.Write(output);
            return File(output.ToArray(),
             "application/vnd.ms-excel",
             NombreArchivo + ".xls");
        }

        public ActionResult GetChequesCtaBancaria(short? CuentaBancaria)
        {
            try
            {
                List<Ma_Contrarecibos> Lista = contrarecibosDal.Get(x => (CuentaBancaria.HasValue ? x.Id_CtaBancaria == CuentaBancaria.Value : x.Id_CtaBancaria != null) && x.No_Cheque != null && x.No_Cheque > 0 && x.Id_FolioPO_Pagado != null && x.Id_EstatusCR == 2 && x.Impreso_CH == true).ToList();
                List<Ma_ContrarecibosModel> ListaCheques = new List<Ma_ContrarecibosModel>();
                Lista.ForEach(x => ListaCheques.Add(ModelFactory.getModel<Ma_ContrarecibosModel>(new Llenado().LLenado_MaContrarecibos(x.Id_TipoCR, x.Id_FolioCR), new Ma_ContrarecibosModel())));
                ListaCheques.ForEach(x => x.NombreCompleto = x.Ca_Beneficiarios.NombreCompleto);
                return Json(new { Cheques = ListaCheques });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        public ActionResult ContrarecibosSinCuentaBancaria()
        {
            return View(new ContrarecibosSinFuenteDAL().Get().ToList());
        }

        public ActionResult ReporteContrarecibosSinCuenta()
        {
            List<VW_contrarecibosSinFuente> lista = new ContrarecibosSinFuenteDAL().Get().ToList();
            ConvertHtmlToString reports = new ConvertHtmlToString();
            return File(reports.GenerarPDF_Horizontal("ReporteContrarecibosSinCuenta", lista, this.ControllerContext), "Application/PDF");
        }
        #endregion

        #region ReporteProveedores
        public ActionResult AnalisisProveedoresUsados()
        {
            List<VW_ProvedoresUsadosModel> models = new List<VW_ProvedoresUsadosModel>();
            Llenado llenar = new Llenado();
            vProveedoresUsadosDAL.Get().ToList().ForEach(x => {
                models.Add(llenar.LLenado_vProveedoresUsados(x));
            });
            return View(models);
        }
        public ActionResult AnalisisProveedoresUsadosPDF()
        {
            List<VW_ProvedoresUsadosModel> models = new List<VW_ProvedoresUsadosModel>();
            Llenado llenar = new Llenado();
            vProveedoresUsadosDAL.Get().ToList().ForEach(x =>
            {
                models.Add(llenar.LLenado_vProveedoresUsados(x));
            });
            ViewBag.Title = "PROVEEDORES";
            return File(reports.GenerarPDF("AnalisisProveedoresUsadosPDF", models, this.ControllerContext), "Application/PDF");
        }
        public ActionResult SeleccionTipoExcel()
        {
            return View();
        }
        public ActionResult AnalisisProveedoresUsadosExcel()
        {
            List<VW_ProvedoresUsadosModel> models = new List<VW_ProvedoresUsadosModel>();
            Llenado llenar = new Llenado();
            vProveedoresUsadosDAL.Get().OrderBy(x => x.Nombre).ToList().ForEach(x =>
            {
                models.Add(llenar.LLenado_vProveedoresUsados(x));
            });
            //EXCEL DETALLADO
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            int contador = 0;
            foreach (string item in ReporteProveedores.ListaCeldas())
            {
                sheet.SetColumnWidth(contador, Convert.ToInt32(item.Split('-')[1])* 256);
                contador++;
            }
            var headerRow = sheet.CreateRow(0);
            IFont f1 = workbook.CreateFont();
            f1.Color = HSSFColor.White.Index;
            f1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            ICellStyle estiloHead = workbook.CreateCellStyle();
            estiloHead.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Green.Index;
            estiloHead.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Green.Index;
            estiloHead.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            contador = 0;
            foreach (string item in ReporteProveedores.ListaCeldas())
            {
                headerRow.CreateCell(contador).SetCellValue(item.Split('-')[0]);
                //headerRow.GetCell(contador).RichStringCellValue.ApplyFont(0, headerRow.GetCell(contador).StringCellValue.Length, f1);
                estiloHead.SetFont(f1);
                headerRow.GetCell(contador).CellStyle = estiloHead;
                contador++;
            }
            sheet.CreateFreezePane(0, 1, 0, 1);
            int rowNumber = 1;
            foreach (VW_ProvedoresUsadosModel item in models)
            {
                var row = sheet.CreateRow(rowNumber++);
                ICellStyle estiloRow = workbook.CreateCellStyle();
                ICellStyle estiloDerecha = workbook.CreateCellStyle();
                if (rowNumber % 2 == 0)
                {
                    estiloRow.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
                    estiloRow.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    estiloDerecha.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
                    estiloDerecha.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    estiloDerecha.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
                }
                else
                {
                    estiloRow.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
                    estiloRow.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    estiloDerecha.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
                    estiloDerecha.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    estiloDerecha.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
                }
                row.CreateCell(0).SetCellValue(item.TipoCR);
                row.GetCell(0).CellStyle = estiloRow;
                row.CreateCell(1).SetCellValue(item.Id_FolioCR);
                row.GetCell(1).CellStyle = estiloRow;
                row.CreateCell(2).SetCellValue((item.Fecha.HasValue ? item.Fecha.Value.ToShortDateString() : " "));
                row.GetCell(2).CellStyle = estiloRow;
                row.CreateCell(3).SetCellValue(item.RFC);
                row.GetCell(3).CellStyle = estiloRow;
                row.CreateCell(4).SetCellValue(item.Nombre);
                row.GetCell(4).CellStyle = estiloRow;
                row.CreateCell(5).SetCellValue(item.CURP);
                row.GetCell(5).CellStyle = estiloRow;
                row.CreateCell(6).SetCellValue(item.Estado);
                row.GetCell(6).CellStyle = estiloRow;
                row.CreateCell(7).SetCellValue(item.Municipio);
                row.GetCell(7).CellStyle = estiloRow;
                row.CreateCell(8).SetCellValue(item.Localidad);
                row.GetCell(8).CellStyle = estiloRow;
                row.CreateCell(9).SetCellValue(item.CP);
                row.GetCell(9).CellStyle = estiloRow;
                row.CreateCell(10).SetCellValue(item.Colonia);
                row.GetCell(10).CellStyle = estiloRow;
                row.CreateCell(11).SetCellValue(item.Calle);
                row.GetCell(11).CellStyle = estiloRow;
                row.CreateCell(12).SetCellValue(item.TipoDocto);
                row.GetCell(12).CellStyle = estiloRow;
                row.CreateCell(13).SetCellValue(item.No_docto);
                row.GetCell(13).CellStyle = estiloRow;
                row.CreateCell(14).SetCellValue((item.Fecha.HasValue ? item.Fecha.Value.ToShortDateString() : " "));
                row.GetCell(14).CellStyle = estiloRow;
                row.CreateCell(15).SetCellValue((item.SubTotal.HasValue? item.SubTotal.Value.ToString("N"):"0.00"));
                row.GetCell(15).CellStyle = estiloDerecha;
                row.CreateCell(16).SetCellValue((item.Ret_ISR.HasValue? item.Ret_ISR.Value.ToString("N"):"0.00"));
                row.GetCell(16).CellStyle = estiloDerecha;
                row.CreateCell(17).SetCellValue((item.Ret_IVA.HasValue? item.Ret_IVA.Value.ToString("N"):"0.00"));
                row.GetCell(17).CellStyle = estiloDerecha;
                row.CreateCell(18).SetCellValue((item.IVA.HasValue ? item.IVA.Value.ToString("N") : "0.00"));
                row.GetCell(18).CellStyle = estiloDerecha;
                row.CreateCell(19).SetCellValue((item.Ret_Obra.HasValue? item.Ret_Obra.Value.ToString("N"):"0.00"));
                row.GetCell(19).CellStyle = estiloDerecha;
                row.CreateCell(20).SetCellValue(item.DescripcionDeduccion);//Deduccion
                row.GetCell(20).CellStyle = estiloDerecha;
                row.CreateCell(21).SetCellValue((item.Ret_Otras.HasValue ? item.Ret_Otras.Value.ToString("N") : "0.00"));//Importe Deduccion
                row.GetCell(21).CellStyle = estiloDerecha;
                row.CreateCell(22).SetCellValue(item.DescripcionImpuesto);//Impuestos
                row.GetCell(22).CellStyle = estiloDerecha;
                row.CreateCell(23).SetCellValue((item.Otros.HasValue ? item.Otros.Value.ToString("N") : "0.00"));//Importe Impuestos
                row.GetCell(23).CellStyle = estiloDerecha;
                row.CreateCell(24).SetCellValue((item.TOTAL.HasValue? item.TOTAL.Value.ToString("N"):"0.00"));
                row.GetCell(24).CellStyle = estiloDerecha;
                //row.CreateCell(24).SetCellValue("Proyecto");//proyecto
                //row.GetCell(24).CellStyle = estiloRow;
                //row.CreateCell(25).SetCellValue("Observaciones");//Observaciones
                //row.GetCell(25).CellStyle = estiloRow;
                row.CreateCell(25).SetCellValue(item.TipoBeneficiario);//Observaciones
                row.GetCell(25).CellStyle = estiloRow;
                row.CreateCell(26).SetCellValue(item.ClasificaBeneficiario);//Observaciones
                row.GetCell(26).CellStyle = estiloRow;
            }
            DateTime Fecha = DateTime.Now;
            String NombreArchivo =  "ReporteProveedores-" + Fecha.ToShortDateString() + "_" + Fecha.ToString("HH") + "_" + Fecha.ToString("mm");
            MemoryStream output = new MemoryStream();
            workbook.Write(output);
            return File(output.ToArray(),"application/vnd.ms-excel",NombreArchivo + ".xls");
        }
        public ActionResult AnalisisProveedoresAgrupadosExcel()
        {
            List<VW_ProvedoresUsadosModel> models = new List<VW_ProvedoresUsadosModel>();
            Llenado llenar = new Llenado();
            vProveedoresUsadosAgrupadosDAL.Get().OrderBy(x => x.Nombre).ToList().ForEach(x =>
            {
                models.Add(llenar.LLenado_vProveedoresUsados(x));
            });
            //EXCEL AGRUPADO
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            int contador = 0;
            foreach (string item in ReporteProveedores.ListaCeldasAgrupado())
            {
                sheet.SetColumnWidth(contador, Convert.ToInt32(item.Split('-')[1]) * 256);
                contador++;
            }
            var headerRow = sheet.CreateRow(0);
            IFont f1 = workbook.CreateFont();
            f1.Color = HSSFColor.White.Index;
            f1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            ICellStyle estiloHead = workbook.CreateCellStyle();
            estiloHead.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Green.Index;
            estiloHead.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Green.Index;
            estiloHead.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;  
            contador = 0;
            foreach (string item in ReporteProveedores.ListaCeldasAgrupado())
            {
                headerRow.CreateCell(contador).SetCellValue(item.Split('-')[0]);
                //headerRow.GetCell(contador).RichStringCellValue.ApplyFont(0, headerRow.GetCell(contador).StringCellValue.Length, f1);
                estiloHead.SetFont(f1);
                headerRow.GetCell(contador).CellStyle = estiloHead;
                contador++;
            }
            sheet.CreateFreezePane(0, 1, 0, 1);
            int rowNumber = 1;
            foreach (VW_ProvedoresUsadosModel item in models)
            {
                var row = sheet.CreateRow(rowNumber++);
                ICellStyle estiloRow = workbook.CreateCellStyle();
                ICellStyle estiloDerecha = workbook.CreateCellStyle();
                if (rowNumber % 2 == 0)
                {
                    estiloRow.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
                    estiloRow.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    estiloDerecha.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
                    estiloDerecha.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    estiloDerecha.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
                }
                else
                {
                    estiloRow.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
                    estiloRow.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    estiloDerecha.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
                    estiloDerecha.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                    estiloDerecha.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
                }
                row.CreateCell(0).SetCellValue(item.Nombre);
                row.GetCell(0).CellStyle = estiloRow;
                row.CreateCell(1).SetCellValue((item.SubTotal.HasValue ? item.SubTotal.Value.ToString("N") : "0.00"));
                row.GetCell(1).CellStyle = estiloDerecha;
                row.CreateCell(2).SetCellValue((item.IVA.HasValue ? item.IVA.Value.ToString("N") : "0.00"));
                row.GetCell(2).CellStyle = estiloDerecha;
                row.CreateCell(3).SetCellValue((item.Ret_ISR.HasValue ? item.Ret_ISR.Value.ToString("N") : "0.00"));
                row.GetCell(3).CellStyle = estiloDerecha;
                row.CreateCell(4).SetCellValue((item.Ret_IVA.HasValue ? item.Ret_IVA.Value.ToString("N") : "0.00"));
                row.GetCell(4).CellStyle = estiloDerecha;
                row.CreateCell(5).SetCellValue((item.Ret_Obra.HasValue ? item.Ret_Obra.Value.ToString("N") : "0.00"));
                row.GetCell(5).CellStyle = estiloDerecha;
                row.CreateCell(6).SetCellValue((item.Ret_Otras.HasValue ? item.Ret_Otras.Value.ToString("N") : "0.00"));
                row.GetCell(6).CellStyle = estiloDerecha;
                row.CreateCell(7).SetCellValue((item.Otros.HasValue ? item.Otros.Value.ToString("N") : "0.00"));
                row.GetCell(7).CellStyle = estiloDerecha;
                row.CreateCell(8).SetCellValue((item.TOTAL.HasValue ? item.TOTAL.Value.ToString("N") : "0.00"));
                row.GetCell(8).CellStyle = estiloDerecha;
                row.CreateCell(9).SetCellValue(item.RFC);
                row.GetCell(9).CellStyle = estiloRow;
                row.CreateCell(10).SetCellValue(item.CURP);
                row.GetCell(10).CellStyle = estiloRow;
                row.CreateCell(11).SetCellValue(item.Estado);
                row.GetCell(11).CellStyle = estiloRow;
                row.CreateCell(12).SetCellValue(item.Municipio);
                row.GetCell(12).CellStyle = estiloRow;
                row.CreateCell(13).SetCellValue(item.Localidad);
                row.GetCell(13).CellStyle = estiloRow;
                row.CreateCell(14).SetCellValue(item.CP);
                row.GetCell(14).CellStyle = estiloRow;
                row.CreateCell(15).SetCellValue(item.Colonia);
                row.GetCell(15).CellStyle = estiloRow;
                row.CreateCell(16).SetCellValue(item.Calle);
                row.GetCell(16).CellStyle = estiloRow;
                //row.CreateCell(17).SetCellValue("Tipo de información");
                //row.GetCell(17).CellStyle = estiloRow;
                row.CreateCell(17).SetCellValue(item.TipoBeneficiario);
                row.GetCell(17).CellStyle = estiloRow;
                row.CreateCell(18).SetCellValue(item.ClasificaBeneficiario);
                row.GetCell(18).CellStyle = estiloRow;
            }
            DateTime Fecha = DateTime.Now;
            String NombreArchivo = "ReporteProveedoresAgrupados-" + Fecha.ToShortDateString() + "_" + Fecha.ToString("HH") + "_" + Fecha.ToString("mm");
            MemoryStream output = new MemoryStream();
            workbook.Write(output);
            return File(output.ToArray(), "application/vnd.ms-excel", NombreArchivo + ".xls");
        }
        #endregion
    }
}
