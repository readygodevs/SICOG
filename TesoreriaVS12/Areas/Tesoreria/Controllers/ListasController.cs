using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    
    public class ListasController : Controller
    {
        protected MunicipiosDAL municipios { get; set; }
        protected LocalidadesDAL localidades { get; set; }
        protected CallesDAL calles { get; set; }
        protected ColoniasDAL colonias { get; set; }
        protected CuentasBancariasDAL cuentasbancarias { get; set; }
        protected BancosRHDAL dalBancosRH { get; set; }
        protected TipoDoctosDAL dalDoctos { get; set; }
        protected ImpuestosDeduccionDAL dalImpuestos { get; set; }
        protected BancosDAL dalBancos { get; set; }
        protected ClasificaBeneficiariosDAL DALClasificaBeneficiarios { get; set; }
        protected BeneficiariosCuentasDAL DALBeneficiariosCuentas { get; set; }
        private CaCajasReceptorasDAL DALCajasReceptoras { get; set; }
        protected ParametrosDAL parametrosDal { get; private set; }
        protected TipoMovBancariosDAL DALTipoMovBan { get; private set; }
        protected FuenteDAL DALFuentes { get; private set; }
        

        public ListasController()
        {
            if (municipios == null) municipios = new MunicipiosDAL();
            if (localidades == null) localidades = new LocalidadesDAL();
            if (calles == null) calles = new CallesDAL();
            if (colonias == null) colonias = new ColoniasDAL();
            if (cuentasbancarias == null) cuentasbancarias = new CuentasBancariasDAL();
            if (dalBancosRH == null) dalBancosRH = new BancosRHDAL();
            if (dalImpuestos == null) dalImpuestos = new ImpuestosDeduccionDAL();
            if (dalBancos == null) dalBancos = new BancosDAL();
            if (DALClasificaBeneficiarios == null) DALClasificaBeneficiarios = new ClasificaBeneficiariosDAL();
            if (DALBeneficiariosCuentas == null) DALBeneficiariosCuentas = new BeneficiariosCuentasDAL();
            if (parametrosDal == null) parametrosDal = new ParametrosDAL();
            if (DALCajasReceptoras == null) DALCajasReceptoras = new CaCajasReceptorasDAL();
            if (DALTipoMovBan == null) DALTipoMovBan = new TipoMovBancariosDAL();
            if (DALFuentes == null) DALFuentes = new FuenteDAL();
        }

        public ActionResult Lista_Municipios(byte? Id_Estado)
        {
            if(Id_Estado.HasValue)
                return new JsonResult { Data = new SelectList(municipios.Get(x => x.Id_Estado == Id_Estado).OrderBy(x => x.Descripcion), "Id_Municipio", "Descripcion") };
            else
                return new JsonResult { Data = new SelectList(new List<Ca_Municipios>(), "Id_Municipio", "Descripcion") };
        }

        public ActionResult Lista_Localidades(byte? Id_Estado, short? Id_Municipio)
        {
            if (Id_Estado.HasValue && Id_Municipio.HasValue)
                return new JsonResult { Data = new SelectList(localidades.Get(x => x.Id_Estado == Id_Estado && x.Id_Municipio == Id_Municipio).OrderBy(x => x.Descripcion), "Id_Localidad", "Descripcion") };
            else
                return new JsonResult { Data = new SelectList(new List<Ca_Localidades>(), "Id_Localidad", "Descripcion") };
        }

        public ActionResult Lista_Calles(byte? Id_Estado, short? Id_Municipio, short? Id_Localidad)
        {
            if (Id_Estado.HasValue && Id_Municipio.HasValue && Id_Localidad.HasValue)
                return new JsonResult { Data = new SelectList(calles.Get(x => x.Id_Estado == Id_Estado && x.Id_Municipio == Id_Municipio && x.Id_Localidad == Id_Localidad).OrderBy(x=>x.Descripcion), "id_calle", "Descripcion") };
            else
                return new JsonResult { Data = new SelectList(new List<Ca_Calles>(), "id_calle", "Descripcion") };
        }

        public ActionResult Lista_Colonias(byte? Id_Estado, short? Id_Municipio, short? Id_Localidad)
        {
            if (Id_Estado.HasValue && Id_Municipio.HasValue && Id_Localidad.HasValue)
                return new JsonResult { Data = new SelectList(colonias.Get(x => x.Id_Estado == Id_Estado && x.Id_Municipio == Id_Municipio && x.Id_Localidad == Id_Localidad).OrderBy(x => x.Descripcion), "id_colonia", "Descripcion") };
            else
                return new JsonResult { Data = new SelectList(new List<Ca_Colonias>(), "id_colonia", "Descripcion") };
        }

        [HttpPost]
        public ActionResult List_CtaBancaria(string Id_Fuente)
        {
            return new JsonResult { Data = new SelectList(cuentasbancarias.Get(x => x.Id_Fuente == Id_Fuente), "Id_CtaBancaria", "Descripcion") };
        }
        public ActionResult ListCtaBancaria(int Id_Banco)
        {
            return new JsonResult { Data = new SelectList(cuentasbancarias.Get(x => x.Id_Banco == Id_Banco), "Id_CtaBancaria", "Descripcion") };
        }
        public ActionResult List_BancosRH()
        {
            return new JsonResult { Data = new SelectList(dalBancosRH.Get(), "Id_BancoRH", "Id_BancoRH"), JsonRequestBehavior =  JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult List_TipoDocto()
        {
            dalDoctos = new TipoDoctosDAL();
            return new JsonResult { Data = new SelectList(dalDoctos.Get(), "Id_Tipodocto", "Descripcion",1), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult List_Impuesto()
        {
            return new JsonResult { Data = new SelectList(dalImpuestos.Get(x=> x.Id_Tipo_ImpDed == 1), "Id_ImpDed", "Descripcion", 1), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult List_Deduccion()
        {
            return new JsonResult { Data = new SelectList(dalImpuestos.Get(x => x.Id_Tipo_ImpDed == 2), "Id_ImpDed", "Descripcion", 1), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult List_Bancos()
        {
            return new JsonResult { Data = new SelectList(dalBancos.Get().OrderBy(x=>x.Descripcion), "Id_Banco", "Descripcion") };
        }
        public ActionResult List_CajasReceptoras()
        {
            return new JsonResult { Data = new SelectList(DALCajasReceptoras.Get(), "Id_CajaR", "Descripcion") };
        }
        [HttpPost]
        public ActionResult List_BancosDesasignacion()
        {
            BD_TesoreriaEntities db = new BD_TesoreriaEntities();
            return new JsonResult { Data = new SelectList(db.vBancosDesasignacionCheques, "Id_CtaBancaria", "NombreCuentaBancaria") };
        }
        [HttpPost]
        public ActionResult getClasificacionBeneficiario(int? beneficiario)
        {
            List<Ca_ClasificaBeneficiarios> entities = new List<Ca_ClasificaBeneficiarios>();
            DALBeneficiariosCuentas.Get(reg => reg.Id_Beneficiario == beneficiario).ToList().ForEach(item => { entities.Add(DALClasificaBeneficiarios.GetByID(reg2 => reg2.Id_ClasificaBene == item.Id_ClasBeneficiario)); } );

            if(!beneficiario.HasValue)
                return new JsonResult { Data = new SelectList(dalImpuestos.Get(x => x.Id_Tipo_ImpDed == 2), "Id_ImpDed", "Descripcion", 1), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return new JsonResult { Data = new SelectList(entities, "Id_ClasificaBene", "Descripcion")};
        }

        [HttpPost]
        public JsonResult Lista_AnioFinanciamiento()
        {
            int param = Convert.ToInt32(parametrosDal.GetByID(x => x.Nombre == "Ejercicio").Valor);
            IDictionary<int,int> Anios = new Dictionary<int,int>{ { param-2002, param - 2 }, { param-2001, param - 1 },{ param-2000, param },{ param - 1999, param + 1 },{ param -1998, param + 2 }};
            return Json(new SelectList(Anios, "Key", "Value", param - 2000));
        }
        [HttpPost]
        public JsonResult Lista_FolioMovBancarios(int? Id_TipoMovimiento)
        {
            if(Id_TipoMovimiento.HasValue)
                return Json(new SelectList(DALTipoMovBan.Get(x => x.Id_TipoMovB == Id_TipoMovimiento), "Id_FolioMovB", "Descripcion"));
            return Json(new SelectList(DALTipoMovBan.Get(), "Id_FolioMovB", "Descripcion"));
        }
        [HttpPost]
        public ActionResult List_Meses()
        {
            return new JsonResult { Data = new SelectList(Diccionarios.Meses, "key", "value") };
        }
        [HttpPost]
        public ActionResult List_Fuentes()
        {
            List<jsonDefault> entities = new List<jsonDefault>();
            DALFuentes.Get().OrderBy(x=>x.Id_Fuente).ToList().ForEach(item => {
                jsonDefault temp = new jsonDefault();
                temp.Id = item.Id_Fuente;
                temp.Descripcion = String.Format("{0}-{1}", item.Id_Fuente, item.Descripcion);
                entities.Add(temp); 
            });
            return new JsonResult { Data = new SelectList(entities, "Id", "Descripcion") };
        }
    }
}

