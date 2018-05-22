using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    public class FocusOutController : Controller
    {
        protected AccionDAL accion { get; set; }
        protected ActividadMIRDAL actividad { get; set; }
        protected ActividadDAL actividadins { get; set; }
        protected AlcanceDAL alcancegeo { get; set; }
        protected AreasDAL areas { get; set; }
        protected ClasProgramaticaDAL clasprogramatica { get; set; }
        protected FuenteDAL fuentesfin { get; set; }
        protected FuncionDAL funciones { get; set; }
        protected ObjetoGDAL objetogasto { get; set; }
        protected ProgramaDAL programas { get; set; }
        protected ProcesoDAL proyecto { get; set; }
        protected TipoGastosDAL tipogasto { get; set; }
        protected TipoMetaDAL tipometa { get; set; }
        protected Llenado llenar { get; set; }
        protected BeneficiariosDAL beneficiarioDal { get; set; }
        protected MaPresupuestoEgDAL presupuestoDal { get; private set; }
        protected MaPresupuestoIngDAL ingresosDal { get; private set; }
        protected CuentasDAL cuentas { get; private set; }
        protected CentroRecaudadorDAL dalCentroR { get; private set; }
        protected FuenteIngDAL dalFuentesIng { get; private set; }
        protected ConceptosIngresosDAL dalConceptoIng { get; set; }
        protected Ca_CURDAL dalCUR { get; set; }
        private ParametrosDAL parametrosDAL { get; set; }
        public FocusOutController()
        {
            if (areas == null) areas = new AreasDAL();
            if (funciones == null) funciones = new FuncionDAL();
            if (actividadins == null) actividadins = new ActividadDAL();
            if (clasprogramatica == null) clasprogramatica = new ClasProgramaticaDAL();
            if (programas == null) programas = new ProgramaDAL();
            if (proyecto == null) proyecto = new ProcesoDAL();
            if (tipometa == null) tipometa = new TipoMetaDAL();
            if (actividad == null) actividad = new ActividadMIRDAL();
            if (accion == null) accion = new AccionDAL();
            if (alcancegeo == null) alcancegeo = new AlcanceDAL();
            if (tipogasto == null) tipogasto = new TipoGastosDAL();
            if (fuentesfin == null) fuentesfin = new FuenteDAL();
            if (objetogasto == null) objetogasto = new ObjetoGDAL();
            if (beneficiarioDal == null) beneficiarioDal = new BeneficiariosDAL();
            if (llenar == null) llenar = new Llenado();
            if (presupuestoDal == null) presupuestoDal = new MaPresupuestoEgDAL();
            if (cuentas == null) cuentas = new CuentasDAL();
            if (dalCentroR == null) dalCentroR = new CentroRecaudadorDAL();
            if (ingresosDal == null) ingresosDal = new MaPresupuestoIngDAL();
            if (dalFuentesIng == null) dalFuentesIng = new FuenteIngDAL();
            if (dalConceptoIng == null) dalConceptoIng = new ConceptosIngresosDAL();
            if (dalCUR == null) dalCUR = new Ca_CURDAL();
            if (parametrosDAL == null) parametrosDAL = new ParametrosDAL();
        }

        #region Presupuesto Egresos


        [HttpPost]
        public ActionResult Areas(De_ClavePresupuestal claves)
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            var Areas = presupuestoDal.Get(x => String.IsNullOrEmpty(claves.Id_Fuente_Filtro) ? x.Id_Area != null : x.Id_Fuente == claves.Id_Fuente_Filtro).GroupBy(x => x.Id_Area);//.ForEach(item=> { dataModel.Add(String.Format("{0}-{1}",areas.GetByID(x=> x.Id_Area == item.Key).Id_Area); });
            foreach (var item in Areas)
            {
                Ca_Areas temp = areas.GetByID(x=> x.Id_Area == item.Key);
                if(temp != null)
                {
                    dataModel.Add(String.Format("{0}-{1}",temp.Id_Area,temp.Descripcion));
                    dataIds.Add(temp.Id_Area);
                }
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public ActionResult CentroRecaudador()
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            var Centros = ingresosDal.Get().GroupBy(x => x.Id_CentroRecaudador);//.ForEach(item=> { dataModel.Add(String.Format("{0}-{1}",areas.GetByID(x=> x.Id_Area == item.Key).Id_Area); });
            foreach (var item in Centros)
            {
                Ca_CentroRecaudador temp = dalCentroR.GetByID(x => x.Id_CRecaudador == item.Key);
                if(temp != null)
                {
                    dataModel.Add(String.Format("{0}-{1}", temp.Id_CRecaudador, temp.Descripcion));
                    dataIds.Add(temp.Id_CRecaudador);
                }
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }
        
        [HttpPost]
        public ActionResult Funciones(De_ClavePresupuestal claves)
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            var Areas = presupuestoDal.Get(x => string.IsNullOrEmpty(claves.Id_Fuente_Filtro) ? x.Id_Area == claves.Id_Area : (x.Id_Area == claves.Id_Area && x.Id_Fuente == claves.Id_Fuente_Filtro)).GroupBy(x => x.Id_Funcion);
            foreach (var item in Areas)
            {
                Ca_Funciones temp = funciones.GetByID(x => x.Id_Funcion == item.Key);
                if(temp != null)
                {
                    dataModel.Add(String.Format("{0}-{1}", temp.Id_Funcion, temp.Descripcion));
                    dataIds.Add(temp.Id_Funcion);
                }
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public ActionResult GenericSearchClave(De_ClavePresupuestal claves)
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            if (claves.Id_Actual == "Id_ObjetoG")
            {
                List<CA_Cuentas> cuentasLst = cuentas.Get(x => x.Id_ObjetoG == claves.Id_ObjetoG).ToList();
                cuentasLst.ForEach(x => { dataModel.Add(String.Format("{0}-{1}", x.Id_Cuenta, x.Descripcion)); dataIds.Add(x.Id_Cuenta); });
            }
            else
            {
                IEnumerable<MA_PresupuestoEg> Areas = presupuestoDal.Get(x =>
                    (string.IsNullOrEmpty(claves.Id_Area) ? x.Id_Area != null : x.Id_Area.Equals(claves.Id_Area)) &&
                    (string.IsNullOrEmpty(claves.Id_Funcion) ? x.Id_Funcion != null : x.Id_Funcion == claves.Id_Funcion) &&
                    (string.IsNullOrEmpty(claves.Id_Actividad) ? x.Id_Actividad != null : x.Id_Actividad == claves.Id_Actividad) &&
                    (string.IsNullOrEmpty(claves.Id_ClasificacionP) ? x.Id_ClasificacionP != null : x.Id_ClasificacionP == claves.Id_ClasificacionP) &&
                    (string.IsNullOrEmpty(claves.Id_Programa) ? x.Id_Programa != null : x.Id_Programa == claves.Id_Programa) &&
                    (string.IsNullOrEmpty(claves.Id_Proceso) ? x.Id_Proceso != null : x.Id_Proceso == claves.Id_Proceso) &&
                    (string.IsNullOrEmpty(claves.Id_TipoMeta) ? x.Id_TipoMeta != null : x.Id_TipoMeta == claves.Id_TipoMeta) &&
                    (string.IsNullOrEmpty(claves.Id_ActividadMIR) ? x.Id_ActividadMIR != null : x.Id_ActividadMIR == claves.Id_ActividadMIR) &&
                    (string.IsNullOrEmpty(claves.Id_Alcance) ? x.Id_Alcance != null : x.Id_Alcance == claves.Id_Alcance) &&
                    (string.IsNullOrEmpty(claves.Id_TipoG) ? x.Id_TipoG != null : x.Id_TipoG == claves.Id_TipoG) &&
                    (string.IsNullOrEmpty(claves.Id_Fuente) ? x.Id_Fuente != null : x.Id_Fuente == claves.Id_Fuente) &&
                    (string.IsNullOrEmpty(claves.AnioFin) ? x.AnioFin != null : x.AnioFin == claves.AnioFin) &&
                    (string.IsNullOrEmpty(claves.Id_ObjetoG) ? x.Id_ObjetoG != null : x.Id_ObjetoG == claves.Id_ObjetoG) &&
                    (string.IsNullOrEmpty(claves.Id_Fuente_Filtro) ? x.Id_Fuente != null : x.Id_Fuente == claves.Id_Fuente_Filtro)
                    );
                switch (claves.Id_Actual)
                {
                    case "Id_Area":
                        foreach (var item in Areas.GroupBy(g => g.Id_Funcion))
                        {
                            Ca_Funciones temp = funciones.GetByID(x => x.Id_Funcion == item.Key);
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_Funcion, temp.Descripcion));
                            dataIds.Add(temp.Id_Funcion);
                        }
                        break;
                    case "Id_Funcion":
                        foreach (var item in Areas.GroupBy(g => g.Id_Actividad))
                        {
                            Ca_ActividadesInst temp = actividadins.GetByID(x => x.Id_Actividad == item.Key);
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_Actividad, temp.Descripcion));
                            dataIds.Add(temp.Id_Actividad);
                        }
                        break;
                    case "Id_Actividad":
                        foreach (var item in Areas.GroupBy(g => g.Id_ClasificacionP))
                        {
                            Ca_ClasProgramatica temp = clasprogramatica.GetByID(x => x.Id_ClasificacionP == item.Key);
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_ClasificacionP, temp.Descripcion));
                            dataIds.Add(temp.Id_ClasificacionP);
                        }
                        break;
                    case "Id_ClasificacionP":
                        foreach (var item in Areas.GroupBy(g => g.Id_Programa))
                        {
                            CA_Programas temp = programas.GetByID(x => x.Id_Programa == item.Key);
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_Programa, temp.Descripcion));
                            dataIds.Add(temp.Id_Programa);
                        }
                        break;
                    case "Id_Programa":
                        foreach (var item in Areas.GroupBy(g => g.Id_Proceso))
                        {
                            Ca_Proyecto temp = proyecto.GetByID(x => x.Id_Proceso == item.Key);
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_Proceso, temp.Descripcion));
                            dataIds.Add(temp.Id_Proceso);
                        }
                        break;
                    case "Id_Proceso":
                        foreach (var item in Areas.GroupBy(g => g.Id_TipoMeta))
                        {
                            Ca_TipoMeta temp = tipometa.GetByID(x => x.Id_TipoMeta == item.Key);
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_TipoMeta, temp.Descripcion));
                            dataIds.Add(temp.Id_TipoMeta);
                        }
                        break;
                    case "Id_TipoMeta":
                        foreach (var item in Areas.GroupBy(g => g.Id_ActividadMIR))
                        {
                            Ca_Actividad temp = actividad.GetByID(x => x.Id_ActividadMIR2 == item.Key && x.Id_Proceso == claves.Id_Proceso);
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_ActividadMIR2, temp.Descripcion));
                            dataIds.Add(temp.Id_ActividadMIR2);
                        }
                        break;
                    case "Id_ActividadMIR":
                        foreach (var item in Areas.GroupBy(g => g.Id_Accion))
                        {
                            Ca_Acciones temp = accion.GetByID(x => x.Id_Accion2 == item.Key && x.Id_Proceso == claves.Id_Proceso && x.Id_ActividadMIR2 == claves.Id_ActividadMIR);
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_Accion2, temp.Descripcion));
                            dataIds.Add(temp.Id_Accion2);
                        }
                        break;
                    case "Id_Accion":
                        foreach (var item in Areas.GroupBy(g => g.Id_Alcance))
                        {
                            Ca_AlcanceGeo temp = alcancegeo.GetByID(x => x.Id_AlcanceGeo == item.Key);
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_AlcanceGeo, temp.Descripcion));
                            dataIds.Add(temp.Id_AlcanceGeo);
                        }
                        break;
                    case "Id_Alcance":
                        foreach (var item in Areas.GroupBy(g => g.Id_TipoG))
                        {
                            Ca_TipoGastos temp = tipogasto.GetByID(x => x.Id_TipoGasto == item.Key);
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_TipoGasto, temp.Descripcion));
                            dataIds.Add(temp.Id_TipoGasto);
                        }
                        break;
                    case "Id_TipoG":
                        foreach (var item in Areas.GroupBy(g => g.Id_Fuente))
                        {
                            Ca_FuentesFin temp = fuentesfin.GetByID(x => x.Id_Fuente == item.Key && x.UltimoNivel == true);
                            if (temp != null)
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_Fuente, temp.Descripcion));
                                dataIds.Add(temp.Id_Fuente);
                            }
                        }
                        break;
                    case "Id_Fuente":
                        foreach (var item in Areas.GroupBy(g => g.AnioFin))
                        {
                            dataModel.Add(String.Format("{0}-{1}", item.Key, Convert.ToInt16(item.Key) + 2000));
                            dataIds.Add(item.Key);
                        }
                        break;
                    case "AnioFin":
                        foreach (var item in Areas.GroupBy(g => g.Id_ObjetoG))
                        {
                            CA_ObjetoGasto temp = objetogasto.GetByID(x => x.Id_ObjetoG == item.Key);
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_ObjetoG, temp.Descripcion));
                            dataIds.Add(temp.Id_ObjetoG);
                        }
                        break;
                }
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }
        //[HttpPost]
        //public ActionResult Area(string Id)
        //{
        //    return new JsonResult() { Data = areas.GetByID(x=> x.Id_Area == Id && x.UltimoNivel == true) }; 
        //}

        //[HttpPost]
        //public ActionResult Funcion(string Id)
        //{
        //    return new JsonResult() { Data = funciones.GetByID(x => x.Id_Funcion == Id && x.Subfuncion > 0) }; 
        //}
        [HttpPost]
        public ActionResult Actividad(String IdArea, String Id_Funcion)
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            var Areas = presupuestoDal.Get(x => x.Id_Area == IdArea && x.Id_Funcion == Id_Funcion).GroupBy(x => x.Id_Actividad);
            foreach (var item in Areas)
            {
                Ca_ActividadesInst temp = actividadins.GetByID(x => x.Id_Actividad == item.Key);
                dataModel.Add(String.Format("{0}-{1}", temp.Id_Actividad, temp.Descripcion));
                dataIds.Add(temp.Id_Actividad);
            }
            return Json(new { Data = dataModel, Ids = dataIds });
            //return new JsonResult() { Data = llenar.LLenado_CaActividades(Id.ToUpper()) }; 
        }
        [HttpPost]
        public ActionResult ClasificacionP(String IdArea, String Id_Funcion, String Id_Actividad)
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            var Areas = presupuestoDal.Get(x => x.Id_Area == IdArea && x.Id_Funcion == Id_Funcion && x.Id_Actividad == Id_Actividad).GroupBy(x => x.Id_ClasificacionP);
            foreach (var item in Areas)
            {
                Ca_ClasProgramatica temp = clasprogramatica.GetByID(x => x.Id_ClasificacionP == item.Key);
                dataModel.Add(String.Format("{0}-{1}", temp.Id_ClasificacionP, temp.Descripcion));
                dataIds.Add(temp.Id_ClasificacionP);
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }
        [HttpPost]
        public ActionResult Programa(String IdArea, String Id_Funcion, String Id_Actividad, String Id_ClasificacionP)
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            var Areas = presupuestoDal.Get(x => x.Id_Area == IdArea && x.Id_Funcion == Id_Funcion && x.Id_Actividad == Id_Actividad && x.Id_ClasificacionP == Id_ClasificacionP).GroupBy(x => x.Id_Programa);
            foreach (var item in Areas)
            {
                CA_Programas temp = programas.GetByID(x => x.Id_Programa == item.Key);
                dataModel.Add(String.Format("{0}-{1}", temp.Id_Programa, temp.Descripcion));
                dataIds.Add(temp.Id_Programa);
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }
        
        [HttpPost]
        public ActionResult Proceso(String IdArea, String Id_Funcion, String Id_Actividad, String Id_ClasificacionP, String Id_Programa)
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            var Areas = presupuestoDal.Get(x => x.Id_Programa == Id_Programa && x.Id_ClasificacionP == Id_ClasificacionP && x.Id_Area == IdArea && x.Id_Funcion == Id_Funcion && x.Id_Actividad == Id_Actividad && x.Id_ClasificacionP == Id_ClasificacionP).GroupBy(x => x.Id_Proceso);
            foreach (var item in Areas)
            {
                Ca_Proyecto temp = proyecto.GetByID(x => x.Id_Proceso == item.Key);
                dataModel.Add(String.Format("{0}-{1}", temp.Id_Proceso, temp.Descripcion));
                dataIds.Add(temp.Id_Proceso);
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public ActionResult TipoMeta(String IdArea, String Id_Funcion, String Id_Actividad, String Id_ClasificacionP, String Id_Proceso)
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            var Areas = presupuestoDal.Get(x => x.Id_Proceso == Id_Proceso && x.Id_ClasificacionP == Id_ClasificacionP && x.Id_Area == IdArea && x.Id_Funcion == Id_Funcion && x.Id_Actividad == Id_Actividad && x.Id_ClasificacionP == Id_ClasificacionP).GroupBy(x => x.Id_TipoMeta);
            foreach (var item in Areas)
            {
                Ca_TipoMeta temp = tipometa.GetByID(x => x.Id_TipoMeta == item.Key);
                dataModel.Add(String.Format("{0}-{1}", temp.Id_TipoMeta, temp.Descripcion));
                dataIds.Add(temp.Id_TipoMeta);
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public ActionResult ActividadMIR(String IdArea, String Id_Funcion, String Id_Actividad, String Id_ClasificacionP, String Id_Proceso, String Id_TipoMeta)
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            var Areas = presupuestoDal.Get(x => x.Id_TipoMeta == Id_TipoMeta && x.Id_Proceso == Id_Proceso && x.Id_ClasificacionP == Id_ClasificacionP && x.Id_Area == IdArea && x.Id_Funcion == Id_Funcion && x.Id_Actividad == Id_Actividad && x.Id_ClasificacionP == Id_ClasificacionP).GroupBy(x => x.Id_ActividadMIR);
            foreach (var item in Areas)
            {
                Ca_Actividad temp = actividad.GetByID(x => x.Id_ActividadMIR2 == item.Key && x.Id_Proceso == Id_Proceso);
                dataModel.Add(String.Format("{0}-{1}", temp.Id_ActividadMIR2, temp.Descripcion));
                dataIds.Add(temp.Id_ActividadMIR2);
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public ActionResult Accion(String IdArea, String Id_Funcion, String Id_Actividad, String Id_ClasificacionP, String Id_Proceso, String Id_TipoMeta, String Id_ActividarMIR, String Id_Programa)
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            var Areas = presupuestoDal.Get(x => x.Id_ActividadMIR == Id_ActividarMIR && x.Id_TipoMeta == Id_TipoMeta && x.Id_Proceso == Id_Proceso && x.Id_Programa == Id_Programa && x.Id_Area == IdArea && x.Id_Funcion == Id_Funcion && x.Id_Actividad == Id_Actividad && x.Id_ClasificacionP == Id_ClasificacionP).GroupBy(x => x.Id_Accion);
            foreach (var item in Areas)
            {
                Ca_Acciones temp = accion.GetByID(x => x.Id_Accion2 == item.Key && x.Id_Proceso == Id_Proceso && x.Id_ActividadMIR2 == Id_ActividarMIR);
                dataModel.Add(String.Format("{0}-{1}", temp.Id_Accion2, temp.Descripcion));
                dataIds.Add(temp.Id_Accion2);
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public ActionResult Alcance(String IdArea, String Id_Funcion, String Id_Actividad, String Id_ClasificacionP, String Id_Proceso, String Id_TipoMeta, String Id_ActividarMIR, String Id_Accion)
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            var Areas = presupuestoDal.Get(x => x.Id_Accion == Id_Accion && x.Id_ActividadMIR == Id_ActividarMIR && x.Id_TipoMeta == Id_TipoMeta && x.Id_Proceso == Id_Proceso && x.Id_ClasificacionP == Id_ClasificacionP && x.Id_Area == IdArea && x.Id_Funcion == Id_Funcion && x.Id_Actividad == Id_Actividad && x.Id_ClasificacionP == Id_ClasificacionP).GroupBy(x => x.Id_Alcance);
            foreach (var item in Areas)
            {
                Ca_AlcanceGeo temp = alcancegeo.GetByID(x => x.Id_AlcanceGeo == item.Key);
                dataModel.Add(String.Format("{0}-{1}", temp.Id_AlcanceGeo, temp.Descripcion));
                dataIds.Add(temp.Id_AlcanceGeo);
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public ActionResult TipoG(String IdArea, String Id_Funcion, String Id_Actividad, String Id_ClasificacionP, String Id_Proceso, String Id_TipoMeta, String Id_ActividarMIR, String Id_Accion, String Id_Alcance)
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            var Areas = presupuestoDal.Get(x => x.Id_Alcance == Id_Alcance && x.Id_Accion == Id_Accion && x.Id_ActividadMIR == Id_ActividarMIR && x.Id_TipoMeta == Id_TipoMeta && x.Id_Proceso == Id_Proceso && x.Id_ClasificacionP == Id_ClasificacionP && x.Id_Area == IdArea && x.Id_Funcion == Id_Funcion && x.Id_Actividad == Id_Actividad && x.Id_ClasificacionP == Id_ClasificacionP).GroupBy(x => x.Id_TipoG);
            foreach (var item in Areas)
            {
                Ca_TipoGastos temp = tipogasto.GetByID(x => x.Id_TipoGasto == item.Key);
                dataModel.Add(String.Format("{0}-{1}", temp.Id_TipoGasto, temp.Descripcion));
                dataIds.Add(temp.Id_TipoGasto);
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }
        
        [HttpPost]
        public ActionResult Fuente(String IdArea, String Id_Funcion, String Id_Actividad, String Id_ClasificacionP, String Id_Proceso, String Id_TipoMeta, String Id_ActividarMIR, String Id_Accion, String Id_Alcance, String Id_TipoG)
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            var Areas = presupuestoDal.Get(x => x.Id_TipoG == Id_TipoG && x.Id_Alcance == Id_Alcance && x.Id_Accion == Id_Accion && x.Id_ActividadMIR == Id_ActividarMIR && x.Id_TipoMeta == Id_TipoMeta && x.Id_Proceso == Id_Proceso && x.Id_ClasificacionP == Id_ClasificacionP && x.Id_Area == IdArea && x.Id_Funcion == Id_Funcion && x.Id_Actividad == Id_Actividad && x.Id_ClasificacionP == Id_ClasificacionP).GroupBy(x => x.Id_Fuente);
            foreach (var item in Areas)
            {
                Ca_FuentesFin temp = fuentesfin.GetByID(x => x.Id_Fuente == item.Key && x.UltimoNivel == true);
                if(temp != null)
                {
                    dataModel.Add(String.Format("{0}-{1}", temp.Id_Fuente, temp.Descripcion));
                    dataIds.Add(temp.Id_Fuente);
                }
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public ActionResult FuenteCP(string name)
        {
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            fuentesfin.Get(reg => reg.Id_Fuente != null);
            List<Ca_FuentesFin> ca_fuentes = fuentesfin.Get(x => name.Equals("-") ? x.Id_Fuente != null : x.Id_Fuente.Contains(name) || x.Descripcion.Contains(name)).ToList();
            var fuentes = ca_fuentes.GroupBy(g => g.Id_Fuente);
            foreach (var item in fuentes)
            {
                Ca_FuentesFin registro = ca_fuentes.Where(reg => reg.Id_Fuente.Equals(item.Key)).First();
                if (presupuestoDal.Get(reg => reg.Id_Fuente.Contains(item.Key)).Count() > 0)
                {
                    dataModel.Add(String.Format("{0}-{1}", registro.Id_Fuente, registro.Descripcion));
                    dataIds.Add(registro.Id_Fuente);
                }                
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

       

        [HttpPost]
        public ActionResult FuenteCRI(String IdCentroR)
        {
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            var Fuentes = ingresosDal.Get(x => x.Id_CentroRecaudador == IdCentroR).GroupBy(x => x.Id_Fuente);
            foreach(var item in Fuentes)
            {
                Ca_FuentesFin_Ing temp = dalFuentesIng.GetByID(x => x.Id_FuenteFinancia == item.Key && x.UltimoNivel == true);
                if(temp != null)
                {
                    dataModel.Add(String.Format("{0}-{1}", temp.Id_FuenteFinancia, temp.Descripcion));
                    dataIds.Add(temp.Id_FuenteFinancia);
                }
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public ActionResult AlcanceCRI(String IdAnio,String IdCentroR, String IdFuente)
        {
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            var Fuentes = ingresosDal.Get(x => x.AnioFin == IdAnio && x.Id_CentroRecaudador == IdCentroR && x.Id_Fuente == IdFuente).GroupBy(x => x.Id_Alcance);
            foreach (var item in Fuentes)
            {
                Ca_AlcanceGeo temp = alcancegeo.GetByID(x => x.Id_AlcanceGeo == item.Key );
                dataModel.Add(String.Format("{0}-{1}", temp.Id_AlcanceGeo, temp.Descripcion));
                dataIds.Add(temp.Id_AlcanceGeo);
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }
        [HttpPost]
        public ActionResult ConceptoCRI(String IdCentroR, String IdFuente, String IdAlcance)
        {
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            var Fuentes = ingresosDal.Get(x => x.Id_CentroRecaudador == IdCentroR && x.Id_Fuente == IdFuente && x.Id_Alcance == IdAlcance).GroupBy(x => x.Id_Concepto);
            foreach (var item in Fuentes)
            {
                Ca_ConceptosIngresos temp = dalConceptoIng.GetByID(x => x.Id_Concepto == item.Key && x.UltimoNivel == true);
                if(temp != null)
                {
                    dataModel.Add(String.Format("{0}-{1}", temp.Id_Concepto, temp.Descripcion));
                    dataIds.Add(temp.Id_Concepto);
                }
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public ActionResult CUR(String IdConcepto)
        {
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            var Curs = dalCUR.Get(x => x.Id_Concepto == IdConcepto && x.UltimoNivel == true);
            foreach (var item in Curs)
            {
                dataModel.Add(String.Format("{0}-{1}", item.IdCUR, item.Descripcion));
                dataIds.Add(item.IdCUR);
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public ActionResult ObjetoG(String IdAnio, String IdArea, String Id_Funcion, String Id_Actividad, String Id_ClasificacionP, String Id_Proceso, String Id_TipoMeta, String Id_ActividarMIR, String Id_Accion, String Id_Alcance, String Id_TipoG, String Id_Fuente)
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            var Areas = presupuestoDal.Get(x => x.AnioFin == IdAnio && x.Id_Fuente == Id_Fuente && x.Id_TipoG == Id_TipoG && x.Id_Alcance == Id_Alcance && x.Id_Accion == Id_Accion && x.Id_ActividadMIR == Id_ActividarMIR && x.Id_TipoMeta == Id_TipoMeta && x.Id_Proceso == Id_Proceso && x.Id_ClasificacionP == Id_ClasificacionP && x.Id_Area == IdArea && x.Id_Funcion == Id_Funcion && x.Id_Actividad == Id_Actividad && x.Id_ClasificacionP == Id_ClasificacionP).GroupBy(x => x.Id_ObjetoG);
            foreach (var item in Areas)
            {
                CA_ObjetoGasto temp = objetogasto.GetByID(x => x.Id_ObjetoG == item.Key);
                dataModel.Add(String.Format("{0}-{1}", temp.Id_ObjetoG, temp.Descripcion));
                dataIds.Add(temp.Id_ObjetoG);
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public ActionResult AnioFin(String IdArea, String Id_Funcion, String Id_Actividad, String Id_ClasificacionP, String Id_Proceso, String Id_TipoMeta, String Id_ActividarMIR, String Id_Accion, String Id_Alcance, String Id_TipoG, String Id_Fuente)
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            var Areas = presupuestoDal.Get(x => x.Id_Fuente == Id_Fuente && x.Id_TipoG == Id_TipoG && x.Id_Alcance == Id_Alcance && x.Id_Accion == Id_Accion && x.Id_ActividadMIR == Id_ActividarMIR && x.Id_TipoMeta == Id_TipoMeta && x.Id_Proceso == Id_Proceso && x.Id_ClasificacionP == Id_ClasificacionP && x.Id_Area == IdArea && x.Id_Funcion == Id_Funcion && x.Id_Actividad == Id_Actividad && x.Id_ClasificacionP == Id_ClasificacionP).GroupBy(x => x.AnioFin);
            foreach (var item in Areas)
            {
                dataModel.Add(String.Format("{0}-{1}", item.Key, Convert.ToInt16(item.Key) + 2000));
                dataIds.Add(item.Key);
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public ActionResult AnioFinCRI(String IdCentroR, String IdFuente)
        {
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            var Fuentes = ingresosDal.Get(x => x.Id_CentroRecaudador == IdCentroR && x.Id_Fuente == IdFuente).GroupBy(x => x.AnioFin);
            foreach (var item in Fuentes)
            {
                dataModel.Add(String.Format("{0}-{1}", item.Key, Convert.ToInt16(item.Key) + 2000 ));
                dataIds.Add(item.Key);
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public JsonResult Beneficiario(Int32? IdBeneficiario)
        {
            try
            {
                Ca_BeneficiariosModel dataModel = llenar.Llenado_CaBeneficiarios(IdBeneficiario);
                return new JsonResult() { Data = dataModel };
            }
            catch (Exception ex)
            {
                return new JsonResult() { Data = new Ca_BeneficiariosModel() };
            }
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
            entities.ForEach(item => { models.Add(llenar.Llenado_CaBeneficiarios(item.Id_Beneficiario)); });
            if (!String.IsNullOrEmpty(BDescripcionBeneficiario))
                models = models.Where(reg => reg.NombreCompleto.Contains(BDescripcionBeneficiario)).ToList();
            return View(models);                
        }

        #endregion

        public JsonResult ValidarProceso(String Id_Programa)
        {
            //List<MA_PresupuestoEg> presupuesto = presupuestoDal.Get(x => x.Id_Programa == Id_Programa).ToList();
            if (presupuestoDal.Get(x => x.Id_Programa == Id_Programa).Count() > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidarMeta(String Id_Programa, String Id_Proceso)
        {
            List<MA_PresupuestoEg> presupuesto = presupuestoDal.Get(x => x.Id_Programa == Id_Programa && x.Id_Proceso == Id_Proceso).ToList();
            if (presupuesto != null && presupuesto.Count > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidarMIR(String Id_Programa, String Id_Proceso, String Id_TipoMeta)
        {
            if (presupuestoDal.Get(x => x.Id_Programa == Id_Programa && x.Id_Proceso == Id_Proceso && x.Id_TipoMeta == Id_TipoMeta).Count() > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidarAccion(String Id_Programa, String Id_Proceso, String Id_TipoMeta,String Id_ActividadMIR)
        {
            if (presupuestoDal.Get(x => x.Id_Programa == Id_Programa && x.Id_Proceso == Id_Proceso && x.Id_TipoMeta == Id_TipoMeta && x.Id_ActividadMIR == Id_ActividadMIR).Count() > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarArea(string Id_Area)
        {
            if (areas.Get(x => x.Id_Area == Id_Area && x.UltimoNivel == true).Count() > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidarFuncion(string Id_Funcion)
        {
            if (funciones.Get(x => x.Id_Funcion == Id_Funcion && x.Subfuncion > 0).Count() > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidarActividad(string Id_Actividad)
        {
            if (actividadins.Get(x => x.Id_Actividad == Id_Actividad).Count() > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidarClafificacion(string Id_ClasificacionP)
        {
            if (clasprogramatica.Get(x => x.Id_ClasificacionP == Id_ClasificacionP).Count() > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarPrograma(string Id_Programa)
        {
            if (presupuestoDal.Get(x => x.Id_Programa == Id_Programa).Count() > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidarAlcance(string Id_Alcance)
        {
            if (alcancegeo.Get(x => x.Id_AlcanceGeo == Id_Alcance).Count() > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidarTipoG(string Id_TipoG)
        {

            if (tipogasto.Get(x => x.Id_TipoGasto == Id_TipoG).Count() > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidarFuente(string Id_Fuente)
        {
            if (fuentesfin.Get(x => x.Id_Fuente == Id_Fuente).Count() > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidarObjetoG(string Id_ObjetoG)
        {
            if (objetogasto.Get(x => x.Id_ObjetoG == Id_ObjetoG && x.UltimoNivel == true).Count() > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CuentaGC(byte TipoCR)
        {
            try
            {
                if (TipoCR == Diccionarios.TiposCR.FondosRevolventes)
                    return Json(new { Data = parametrosDAL.GetByID(x => x.Nombre == "CuentaFR").Valor, Exito = true }, JsonRequestBehavior.AllowGet);
                else if(TipoCR == Diccionarios.TiposCR.AnticiposPrestamos)
                    return Json(new { Data = parametrosDAL.GetByID(x => x.Nombre == "Anticipos").Valor, Exito = true }, JsonRequestBehavior.AllowGet);
                else if (TipoCR == Diccionarios.TiposCR.CancelacionPasivos)
                    return Json(new { Data = parametrosDAL.GetByID(x => x.Nombre == "CancelaPasivos").Valor, Exito = true }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Data = parametrosDAL.GetByID(x => x.Nombre == "CuentaGC").Valor, Exito = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Exito = false, Mensaje = "No se encontró el parametro o no tiene valor."}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CuentaxPagar()
        {
            try
            {
                return Json(parametrosDAL.GetByID(x => x.Nombre == "CuentaPorPagar_GCFR").Valor, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CuentasEfectivo()
        {
            try
            {
                return Json(parametrosDAL.GetByID(x => x.Nombre == "Cuentas_Efectivo_GCFR").Valor, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CuentasPorOG(String Id_ObjetoG)
        {
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            List<CA_Cuentas> cuentasLst = cuentas.Get(x=> x.Id_ObjetoG == Id_ObjetoG).ToList();
            cuentasLst.ForEach(x => { dataModel.Add(String.Format("{0}-{1}", x.Id_Cuenta, x.Descripcion)); dataIds.Add(x.Id_Cuenta); });

            return Json(new { Data = dataModel, Ids = dataIds });
        }
        [HttpPost]
        public JsonResult CuentasPorCRI(String IdConcepto)
        {
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            List<CA_Cuentas> cuentasLst = cuentas.Get(x => x.Id_Concepto == IdConcepto).ToList();
            cuentasLst.ForEach(x => { dataModel.Add(String.Format("{0}-{1}", x.Id_Cuenta,x.Descripcion)); dataIds.Add(x.Id_Cuenta); });
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        public ActionResult CuentasPorBalance()
        {
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            FiltrosCuentas filtros = new FiltrosCuentas();
            filtros.GeneroStr = "1,2,3,6,7,9";
            filtros.selectUltimoNivel = true;
            filtros.UltimoNivel = true;
            Cuentas c = new Cuentas();
            List<Ca_CuentasModel> cuentasLst = c.listaCuentas(filtros);
            cuentasLst.Where(x=> x.Nivel == true).ToList().ForEach(x => { dataModel.Add(String.Format("{0}-{1}", x.Id_Cuenta, x.Descripcion)); dataIds.Add(x.Id_Cuenta); });
            CA_Parametros param = parametrosDAL.GetByID(x => x.Nombre=="Cuenta_Banco");
            return Json(new { Data = dataModel, Ids = dataIds, cuentasBancos = param.Valor.Split(',') });
        }

        [HttpPost]
        [ActionName("CancelacionPasivos")]
        public ActionResult CuentasPorBalance(string generos)
        {
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            FiltrosCuentas filtros = new FiltrosCuentas();
            filtros.GeneroStr = generos;
            filtros.selectUltimoNivel = true;
            filtros.UltimoNivel = true;
            Cuentas c = new Cuentas();
            List<Ca_CuentasModel> cuentasLst = c.listaCuentas(filtros);
            cuentasLst.Where(x => x.Nivel == true).ToList().ForEach(x => { dataModel.Add(String.Format("{0}-{1}", x.Id_Cuenta, x.Descripcion)); dataIds.Add(x.Id_Cuenta); });
            CA_Parametros param = parametrosDAL.GetByID(x => x.Nombre == "Cuenta_Banco");
            return Json(new { Data = dataModel, Ids = dataIds, cuentasBancos = param.Valor.Split(',') });
        }

        [HttpPost]
        [ActionName("Gastos_Fondos")]
        public ActionResult CuentasPorBalance(byte TipoCR)
        {
            /*else if(TipoCR == Diccionarios.TiposCR.AnticiposPrestamos)
                    return Json(new { Data = parametrosDAL.GetByID(x => x.Nombre == "Anticipos").Valor, Exito = true }, JsonRequestBehavior.AllowGet);
                else if (TipoCR == Diccionarios.TiposCR.CancelacionPasivos)
                    return Json(new { Data = parametrosDAL.GetByID(x => x.Nombre == "CancelaPasivos").Valor, Exito = true }, JsonRequestBehavior.AllowGet);*/
            FiltrosCuentas filtros = new FiltrosCuentas();
            switch (TipoCR)
            {
                case Diccionarios.TiposCR.FondosRevolventes:
                    filtros.IdCuenta = parametrosDAL.GetByID(x => x.Nombre == "CuentaFR").Valor;
                    break;
                case Diccionarios.TiposCR.AnticiposPrestamos:
                    filtros.IdCuenta = parametrosDAL.GetByID(x => x.Nombre == "Anticipos").Valor;
                    break;
                case Diccionarios.TiposCR.CancelacionPasivos:
                    filtros.IdCuenta = parametrosDAL.GetByID(x => x.Nombre == "CancelaPasivos").Valor;
                    break;
                case Diccionarios.TiposCR.GastosComprobar:
                    filtros.IdCuenta = parametrosDAL.GetByID(x => x.Nombre == "CuentaGC").Valor;
                    break;
                case Diccionarios.TiposCR.Arrendamientos:
                    filtros.IdCuenta = parametrosDAL.GetByID(x => x.Nombre == "CuentaArrendamiento").Valor;
                    break;
                case Diccionarios.TiposCR.CancelacionActivos:
                    filtros.IdCuenta = parametrosDAL.GetByID(x => x.Nombre == "CuentaCancelacionActivo").Valor;
                    break;
                case Diccionarios.TiposCR.Honorarios:
                    filtros.IdCuenta = parametrosDAL.GetByID(x => x.Nombre == "CuentaHonorarios").Valor;
                    break;
                case Diccionarios.TiposCR.HonorariosAsimilables:
                    filtros.IdCuenta = parametrosDAL.GetByID(x => x.Nombre == "CuentaHonorariosAsimilables").Valor;
                    break;
                default:
                    break;
            }
            filtros.Completa = true;
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            Cuentas c = new Cuentas();
            List<Ca_CuentasModel> cuentasLst = c.listaCuentas(filtros);
            cuentasLst.Where(x => x.Nivel == true).ToList().ForEach(x => { dataModel.Add(String.Format("{0}-{1}", x.Id_Cuenta, x.Descripcion)); dataIds.Add(x.Id_Cuenta); });
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        [ActionName("CuentasxPagar")]
        public ActionResult CuentasPorBalance(bool? nada)
        {
            FiltrosCuentas filtros = new FiltrosCuentas();
            filtros.IdCuenta = parametrosDAL.GetByID(x => x.Nombre == "CuentaPorPagar_GCFR").Valor;
            filtros.Completa = true;
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            Cuentas c = new Cuentas();
            List<Ca_CuentasModel> cuentasLst = c.listaCuentas(filtros);
            cuentasLst.Where(x => x.Nivel == true).ToList().ForEach(x => { dataModel.Add(String.Format("{0}-{1}", x.Id_Cuenta, x.Descripcion)); dataIds.Add(x.Id_Cuenta); });
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        [ActionName("CuentasEfectivo")]
        public ActionResult CuentasPorBalance(short? nada)
        {
            FiltrosCuentas filtros = new FiltrosCuentas();
            filtros.IdCuenta = parametrosDAL.GetByID(x => x.Nombre == "Cuentas_Efectivo_GCFR").Valor;
            filtros.Completa = true;
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            Cuentas c = new Cuentas();
            List<Ca_CuentasModel> cuentasLst = c.listaCuentas(filtros);
            cuentasLst.Where(x => x.Nivel == true).ToList().ForEach(x => { dataModel.Add(String.Format("{0}-{1}", x.Id_Cuenta, x.Descripcion)); dataIds.Add(x.Id_Cuenta); });
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        [HttpPost]
        [ActionName("SearchCuentasBalance")]
        public ActionResult CuentasPorBalance(FiltrosCuentas filtros)
        {
            List<String> dataModel = new List<string>();
            List<String> dataIds = new List<string>();
            Cuentas c = new Cuentas();
            List<Ca_CuentasModel> cuentasLst = c.listaCuentas(filtros);
            cuentasLst.Where(x => x.Nivel == true).ToList().ForEach(x => { dataModel.Add(String.Format("{0}-{1}", x.Id_Cuenta, x.Descripcion)); dataIds.Add(x.Id_Cuenta); });
            CA_Parametros param = parametrosDAL.GetByID(x => x.Nombre == "Cuenta_Banco");
            return Json(new { Data = dataModel, Ids = dataIds, cuentasBancos = param.Valor.Split(',') });
        }
    }
}
