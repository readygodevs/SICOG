using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.BL;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.DAL.Vistas;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Filters;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    [AuthorizeLogin]
    public class ContabilidadController : Controller
    {
        //
        // GET: /Tesoreria/Contabilidad/
        protected DePolizasDAL DALDePolizas { get; set; }
        protected CuentasDAL DALCuentas { get; set; }
        protected MaPolizasDAL DALMaPolizas { get; set; }
        protected ParametrosDAL DALParametros { get; set; }
        protected CierreMensualDAL DALCierreMensual { get; set; }
        private PolizasBL polizasBL { get; set; }
        protected ConvertHtmlToString reports { get; set; }

        public ContabilidadController()
        {
            if (DALDePolizas == null) DALDePolizas = new DePolizasDAL();
            if (DALCuentas == null) DALCuentas = new CuentasDAL();
            if (DALMaPolizas == null) DALMaPolizas = new MaPolizasDAL();
            if (DALParametros == null) DALParametros = new ParametrosDAL();
            if (DALCierreMensual == null) DALCierreMensual = new CierreMensualDAL();
            if (polizasBL == null) polizasBL = new PolizasBL();
            if (reports == null) reports = new ConvertHtmlToString();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult V_SaldosIniciales()
        {

            Ma_Polizas poliza = DALMaPolizas.GetByID(x => x.Id_TipoPoliza == 3 && x.Id_FolioPoliza == 0 && x.Id_MesPoliza == 13);
            Ca_CierreMensualModel cierre = new Ca_CierreMensualModel();
            byte total =0;
            if (DALCierreMensual.Get(x => x.Contable == true).Count() > 0)
                total = DALCierreMensual.Get(x => x.Contable == true).Last().Id_Mes;
            ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, total + 1, 1).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            if (poliza != null)
            {
                DateTime FechaActual = DateTime.Now;
                CA_Parametros parametro = DALParametros.GetByID(x => x.Nombre == "Fecha_SaldosIniciales");
                CA_Parametros dias = DALParametros.GetByID(x => x.Nombre == "Dias_SaldosIniciales");
                TimeSpan diferencia = FechaActual - Convert.ToDateTime(parametro.Valor);
                if (diferencia.Days <= Convert.ToInt32(dias.Valor))
                {
                    IEnumerable<De_Polizas> ListaPolizas = DALDePolizas.Get(x => x.Id_TipoPoliza == 3 && x.Id_FolioPoliza == 0 && x.Id_MesPoliza == 13);
                    if (ListaPolizas != null)
                    {
                        List<SaldosIniciales> ListaSaldos = new List<SaldosIniciales>();
                        foreach (De_Polizas item in ListaPolizas)
                        {
                            SaldosIniciales temp = new SaldosIniciales();
                            CA_Cuentas TempC = DALCuentas.GetByID(x => x.Id_Cuenta == item.Id_Cuenta);
                            temp.Id_CuentaFormato = TempC.Id_CuentaFormato;
                            temp.Id_Cuenta = TempC.Id_Cuenta;
                            temp.Id_TipoMovimiento = item.Id_Movimiento.Value;
                            temp.DescCuenta = TempC.Descripcion.Length > 50 ? TempC.Descripcion.Substring(0, 50) : TempC.Descripcion;
                            temp.Importe = item.Importe.Value;
                            temp.Fecha = item.Fecha.Value;
                            ListaSaldos.Add(temp);
                        }
                        HttpContext.Session["ListaSaldos"] = ListaSaldos;
                    }
                    else
                    {
                        HttpContext.Session["ListaSaldos"] = null;
                    }
                    GuardarMAPoliza MApoliza = new GuardarMAPoliza();
                    MApoliza.Descripcion = poliza.Descripcion;
                    MApoliza.Fecha = poliza.Fecha;
                    return View(MApoliza);
                }
                else
                    return View("Error");
            }
            else
            {
                return View(new GuardarMAPoliza());
            }


        }
        public ActionResult DivAgregarSaldo()
        {
            return View();
        }
        public ActionResult V_BuscarCuenta()
        {
            return View();
        }
        public ActionResult TablaSaldos()
        {
            if (HttpContext.Session["ListaSaldos"] == null)
            {
                HttpContext.Session["ListaSaldos"] = new List<SaldosIniciales>();
                return View(new List<SaldosIniciales>());
            }
            else
            {
                List<SaldosIniciales> lst = HttpContext.Session["ListaSaldos"] as List<SaldosIniciales>;
                return View(lst);
            }
        }
        [HttpPost]
        public JsonResult GuardarSaldoSession(SaldosIniciales model)
        {
            try
            {
                decimal cargo=0,abono=0;
                De_Polizas de_poliza = DALDePolizas.GetByID(x => x.Id_TipoPoliza == 3 && x.Id_FolioPoliza == 0 && x.Id_MesPoliza == 13 && x.Id_Movimiento == model.Id_TipoMovimiento && x.Id_Cuenta == model.Id_Cuenta);
                if (de_poliza != null)
                {
                    de_poliza.Importe += model.Importe;
                    DALDePolizas.Update(de_poliza);
                    DALDePolizas.Save();
                    CA_Cuentas cuenta = DALCuentas.GetByID(x => x.Id_Cuenta == model.Id_Cuenta);
                    if (model.Id_TipoMovimiento == 1)
                    {
                        cuenta.Cargo_Inicial = de_poliza.Importe;
                        cargo=model.Importe;
                    }
                    else
                    {
                        cuenta.Abono_Inicial = de_poliza.Importe;
                        abono=model.Importe;
                    }
                    DALCuentas.Update(cuenta);
                    DALCuentas.Save();
                }
                else
                {
                    Ma_Polizas MApoliza = DALMaPolizas.GetByID(x => x.Id_TipoPoliza == 3 && x.Id_FolioPoliza == 0 && x.Id_MesPoliza == 13);
                    De_PolizasModel de_polizasmodel = new De_PolizasModel();
                    De_Polizas poliza = new De_Polizas();
                    poliza.Id_TipoPoliza = 3;
                    poliza.Id_FolioPoliza = 0;
                    poliza.Id_MesPoliza = 13;
                    poliza.Fecha = MApoliza.Fecha;
                    poliza.Id_Cuenta = model.Id_Cuenta;
                    poliza.Importe = model.Importe;
                    poliza.Id_Movimiento = Convert.ToByte(model.Id_TipoMovimiento);
                    poliza.Estatus = 1;
                    poliza.Id_Registro = de_polizasmodel.ObtenerFolio(3, 0, 13);
                    DALDePolizas.Insert(poliza);
                    DALDePolizas.Save();
                    CA_Cuentas cuenta = DALCuentas.GetByID(x => x.Id_Cuenta == model.Id_Cuenta);
                    if (model.Id_TipoMovimiento == 1)
                    {
                        cuenta.Cargo_Inicial = model.Importe;
                        cargo=model.Importe;
                    }
                    else
                    {
                        cuenta.Abono_Inicial = model.Importe;
                        abono=model.Importe;
                    }
                    DALCuentas.Update(cuenta);
                    DALCuentas.Save();
                }
                CA_Cuentas TempC = DALCuentas.GetByID(x => x.Id_Cuenta == model.Id_Cuenta);
                model.Id_CuentaFormato = TempC.Id_CuentaFormato;
                model.Id_Cuenta = TempC.Id_Cuenta;
                model.DescCuenta = TempC.Descripcion.Length > 50 ? TempC.Descripcion.Substring(0, 50) : TempC.Descripcion;
                List<SaldosIniciales> lst = HttpContext.Session["ListaSaldos"] as List<SaldosIniciales>;
                lst.Add(model);
                HttpContext.Session["ListaOpciones"] = lst;
                return Json(new { Exito = true,Registro=model,cargo= String.Format("{0:N}",cargo), abono=String.Format("{0:N}", abono) }, "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }
        [HttpPost]
        public JsonResult GuardarMa(GuardarMAPoliza Model)
        {
            try
            {
                Ma_Polizas MApolizaaux = new Ma_Polizas();
                MApolizaaux.Descripcion = Model.Descripcion;
                MApolizaaux.Fecha = Model.Fecha;
                MApolizaaux.Id_TipoPoliza = 3;
                MApolizaaux.Id_FolioPoliza = 0;
                MApolizaaux.Id_MesPoliza = 13;
                DALMaPolizas.Insert(MApolizaaux);
                DALMaPolizas.Save();
                CA_Parametros parametro = DALParametros.GetByID(x => x.Nombre == "Fecha_SaldosIniciales");
                parametro.Valor = DateTime.Now.ToShortDateString().ToString();
                DALParametros.Update(parametro);
                DALParametros.Save();
                return Json(new { Exito = true, Mensaje = "Registro guardado correctamente." }, "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }
        [HttpPost]
        public JsonResult EliminarSaldoSession(SaldosIniciales model)
        {
            try
            {
                De_Polizas de_poliza = DALDePolizas.GetByID(x => x.Id_TipoPoliza == 3 && x.Id_FolioPoliza == 0 && x.Id_MesPoliza == 13 && x.Id_Movimiento == model.Id_TipoMovimiento && x.Id_Cuenta == model.Id_Cuenta);
                de_poliza.Importe -= model.Importe;
                DALDePolizas.Update(de_poliza);
                DALDePolizas.Save();
                CA_Cuentas cuenta = DALCuentas.GetByID(x => x.Id_Cuenta == model.Id_Cuenta);
                if (model.Id_TipoMovimiento == 1)
                    cuenta.Cargo_Inicial = model.Importe;
                else
                    cuenta.Abono_Inicial = model.Importe;
                DALCuentas.Update(cuenta);
                DALCuentas.Save();
                if (de_poliza.Importe == 0)
                {
                    DALDePolizas.Delete(x => x.Id_TipoPoliza == 3 && x.Id_FolioPoliza == 0 && x.Id_MesPoliza == 13 && x.Id_Movimiento == model.Id_TipoMovimiento && x.Id_Cuenta == model.Id_Cuenta);
                    DALDePolizas.Save();
                }
                return Json(new { Exito = true, Mensaje = "Registro guardado correctamente." }, "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }

        #region CierreMes
        public ActionResult CierreMes()
        {
            int m = new Ca_CierreMensualModel().ObtenerMes();
            String Mes = Diccionarios.Meses[m == 13 ? m-1 : m == 0 ? m+1:m];
            ViewBag.Mes = Mes;
            if (m == 13)
                ViewBag.Cerrados = true;
            else
                ViewBag.Cerrados = false;
            return View(new VerificaPolizaDAL().Get(x => x.Id_MesPoliza == m));
        }

        public ActionResult VerificacionPolizas()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetPolizasMes(int Id_Mes)
        {
            return View(new VerificaPolizaDAL().Get(x => x.Id_MesPoliza == Id_Mes));
        }
        [HttpPost]
        public JsonResult CerrarMes()
        {
            try
            {
                int m = new Ca_CierreMensualModel().ObtenerMes();
                if (m <13)
                {
                    CierreMensualDAL cierreDal = new CierreMensualDAL();
                    Ca_CierreMensual cierre = cierreDal.GetByID(x => x.Id_Mes == (m == 0? m+1:m));
                    cierre.Contable = true;
                    cierreDal.Update(cierre);
                    cierreDal.Save();
                    return Json(new { Exito = true, Mensaje = "Mes cerrado correctamente" }, "application/json", Encoding.UTF8);
                }
                else
                    return Json(new { Exito = false, Mensaje = "Todos los meses se encuentran cerrados" }, "application/json", Encoding.UTF8);
                
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }

        #endregion
        
        #region AnaliticoPolizas
        public ActionResult V_AnaliticoPolizas()
        {
            ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, 1, 1).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            return View(new AnaliticoPolizas());
        }
        [HttpPost]
        public ActionResult V_AnaliticoPolizasTbl(AnaliticoPolizas busqueda)
        {
            return View(polizasBL.BuscarCompromisos(busqueda));
        }
        public ActionResult ReportePolizas(AnaliticoPolizas busqueda)
        {
            List<Ma_PolizasModel> Polizas = polizasBL.BuscarCompromisos(busqueda);
            return File(reports.GenerarPDF("ReportePolizas", Polizas, this.ControllerContext), "Application/PDF");
        }
        #endregion
    }
}
