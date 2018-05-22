using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    public class CuentasController : Controller
    {
        //
        // GET: /Tesoreria/Cuentas/
        private CuentasDAL _dalCuentas;
        private ParametrosDAL _dalParametros;
        private Cuentas _cuentas;

        public Cuentas cuentas
        {
            get { return _cuentas; }
            set { _cuentas = value; }
        }

        public ParametrosDAL dalParametros
        {
            get { return _dalParametros; }
            set { _dalParametros = value; }
        }

        protected CuentasDAL dalCuentas
        {
            get { return _dalCuentas; }
            set { _dalCuentas = value; }
        }

        public CuentasController()
        {
            if (cuentas == null) cuentas = new Cuentas();
        }

        public ActionResult BuscarCuenta()
        {
            return View(new FiltrosCuentas());
        }

        [HttpPost]
        public ActionResult tblBuscarCuenta(FiltrosCuentas filtros)
        {
            return View(cuentas.listaCuentas(filtros));
        }

        [HttpPost]
        public ActionResult getCuenta(String IdCuenta)
        {
            return Json( dalCuentas.GetByID(x=> x.Id_Cuenta == IdCuenta) );
        }

        public ActionResult partialClavePresupuestalCRI()
        {
            return View("_ClavePresupuestalCRI", new De_ClavePresupuestal());
        }

        public ActionResult partialClavePresupuestal()
        {
            return View("_ClavePresupuestal", new De_ClavePresupuestal());
        }


    }
}
