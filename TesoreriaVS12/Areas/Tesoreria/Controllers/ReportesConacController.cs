using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Filters;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
     [AuthorizeLogin]
    public class ReportesConacController : Controller
    {
         protected AccionDAL accion { get; set; }
         protected AreasDAL areas { get; set; }
         protected ConceptosIngresosDAL cri { get; set; }
         protected  DeDisponibilidadDAL disponibilidad { get; set; }
         protected DeEvolucionDAL evolucion { get; set; }
         protected FirmasDAL firmas { get; set; }
         protected FuncionDAL funcion { get; set; }
         protected FuenteDAL fuente { get; set; }
         protected FuenteIngDAL fuenteIng { get; set; }
         protected ObjetoGDAL objetoG { get; set; }
         protected ProceduresDAL procedures { get; set; }
         protected ProcesoDAL proyecto { get; set; }
         protected ClasProgramaticaDAL pragmatica { get; set; }
         protected ConvertHtmlToString reports { get; set; }
         protected TipoGastosDAL tipoGasto  { get; set; }
        //
        // GET: /Tesoreria/ReportesConac/
         public ReportesConacController()
         {
             if (accion == null) accion = new AccionDAL();
             if (areas == null) areas = new AreasDAL();
             if (cri == null) cri = new ConceptosIngresosDAL ();
             if (disponibilidad == null) disponibilidad = new DeDisponibilidadDAL();
             if (evolucion == null) evolucion = new DeEvolucionDAL();
             if (firmas == null) firmas = new FirmasDAL();
             if (funcion == null) funcion = new FuncionDAL();
             if (fuente == null) fuente = new FuenteDAL();
             if (fuenteIng == null) fuenteIng = new FuenteIngDAL();
             if (objetoG == null) objetoG = new ObjetoGDAL();
             if (pragmatica == null) pragmatica = new ClasProgramaticaDAL();
             if (procedures == null) procedures = new ProceduresDAL();
             if (proyecto == null) proyecto = new ProcesoDAL();
             if (reports == null) reports = new ConvertHtmlToString();
             if (tipoGasto == null) tipoGasto = new TipoGastosDAL();
         }
        public ActionResult Index()
        {
            return View();
        }
        #region EstadoSituacionFinanciera
        public ActionResult V_EstadoSituacionFinanciera()
         {
             ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, 1, 1).ToShortDateString();
             ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
             return View();
         }
         public ActionResult ConsultaESF(DateTime? FechaFin, bool? ceros, bool? firmantes)
         {
             short anio1 = Convert.ToInt16(DateTime.Now.Year);
             short anio2 = Convert.ToInt16(anio1-1);
              List<tblRepCuentas> entities= new List<tblRepCuentas>();
             if(ceros.Value)
                 entities = procedures.PA_ReporteCuentasEjercicios(anio1, anio2, null, FechaFin).Where(x=>(x.Ejercicio1.Value>0 || x.Ejercicio2.Value>0) && x.Genero<4).ToList();
             else
                 entities = procedures.PA_ReporteCuentasEjercicios(anio1, anio2, null, FechaFin).Where(x=>x.Genero<4).ToList();
             List<tblRepCuentasModel> Lst = new List<tblRepCuentasModel>();
             //DateTime fecha = new DateTime(DateTime.Now.Year, mes.Value, 1).AddMonths(1);
             ViewBag.fecha = String.Format("al {0} de {1} de {2} y {3}", FechaFin.Value.Day, Diccionarios.Meses[FechaFin.Value.Month].ToLower(), DateTime.Now.Year, DateTime.Now.Year - 1);
             ViewBag.Ejercicio1 = DateTime.Now.Year;
             ViewBag.Ejercicio2 = DateTime.Now.Year-1;
             ViewBag.Firmante1N = firmas.Get(x => x.IdTipo == 1).FirstOrDefault().Nombre;
             ViewBag.Firmante1C = firmas.Get(x => x.IdTipo == 1).FirstOrDefault().Cargo;
             ViewBag.Firmante2N = firmas.Get(x => x.IdTipo == 1).LastOrDefault().Nombre;
             ViewBag.Firmante2C= firmas.Get(x => x.IdTipo == 1).LastOrDefault().Cargo;
             ViewBag.firmantes = firmantes;
             foreach (tblRepCuentas item in entities)
             {
                 tblRepCuentasModel model = ModelFactory.getModel<tblRepCuentasModel>(item, new tblRepCuentasModel());
                 Lst.Add(model);
             }
             return View(Lst);
         }
         public ActionResult ReporteESF(string FechaFinS, bool? ceros, bool? firmantes, bool? encabezado)
         {
             DateTime? FechaFin = Convert.ToDateTime(FechaFinS);
             short anio1 = Convert.ToInt16(DateTime.Now.Year);
             short anio2 = Convert.ToInt16(anio1 - 1);
             List<tblRepCuentas> entities = new List<tblRepCuentas>();
             if (ceros.Value)
                 entities = procedures.PA_ReporteCuentasEjercicios(anio1, anio2, null, FechaFin).Where(x => (x.Ejercicio1.Value > 0 || x.Ejercicio2.Value > 0) && x.Genero < 4).ToList();
             else
                 entities = procedures.PA_ReporteCuentasEjercicios(anio1, anio2, null, FechaFin).Where(x => x.Genero < 4).ToList();
             List<tblRepCuentasModel> Lst = new List<tblRepCuentasModel>();
             //DateTime fecha = new DateTime(DateTime.Now.Year, mes.Value, 1).AddMonths(1);
             ViewBag.fecha = String.Format("al {0} de {1} de {2} y {3}", FechaFin.Value.Day, Diccionarios.Meses[FechaFin.Value.Month].ToLower(), DateTime.Now.Year, DateTime.Now.Year - 1);
             ViewBag.Ejercicio1 = DateTime.Now.Year;
             ViewBag.Ejercicio2 = DateTime.Now.Year - 1;
             ViewBag.Firmante1N = firmas.Get(x => x.IdTipo == 1).FirstOrDefault().Nombre;
             ViewBag.Firmante1C = firmas.Get(x => x.IdTipo == 1).FirstOrDefault().Cargo;
             ViewBag.Firmante2N = firmas.Get(x => x.IdTipo == 1).LastOrDefault().Nombre;
             ViewBag.Firmante2C = firmas.Get(x => x.IdTipo == 1).LastOrDefault().Cargo;
             ViewBag.firmantes = firmantes;
             ViewBag.encabezado = encabezado;
             foreach (tblRepCuentas item in entities)
             {
                 tblRepCuentasModel model = ModelFactory.getModel<tblRepCuentasModel>(item, new tblRepCuentasModel());
                 Lst.Add(model);
             }
             return File(reports.GenerarPDF_Horizontal("ReporteESF", Lst, this.ControllerContext), "Application/PDF");
         }
        #endregion

         #region EstadoActividades

         public ActionResult V_EstadoActividades()
         {
             ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, 1, 1).ToShortDateString();
             ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
             return View();
         }
         public ActionResult ConsultaEA(DateTime? FechaInicio, DateTime? FechaFin, bool? ceros, bool? firmantes)
         {
            short anio1 = Convert.ToInt16(Session["Ejercicio"]);//DateTime.Now.Year);
             short anio2 = Convert.ToInt16(anio1 - 1);

            FechaInicio = new DateTime(anio1, FechaInicio.Value.Month, FechaInicio.Value.Day);
            FechaFin = new DateTime(anio1, FechaFin.Value.Month, FechaFin.Value.Day);
            

             List<tblRepCuentas> entities = new List<tblRepCuentas>();
             if (ceros.Value)
                 entities = procedures.PA_ReporteCuentasEjercicios(anio1, anio2, null, FechaFin).Where(x => (x.Ejercicio1.Value > 0 || x.Ejercicio2.Value > 0) && x.Genero>3).ToList();
             else
                 entities = procedures.PA_ReporteCuentasEjercicios(anio1, anio2, null, FechaFin).Where(x=>x.Genero>3).ToList();
             List<tblRepCuentasModel> Lst = new List<tblRepCuentasModel>();
             //DateTime fecha = new DateTime(DateTime.Now.Year, mes.Value, 1).AddMonths(1);
             ViewBag.fecha = String.Format("Del {0} de {1} al {4} de {5} de {2} y {3}", FechaInicio.Value.Day, Diccionarios.Meses[FechaInicio.Value.Month].ToLower(), DateTime.Now.Year, DateTime.Now.Year - 1, FechaFin.Value.Day, Diccionarios.Meses[FechaFin.Value.Month].ToLower());
             ViewBag.Ejercicio1 = DateTime.Now.Year;
             ViewBag.Ejercicio2 = DateTime.Now.Year - 1;
             ViewBag.Firmante1N = firmas.Get(x => x.IdTipo == 1).FirstOrDefault().Nombre;
             ViewBag.Firmante1C = firmas.Get(x => x.IdTipo == 1).FirstOrDefault().Cargo;
             ViewBag.Firmante2N = firmas.Get(x => x.IdTipo == 1).LastOrDefault().Nombre;
             ViewBag.Firmante2C = firmas.Get(x => x.IdTipo == 1).LastOrDefault().Cargo;
             ViewBag.firmantes = firmantes;
             foreach (tblRepCuentas item in entities)
             {
                 tblRepCuentasModel model = ModelFactory.getModel<tblRepCuentasModel>(item, new tblRepCuentasModel());
                 Lst.Add(model);
             }
             return View(Lst);
         }
         public ActionResult ReporteEA(string FechaInicioS, string FechaFinS, bool? ceros, bool? firmantes, bool? encabezado)
         {
             DateTime? FechaInicio = Convert.ToDateTime(FechaInicioS);
             DateTime? FechaFin = Convert.ToDateTime(FechaFinS);
             short anio1 = Convert.ToInt16(DateTime.Now.Year);
             short anio2 = Convert.ToInt16(anio1 - 1);
             List<tblRepCuentas> entities = new List<tblRepCuentas>();
             if (ceros.Value)
                 entities = procedures.PA_ReporteCuentasEjercicios(anio1, anio2, null, FechaFin).Where(x => (x.Ejercicio1.Value > 0 || x.Ejercicio2.Value > 0) && x.Genero > 3).ToList();
             else
                 entities = procedures.PA_ReporteCuentasEjercicios(anio1, anio2, null, FechaFin).Where(x => x.Genero > 3).ToList();
             List<tblRepCuentasModel> Lst = new List<tblRepCuentasModel>();
             //DateTime fecha = new DateTime(DateTime.Now.Year, mes.Value, 1).AddMonths(1);
             ViewBag.fecha = String.Format("Del {0} de {1} al {4} de {5} de {2} y {3}", FechaInicio.Value.Day, Diccionarios.Meses[FechaInicio.Value.Month].ToLower(), DateTime.Now.Year, DateTime.Now.Year - 1, FechaFin.Value.Day, Diccionarios.Meses[FechaFin.Value.Month].ToLower());
             ViewBag.Ejercicio1 = DateTime.Now.Year;
             ViewBag.Ejercicio2 = DateTime.Now.Year - 1;
             ViewBag.Firmante1N = firmas.Get(x => x.IdTipo == 1).FirstOrDefault().Nombre;
             ViewBag.Firmante1C = firmas.Get(x => x.IdTipo == 1).FirstOrDefault().Cargo;
             ViewBag.Firmante2N = firmas.Get(x => x.IdTipo == 1).LastOrDefault().Nombre;
             ViewBag.Firmante2C = firmas.Get(x => x.IdTipo == 1).LastOrDefault().Cargo;
             ViewBag.firmantes = firmantes;
             ViewBag.encabezado = encabezado;
             foreach (tblRepCuentas item in entities)
             {
                 tblRepCuentasModel model = ModelFactory.getModel<tblRepCuentasModel>(item, new tblRepCuentasModel());
                 Lst.Add(model);
             }
             return File(reports.GenerarPDF_Horizontal("ReporteEA", Lst, this.ControllerContext), "Application/PDF");
         }

         #endregion

         #region AnaliticoIngresos
         public ActionResult V_AnaliticoIngresos()
         {
             return View();
         }
         public ActionResult ConsultaAnaliticoIngresos(Int32 IdMes, Int32 IdMesFin)
         {

             List<ReporteClavePresupuestariaModel> models = new List<ReporteClavePresupuestariaModel>();
             foreach (string item in ReporteClavePresupuestaria.Cri())
             {
                 string id = item;
                 int length = item.Length;
                 ReporteClavePresupuestariaModel temp = new ReporteClavePresupuestariaModel();
                 if (item.Length == 1)
                     temp.Clave = id;
                 else
                     temp.Clave = String.Format("{0}.{1}", item.Substring(0, 1), item.Substring(1, 1));
                 string clave = item.PadRight(3, '0');
                 temp.Concepto = cri.GetByID(x => x.Id_Concepto == clave).Descripcion;
                 if (evolucion.Get(x => x.Id_Concepto.Substring(0, length) == item && x.Mes >= IdMes && x.Mes <= IdMesFin).Count() > 0)
                 {
                     temp.Estimado = evolucion.Get(x => x.Id_Concepto.Substring(0, length) == item && x.Mes >= IdMes && x.Mes <= IdMesFin).Sum(x => x.Estimado).Value;
                     temp.AmpRed = evolucion.Get(x => x.Id_Concepto.Substring(0, length) == item && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Ampliaciones).Value;
                     temp.Modificado = temp.Recaudado + temp.AmpRed;
                     temp.Devengado = evolucion.Get(x => x.Id_Concepto.Substring(0, length) == item && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Devengado).Value;
                     temp.Recaudado = evolucion.Get(x => x.Id_Concepto.Substring(0, length) == item && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Recaudado).Value;
                     temp.Diferencia = temp.Recaudado - temp.Estimado;
                     models.Add(temp);
                 }
                 else
                 {
                     temp.Estimado = 0;
                     temp.AmpRed = 0;
                     temp.Modificado = 0;
                     temp.Devengado = 0;
                     temp.Recaudado = 0;
                     temp.Diferencia = 0;
                     models.Add(temp);
                 }

             }
             //foreach (Ca_ConceptosIngresos item in cri.Get())
             //{
             //    string id = item.Id_Concepto;
             //    ReporteClavePresupuestariaModel temp = new ReporteClavePresupuestariaModel();
             //    if (evolucion.Get(x => x.Id_Concepto == item.Id_Concepto && x.Mes >= IdMes && x.Mes <= IdMesFin).Count() > 0)
             //    {
             //        temp.Clave = id;
             //        temp.Concepto = item.Descripcion;
             //        temp.Estimado = evolucion.Get(x => x.Id_Concepto == item.Id_Concepto && x.Mes >= IdMes && x.Mes <= IdMesFin).Sum(x => x.Estimado).Value;
             //        temp.AmpRed = evolucion.Get(x => x.Id_Concepto == item.Id_Concepto && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Ampliaciones).Value;
             //        temp.Modificado = temp.Recaudado + temp.AmpRed;
             //        temp.Devengado = evolucion.Get(x => x.Id_Concepto == item.Id_Concepto && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Devengado).Value;
             //        temp.Recaudado = evolucion.Get(x => x.Id_Concepto == item.Id_Concepto && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Recaudado).Value;
             //        temp.Diferencia = temp.Recaudado - temp.Estimado;
             //        models.Add(temp);
             //    }
             //    else
             //    {
             //        temp.Clave = id;
             //        temp.Concepto = item.Descripcion;
             //        temp.Estimado = 0;
             //        temp.AmpRed = 0;
             //        temp.Modificado = 0;
             //        temp.Devengado = 0;
             //        temp.Recaudado = 0;
             //        temp.Diferencia = 0;
             //        models.Add(temp);
             //    }

             //}
             if (this.HttpContext.Request.IsAjaxRequest())
             {
                 ViewBag.Imprimir = false;
                 return View(models);
             }
             else
             {
                 ConvertHtmlToString pdf = new ConvertHtmlToString();
                 ViewBag.Imprimir = true;
                 return File(pdf.GenerarPDF_Horizontal("ConsultaAnaliticoIngresos", models, this.ControllerContext), "application/pdf");
             }
         }
         #endregion

         #region ObjetoGasto
         public ActionResult V_ObjetoGasto()
         {
             return View();
         }
         public ActionResult ConsultaObjetoGasto(Int32 IdMes, Int32 IdMesFin)
         {

             List<ReporteClavePresupuestariaModel> models = new List<ReporteClavePresupuestariaModel>();
             foreach (int item in ReporteClavePresupuestaria.ObjetoGato())
             {
                 string id = item.ToString();
                 ReporteClavePresupuestariaModel temp = new ReporteClavePresupuestariaModel();
                 temp.Clave = item.ToString();
                 temp.Concepto = objetoG.GetByID(x => x.Id_ObjetoG == id).Descripcion;
                 temp.Aprobado = disponibilidad.Get(x => x.Id_ObjetoG == id && x.Mes >= IdMes && x.Mes<=IdMesFin).Sum(x => x.Aprobado).Value;
                 temp.AmpRed = disponibilidad.Get(x => x.Id_ObjetoG == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Ampliaciones).Value;
                 temp.Modificado = temp.Aprobado + temp.AmpRed;
                 temp.Devengado = disponibilidad.Get(x => x.Id_ObjetoG == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Devengado).Value;
                 temp.Pagado = disponibilidad.Get(x => x.Id_ObjetoG == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Pagado).Value;
                 temp.Subejercicio = temp.Modificado - temp.Devengado;
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
                 return File(pdf.GenerarPDF_Horizontal("ConsultaObjetoGasto", models, this.ControllerContext), "application/pdf");
             }
         }
        #endregion

         #region CentroGestor
         public ActionResult V_CentroGestor()
         {
             return View();
         }
         public ActionResult ConsultaCentroGestor(Int32 IdMes, Int32 IdMesFin)
         {

             List<ReporteClavePresupuestariaModel> models = new List<ReporteClavePresupuestariaModel>();
             foreach (Ca_Areas item in areas.Get().OrderBy(x => x.Id_Area))
             {
                 string id = item.Id_Area;
                 ReporteClavePresupuestariaModel temp = new ReporteClavePresupuestariaModel();
                 if (disponibilidad.Get(x => x.Id_Area == id && x.Mes >= IdMes && x.Mes <= IdMesFin).Count() > 0)
                 {
                     temp.Clave = id;
                     temp.Concepto = item.Descripcion;
                     temp.Aprobado = disponibilidad.Get(x => x.Id_Area == id && x.Mes >= IdMes && x.Mes <= IdMesFin).Sum(x => x.Aprobado).Value;
                     temp.AmpRed = disponibilidad.Get(x => x.Id_Area == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Ampliaciones).Value;
                     temp.Modificado = temp.Aprobado + temp.AmpRed;
                     temp.Devengado = disponibilidad.Get(x => x.Id_Area == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Devengado).Value;
                     temp.Pagado = disponibilidad.Get(x => x.Id_Area == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Pagado).Value;
                     temp.Subejercicio = temp.Modificado - temp.Devengado;
                     temp.ultimoNivel = item.UltimoNivel.Value;
                     models.Add(temp);
                 }
                 else
                 {
                     List<Ca_Areas> listaAreas = new List<Ca_Areas>();
                     if (item.Id_UR == 0 && item.Id_UE == 0 && item.UltimoNivel==false)
                     {
                         temp.Clave = id;
                         temp.Concepto = item.Descripcion;
                         temp.ultimoNivel = item.UltimoNivel.Value;
                         listaAreas = areas.Get(x => x.Id_UP == item.Id_UP).ToList();
                         foreach (Ca_Areas itemArea in listaAreas)
                         {
                             temp.Aprobado += disponibilidad.Get(x => x.Id_Area == itemArea.Id_Area && x.Mes >= IdMes && x.Mes <= IdMesFin).Sum(x => x.Aprobado).Value;
                             temp.AmpRed += disponibilidad.Get(x => x.Id_Area == itemArea.Id_Area && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Ampliaciones).Value;
                             temp.Modificado += temp.Aprobado + temp.AmpRed;
                             temp.Devengado += disponibilidad.Get(x => x.Id_Area == itemArea.Id_Area && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Devengado).Value;
                             temp.Pagado += disponibilidad.Get(x => x.Id_Area == itemArea.Id_Area && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Pagado).Value;
                             temp.Subejercicio += temp.Modificado - temp.Devengado;
                         }
                     }
                     else
                     {
                         if (item.Id_UR > 0 &&  item.UltimoNivel == false)
                         {
                             temp.Clave = id;
                             temp.Concepto = item.Descripcion;
                             temp.ultimoNivel = item.UltimoNivel.Value;
                             listaAreas = areas.Get(x => x.Id_UP == item.Id_UP && x.Id_UR == item.Id_UR).ToList();
                             foreach (Ca_Areas itemArea in listaAreas)
                             {
                                 temp.Aprobado += disponibilidad.Get(x => x.Id_Area == itemArea.Id_Area && x.Mes >= IdMes && x.Mes <= IdMesFin).Sum(x => x.Aprobado).Value;
                                 temp.AmpRed += disponibilidad.Get(x => x.Id_Area == itemArea.Id_Area && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Ampliaciones).Value;
                                 temp.Modificado += temp.Aprobado + temp.AmpRed;
                                 temp.Devengado += disponibilidad.Get(x => x.Id_Area == itemArea.Id_Area && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Devengado).Value;
                                 temp.Pagado += disponibilidad.Get(x => x.Id_Area == itemArea.Id_Area && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Pagado).Value;
                                 temp.Subejercicio += temp.Modificado - temp.Devengado;
                             }
                         }
                         else {
                             temp.Clave = id;
                             temp.Concepto = item.Descripcion;
                             temp.Aprobado = 0;
                             temp.AmpRed = 0;
                             temp.Modificado = 0;
                             temp.Devengado = 0;
                             temp.Pagado = 0;
                             temp.Subejercicio = 0;
                             temp.ultimoNivel = item.UltimoNivel.Value;
                         }
                         
                     }
                     models.Add(temp);
                 }

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
                 return File(pdf.GenerarPDF_Horizontal("ConsultaCentroGestor", models, this.ControllerContext), "application/pdf");
             }
         }
         #endregion

         #region Funcion
         public ActionResult V_Funcion()
         {
             return View();
         }
         public ActionResult ConsultaFuncion(Int32 IdMes, Int32 IdMesFin)
         {

             List<ReporteClavePresupuestariaModel> models = new List<ReporteClavePresupuestariaModel>();
             foreach (int item in ReporteClavePresupuestaria.Funcion())
             {
                 string id = item.ToString();
                 ReporteClavePresupuestariaModel temp = new ReporteClavePresupuestariaModel();
                 temp.Clave = item.ToString();
                 temp.Concepto = funcion.GetByID(x => x.Id_Funcion == id).Descripcion;
                 temp.Aprobado = disponibilidad.Get(x => x.Id_Funcion == id && x.Mes >= IdMes && x.Mes <= IdMesFin).Sum(x => x.Aprobado).Value;
                 temp.AmpRed = disponibilidad.Get(x => x.Id_Funcion == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Ampliaciones).Value;
                 temp.Modificado = temp.Aprobado + temp.AmpRed;
                 temp.Devengado = disponibilidad.Get(x => x.Id_Funcion == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Devengado).Value;
                 temp.Pagado = disponibilidad.Get(x => x.Id_Funcion == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Pagado).Value;
                 temp.Subejercicio = temp.Modificado - temp.Devengado;
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
                 return File(pdf.GenerarPDF_Horizontal("ConsultaFuncion", models, this.ControllerContext), "application/pdf");
             }
         }
         #endregion

         #region FuenteFinanciamiento
         public ActionResult V_FuenteFin()
         {
             return View();
         }
         public ActionResult ConsultaFuenteFin(Int32 IdMes, Int32 IdMesFin)
         {

             List<ReporteClavePresupuestariaModel> models = new List<ReporteClavePresupuestariaModel>();
             foreach (Ca_FuentesFin item in fuente.Get().OrderBy(x => x.Id_Fuente))
             {
                 string id = item.Id_Fuente;
                 ReporteClavePresupuestariaModel temp = new ReporteClavePresupuestariaModel();
                 if (disponibilidad.Get(x => x.Id_Fuente == id && x.Mes >= IdMes && x.Mes <= IdMesFin).Count() > 0)
                 {
                     temp.Clave = id;
                     temp.Concepto = item.Descripcion;
                     temp.Aprobado = disponibilidad.Get(x => x.Id_Fuente == id && x.Mes >= IdMes && x.Mes <= IdMesFin).Sum(x => x.Aprobado).Value;
                     temp.AmpRed = disponibilidad.Get(x => x.Id_Fuente == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Ampliaciones).Value;
                     temp.Modificado = temp.Aprobado + temp.AmpRed;
                     temp.Devengado = disponibilidad.Get(x => x.Id_Fuente == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Devengado).Value;
                     temp.Pagado = disponibilidad.Get(x => x.Id_Fuente == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Pagado).Value;
                     temp.Subejercicio = temp.Modificado - temp.Devengado;
                     temp.ultimoNivel = item.UltimoNivel.Value;
                     models.Add(temp);
                 }
                 else
                 {
                     List<Ca_FuentesFin> listaFuentes = new List<Ca_FuentesFin>();
                     if (item.Id_Fuente.Substring(1,3) =="000" && item.UltimoNivel == false)
                     {
                         temp.Clave = id;
                         temp.Concepto = item.Descripcion;
                         temp.ultimoNivel = item.UltimoNivel.Value;
                         listaFuentes = fuente.Get(x => x.Id_Fuente.StartsWith(item.Id_Fuente.Substring(0, 1))).ToList();
                         foreach (Ca_FuentesFin itemArea in listaFuentes)
                         {
                             temp.Aprobado += disponibilidad.Get(x => x.Id_Fuente == itemArea.Id_Fuente && x.Mes >= IdMes && x.Mes <= IdMesFin).Sum(x => x.Aprobado).Value;
                             temp.AmpRed += disponibilidad.Get(x => x.Id_Fuente == itemArea.Id_Fuente && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Ampliaciones).Value;
                             temp.Modificado += temp.Aprobado + temp.AmpRed;
                             temp.Devengado += disponibilidad.Get(x => x.Id_Fuente == itemArea.Id_Fuente && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Devengado).Value;
                             temp.Pagado += disponibilidad.Get(x => x.Id_Fuente == itemArea.Id_Fuente && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Pagado).Value;
                             temp.Subejercicio += temp.Modificado - temp.Devengado;
                         }
                     }
                     else
                     {
                         if (item.Id_Fuente.Substring(2, 2) == "00" && item.UltimoNivel == false)
                         {
                             temp.Clave = id;
                             temp.Concepto = item.Descripcion;
                             temp.ultimoNivel = item.UltimoNivel.Value;
                             listaFuentes = fuente.Get(x => x.Id_Fuente.StartsWith(item.Id_Fuente.Substring(0, 2))).ToList();
                             foreach (Ca_FuentesFin itemArea in listaFuentes)
                             {
                                 temp.Aprobado += disponibilidad.Get(x => x.Id_Fuente == itemArea.Id_Fuente && x.Mes >= IdMes && x.Mes <= IdMesFin).Sum(x => x.Aprobado).Value;
                                 temp.AmpRed += disponibilidad.Get(x => x.Id_Fuente == itemArea.Id_Fuente && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Ampliaciones).Value;
                                 temp.Modificado += temp.Aprobado + temp.AmpRed;
                                 temp.Devengado += disponibilidad.Get(x => x.Id_Fuente == itemArea.Id_Fuente && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Devengado).Value;
                                 temp.Pagado += disponibilidad.Get(x => x.Id_Fuente == itemArea.Id_Fuente && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Pagado).Value;
                                 temp.Subejercicio += temp.Modificado - temp.Devengado;
                             }
                         }
                         else
                         {
                             temp.Clave = id;
                             temp.Concepto = item.Descripcion;
                             temp.Aprobado = 0;
                             temp.AmpRed = 0;
                             temp.Modificado = 0;
                             temp.Devengado = 0;
                             temp.Pagado = 0;
                             temp.Subejercicio = 0;
                             temp.ultimoNivel = item.UltimoNivel.Value;
                         }

                     }
                     models.Add(temp);
                 }

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
                 return File(pdf.GenerarPDF_Horizontal("ConsultaCentroGestor", models, this.ControllerContext), "application/pdf");
             }
         }
         #endregion

         #region Programatica
         public ActionResult V_Programatica()
         {
             return View();
         }
         public ActionResult ConsultaProgramatica(Int32 IdMes, Int32 IdMesFin)
         {

             List<ReporteClavePresupuestariaModel> models = new List<ReporteClavePresupuestariaModel>();
             foreach (Ca_ClasProgramatica item in pragmatica.Get())
             {
                 string id = item.Id_ClasificacionP;
                 ReporteClavePresupuestariaModel temp = new ReporteClavePresupuestariaModel();
                 if (disponibilidad.Get(x => x.Id_ClasificacionP == id && x.Mes >= IdMes && x.Mes <= IdMesFin).Count() > 0)
                 {
                     temp.Clave = item.Id_ClasificacionP;
                     temp.Concepto = item.Descripcion;
                     temp.Aprobado = disponibilidad.Get(x => x.Id_ClasificacionP == id && x.Mes >= IdMes && x.Mes <= IdMesFin).Sum(x => x.Aprobado).Value;
                     temp.AmpRed = disponibilidad.Get(x => x.Id_ClasificacionP == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Ampliaciones).Value;
                     temp.Modificado = temp.Aprobado + temp.AmpRed;
                     temp.Devengado = disponibilidad.Get(x => x.Id_ClasificacionP == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Devengado).Value;
                     temp.Pagado = disponibilidad.Get(x => x.Id_ClasificacionP == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Pagado).Value;
                     temp.Subejercicio = temp.Modificado - temp.Devengado;
                     models.Add(temp);
                 }
                 else
                 {
                     temp.Clave = item.Id_ClasificacionP;
                     temp.Concepto = item.Descripcion;
                     temp.Aprobado = 0;
                     temp.AmpRed = 0;
                     temp.Modificado = 0;
                     temp.Devengado = 0;
                     temp.Pagado = 0;
                     temp.Subejercicio = 0;
                     models.Add(temp);
                 }
                 
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
                 return File(pdf.GenerarPDF_Horizontal("ConsultaProgramatica", models, this.ControllerContext), "application/pdf");
             }
         }
         #endregion

         #region TipoGasto
         public ActionResult V_TipoGasto()
         {
             return View();
         }
         public ActionResult ConsultaTipoGasto(Int32 IdMes, Int32 IdMesFin)
         {

             List<ReporteClavePresupuestariaModel> models = new List<ReporteClavePresupuestariaModel>();
             foreach (Ca_TipoGastos item in tipoGasto.Get())
             {
                 string id = item.Id_TipoGasto;
                 ReporteClavePresupuestariaModel temp = new ReporteClavePresupuestariaModel();
                 if (disponibilidad.Get(x => x.Id_TipoG == id && x.Mes >= IdMes && x.Mes <= IdMesFin).Count() > 0)
                 {
                     temp.Clave = item.Id_TipoGasto;
                     temp.Concepto = item.Descripcion;
                     temp.Aprobado = disponibilidad.Get(x => x.Id_TipoG == id && x.Mes >= IdMes && x.Mes <= IdMesFin).Sum(x => x.Aprobado).Value;
                     temp.AmpRed = disponibilidad.Get(x => x.Id_TipoG == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Ampliaciones).Value;
                     temp.Modificado = temp.Aprobado + temp.AmpRed;
                     temp.Devengado = disponibilidad.Get(x => x.Id_TipoG == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Devengado).Value;
                     temp.Pagado = disponibilidad.Get(x => x.Id_TipoG == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Pagado).Value;
                     temp.Subejercicio = temp.Modificado - temp.Devengado;
                     models.Add(temp);
                 }
                 else
                 {
                     temp.Clave = item.Id_TipoGasto;
                     temp.Concepto = item.Descripcion;
                     temp.Aprobado = 0;
                     temp.AmpRed = 0;
                     temp.Modificado = 0;
                     temp.Devengado = 0;
                     temp.Pagado = 0;
                     temp.Subejercicio = 0;
                     models.Add(temp);
                 }

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
                 return File(pdf.GenerarPDF_Horizontal("ConsultaTipoGasto", models, this.ControllerContext), "application/pdf");
             }
         }
         #endregion

         #region Proyecto
         public ActionResult V_Proyecto()
         {
             return View();
         }
         public ActionResult ConsultaProyecto(Int32 IdMes, Int32 IdMesFin)
         {

             List<ReporteClavePresupuestariaModel> models = new List<ReporteClavePresupuestariaModel>();
             foreach (Ca_Proyecto item in proyecto.Get().OrderBy(x=>x.Id_Proceso))
             {
                 string id = item.Id_Proceso;
                 ReporteClavePresupuestariaModel temp = new ReporteClavePresupuestariaModel();
                 if (disponibilidad.Get(x => x.Id_Proceso == id && x.Mes >= IdMes && x.Mes <= IdMesFin).Count() > 0)
                 {
                     temp.Clave = id;
                     temp.Concepto = item.Descripcion;
                     temp.Aprobado = disponibilidad.Get(x => x.Id_Proceso == id && x.Mes >= IdMes && x.Mes <= IdMesFin).Sum(x => x.Aprobado).Value;
                     temp.AmpRed = disponibilidad.Get(x => x.Id_Proceso == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Ampliaciones).Value;
                     temp.Modificado = temp.Aprobado + temp.AmpRed;
                     temp.Devengado = disponibilidad.Get(x => x.Id_Proceso == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Devengado).Value;
                     temp.Pagado = disponibilidad.Get(x => x.Id_Proceso == id && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Pagado).Value;
                     temp.Subejercicio = temp.Modificado - temp.Devengado;
                     models.Add(temp);
                 }
                 else
                 {
                     temp.Clave = id;
                     temp.Concepto = item.Descripcion;
                     temp.Aprobado = 0;
                     temp.AmpRed = 0;
                     temp.Modificado = 0;
                     temp.Devengado = 0;
                     temp.Pagado = 0;
                     temp.Subejercicio = 0;
                     models.Add(temp);
                 }

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
                 return File(pdf.GenerarPDF_Horizontal("ConsultaTipoGasto", models, this.ControllerContext), "application/pdf");
             }
         }
         #endregion

         #region AccionObra
         public ActionResult V_Accion()
         {
             return View();
         }
         public ActionResult ConsultaAccion(Int32 IdMes, Int32 IdMesFin)
         {

             List<ReporteClavePresupuestariaModel> models = new List<ReporteClavePresupuestariaModel>();
             foreach (Ca_Acciones item in accion.Get().OrderBy(x => x.Id_Proceso))
             {
                 string id = String.Format("{0}-{1}-{2}",item.Id_Proceso,item.Id_ActividadMIR2,item.Id_Accion2);
                 ReporteClavePresupuestariaModel temp = new ReporteClavePresupuestariaModel();
                 if (disponibilidad.Get(x => x.Id_Proceso == item.Id_Proceso && x.Id_Accion==item.Id_Accion2  && x.Id_ActividadMIR==item.Id_ActividadMIR2 && x.Mes >= IdMes && x.Mes <= IdMesFin).Count() > 0)
                 {
                     temp.Clave = id;
                     temp.Concepto = item.Descripcion;
                     temp.Aprobado = disponibilidad.Get(x => x.Id_Proceso == item.Id_Proceso && x.Id_Accion == item.Id_Accion2 && x.Id_ActividadMIR == item.Id_ActividadMIR2 && x.Mes >= IdMes && x.Mes <= IdMesFin).Sum(x => x.Aprobado).Value;
                     temp.AmpRed = disponibilidad.Get(x => x.Id_Proceso == item.Id_Proceso && x.Id_Accion == item.Id_Accion2 && x.Id_ActividadMIR == item.Id_ActividadMIR2 && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Ampliaciones).Value;
                     temp.Modificado = temp.Aprobado + temp.AmpRed;
                     temp.Devengado = disponibilidad.Get(x => x.Id_Proceso == item.Id_Proceso && x.Id_Accion == item.Id_Accion2 && x.Id_ActividadMIR == item.Id_ActividadMIR2 && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Devengado).Value;
                     temp.Pagado = disponibilidad.Get(x => x.Id_Proceso == item.Id_Proceso && x.Id_Accion == item.Id_Accion2 && x.Id_ActividadMIR == item.Id_ActividadMIR2 && x.Mes == IdMes && x.Mes <= IdMesFin).Sum(x => x.Pagado).Value;
                     temp.Subejercicio = temp.Modificado - temp.Devengado;
                     models.Add(temp);
                 }
                 else
                 {
                     temp.Clave = id;
                     temp.Concepto = item.Descripcion;
                     temp.Aprobado = 0;
                     temp.AmpRed = 0;
                     temp.Modificado = 0;
                     temp.Devengado = 0;
                     temp.Pagado = 0;
                     temp.Subejercicio = 0;
                     models.Add(temp);
                 }

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
                 return File(pdf.GenerarPDF_Horizontal("ConsultaAccion", models, this.ControllerContext), "application/pdf");
             }
         }
         #endregion

    }
}
