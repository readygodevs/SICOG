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
    public class BusquedasController : Controller
    {
        protected BeneficiariosDAL beneficiarioDal { get; set; }
        protected AreasDAL areaDal { get; set; }
        protected Llenado llenar { get; set; }
        protected PersonasDAL personas { get; set; }
        public BusquedasController()
        {
            if (beneficiarioDal == null) beneficiarioDal = new BeneficiariosDAL();
            if (areaDal == null) areaDal = new AreasDAL();
            if (llenar == null) llenar = new Llenado();
            if (personas == null) personas = new PersonasDAL();
        }

        [HttpGet]
        public ActionResult Buscar_Beneficiario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Tbl_Beneficiario(string BDescripcionBeneficiario)
        {
            //List<Ca_Beneficiarios> entities = beneficiarioDal.Get(reg => reg.Nombre.Contains(BDescripcionBeneficiario) || reg.ApellidoPaterno.Contains(BDescripcionBeneficiario) || reg.ApellidoMaterno.Contains(BDescripcionBeneficiario)).ToList();
            List<Ca_Beneficiarios> entities = beneficiarioDal.Get().ToList();
            List<Ca_BeneficiariosModel> models = new List<Ca_BeneficiariosModel>();
            entities.ForEach(item => { models.Add(llenar.BusquedaBeneficiario(item.Id_Beneficiario,item.Id_TipoBeneficiario)); });
            if(!String.IsNullOrEmpty(BDescripcionBeneficiario))
                models = models.Where(reg => reg.NombreCompleto != null && reg.NombreCompleto.Contains(BDescripcionBeneficiario)).ToList();
            return View(models);
        }

        [HttpGet]
        public ActionResult Area()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Tbl_Area(string BDescripcionArea)
        {
            List<Ca_Areas> entities = areaDal.Get(reg => reg.Descripcion.Contains(BDescripcionArea)).ToList();
            List<Ca_AreasModel> models = new List<Ca_AreasModel>();
            entities.ForEach(item => { models.Add(llenar.LLenado_CaAreas(item.Id_Area)); });
            return View(models);
        }

        [HttpPost]
        public ActionResult Tbl_Personas(string BDescripcionPersona)
        {
            //List<Ca_Beneficiarios> entities = beneficiarioDal.Get(reg => reg.Nombre.Contains(BDescripcionBeneficiario) || reg.ApellidoPaterno.Contains(BDescripcionBeneficiario) || reg.ApellidoMaterno.Contains(BDescripcionBeneficiario)).ToList();
            List<CA_Personas> entities = personas.Get(reg => reg.Nombre.Contains(BDescripcionPersona) || reg.ApellidoPaterno.Contains(BDescripcionPersona) || reg.ApellidoMaterno.Contains(BDescripcionPersona) || reg.RazonSocial.Contains(BDescripcionPersona)).ToList();
            List<Ca_PersonasModel> models = new List<Ca_PersonasModel>();
            entities.ForEach(item => { models.Add(llenar.Llenado_CaPersonas(item.IdPersona)); });
            //models = models.Where(reg => reg.NombreCompleto.Contains(BDescripcionPersona)).ToList();
            return View(models);
        }

    }
}
