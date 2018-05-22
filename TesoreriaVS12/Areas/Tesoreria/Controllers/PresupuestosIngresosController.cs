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
    public class PresupuestosIngresosController : Controller
    {
        private MaPresupuestoIngDAL DALPresupuestoIng;
        private ConvertHtmlToString reports { get; set; }
        protected DeEvolucionDAL DALEvolucion { get; set; }
        protected EvolucionAnualDAL DALvEvolucion { get; set; }
        protected CierreMensualDAL cierreMensualDAL { get; set; }

        public PresupuestosIngresosController()
        {
            DALPresupuestoIng = new MaPresupuestoIngDAL();
            if (reports == null) reports = new ConvertHtmlToString();
            if (DALEvolucion == null) DALEvolucion = new DeEvolucionDAL();
            if (DALvEvolucion == null) DALvEvolucion = new EvolucionAnualDAL();
            if (cierreMensualDAL == null) cierreMensualDAL = new CierreMensualDAL();
        }
        public ActionResult Index()
        {
            ViewBag.Ejercicio = new ParametrosDAL().GetByID(x => x.Descripcion == "Ejercicio contable").Valor;
            return View(new Ma_PresupuestoIngModel());
        }

        public ActionResult GetDescripcion(string id, string objeto, string parametros, string tipo)
        {
            ClaseJson jsonResult = new PresupuestosIngresos().GetDescripcion(id, objeto, parametros, tipo);
            return Json(new { Data = jsonResult.Descripcion,Ids= jsonResult.Ids }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarCRI(String Id_Concepto)
        {
            if (new ConceptosIngresosDAL().GetByID(x => x.Id_Concepto == Id_Concepto) != null)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GuardarPresupuesto(Ma_PresupuestoIngModel presupuesto)
        {
            if (ViewData.ModelState.IsValid)
            {
                try
                {
                    UsuarioLogueado usuario= Session["appUsuario"] as UsuarioLogueado;
                    DateTime Fecha_Estimado = Convert.ToDateTime(presupuesto.Fecha_Estimado.Value);
                    int fecha = Convert.ToInt16(Fecha_Estimado.Year);
                    ParametrosDAL pDal = new ParametrosDAL();
                    if (fecha == Convert.ToInt32(pDal.GetByID(x => x.Nombre == "Ejercicio").Valor))
                    {
                        CierreMensualDAL cierreDal = new CierreMensualDAL();
                        int mes = Convert.ToInt16(Fecha_Estimado.Month);
                        if (!new CompromisosBL().isClosed(presupuesto.Fecha_Estimado.Value))
                        {
                            Ma_PresupuestoIng p = EntityFactory.getEntity<Ma_PresupuestoIng>(presupuesto, new Ma_PresupuestoIng());
                            p.AnioFin = presupuesto.AnioFin;
                            p.Id_ClavePresupuesto = StringID.IdClavePresupuestoIngreso(p.Id_CentroRecaudador.Trim(), p.Id_Fuente.Trim(), p.AnioFin.Trim(), p.Id_Alcance.Trim(), p.Id_Concepto.Trim());
                            p.Fecha_Estimado = Fecha_Estimado;
                            p.Fecha_act = DateTime.Now;
                            p.Usu_Act = (short)usuario.IdUsuario;
                            if (p.Total > 0)
                            {
                                byte Id_MesPol = 0;
                                int Id_FolioPol = 0;
                                new ProceduresDAL().Pa_Genera_PolizaOrden_Estimado(p.Id_Concepto.Trim(), p.Total, p.Fecha_Estimado.Value, ref Id_MesPol, ref Id_FolioPol, (short)usuario.IdUsuario);
                                p.Id_MesPO_Estimado = Id_MesPol;
                                p.Id_FolioPO_Estimado = Id_FolioPol;
                            }
                            DALPresupuestoIng.Insert(p);
                            DALPresupuestoIng.Save();
                            return Json(new { Exito = true, Mensaje = "Presupuesto agregado correctamente", Poliza = StringID.Polizas(p.Id_FolioPO_Estimado, p.Id_MesPO_Estimado), Id_ClavePresupuesto = p.Id_ClavePresupuesto, Id_MesPO_Estimado = p.Id_MesPO_Estimado, Id_FolioPO_Estimado = p.Id_FolioPO_Estimado });
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
                catch (Exception ex)
                {
                    return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
                }
            }
            return Json(new { Exito = false, Mensaje = "Modelo Inválido" });
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

        public JsonResult ValidarId_Area(string Id_CentroRecaudador)
        {
            if (new CentroRecaudadorDAL().GetByID(x => x.Id_CRecaudador == Id_CentroRecaudador) != null)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidarFuenteFin(string Id_Fuente)
        {
            if (new FuenteIngDAL().GetByID(x => x.Id_FuenteFinancia== Id_Fuente) != null)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidarDimenGeo(string Id_Alcance)
        {
            if (new AlcanceDAL().GetByID(x => x.Id_AlcanceGeo == Id_Alcance) != null)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarCatalogo(string tipo, string label, string id)
        {
            ViewBag.Tipo = tipo;
            ViewBag.Label = label;
            ViewBag.Id = id;
            return View("BuscarCatalogo");
        }

        public ActionResult Buscar(string objeto, string clave, string descripcion, int tipoBusqueda = 0)
        {
            return View(PresupuestosIngresos.ListaIngresos(clave, descripcion, objeto,tipoBusqueda));
        }

        public ActionResult ModalPresupuestoIng()
        {
            return View();
        }

        public ActionResult BuscarPresupuestos(ModalPresupuestoIngModel presupuesto)
        {
            List<Ma_PresupuestoIng> lista = new List<Ma_PresupuestoIng>();
            lista = PresupuestosIngresos.BuscarPresupuesto(presupuesto);
            return View(lista);
        }

        public JsonResult GetPresupuesto(String Id_ClavePresupuesto)
        {
            try
            {
                /*Ma_PresupuestoIng presupuesto = DALPresupuestoIng.GetByID(x => x.Id_ClavePresupuesto == Id_ClavePresupuesto.Trim());
                Ma_PresupuestoIngModel presupuestoModel = ModelFactory.getModel<Ma_PresupuestoIngModel>(presupuesto, new Ma_PresupuestoIngModel());
                presupuestoModel.FolioPoliza = StringID.Polizas(presupuestoModel.Id_FolioPO_Estimado, presupuestoModel.Id_MesPO_Estimado);*/
                return Json(new { Exito = true, Presupuesto = new Llenado().LLenado_MaPresupuestoING(Id_ClavePresupuesto) });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult CancelarPresupuesto(string Id_ClavePresupuesto, string Fecha)
        {
            try
            {
                DateTime f = Convert.ToDateTime(Fecha);
                Ca_CierreMensual cierre = cierreMensualDAL.GetByID(x => x.Id_Mes == f.Month);
                if (!cierre.Contable.Value)
                {
                    Ma_PresupuestoIng presupuesto = DALPresupuestoIng.GetByID(x => x.Id_ClavePresupuesto == Id_ClavePresupuesto.Trim());
                    presupuesto.Fecha_CancelaEstimado = f;
                    DALPresupuestoIng.Update(presupuesto);
                    DALPresupuestoIng.Save();
                    DALPresupuestoIng.Delete(x => x.Id_ClavePresupuesto == Id_ClavePresupuesto);
                    DALPresupuestoIng.Save();
                    return Json(new { Exito = true, Mensaje = "Presupuesto de Ingreso eliminado correctamente" });
                }
                return Json(new { Exito = true, Mensaje = "El Presupuestos de Ingreso no puede ser eliminado debido a que el mes se encuentra cerrado" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        public ActionResult ModalEliminar()
        {
            return View();
        }

        #region Reportes
        public ActionResult PresupuestoEstimado()
        {
            List<Ma_PresupuestoIng> entities = DALPresupuestoIng.Get().ToList();
            List<Ma_PresupuestoIngModel> models = new List<Ma_PresupuestoIngModel>();
            entities.ForEach(item => { models.Add(ModelFactory.getModel<Ma_PresupuestoIngModel>(item, new Ma_PresupuestoIngModel())); });
            models = models.OrderBy(reg => reg.Id_ClavePresupuesto).ToList();
            return File(reports.GenerarPDF_Horizontal("PresupuestoEstimado", models, this.ControllerContext), "Application/PDF");
            //return View(models);
        }

        public ActionResult Evolucion()
        {
            Modelos.EvolucionModel model = new Modelos.EvolucionModel();
            Dictionary<int, string> meses = Diccionarios.Meses;
            if (meses.Count < 13)
                meses.Add(13, "ANUAL");

            model.Lista_Meses = new SelectList(Diccionarios.Meses, "Key", "Value", 13);
            return View(model);
        }

        [HttpPost]
        public ActionResult tblEvolucion(byte Id_Mes)
        {
            List<DE_EvolucionModel> models = new List<DE_EvolucionModel>();            
            if (Id_Mes == 13)
                DALvEvolucion.Get().ToList().ForEach(item => { models.Add(ModelFactory.getModel<DE_EvolucionModel>(item, new DE_EvolucionModel())); });
            else
                DALEvolucion.Get(reg => reg.Mes == Id_Mes).ToList().ForEach(item => { models.Add(ModelFactory.getModel<DE_EvolucionModel>(item, new DE_EvolucionModel())); });

            models = models.OrderBy(reg => reg.Id_ClavePresupuesto).ToList();
            return View(models);
        }

        [HttpGet]
        public ActionResult rptEvolucion(byte Id_Mes)
        {
            List<DE_EvolucionModel> models = new List<DE_EvolucionModel>();
            if (Id_Mes == 13)
            {
                DALvEvolucion.Get().ToList().ForEach(item => { models.Add(ModelFactory.getModel<DE_EvolucionModel>(item, new DE_EvolucionModel())); });
                ViewBag.TituloMes = "DEL AÑO COMPLETO";
            }
            else
            {
                DALEvolucion.Get(reg => reg.Mes == Id_Mes).ToList().ForEach(item => { models.Add(ModelFactory.getModel<DE_EvolucionModel>(item, new DE_EvolucionModel())); });
                ViewBag.TituloMes = String.Format("DEL MES DE {0}", Diccionarios.Meses[Id_Mes]);
            }

            models = models.OrderBy(reg => reg.Id_ClavePresupuesto).ToList();
            return File(reports.GenerarPDF("rptEvolucion", models, this.ControllerContext), "Application/PDF");
        }
        #endregion
    }
}
