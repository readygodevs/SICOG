using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;

namespace TesoreriaVS12.Areas.Tesoreria.Models
{

    public class BusquedaPresupuestos
    {
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public bool? UltimoNivel { get; set; }
        public string ProyectoProceso { get; set; }
        public string ActividadMIR { get; set; }
        #region Egresos
        public static List<BusquedaPresupuestos> Lista(string clave, string descripcion, string tipo, string ProyectoProceso, string ActividadMIR, int tipoBusqueda = 0)
        {
            List<BusquedaPresupuestos> lista = new List<BusquedaPresupuestos>();
            int o = Convert.ToInt16(tipo);
            switch (o)
            {
                case 1:
                    AreasDAL areasmodel = new AreasDAL();
                    List<Ca_Areas> areas = new List<Ca_Areas>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        areas = areasmodel.Get(x => (tipoBusqueda == 1 ? x.Id_Area.StartsWith(clave):tipoBusqueda == 2 ? x.Id_Area.Contains(clave):x.Id_Area.EndsWith(clave))).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        areas = areasmodel.Get(x => (tipoBusqueda == 1 ? x.Id_Area.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_Area.Contains(clave) : x.Id_Area.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        areas = areasmodel.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        areas = areasmodel.Get().ToList();
                    }
                    foreach (var a in areas)
                    {
                        BusquedaPresupuestos p = new BusquedaPresupuestos();
                        p.Clave = a.Id_Area;
                        p.Descripcion = a.Descripcion;
                        p.UltimoNivel= a.UltimoNivel.HasValue ? a.UltimoNivel.Value:false;
                        lista.Add(p);
                    }
                    break;
                case 2:
                    FuncionDAL funcionModel = new FuncionDAL();
                    List<Ca_Funciones> funciones = new List<Ca_Funciones>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        funciones = funcionModel.Get(x => (tipoBusqueda == 1 ? x.Id_Funcion.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_Funcion.Contains(clave) : x.Id_Funcion.EndsWith(clave))).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        funciones = funcionModel.Get(x => (tipoBusqueda == 1 ? x.Id_Funcion.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_Funcion.Contains(clave) : x.Id_Funcion.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        funciones = funcionModel.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        funciones = funcionModel.Get().ToList();
                    }
                    foreach (var f in funciones)
                    {
                        BusquedaPresupuestos p = new BusquedaPresupuestos();
                        p.Clave = f.Id_Funcion;
                        p.Descripcion = f.Descripcion;
                        p.UltimoNivel = f.Subfuncion > 0 ? true : false;
                        lista.Add(p);
                    }
                    break;
                case 3:
                    ActividadDAL actividadModel = new ActividadDAL();
                    List<Ca_ActividadesInst> actividades = new List<Ca_ActividadesInst>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        actividades = actividadModel.Get(x => (tipoBusqueda == 1 ? x.Id_Actividad.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_Actividad.Contains(clave) : x.Id_Actividad.EndsWith(clave))).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        actividades = actividadModel.Get(x => (tipoBusqueda == 1 ? x.Id_Actividad.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_Actividad.Contains(clave) : x.Id_Actividad.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        actividades = actividadModel.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        actividades = actividadModel.Get().ToList();
                    }
                    foreach (var a in actividades)
                    {
                        BusquedaPresupuestos p = new BusquedaPresupuestos();
                        p.Clave = a.Id_Actividad;
                        p.Descripcion = a.Descripcion;
                        p.UltimoNivel = true;
                        lista.Add(p);
                    }
                    break;
                case 4:
                    ClasificacionPDAL clasificacionModel = new ClasificacionPDAL();
                    List<Ca_ClasProgramatica> clasificacion = new List<Ca_ClasProgramatica>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        clasificacion = clasificacionModel.Get(x => (tipoBusqueda == 1 ? x.Id_ClasificacionP.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_ClasificacionP.Contains(clave) : x.Id_ClasificacionP.EndsWith(clave))).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        clasificacion = clasificacionModel.Get(x => (tipoBusqueda == 1 ? x.Id_ClasificacionP.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_ClasificacionP.Contains(clave) : x.Id_ClasificacionP.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        clasificacion = clasificacionModel.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        clasificacion = clasificacionModel.Get().ToList();
                    }
                    foreach (var c in clasificacion)
                    {
                        BusquedaPresupuestos p = new BusquedaPresupuestos();
                        p.Clave = c.Id_ClasificacionP;
                        p.Descripcion = c.Descripcion;
                        p.UltimoNivel = true;
                        lista.Add(p);
                    }
                    break;
                case 5:
                    ProgramaDAL programaModel = new ProgramaDAL();
                    List<CA_Programas> programas = new List<CA_Programas>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        programas = programaModel.Get(x => (tipoBusqueda == 1 ? x.Id_Programa.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_Programa.Contains(clave) : x.Id_Programa.EndsWith(clave))).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        programas = programaModel.Get(x => (tipoBusqueda == 1 ? x.Id_Programa.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_Programa.Contains(clave) : x.Id_Programa.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        programas = programaModel.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        programas = programaModel.Get().ToList();
                    }
                    foreach (var c in programas)
                    {
                        BusquedaPresupuestos p = new BusquedaPresupuestos();
                        p.Clave = c.Id_Programa;
                        p.Descripcion = c.Descripcion;
                        p.UltimoNivel = true;
                        lista.Add(p);
                    }
                    break;
                case 6:
                    ProcesoDAL procesoModel = new ProcesoDAL();
                    List<Ca_Proyecto> procesos = new List<Ca_Proyecto>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        procesos = procesoModel.Get(x => (tipoBusqueda == 1 ? x.Id_Proceso.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_Proceso.Contains(clave) : x.Id_Proceso.EndsWith(clave))).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        procesos = procesoModel.Get(x => (tipoBusqueda == 1 ? x.Id_Proceso.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_Proceso.Contains(clave) : x.Id_Proceso.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        procesos = procesoModel.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        procesos = procesoModel.Get().ToList();
                    }
                    foreach (var c in procesos)
                    {
                        BusquedaPresupuestos p = new BusquedaPresupuestos();
                        p.Clave = c.Id_Proceso;
                        p.Descripcion = c.Descripcion;
                        p.UltimoNivel = true;
                        lista.Add(p);
                    }
                    break;
                case 7:
                    TipoMetaDAL metaModel = new TipoMetaDAL();
                    List<Ca_TipoMeta> tiposMeta = new List<Ca_TipoMeta>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        tiposMeta = metaModel.Get(x => (tipoBusqueda == 1 ? x.Id_TipoMeta.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_TipoMeta.Contains(clave) : x.Id_TipoMeta.EndsWith(clave))).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        tiposMeta = metaModel.Get(x => (tipoBusqueda == 1 ? x.Id_TipoMeta.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_TipoMeta.Contains(clave) : x.Id_TipoMeta.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        tiposMeta = metaModel.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        tiposMeta = metaModel.Get().ToList();
                    }
                    foreach (var c in tiposMeta)
                    {
                        BusquedaPresupuestos p = new BusquedaPresupuestos();
                        p.Clave = c.Id_TipoMeta;
                        p.Descripcion = c.Descripcion;
                        p.UltimoNivel = true;
                        lista.Add(p);
                    }
                    break;
                case 8:
                    ActividadMIRDAL actividadMirModel = new ActividadMIRDAL();
                    List<Ca_Actividad> actividadMir = new List<Ca_Actividad>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        actividadMir = actividadMirModel.Get(x => (tipoBusqueda == 1 ? x.Id_ActividadMIR2.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_ActividadMIR2.Contains(clave) : x.Id_ActividadMIR2.EndsWith(clave)) && x.Id_Proceso == ProyectoProceso).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        actividadMir = actividadMirModel.Get(x => (tipoBusqueda == 1 ? x.Id_ActividadMIR2.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_ActividadMIR2.Contains(clave) : x.Id_ActividadMIR2.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion)) && x.Id_Proceso == ProyectoProceso).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        actividadMir = actividadMirModel.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion)) && x.Id_Proceso == ProyectoProceso).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        actividadMir = actividadMirModel.Get(x=>x.Id_Proceso == ProyectoProceso).ToList();
                    }
                    foreach (var c in actividadMir)
                    {
                        BusquedaPresupuestos p = new BusquedaPresupuestos();
                        p.Clave = c.Id_ActividadMIR2;
                        p.Descripcion = c.Descripcion;
                        p.UltimoNivel = true;
                        p.ProyectoProceso = c.Id_Proceso;
                        HttpContext.Current.Session.Add("BusquedaProyecto", true);
                        lista.Add(p);
                    }
                    break;
                case 9:
                    AccionDAL accionModel = new AccionDAL();
                    List<Ca_Acciones> acciones = new List<Ca_Acciones>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        acciones = accionModel.Get(x => (tipoBusqueda == 1 ? x.Id_Accion2.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_Accion2.Contains(clave) : x.Id_Accion2.EndsWith(clave)) && x.Id_ActividadMIR2 == ActividadMIR && x.Id_Proceso == ProyectoProceso).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        acciones = accionModel.Get(x => (tipoBusqueda == 1 ? x.Id_Accion2.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_Accion2.Contains(clave) : x.Id_Accion2.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion)) && x.Id_ActividadMIR2 == ActividadMIR && x.Id_Proceso == ProyectoProceso).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        acciones = accionModel.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion)) && x.Id_ActividadMIR2 == ActividadMIR && x.Id_Proceso == ProyectoProceso).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion) )
                    {
                        acciones = accionModel.Get(x => (!String.IsNullOrEmpty(ActividadMIR) ? x.Id_ActividadMIR2 == ActividadMIR : x.Id_ActividadMIR2 != null) && (!String.IsNullOrEmpty(ProyectoProceso) ? x.Id_Proceso == ProyectoProceso : x.Id_Proceso != null)).ToList();
                    }
                    foreach (var c in acciones)
                    {
                        BusquedaPresupuestos p = new BusquedaPresupuestos();
                        p.Clave = c.Id_Accion2;
                        p.Descripcion = c.Descripcion;
                        p.UltimoNivel = true;
                        p.ActividadMIR = c.Id_ActividadMIR2;
                        p.ProyectoProceso = c.Id_Proceso;
                        HttpContext.Current.Session.Add("BusquedaActividad", true);
                        lista.Add(p);
                    }
                    break;
                case 10:
                    AlcanceDAL alcanceModel = new AlcanceDAL();
                    List<Ca_AlcanceGeo> alcances = new List<Ca_AlcanceGeo>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        alcances = alcanceModel.Get(x => (tipoBusqueda == 1 ? x.Id_AlcanceGeo.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_AlcanceGeo.Contains(clave) : x.Id_AlcanceGeo.EndsWith(clave))).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        alcances = alcanceModel.Get(x => (tipoBusqueda == 1 ? x.Id_AlcanceGeo.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_AlcanceGeo.Contains(clave) : x.Id_AlcanceGeo.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        alcances = alcanceModel.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        alcances = alcanceModel.Get().ToList();
                    }
                    foreach (var c in alcances)
                    {
                        BusquedaPresupuestos p = new BusquedaPresupuestos();
                        p.Clave = c.Id_AlcanceGeo;
                        p.Descripcion = c.Descripcion;
                        p.UltimoNivel = true;
                        lista.Add(p);
                    }
                    break;
                case 11:
                    TipoGastosDAL gastoModel = new TipoGastosDAL();
                    List<Ca_TipoGastos> gastos = new List<Ca_TipoGastos>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        gastos = gastoModel.Get(x => (tipoBusqueda == 1 ? x.Id_TipoGasto.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_TipoGasto.Contains(clave) : x.Id_TipoGasto.EndsWith(clave))).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        gastos = gastoModel.Get(x => (tipoBusqueda == 1 ? x.Id_TipoGasto.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_TipoGasto.Contains(clave) : x.Id_TipoGasto.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        gastos = gastoModel.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        gastos = gastoModel.Get().ToList();
                    }
                    foreach (var c in gastos)
                    {
                        BusquedaPresupuestos p = new BusquedaPresupuestos();
                        p.Clave = c.Id_TipoGasto;
                        p.Descripcion = c.Descripcion;
                        p.UltimoNivel = true;
                        lista.Add(p);
                    }
                    break;
                case 12:
                    FuenteDAL fuenteModel = new FuenteDAL();
                    List<Ca_FuentesFin> fuentes = new List<Ca_FuentesFin>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        fuentes = fuenteModel.Get(x => (tipoBusqueda == 1 ? x.Id_Fuente.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_Fuente.Contains(clave) : x.Id_Fuente.EndsWith(clave))).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        fuentes = fuenteModel.Get(x => (tipoBusqueda == 1 ? x.Id_Fuente.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_Fuente.Contains(clave) : x.Id_Fuente.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        fuentes = fuenteModel.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        fuentes = fuenteModel.Get().ToList();
                    }
                    foreach (var c in fuentes)
                    {
                        BusquedaPresupuestos p = new BusquedaPresupuestos();
                        p.Clave = c.Id_Fuente;
                        p.Descripcion = c.Descripcion;
                        p.UltimoNivel = true;
                        lista.Add(p);
                    }
                    break;
                //case 13:
                //     funcionModel = new FuncionDAL();
                //    Ca_Funciones funcion = funcionModel.GetByID(x=>x.Id_Funcion == id);
                //    descripcion= funcion.Descripcion;
                //    break;
                case 14:
                    ObjetoGDAL objetoModel = new ObjetoGDAL();
                    List<CA_ObjetoGasto> objetos = new List<CA_ObjetoGasto>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        objetos = objetoModel.Get(x => (tipoBusqueda == 1 ? x.Id_ObjetoG.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_ObjetoG.Contains(clave) : x.Id_ObjetoG.EndsWith(clave))).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        objetos = objetoModel.Get(x => (tipoBusqueda == 1 ? x.Id_ObjetoG.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_ObjetoG.Contains(clave) : x.Id_ObjetoG.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        objetos = objetoModel.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        objetos = objetoModel.Get().ToList();
                    }
                    foreach (var c in objetos)
                    {
                        BusquedaPresupuestos p = new BusquedaPresupuestos();
                        p.Clave = c.Id_ObjetoG;
                        p.Descripcion = c.Descripcion;
                        p.UltimoNivel = c.UltimoNivel.HasValue ? c.UltimoNivel.Value : false;
                        lista.Add(p);
                    }
                    break;
                default:
                    descripcion = "";
                    break;
            }
            return lista;
        }

        public static List<MA_PresupuestoEg> BuscarPresupuesto(ModalPresupuestoModel presupuesto)
        {
            List<MA_PresupuestoEg> listaPresupuestos = new List<MA_PresupuestoEg>();
            try
            {
                //if (!String.IsNullOrEmpty(presupuesto.AnioFinModal))
                //    presupuesto.AnioFinModal = presupuesto.AnioFinModal.Substring(2, 2);
                MaPresupuestoEgDAL pDal = new MaPresupuestoEgDAL();
                listaPresupuestos = pDal.Get(x => (!String.IsNullOrEmpty(presupuesto.Id_AreaModal) ? x.Id_Area == presupuesto.Id_AreaModal : x.Id_Area != null) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_FuncionModal) ? x.Id_Funcion == presupuesto.Id_FuncionModal : x.Id_Funcion != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_ActividadModal) ? x.Id_Actividad == presupuesto.Id_ActividadModal : x.Id_Actividad != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_ClasificacionPModal) ? x.Id_ClasificacionP == presupuesto.Id_ClasificacionPModal : x.Id_ClasificacionP != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_ProgramaModal) ? x.Id_Programa == presupuesto.Id_ProgramaModal : x.Id_Programa != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_ProcesoModal) ? x.Id_Proceso == presupuesto.Id_ProcesoModal : x.Id_Proceso != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_TipoMetaModal) ? x.Id_TipoMeta == presupuesto.Id_TipoMetaModal : x.Id_TipoMeta != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_ActividadMIRModal) ? x.Id_ActividadMIR == presupuesto.Id_ActividadMIRModal : x.Id_ActividadMIR != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_AccionModal) ? x.Id_Accion == presupuesto.Id_AccionModal : x.Id_Accion != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_AlcanceModal) ? x.Id_Alcance == presupuesto.Id_AlcanceModal : x.Id_Alcance != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_TipoGModal) ? x.Id_TipoG == presupuesto.Id_TipoGModal : x.Id_TipoG != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_FuenteModal) ? x.Id_Fuente == presupuesto.Id_FuenteModal : x.Id_Fuente != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.AnioFinModal) ? x.AnioFin == presupuesto.AnioFinModal : x.AnioFin != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_ObjetoGModal) ? x.Id_ObjetoG == presupuesto.Id_ObjetoGModal : x.Id_ObjetoG != null))).ToList();
            }
            catch (Exception ex)
            {
                return listaPresupuestos;
            }

            return listaPresupuestos;
        }

        #endregion
        

    }
    public class EvolucionIngresos
    {
        public string Mes { get; set; }
        public decimal? Estimado { get; set; }
        public decimal? Ampliaciones { get; set; }
        public decimal? Reducciones { get; set; }
        public decimal? Modificado { get; set; }
        public decimal? Devengado { get; set; }
        public decimal? Recaudado { get; set; }
        public decimal? PorEjecutar { get; set; }
    }

    public class Disponibildad
    {
        public string Mes { get; set; }
        public decimal Aprobado { get; set; }
        public decimal Ampliaciones1 { get; set; }
        public decimal Reducciones1 { get; set; }
        public decimal Ampliaciones2 { get; set; }
        public decimal Reducciones2 { get; set; }
        public decimal Vigente { get; set; }
        public decimal PreComprometido { get; set; }
        public decimal Comprometido { get; set; }
        public decimal Devengado { get; set; }
        public decimal Ejercido { get; set; }
        public decimal Pagado { get; set; }
        public decimal Disponible { get; set; }
        public static List<Disponibildad> ObtenerDisponibilidad(De_DisponibilidadModel presupuesto)
        {
            List<Disponibildad> disponibilidad = new List<Disponibildad>();
            //if (!String.IsNullOrEmpty(presupuesto.AnioFin))
            //    presupuesto.AnioFin = presupuesto.AnioFin.Substring(2, 2);
            DeDisponibilidadDAL dispDal = new DeDisponibilidadDAL();
            //List<DE_Disponibilidad> listaDisponibilidad = pDal.Get(x=>);
            List<DE_Disponibilidad> listaPresupuestos = dispDal.Get(x => (!String.IsNullOrEmpty(presupuesto.Id_Area) ? x.Id_Area == presupuesto.Id_Area : x.Id_Area != null) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_Funcion) ? x.Id_Funcion == presupuesto.Id_Funcion : x.Id_Funcion != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_Actividad) ? x.Id_Actividad == presupuesto.Id_Actividad : x.Id_Actividad != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_ClasificacionP) ? x.Id_ClasificacionP == presupuesto.Id_ClasificacionP : x.Id_ClasificacionP != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_Programa) ? x.Id_Programa == presupuesto.Id_Programa : x.Id_Programa != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_Proceso) ? x.Id_Proceso == presupuesto.Id_Proceso : x.Id_Proceso != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_TipoMeta) ? x.Id_TipoMeta == presupuesto.Id_TipoMeta : x.Id_TipoMeta != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_ActividadMIR) ? x.Id_ActividadMIR == presupuesto.Id_ActividadMIR : x.Id_ActividadMIR != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_Accion) ? x.Id_Accion == presupuesto.Id_Accion : x.Id_Accion != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_Alcance) ? x.Id_Alcance == presupuesto.Id_Alcance : x.Id_Alcance != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_TipoG) ? x.Id_TipoG == presupuesto.Id_TipoG : x.Id_TipoG != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_Fuente) ? x.Id_Fuente == presupuesto.Id_Fuente : x.Id_Fuente != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.AnioFin) ? x.AnioFin == presupuesto.AnioFin : x.AnioFin != null)) &&
                    ((!String.IsNullOrEmpty(presupuesto.Id_ObjetoG) ? x.Id_ObjetoG == presupuesto.Id_ObjetoG : x.Id_ObjetoG != null))).OrderBy(x => x.Mes).ToList();
            for (int i = 1; i < 13; i++)
            {
                Disponibildad d = new Disponibildad();
                d.Aprobado =  listaPresupuestos.Where(x => x.Mes == i).Sum(x => x.Aprobado).Value;
                d.Ampliaciones1 = listaPresupuestos.Where(x => x.Mes == i).Sum(x => x.Ampliaciones1).Value;
                d.Reducciones1 = listaPresupuestos.Where(x => x.Mes == i).Sum(x => x.Reducciones1).Value;
                d.Ampliaciones2 = listaPresupuestos.Where(x => x.Mes == i).Sum(x => x.Ampliaciones).Value;
                d.Reducciones2 = listaPresupuestos.Where(x => x.Mes == i).Sum(x => x.Reducciones).Value;
                d.Vigente = listaPresupuestos.Where(x => x.Mes == i).Sum(x => x.Vigente).Value;
                d.PreComprometido = listaPresupuestos.Where(x => x.Mes == i).Sum(x => x.PreComprometido).Value;
                d.Comprometido = listaPresupuestos.Where(x => x.Mes == i).Sum(x => x.Comprometido).Value;
                d.Devengado = listaPresupuestos.Where(x => x.Mes == i).Sum(x => x.Devengado).Value;
                d.Ejercido = listaPresupuestos.Where(x => x.Mes == i).Sum(x => x.Ejercido).Value;
                d.Pagado = listaPresupuestos.Where(x => x.Mes == i).Sum(x => x.Pagado).Value;
                d.Disponible = listaPresupuestos.Where(x => x.Mes == i).Sum(x => x.Disponible).Value;
                d.Mes = Diccionarios.Meses[i];
                disponibilidad.Add(d);
            }
            return disponibilidad;
        }
    }

}