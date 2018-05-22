using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

    /*
     * Presupuesto de Egresos
     */
    public class PresupuestosController : Controller
    {
        protected MaTrasnferenciasDAL MaTranferenciasDAL { get; set; }
        protected MaPolizasDAL MaPolizasDAL { get; set; }
        protected CierreMensualDAL cierreMensualDAL { get; set; }
        protected DeDisponibilidadDAL DeDisponibilidadDAL { get; set; }
        protected DeTransferenciaDAL DeTransferenciasDAL { get; set; }
        protected ProceduresDAL proceduresDAL { get; set; }

        protected MaPresupuestoEgDAL DALMaPresupuestoEg { get; set; }
        protected DeDisponibilidadDAL DALDeDisponibilidad { get; set; }
        protected DisponibilidadAnualDAL DALvDisponibilidad { get; set; }
        protected ConvertHtmlToString reports { get; set; }

        protected VW_DisponibilidadMesesDAL disponibilidadMeses { get; set; }

        protected AmpliacionesReduccionesBL AmpliacionesReduccionesBL { get; set; }
        protected Llenado llenar { get; set; }
        public PresupuestosController()
        {
            if (MaPolizasDAL == null) MaPolizasDAL = new MaPolizasDAL();
            if (MaTranferenciasDAL == null) MaTranferenciasDAL = new MaTrasnferenciasDAL();
            if (cierreMensualDAL == null) cierreMensualDAL = new CierreMensualDAL();
            if (DeDisponibilidadDAL == null) DeDisponibilidadDAL = new DeDisponibilidadDAL();
            if (DeTransferenciasDAL == null) DeTransferenciasDAL = new DeTransferenciaDAL();
            if (proceduresDAL == null) proceduresDAL = new ProceduresDAL();

            if (AmpliacionesReduccionesBL == null) AmpliacionesReduccionesBL = new AmpliacionesReduccionesBL();
            if (llenar == null) llenar = new Llenado();

            if (DALMaPresupuestoEg == null) DALMaPresupuestoEg = new MaPresupuestoEgDAL();
            if (DALDeDisponibilidad == null) DALDeDisponibilidad = new DeDisponibilidadDAL();
            if (DALvDisponibilidad == null) DALvDisponibilidad = new DisponibilidadAnualDAL();
            if (reports == null) reports = new ConvertHtmlToString();

            if (disponibilidadMeses == null) disponibilidadMeses = new VW_DisponibilidadMesesDAL();
        }
        public ActionResult Index()
        {
            GetTipoMeta();
            ViewBag.Ejercicio = new ParametrosDAL().GetByID(x => x.Descripcion == "Ejercicio contable").Valor;
            return View(new MA_PresupuestoEgModel());
        }

        public void GetTipoMeta()
        {
            TipoMetaDAL tDal = new TipoMetaDAL();
            List<Ca_TipoMeta> tipoMeta = tDal.Get().ToList();
            if (tipoMeta.Count > 0)
            {
                if (tipoMeta.Count == 1)
                {
                    ViewBag.Id_TipoMeta = tipoMeta.FirstOrDefault().Id_TipoMeta;
                }
                else
                {
                    ViewBag.Id_TipoMeta = 0;
                }
            }
        }
        public List<String>[] Areas()
        {
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            AreasDAL DALAreas = new AreasDAL();
            var Areas = DALAreas.Get().GroupBy(x => x.Id_Area);//.ForEach(item=> { dataModel.Add(String.Format("{0}-{1}",areas.GetByID(x=> x.Id_Area == item.Key).Id_Area); });
            foreach (var item in Areas)
            {
                Ca_Areas temp = DALAreas.GetByID(x => x.Id_Area == item.Key);
                dataModel.Add(String.Format("{0}-{1}", temp.Id_Area, temp.Descripcion));
                dataIds.Add(temp.Id_Area);
            }
            return new List<String>[] { dataModel, dataIds };
        }
        [HttpPost]
        public JsonResult GetDescripcionTipoG(string Id_TipoG)
        {
            TipoGastosDAL DALTipo = new TipoGastosDAL();
            string Descripcion = DALTipo.GetByID(x => x.Id_TipoGasto == Id_TipoG).Descripcion;
            return Json(new { Descripcion = !String.IsNullOrEmpty(Descripcion) ? Descripcion : "" });
        }
        [HttpPost]
        public ActionResult GetDescripcion(string id, string objeto, string parametros, string tipo)
        {
            id = id.Trim();
            int o = Convert.ToInt16(objeto);
            MaPresupuestoEgDAL DALPrespuesto = new MaPresupuestoEgDAL();
            List<MA_PresupuestoEg> presupuesto = new List<MA_PresupuestoEg>();
            string[] arrParametros = null;
            if (!String.IsNullOrEmpty(parametros))
            {
                arrParametros = parametros.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            }
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            int take = 15;
            switch (o)
            {
                case 1:
                    AreasDAL DALAreas = new AreasDAL();
                    if (tipo == "2")
                    {
                        List<MA_PresupuestoEg> resultados = DALPrespuesto.Get(x => (id == "-" ? x.Id_Area != null : x.Id_Area.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            Ca_Areas temp = DALAreas.GetByID(x => x.Id_Area == item.Id_Area);
                            if (temp != null)
                            {
                                if (!dataIds.Contains(temp.Id_Area))
                                {
                                    dataModel.Add(String.Format("{0}-{1}", temp.Id_Area, temp.Descripcion));
                                    dataIds.Add(temp.Id_Area);
                                }
                            }
                        }
                    }
                    else
                    {
                        var Areas = DALAreas.Get(x => x.UltimoNivel == true && (x.Id_Area.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id))).OrderBy(x => x.Id_Area).GroupBy(x => x.Id_Area).Take(take);//.ForEach(item=> { dataModel.Add(String.Format("{0}-{1}",areas.GetByID(x=> x.Id_Area == item.Key).Id_Area); });
                        foreach (var item in Areas)
                        {
                            Ca_Areas temp = DALAreas.GetByID(x => x.Id_Area == item.Key);
                            if (temp != null)
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_Area, temp.Descripcion));
                                dataIds.Add(temp.Id_Area);
                            }

                        }
                    }
                    break;
                case 2:
                    FuncionDAL funcionModel = new FuncionDAL();
                    if (tipo == "2")
                    {
                        List<MA_PresupuestoEg> resultados = DALPrespuesto.Get(x => (id == "-" ? x.Id_Area != null : x.Id_Funcion.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            Ca_Funciones temp = funcionModel.GetByID(x => x.Id_Funcion == item.Id_Funcion);
                            if (temp != null)
                            {
                                if (!dataIds.Contains(temp.Id_Funcion))
                                {
                                    dataModel.Add(String.Format("{0}-{1}", temp.Id_Funcion, temp.Descripcion));
                                    dataIds.Add(temp.Id_Funcion);
                                }
                            }
                        }
                    }
                    else
                    {
                        var Funciones = funcionModel.Get(x => x.Subfuncion > 0 && (x.Id_Funcion.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id))).GroupBy(x => x.Id_Funcion).Take(take);
                        foreach (var item in Funciones)
                        {
                            Ca_Funciones temp = funcionModel.GetByID(x => x.Id_Funcion == item.Key);
                            if (temp != null)
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_Funcion, temp.Descripcion));
                                dataIds.Add(temp.Id_Funcion);
                            }

                        }
                    }
                    break;
                case 3:
                    ActividadDAL actividadModel = new ActividadDAL();
                    if (tipo == "2")
                    {
                        List<MA_PresupuestoEg> resultados = DALPrespuesto.Get(x => (id == "-" ? x.Id_Area != null : x.Id_Actividad.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            Ca_ActividadesInst temp = actividadModel.GetByID(x => x.Id_Actividad == item.Id_Actividad);
                            if (temp != null)
                            {
                                if (!dataIds.Contains(temp.Id_Actividad))
                                {
                                    dataModel.Add(String.Format("{0}-{1}", temp.Id_Actividad, temp.Descripcion));
                                    dataIds.Add(temp.Id_Actividad);
                                }
                            }

                        }
                    }
                    else
                    {
                        var Actividades = actividadModel.Get(x => x.Id_Actividad.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id)).GroupBy(x => x.Id_Actividad).Take(take);
                        foreach (var item in Actividades)
                        {
                            Ca_ActividadesInst temp = actividadModel.GetByID(x => x.Id_Actividad == item.Key);
                            if (temp != null)
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_Actividad, temp.Descripcion));
                                dataIds.Add(temp.Id_Actividad);
                            }
                        }
                    }
                    return Json(new { Data = dataModel, Ids = dataIds });
                case 4:
                    ClasificacionPDAL clasificacionModel = new ClasificacionPDAL();
                    if (tipo == "2")
                    {
                        List<MA_PresupuestoEg> resultados = DALPrespuesto.Get(x => (id == "-" ? x.Id_Area != null : x.Id_ClasificacionP.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            Ca_ClasProgramatica temp = clasificacionModel.GetByID(x => x.Id_ClasificacionP == item.Id_ClasificacionP);
                            if (temp != null)
                            {
                                if (!dataIds.Contains(temp.Id_ClasificacionP))
                                {
                                    dataModel.Add(String.Format("{0}-{1}", temp.Id_ClasificacionP, temp.Descripcion));
                                    dataIds.Add(temp.Id_ClasificacionP);
                                }
                            }
                        }
                    }
                    else
                    {
                        var Clasificaciones = clasificacionModel.Get(x => x.Id_ClasificacionP.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id)).GroupBy(x => x.Id_ClasificacionP).Take(take);
                        foreach (var item in Clasificaciones)
                        {
                            Ca_ClasProgramatica temp = clasificacionModel.GetByID(x => x.Id_ClasificacionP == item.Key);
                            if (temp != null)
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_ClasificacionP, temp.Descripcion));
                                dataIds.Add(temp.Id_ClasificacionP);
                            }
                        }
                    }
                    return Json(new { Data = dataModel, Ids = dataIds });
                case 5:
                    ProgramaDAL programaModel = new ProgramaDAL();
                    if (tipo == "2")
                    {
                        List<MA_PresupuestoEg> resultados = DALPrespuesto.Get(x => (id == "-" ? x.Id_Area != null : x.Id_Programa.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            CA_Programas temp = programaModel.GetByID(x => x.Id_Programa == item.Id_Programa);
                            if (temp != null)
                            {
                                if (!dataIds.Contains(temp.Id_Programa))
                                {
                                    dataModel.Add(String.Format("{0}-{1}", temp.Id_Programa, temp.Descripcion));
                                    dataIds.Add(temp.Id_Programa);
                                }
                            }
                        }
                    }
                    else
                    {
                        var Programas = programaModel.Get(x => x.Id_Programa.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id)).GroupBy(x => x.Id_Programa).Take(take);
                        foreach (var item in Programas)
                        {
                            CA_Programas temp = programaModel.GetByID(x => x.Id_Programa == item.Key);
                            if (temp != null)
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_Programa, temp.Descripcion));
                                dataIds.Add(temp.Id_Programa);
                            }
                        }
                    }
                    return Json(new { Data = dataModel, Ids = dataIds });
                case 6:
                    ProcesoDAL procesoModel = new ProcesoDAL();
                    if (tipo == "2")
                    {
                        List<MA_PresupuestoEg> resultados = DALPrespuesto.Get(x => (id == "-" ? x.Id_Area != null : x.Id_Proceso.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            Ca_Proyecto temp = procesoModel.GetByID(x => x.Id_Proceso == item.Id_Proceso);
                            if (temp != null)
                            {
                                if (!dataIds.Contains(temp.Id_Proceso))
                                {
                                    dataModel.Add(String.Format("{0}-{1}", temp.Id_Proceso, temp.Descripcion));
                                    dataIds.Add(temp.Id_Proceso);
                                }
                            }
                        }
                    }
                    else
                    {
                        var Procesos = procesoModel.Get(x => x.Id_Proceso.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id)).GroupBy(x => x.Id_Proceso).Take(take);
                        foreach (var item in Procesos)
                        {
                            Ca_Proyecto temp = procesoModel.GetByID(x => x.Id_Proceso == item.Key);
                            if (temp != null)
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_Proceso, temp.Descripcion));
                                dataIds.Add(temp.Id_Proceso);
                            }
                        }
                    }
                    return Json(new { Data = dataModel, Ids = dataIds });
                case 7:
                    TipoMetaDAL metaModel = new TipoMetaDAL();
                    if (tipo == "2")
                    {
                        List<MA_PresupuestoEg> resultados = DALPrespuesto.Get(x => (id == "-" ? x.Id_Area != null : x.Id_TipoMeta.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            Ca_TipoMeta temp = metaModel.GetByID(x => x.Id_TipoMeta == item.Id_TipoMeta);
                            if (temp != null)
                            {
                                if (!dataIds.Contains(temp.Id_TipoMeta))
                                {
                                    dataModel.Add(String.Format("{0}-{1}", temp.Id_TipoMeta, temp.Descripcion));
                                    dataIds.Add(temp.Id_TipoMeta);
                                }
                            }

                        }
                    }
                    else
                    {
                        var TiposMeta = metaModel.Get(x => x.Id_TipoMeta.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id)).GroupBy(x => x.Id_TipoMeta).Take(take);
                        foreach (var item in TiposMeta)
                        {
                            Ca_TipoMeta temp = metaModel.GetByID(x => x.Id_TipoMeta == item.Key);
                            if (temp != null)
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_TipoMeta, temp.Descripcion));
                                dataIds.Add(temp.Id_TipoMeta);
                            }
                        }
                    }
                    return Json(new { Data = dataModel, Ids = dataIds });
                case 8:
                    ActividadMIRDAL actividadMirModel = new ActividadMIRDAL();
                    if (tipo == "2")
                    {
                        List<MA_PresupuestoEg> resultados = DALPrespuesto.Get(x => (id == "-" ? x.Id_Area != null : x.Id_ActividadMIR.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            Ca_Actividad temp = actividadMirModel.Get(x => x.Id_ActividadMIR2 == item.Id_ActividadMIR).FirstOrDefault();
                            if (temp != null)
                            {
                                if (!dataIds.Contains(temp.Id_ActividadMIR2))
                                {
                                    dataModel.Add(String.Format("{0}-{1}", temp.Id_ActividadMIR2, temp.Descripcion));
                                    dataIds.Add(temp.Id_ActividadMIR2);
                                }
                            }

                        }
                    }
                    else
                    {
                        var ActMir = actividadMirModel.Get(x => x.Id_Proceso.Contains(id) || id == "-" || String.IsNullOrEmpty(id) ? x.Descripcion != null : x.Descripcion.Contains(id)).GroupBy(x => new { x.Id_Proceso, x.Id_ActividadMIR2 }).Take(take);
                        foreach (var item in ActMir)
                        {
                            Ca_Actividad temp = actividadMirModel.GetByID(x => x.Id_ActividadMIR2 == item.Key.Id_ActividadMIR2 && x.Id_Proceso == item.Key.Id_Proceso);
                            if (temp != null)
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_ActividadMIR2, temp.Descripcion));
                                dataIds.Add(temp.Id_ActividadMIR2);
                            }

                        }
                    }
                    return Json(new { Data = dataModel, Ids = dataIds });
                case 9:
                    AccionDAL accionModel = new AccionDAL();
                    if (tipo == "2")
                    {
                        List<MA_PresupuestoEg> resultados = DALPrespuesto.Get(x => (id == "-" ? x.Id_Area != null : x.Id_Accion.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            Ca_Acciones temp = accionModel.Get(x => x.Id_Accion2 == item.Id_Accion).FirstOrDefault();
                            if (temp != null)
                            {
                                if (!dataIds.Contains(temp.Id_Accion2))
                                {
                                    dataModel.Add(String.Format("{0}-{1}", temp.Id_Accion2, temp.Descripcion));
                                    dataIds.Add(temp.Id_Accion2);
                                }
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            string idProceso1 = "";
                            string idActividadMIR = "";
                            if (arrParametros.Length == 2)
                            {
                                idProceso1 = arrParametros[0].ToString();
                                idActividadMIR = arrParametros[1].ToString();
                            }

                            var Acciones = accionModel.Get(x => (String.IsNullOrEmpty(idProceso1) || idProceso1 == "-" ? x.Id_Proceso != null : x.Id_Proceso == idProceso1) && (String.IsNullOrEmpty(idActividadMIR) || idActividadMIR == "-" ? x.Id_ActividadMIR2 != null : x.Id_ActividadMIR2 == idActividadMIR) && (x.Id_Accion2.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id))).GroupBy(x => new { x.Id_Accion2, x.Id_Proceso, x.Id_ActividadMIR2 }).Take(take);
                            foreach (var item in Acciones)
                            {
                                Ca_Acciones temp = accionModel.GetByID(x => x.Id_Accion2 == item.Key.Id_Accion2 && x.Id_Proceso == item.Key.Id_Proceso && x.Id_ActividadMIR2 == item.Key.Id_ActividadMIR2);
                                if (temp != null)
                                {
                                    dataModel.Add(String.Format("{0}-{1}", temp.Id_Accion2, temp.Descripcion));
                                    dataIds.Add(temp.Id_Accion2);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            return Json(new { Data = dataModel, Ids = dataIds });
                        }
                    }
                    return Json(new { Data = dataModel, Ids = dataIds });
                case 10:
                    AlcanceDAL alcanceModel = new AlcanceDAL();
                    if (tipo == "2")
                    {
                        List<MA_PresupuestoEg> resultados = DALPrespuesto.Get(x => (id == "-" ? x.Id_Area != null : x.Id_Alcance.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            Ca_AlcanceGeo temp = alcanceModel.GetByID(x => x.Id_AlcanceGeo == item.Id_Alcance);
                            if (temp != null)
                            {
                                if (!dataIds.Contains(temp.Id_AlcanceGeo))
                                {
                                    dataModel.Add(String.Format("{0}-{1}", temp.Id_AlcanceGeo, temp.Descripcion));
                                    dataIds.Add(temp.Id_AlcanceGeo);
                                }
                            }
                        }
                    }
                    else
                    {
                        var Alcance = alcanceModel.Get(x => x.Id_AlcanceGeo.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id)).GroupBy(x => x.Id_AlcanceGeo).Take(take);
                        foreach (var item in Alcance)
                        {
                            Ca_AlcanceGeo temp = alcanceModel.GetByID(x => x.Id_AlcanceGeo == item.Key);
                            if (temp != null)
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_AlcanceGeo, temp.Descripcion));
                                dataIds.Add(temp.Id_AlcanceGeo);
                            }
                        }
                    }
                    return Json(new { Data = dataModel, Ids = dataIds });
                case 11:
                    TipoGastosDAL gastoModel = new TipoGastosDAL();
                    if (tipo == "2")
                    {
                        List<MA_PresupuestoEg> resultados = DALPrespuesto.Get(x => (id == "-" ? x.Id_Area != null : x.Id_TipoG.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            Ca_TipoGastos temp = gastoModel.GetByID(x => x.Id_TipoGasto == item.Id_TipoG);
                            if (temp != null)
                            {
                                if (!dataIds.Contains(temp.Id_TipoGasto))
                                {
                                    dataModel.Add(String.Format("{0}-{1}", temp.Id_TipoGasto, temp.Descripcion));
                                    dataIds.Add(temp.Id_TipoGasto);
                                }
                            }
                        }
                    }
                    else
                    {
                        var Gastos = gastoModel.Get(x => x.Id_TipoGasto.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id)).GroupBy(x => x.Id_TipoGasto).Take(take);
                        foreach (var item in Gastos)
                        {
                            Ca_TipoGastos temp = gastoModel.GetByID(x => x.Id_TipoGasto == item.Key);
                            if (temp != null)
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_TipoGasto, temp.Descripcion));
                                dataIds.Add(temp.Id_TipoGasto);
                            }
                        }
                    }
                    return Json(new { Data = dataModel, Ids = dataIds });
                case 12:
                    FuenteDAL fuenteModel = new FuenteDAL();
                    if (tipo == "2")
                    {
                        List<MA_PresupuestoEg> resultados = DALPrespuesto.Get(x => (id == "-" ? x.Id_Area != null : x.Id_Fuente.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            Ca_FuentesFin temp = fuenteModel.GetByID(x => x.Id_Fuente == item.Id_Fuente);
                            if (temp != null)
                            {
                                if (!dataIds.Contains(temp.Id_Fuente))
                                {
                                    dataModel.Add(String.Format("{0}-{1}", temp.Id_Fuente, temp.Descripcion));
                                    dataIds.Add(temp.Id_Fuente);
                                }
                            }
                        }
                    }
                    else
                    {
                        var Fuentes = fuenteModel.Get(x => x.UltimoNivel == true && (x.Id_Fuente.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id))).GroupBy(x => x.Id_Fuente).Take(take);
                        foreach (var item in Fuentes)
                        {
                            Ca_FuentesFin temp = fuenteModel.GetByID(x => x.Id_Fuente == item.Key);
                            if (temp != null)
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_Fuente, temp.Descripcion));
                                dataIds.Add(temp.Id_Fuente);
                            }
                        }
                    }
                    return Json(new { Data = dataModel, Ids = dataIds });
                //case 13:
                //     funcionModel = new FuncionDAL();
                //    Ca_Funciones funcion = funcionModel.GetByID(x=>x.Id_Funcion == id);
                //    descripcion= funcion.Descripcion;
                //    break;
                case 14:
                    ObjetoGDAL objetoModel = new ObjetoGDAL();
                    if (tipo == "2")
                    {
                        List<MA_PresupuestoEg> resultados = DALPrespuesto.Get(x => (id == "-" ? x.Id_Area != null : x.Id_ObjetoG.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            CA_ObjetoGasto temp = objetoModel.GetByID(x => x.Id_ObjetoG == item.Id_ObjetoG);
                            if (temp != null)
                            {
                                if (!dataIds.Contains(temp.Id_ObjetoG))
                                {
                                    dataModel.Add(String.Format("{0}-{1}", temp.Id_ObjetoG, temp.Descripcion));
                                    dataIds.Add(temp.Id_ObjetoG);
                                }
                            }
                        }
                    }
                    else
                    {
                        var Objetos = objetoModel.Get(x => x.UltimoNivel == true && (x.Id_ObjetoG.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id))).GroupBy(x => x.Id_ObjetoG).Take(take);
                        foreach (var item in Objetos)
                        {
                            CA_ObjetoGasto temp = objetoModel.GetByID(x => x.Id_ObjetoG == item.Key);
                            if (temp != null)
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_ObjetoG, temp.Descripcion));
                                dataIds.Add(temp.Id_ObjetoG);
                            }
                        }
                    }
                    return Json(new { Data = dataModel, Ids = dataIds });
                case 15:
                    CuentasDAL cuentas = new CuentasDAL();
                    var Cuentas = cuentas.Get(x => x.Id_Cuenta.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id)).GroupBy(x => x.Id_Cuenta).Take(take);
                    foreach (var item in Cuentas)
                    {
                        CA_Cuentas temp = cuentas.GetByID(x => x.Id_Cuenta == item.Key);
                        if (temp != null)
                        {
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_Cuenta, temp.Descripcion));
                            dataIds.Add(temp.Id_Cuenta);
                        }
                    }
                    return Json(new { Data = dataModel, Ids = dataIds });
                case 16:
                    List<MA_PresupuestoEg> res = DALPrespuesto.Get(x => id == "-" ? x.AnioFin != null : x.AnioFin.Contains(id)).ToList();
                    foreach (var item in res.GroupBy(x => x.AnioFin))
                    {
                        dataModel.Add(String.Format("{0}-{1}", item.Key, Convert.ToInt16(item.Key) + 2000));
                        dataIds.Add(item.Key);
                    }
                    break;
                default:
                    break;
            }
            return Json(new { Data = dataModel, Ids = dataIds });
        }

        public ActionResult NuevoPresupuesto()
        {
            try
            {
                return Json(new { Exito = true, Mensaje = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Buscar(string objeto, string clave, string descripcion, string ProyectoProceso, string ActividadMIR, int tipoBusqueda = 0)
        {
            return View(BusquedaPresupuestos.Lista(clave, descripcion, objeto, ProyectoProceso, ActividadMIR, tipoBusqueda));
        }

        public ActionResult BuscarCatalogo(string tipo, string label, string id, string ProyectoProceso, string ActividadMIR)
        {
            ViewBag.Tipo = tipo;
            ViewBag.Label = label;
            ViewBag.Id = id;
            ViewBag.ProyectoProceso = String.IsNullOrEmpty(ProyectoProceso) ? "" : ProyectoProceso;
            ViewBag.ActividadMIR = String.IsNullOrEmpty(ActividadMIR) ? "" : ActividadMIR;
            return View("BuscarCatalogo");
        }

        [HttpPost]
        public ActionResult AgregarPresupuesto(MA_PresupuestoEgModel presupuesto, FormCollection form)
        {
            try
            {
                if (ViewData.ModelState.IsValid)
                {
                    DateTime fechaAprobado = Convert.ToDateTime(presupuesto.Fecha_Aprobado.Value);
                    int fecha = Convert.ToInt16(fechaAprobado.Year);
                    ParametrosDAL pDal = new ParametrosDAL();
                    if (fecha == Convert.ToInt32(pDal.GetByID(x => x.Nombre == "Ejercicio").Valor))
                    {
                        CierreMensualDAL cierreDal = new CierreMensualDAL();
                        int mes = Convert.ToInt16(fechaAprobado.Month);
                        if (!(bool)cierreDal.GetByID(x => x.Id_Mes == mes).Contable)
                        {
                            try
                            {
                                MaPresupuestoEgDAL presupuestoDal = new MaPresupuestoEgDAL();
                                MA_PresupuestoEg p = EntityFactory.getEntity<MA_PresupuestoEg>(presupuesto, new MA_PresupuestoEg());
                                p.AnioFin = presupuesto.AnioFin;
                                string ClavePresupuesto = StringID.IdClavePresupuesto(p.Id_Area, p.Id_Funcion, p.Id_Actividad, p.Id_ClasificacionP, p.Id_Programa, p.Id_Proceso, p.Id_TipoMeta
                                    , p.Id_ActividadMIR, p.Id_Accion, p.Id_Alcance, p.Id_TipoG, p.Id_Fuente, p.AnioFin, p.Id_ObjetoG);
                                p.Id_ClavePresupuesto = ClavePresupuesto;
                                p.Fecha_Aprobado = fechaAprobado;
                                p.Fecha_act = DateTime.Now;
                                byte Id_MesPol = 0;
                                int Id_FolioPol = 0;
                                if (p.Total > 0)
                                {
                                    new ProceduresDAL().Pa_Genera_PolizaOrden_Aprobado(p.Id_ObjetoG, p.Total, p.Fecha_Aprobado.Value, ref Id_MesPol, ref Id_FolioPol, 1);
                                    p.Id_MesPO_Aprobado = Id_MesPol;
                                    p.Id_FolioPO_Aprobado = Id_FolioPol;
                                }
                                presupuestoDal.Insert(p);
                                presupuestoDal.Save();
                                p = presupuestoDal.GetByID(x => x.Id_ClavePresupuesto == ClavePresupuesto);
                                return Json(new
                                {
                                    Exito = true,
                                    Mensaje = "Presupuesto agregado correctamente",
                                    Poliza = Id_MesPol == 0 ? "" : StringID.Polizas(Id_FolioPol, Id_MesPol),
                                    ClavePresupuestal = p.Id_ClavePresupuesto,
                                    Id_FolioPO_Aprobado = p.Id_FolioPO_Aprobado,
                                    Id_MesPO_Aprobado = p.Id_MesPO_Aprobado
                                });
                            }
                            catch (Exception ex)
                            {
                                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
                            }

                        }
                        else
                        {
                            return Json(new { Exito = false, Mensaje = "El mes ya está cerrado" });
                        }
                    }
                    else
                    {
                        return Json(new { Exito = false, Mensaje = "No se puede agregar el presupuesto porque el año de la fecha es mayor al ejercicio" });
                    }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        public ActionResult ModalPresupuesto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BuscarPresupuestos(ModalPresupuestoModel presupuesto)
        {
            List<MA_PresupuestoEg> lista = new List<MA_PresupuestoEg>();
            lista = BusquedaPresupuestos.BuscarPresupuesto(presupuesto);
            return View(lista);
        }

        public ActionResult GetPresupuesto(String clavePresupuesto)
        {
            try
            {
                return Json(new { Exito = true, Presupuesto = new Llenado().LLenado_MaPresupuestoEG(clavePresupuesto.Trim()), ClavePresupuestal = clavePresupuesto });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult CancelarPresupuesto(string clavePresupuesto, string fecha)
        {
            try
            {
                DateTime f = Convert.ToDateTime(fecha);
                Ca_CierreMensual cierre = cierreMensualDAL.GetByID(x => x.Id_Mes == f.Month);
                if (!cierre.Contable.Value)
                {
                    MaPresupuestoEgDAL pDal = new MaPresupuestoEgDAL();
                    MA_PresupuestoEg presupuesto = pDal.GetByID(x => x.Id_ClavePresupuesto == clavePresupuesto.Trim());
                    presupuesto.Fecha_CancelaAprobado = f;
                    pDal.Update(presupuesto);
                    pDal.Save();
                    pDal.Delete(x => x.Id_ClavePresupuesto == clavePresupuesto);
                    pDal.Save();
                    return Json(new { Exito = true, Mensaje = "Presupuesto eliminado correctamente" });
                }
                return Json(new { Exito = true, Mensaje = "El Presupuestos de Egreso no puede ser eliminado debido a que el mes se encuentra cerrado" });
            }
            catch (Exception)
            {
                return Json(new { Exito = false, Mensaje = "El Presupuesto de Egresos no puede ser eliminado debido a que se encuentra en uso" });//new Errores(ex.HResult, ex.Message).Mensaje 
            }
        }

        public ActionResult ModalEliminar()
        {
            return View();
        }



        public JsonResult ValidarProceso(String Id_Proceso, String Id_TipoMeta)
        {
            MaPresupuestoEgDAL pDal = new MaPresupuestoEgDAL();
            List<MA_PresupuestoEg> presupuesto = pDal.Get(x => x.Id_Proceso == Id_Proceso && x.Id_TipoMeta == Id_TipoMeta).ToList();
            if (presupuesto != null && presupuesto.Count > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarActividadMIR(String Id_ActividadMIR, String Id_Proceso)
        {
            if (new ActividadMIRDAL().GetByID(x => x.Id_ActividadMIR2 == Id_ActividadMIR && x.Id_Proceso == Id_Proceso) != null)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarTotal(decimal Total)
        {
            if (Total > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarAccion(String Id_Accion, String Id_ActividadMIR, String Id_Proceso)
        {
            if (new AccionDAL().GetByID(x => x.Id_Accion2 == Id_Accion && x.Id_ActividadMIR2 == Id_ActividadMIR && x.Id_Proceso == Id_Proceso) != null)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DisponibilidadEgresos()
        {
            GetTipoMeta();
            return View(new De_DisponibilidadModel());
        }

        public ActionResult BuscarDisponibilidad(De_DisponibilidadModel presupuesto)
        {
            return View(Disponibildad.ObtenerDisponibilidad(presupuesto));
        }

        #region AmpliacionesyReducciones
        public ActionResult V_AmpliacionesReducciones()
        {
            ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, AmpliacionesReduccionesBL.GetUltimoMesCerrado() + 1, 1).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            ViewBag.post = false;
            return View(new Ma_TransferenciasModel());
        }
        [HttpPost]
        public ActionResult V_AmpliacionesReducciones(Int32 Id_Transferencia)
        {
            ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, AmpliacionesReduccionesBL.GetUltimoMesCerrado() + 1, 1).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            ViewBag.post = true;
            Ma_TransferenciasModel maModel = llenar.LLenado_MaTransferencias(Id_Transferencia);
            if (maModel.Importe_AMP != null)
                maModel.Importe_AMP_f = maModel.Importe_AMP.Value.ToString();//.ToString("N");
            else
                maModel.Importe_AMP_f = "0";// String.Format("{0:N}", "0");
            if (maModel.Importe_RED != null)
                maModel.Importe_RED_f = maModel.Importe_RED.Value.ToString();//.ToString("N");
            else
                maModel.Importe_RED_f = "0";//String.Format("{0:N}", "0");
            maModel.PolizaOrdenModificado = StringID.Polizas(maModel.Id_FolioPO_Modificado, maModel.Id_MesPO_Modificado);
            maModel.PolizaOrdenCancelado = StringID.Polizas(maModel.Id_FolioPO_Modificado_Cancela, maModel.Id_MesPO_Modificado_Cancela);
            return View(maModel);
        }
        [HttpPost]
        public JsonResult ValidarExistencia()
        {
            string seleccion = "";
            if (AmpliacionesReduccionesBL.ValidarSinAfectar() > 0)
            {
                Ma_Transferencias model = MaTranferenciasDAL.GetFisrt(x => x.Id_Estatus == 1 && x.Id_PptoModificado == true);
                if (model.Id_TipoT == 1)
                    seleccion = " Ampliación";
                else
                    seleccion = "Reducción";
                return Json(new { Exito = false, Mensaje = "No se puede agregar porque la " + seleccion + " No." + model.Id_Transferencia + " no está afectada" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Exito = true, Mensaje = "Todo bien" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult V_BuscarAmpliacionReduccion(byte idPpto)
        {
            ViewBag.idPpto = idPpto;
            return View();
        }

        public ActionResult V_TablaBusquedaAmpliacionReduccion(Int32? folioBusqueda, string DescripciónBusqueda, byte idPpto)
        {
            //idPpto 1=ampliacionReduccion; 2=transferencias,3 arrastre de saldos
            //IEnumerable<Ma_Transferencias> entities = MaTranferenciasDAL.Get(x => folioBusqueda > 0 ? x.Id_Transferencia == folioBusqueda : x.Id_Transferencia != null && !String.IsNullOrEmpty(Desc) ? x.Descrip == Desc : x.Descrip != null);
            List<Ma_TransferenciasModel> Lst = new List<Ma_TransferenciasModel>();
            List<Ma_Transferencias> lista = new List<Ma_Transferencias>();
            if (idPpto == 1)
            {
                lista = MaTranferenciasDAL.Get(x => folioBusqueda > 0 ? x.Id_Transferencia == folioBusqueda : x.Id_Transferencia != null && !String.IsNullOrEmpty(DescripciónBusqueda) ? x.Descrip.Contains(DescripciónBusqueda) : x.Descrip != null && x.Id_PptoModificado == true).ToList();
                foreach (Ma_Transferencias item in lista)
                {
                    if (item.Id_PptoModificado == true)
                    {
                        Lst.Add((ModelFactory.getModel<Ma_TransferenciasModel>(item, new Ma_TransferenciasModel())));
                    }
                }
            }
            if (idPpto == 2)
            {
                lista = MaTranferenciasDAL.Get(x => folioBusqueda > 0 ? x.Id_Transferencia == folioBusqueda : x.Id_Transferencia != null && !String.IsNullOrEmpty(DescripciónBusqueda) ? x.Descrip.Contains(DescripciónBusqueda) : x.Descrip != null && x.Id_PptoModificado == false).ToList();
                foreach (Ma_Transferencias item in lista)
                {
                    if (item.Id_PptoModificado == false)
                    {
                        Lst.Add((ModelFactory.getModel<Ma_TransferenciasModel>(item, new Ma_TransferenciasModel())));
                    }
                }
            }
            if (idPpto == 3)
            {
                lista = MaTranferenciasDAL.Get(x => folioBusqueda > 0 ? x.Id_Transferencia == folioBusqueda : x.Id_Transferencia != null && !String.IsNullOrEmpty(DescripciónBusqueda) ? x.Descrip.Contains(DescripciónBusqueda) : x.Descrip != null && x.Id_PptoModificado == null).ToList();
                foreach (Ma_Transferencias item in lista)
                {
                    if (item.Id_PptoModificado == null)
                    {
                        Lst.Add((ModelFactory.getModel<Ma_TransferenciasModel>(item, new Ma_TransferenciasModel())));
                    }
                }
            }
            return View(Lst);
        }
        [HttpPost]
        public JsonResult SeleccionarTransferencia(Int32 IdTransferencia)
        {
            try
            {
                Ma_Transferencias model = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == IdTransferencia);
                if (model != null)
                {
                    int afecta = 0;
                    if (DeTransferenciasDAL.Get(x => x.Id_Transferencia == IdTransferencia).Count() > 0 && model.Id_Estatus == 1)
                        afecta = 1;
                    else
                        afecta = 0;
                    Ma_TransferenciasModel datamodel = ModelFactory.getModel<Ma_TransferenciasModel>(model, new Ma_TransferenciasModel());
                    Ma_TransferenciasModel maModel = llenar.LLenado_MaTransferencias(IdTransferencia);
                    maModel.PolizaOrdenModificado = StringID.Polizas(datamodel.Id_FolioPO_Modificado, datamodel.Id_MesPO_Modificado);
                    maModel.PolizaOrdenCancelado = StringID.Polizas(datamodel.Id_FolioPO_Modificado_Cancela, datamodel.Id_MesPO_Modificado_Cancela);
                    if (maModel.Importe_AMP != null)
                        maModel.Importe_AMP_f = maModel.Importe_AMP.Value.ToString();//.ToString("N");
                    else
                        maModel.Importe_AMP_f = "0";// String.Format("{0:N}", 0);
                    if (maModel.Importe_RED != null)
                        maModel.Importe_RED_f = maModel.Importe_RED.Value.ToString();//.ToString("N");
                    else
                        maModel.Importe_RED_f = "0";//String.Format("{0:N}", 0);
                    return Json(new { Exito = true, Mensaje = "OK", Registro = maModel, Afectar = afecta });
                }

                return Json(new { Exito = false, Mensaje = "Registro no encontrado." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }

        }
        [HttpPost]
        public JsonResult GuardarAmpRed(Ma_Transferencias modelo)
        {
            try
            {
                Ca_CierreMensual cierre = cierreMensualDAL.GetByID(x => x.Id_Mes == modelo.Fecha.Value.Month);
                if (!cierre.Contable.Value)
                {
                    if (modelo.Id_Transferencia == 0)
                    {
                        UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                        modelo.Usu_Act = (short)appUsuario.IdUsuario;
                        modelo.Fecha_Act = DateTime.Now;
                        //if (modelo.Id_TipoT == 1)
                        //    modelo.Importe_AMP = 0;
                        //else
                        //    modelo.Importe_RED = 0;
                        modelo.Id_PptoModificado = true;
                        if (MaTranferenciasDAL.Get().Count() == 0)
                            modelo.Id_Transferencia = 1;
                        else
                            modelo.Id_Transferencia = MaTranferenciasDAL.Get().Max(x => x.Id_Transferencia) + 1;
                        modelo.Id_Estatus = 1;
                        MaTranferenciasDAL.Insert(modelo);
                        MaTranferenciasDAL.Save();
                        return Json(new { Exito = true, Mensaje = "Registro guardado correctamente.", Registro = modelo });
                    }
                    else
                    {
                        Ma_Transferencias modeloupdate = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == modelo.Id_Transferencia);
                        modeloupdate.Descrip = modelo.Descrip;
                        modeloupdate.Fecha = modelo.Fecha;
                        MaTranferenciasDAL.Update(modeloupdate);
                        MaTranferenciasDAL.Save();
                        return Json(new { Exito = true, Mensaje = "Registro guardado correctamente.", Registro = modeloupdate });
                    }

                }
                return Json(new { Exito = false, Mensaje = "El mes ya esta cerrado." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }

        }
        [HttpPost]
        public JsonResult EliminarTransferencia(Int32 IdTransferencia, DateTime FechaCancelacion)
        {
            try
            {
                Ma_Transferencias model = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == IdTransferencia);
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                if (model.Id_Estatus == 1)
                {
                    model.Id_Estatus = 3;
                    MaTranferenciasDAL.Update(model);
                    MaTranferenciasDAL.Save();
                }
                else
                {

                    model.Id_Estatus = 3;
                    byte idMes = 0;
                    int idFolio = 0;
                    if (model.Id_TipoT == 1)
                        proceduresDAL.Pa_Genera_PolizaOrden_Modificado_Amp_Cancela(IdTransferencia, FechaCancelacion, (short)appUsuario.IdUsuario, ref idMes, ref idFolio);
                    else
                        proceduresDAL.Pa_Genera_PolizaOrden_Modificado_Red_Cancela(IdTransferencia, FechaCancelacion, (short)appUsuario.IdUsuario, ref idMes, ref idFolio);
                    model.Id_MesPO_Modificado_Cancela = idMes;
                    model.Id_FolioPO_Modificado_Cancela = idFolio;
                    MaTranferenciasDAL.Update(model);
                    MaTranferenciasDAL.Save();
                    Ma_TransferenciasModel datamodel = ModelFactory.getModel<Ma_TransferenciasModel>(model, new Ma_TransferenciasModel());
                    datamodel.PolizaOrdenModificado = StringID.Polizas(datamodel.Id_FolioPO_Modificado, datamodel.Id_MesPO_Modificado);
                    datamodel.PolizaOrdenCancelado = StringID.Polizas(datamodel.Id_FolioPO_Modificado_Cancela, datamodel.Id_MesPO_Modificado_Cancela);
                    return Json(new { Exito = true, Mensaje = "Registro eliminado correctamente.", Registro = datamodel });

                }
                return Json(new { Exito = true, Mensaje = "Registro no encontrado." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }

        }
        [HttpPost]
        public JsonResult ValidarEliminar(Int32 IdTransferencia)
        {
            try
            {
                Ma_Transferencias model = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == IdTransferencia);
                if (model.Id_Estatus == 1)
                {

                }
                else
                {
                    if (!proceduresDAL.PA_VerificaCierreTransferencia(IdTransferencia))
                    {
                        if (model.Id_Estatus == 2)//SI YA FUE AFECTADA
                        {
                            if (MaPolizasDAL.Get(x => x.Id_FolioPoliza == model.Id_FolioPO_Modificado && x.Id_MesPoliza == model.Id_MesPO_Modificado && x.Id_TipoPoliza == 4).Count() > 0)//VALIDAR SI SE GENERARON POLIZAS MODIFICACION
                                return Json(new { Exito = true, Mensaje = "No hubo error." });
                            else
                                return Json(new { Exito = false, Mensaje = "No se generaron polizas de modificación." });
                        }
                        return Json(new { Exito = true, Mensaje = "No hubo error." });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = "No se puede eliminar, hay detalles con meses cerrados." });
                }
                return Json(new { Exito = true, Mensaje = "Registro no encontrado." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }

        }
        public ActionResult V_ModalEliminar(Int32 Id_Transferencia)
        {
            Ma_Transferencias model = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == Id_Transferencia);
            DateTime fecha = new DateTime();
            if (model.Id_Estatus == 1)
                fecha = model.Fecha.Value;
            else
                fecha = model.Fecha_Afecta.Value;
            ViewBag.FechaInicio = fecha.ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            ViewBag.id = model.Id_Transferencia;
            return View();
        }
        [HttpPost]
        public JsonResult ValidarAfectacion(Int32 IdTransferencia)
        {
            try
            {
                if (DeTransferenciasDAL.Get(x => x.Id_Transferencia == IdTransferencia).Count() > 0)
                {
                    if (!proceduresDAL.PA_VerificaCierreTransferencia(IdTransferencia))
                    {
                        return Json(new { Exito = true, Mensaje = "No hubo error." });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = "No se puede afectar, hay detalles con meses cerrados." });
                }
                else
                    return Json(new { Exito = false, Mensaje = "No se puede afectar porque no tiene movimientos." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }

        }
        public ActionResult V_ModalAfectacion(Int32 IdTransferencia, byte IdTipo)
        {
            Ma_Transferencias model = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == IdTransferencia);
            if (AmpliacionesReduccionesBL.ObtenerTipoMaTransferencia(IdTransferencia) == 1)
                ViewBag.Tipo = "Ampliación";
            else
                ViewBag.Tipo = "Reducción";
            ViewBag.Trans = IdTipo;
            ViewBag.Importe = DeTransferenciasDAL.Get(x => x.Id_Transferencia == IdTransferencia && x.Id_Movimiento == 1).Sum(x => x.Importe).Value.ToString("N");
            ViewBag.FechaInicio = AmpliacionesReduccionesBL.GetFechaMayor(model.Fecha.Value).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            ViewBag.total = DeTransferenciasDAL.Get(x => x.Id_Transferencia == IdTransferencia).Sum(x => x.Importe);
            ViewBag.id = model.Id_Transferencia;
            return View();
        }
        public ActionResult V_DetalleDeTransferencia(Int32 IdTransferencia, byte origen)
        {
            ViewBag.origen = origen;
            Ma_TransferenciasModel dataModal = llenar.LLenado_MaTransferencias(IdTransferencia);
            return View(dataModal);
        }
        [HttpPost]
        public JsonResult GetMesesCerrados()
        {
            IEnumerable<Ca_CierreMensual> meses = cierreMensualDAL.Get(x => x.Contable == true);
            return Json(new { Exito = true, Meses = meses });
        }
        [HttpPost]
        public JsonResult Afectar(Int32 IdTransferencia, DateTime FechaAfectacion)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                Ca_CierreMensual cierre = cierreMensualDAL.GetByID(x => x.Id_Mes == FechaAfectacion.Month);
                if (!cierre.Contable.Value)
                {
                    Ma_Transferencias model = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == IdTransferencia);
                    byte idMes = 0;
                    int idFolio = 0;
                    if (model.Id_TipoT == 1)
                        proceduresDAL.Pa_Genera_PolizaOrden_Modificado_Amp(IdTransferencia, FechaAfectacion, (short)appUsuario.IdUsuario, ref idMes, ref idFolio);
                    else
                        proceduresDAL.Pa_Genera_PolizaOrden_Modificado_Red(IdTransferencia, FechaAfectacion, (short)appUsuario.IdUsuario, ref idMes, ref idFolio);
                    model.Fecha_Afecta = FechaAfectacion;
                    model.Id_Estatus = 2;
                    model.Id_MesPO_Modificado = idMes;
                    model.Id_FolioPO_Modificado = idFolio;
                    MaTranferenciasDAL.Update(model);
                    MaTranferenciasDAL.Save();
                    Ma_TransferenciasModel datamodel = ModelFactory.getModel<Ma_TransferenciasModel>(model, new Ma_TransferenciasModel());
                    datamodel.PolizaOrdenModificado = StringID.Polizas(datamodel.Id_FolioPO_Modificado, datamodel.Id_MesPO_Modificado);
                    datamodel.PolizaOrdenCancelado = StringID.Polizas(datamodel.Id_FolioPO_Modificado_Cancela, datamodel.Id_MesPO_Modificado_Cancela);
                    return Json(new { Exito = true, Mensaje = "Afectado correctamente.", Registro = datamodel });

                }
                return Json(new { Exito = false, Mensaje = "No se puede afectar porque el mes esta cerrado." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        public ActionResult V_TablaDetallesTransferencia(Int32 Id_Transferencia)
        {
            ViewBag.Estatus = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == Id_Transferencia).Id_Estatus;
            ViewBag.ppto = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == Id_Transferencia).Id_PptoModificado;
            List<De_TransferenciaModel> Lst = new List<De_TransferenciaModel>();
            DeTransferenciasDAL.Get(x => x.Id_Transferencia == Id_Transferencia).ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<De_TransferenciaModel>(x, new De_TransferenciaModel())); });
            return View(Lst);
        }
        [HttpPost]
        public ActionResult GuardarDeTransferencia(De_TransferenciaModel dataModel, De_Transferencia dataTransferencia, De_ClavePresupuestal clave)
        {
            try
            {
                if (dataModel.Id_Accion != null)
                    dataTransferencia.Id_ClavePresupuesto = StringID.IdClavePresupuesto(dataModel.Id_Area, dataModel.Id_Funcion, dataModel.Id_Actividad, dataModel.Id_ClasificacionP, dataModel.Id_Programa, dataModel.Id_Proceso, dataModel.Id_TipoMeta, dataModel.Id_ActividadMIR, dataModel.Id_Accion, dataModel.Id_Alcance, dataModel.Id_TipoG, dataModel.Id_Fuente, dataModel.AnioFin, dataModel.Id_ObjetoG);
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                dataTransferencia.Usu_Act = (short)appUsuario.IdUsuario;
                dataTransferencia.Fecha_Act = DateTime.Now;
                if (dataTransferencia.Id_Consecutivo == 0)
                {
                    if (DeTransferenciasDAL.Get(x => x.Id_Transferencia == dataTransferencia.Id_Transferencia).Count() > 0)
                        dataTransferencia.Id_Consecutivo = Convert.ToInt16(DeTransferenciasDAL.Get(x => x.Id_Transferencia == dataTransferencia.Id_Transferencia).Max(x => x.Id_Consecutivo) + 1);
                    else
                        dataTransferencia.Id_Consecutivo = 1;
                    DeTransferenciasDAL.Insert(dataTransferencia);
                    DeTransferenciasDAL.Save();

                    Ma_Transferencias trans = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == dataTransferencia.Id_Transferencia);
                    if (dataTransferencia.Id_Movimiento == 1)
                        trans.Importe_AMP += dataTransferencia.Importe;
                    else
                        trans.Importe_RED += dataTransferencia.Importe;
                    MaTranferenciasDAL.Update(trans);
                    MaTranferenciasDAL.Save();
                }
                else
                {
                    DeTransferenciasDAL.Update(dataTransferencia);
                    DeTransferenciasDAL.Save();
                }
                return Json(new { Exito = true, Mensaje = "Guardado correctamente.", Registro = dataTransferencia });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        [HttpPost]
        public JsonResult getDetalleTransferencia(Int32 Id_Transferencia, Int32 Id_Consecutivo)
        {
            try
            {
                De_TransferenciaModel dataModal = llenar.LLenado_DeTransferencias(Id_Transferencia, Id_Consecutivo);
                return Json(new { Exito = true, Mensaje = "OK", Registro = dataModal });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje, Registro = new De_PolizasModel() });
            }
        }
        [HttpPost]
        public JsonResult EliminarDeTransferencia(Int32 Id_Transferencia, Int32 Id_Consecutivo)
        {
            try
            {
                DeTransferenciasDAL.Delete(x => x.Id_Transferencia == Id_Transferencia && x.Id_Consecutivo == Id_Consecutivo);
                DeTransferenciasDAL.Save();
                return Json(new { Exito = true, Mensaje = "Eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje, Registro = new De_PolizasModel() });
            }
        }
        #endregion

        #region Transferencias
        public ActionResult V_Transferencias()
        {
            ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, AmpliacionesReduccionesBL.GetUltimoMesCerrado() + 1, 1).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            ViewBag.post = false;
            return View(new Ma_TransferenciasModel());
        }
        [HttpPost]
        public ActionResult V_Transferencias(Int32 Id_Transferencia)
        {
            ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, AmpliacionesReduccionesBL.GetUltimoMesCerrado() + 1, 1).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            ViewBag.post = true;
            return View(llenar.LLenado_MaTransferencias(Id_Transferencia));
        }
        [HttpPost]
        public JsonResult GuardarTransferencia(Ma_Transferencias modelo)
        {
            try
            {
                Ca_CierreMensual cierre = cierreMensualDAL.GetByID(x => x.Id_Mes == modelo.Fecha.Value.Month);
                if (!cierre.Contable.Value)
                {
                    if (modelo.Id_Transferencia == 0)
                    {
                        UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                        modelo.Usu_Act = (short)appUsuario.IdUsuario;
                        modelo.Fecha_Act = DateTime.Now;
                        modelo.Id_PptoModificado = false;
                        if (MaTranferenciasDAL.Get().Count() == 0)
                            modelo.Id_Transferencia = 1;
                        else
                            modelo.Id_Transferencia = MaTranferenciasDAL.Get().Max(x => x.Id_Transferencia) + 1;
                        modelo.Id_Estatus = 1;
                        MaTranferenciasDAL.Insert(modelo);
                        MaTranferenciasDAL.Save();
                        return Json(new { Exito = true, Mensaje = "Registro guardado correctamente.", Registro = modelo });
                    }
                    else
                    {
                        Ma_Transferencias modeloupdate = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == modelo.Id_Transferencia);
                        modeloupdate.Descrip = modelo.Descrip;
                        modeloupdate.Fecha = modelo.Fecha;
                        modelo.Id_Mes = modelo.Id_Mes;

                        //valida si hubo cambio en las claves 
                        if (modeloupdate.Id_Area != modelo.Id_Area || modeloupdate.Id_OGFinal != modelo.Id_OGFinal || modeloupdate.Id_OGInicial != modelo.Id_OGInicial || modeloupdate.Id_TipoT != modelo.Id_TipoT)
                        {
                            if (DeTransferenciasDAL.Get(x => x.Id_Transferencia == modelo.Id_Transferencia).Count() > 0)
                            {
                                List<De_Transferencia> lista = DeTransferenciasDAL.Get(x => x.Id_Transferencia == modelo.Id_Transferencia).ToList();
                                DeTransferenciasDAL.DeleteAll(lista);
                                DeTransferenciasDAL.Save();
                            }
                        }
                        if (modeloupdate.Id_TipoT != modelo.Id_TipoT && modeloupdate.Id_TipoT == 2)
                            modeloupdate.Id_Area = null;
                        modeloupdate.Id_Mes = modelo.Id_Mes;
                        modeloupdate.Id_OGFinal = modelo.Id_OGFinal;
                        modeloupdate.Id_OGInicial = modelo.Id_OGInicial;
                        modeloupdate.Id_TipoT = modelo.Id_TipoT;
                        MaTranferenciasDAL.Update(modeloupdate);
                        MaTranferenciasDAL.Save();
                        bool afecta = false;
                        if (DeTransferenciasDAL.Get(x => x.Id_Transferencia == modeloupdate.Id_Transferencia).Count() > 0)
                            afecta = true;
                        return Json(new { Exito = true, Mensaje = "Registro guardado correctamente.", Registro = modeloupdate, Afecta = afecta });
                    }

                }
                return Json(new { Exito = false, Mensaje = "El mes ya esta cerrado." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }

        }
        [HttpPost]
        public JsonResult EliminarTransferencia2(Int32 IdTransferencia)
        {
            try
            {
                Ma_Transferencias model = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == IdTransferencia);
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                model.Id_Estatus = 3;
                if (DeTransferenciasDAL.Get(x => x.Id_Transferencia == model.Id_Transferencia).Count() > 0)
                {
                    List<De_Transferencia> lista = DeTransferenciasDAL.Get(x => x.Id_Transferencia == model.Id_Transferencia).ToList();
                    DeTransferenciasDAL.DeleteAll(lista);
                    DeTransferenciasDAL.Save();
                }
                MaTranferenciasDAL.Update(model);
                MaTranferenciasDAL.Save();
                Ma_TransferenciasModel datamodel = ModelFactory.getModel<Ma_TransferenciasModel>(model, new Ma_TransferenciasModel());
                datamodel.PolizaOrdenModificado = StringID.Polizas(datamodel.Id_FolioPO_Modificado, datamodel.Id_MesPO_Modificado);
                datamodel.PolizaOrdenCancelado = StringID.Polizas(datamodel.Id_FolioPO_Modificado_Cancela, datamodel.Id_MesPO_Modificado_Cancela);
                return Json(new { Exito = true, Mensaje = "Registro eliminado correctamente.", Registro = datamodel });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        [HttpPost]
        public JsonResult ValidarEliminarTransferencia(Int32 IdTransferencia)
        {
            try
            {
                Ma_Transferencias model = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == IdTransferencia);
                if (model.Id_Estatus == 1)
                {

                }
                else
                {
                    if (!proceduresDAL.PA_VerificaCierreTransferencia(IdTransferencia))
                    {
                        return Json(new { Exito = true, Mensaje = "No hubo error." });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = "No se puede eliminar, hay detalles con meses cerrados." });
                }
                return Json(new { Exito = true, Mensaje = "Registro no encontrado." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }

        }
        public ActionResult V_DeTransferencia(Int32 IdTransferencia)
        {
            Ma_TransferenciasModel dataModal = llenar.LLenado_MaTransferencias(IdTransferencia);
            return View(dataModal);
        }
        [HttpPost]
        public JsonResult GetDisponibilidadTransferencia(De_TransferenciaModel dataModel)
        {
            try
            {
                string Clave = StringID.IdClavePresupuesto(dataModel.Id_Area, dataModel.Id_Funcion, dataModel.Id_Actividad, dataModel.Id_ClasificacionP, dataModel.Id_Programa, dataModel.Id_Proceso, dataModel.Id_TipoMeta, dataModel.Id_ActividadMIR, dataModel.Id_Accion, dataModel.Id_Alcance, dataModel.Id_TipoG, dataModel.Id_Fuente, dataModel.AnioFin, dataModel.Id_ObjetoG);
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                Ma_Transferencias trans = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == dataModel.Id_Transferencia);
                List<DE_Disponibilidad> meses = new List<DE_Disponibilidad>();
                List<De_DisponibilidadModel> mesesModel = new List<De_DisponibilidadModel>();
                decimal total = 0;
                if (trans.Id_TipoT == 1)
                    meses = DeDisponibilidadDAL.Get(x => x.Id_ClavePresupuesto == Clave).ToList();
                else
                    meses = DeDisponibilidadDAL.Get(x => x.Id_ClavePresupuesto == Clave && x.Mes == trans.Id_Mes).ToList();
                foreach (DE_Disponibilidad item in meses)
                {
                    De_DisponibilidadModel temp = ModelFactory.getModel<De_DisponibilidadModel>(item, new De_DisponibilidadModel());
                    temp.importeFormato = temp.Disponible.Value.ToString("N");
                    total += item.Disponible.Value;
                    mesesModel.Add(temp);
                }
                return Json(new { Exito = true, Mensaje = "Guardado correctamente.", Meses = mesesModel, total = "$" + total.ToString("N") });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        [HttpPost]
        public JsonResult GenerarDetalles(Int32 Id_Transferencia)
        {
            try
            {
                Ma_Transferencias model = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == Id_Transferencia);
                if (model.Id_TipoT == 2)
                {
                    UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                    decimal[] result = proceduresDAL.PA_Genera_Detalle_Transferencia_Mensual(model.Id_Transferencia, model.Id_Mes, model.Id_Area, Convert.ToInt32(model.Id_OGInicial), Convert.ToInt32(model.Id_OGFinal), (short)appUsuario.IdUsuario);
                    if (result[0] == 0)
                        return Json(new { Exito = false, Mensaje = "No se generaron detalles a esta transferencia porque no hay saldos negativos a cubrir." });
                    if (result[1] == 0)
                        return Json(new { Exito = false, Mensaje = "No se generaron detalles a esta transferencia porque no hay disponibilidad suficiente." });
                    if (DeTransferenciasDAL.Get(x => x.Id_Transferencia == Id_Transferencia).Count() > 0)
                        return Json(new { Exito = true, Mensaje = "La propuesta de la transferencia se realizo correctamente. Revisar los detalles. Importe a cubrir:" + result[0] + " Importe requerido: " + result[1] });
                    else
                        return Json(new { Exito = false, Mensaje = "No se encontraron partidas disponibles para generar el Detalles." });
                }
                return Json(new { Exito = true, Mensaje = "Guardado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        [HttpPost]
        public JsonResult GetMesesCerradosTransferencia(Int32 Id_Transferencia)
        {
            Ma_Transferencias model = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == Id_Transferencia);
            List<Ca_CierreMensual> meses = new List<Ca_CierreMensual>();
            if (model.Id_TipoT == 2)
            {
                meses = cierreMensualDAL.Get(x => x.Contable == false && x.Id_Mes == model.Id_Mes).ToList();
            }
            else
            {
                meses = cierreMensualDAL.Get(x => x.Contable == false).ToList();
            }
            return Json(new { Exito = true, Meses = meses });
        }
        [HttpPost]
        public JsonResult ValidarExistenciaTransferencia()
        {
            if (AmpliacionesReduccionesBL.ValidarSinTransferencia() > 0)
            {
                Ma_Transferencias model = MaTranferenciasDAL.GetFisrt(x => x.Id_Estatus == 1 && x.Id_PptoModificado == false);
                return Json(new { Exito = false, Mensaje = "No se puede agregar porque la transferencia No. " + model.Id_Transferencia + " no está afectada" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Exito = true, Mensaje = "Todo bien" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ValidarAfectacionTransferencia(Int32 IdTransferencia)
        {
            try
            {
                if (DeTransferenciasDAL.Get(x => x.Id_Transferencia == IdTransferencia).Count() > 0)
                {
                    if (!proceduresDAL.PA_VerificaCierreTransferencia(IdTransferencia))
                    {
                        if (DeTransferenciasDAL.Get(x => x.Id_Transferencia == IdTransferencia && x.Id_Movimiento == 1).Sum(x => x.Importe) == DeTransferenciasDAL.Get(x => x.Id_Transferencia == IdTransferencia && x.Id_Movimiento == 2).Sum(x => x.Importe))
                            return Json(new { Exito = true, Mensaje = "No hubo error." });
                        else
                            return Json(new { Exito = false, Mensaje = "No se puede afectar porque los importes no estan cuadrados." });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = "No se puede afectar, hay detalles con meses cerrados." });
                }
                else
                    return Json(new { Exito = false, Mensaje = "No se puede afectar porque no tiene movimientos." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }

        }
        [HttpPost]
        public JsonResult AfectarTransferencia(Int32 IdTransferencia, DateTime FechaAfectacion)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                Ca_CierreMensual cierre = cierreMensualDAL.GetByID(x => x.Id_Mes == FechaAfectacion.Month);
                if (!cierre.Contable.Value)
                {
                    Ma_Transferencias model = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == IdTransferencia);
                    if (model.Fecha < FechaAfectacion)
                        return Json(new { Exito = false, Mensaje = "No se puede afectar porque la fecha de Afectación es menor a la fecha de Registro." });
                    model.Fecha_Afecta = FechaAfectacion;
                    model.Id_Estatus = 2;
                    MaTranferenciasDAL.Update(model);
                    MaTranferenciasDAL.Save();
                    return Json(new { Exito = true, Mensaje = "Afectado correctamente.", Registro = model });

                }
                return Json(new { Exito = false, Mensaje = "No se puede afectar porque el mes esta cerrado." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        #endregion

        #region ArrastreSaldos
        public ActionResult V_ArrasatreSaldos()
        {
            ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, AmpliacionesReduccionesBL.GetUltimoMesCerrado() + 1, 1).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            Ma_TransferenciasModel model = new Ma_TransferenciasModel();
            model.Id_TipoT = 2;
            ViewBag.post = false;
            return View(model);
        }
        [HttpPost]
        public ActionResult V_ArrasatreSaldos(Int32 Id_Transferencia)
        {
            ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, AmpliacionesReduccionesBL.GetUltimoMesCerrado() + 1, 1).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            ViewBag.post = true;
            return View(llenar.LLenado_MaTransferencias(Id_Transferencia));
        }
        [HttpPost]
        public JsonResult ValidarExistenciaArrastre()
        {
            if (AmpliacionesReduccionesBL.ValidarSinAfectarArrastre() > 0)
            {
                Ma_Transferencias model = MaTranferenciasDAL.GetFisrt(x => x.Id_Estatus == 1 && x.Id_PptoModificado == null);
                return Json(new { Exito = false, Mensaje = "No se puede agregar porque el arrastre No." + model.Id_Transferencia + " no está afectada" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Exito = true, Mensaje = "Todo bien" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarArrastre(Ma_Transferencias modelo)
        {
            try
            {
                Ca_CierreMensual cierre = cierreMensualDAL.GetByID(x => x.Id_Mes == modelo.Fecha.Value.Month);
                if (!cierre.Contable.Value)
                {
                    if (modelo.Id_Transferencia == 0)
                    {
                        UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                        modelo.Usu_Act = (short)appUsuario.IdUsuario;
                        modelo.Fecha_Act = DateTime.Now;
                        modelo.Id_PptoModificado = null;
                        if (MaTranferenciasDAL.Get().Count() == 0)
                            modelo.Id_Transferencia = 1;
                        else
                            modelo.Id_Transferencia = MaTranferenciasDAL.Get().Max(x => x.Id_Transferencia) + 1;
                        modelo.Id_Estatus = 1;
                        MaTranferenciasDAL.Insert(modelo);
                        MaTranferenciasDAL.Save();
                        return Json(new { Exito = true, Mensaje = "Registro guardado correctamente.", Registro = modelo });
                    }
                    else
                    {
                        Ma_Transferencias modeloupdate = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == modelo.Id_Transferencia);
                        modeloupdate.Descrip = modelo.Descrip;
                        modeloupdate.Fecha = modelo.Fecha;
                        //valida si hubo cambio en las claves 
                        if (modeloupdate.Id_Area != modelo.Id_Area || modeloupdate.Id_OGFinal != modelo.Id_OGFinal || modeloupdate.Id_OGInicial != modelo.Id_OGInicial || modeloupdate.Id_Mes != modelo.Id_Mes || modeloupdate.Id_Mes_Origen != modelo.Id_Mes_Origen)
                        {
                            if (DeTransferenciasDAL.Get(x => x.Id_Transferencia == modelo.Id_Transferencia).Count() > 0)
                            {
                                List<De_Transferencia> lista = DeTransferenciasDAL.Get(x => x.Id_Transferencia == modelo.Id_Transferencia).ToList();
                                DeTransferenciasDAL.DeleteAll(lista);
                                DeTransferenciasDAL.Save();
                            }
                        }
                        modeloupdate.Id_OGFinal = modelo.Id_OGFinal;
                        modeloupdate.Id_OGInicial = modelo.Id_OGInicial;
                        modeloupdate.Id_Area = modelo.Id_Area;
                        modeloupdate.Id_Mes = modelo.Id_Mes;
                        modeloupdate.Id_Mes_Origen = modelo.Id_Mes_Origen;
                        MaTranferenciasDAL.Update(modeloupdate);
                        MaTranferenciasDAL.Save();
                        bool afecta = false;
                        if (DeTransferenciasDAL.Get(x => x.Id_Transferencia == modeloupdate.Id_Transferencia).Count() > 0)
                            afecta = true;
                        return Json(new { Exito = true, Mensaje = "Registro guardado correctamente.", Registro = modeloupdate, Afecta = afecta });
                    }

                }
                return Json(new { Exito = false, Mensaje = "El mes ya esta cerrado." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }

        }
        [HttpPost]
        public JsonResult GenerarDetallesArrastre(Int32 Id_Transferencia)
        {
            try
            {
                Ma_Transferencias model = MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == Id_Transferencia);
                if (model.Id_TipoT == 2)
                {
                    UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                    decimal[] result = proceduresDAL.PA_Genera_Detalle_Transferencia_EntreMeses(model.Id_Transferencia, model.Id_Mes_Origen, model.Id_Mes, model.Id_Area, model.Id_OGInicial, model.Id_OGFinal, (short)appUsuario.IdUsuario);
                    if (DeTransferenciasDAL.Get(x => x.Id_Transferencia == Id_Transferencia).Count() > 0)
                        return Json(new { Exito = true, Mensaje = "La propuesta de Arrastre de Saldos se realizo correctamente. Revisar los detalles." });
                    else
                        return Json(new { Exito = false, Mensaje = "No se generaron detalles." });
                }
                return Json(new { Exito = true, Mensaje = "Guardado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        #endregion

        #region Reportes
        public ActionResult PresupuestoAprobado()
        {
            List<MA_PresupuestoEg> entities = DALMaPresupuestoEg.Get().ToList();
            List<MA_PresupuestoEgModel> models = new List<MA_PresupuestoEgModel>();
            entities.ForEach(item => { models.Add(ModelFactory.getModel<MA_PresupuestoEgModel>(item, new MA_PresupuestoEgModel())); });
            models = models.OrderBy(reg => reg.Id_ClavePresupuesto).ToList();
            return File(reports.GenerarPDF_Horizontal("PresupuestoAprobado", models, this.ControllerContext), "Application/PDF");
            //return View(models);
        }

        public ActionResult Disponibilidad()
        {
            Modelos.DisponibilidadModel model = new Modelos.DisponibilidadModel();
            Dictionary<int, string> meses = Diccionarios.Meses;
            if (meses.Count < 13)
                meses.Add(13, "ANUAL");

            model.Lista_Meses = new SelectList(Diccionarios.Meses, "Key", "Value", 13);
            return View(model);
        }

        //[HttpPost]
        public ActionResult tblDisponibilidad(byte Id_Mes)
        {
            List<De_DisponibilidadModel> models = new List<De_DisponibilidadModel>();
            List<DE_Disponibilidad> entities = new List<DE_Disponibilidad>();
            if (Id_Mes == 13)
            {
                DALvDisponibilidad.Get().ToList().ForEach(item => { models.Add(ModelFactory.getModel<De_DisponibilidadModel>(item, new De_DisponibilidadModel())); });
                models.ForEach(item => { item.Ampliaciones1 = item.Ampliaciones1 + item.Ampliaciones; item.Reducciones1 = item.Reducciones1 + item.Reducciones; });
                return View(models);
            }

            entities = DALDeDisponibilidad.Get(reg => reg.Mes == Id_Mes).ToList();
            entities.ForEach(item => { models.Add(ModelFactory.getModel<De_DisponibilidadModel>(item, new De_DisponibilidadModel())); });
            models.ForEach(item => { item.Ampliaciones1 = item.Ampliaciones1 + item.Ampliaciones; item.Reducciones1 = item.Reducciones1 + item.Reducciones; });
            models = models.OrderBy(reg => reg.Id_ClavePresupuesto).ToList();
            return View(models);
        }

        [HttpGet]
        public ActionResult rptDisponibilidad(byte Id_Mes)
        {
            List<De_DisponibilidadModel> models = new List<De_DisponibilidadModel>();
            if (Id_Mes == 13)
            {
                DALvDisponibilidad.Get().ToList().ForEach(item => { models.Add(ModelFactory.getModel<De_DisponibilidadModel>(item, new De_DisponibilidadModel())); });
                ViewBag.TituloMes = "DEL AÑO COMPLETO";
            }
            else
            {
                DALDeDisponibilidad.Get(reg => reg.Mes == Id_Mes).ToList().ForEach(item => { models.Add(ModelFactory.getModel<De_DisponibilidadModel>(item, new De_DisponibilidadModel())); });
                ViewBag.TituloMes = String.Format("DEL MES DE {0}", Diccionarios.Meses[Id_Mes]);
            }

            models.ForEach(item => { item.Ampliaciones1 = item.Ampliaciones1 + item.Ampliaciones; item.Reducciones1 = item.Reducciones1 + item.Reducciones; });
            models = models.OrderBy(reg => reg.Id_ClavePresupuesto).ToList();
            return File(reports.GenerarPDF_Horizontal("rptDisponibilidad", models, this.ControllerContext), "Application/PDF");
        }
        public ActionResult V_ReporteDisponibles()
        {
            IEnumerable<VW_DisponibilidadMeses> models = disponibilidadMeses.Get().OrderBy(x => x.Id_ClavePresupuesto);
            return File(reports.GenerarPDF_Horizontal("V_ReporteDisponibles", models, this.ControllerContext), "Application/PDF");
        }
        public ActionResult BusquedaDisponibilidadAvanzada()
        {
            return View(new BusquedaDisponibilidadAvanzada());
        }
        #endregion


    }
}
