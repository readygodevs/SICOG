using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    public class ReportesController : Controller
    {
        protected DePolizasDAL depolizas { get; set; }
        protected MaPolizasDAL mapolizas { get; set; }
        protected BeneficiariosDAL beneficiarios { get; set; }
        protected CuentasBancariasDAL cuentasbancarias { get; set; }
        protected BancosDAL bancos { get; set; }
        
        public ReportesController()
        {
            if (mapolizas == null) mapolizas = new MaPolizasDAL();
            if (depolizas == null) depolizas = new DePolizasDAL(); 
            if (beneficiarios == null) beneficiarios = new BeneficiariosDAL();
            if (cuentasbancarias == null) cuentasbancarias = new CuentasBancariasDAL();
            if (bancos == null) bancos = new BancosDAL();
        }

        public ActionResult Polizas(byte TipoPoliza, int FolioPoliza, byte MesPoliza)
        {

            Ma_PolizasModel model = ModelFactory.getModel<Ma_PolizasModel>(mapolizas.GetByID(x => x.Id_TipoPoliza == TipoPoliza && x.Id_FolioPoliza == FolioPoliza && x.Id_MesPoliza == MesPoliza), new Ma_PolizasModel());
            IEnumerable<De_Polizas> lstmodel = depolizas.Get(x => x.Id_TipoPoliza == TipoPoliza && x.Id_FolioPoliza == FolioPoliza && x.Id_MesPoliza == MesPoliza);
            foreach (De_Polizas item in lstmodel)
            {
                De_PolizasModel depol = ModelFactory.getModel<De_PolizasModel>(depolizas.GetByID(x => x.Id_TipoPoliza == item.Id_TipoPoliza && x.Id_FolioPoliza == item.Id_FolioPoliza && x.Id_MesPoliza == item.Id_MesPoliza), new De_PolizasModel());
                model.De_Polizas.Add(depol);
            }
            if(model.Id_Beneficiario != null)
                model.Ca_Beneficiarios = ModelFactory.getModel<Ca_BeneficiariosModel>( beneficiarios.GetByID(x=> x.Id_Beneficiario == model.Id_Beneficiario), new Ca_BeneficiariosModel());
            if (model.Id_CtaBancaria != null)
            {
                model.Ca_CuentasBancarias = ModelFactory.getModel<Ca_CuentasBancariasModel>(cuentasbancarias.GetByID(x => x.Id_CtaBancaria == model.Id_CtaBancaria), new Ca_CuentasBancariasModel());
                if(model.Ca_CuentasBancarias.Id_Banco != null)
                    model.Ca_CuentasBancarias.Ca_Bancos = ModelFactory.getModel<Ca_BancosModel>(bancos.GetByID(x => x.Id_Banco == model.Ca_CuentasBancarias.Id_Banco), new Ca_BancosModel());
            }
            return View(model);
        }

    }
}
