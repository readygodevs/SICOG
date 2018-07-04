using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Utils;
using TesoreriaVS12.Models;
using TesoreriaVS12.Filters;
using System.Web.Script.Serialization;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.DAL;
using TesoreriaVS12.Areas.Tesoreria.BL;
using System.Configuration;
using System.IO;

namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    [AuthorizeLogin]
    public class CatalogosController : Controller
    {
        protected AccionDAL accion { get; set; }
        protected ActividadMIRDAL actividad { get; set; }
        protected ActividadDAL actividadins { get; set; }
        protected AlcanceDAL alcancegeo { get; set; }
        protected AreasDAL areas { get; set; }
        protected BancosDAL bancos { get; set; }
        protected BancosRHDAL bancosrh { get; set; }
        protected BeneficiariosDAL beneficiarios { get; set; }
        protected BeneficiariosCuentasDAL beneficiarioscuentas { get; set; }
        protected CallesDAL calles { get; set; }
        protected CausasCancelacionDAL causascancelacion { get; set; }
        protected CentroRecaudadorDAL CentroRecaudadorDAL { get; set; }
        protected CierreBancoDAL cierrebanco { get; set; }
        protected CierreMensualDAL cierremensual { get; set; }
        protected CaCajasReceptorasDAL CajasReceptoras { get; set; }
        protected ClasificaBeneficiariosDAL clasificacionbeneficiario { get; set; }
        protected ClasificaPolizaDAL clasificacionpolizas { get; set; }
        protected ClasProgramaticaDAL clasprogramatica { get; set; }
        protected ColoniasDAL colonias { get; set; }
        protected ConceptosIngresosDAL conceptoingresos { get; set; }
        protected CuentasDAL cuentas { get; set; }
        protected CuentasBancariasDAL cuentasbancarias { get; set; }
        protected Ca_CURDAL CUR { get; set; }
        protected DeBancoDAL debanco { get; set; }
        protected DeBancoChequeDAL debancocheque { get; set; }
        protected DeCRFacturasDAL crFacturas { get; set; }
        protected DiasInhabilesDAL diasInabil { get; set; }
        protected EstadosDAL estados { get; set; }
        protected FirmasDAL firmas { get; set; }
        protected FuenteDAL fuentesfin { get; set; }
        protected FuenteIngDAL fuentesfinIng { get; set; }
        protected FuncionDAL funciones { get; set; }
        protected ImpuestosDeduccionDAL impuestodeduccion { get; set; }
        protected LocalidadesDAL localidades { get; set; }
        protected MunicipiosDAL municipios { get; set; }
        protected ObjetoGDAL objetogasto { get; set; }
        protected PaisesDAL paises { get; set; }
        protected ParametrosDAL parametros { get; set; }
        protected PercepDeducDAL percepdeduc { get; set; }
        protected ProgramaDAL programas { get; set; }
        protected ProcesoDAL proyecto { get; set; }
        protected TipoBeneficiariosDAL tipobeneficiario { get; set; }
        protected TipoCompromisosDAL tipocompromiso { get; set; }
        protected TipoContrarecibosDAL tipocontrarecibo { get; set; }
        protected TipoDoctosDAL tipodoctos { get; set; }
        protected TipoFormatoChequesDAL tipoformatocheques { get; set; }
        protected TipoGastosDAL tipogasto { get; set; }
        protected TipoImpuestosDAL tipoimpuesto { get; set; }
        //protected TipoIngresosDAL tipoingresos { get; set; }
        protected TipoMetaDAL tipometa { get; set; }
        protected TipoMovBancariosDAL tipomovbancarios { get; set; }
        protected TipoPagosDAL tipopagos { get; set; }
        protected TipoPolizasDAL tipopolizas { get; set; }
        protected TipoTrasferenciasEgDAL tipotransferenciaseg { get; set; }
        protected TipoTransferenciasIngDAL tipotrasnferenciasing { get; set; }
        protected DePolizasDAL depolizas { get; set; }
        protected MaPolizasDAL mapolizas { get; set; }
        protected GirosDAL giros { get; set; }
        protected DeBeneficiariosGirosDAL debeneficiariosgiros { get; set; }
        protected DeBeneficiariosContactosDAL debeneficiarioscontactos { get; set; }
        protected PersonasDAL personas { get; set; }
        protected MaRecibosDAL MaRecibosDAL { get; set; }




        public CatalogosController()
        {
            if (accion == null) accion = new AccionDAL();
            if (actividad == null) actividad = new ActividadMIRDAL();
            if (actividadins == null) actividadins = new ActividadDAL();
            if (alcancegeo == null) alcancegeo = new AlcanceDAL();
            if (areas == null) areas = new AreasDAL();
            if (bancos == null) bancos = new BancosDAL();
            if (bancosrh == null) bancosrh = new BancosRHDAL();
            if (beneficiarios == null) beneficiarios = new BeneficiariosDAL();
            if (beneficiarioscuentas == null) beneficiarioscuentas = new BeneficiariosCuentasDAL();
            if (calles == null) calles = new CallesDAL();
            if (causascancelacion == null) causascancelacion = new CausasCancelacionDAL();
            if (CajasReceptoras == null) CajasReceptoras = new CaCajasReceptorasDAL();
            if (CentroRecaudadorDAL == null) CentroRecaudadorDAL = new CentroRecaudadorDAL();
            if (cierrebanco == null) cierrebanco = new CierreBancoDAL();
            if (cierremensual == null) cierremensual = new CierreMensualDAL();
            if (clasificacionbeneficiario == null) clasificacionbeneficiario = new ClasificaBeneficiariosDAL();
            if (clasificacionpolizas == null) clasificacionpolizas = new ClasificaPolizaDAL();
            if (clasprogramatica == null) clasprogramatica = new ClasProgramaticaDAL();
            if (colonias == null) colonias = new ColoniasDAL();
            if (conceptoingresos == null) conceptoingresos = new ConceptosIngresosDAL();
            if (cuentas == null) cuentas = new CuentasDAL();
            if (cuentasbancarias == null) cuentasbancarias = new CuentasBancariasDAL();
            if (CUR == null) CUR = new Ca_CURDAL();
            if (debanco == null) debanco = new DeBancoDAL();
            if (debancocheque == null) debancocheque = new DeBancoChequeDAL();
            if (diasInabil == null) diasInabil = new DiasInhabilesDAL();
            if (crFacturas == null) crFacturas = new DeCRFacturasDAL();
            if (estados == null) estados = new EstadosDAL();
            if (firmas == null) firmas = new FirmasDAL();
            if (fuentesfin == null) fuentesfin = new FuenteDAL();
            if (fuentesfinIng == null) fuentesfinIng = new FuenteIngDAL();
            if (funciones == null) funciones = new FuncionDAL();
            if (impuestodeduccion == null) impuestodeduccion = new ImpuestosDeduccionDAL();
            if (localidades == null) localidades = new LocalidadesDAL();
            if (municipios == null) municipios = new MunicipiosDAL();
            if (objetogasto == null) objetogasto = new ObjetoGDAL();
            if (paises == null) paises = new PaisesDAL();
            if (parametros == null) parametros = new ParametrosDAL();
            if (percepdeduc == null) percepdeduc = new PercepDeducDAL();
            if (programas == null) programas = new ProgramaDAL();
            if (proyecto == null) proyecto = new ProcesoDAL();
            if (tipobeneficiario == null) tipobeneficiario = new TipoBeneficiariosDAL();
            if (tipocompromiso == null) tipocompromiso = new TipoCompromisosDAL();
            if (tipocontrarecibo == null) tipocontrarecibo = new TipoContrarecibosDAL();
            if (tipodoctos == null) tipodoctos = new TipoDoctosDAL();
            if (tipoformatocheques == null) tipoformatocheques = new TipoFormatoChequesDAL();
            if (tipogasto == null) tipogasto = new TipoGastosDAL();
            if (tipoimpuesto == null) tipoimpuesto = new TipoImpuestosDAL();
            //if (tipoingresos == null) tipoingresos = new TipoIngresosDAL();
            if (tipometa == null) tipometa = new TipoMetaDAL();
            if (tipomovbancarios == null) tipomovbancarios = new TipoMovBancariosDAL();
            if (tipopagos == null) tipopagos = new TipoPagosDAL();
            if (tipopolizas == null) tipopolizas = new TipoPolizasDAL();
            if (tipotransferenciaseg == null) tipotransferenciaseg = new TipoTrasferenciasEgDAL();
            if (tipotrasnferenciasing == null) tipotrasnferenciasing = new TipoTransferenciasIngDAL();
            if (mapolizas == null) mapolizas = new MaPolizasDAL();
            if (depolizas == null) depolizas = new DePolizasDAL();
            if (giros == null) giros = new GirosDAL();
            if (debeneficiarioscontactos == null) debeneficiarioscontactos = new DeBeneficiariosContactosDAL();
            if (personas == null) personas = new PersonasDAL();
            if (debeneficiariosgiros == null) debeneficiariosgiros = new DeBeneficiariosGirosDAL();
            if (MaRecibosDAL == null) MaRecibosDAL = new MaRecibosDAL();
        }

        #region Ca_Acciones

        public ActionResult V_Acciones()
        {
            List<Ca_AccionesModel> accLst = new List<Ca_AccionesModel>();
            IEnumerable<Ca_Acciones> entities = accion.Get();
            foreach (Ca_Acciones item in entities)
            {
                Ca_AccionesModel model = ModelFactory.getModel<Ca_AccionesModel>(item, new Ca_AccionesModel());
                model.CA_Proyecto = ModelFactory.getModel<Ca_ProyectoModel>(proyecto.GetByID(x => x.Id_Proceso == model.Id_Proceso), new Ca_ProyectoModel());
                model.CA_Actividad = ModelFactory.getModel<Ca_ActividadModel>(actividad.GetByID(x => x.Id_ActividadMIR2 == model.Id_ActividadMIR2 && x.Id_Proceso == model.Id_Proceso), new Ca_ActividadModel());
                accLst.Add(model);
            }
            return View(accLst);
        }
        public ActionResult V_AccionDetalles(String IdProceso, string IdActividadMIR2, short Id)
        {
            Ca_AccionesModel model = ModelFactory.getModel<Ca_AccionesModel>(accion.GetByID(x => x.Id_Accion == Id && x.Id_Proceso == IdProceso && x.Id_ActividadMIR2 == IdActividadMIR2), new Ca_AccionesModel());
            model.CA_Proyecto = ModelFactory.getModel<Ca_ProyectoModel>(proyecto.GetByID(x => x.Id_Proceso == model.Id_Proceso), new Ca_ProyectoModel());
            model.CA_Actividad = ModelFactory.getModel<Ca_ActividadModel>(actividad.GetByID(x => x.Id_ActividadMIR2 == model.Id_ActividadMIR2 && x.Id_Proceso == model.Id_Proceso), new Ca_ActividadModel());
            return View(model);
        }
        [HttpGet]
        public ActionResult AgregarAccion()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AgregarAccion(Ca_AccionesModel accion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    accion.Id_Accion2 = accion.Id_Accion2.PadLeft(3, '0');
                    AccionDAL DALAccion = new AccionDAL();
                    if (DALAccion.GetByID(x => x.Id_Accion2 == accion.Id_Accion2 && x.Id_Proceso == accion.Id_Proceso && x.Id_ActividadMIR2 == accion.Id_ActividadMIR2) != null)
                        return Json(new { Exito = false, Mensaje = "La Clave de la Acción/Obra ya existe, intente con otro" });
                    UsuarioLogueado usuario = Session["appUsuario"] as UsuarioLogueado;
                    accion.Usu_Act = (short)usuario.IdUsuario;
                    accion.Fecha_Act = DateTime.Now;
                    accion.CA_Actividad = new Llenado().LLenado_CaActividad(accion.Id_Proceso, accion.Id_ActividadMIR2);
                    accion.CA_Proyecto = new Llenado().LLenado_CaProyecto(accion.Id_Proceso);
                    accion.Id_Accion = new AccionesBL().nextIdAccion(accion.Id_Proceso, accion.Id_ActividadMIR2);
                    DALAccion.Insert(EntityFactory.getEntity<Ca_Acciones>(accion, new Ca_Acciones()));
                    DALAccion.Save();
                    return Json(new { Exito = true, Mensaje = "Acción registrada correctamente", Registro = accion });
                }
                catch (Exception ex)
                {
                    //return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
                    return Json(new { Exito = false, Mensaje = "Datos incorrectos" });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Datos incorrectos" });
            }

        }
        [HttpGet]
        public ActionResult EditarAccion(String IdProceso, string IdActividadMIR2, short Id)
        {
            Ca_AccionesModel model = ModelFactory.getModel<Ca_AccionesModel>(accion.GetByID(x => x.Id_Accion == Id && x.Id_Proceso == IdProceso && x.Id_ActividadMIR2 == IdActividadMIR2), new Ca_AccionesModel());
            model.CA_Proyecto = ModelFactory.getModel<Ca_ProyectoModel>(proyecto.GetByID(x => x.Id_Proceso == model.Id_Proceso), new Ca_ProyectoModel());
            model.CA_Actividad = ModelFactory.getModel<Ca_ActividadModel>(actividad.GetByID(x => x.Id_ActividadMIR2 == model.Id_ActividadMIR2 && x.Id_Proceso == model.Id_Proceso), new Ca_ActividadModel());
            return View(model);
        }
        [HttpPost]
        public ActionResult EditarAccion(Ca_AccionesModel accion)
        {
            try
            {
                AccionDAL DALAccion = new AccionDAL();
                DALAccion.Update(EntityFactory.getEntity<Ca_Acciones>(accion, new Ca_Acciones()));
                DALAccion.Save();
                accion.CA_Actividad = new Llenado().LLenado_CaActividad(accion.Id_Proceso, accion.Id_ActividadMIR2);
                accion.CA_Proyecto = new Llenado().LLenado_CaProyecto(accion.Id_Proceso);
                return Json(new { Exito = true, Mensaje = "Registro actualizado con éxito", Registro = accion });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        public ActionResult EliminarAccion(String IdProceso, string IdActividadMIR2, short Id)
        {
            try
            {
                AccionDAL DALAccion = new AccionDAL();
                Ca_Acciones accion = DALAccion.GetByID(x => x.Id_Proceso == IdProceso && x.Id_ActividadMIR2 == IdActividadMIR2 && x.Id_Accion == Id);
                if (accion != null)
                {
                    DALAccion.Delete(x => x.Id_Proceso == IdProceso && x.Id_ActividadMIR2 == IdActividadMIR2 && x.Id_Accion == Id);
                    DALAccion.Save();
                    return Json(new { Exito = true, Mensaje = "Acción u Obra eliminada con éxito" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Exito = false, Mensaje = "Acción no encontrada" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = "La Acción u Obra no se puede eliminar debido a que ya fue utilizada" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Ca_Actividad
        public ActionResult V_Actividades()
        {
            List<Ca_ActividadModel> Lst = new List<Ca_ActividadModel>();
            IEnumerable<Ca_Actividad> entities = actividad.Get();
            foreach (Ca_Actividad item in entities)
            {
                Ca_ActividadModel model = ModelFactory.getModel<Ca_ActividadModel>(item, new Ca_ActividadModel());
                model.CA_Proyecto = ModelFactory.getModel<Ca_ProyectoModel>(proyecto.GetByID(x => x.Id_Proceso == model.Id_Proceso), new Ca_ProyectoModel());
                Lst.Add(model);
            }
            return View(Lst);
        }
        public ActionResult V_AgregarActividad()
        {
            ViewBag.proyecto = proyecto.Get().ToList();
            return View(new Ca_ActividadModel());
        }

        public ActionResult V_ObtenerActividad(Int16 Id)
        {
            ViewBag.proyecto = proyecto.Get().ToList();
            if (Id != 0)
                return View(ModelFactory.getModel<Ca_ActividadModel>(actividad.GetByID(x => x.Id_ActividadMIR == Id), new Ca_ActividadModel()));
            return View(new Ca_ActividadModel());
        }

        [HttpPost]
        public ActionResult EditarActividad(Ca_Actividad dataModel)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                dataModel.Usu_Act = (Int16)appUsuario.IdUsuario;
                dataModel.Fecha_Act = DateTime.Now;
                actividad.Update(dataModel);
                actividad.Save();
                return Json(new { Exito = true, Mensaje = "Registro guardado correctamente", Registro = dataModel });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult AgregarActividad(Ca_Actividad dataModel)
        {
            try
            {
                Ca_ActividadModel model = new Ca_ActividadModel();
                string msn = "";
                if (model.Valida(dataModel.Id_ActividadMIR, dataModel.Descripcion, ref msn) == true)
                {
                    UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                    dataModel.Usu_Act = (Int16)appUsuario.IdUsuario;
                    dataModel.Descripcion = dataModel.Descripcion.Trim();
                    dataModel.Fecha_Act = DateTime.Now;
                    dataModel.Id_ActividadMIR2 = StringID.IdActividadMIR(dataModel.Id_ActividadMIR);
                    actividad.Insert(dataModel);
                    actividad.Save();
                    return Json(new { Exito = true, Mensaje = "Registro guardado correctamente.", Registro = dataModel });
                }
                else
                    return Json(new { Exito = false, Mensaje = msn });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }

        public ActionResult EliminarActividad(Int16 Id)
        {
            try
            {
                actividad.Delete(x => x.Id_ActividadMIR == Id);
                actividad.Save();
                return Json(new { Exito = true, Mensaje = "Registro eliminado correctamente." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult V_DetallesActividad(string Proceso, string Id)
        {
            return View(new Llenado().LLenado_CaActividad(Proceso, Id));
        }
        #endregion

        //(COMPROMISOS)
        #region ActividadesInst
        //LISTA DE COMPROMISOS
        public ActionResult V_Compromisos()
        {
            List<Ca_ActividadesInstModel> actividadesLst = new List<Ca_ActividadesInstModel>();
            actividadins.Get().ToList().ForEach(x => { actividadesLst.Add(ModelFactory.getModel<Ca_ActividadesInstModel>(x, new Ca_ActividadesInstModel())); });
            return View(actividadesLst);
        }
        //MODAL PARA OBTENER MODELO A EDITAR O GUARDAR
        public ActionResult V_AgregarCompromiso(string IdCompromiso, int nuevo)
        {
            //Session["nuevo"] = nuevo;
            ViewBag.nuevo = nuevo;
            if (IdCompromiso != null)
            {
                return View(ModelFactory.getModel<Ca_ActividadesInstModel>(actividadins.GetByID(x => x.Id_Actividad == IdCompromiso), new Ca_ActividadesInstModel()));//MODELO A EDITAR
            }
            return View(new Ca_ActividadesInstModel());//MODELO NUEVO
        }
        //METODO PARA GUARDAR UNA FUNCION
        [HttpPost]
        public ActionResult GuardarCompromiso(Ca_ActividadesInst dataModel)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                Ca_ActividadesInstModel model = new Ca_ActividadesInstModel();
                bool Nuevo = true;
                string msn = "";
                string id = StringID.IdActividadInst(Convert.ToByte(dataModel.Id_Actividad));
                if (model.Valida(id, dataModel.Descripcion, ref msn) == true)
                {
                    dataModel.Id_Actividad = id;
                    dataModel.Usu_Act = (Int16)appUsuario.IdUsuario;
                    dataModel.Fecha_Act = DateTime.Now;
                    actividadins.Insert(dataModel);
                    actividadins.Save();
                    Nuevo = true;
                    return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Nuevo = Nuevo, Registro = dataModel });
                }
                else
                    return Json(new { Exito = false, Mensaje = msn });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult ModificarCompromiso(Ca_ActividadesInst dataModel)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                Ca_ActividadesInstModel model = new Ca_ActividadesInstModel();
                bool Nuevo = false;
                string msn = "";
                //MODELO A EDITAR
                if (model.ValidaDesc(dataModel.Descripcion, ref msn))
                {
                    dataModel.Usu_Act = (Int16)appUsuario.IdUsuario;
                    dataModel.Fecha_Act = DateTime.Now;
                    actividadins.Update(dataModel);
                    actividadins.Save();
                }
                else
                    return Json(new { Exito = false, Mensaje = msn });

                return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Nuevo = Nuevo, Registro = dataModel });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = ex.Message });
            }
        }
        //METODO PARA ELIMINAR UNA FUNCION
        public ActionResult EliminarCompromiso(string IdCompromiso)
        {
            try
            {
                actividadins.Delete(m => m.Id_Actividad == IdCompromiso);
                actividadins.Save();
                return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente" });

            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = "Ocurrio un Error " + ex });
            }
        }
        //DETALLES DE UNA FUNCION
        public ActionResult V_DetallesCompromiso(string IdCompromiso)
        {
            return View(ModelFactory.getModel<Ca_ActividadesInstModel>(actividadins.GetByID(x => x.Id_Actividad == IdCompromiso), new Ca_ActividadesInstModel()));
        }
        #endregion

        #region Ca_AlcanceGeo
        public ActionResult V_AlcanceGeo()
        {
            List<Ca_AlcanceGeoModel> Lst = new List<Ca_AlcanceGeoModel>();
            alcancegeo.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_AlcanceGeoModel>(x, new Ca_AlcanceGeoModel())); });
            return View(Lst);
        }
        public ActionResult V_AlcanceGeoDetalles(string Id)
        {
            return View(ModelFactory.getModel<Ca_AlcanceGeoModel>(alcancegeo.GetByID(x => x.Id_AlcanceGeo == Id), new Ca_AlcanceGeoModel()));
        }
        #endregion

        #region Ca_Almacenes

        #endregion

        #region Ca_Areas
        public ActionResult V_Areas()
        {
            List<Ca_AreasModel> areLst = new List<Ca_AreasModel>();
            areas.Get().ToList().ForEach(x => { areLst.Add(ModelFactory.getModel<Ca_AreasModel>(x, new Ca_AreasModel())); });
            return View(areLst);
        }

        public ActionResult V_Areas_Detalles(string Id)
        {
            return View(ModelFactory.getModel<Ca_AreasModel>(areas.GetByID(x => x.Id_Area == Id), new Ca_AreasModel()));
        }

        public ActionResult V_Areas_Agregar()
        {
            return View(new Ca_AreasModel());
        }

        [HttpPost]
        public ActionResult V_Areas_Agregar(Ca_Areas modelo)
        {
            string debug = "0";
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            Ca_AreasModel model = new Ca_AreasModel();
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    if (model.ValidaAdd(modelo.Id_UP, modelo.Id_UR, modelo.Id_UE, modelo.Descripcion, modelo.UltimoNivel, ref msn) == true)
                    {
                        debug = "1";
                        modelo.Id_Area = StringID.IdArea(modelo.Id_UP, modelo.Id_UR, modelo.Id_UE);
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                        modelo.Fecha_Act = DateTime.Now;
                        if (modelo.Id_UP > 0 && modelo.Id_UR > 0 && modelo.Id_UE > 0)
                            modelo.UltimoNivel = true;
                        else
                            modelo.UltimoNivel = false;
                        debug = "2";
                        areas.Insert(modelo);
                        debug = "3";
                        areas.Save();
                        debug = "4";
                        return Json(new { Exito = true, Mensaje = "Registro guardado exitosamente", Datos = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    return Json(new { Exito = false, Mensaje = "Debug:" + debug + "," + StringID.Exceptions + ex });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información no válida" });
            }
        }

        public ActionResult V_Areas_Editar(string Id)
        {
            return View(ModelFactory.getModel<Ca_AreasModel>(areas.GetByID(x => x.Id_Area == Id), new Ca_AreasModel()));
        }

        [HttpPost]
        public ActionResult V_Areas_Editar(Ca_Areas modelo)
        {
            Ca_AreasModel model = new Ca_AreasModel();
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    if (model.ValidaEdit(modelo.Id_UP, modelo.Id_UR, modelo.Id_UE, modelo.Descripcion, modelo.UltimoNivel, ref msn) == true)
                    {
                        modelo.Fecha_Act = DateTime.Now;
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                        if (modelo.Id_UP > 0 && modelo.Id_UR > 0 && modelo.Id_UE > 0) modelo.UltimoNivel = true;
                        areas.Update(modelo);
                        areas.Save();
                        return Json(new { Exito = true, Mensaje = "Registro modificado exitosamente", Datos = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información no válida" });
            }
        }
        [AuthorizeLogin]
        public ActionResult V_Areas_Eliminar(string Id)
        {
            try
            {
                Ca_AreasModel model = new Ca_AreasModel();
                string msn = "";
                Ca_AreasModel modelo = ModelFactory.getModel<Ca_AreasModel>(areas.GetByID(x => x.Id_Area == Id), new Ca_AreasModel());
                if (model.ValidaDelete(modelo.Id_UP, modelo.Id_UR, modelo.Id_UE, ref msn))
                {
                    areas.Delete(m => m.Id_Area == Id);
                    areas.Save();
                    return Json(new { Exito = true, Mensaje = "Registro eliminado exitosamente", Datos = modelo }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Exito = false, Mensaje = msn }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = (ex.HResult == -2146233087 ? "El registro no puede ser eliminado porque ya se ha usado" : new Errores(ex.HResult, ex.Message).Mensaje) }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Ca_Bancos

        public ActionResult Bancos()
        {
            List<Ca_BancosModel> dataModel = new List<Ca_BancosModel>();
            bancos.Get().ToList().ForEach(item =>
            {
                Ca_BancosModel temp = ModelFactory.getModel<Ca_BancosModel>(item, new Ca_BancosModel());
                temp.Ca_Cuentas = ModelFactory.getModel<Ca_CuentasModel>(cuentas.GetByID(x => x.Id_Cuenta == temp.Id_Cuenta), new Ca_CuentasModel());
                dataModel.Add(temp);
            });
            return View(dataModel);
        }

        public ActionResult AgregarBanco()
        {
            Ca_BancosModel model = new Ca_BancosModel();
            model.ListaIdBancoRH = new SelectList(bancosrh.Get(), "Id_BancoRH", "Descripcion");
            return View(model);
        }

        [HttpPost]
        public ActionResult AgregarBanco(Ca_BancosModel dataModel, FormCollection frm)
        {
            try
            {
                String filters = frm["filters"];
                FiltrosCuentas filtros = new JavaScriptSerializer().Deserialize<FiltrosCuentas>(filters);
                Cuentas cuentasM = new Cuentas();
                if (cuentasM.hasValid(filtros))
                {
                    List<Ca_Bancos> bancosLst = bancos.Get().ToList();
                    if (bancosLst.Exists(x => x.Id_Cuenta.Trim().Equals(filtros.IdCuenta)))
                        return Json(new { Exito = false, Mensaje = "Esta cuenta ya esta asignada a otro banco, elige otra" });
                    if (bancosLst.Exists(x => x.Id_BancoRH.Equals(dataModel.Id_BancoRH)))
                        return Json(new { Exito = false, Mensaje = "Este banco RH ya esta siendo utilizado" });
                    if (bancosLst.Exists(x => x.Descripcion == dataModel.Descripcion))
                        return Json(new { Exito = false, Mensaje = "No puedes repetir la descripcion del banco" });
                    if (cuentas.GetByID(x => x.Id_Cuenta == dataModel.Id_Cuenta).Nivel)
                        return Json(new { Exito = false, Mensaje = "La Cuenta Bancaria no debe de ser de último nivel." });
                    UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                    dataModel.Usu_Act = Convert.ToInt16(appUsuario.IdUsuario);
                    dataModel.Fecha_Act = DateTime.Now;
                    bancos.Insert(EntityFactory.getEntity<Ca_Bancos>(dataModel, new Ca_Bancos()));
                    bancos.Save();
                    dataModel.Ca_Cuentas = ModelFactory.getModel<Ca_CuentasModel>(cuentas.GetByID(x => x.Id_Cuenta == dataModel.Id_Cuenta), new Ca_CuentasModel());
                    return Json(new { Exito = true, Mensaje = "Ok", Registro = dataModel });
                }
                else
                    return Json(new { Exito = false, Mensaje = "La cuenta no es válida, por favor intente con otra" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        public ActionResult EditarBanco(Int32 Id)
        {
            Ca_BancosModel model = ModelFactory.getModel<Ca_BancosModel>(bancos.GetByID(x => x.Id_Banco == Id), new Ca_BancosModel());
            model.ListaIdBancoRH = new SelectList(bancosrh.Get(), "Id_BancoRH", "Descripcion", model.Id_BancoRH);
            return View(model);
        }

        [HttpPost]
        public JsonResult EditarBanco(Ca_BancosModel dataModel)
        {
            try
            {
                Ca_Bancos banco = bancos.GetByID(x => x.Id_Banco == dataModel.Id_Banco);
                if (banco != null)
                {
                    UsuarioLogueado appUSuario = Logueo.GetUsrLogueado();
                    dataModel.Usu_Act = (short)appUSuario.IdUsuario;
                    dataModel.Fecha_Act = DateTime.Now;
                    bancos.Update(EntityFactory.getEntity<Ca_Bancos>(dataModel, new Ca_Bancos()));
                    bancos.Save();
                    dataModel.Ca_Cuentas = ModelFactory.getModel<Ca_CuentasModel>(cuentas.GetByID(x => x.Id_Cuenta == dataModel.Id_Cuenta), new Ca_CuentasModel());
                }
                return Json(new { Exito = true, Mensaje = "OK", Registro = dataModel });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        public JsonResult EliminarBanco(Int16 Id)
        {
            try
            {
                bancos.Delete(x => x.Id_Banco.Equals(Id));
                bancos.Save();
                return Json(new { Exito = true, Mensaje = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DetallesBanco(Int16 Id)
        {
            return View(ModelFactory.getModel<Ca_BancosModel>(bancos.GetByID(x => x.Id_Banco == Id), new Ca_BancosModel()));
        }

        #endregion

        #region Ca_BancosRH



        #endregion

        #region Ca_Beneficiarios


        public ActionResult V_Beneficiarios()
        {
            Ca_BeneficiariosModel model = new Ca_BeneficiariosModel();
            model.ListaIdEstado = new SelectList(estados.Get(), "Id_Estado", "Descripcion");
            model.ListaIdMunicipio = new SelectList(municipios.Get(x => x.Id_Estado == model.IdEstado), "Id_Municipio", "Descripcion");
            model.ListaIdLocalidad = new SelectList(localidades.Get(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio), "Id_Localidad", "Descripcion");
            model.ListaIdColonia = new SelectList(colonias.Get(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio && x.Id_Localidad == model.IdLocalidad), "id_colonia", "Descripcion");
            model.ListaIdCalle = new SelectList(calles.Get(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio && x.Id_Localidad == model.IdLocalidad), "id_calle", "Descripcion");
            return View(model);
        }

        [HttpPost]
        public ActionResult V_BeneficiariosInsert(Ca_BeneficiariosModel modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            int IdPersona = 0;
            int IdBeneficiario = 0;
            CA_Personas persona = ModelFactory.getModel<CA_Personas>(modelo, new CA_Personas());
            Ca_Beneficiarios beneficiario = ModelFactory.getModel<Ca_Beneficiarios>(modelo, new Ca_Beneficiarios());
            List<CA_BeneficiariosCuentas> clasificacion = new List<CA_BeneficiariosCuentas>();
            List<De_Beneficiarios_Giros> giro = new List<De_Beneficiarios_Giros>();
            List<DE_BeneficiarioContactos> contacto = new List<DE_BeneficiarioContactos>();
            string[] listaClasificacion = { };
            string[] listaGitos = { };
            string[] listaContactos = { };
            if (!string.IsNullOrEmpty(modelo.listaidclasificacion)) listaClasificacion = modelo.listaidclasificacion.Split(',');
            if (!string.IsNullOrEmpty(modelo.listaidgiros)) listaGitos = modelo.listaidgiros.Split(',');
            if (!string.IsNullOrEmpty(modelo.listacontactos)) listaContactos = modelo.listacontactos.Split(',');

            string NombreCompleto = "";
            if (persona.PersonaFisica == true)
                NombreCompleto = String.Format("{0} {1} {2}", persona.Nombre, persona.ApellidoPaterno, persona.ApellidoMaterno);
            else
                NombreCompleto = persona.RazonSocial;

            //Empezar a guardar Elementos
            IdPersona = new Ca_PersonasModel().Id_Calculate();
            persona.IdPersona = IdPersona;
            try
            {
                personas.Insert(persona);
                personas.Save();
            }
            catch
            {
                return Json(new { Exito = false, Mensaje = "No se guardo adecuadamente la persona" });
            }
            //----------------------//
            beneficiario.IdPersona = IdPersona;
            IdBeneficiario = new Ca_BeneficiariosModel().Id_Calculate();
            beneficiario.Id_Beneficiario = IdBeneficiario;
            beneficiario.Usu_Act = (short)appUsuario.IdUsuario;
            beneficiario.Fecha_Act = DateTime.Now;
            try
            {

                beneficiarios.Insert(beneficiario);
                beneficiarios.Save();
            }
            catch
            {
                return Json(new { Exito = false, Mensaje = "No se guardo adecuadamente el beneficiario" });
            }
            //----------------------//
            foreach (string item in listaClasificacion)
            {
                byte clas = Convert.ToByte(item);
                string cta = clasificacionbeneficiario.GetByID(x => x.Id_ClasificaBene == clas).Id_Cuenta;
                string ctanueva = CuentaConsecutivaGuardar(cta, NombreCompleto, appUsuario.IdUsuario);
                try
                {
                    beneficiarioscuentas.Insert(new CA_BeneficiariosCuentas() { Id_Beneficiario = IdBeneficiario, Id_ClasBeneficiario = Convert.ToByte(item), Fecha_Act = DateTime.Now, Usu_act = (short)appUsuario.IdUsuario, Id_Cuenta = ctanueva });
                    beneficiarioscuentas.Save();
                }
                catch
                {
                    return Json(new { Exito = false, Mensaje = "No se guardo adecuadamente la clasificación: " + clas });
                }
            }
            //----------------------//
            if (listaGitos != null)
                foreach (string item in listaGitos)
                {
                    byte gir = Convert.ToByte(item);
                    try
                    {
                        debeneficiariosgiros.Insert(new De_Beneficiarios_Giros() { Id_Beneficiario = IdBeneficiario, Id_GiroComercial = gir, Fecha_Act = DateTime.Now, Usu_Act = (short)appUsuario.IdUsuario });
                        debeneficiariosgiros.Save();
                    }
                    catch
                    {
                        return Json(new { Exito = false, Mensaje = "No se guardo adecuadamente el giro: " + gir });
                    }
                }
            if (listaContactos != null)
            {
                int aux = listaContactos.Length / 3;
                for (int i = 0; i < aux; i++)
                {
                    try
                    {
                        debeneficiarioscontactos.Insert(new DE_BeneficiarioContactos() { Id_Beneficiario = IdBeneficiario, IdContacto = new De_BeneficiarioContactosModel().Id_Calculate(IdBeneficiario), CURP = listaContactos[i + aux], Nombre = listaContactos[i + aux * 2] });
                        debeneficiarioscontactos.Save();
                    }
                    catch
                    {
                        return Json(new { Exito = false, Mensaje = "No se guardo adecuadamente el contacto: " + listaContactos[i + aux] });
                    }
                }
            }
            //Ca_BeneficiariosModel model = new Ca_BeneficiariosModel();
            //model = ModelFactory.getModel<Ca_BeneficiariosModel>(beneficiarios.GetByID(x => x.Id_Beneficiario == IdBeneficiario), new Ca_BeneficiariosModel());
            //model = ModelFactory.getModel<Ca_BeneficiariosModel>(new Llenado().Llenado_CaPersonas(model.IdPersona), model);
            //List<Ca_BeneficiariosCuentasModel> listaregresar = new List<Ca_BeneficiariosCuentasModel>();
            //foreach (CA_BeneficiariosCuentas item in beneficiarioscuentas.Get(x => x.Id_Beneficiario == IdBeneficiario))
            //{
            //    listaregresar.Add(new Llenado().LLenado_CaBeneficiariosCuentas(item.Id_Beneficiario, item.Id_ClasBeneficiario));
            //}
            //List<DE_BeneficiarioContactos> listbenecontactos = new List<DE_BeneficiarioContactos>();
            //listbenecontactos = debeneficiarioscontactos.Get(x => x.Id_Beneficiario == modelo.Id_Beneficiario).ToList();
            //List<De_Beneficiarios_GirosModel> listgiros = new List<De_Beneficiarios_GirosModel>();
            //debeneficiariosgiros.Get(x => x.Id_Beneficiario == modelo.Id_Beneficiario).ToList().ForEach(reg => listgiros.Add(new Llenado().LLenado_DeBeneficiariosGiros(reg.Id_Beneficiario, reg.Id_GiroComercial)));
            return Json(new { Exito = true, Registro = new Llenado().Llenado_CaBeneficiarios(modelo.Id_Beneficiario), Mensaje = "Se Guardo Correctamente" });
            //return Json(new { Exito = true, Registro = model, ListaClasificacion = listaregresar, ListaGiros = listgiros, ListaContactos = listbenecontactos, Mensaje = "Se Guardo Correctamente" });
        }

        [HttpPost]
        public ActionResult V_BeneficiariosEdit(Ca_BeneficiariosModel modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            CA_Personas persona = personas.GetByID(x => x.IdPersona == modelo.IdPersona);

            persona = EntityFactory.getEntity<CA_Personas>(modelo, persona);
            try
            {
                personas.Update(persona);
                personas.Save();
            }
            catch
            {
                return Json(new { Exito = false, Mensaje = "No se guardo adecuadamente la persona" });
            }

            Ca_Beneficiarios beneficiario = beneficiarios.GetByID(x => x.Id_Beneficiario == modelo.Id_Beneficiario);
            beneficiario = ModelFactory.getModel<Ca_Beneficiarios>(modelo, beneficiario);
            try
            {
                beneficiarios.Update(beneficiario);
                beneficiarios.Save();
            }
            catch
            {
                return Json(new { Exito = false, Mensaje = "No se guardo adecuadamente el beneficiario" });
            }

            string[] listaGitos = { };
            string[] listaContactos = { };
            if (!string.IsNullOrEmpty(modelo.listaidgiros)) listaGitos = modelo.listaidgiros.Split(',');
            if (!string.IsNullOrEmpty(modelo.listacontactos)) listaContactos = modelo.listacontactos.Split(',');

            string NombreCompleto = "";
            if (persona.PersonaFisica == true)
                NombreCompleto = String.Format("{0} {1} {2}", persona.Nombre, persona.ApellidoPaterno, persona.ApellidoMaterno);
            else
                NombreCompleto = persona.RazonSocial;

            //-----------------------------------------------------------------------------------------------------------------------------------------//
            List<byte> listavieja = beneficiarioscuentas.Get(x => x.Id_Beneficiario == modelo.Id_Beneficiario).Select(x => x.Id_ClasBeneficiario).ToList();
            string[] listanueva = modelo.listaidclasificacion.Split(',');
            List<byte> listacompleta = new List<byte>();
            //Guardar Lista 1 en la completa
            foreach (byte item in listavieja)
            {
                listacompleta.Add(item);
            }
            //poner la 2da sin que se repitan
            foreach (string item in listanueva)
            {
                byte valor = Convert.ToByte(item);
                if (!listacompleta.Contains(valor))
                    listacompleta.Add(valor);
            }
            //Recorrer la lista completa.. si lo encuentra en la vieja y en la nueva no hacer nada.. si solamente en la vieja elimnarlo y si solamente en la nueva agregarlo
            foreach (byte item in listacompleta)
            {
                bool vieja = false, nueva = false; // 0 (Las 2), 1 (la vieja), 2 (La nueva)
                if (listavieja.Contains(item)) vieja = true;
                if (listanueva.Contains(item.ToString())) nueva = true;
                if (vieja == true && nueva == false) //Eliminar
                {
                    string borrarcta = beneficiarioscuentas.GetByID(x => x.Id_Beneficiario == modelo.Id_Beneficiario && x.Id_ClasBeneficiario == item).Id_Cuenta;
                    try
                    {
                        beneficiarioscuentas.Delete(x => x.Id_ClasBeneficiario == item && x.Id_Beneficiario == modelo.Id_Beneficiario);
                        beneficiarioscuentas.Save();
                    }
                    catch
                    {
                        return Json(new { Exito = false, Mensaje = "No se elimino adecuadamente la clasificación: " + item + ", favor de verificar que no este siendo utilizada en otro proceso." });
                    }
                    try
                    {
                        cuentas.Delete(x => x.Id_Cuenta == borrarcta);
                        cuentas.Save();
                    }
                    catch
                    {
                        return Json(new { Exito = false, Mensaje = "No se elimino adecuadamente la cuenta de la clasificación: " + item + ", favor de verificar que no este siendo utilizada en otro proceso." });
                    }
                }
                if (vieja == false && nueva == true) //Agregar
                {
                    string cta = clasificacionbeneficiario.GetByID(x => x.Id_ClasificaBene == item).Id_Cuenta;
                    string ctanueva = "";
                    try
                    {
                        ctanueva = CuentaConsecutivaGuardar(cta, NombreCompleto, appUsuario.IdUsuario);
                    }
                    catch
                    {
                        return Json(new { Exito = false, Mensaje = "No se guardo adecuadamente la cuenta de la clasificación: " + item });
                    }
                    try
                    {
                        beneficiarioscuentas.Insert(new CA_BeneficiariosCuentas() { Id_Beneficiario = modelo.Id_Beneficiario, Id_ClasBeneficiario = Convert.ToByte(item), Fecha_Act = DateTime.Now, Usu_act = (short)appUsuario.IdUsuario, Id_Cuenta = ctanueva });
                        beneficiarioscuentas.Save();
                    }
                    catch
                    {
                        return Json(new { Exito = false, Mensaje = "No se guardo adecuadamente la clasificación: " + item });
                    }
                }

            }
            //-----------------------------------------------------------------------------------------------------------------------------------------//
            List<short> listavieja2 = debeneficiariosgiros.Get(x => x.Id_Beneficiario == modelo.Id_Beneficiario).Select(x => x.Id_GiroComercial).ToList();
            if (!string.IsNullOrEmpty(modelo.listaidgiros))
                listanueva = modelo.listaidgiros.Split(',');
            else
                listanueva = new string[] { };
            List<short> listacompleta2 = new List<short>();
            //Guardar Lista 1 en la completa
            foreach (byte item in listavieja2)
            {
                listacompleta2.Add(item);
            }
            //poner la 2da sin que se repitan
            foreach (string item in listanueva)
            {
                short valor = Convert.ToInt16(item);
                if (!listacompleta2.Contains(valor))
                    listacompleta2.Add(valor);
            }
            //Recorrer la lista completa.. si lo encuentra en la vieja y en la nueva no hacer nada.. si solamente en la vieja elimnarlo y si solamente en la nueva agregarlo
            foreach (short item in listacompleta2)
            {
                bool vieja = false, nueva = false; // 0 (Las 2), 1 (la vieja), 2 (La nueva)
                if (listavieja2.Contains(item)) vieja = true;
                if (listanueva.Contains(item.ToString())) nueva = true;
                if (vieja == true && nueva == false) //Eliminar
                {
                    try
                    {
                        debeneficiariosgiros.Delete(x => x.Id_GiroComercial == item && x.Id_Beneficiario == modelo.Id_Beneficiario);
                        debeneficiariosgiros.Save();
                    }
                    catch
                    {
                        return Json(new { Exito = false, Mensaje = "No se elimino adecuadamente el giro: " + item + ", favor de verificar que no este siendo utilizada en otro proceso." });
                    }
                }
                if (vieja == false && nueva == true) //Agregar
                {
                    try
                    {
                        debeneficiariosgiros.Insert(new De_Beneficiarios_Giros() { Id_Beneficiario = modelo.Id_Beneficiario, Id_GiroComercial = item, Fecha_Act = DateTime.Now, Usu_Act = (short)appUsuario.IdUsuario });
                        debeneficiariosgiros.Save();
                    }
                    catch
                    {
                        return Json(new { Exito = false, Mensaje = "No se guardo adecuadamente el giro: " + item });
                    }
                }

            }

            ////-----------------------------------------------------------------------------------------------------------------------------------------//
            listavieja2 = debeneficiarioscontactos.Get(x => x.Id_Beneficiario == modelo.Id_Beneficiario).Select(x => x.IdContacto).ToList();
            string[] listaaux = { };
            if (!string.IsNullOrEmpty(modelo.listacontactos))
                listaaux = modelo.listacontactos.Split(',');
            listacompleta2 = new List<short>();
            int aux = listaContactos.Length / 3;
            listanueva = new string[aux];
            for (int i = 0; i < aux; i++)
            {
                listanueva[i] = listaaux[i];
            }
            //Guardar Lista 1 en la completa
            foreach (byte item in listavieja2)
            {
                listacompleta2.Add(item);
            }
            //poner la 2da sin que se repitan
            foreach (string item in listanueva)
            {
                short valor = Convert.ToInt16(item);
                if (!listacompleta2.Contains(valor))
                    listacompleta2.Add(valor);
            }
            //Recorrer la lista completa.. si lo encuentra en la vieja y en la nueva no hacer nada.. si solamente en la vieja elimnarlo y si solamente en la nueva agregarlo
            foreach (short item in listacompleta2)
            {
                bool vieja = false, nueva = false; // 0 (Las 2), 1 (la vieja), 2 (La nueva)
                if (listavieja2.Contains(item)) vieja = true;
                if (listanueva.Contains(item.ToString())) nueva = true;
                if (vieja == true && nueva == false) //Eliminar
                {
                    try
                    {
                        debeneficiarioscontactos.Delete(x => x.IdContacto == item && x.Id_Beneficiario == modelo.Id_Beneficiario);
                        debeneficiariosgiros.Save();
                    }
                    catch
                    {
                        return Json(new { Exito = false, Mensaje = "No se elimino adecuadamente el contacto: " + item + ", favor de verificar que no este siendo utilizada en otro proceso." });
                    }
                }
                if (vieja == false && nueva == true) //Agregar
                {
                    int i = 0;
                    try
                    {
                        //obteber posicion
                        foreach (string itemaux in listanueva)
                        {
                            if (itemaux == item.ToString())
                                break;
                            i++;
                        }
                        debeneficiarioscontactos.Insert(new DE_BeneficiarioContactos() { Id_Beneficiario = modelo.Id_Beneficiario, IdContacto = new De_BeneficiarioContactosModel().Id_Calculate(modelo.Id_Beneficiario), CURP = listaContactos[i + aux], Nombre = listaContactos[i + aux * 2] });
                        debeneficiarioscontactos.Save();
                    }
                    catch
                    {
                        return Json(new { Exito = false, Mensaje = "No se guardo adecuadamente el contacto: " + listaContactos[i + aux] });
                    }
                }

            }
            return Json(new { Exito = true, Registro = new Llenado().Llenado_CaBeneficiarios(modelo.Id_Beneficiario), Mensaje = "Se Guardo Correctamente" });
        }

        [HttpPost]
        public ActionResult V_BeneficiariosDelete(int IdBeneficiario)
        {
            string Error = "";
            try
            {
                foreach (DE_BeneficiarioContactos item in debeneficiarioscontactos.Get(x => x.Id_Beneficiario == IdBeneficiario))
                {
                    Error = "Error al borrar el contacto: " + item.Nombre + ", favor de verificar que no este siendo utilizada en otro proceso.";
                    debeneficiarioscontactos.Delete(x => x.Id_Beneficiario == item.Id_Beneficiario && x.IdContacto == item.IdContacto);
                }
                debeneficiarioscontactos.Save();

                foreach (De_Beneficiarios_Giros item in debeneficiariosgiros.Get(x => x.Id_Beneficiario == IdBeneficiario))
                {
                    Error = "Error al borrar el giro: " + item.Id_GiroComercial + ", favor de verificar que no este siendo utilizada en otro proceso.";
                    debeneficiariosgiros.Delete(x => x.Id_Beneficiario == item.Id_Beneficiario && x.Id_GiroComercial == item.Id_GiroComercial);
                }
                debeneficiariosgiros.Save();

                List<string> cta = beneficiarioscuentas.Get(x => x.Id_Beneficiario == IdBeneficiario).Select(x => x.Id_Cuenta).ToList();
                int i = 0;
                foreach (CA_BeneficiariosCuentas item in beneficiarioscuentas.Get(x => x.Id_Beneficiario == IdBeneficiario))
                {
                    Error = "Error al borrar la clasificación: " + item.Id_ClasBeneficiario + ", favor de verificar que no este siendo utilizada en otro proceso.";
                    beneficiarioscuentas.Delete(x => x.Id_Beneficiario == item.Id_Beneficiario && x.Id_ClasBeneficiario == item.Id_ClasBeneficiario);
                }
                beneficiarioscuentas.Save();

                foreach (string item in cta)
                {
                    Error = "Error al borrar la cuenta: " + cuentas.GetByID(x => x.Id_Cuenta == item).Id_CuentaFormato + ", favor de verificar que no este siendo utilizada en otro proceso.";
                    cuentas.Delete(x => x.Id_Cuenta == item);
                }
                cuentas.Save();

                Error = "Error al borrar el beneficiario: " + IdBeneficiario + ", favor de verificar que no este siendo utilizada en otro proceso.";
                beneficiarios.Delete(x => x.Id_Beneficiario == IdBeneficiario);
                beneficiarios.Save();

                int Persona = beneficiarios.GetByID(x => x.Id_Beneficiario == IdBeneficiario).IdPersona;
                Error = "Error al borrar la persona: " + Persona + ", favor de verificar que no este siendo utilizada en otro proceso.";
                personas.Delete(x => x.IdPersona == Persona);
                personas.Save();

                return Json(new { Exito = true, Mensaje = "Se Elimino Correctamente" });
            }
            catch
            {
                return Json(new { Exito = false, Mensaje = Error });
            }
        }

        public string CuentaConsecutivaGuardar(string IdCuenta, string Descripcio, int usuario)
        {
            Ca_CuentasModel model = new Ca_CuentasModel();
            CuentasDAL dal = new CuentasDAL();
            CA_Cuentas cta = dal.GetByID(x => x.Id_Cuenta == IdCuenta);
            byte Genero = 0;
            byte Grupo = 0;
            byte Rubro = 0;
            byte Cuenta = 0;
            byte SubCuentaO1 = 0;
            int SubCuentaO2 = 0;
            short SubCuentaO3 = 0;
            int SubCuentaO4 = 0;
            model.Consecutivo(cta.Genero, cta.Grupo, cta.Rubro, cta.Cuenta, cta.SubCuentaO1, cta.SubCuentaO2, cta.SubCuentaO3, cta.SubCuentaO4, ref Genero, ref Grupo, ref Rubro, ref Cuenta, ref SubCuentaO1, ref SubCuentaO2, ref SubCuentaO3, ref SubCuentaO4);
            string IdCuentaNueva = StringID.IdCuenta(Genero, Grupo, Rubro, Cuenta, SubCuentaO1, SubCuentaO2, SubCuentaO3, SubCuentaO4);
            cuentas.Insert(new CA_Cuentas() { Genero = Genero, Grupo = Grupo, Rubro = Rubro, Cuenta = Cuenta, SubCuentaO1 = SubCuentaO1, SubCuentaO2 = SubCuentaO2, SubCuentaO3 = SubCuentaO3, SubCuentaO4 = SubCuentaO4, Nivel = true, Descripcion = Descripcio, Fecha_Act = DateTime.Now, Naturaleza = cta.Naturaleza, Usu_Act = (short)usuario, Id_Cuenta = IdCuentaNueva });
            cuentas.Save();
            return IdCuentaNueva;
        }

        [HttpPost]
        public ActionResult V_Beneficiarios_Buscar(int IdBeneficiario)
        {
            return Json(new Llenado().Llenado_CaBeneficiarios(IdBeneficiario));
        }

        public ActionResult V_Beneficiarios_Clasificacion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult V_Beneficiarios_ClasificacionTbl(string Descripcion)
        {
            List<Ca_ClasificaBeneficiariosModel> Lst = new List<Ca_ClasificaBeneficiariosModel>();
            foreach (Ca_ClasificaBeneficiarios item in clasificacionbeneficiario.Get(x => x.Descripcion.Contains(Descripcion)).ToList())
            {
                CA_Cuentas cta = cuentas.GetByID(x => x.Id_Cuenta == item.Id_Cuenta);
                if (cta.Genero != 0 && cta.Grupo != 0 && cta.Rubro != 0 && cta.Cuenta != 0 && cta.SubCuentaO1 > 0)
                    Lst.Add(new Llenado().LLenado_CaClasificaBeneficiarios(item.Id_ClasificaBene));
            }
            return View(Lst);
        }

        public ActionResult V_Beneficiarios_Giros()
        {
            return View();
        }

        [HttpPost]
        public ActionResult V_Beneficiarios_GirosTbl(string Descripcion)
        {
            List<Ca_GirosModel> Lst = new List<Ca_GirosModel>();
            foreach (CA_Giros item in giros.Get(x => x.Descripcion.Contains(Descripcion)).ToList())
            {
                Lst.Add(new Llenado().LLenado_CaGiros(item.Id_GiroComercial));
            }
            return View(Lst);
        }

        public ActionResult V_Beneficiarios_Contactos()
        {
            return View(new De_BeneficiarioContactosModel());
        }

        [HttpPost]
        public ActionResult V_Beneficiarios_Contactos(DE_BeneficiarioContactos model)
        {
            return View();
        }

        //-----------------------------------------------------------------//
        public ActionResult V_Beneficiarios_BuscarTbl()
        {
            ViewBag.B_Estados = estados.Get().OrderBy(x => x.Descripcion);
            ViewBag.B_Municipios = new List<Ca_MunicipiosModel>();
            ViewBag.B_Localidades = new List<Ca_LocalidadesModel>();
            List<Ca_BeneficiariosModel> benLst = new List<Ca_BeneficiariosModel>();
            IEnumerable<Ca_Beneficiarios> lst = beneficiarios.Get();
            foreach (Ca_Beneficiarios item in lst)
            {
                Ca_BeneficiariosModel model = new Ca_BeneficiariosModel();
                model = ModelFactory.getModel<Ca_BeneficiariosModel>(beneficiarios.GetByID(x => x.Id_Beneficiario == item.Id_Beneficiario), new Ca_BeneficiariosModel());
                model = ModelFactory.getModel<Ca_BeneficiariosModel>(new Llenado().Llenado_CaPersonas(model.IdPersona), model);

                benLst.Add(model);
            }
            return View(benLst);
        }

        public ActionResult V_Beneficiarios_Tbl(string Estado, string Municipio, string Localidad, string Nombre)
        {
            List<Ca_BeneficiariosModel> benLst = new List<Ca_BeneficiariosModel>();
            beneficiarios.Get().ToList().ForEach(x => { benLst.Add(ModelFactory.getModel<Ca_BeneficiariosModel>(x, new Ca_BeneficiariosModel())); });
            return View(benLst);
        }

        public ActionResult V_Beneficiarios_Detalles(int IdBeneficiario)
        {
            return View(ModelFactory.getModel<Ca_BeneficiariosModel>(beneficiarios.GetByID(x => x.Id_Beneficiario == IdBeneficiario), new Ca_BeneficiariosModel()));
        }

        public ActionResult V_Beneficiarios_Agregar()
        {
            return View(new Ca_BeneficiariosModel());
        }

        [HttpPost]
        public ActionResult V_Beneficiarios_Agregar(Ca_Beneficiarios modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            Ca_Beneficiarios model = new Ca_Beneficiarios();
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    if (msn == "")
                    //if (model.ValidaAdd(modelo.Id_UP, modelo.Id_UR, modelo.Id_UE, modelo.Descripcion, modelo.UltimoNivel, ref msn) == true)
                    {
                        //modelo.Id_Area = StringID.IdArea(modelo.Id_UP, modelo.Id_UR, modelo.Id_UE);
                        //modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                        //modelo.Fecha_Act = DateTime.Now;
                        //if (modelo.Id_UP > 0 && modelo.Id_UR > 0 && modelo.Id_UE > 0) modelo.UltimoNivel = true;
                        //areas.Insert(modelo);
                        //areas.Save();
                        return Json(new { Exito = true, Mensaje = StringID.Agregado, Datos = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    return Json(new { Exito = false, Mensaje = StringID.Exceptions + ex });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = StringID.IsValid });
            }
        }

        #endregion

        #region BeneficiariosCuentas

        #endregion

        #region Ca_Calles
        public ActionResult V_Calles()
        {
            List<Ca_CallesModel> Lst = new List<Ca_CallesModel>();
            IEnumerable<Ca_Calles> entities = calles.Get();
            foreach (Ca_Calles item in entities)
            {
                try
                {
                    Ca_CallesModel model = ModelFactory.getModel<Ca_CallesModel>(item, new Ca_CallesModel());
                    model.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                    model.CA_Municipio = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado), new Ca_MunicipiosModel());
                    model.CA_Localidad = ModelFactory.getModel<Ca_LocalidadesModel>(localidades.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado && x.Id_Localidad == model.Id_Localidad), new Ca_LocalidadesModel());
                    Lst.Add(model);
                }
                catch (Exception ex)
                {

                }

            }
            return View(Lst);
        }
        public ActionResult V_CallesDetalles(Int16 IdEstado, Int16 IdMunicipio, Int16 IdLocalidad, Int16 IdCalle)
        {
            Ca_CallesModel model = ModelFactory.getModel<Ca_CallesModel>(calles.GetByID(x => x.Id_Localidad == IdLocalidad && x.Id_Estado == IdEstado && x.Id_Municipio == IdMunicipio && x.id_calle == IdCalle), new Ca_CallesModel());
            model.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
            model.CA_Municipio = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado), new Ca_MunicipiosModel());
            model.CA_Localidad = ModelFactory.getModel<Ca_LocalidadesModel>(localidades.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado && x.Id_Localidad == model.Id_Localidad), new Ca_LocalidadesModel());
            return View(model);
        }
        public ActionResult V_Calles_Guardar()
        {
            Ca_CallesModel model = new Ca_CallesModel();
            model.ListaIdEstado = new SelectList(estados.Get(), "Id_Estado", "Descripcion");
            model.ListaIdMunicipio = new SelectList(municipios.Get(x => x.Id_Estado == model.Id_Estado), "Id_Municipio", "Descripcion");
            model.ListaIdLocalidad = new SelectList(localidades.Get(x => x.Id_Estado == model.Id_Estado && x.Id_Municipio == model.Id_Municipio), "Id_Localidad", "Descripcion");
            return View(model);
        }
        [HttpPost]
        public ActionResult V_Calles_Guardar(Ca_Calles model)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;

            try
            {
                if (calles.Get(x => x.Id_Estado == model.Id_Estado && x.Id_Municipio == model.Id_Municipio && x.Id_Localidad == model.Id_Localidad).Count() == 0)
                    model.id_calle = 1;
                else
                    model.id_calle = Convert.ToInt16(calles.Get(x => x.Id_Estado == model.Id_Estado && x.Id_Municipio == model.Id_Municipio && x.Id_Localidad == model.Id_Localidad).Max(x => x.id_calle) + 1);
                model.Usu_Act = (Int16)appUsuario.IdUsuario;
                model.Fecha_Act = DateTime.Now;
                calles.Insert(model);
                calles.Save();
                Ca_CallesModel modelo = ModelFactory.getModel<Ca_CallesModel>(model, new Ca_CallesModel());
                modelo.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                modelo.CA_Municipio = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado), new Ca_MunicipiosModel());
                modelo.CA_Localidad = ModelFactory.getModel<Ca_LocalidadesModel>(localidades.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado && x.Id_Localidad == model.Id_Localidad), new Ca_LocalidadesModel());
                return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = modelo });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult V_Calles_Editar(Int16 IdEstado, Int16 IdMunicipio, Int16 IdLocalidad, Int16 IdCalle)
        {
            Ca_CallesModel model = ModelFactory.getModel<Ca_CallesModel>(calles.GetByID(x => x.Id_Estado == IdEstado && x.Id_Municipio == IdMunicipio && x.Id_Localidad == IdLocalidad && x.id_calle == IdCalle), new Ca_CallesModel());
            model.ListaIdEstado = new SelectList(estados.Get(), "Id_Estado", "Descripcion", model.Id_Estado);
            model.ListaIdMunicipio = new SelectList(municipios.Get(x => x.Id_Estado == model.Id_Estado), "Id_Municipio", "Descripcion", model.Id_Municipio);
            model.ListaIdLocalidad = new SelectList(localidades.Get(x => x.Id_Estado == model.Id_Estado && x.Id_Municipio == model.Id_Municipio), "Id_Localidad", "Descripcion");
            return View(model);
        }
        [HttpPost]
        public ActionResult V_Calles_Editar(Ca_Calles model)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;

            try
            {
                model.Usu_Act = (Int16)appUsuario.IdUsuario;
                model.Fecha_Act = DateTime.Now;
                calles.Update(model);
                calles.Save();
                Ca_CallesModel modelo = ModelFactory.getModel<Ca_CallesModel>(model, new Ca_CallesModel());
                modelo.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                modelo.CA_Municipio = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado), new Ca_MunicipiosModel());
                modelo.CA_Localidad = ModelFactory.getModel<Ca_LocalidadesModel>(localidades.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado && x.Id_Localidad == model.Id_Localidad), new Ca_LocalidadesModel());
                return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = modelo });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult V_Calles_Eliminar(Int16 IdEstado, Int16 IdMunicipio, Int16 IdLocalidad, Int16 IdCalle)
        {
            try
            {
                calles.Delete(x => x.Id_Estado == IdEstado && x.Id_Municipio == IdMunicipio && x.Id_Localidad == IdLocalidad && x.id_calle == IdCalle);
                calles.Save();
                return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult V_CallesGuardar(byte Estado, short Municipio, short Localidad, string Calle)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                Ca_CallesModel modelo = new Ca_CallesModel();
                Ca_Calles model = new Ca_Calles();
                model.Id_Estado = Estado;
                model.Id_Municipio = Municipio;
                model.Id_Localidad = Localidad;
                short Id = modelo.Id_Calculate(Estado, Municipio, Localidad);
                model.id_calle = Id;
                model.Descripcion = Calle;
                model.Usu_Act = (short)appUsuario.IdUsuario;
                model.Fecha_Act = DateTime.Now;
                calles.Insert(model);
                calles.Save();
                return new JsonResult() { Data = new SelectList(calles.Get(x => x.Id_Estado == Estado && x.Id_Municipio == Municipio && x.Id_Localidad == Localidad), "id_calle", "Descripcion", Id) };
            }
            catch
            {
                return new JsonResult() { Data = new SelectList(new List<Ca_Colonias>(), "id_colonia", "Descripcion") };
            }
        }

        #endregion

        #region Ca_CajasReceptoras
        public ActionResult V_CajasReceptoras()
        {

            List<Ca_CajasReceptorasModel> Lst = new List<Ca_CajasReceptorasModel>();
            CajasReceptoras.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_CajasReceptorasModel>(x, new Ca_CajasReceptorasModel())); });
            return View(Lst);
        }

        public ActionResult V_CajasReceptorasDetalles(Int32 Id)
        {
            return View(ModelFactory.getModel<Ca_CajasReceptorasModel>(CajasReceptoras.GetByID(x => x.Id_CajaR == Id), new Ca_CajasReceptorasModel()));
        }

        public ActionResult V_CajasReceptorasAgregar()
        {
            return View(new Ca_CajasReceptorasModel());
        }

        [HttpPost]
        public ActionResult V_CajasReceptorasAgregar(Ca_CajasReceptoras modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                    modelo.Fecha_Act = DateTime.Now;
                    modelo.Id_CajaR = (CajasReceptoras.Get().Count() > 0) ? Convert.ToByte(CajasReceptoras.Get().Max(x => x.Id_CajaR) + 1) : (byte)1;
                    //if (modelo.Id_CajaR == null || modelo.Id_CajaR < 0)
                    //{
                    //    modelo.Id_CajaR = 1;
                    //}
                    //
                    CajasReceptoras.Insert(modelo);
                    CajasReceptoras.Save();
                    return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = modelo });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }

        public ActionResult V_CajasReceptorasEditar(Int32 Id)
        {
            return View(ModelFactory.getModel<Ca_CajasReceptorasModel>(CajasReceptoras.GetByID(x => x.Id_CajaR == Id), new Ca_CajasReceptorasModel()));
        }

        [HttpPost]
        public ActionResult V_CajasReceptorasEditar(Ca_CajasReceptoras modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    modelo.Fecha_Act = DateTime.Now;
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                    CajasReceptoras.Update(modelo);
                    CajasReceptoras.Save();
                    return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Registro = modelo });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        public ActionResult V_CajasReceptorasEliminar(Int32 Id)
        {
            try
            {
                if (MaRecibosDAL.Get(x => x.Id_CajaR == Id).Count() == 0)
                {
                    CajasReceptoras.Delete(m => m.Id_CajaR == Id);
                    CajasReceptoras.Save();
                    return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Exito = false, Mensaje = "No se puede eliminar porque se esta usando en Ma recibos." }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Ca_CausasCancelacion
        public ActionResult V_CausasCancelacion()
        {
            List<Ca_CausaCancelacionModel> Lst = new List<Ca_CausaCancelacionModel>();
            causascancelacion.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_CausaCancelacionModel>(x, new Ca_CausaCancelacionModel())); });
            return View(Lst);
        }
        public ActionResult V_CausasCancelacionDetalles(Int16 Id_Causa, Int16 Id_Tipo)
        {
            return View(ModelFactory.getModel<Ca_CausaCancelacionModel>(causascancelacion.GetByID(x => x.Id_Causa == Id_Causa && x.Id_TipoCausa == Id_Tipo), new Ca_CausaCancelacionModel()));
        }
        #endregion

        #region Ca_CentroRecaudador
        public ActionResult V_CentroRecaudador()
        {

            List<Ca_CentroRecaudadorModel> Lst = new List<Ca_CentroRecaudadorModel>();
            CentroRecaudadorDAL.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_CentroRecaudadorModel>(x, new Ca_CentroRecaudadorModel())); });
            return View(Lst);
        }
        public ActionResult V_CentroRecaudadorDetalles(string Id)
        {
            return View(ModelFactory.getModel<Ca_CentroRecaudadorModel>(CentroRecaudadorDAL.GetByID(x => x.Id_CRecaudador == Id), new Ca_CentroRecaudadorModel()));
        }
        public ActionResult V_CentroRecaudadorAgregar()
        {
            return View(new Ca_CentroRecaudadorModel());
        }
        [HttpPost]
        public ActionResult V_CentroRecaudadorAgregar(Ca_CentroRecaudador modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            Ca_CentroRecaudadorModel model = new Ca_CentroRecaudadorModel();
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    if (model.ValidaAdd(modelo.Id_UPI, modelo.Id_URI, modelo.Id_UEI, modelo.Descripcion, modelo.UltimoNivel, ref msn) == true)
                    {
                        modelo.Id_CRecaudador = StringID.IdArea(modelo.Id_UPI, modelo.Id_URI, modelo.Id_UEI);
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                        modelo.Fecha_Act = DateTime.Now;
                        if (modelo.Id_URI > 0 && modelo.Id_UEI > 0 && modelo.Id_UPI > 0)
                            modelo.UltimoNivel = true;
                        else
                            modelo.UltimoNivel = false;
                        CentroRecaudadorDAL.Insert(modelo);
                        CentroRecaudadorDAL.Save();
                        return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        public ActionResult V_CentroRecaudadorEditar(string Id)
        {
            return View(ModelFactory.getModel<Ca_CentroRecaudadorModel>(CentroRecaudadorDAL.GetByID(x => x.Id_CRecaudador == Id), new Ca_CentroRecaudadorModel()));
        }

        [HttpPost]
        public ActionResult V_CentroRecaudadorEditar(Ca_CentroRecaudador modelo)
        {

            Ca_CentroRecaudadorModel model = new Ca_CentroRecaudadorModel();
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    if (model.ValidaEdit(modelo.Id_UPI, modelo.Id_URI, modelo.Id_UEI, modelo.Descripcion, modelo.UltimoNivel, ref msn) == true)
                    {
                        modelo.Fecha_Act = DateTime.Now;
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                        if (modelo.Id_URI > 0 && modelo.Id_UEI > 0 && modelo.Id_UPI > 0) modelo.UltimoNivel = true;
                        CentroRecaudadorDAL.Update(modelo);
                        CentroRecaudadorDAL.Save();
                        return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Registro = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        public ActionResult V_CentroRecaudadorEliminar(string Id)
        {
            try
            {
                Ca_CentroRecaudadorModel model = new Ca_CentroRecaudadorModel();
                string msn = "";
                Ca_CentroRecaudadorModel modelo = ModelFactory.getModel<Ca_CentroRecaudadorModel>(CentroRecaudadorDAL.GetByID(x => x.Id_CRecaudador == Id), new Ca_CentroRecaudadorModel());
                if (model.ValidaDelete(modelo.Id_UPI, modelo.Id_URI, modelo.Id_UEI, ref msn))
                {
                    CentroRecaudadorDAL.Delete(m => m.Id_CRecaudador == Id);
                    CentroRecaudadorDAL.Save();
                    return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente", Datos = modelo }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Exito = false, Mensaje = msn }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Ca_CierreBanco

        #endregion

        #region Ca_Cierremensual

        #endregion

        #region Ca_ClasificaBeneficiarios
        public ActionResult V_ClasificaBeneficiario()
        {
            List<Ca_ClasificaBeneficiariosModel> Lst = new List<Ca_ClasificaBeneficiariosModel>();
            clasificacionbeneficiario.Get().ToList().ForEach(x =>
            {
                Ca_ClasificaBeneficiariosModel temp = ModelFactory.getModel<Ca_ClasificaBeneficiariosModel>(x, new Ca_ClasificaBeneficiariosModel());
                temp.Ca_Cuentas = ModelFactory.getModel<Ca_CuentasModel>(cuentas.GetByID(reg => reg.Id_Cuenta == temp.Id_Cuenta), new Ca_CuentasModel());
                Lst.Add(temp);
            });
            return View(Lst);
        }
        public ActionResult V_ClasificaBeneficiario_Detalles(byte? Id)
        {
            if (Id != null && Id != 0)
                return View(ModelFactory.getModel<Ca_ClasificaBeneficiariosModel>(clasificacionbeneficiario.GetByID(x => x.Id_ClasificaBene == Id), new Ca_ClasificaBeneficiariosModel()));
            return View(new Ca_ClasificaBeneficiariosModel());
        }

        public ActionResult V_ClasificaBeneficiario_Agregar()
        {
            ViewBag.genero = parametros.GetByID(x => x.Nombre == "Cuenta_ClasificaBene").Valor;
            return View(new Ca_ClasificaBeneficiariosModel());
        }

        [HttpPost]
        public ActionResult V_ClasificaBeneficiario_Agregar(Ca_ClasificaBeneficiarios modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            Ca_ClasificaBeneficiariosModel model = new Ca_ClasificaBeneficiariosModel();
            if (ModelState.IsValid)
            {
                try
                {
                    if (clasificacionbeneficiario.Get(x => x.Id_Cuenta == modelo.Id_Cuenta).Count() == 0)
                    {
                        if (cuentas.GetByID(x => x.Id_Cuenta == modelo.Id_Cuenta).Nivel == false)
                        {
                            modelo.Descripcion = modelo.Descripcion.Trim();
                            modelo.Id_ClasificaBene = 0;//Convert.ToByte(clasificacionbeneficiario.Get().Max(x => x.Id_ClasificaBene) + 1);
                            modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                            modelo.Fecha_Act = DateTime.Now;
                            clasificacionbeneficiario.Insert(modelo);
                            clasificacionbeneficiario.Save();
                            model = ModelFactory.getModel<Ca_ClasificaBeneficiariosModel>(modelo, new Ca_ClasificaBeneficiariosModel());
                            model.Ca_Cuentas = ModelFactory.getModel<Ca_CuentasModel>(cuentas.GetByID(reg => reg.Id_Cuenta == model.Id_Cuenta), new Ca_CuentasModel());
                            return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = model });
                        }
                        return Json(new { Exito = false, Mensaje = "La cuenta es de Último Nivel", Registro = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = "Ya se encuentra registrada esa cuenta", Registro = modelo });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información no válida" });
            }
        }
        public ActionResult V_ClasificaBeneficiario_Editar(byte Id)
        {
            Ca_ClasificaBeneficiariosModel model = ModelFactory.getModel<Ca_ClasificaBeneficiariosModel>(clasificacionbeneficiario.GetByID(x => x.Id_ClasificaBene == Id), new Ca_ClasificaBeneficiariosModel());
            model.Ca_Cuentas = ModelFactory.getModel<Ca_CuentasModel>(cuentas.GetByID(reg => reg.Id_Cuenta == model.Id_Cuenta), new Ca_CuentasModel());
            return View(model);
        }
        [HttpPost]
        public ActionResult V_ClasificaBeneficiario_Editar(Ca_ClasificaBeneficiarios modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            Ca_ClasificaBeneficiariosModel model = new Ca_ClasificaBeneficiariosModel();
            if (ModelState.IsValid)
            {
                try
                {
                    if (clasificacionbeneficiario.Get(x => x.Id_Cuenta == modelo.Id_Cuenta && x.Id_ClasificaBene != modelo.Id_ClasificaBene).Count() == 0)
                    {
                        if (cuentas.GetByID(x => x.Id_Cuenta == modelo.Id_Cuenta).Nivel == false)
                        {
                            modelo.Descripcion = modelo.Descripcion.Trim();
                            modelo.Fecha_Act = DateTime.Now;
                            modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                            clasificacionbeneficiario.Update(modelo);
                            clasificacionbeneficiario.Save();
                            model = ModelFactory.getModel<Ca_ClasificaBeneficiariosModel>(modelo, new Ca_ClasificaBeneficiariosModel());
                            model.Ca_Cuentas = ModelFactory.getModel<Ca_CuentasModel>(cuentas.GetByID(reg => reg.Id_Cuenta == model.Id_Cuenta), new Ca_CuentasModel());
                            return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Registro = model });
                        }
                        return Json(new { Exito = false, Mensaje = "La cuenta es de Último Nivel", Registro = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = "Ya se encuentra registrada esa cuenta", Registro = modelo });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }

        public ActionResult V_ClasificaBeneficiario_Eliminar(byte Id)
        {
            try
            {
                string msn = "";
                Ca_ClasificaBeneficiariosModel modelo = ModelFactory.getModel<Ca_ClasificaBeneficiariosModel>(clasificacionbeneficiario.GetByID(x => x.Id_ClasificaBene == Id), new Ca_ClasificaBeneficiariosModel());
                clasificacionbeneficiario.Delete(m => m.Id_ClasificaBene == Id);
                clasificacionbeneficiario.Save();
                return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Ca_ClasificaPolizas

        #endregion

        #region Ca_ClasProgramatica
        //LISTA DE CLASIFICACION PRAGMATICA 
        public ActionResult V_ClasProgramatica()
        {
            List<Ca_ClasProgramaticaModel> Lst = new List<Ca_ClasProgramaticaModel>();
            clasprogramatica.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_ClasProgramaticaModel>(x, new Ca_ClasProgramaticaModel())); });
            return View(Lst);
        }
        public ActionResult V_AgregarClasProgramatica()
        {
            return View(new Ca_ClasProgramaticaModel());
        }

        public ActionResult V_ObtenerClasProgramatica(string Id)
        {
            if (Id != null)
                return View(ModelFactory.getModel<Ca_ClasProgramaticaModel>(clasprogramatica.GetByID(x => x.Id_ClasificacionP == Id), new Ca_ClasProgramaticaModel()));
            return View(new Ca_ClasProgramaticaModel());
        }

        [HttpPost]
        public ActionResult EditarClasProgramatica(Ca_ClasProgramatica dataModel)
        {
            try
            {
                Ca_ClasProgramaticaModel model = new Ca_ClasProgramaticaModel();
                string msn = "";
                if (model.ValidaDesc(dataModel.Descripcion, ref msn))
                {
                    UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                    dataModel.Usu_Act = (Int16)appUsuario.IdUsuario;
                    dataModel.Fecha_Act = DateTime.Now;
                    clasprogramatica.Update(dataModel);
                    clasprogramatica.Save();
                    return Json(new { Exito = true, Mensaje = "Registro guardado correctamente", Registro = dataModel });
                }
                else
                    return Json(new { Exito = false, Mensaje = msn });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult AgregarClasProgramatica(Ca_ClasProgramatica dataModel)
        {
            try
            {
                Ca_ClasProgramaticaModel model = new Ca_ClasProgramaticaModel();
                string msn = "";
                if (model.Valida(dataModel.Id_ClasificacionP, dataModel.Descripcion, ref msn) == true)
                {
                    UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                    dataModel.Usu_Act = (Int16)appUsuario.IdUsuario;
                    dataModel.Descripcion = dataModel.Descripcion.Trim();
                    dataModel.Fecha_Act = DateTime.Now;
                    clasprogramatica.Insert(dataModel);
                    clasprogramatica.Save();
                    return Json(new { Exito = true, Mensaje = "Registro guardado correctamente.", Registro = dataModel });
                }
                else
                    return Json(new { Exito = false, Mensaje = msn });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }

        public ActionResult EliminarClasProgramatica(string Id)
        {
            try
            {
                clasprogramatica.Delete(x => x.Id_ClasificacionP == Id);
                clasprogramatica.Save();
                return Json(new { Exito = true, Mensaje = "Registro eliminado correctamente." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult V_DetallesClasProgramatica(string Id)
        {
            return View(ModelFactory.getModel<Ca_ClasProgramaticaModel>(clasprogramatica.GetByID(x => x.Id_ClasificacionP == Id), new Ca_ClasProgramaticaModel()));
        }
        #endregion

        #region Ca_Colonias
        public ActionResult V_Colonias()
        {
            List<Ca_ColoniasModel> Lst = new List<Ca_ColoniasModel>();
            IEnumerable<Ca_Colonias> entities = colonias.Get();
            foreach (Ca_Colonias item in entities)
            {
                try
                {
                    Ca_ColoniasModel model = ModelFactory.getModel<Ca_ColoniasModel>(item, new Ca_ColoniasModel());
                    model.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                    model.CA_Municipio = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado), new Ca_MunicipiosModel());
                    model.CA_Localidad = ModelFactory.getModel<Ca_LocalidadesModel>(localidades.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado && x.Id_Localidad == model.Id_Localidad), new Ca_LocalidadesModel());
                    Lst.Add(model);
                }
                catch (Exception ex)
                {

                }

            }
            return View(Lst);
        }
        public ActionResult V_ColoniasDetalles(Int16 IdEstado, Int16 IdMunicipio, Int16 IdLocalidad, Int16 IdColonia)
        {
            Ca_ColoniasModel model = ModelFactory.getModel<Ca_ColoniasModel>(colonias.GetByID(x => x.Id_Localidad == IdLocalidad && x.Id_Estado == IdEstado && x.Id_Municipio == IdMunicipio && x.id_colonia == IdColonia), new Ca_ColoniasModel());
            model.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
            model.CA_Municipio = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado), new Ca_MunicipiosModel());
            model.CA_Localidad = ModelFactory.getModel<Ca_LocalidadesModel>(localidades.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado && x.Id_Localidad == model.Id_Localidad), new Ca_LocalidadesModel());
            return View(model);
        }
        public ActionResult V_Colonias_Guardar()
        {
            Ca_ColoniasModel model = new Ca_ColoniasModel();
            model.ListaIdEstado = new SelectList(estados.Get(), "Id_Estado", "Descripcion");
            model.ListaIdMunicipio = new SelectList(municipios.Get(x => x.Id_Estado == model.Id_Estado), "Id_Municipio", "Descripcion");
            model.ListaIdLocalidad = new SelectList(localidades.Get(x => x.Id_Estado == model.Id_Estado && x.Id_Municipio == model.Id_Municipio), "Id_Localidad", "Descripcion");
            return View(model);
        }
        [HttpPost]
        public ActionResult V_Colonias_Guardar(Ca_Colonias model)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;

            try
            {
                if (colonias.Get(x => x.Id_Estado == model.Id_Estado && x.Id_Municipio == model.Id_Municipio && x.Id_Localidad == model.Id_Localidad).Count() == 0)
                    model.id_colonia = 1;
                else
                    model.id_colonia = Convert.ToInt16(colonias.Get(x => x.Id_Estado == model.Id_Estado && x.Id_Municipio == model.Id_Municipio && x.Id_Localidad == model.Id_Localidad).Max(x => x.id_colonia) + 1);
                model.Usu_Act = (Int16)appUsuario.IdUsuario;
                model.Fecha_Act = DateTime.Now;
                colonias.Insert(model);
                colonias.Save();
                Ca_ColoniasModel modelo = ModelFactory.getModel<Ca_ColoniasModel>(model, new Ca_ColoniasModel());
                modelo.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                modelo.CA_Municipio = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado), new Ca_MunicipiosModel());
                modelo.CA_Localidad = ModelFactory.getModel<Ca_LocalidadesModel>(localidades.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado && x.Id_Localidad == model.Id_Localidad), new Ca_LocalidadesModel());
                return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = modelo });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult V_Colonias_Editar(Int16 IdEstado, Int16 IdMunicipio, Int16 IdLocalidad, Int16 IdColonia)
        {
            Ca_ColoniasModel model = ModelFactory.getModel<Ca_ColoniasModel>(colonias.GetByID(x => x.Id_Estado == IdEstado && x.Id_Municipio == IdMunicipio && x.Id_Localidad == IdLocalidad && x.id_colonia == IdColonia), new Ca_ColoniasModel());
            model.ListaIdEstado = new SelectList(estados.Get(), "Id_Estado", "Descripcion", model.Id_Estado);
            model.ListaIdMunicipio = new SelectList(municipios.Get(x => x.Id_Estado == model.Id_Estado), "Id_Municipio", "Descripcion", model.Id_Municipio);
            model.ListaIdLocalidad = new SelectList(localidades.Get(x => x.Id_Estado == model.Id_Estado && x.Id_Municipio == model.Id_Municipio), "Id_Localidad", "Descripcion");
            return View(model);
        }
        [HttpPost]
        public ActionResult V_Colonias_Editar(Ca_Colonias model)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;

            try
            {
                model.Usu_Act = (Int16)appUsuario.IdUsuario;
                model.Fecha_Act = DateTime.Now;
                colonias.Update(model);
                colonias.Save();
                Ca_ColoniasModel modelo = ModelFactory.getModel<Ca_ColoniasModel>(model, new Ca_ColoniasModel());
                modelo.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                modelo.CA_Municipio = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado), new Ca_MunicipiosModel());
                modelo.CA_Localidad = ModelFactory.getModel<Ca_LocalidadesModel>(localidades.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado && x.Id_Localidad == model.Id_Localidad), new Ca_LocalidadesModel());
                return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = modelo });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult V_Colonias_Eliminar(Int16 IdEstado, Int16 IdMunicipio, Int16 IdLocalidad, Int16 IdColonia)
        {
            try
            {
                colonias.Delete(x => x.Id_Estado == IdEstado && x.Id_Municipio == IdMunicipio && x.Id_Localidad == IdLocalidad && x.id_colonia == IdColonia);
                colonias.Save();
                return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult V_ColoniasGuardar(byte Estado, short Municipio, short Localidad, string Colonia)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                Ca_ColoniasModel modelo = new Ca_ColoniasModel();
                Ca_Colonias model = new Ca_Colonias();
                model.Id_Estado = Estado;
                model.Id_Municipio = Municipio;
                model.Id_Localidad = Localidad;
                short Id = modelo.Id_Calculate(Estado, Municipio, Localidad);
                model.id_colonia = Id;
                model.Descripcion = Colonia;
                model.Usu_Act = (short)appUsuario.IdUsuario;
                model.Fecha_Act = DateTime.Now;
                colonias.Insert(model);
                colonias.Save();
                return new JsonResult() { Data = new SelectList(colonias.Get(x => x.Id_Estado == Estado && x.Id_Municipio == Municipio && x.Id_Localidad == Localidad), "id_colonia", "Descripcion", Id) };
            }
            catch
            {
                return new JsonResult() { Data = new SelectList(new List<Ca_Colonias>(), "id_colonia", "Descripcion") };
            }
        }
        #endregion

        #region Ca_ConceptoRubroIngresos

        #endregion

        #region Ca_ConceptoIngresos
        public ActionResult V_ConceptoIngresos()
        {
            List<Ca_ConceptosIngresosModel> Lst = new List<Ca_ConceptosIngresosModel>();
            conceptoingresos.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_ConceptosIngresosModel>(x, new Ca_ConceptosIngresosModel())); });
            return View(Lst);
        }

        public ActionResult V_ConceptoIngresosDetalles(string Id)
        {
            return View(ModelFactory.getModel<Ca_ConceptosIngresosModel>(conceptoingresos.GetByID(x => x.Id_Concepto == Id), new Ca_ConceptosIngresosModel()));
        }

        public ActionResult V_ConceptoIngresosAgregar()
        {
            return View(new Ca_ConceptosIngresosModel());
        }

        [HttpPost]
        public ActionResult V_ConceptoIngresosAgregar(Ca_ConceptosIngresos modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            Ca_ConceptosIngresosModel model = new Ca_ConceptosIngresosModel();
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    if (model.ValidaAdd(modelo.Rubro, modelo.Tipo, modelo.Clase, modelo.Descripcion, modelo.UltimoNivel, ref msn) == true)
                    {
                        modelo.Id_Concepto = StringID.IdConceptoIngreso(modelo.Rubro, modelo.Tipo, modelo.Clase);
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                        modelo.Fecha_Act = DateTime.Now;
                        if (modelo.Rubro > 0 && modelo.Tipo > 0 && modelo.Clase > 0) modelo.UltimoNivel = true;
                        conceptoingresos.Insert(modelo);
                        conceptoingresos.Save();
                        return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }

        public ActionResult V_ConceptoIngresosEditar(string Id)
        {
            return View(ModelFactory.getModel<Ca_ConceptosIngresosModel>(conceptoingresos.GetByID(x => x.Id_Concepto == Id), new Ca_ConceptosIngresosModel()));
        }

        [HttpPost]
        public ActionResult V_ConceptoIngresosEditar(Ca_ConceptosIngresos modelo)
        {
            Ca_ConceptosIngresosModel model = new Ca_ConceptosIngresosModel();
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    if (model.ValidaEdit(modelo.Rubro, modelo.Tipo, modelo.Clase, modelo.Descripcion, modelo.UltimoNivel, ref msn) == true)
                    {
                        modelo.Fecha_Act = DateTime.Now;
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                        if (modelo.Rubro > 0 && modelo.Tipo > 0 && modelo.Clase > 0) modelo.UltimoNivel = true;
                        conceptoingresos.Update(modelo);
                        conceptoingresos.Save();
                        return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Registro = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        public ActionResult V_ConceptoIngresosEliminar(string Id)
        {
            try
            {
                Ca_ConceptosIngresosModel model = new Ca_ConceptosIngresosModel();
                string msn = "";
                Ca_ConceptosIngresosModel modelo = ModelFactory.getModel<Ca_ConceptosIngresosModel>(conceptoingresos.GetByID(x => x.Id_Concepto == Id), new Ca_ConceptosIngresosModel());
                if (model.ValidaDelete(modelo.Rubro, modelo.Tipo, modelo.Clase, ref msn))
                {
                    conceptoingresos.Delete(m => m.Id_Concepto == Id);
                    conceptoingresos.Save();
                    return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente", Datos = modelo }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Exito = false, Mensaje = msn }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Ca_Cuentas

        public void BtnsCuentas(string IdCuenta, ref bool BtnAgregar, ref bool BtnEditar, ref bool BtnEliminar, ref bool BtnDetalles)
        {
            Ca_CuentasModel model = new Ca_CuentasModel();
            CA_Cuentas cta = cuentas.GetByID(x => x.Id_Cuenta == IdCuenta);
            BtnDetalles = true;
            BtnEditar = true;
            if (cta.UNivel_Armonizado == true)
            {
                BtnEditar = false;
                BtnEliminar = false;
            }
            if (cta.Nivel == true)
            {
                BtnAgregar = false;
                if (cta.Genero == 4 || cta.Genero == 5 || cta.Genero == 8 || cta.UNivel_Armonizado == true)
                    BtnEliminar = false;
                else
                    BtnEliminar = true;
            }
            else
            {
                if (cta.Genero == 4 || cta.Genero == 5 || cta.Genero == 8)
                {
                    BtnAgregar = false;
                    BtnEditar = false;
                    BtnEliminar = false;
                }
                else
                {
                    BtnAgregar = model.ValidaNoHijos(cta.Genero, cta.Grupo, cta.Rubro, cta.Cuenta, cta.SubCuentaO1, cta.SubCuentaO2, cta.SubCuentaO3, cta.SubCuentaO4);
                    string msn = "";
                    BtnEliminar = model.ValidaDelete(cta.Genero, cta.Grupo, cta.Rubro, cta.Cuenta, cta.SubCuentaO1, cta.SubCuentaO2, cta.SubCuentaO3, cta.SubCuentaO4, ref msn);
                }
            }
        }

        public ActionResult V_Cuentas()
        {
            List<Ca_CuentasModel> ctaLstM = new List<Ca_CuentasModel>();

            IEnumerable<CA_Cuentas> ctaLst = cuentas.Get(x => x.Genero == 1).OrderBy(x => x.Id_CuentaFormato);
            foreach (CA_Cuentas item in ctaLst)
            {
                Ca_CuentasModel model = ModelFactory.getModel<Ca_CuentasModel>(cuentas.GetByID(x => x.Id_Cuenta == item.Id_Cuenta), new Ca_CuentasModel());
                bool BtnAgregar = false, BtnDetalles = false, BtnEditar = false, BtnEliminar = false;
                BtnsCuentas(model.Id_Cuenta, ref BtnAgregar, ref BtnEditar, ref BtnEliminar, ref BtnDetalles);
                model.BtnAgregar = BtnAgregar;
                model.BtnDetalles = BtnDetalles;
                model.BtnEditar = BtnEditar;
                model.BtnEliminar = BtnEliminar;
                ctaLstM.Add(model);
            }
            return View(ctaLstM);
        }

        public CA_Cuentas CtaSiguiente(string IdCuenta)
        {
            Ca_CuentasModel model = new Ca_CuentasModel();
            CuentasDAL dal = new CuentasDAL();
            CA_Cuentas cta = dal.GetByID(x => x.Id_Cuenta == IdCuenta);
            byte Genero = 0;
            byte Grupo = 0;
            byte Rubro = 0;
            byte Cuenta = 0;
            byte SubCuentaO1 = 0;
            int SubCuentaO2 = 0;
            short SubCuentaO3 = 0;
            int SubCuentaO4 = 0;
            model.Consecutivo(cta.Genero, cta.Grupo, cta.Rubro, cta.Cuenta, cta.SubCuentaO1, cta.SubCuentaO2, cta.SubCuentaO3, cta.SubCuentaO4, ref Genero, ref Grupo, ref Rubro, ref Cuenta, ref SubCuentaO1, ref SubCuentaO2, ref SubCuentaO3, ref SubCuentaO4);
            return new CA_Cuentas() { Genero = Genero, Grupo = Grupo, Rubro = Rubro, Cuenta = Cuenta, SubCuentaO1 = SubCuentaO1, SubCuentaO2 = SubCuentaO2, SubCuentaO3 = SubCuentaO3, SubCuentaO4 = SubCuentaO4 };
        }

        public ActionResult V_Cuentas_Agregar(string IdCuenta)
        {
            Ca_CuentasModel model = new Ca_CuentasModel();
            CuentasDAL dal = new CuentasDAL();
            CA_Cuentas cta = CtaSiguiente(IdCuenta);
            ViewBag.Agregar = true;
            return View(ModelFactory.getModel<Ca_CuentasModel>(cta, new Ca_CuentasModel()));
        }

        [HttpPost]
        public ActionResult V_Cuentas_Agregar(CA_Cuentas modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            Ca_CuentasModel model = new Ca_CuentasModel();
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    if (model.ValidaNoHijos(modelo.Genero, modelo.Grupo, modelo.Rubro, modelo.Cuenta, modelo.SubCuentaO1, modelo.SubCuentaO2, modelo.SubCuentaO3, modelo.SubCuentaO4))
                    {
                        if (model.ValidaIdCuenta(modelo.Genero, modelo.Grupo, modelo.Rubro, modelo.Cuenta, modelo.SubCuentaO1, modelo.SubCuentaO2, modelo.SubCuentaO3, modelo.SubCuentaO4, ref msn))
                        {
                            modelo.Id_Cuenta = StringID.IdCuenta(modelo.Genero, modelo.Grupo, modelo.Rubro, modelo.Cuenta, modelo.SubCuentaO1, modelo.SubCuentaO2, modelo.SubCuentaO3, modelo.SubCuentaO4);
                            modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                            modelo.Fecha_Act = DateTime.Now;
                            if (modelo.Genero > 0 && modelo.Grupo > 0 && modelo.Rubro > 0 && modelo.Cuenta > 0 && modelo.SubCuentaO1 > 0 && modelo.SubCuentaO2 > 0 && modelo.SubCuentaO3 > 0 && modelo.SubCuentaO4 > 0) modelo.Nivel = true;
                            modelo.Naturaleza = model.ValidaNaturaleza(modelo.Genero, modelo.Grupo, modelo.Rubro, modelo.Cuenta, modelo.SubCuentaO1, modelo.SubCuentaO2, modelo.SubCuentaO3, modelo.SubCuentaO4);
                            cuentas.Insert(modelo);
                            cuentas.Save();

                            model = ModelFactory.getModel<Ca_CuentasModel>(modelo, new Ca_CuentasModel());
                            bool BtnAgregar = false, BtnDetalles = false, BtnEditar = false, BtnEliminar = false;
                            BtnsCuentas(model.Id_Cuenta, ref BtnAgregar, ref BtnEditar, ref BtnEliminar, ref BtnDetalles);
                            model.BtnAgregar = BtnAgregar;
                            model.BtnDetalles = BtnDetalles;
                            model.BtnEditar = BtnEditar;
                            model.BtnEliminar = BtnEliminar;
                            model.Id_CuentaFormato = StringID.IdCuentaFormato(modelo.Genero, modelo.Grupo, modelo.Rubro, modelo.Cuenta, modelo.SubCuentaO1, modelo.SubCuentaO2, modelo.SubCuentaO3, modelo.SubCuentaO4);

                            return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = model });
                        }
                        else
                            return Json(new { Exito = false, Mensaje = msn });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = "No se Puede Agregar La Cuenta, Verifique que no sobrepase el número de cuentas del nivel correspondiente." });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }

        public ActionResult V_Cuentas_Detalles(string IdCuenta)
        {
            return View(new Llenado().LLenado_CaCuentas(IdCuenta));
        }

        public ActionResult V_Cuentas_Editar(string IdCuenta)
        {
            ViewBag.Agregar = false;
            return View("V_Cuentas_Agregar", ModelFactory.getModel<Ca_CuentasModel>(cuentas.GetByID(x => x.Id_Cuenta == IdCuenta), new Ca_CuentasModel()));
        }

        [HttpPost]
        public ActionResult V_Cuentas_Editar(CA_Cuentas modelo)
        {
            Ca_CuentasModel model = new Ca_CuentasModel();
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    if (modelo.Nivel == true)
                        if (!model.ValidaDelete(modelo.Genero, modelo.Grupo, modelo.Rubro, modelo.Cuenta, modelo.SubCuentaO1, modelo.SubCuentaO2, modelo.SubCuentaO3, modelo.SubCuentaO4, ref msn) == true)
                            return Json(new { Exito = false, Mensaje = "No se puede Editar la Cuenta porque tiene cuentas descendentes" });

                    string Id_Cuenta = StringID.IdCuenta(modelo.Genero, modelo.Grupo, modelo.Rubro, modelo.Cuenta, modelo.SubCuentaO1, modelo.SubCuentaO2, modelo.SubCuentaO3, modelo.SubCuentaO4);
                    CA_Cuentas entity = cuentas.GetByID(x => x.Id_Cuenta == Id_Cuenta);
                    entity.Descripcion = modelo.Descripcion;
                    entity.Nivel = modelo.Nivel;
                    entity.Observaciones = modelo.Observaciones;
                    entity.Fecha_Act = DateTime.Now;
                    entity.Usu_Act = (Int16)appUsuario.IdUsuario;
                    if (modelo.Genero > 0 && modelo.Grupo > 0 && modelo.Rubro > 0 && modelo.Cuenta > 0 && modelo.SubCuentaO1 > 0 && modelo.SubCuentaO2 > 0 && modelo.SubCuentaO3 > 0 && modelo.SubCuentaO4 > 0) entity.Nivel = true;
                    cuentas.Update(entity);
                    cuentas.Save();

                    model = ModelFactory.getModel<Ca_CuentasModel>(entity, new Ca_CuentasModel());
                    bool BtnAgregar = false, BtnDetalles = false, BtnEditar = false, BtnEliminar = false;
                    BtnsCuentas(model.Id_Cuenta, ref BtnAgregar, ref BtnEditar, ref BtnEliminar, ref BtnDetalles);
                    model.BtnAgregar = BtnAgregar;
                    model.BtnDetalles = BtnDetalles;
                    model.BtnEditar = BtnEditar;
                    model.BtnEliminar = BtnEliminar;
                    return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Registro = model });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }

        [HttpPost]
        public ActionResult V_Cuentas_Eliminar(string IdCuenta)
        {
            try
            {
                Ca_CuentasModel model = new Ca_CuentasModel();
                string msn = "";
                CA_Cuentas modelo = cuentas.GetByID(x => x.Id_Cuenta == IdCuenta);
                if (model.UNivel_Armonizado == false)
                    if (model.ValidaDelete(modelo.Genero, modelo.Grupo, modelo.Rubro, modelo.Cuenta, modelo.SubCuentaO1, modelo.SubCuentaO2, modelo.SubCuentaO3, modelo.SubCuentaO4, ref msn))
                    {
                        cuentas.Delete(m => m.Id_Cuenta == IdCuenta);
                        cuentas.Save();
                        return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente", Datos = modelo }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Exito = false, Mensaje = "No se puede eliminar Cuenta de último nivel armonizado" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Tree
        public ActionResult V_Cuentas_Tree()
        {
            ViewBag.MesIni = Diccionarios.Meses;
            ViewBag.MesFin = Diccionarios.Meses;
            //ViewBag.NivelDesc = StringID.Dicc_NivelCuenta();
            return View();
        }

        [HttpPost]
        public ActionResult V_Cuentas_TreeDatos(byte MesIni, byte MesFin, string IdCuenta)
        {
            Tree2 tree = new Tree2();
            return Json(tree.Lista(MesIni, MesFin, IdCuenta));
            //return Json(tree.ListaCuentas(Id));
        }

        public ActionResult V_Cuentas_Meses(string IdCuenta, byte MesIni, byte MesFin)
        {
            Tree2 tree = new Tree2();
            bool[] meses = new bool[13];
            ViewBag.Polizas = depolizas.Get(x => x.Id_Cuenta == IdCuenta && x.Fecha.Value.Month >= MesIni && x.Fecha.Value.Month <= MesFin).Count();
            ViewBag.MesIni = MesIni;
            ViewBag.MesFin = MesFin;
            for (byte i = MesIni; i <= MesFin; i++)
            {
                int c = depolizas.Get(x => x.Id_Cuenta == IdCuenta && x.Fecha.Value.Month == i).Count();
                if (i == 1) if (c > 0) meses[i] = true; else meses[1] = false;
                if (i == 2) if (c > 0) meses[i] = true; else meses[2] = false;
                if (i == 3) if (c > 0) meses[i] = true; else meses[3] = false;
                if (i == 4) if (c > 0) meses[i] = true; else meses[4] = false;
                if (i == 5) if (c > 0) meses[i] = true; else meses[5] = false;
                if (i == 6) if (c > 0) meses[i] = true; else meses[6] = false;
                if (i == 7) if (c > 0) meses[i] = true; else meses[7] = false;
                if (i == 8) if (c > 0) meses[i] = true; else meses[8] = false;
                if (i == 9) if (c > 0) meses[i] = true; else meses[9] = false;
                if (i == 10) if (c > 0) meses[i] = true; else meses[10] = false;
                if (i == 11) if (c > 0) meses[i] = true; else meses[11] = false;
                if (i == 12) if (c > 0) meses[i] = true; else meses[12] = false;
            }
            ViewBag.Meses = meses;
            return View(tree.DatosCuentas(IdCuenta));
        }

        public ActionResult V_Cuentas_Polizas(string IdCuenta, byte MesIni, byte MesFin)
        {
            //Tree2 tree = new Tree2();
            ViewBag.IdCuentaFormato = cuentas.GetByID(x => x.Id_Cuenta.Equals(IdCuenta)).Id_CuentaFormato;
            ViewBag.IdCuenta = cuentas.GetByID(x => x.Id_Cuenta.Equals(IdCuenta)).Id_Cuenta;
            ViewBag.Cuenta = cuentas.GetByID(x => x.Id_Cuenta.Equals(IdCuenta)).Descripcion;
            ViewBag.MesIni = MesIni;
            ViewBag.MesFin = MesFin;
            //tree.DatosCuentas(IdCuenta).Id_CuentaFormato;
            List<Ma_PolizasModel> ctaLst = new List<Ma_PolizasModel>();
            IEnumerable<De_Polizas> entities = depolizas.Get(x => x.Id_Cuenta.Equals(IdCuenta) && x.Fecha.Value.Month >= MesIni && x.Fecha.Value.Month <= MesFin);
            foreach (De_Polizas item in entities)
            {
                ctaLst.Add(new Llenado().LLenado_MaPolizas(item.Id_TipoPoliza, item.Id_FolioPoliza, item.Id_MesPoliza));
            }
            return View(ctaLst);
        }

        [HttpPost]
        public ActionResult V_Cuentas_Polizas_Detalle(byte IdTipoPoliza, int IdFolioPoliza, byte IdMesPoliza)
        {
            List<De_PolizasModel> ctaLst = new List<De_PolizasModel>();
            IEnumerable<De_Polizas> entities = depolizas.Get(x => x.Id_TipoPoliza == IdTipoPoliza && x.Id_FolioPoliza == IdFolioPoliza && x.Id_MesPoliza == IdMesPoliza);
            foreach (De_Polizas item in entities)
            {
                ctaLst.Add(new Llenado().LLenado_DePolizas(item.Id_TipoPoliza, item.Id_FolioPoliza, item.Id_MesPoliza, item.Id_Registro));
            }
            return Json(ctaLst, "json");
        }

        public ActionResult V_Cuentas_Polizas_Reporte(byte TipoPoliza, int FolioPoliza, byte MesPoliza)
        {
            Ma_PolizasModel dataModal = new Llenado().LLenado_MaPolizas(TipoPoliza, FolioPoliza, MesPoliza);
            ConvertHtmlToString pdf = new ConvertHtmlToString();
            //, "Poliza " + StringID.PolizasFormato(dataModal.Id_TipoPoliza, dataModal.Id_MesPoliza, dataModal.Id_FolioPoliza) 
            return File(pdf.GenerarPDF_Horizontal("V_Cuentas_Polizas_Reporte", dataModal, this.ControllerContext), "application/pdf");
        }

        [HttpPost]
        public ActionResult PresupuestoEgFormato(string IdClave)
        {
            if (string.IsNullOrEmpty(IdClave)) return Json("");
            return Json(StringID.IdClavePresupuestoFormato(IdClave));
        }

        #endregion

        #endregion

        #region Ca_CuentasBancarias

        public ActionResult v_CuentasBancarias()
        {

            List<Ca_CuentasBancariasModel> Lst = new List<Ca_CuentasBancariasModel>();
            IEnumerable<Ca_CuentasBancarias> entities = cuentasbancarias.Get();
            foreach (Ca_CuentasBancarias item in entities)
            {
                Ca_CuentasBancariasModel model = ModelFactory.getModel<Ca_CuentasBancariasModel>(item, new Ca_CuentasBancariasModel());
                model.Ca_Bancos = ModelFactory.getModel<Ca_BancosModel>(bancos.GetByID(x => x.Id_Banco == model.Id_Banco), new Ca_BancosModel());
                model.Ca_Cuentas = ModelFactory.getModel<Ca_CuentasModel>(cuentas.GetByID(x => x.Id_Cuenta == model.Id_Cuenta), new Ca_CuentasModel());
                model.Ca_Fuentes = ModelFactory.getModel<Ca_FuentesFinModel>(fuentesfin.GetByID(x => x.Id_Fuente == model.Id_Fuente), new Ca_FuentesFinModel());
                Lst.Add(model);
            }
            return View(Lst);
        }
        public ActionResult V_CuentasBancariasAgregar()
        {
            List<Ca_Bancos> listaBancos = bancos.Get().ToList();
            List<Ca_BancosModel> listaModels = new List<Ca_BancosModel>();
            foreach (Ca_Bancos item in listaBancos)
            {
                Ca_BancosModel temp = ModelFactory.getModel<Ca_BancosModel>(item, new Ca_BancosModel());
                temp.Ca_Cuentas = ModelFactory.getModel<Ca_CuentasModel>(cuentas.GetByID(x => x.Id_Cuenta == temp.Id_Cuenta), new Ca_CuentasModel());
                temp.BancoFormato = String.Format("{0}  {1}", temp.Descripcion, temp.Ca_Cuentas.Id_CuentaFormato);
                listaModels.Add(temp);
            }
            ViewBag.Bancos = listaModels;
            Ca_CuentasBancariasModel model = new Ca_CuentasBancariasModel();
            model.ListaFuentes = new SelectList(fuentesfin.Get(), "Id_Fuente", "Descripcion");
            return View(model);
        }
        public ActionResult V_CuentasBancariasEditar(Int16 IdCuenta)
        {
            Ca_CuentasBancarias model = cuentasbancarias.GetByID(x => x.Id_CtaBancaria == IdCuenta);
            Ca_CuentasBancariasModel temp = ModelFactory.getModel<Ca_CuentasBancariasModel>(model, new Ca_CuentasBancariasModel());
            temp.Ca_Bancos = ModelFactory.getModel<Ca_BancosModel>(bancos.GetByID(x => x.Id_Banco == temp.Id_Banco), new Ca_BancosModel());
            IEnumerable<DE_Banco_Cheque> entities = debancocheque.Get(x => x.Id_CtaBancaria == temp.Id_CtaBancaria);
            if (entities != null && entities.Count() > 0)
                ViewBag.Detalle = true;
            else
                ViewBag.Detalle = false;
            temp.ListaFuentes = new SelectList(fuentesfin.Get(), "Id_Fuente", "Descripcion", temp.Id_Fuente);
            return View(temp);
        }
        [HttpPost]
        public ActionResult V_CuentasBancariasEditar(Ca_CuentasBancariasModel dataModel)
        {
            try
            {
                if (dataModel.NoChequeIni.HasValue && dataModel.NoChequeFin.HasValue)
                {
                    int inicio = dataModel.NoChequeIni.Value;
                    int fin = dataModel.NoChequeFin.Value;
                    for (int i = inicio; i <= fin; i++)
                    {
                        DE_Bancos temp = debanco.GetByID(x => x.Id_CtaBancaria == dataModel.Id_CtaBancaria && x.No_Cheque == i);
                        if (temp == null)
                            return Json(new { Exito = false, Mensaje = "El rango de cheques no es válido, favor de verificarlo." });
                        if (temp.Id_Estatus == 2)
                            return Json(new { Exito = false, Mensaje = "Algún cheque ya usado, favor de verificarlo." });
                    }
                }
                cuentasbancarias.Update(EntityFactory.getEntity<Ca_CuentasBancarias>(dataModel, new Ca_CuentasBancarias()));
                cuentasbancarias.Save();
                //Ca_CuentasBancariasModel model = ModelFactory.getModel<Ca_CuentasBancariasModel>(dataModel, new Ca_CuentasBancariasModel());
                dataModel.CuentaBancaria = String.Format("{0} {1}", dataModel.Descripcion, dataModel.NoCuenta);
                dataModel.Ca_Bancos = ModelFactory.getModel<Ca_BancosModel>(bancos.GetByID(x => x.Id_Banco == dataModel.Id_Banco), new Ca_BancosModel());
                dataModel.Ca_Fuentes = ModelFactory.getModel<Ca_FuentesFinModel>(fuentesfin.GetByID(x => x.Id_Fuente == dataModel.Id_Fuente), new Ca_FuentesFinModel());
                return Json(new { Exito = true, Mensaje = "Registro guardado correctamente", Registro = dataModel });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        public ActionResult V_NuevoCheque(Int16 IdCuenta)
        {
            DE_Banco_ChequeModel model = new DE_Banco_ChequeModel();
            model.Id_CtaBancaria = IdCuenta;
            return View(model);
        }
        public ActionResult V_ControlCheques(Int16 IdCuenta)
        {
            Ca_CuentasBancariasModel model = ModelFactory.getModel<Ca_CuentasBancariasModel>(cuentasbancarias.GetByID(x => x.Id_CtaBancaria == IdCuenta), new Ca_CuentasBancariasModel());
            ViewBag.CuentaFormato = String.Format("{0} {1}", model.Descripcion, model.NoCuenta);
            ViewBag.Cuenta = model.Id_CtaBancaria;
            List<DE_Banco_ChequeModel> Lst = new List<DE_Banco_ChequeModel>();
            IEnumerable<DE_Banco_Cheque> entities = debancocheque.Get(x => x.Id_CtaBancaria == IdCuenta);
            foreach (DE_Banco_Cheque item in entities)
            {
                DE_Banco_ChequeModel temp = ModelFactory.getModel<DE_Banco_ChequeModel>(item, new DE_Banco_ChequeModel());
                temp.Ca_CuentasBancarias = ModelFactory.getModel<Ca_CuentasBancariasModel>(cuentasbancarias.GetByID(x => x.Id_CtaBancaria == temp.Id_CtaBancaria), new Ca_CuentasBancariasModel());
                Lst.Add(temp);
            }
            return View(Lst);
        }
        public ActionResult V_CuentasBancariasValidar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ObetenerCuenta(int idBanco)
        {
            try
            {
                Ca_CuentasModel model = new Ca_CuentasModel();
                CuentasDAL dal = new CuentasDAL();
                string IdCuentaBanco = bancos.GetByID(x => x.Id_Banco == idBanco).Id_Cuenta;
                CA_Cuentas cta = dal.GetByID(x => x.Id_Cuenta == IdCuentaBanco);
                byte Genero = 0;
                byte Grupo = 0;
                byte Rubro = 0;
                byte Cuenta = 0;
                byte SubCuentaO1 = 0;
                int SubCuentaO2 = 0;
                short SubCuentaO3 = 0;
                int SubCuentaO4 = 0;
                model.Consecutivo(cta.Genero, cta.Grupo, cta.Rubro, cta.Cuenta, cta.SubCuentaO1, cta.SubCuentaO2, cta.SubCuentaO3, cta.SubCuentaO4, ref Genero, ref Grupo, ref Rubro, ref Cuenta, ref SubCuentaO1, ref SubCuentaO2, ref SubCuentaO3, ref SubCuentaO4);
                string id = StringID.IdCuenta(Genero, Grupo, Rubro, Cuenta, SubCuentaO1, SubCuentaO2, SubCuentaO3, SubCuentaO4);
                //string id = StringID.IdCuentaFormato(Genero, Grupo, Rubro, Cuenta, SubCuentaO1, SubCuentaO2, SubCuentaO3, SubCuentaO4);
                return Json(new { Exito = true, Id = id });

            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        public bool ValidarCuenta(int idBanco)
        {
            Ca_CuentasModel model = new Ca_CuentasModel();
            CuentasDAL dal = new CuentasDAL();
            string IdCuentaBanco = bancos.GetByID(x => x.Id_Banco == idBanco).Id_Cuenta;
            CA_Cuentas cta = dal.GetByID(x => x.Id_Cuenta == IdCuentaBanco);
            string msn = "";
            byte Genero = 0;
            byte Grupo = 0;
            byte Rubro = 0;
            byte Cuenta = 0;
            byte SubCuentaO1 = 0;
            int SubCuentaO2 = 0;
            short SubCuentaO3 = 0;
            int SubCuentaO4 = 0;
            model.Consecutivo(cta.Genero, cta.Grupo, cta.Rubro, cta.Cuenta, cta.SubCuentaO1, cta.SubCuentaO2, cta.SubCuentaO3, cta.SubCuentaO4, ref Genero, ref Grupo, ref Rubro, ref Cuenta, ref SubCuentaO1, ref SubCuentaO2, ref SubCuentaO3, ref SubCuentaO4);
            return model.ValidaIdCuenta(Genero, Grupo, Rubro, Cuenta, SubCuentaO1, SubCuentaO2, SubCuentaO3, SubCuentaO4, ref msn);

        }
        [HttpPost]
        public ActionResult GuardarCuenta(Ca_CuentasBancariasModel dataModel)
        {
            try
            {
                /*if (cuentasbancarias.Get(x => x.Id_Fuente == dataModel.Id_Fuente).Count() > 0)
                    return Json(new { Exito = false, Mensaje = "La Fuente de Financiamiento ya fue registrada a otra cuenta bancaria.", Registro = dataModel });*/
                if (ValidarCuenta(dataModel.Id_Banco.Value))
                {
                    Ca_CuentasModel model = new Ca_CuentasModel();
                    string IdCuentaBanco = bancos.GetByID(x => x.Id_Banco == dataModel.Id_Banco).Id_Cuenta;
                    CA_Cuentas cta = cuentas.GetByID(x => x.Id_Cuenta == IdCuentaBanco);
                    CA_Cuentas temp = new CA_Cuentas();
                    byte Genero = 0;
                    byte Grupo = 0;
                    byte Rubro = 0;
                    byte Cuenta = 0;
                    byte SubCuentaO1 = 0;
                    int SubCuentaO2 = 0;
                    short SubCuentaO3 = 0;
                    int SubCuentaO4 = 0;
                    model.Consecutivo(cta.Genero, cta.Grupo, cta.Rubro, cta.Cuenta, cta.SubCuentaO1, cta.SubCuentaO2, cta.SubCuentaO3, cta.SubCuentaO4, ref Genero, ref Grupo, ref Rubro, ref Cuenta, ref SubCuentaO1, ref SubCuentaO2, ref SubCuentaO3, ref SubCuentaO4);
                    temp.Genero = Genero;
                    temp.Grupo = Grupo;
                    temp.Rubro = Rubro;
                    temp.Cuenta = Cuenta;
                    temp.SubCuentaO1 = SubCuentaO1;
                    temp.SubCuentaO2 = SubCuentaO2;
                    temp.SubCuentaO3 = SubCuentaO3;
                    temp.SubCuentaO4 = SubCuentaO4;
                    temp.Descripcion = dataModel.Descripcion;
                    string id = StringID.IdCuenta(Genero, Grupo, Rubro, Cuenta, SubCuentaO1, SubCuentaO2, SubCuentaO3, SubCuentaO4);
                    string idF = StringID.IdCuentaFormato(Genero, Grupo, Rubro, Cuenta, SubCuentaO1, SubCuentaO2, SubCuentaO3, SubCuentaO4);
                    temp.Id_Cuenta = id;
                    temp.Id_CuentaFormato = idF;
                    temp.Nivel = true;
                    temp.Naturaleza = 2;
                    cuentas.Insert(temp);
                    cuentas.Save();
                }
                dataModel.TipoFoliador = 2;
                if (cuentasbancarias.Get().Count() > 0)
                    dataModel.Id_CtaBancaria = Convert.ToInt16(cuentasbancarias.Get().Max(x => x.Id_CtaBancaria) + 1);
                else
                    dataModel.Id_CtaBancaria = 1;
                //dataModel.Id_CtaBancaria++;
                cuentasbancarias.Insert(EntityFactory.getEntity<Ca_CuentasBancarias>(dataModel, new Ca_CuentasBancarias()));
                cuentasbancarias.Save();
                //Ca_CuentasBancariasModel model = ModelFactory.getModel<Ca_CuentasBancariasModel>(dataModel, new Ca_CuentasBancariasModel());
                dataModel.CuentaBancaria = String.Format("{0} {1}", dataModel.Descripcion, dataModel.NoCuenta);
                dataModel.Ca_Bancos = ModelFactory.getModel<Ca_BancosModel>(bancos.GetByID(x => x.Id_Banco == dataModel.Id_Banco), new Ca_BancosModel());
                dataModel.Ca_Fuentes = ModelFactory.getModel<Ca_FuentesFinModel>(fuentesfin.GetByID(x => x.Id_Fuente == dataModel.Id_Fuente), new Ca_FuentesFinModel());
                return Json(new { Exito = true, Mensaje = "Registro guardado correctamente", Registro = dataModel });
            }
            catch (Exception ex)
            {
                cuentas.Delete(x => x.Id_Cuenta == dataModel.Id_Cuenta);
                cierrebanco.DeleteAll(cierrebanco.Get(x => x.Id_CtaBancaria == dataModel.Id_CtaBancaria).ToList());
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        [HttpPost]
        public ActionResult GuardarCheque(DE_Banco_ChequeModel dataModel)
        {
            try
            {
                dataModel.Fecha_Asigna = DateTime.Now;
                if (debancocheque.Get(x => x.Id_CtaBancaria == dataModel.Id_CtaBancaria).Count() > 0)
                {
                    dataModel.Id_Asignacion = debancocheque.Get(x => x.Id_CtaBancaria == dataModel.Id_CtaBancaria).Max(x => x.Id_Asignacion);
                    dataModel.Id_Asignacion++;
                }
                else
                    dataModel.Id_Asignacion = 1;
                debancocheque.Insert(EntityFactory.getEntity<DE_Banco_Cheque>(dataModel, new DE_Banco_Cheque()));
                int inicio = dataModel.Cheque_Ini.Value;
                int fin = dataModel.Cheque_Fin.Value;
                for (int i = inicio; i <= fin; i++)
                {
                    DE_Bancos temp = new DE_Bancos();
                    temp.Id_CtaBancaria = dataModel.Id_CtaBancaria;
                    temp.No_Cheque = i;
                    temp.Id_Estatus = 1;
                    debanco.Insert(temp);
                    debanco.Save();
                }
                debancocheque.Save();
                dataModel.Ca_CuentasBancarias = ModelFactory.getModel<Ca_CuentasBancariasModel>(cuentasbancarias.GetByID(x => x.Id_CtaBancaria == dataModel.Id_CtaBancaria), new Ca_CuentasBancariasModel());
                return Json(new { Exito = true, Mensaje = "Registro guardado correctamente", Registro = dataModel });

            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        [HttpPost]
        public JsonResult ValidarCuenta(string password)
        {
            try
            {
                CA_Parametros parametro = parametros.GetByID(x => x.Nombre == "PASS_Cheques");
                if (password == parametro.Valor)
                    return Json(new { Exito = true, Mensaje = "OK" });
                else
                    return Json(new { Exito = false, Mensaje = "Password incorrecta" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public JsonResult EditarCuentasBancarias(Ca_CuentasBancariasModel dataModel)
        {
            try
            {
                Ca_CuentasBancarias banco = cuentasbancarias.GetByID(x => x.Id_CtaBancaria == dataModel.Id_CtaBancaria);
                if (banco != null)
                {
                    UsuarioLogueado appUSuario = Logueo.GetUsrLogueado();
                    dataModel.Usu_Act = (short)appUSuario.IdUsuario;
                    dataModel.Fecha_Act = DateTime.Now;
                    cuentasbancarias.Update(EntityFactory.getEntity<Ca_CuentasBancarias>(dataModel, new Ca_CuentasBancarias()));
                    cuentasbancarias.Save();
                }
                return Json(new { Exito = true, Mensaje = "OK", Registro = dataModel });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        [HttpPost]
        public JsonResult EliminarCheque(Int16 idAsignacion, Int16 idCta)
        {
            try
            {
                DE_Banco_Cheque entity = debancocheque.GetByID(x => x.Id_Asignacion == idAsignacion && x.Id_CtaBancaria == idCta);
                if (debanco.Get(x => x.Id_CtaBancaria == idCta && x.Id_Estatus != 1 && x.No_Cheque <= entity.Cheque_Fin && x.No_Cheque >= entity.Cheque_Ini).Count() > 0)
                    return Json(new { Exito = false, Mensaje = "No se puede eliminar porque al menos un cheque esta siendo utilizado." }, JsonRequestBehavior.AllowGet);
                debancocheque.Delete(x => x.Id_CtaBancaria == idCta && x.Id_Asignacion == idAsignacion);
                debancocheque.Save();
                debanco.DeleteAll(debanco.Get(x => x.Id_CtaBancaria == idCta && x.No_Cheque <= entity.Cheque_Fin && x.No_Cheque >= entity.Cheque_Ini).ToList());
                debanco.Save();
                return Json(new { Exito = true, Mensaje = "Registro eliminado correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult EliminarCuenta(int IdCuenta)
        {
            try
            {
                Ca_CuentasBancarias model = cuentasbancarias.GetByID(x => x.Id_CtaBancaria == IdCuenta);
                IEnumerable<DE_Banco_Cheque> entities = debancocheque.Get(x => x.Id_CtaBancaria == model.Id_CtaBancaria);
                if (entities != null && entities.Count() > 0)
                {
                    if (model.NoChequeIni != null)
                    {
                        int inicio = model.NoChequeIni.Value, fin = model.NoChequeFin.Value;
                        for (int i = inicio; i <= fin; i++)
                        {
                            DE_Bancos temp = debanco.GetByID(x => x.Id_CtaBancaria == model.Id_CtaBancaria && x.No_Cheque == i);
                            if (temp.Id_Estatus == 2)
                                return Json(new { Exito = false, Mensaje = "No se puede eliminar porque uno o mas cheques están en uso." });
                        }
                    }
                    List<DE_Banco_Cheque> listaBancosCheque = debancocheque.Get(x => x.Id_CtaBancaria == model.Id_CtaBancaria).ToList();
                    debancocheque.DeleteAll(listaBancosCheque);
                    debancocheque.Save();
                    List<DE_Bancos> listaBancos = debanco.Get(x => x.Id_CtaBancaria == model.Id_CtaBancaria).ToList();
                    debanco.DeleteAll(listaBancos);
                    debanco.Save();

                }
                List<CA_CierreBanco> listaCierre = cierrebanco.Get(x => x.Id_CtaBancaria == model.Id_CtaBancaria).ToList();
                cierrebanco.DeleteAll(listaCierre);
                cierrebanco.Save();
                cuentasbancarias.Delete(x => x.Id_CtaBancaria == model.Id_CtaBancaria);
                cuentasbancarias.Save();
                cuentas.Delete(x => x.Id_Cuenta == model.Id_Cuenta);
                cuentas.Save();
                return Json(new { Exito = true, Mensaje = "Registro eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DetallesCuentasBancarias(Int16 Id)
        {
            return View(ModelFactory.getModel<Ca_CuentasBancariasModel>(cuentasbancarias.GetByID(x => x.Id_CtaBancaria == Id), new Ca_CuentasBancariasModel()));
        }

        #endregion

        #region Ca_CUR
        public ActionResult V_CUR()
        {

            List<CA_CURModel> Lst = new List<CA_CURModel>();
            CUR.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<CA_CURModel>(x, new CA_CURModel())); });
            return View(Lst);
        }
        public ActionResult V_CURDetalles(string Id)
        {
            string[] valores = Id.Split('|');
            return View(ModelFactory.getModel<CA_CURModel>(CUR.GetByID2(valores[0],
                valores[1],
                valores[2],
                valores[3],
                valores[4],
                valores[5],
                valores[6],
                valores[7],
                valores[8],
                valores[9]), new CA_CURModel()));
        }
        #endregion

        #region Ca_Estados
        public ActionResult V_Estado()
        {
            List<Ca_EstadosModel> Lst = new List<Ca_EstadosModel>();
            estados.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_EstadosModel>(x, new Ca_EstadosModel())); });
            return View(Lst);
        }
        public ActionResult V_Estado_Detalles(byte? Id)
        {
            if (Id != null && Id != 0)
                return View(ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == Id), new Ca_EstadosModel()));
            return View(new Ca_EstadosModel());
        }

        public ActionResult V_Estado_Agregar()
        {
            return View(new Ca_EstadosModel());
        }

        [HttpPost]
        public ActionResult V_Estado_Agregar(Ca_Estados modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            Ca_EstadosModel model = new Ca_EstadosModel();
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    modelo.Id_Estado = 0;
                    //if (model.ValidaDesc(modelo.Descripcion, ref msn) == true)
                    //{
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                    modelo.Fecha_Act = DateTime.Now;
                    estados.Insert(modelo);
                    estados.Save();
                    return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = modelo });
                    //}
                    //else
                    //    return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        public ActionResult V_Estado_Editar(byte Id)
        {
            return View(ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == Id), new Ca_EstadosModel()));
        }
        [HttpPost]
        public ActionResult V_Estado_Editar(Ca_Estados modelo)
        {
            Ca_EstadosModel model = new Ca_EstadosModel();
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    //if (model.ValidaEdit(modelo.Id_TipoBene, modelo.Descripcion, ref msn) == true)
                    //{
                    modelo.Fecha_Act = DateTime.Now;
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;

                    estados.Update(modelo);
                    estados.Save();
                    return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Registro = modelo });
                    //}
                    //else
                    //    return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }

        public ActionResult V_Estado_Eliminar(byte Id)
        {
            try
            {
                Ca_EstadosModel model = new Ca_EstadosModel();
                string msn = "";
                Ca_EstadosModel modelo = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == Id), new Ca_EstadosModel());

                estados.Delete(m => m.Id_Estado == Id);
                estados.Save();
                return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente", Registro = modelo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Ca_EstatusRequisiciones

        #endregion

        #region Ca_FuentesFin
        public ActionResult V_FuentesFin()
        {
            List<Ca_FuentesFinModel> Lst = new List<Ca_FuentesFinModel>();
            fuentesfin.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_FuentesFinModel>(x, new Ca_FuentesFinModel())); });
            return View(Lst);
        }
        public ActionResult V_FuentesFinDetalles(string Id)
        {
            return View(ModelFactory.getModel<Ca_FuentesFinModel>(fuentesfin.GetByID(x => x.Id_Fuente == Id), new Ca_FuentesFinModel()));
        }
        #endregion

        #region Ca_FuentesFinIng
        public ActionResult V_FuentesFinIng()
        {

            List<Ca_FuentesFin_IngModel> Lst = new List<Ca_FuentesFin_IngModel>();
            fuentesfinIng.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_FuentesFin_IngModel>(x, new Ca_FuentesFin_IngModel())); });
            return View(Lst);
        }
        public ActionResult V_FuentesFinIngDetalles(string Id)
        {
            return View(ModelFactory.getModel<Ca_FuentesFin_IngModel>(fuentesfinIng.GetByID(x => x.Id_FuenteFinancia == Id), new Ca_FuentesFin_IngModel()));
        }
        public ActionResult V_FuentesFinIngAgregar()
        {
            return View(new Ca_FuentesFin_IngModel());
        }

        [HttpPost]
        public ActionResult V_FuentesFinIngAgregar(Ca_FuentesFin_Ing modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            Ca_FuentesFin_IngModel model = new Ca_FuentesFin_IngModel();
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    if (model.ValidaAdd(modelo.Id_Fuente, modelo.Id_Aportacion, modelo.Id_Convenio, modelo.Descripcion, modelo.UltimoNivel, ref msn) == true)
                    {
                        modelo.Id_FuenteFinancia = StringID.IdFuenteFin(modelo.Id_Fuente, modelo.Id_Aportacion, modelo.Id_Convenio);
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                        modelo.Fecha_Act = DateTime.Now;
                        if (modelo.Id_Fuente > 0 && modelo.Id_Aportacion > 0 && modelo.Id_Convenio > 0)
                            modelo.UltimoNivel = true;
                        else
                            modelo.UltimoNivel = false;
                        fuentesfinIng.Insert(modelo);
                        fuentesfinIng.Save();
                        return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        public ActionResult V_FuentesFinIngEditar(string Id)
        {
            return View(ModelFactory.getModel<Ca_FuentesFin_IngModel>(fuentesfinIng.GetByID(x => x.Id_FuenteFinancia == Id), new Ca_FuentesFin_IngModel()));
        }

        [HttpPost]
        public ActionResult V_FuentesFinIngEditar(Ca_FuentesFin_Ing modelo)
        {

            Ca_FuentesFin_IngModel model = new Ca_FuentesFin_IngModel();
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    if (model.ValidaEdit(modelo.Id_Fuente, modelo.Id_Aportacion, modelo.Id_Convenio, modelo.Descripcion, modelo.UltimoNivel, ref msn) == true)
                    {
                        modelo.Fecha_Act = DateTime.Now;
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                        if (modelo.Id_Fuente > 0 && modelo.Id_Aportacion > 0 && modelo.Id_Convenio > 0) modelo.UltimoNivel = true;
                        fuentesfinIng.Update(modelo);
                        fuentesfinIng.Save();
                        return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Registro = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        public ActionResult V_FuentesFinIngEliminar(string Id)
        {
            try
            {
                Ca_FuentesFin_IngModel model = new Ca_FuentesFin_IngModel();
                string msn = "";
                Ca_FuentesFin_IngModel modelo = ModelFactory.getModel<Ca_FuentesFin_IngModel>(fuentesfinIng.GetByID(x => x.Id_FuenteFinancia == Id), new Ca_FuentesFin_IngModel());
                if (model.ValidaDelete(modelo.Id_Fuente, modelo.Id_Aportacion, modelo.Id_Convenio, ref msn))
                {
                    fuentesfinIng.Delete(m => m.Id_FuenteFinancia == Id);
                    fuentesfinIng.Save();
                    return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente", Datos = modelo }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Exito = false, Mensaje = msn }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Ca_Funciones
        //LISTA DE LAS FUNCIONES
        public ActionResult V_Funciones()
        {
            List<Ca_FuncionesModel> funcionesLst = new List<Ca_FuncionesModel>();
            funciones.Get().ToList().ForEach(x => { funcionesLst.Add(ModelFactory.getModel<Ca_FuncionesModel>(x, new Ca_FuncionesModel())); });
            return View(funcionesLst);
        }
        //MODAL PARA OBTENER MODELO A EDITAR O GUARDAR
        public ActionResult V_AgregarFuncion(string IdFuncion)
        {
            if (IdFuncion != null)
                return View(ModelFactory.getModel<Ca_FuncionesModel>(funciones.GetByID(x => x.Id_Funcion == IdFuncion), new Ca_FuncionesModel()));//MODELO A EDITAR
            return View(new Ca_FuncionesModel());//MODELO NUEVO
        }
        //METODO PARA GUARDAR UNA FUNCION
        [HttpPost]
        public ActionResult GuardarFuncion(Ca_Funciones dataModel)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                Ca_FuncionesModel model = new Ca_FuncionesModel();
                bool Nuevo = false;
                //VALIDO SI MODELO ES NUEVO
                if (dataModel.Id_Funcion != null)
                {
                    //MODELO A EDITAR
                    string msn = "";
                    if (model.ValidaEdit(dataModel.Finalidad, dataModel.Funcion, dataModel.Subfuncion, dataModel.Descripcion, ref msn) == true)
                    {
                        dataModel.Usu_Act = (Int16)appUsuario.IdUsuario;
                        dataModel.Fecha_Act = DateTime.Now;
                        funciones.Update(dataModel);
                        funciones.Save();
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                //MODELO NUEVO
                else
                {
                    string msn = "";
                    if (model.ValidaAdd(dataModel.Finalidad, dataModel.Funcion, dataModel.Subfuncion, dataModel.Descripcion, ref msn) == true)
                    {
                        dataModel.Id_Funcion = StringID.IdFuncion(dataModel.Finalidad, dataModel.Funcion, dataModel.Subfuncion);
                        dataModel.Usu_Act = (Int16)appUsuario.IdUsuario;
                        dataModel.Fecha_Act = DateTime.Now;
                        funciones.Insert(dataModel);
                        funciones.Save();
                        Nuevo = true;
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Nuevo = Nuevo, Registro = dataModel });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = ex.Message });
            }
        }
        //METODO PARA ELIMINAR UNA FUNCION
        public ActionResult EliminarFuncion(string IdFuncion)
        {
            try
            {
                Ca_FuncionesModel model = new Ca_FuncionesModel();
                string msn = "";
                Ca_FuncionesModel modelo = ModelFactory.getModel<Ca_FuncionesModel>(funciones.GetByID(x => x.Id_Funcion == IdFuncion), new Ca_FuncionesModel());
                //VALIDAR QUE NO TENGA FUNCIONES ASCEDENTES O DESCENDENTES
                if (model.ValidaDelete(modelo.Finalidad, modelo.Funcion, modelo.Subfuncion, ref msn))
                {
                    funciones.Delete(m => m.Id_Funcion == IdFuncion);
                    funciones.Save();
                    return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente", Datos = modelo });
                }
                else
                {
                    return Json(new { Exito = false, Mensaje = msn });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = "Ocurrio un Error " + ex });
            }
        }
        //DETALLES DE UNA FUNCION
        public ActionResult V_DetallesFuncion(string IdFuncion)
        {
            return View(ModelFactory.getModel<Ca_FuncionesModel>(funciones.GetByID(x => x.Id_Funcion == IdFuncion), new Ca_FuncionesModel()));
        }
        #endregion

        #region Ca_Giros_Comerciales

        #endregion

        #region Ca_Impuestos_Deduccion
        [HttpGet]
        public ActionResult ImpuestosDeduccion()
        {
            return View(impuestodeduccion.Get());
        }
        [HttpGet]
        public ActionResult AddImpuestosDeduccion()
        {
            return View(new Ca_Impuestos_DeduccionModel());
        }
        [HttpPost]
        public ActionResult AddImpuestosDeduccion(Ca_Impuestos_DeduccionModel modelo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioLogueado Usuario = Session["appUsuario"] as UsuarioLogueado;
                    modelo.Usu_Act = (short)Usuario.IdUsuario;
                    modelo.Fecha_Act = DateTime.Now;
                    modelo.Id_ImpDed = (short)new ImpuestoDeduccionBL().GetId_ImpDed(modelo.Id_Tipo_ImpDed);
                    modelo.Id_ImpDed2 = StringID.IdImpuestoDeduccion((byte)modelo.Id_Tipo_ImpDed, (byte)modelo.Id_ImpDed);
                    impuestodeduccion.Insert(EntityFactory.getEntity<Ca_Impuestos_Deduccion>(modelo, new Ca_Impuestos_Deduccion()));
                    impuestodeduccion.Save();
                    return Json(new { Exito = true, Mensaje = "Registro guardado correctamente", Registro = modelo });
                }
                else
                {
                    return Json(new { Exito = false, Mensaje = "Ocurrió un error" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        public ActionResult GetImpuestoDeduccion(string Id_ImpDed2)
        {
            return View(ModelFactory.getModel<Ca_Impuestos_DeduccionModel>(impuestodeduccion.GetByID(x => x.Id_ImpDed2 == Id_ImpDed2), new Ca_Impuestos_DeduccionModel()));
        }
        [HttpGet]
        public ActionResult EditImpuestoDeduccion(string Id)
        {
            Ca_Impuestos_DeduccionModel modelo = ModelFactory.getModel<Ca_Impuestos_DeduccionModel>(impuestodeduccion.GetByID(x => x.Id_ImpDed2 == Id), new Ca_Impuestos_DeduccionModel());
            return View(modelo);
        }
        [HttpPost]
        public ActionResult EditImpuestoDeduccion(Ca_Impuestos_DeduccionModel modelo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioLogueado Usuario = Session["appUsuario"] as UsuarioLogueado;
                    modelo.Usu_Act = (short)Usuario.IdUsuario;
                    modelo.Fecha_Act = DateTime.Now;
                    impuestodeduccion.Update(EntityFactory.getEntity<Ca_Impuestos_Deduccion>(modelo, new Ca_Impuestos_Deduccion()));
                    impuestodeduccion.Save();
                    return Json(new { Exito = true, Mensaje = "Registro actualizado correctamente", Registro = modelo });
                }
                else
                {
                    return Json(new { Exito = false, Mensaje = "Ocurrió un error" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        public ActionResult DelImpuestoDeduccion(string Id)
        {
            try
            {
                impuestodeduccion.Delete(x => x.Id_ImpDed2 == Id);
                impuestodeduccion.Save();
                return Json(new { Exito = true, Mensaje = "Registro eliminado correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Ca_TiposDocumentos
        public ActionResult TipoDocumentos()
        {
            List<Ca_TipoDoctosModel> Lst = new List<Ca_TipoDoctosModel>();
            tipodoctos.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoDoctosModel>(x, new Ca_TipoDoctosModel())); });
            return View(Lst);
        }
        public ActionResult V_TipoDocumentos_Detalles(Int32 Id)
        {
            if (Id != null && Id != 0)
                return View(ModelFactory.getModel<Ca_TipoDoctosModel>(tipodoctos.GetByID(x => x.Id_Tipodocto == Id), new Ca_TipoDoctosModel()));
            return View(new Ca_TipoDoctosModel());
        }

        public ActionResult V_TipoDocumentos_Agregar()
        {
            return View(new Ca_TipoDoctosModel());
        }

        [HttpPost]
        public ActionResult V_TipoDocumentos_Agregar(Ca_TipoDoctos modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    modelo.Id_Tipodocto = 0;
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                    modelo.Fecha_Act = DateTime.Now;
                    tipodoctos.Insert(modelo);
                    tipodoctos.Save();
                    return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = modelo });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        public ActionResult V_TipoDocumentos_Editar(Int32 Id)
        {
            return View(ModelFactory.getModel<Ca_TipoDoctosModel>(tipodoctos.GetByID(x => x.Id_Tipodocto == Id), new Ca_TipoDoctosModel()));
        }
        [HttpPost]
        public ActionResult V_TipoDocumentos_Editar(Ca_TipoDoctos modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    modelo.Fecha_Act = DateTime.Now;
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                    tipodoctos.Update(modelo);
                    tipodoctos.Save();
                    return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Registro = modelo });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }

        public ActionResult V_TipoDocumentos_Eliminar(Int32 Id)
        {
            try
            {
                if (crFacturas.Get(x => x.Id_TipoDocto == Id).Count() == 0)
                {
                    tipodoctos.Delete(m => m.Id_Tipodocto == Id);
                    tipodoctos.Save();
                    return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Exito = false, Mensaje = "No se puede eliminar porque el tipo de documento esta siendo utilizado." }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ClasificacionPolizas
        public ActionResult ClasificacionPolizas()
        {

            List<Ca_ClasificaPolizasModel> Lst = new List<Ca_ClasificaPolizasModel>();
            clasificacionpolizas.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_ClasificaPolizasModel>(x, new Ca_ClasificaPolizasModel())); });
            return View(Lst);
        }
        public ActionResult V_ClasificacionPolizasDetalles(Int32 idtipo, Int32 idclasi, Int32 idsub)
        {
            Ca_ClasificaPolizas entitie = clasificacionpolizas.GetByID(x => x.Id_TipoPoliza == idtipo && x.Id_ClasificaPoliza == idclasi && x.Id_SubClasificaPoliza == idsub);
            return View(ModelFactory.getModel<Ca_ClasificaPolizasModel>(entitie, new Ca_ClasificaPolizasModel()));
        }
        #endregion

        #region Paises
        public ActionResult Paises()
        {
            return View();
        }
        #endregion

        #region DiasInhabiles
        public ActionResult DiasInhabiles()
        {
            List<CA_InHabilModel> Lst = new List<CA_InHabilModel>();
            diasInabil.Get().ToList().ForEach(x =>
            {
                CA_InHabilModel model = (ModelFactory.getModel<CA_InHabilModel>(x, new CA_InHabilModel()));
                model.MesLetra = Diccionarios.Meses[model.Mes.Value];
                Lst.Add(model);
            });
            return View(Lst);
        }
        public ActionResult V_DiasInhabiles_Detalles(Int32 Id)
        {
            if (Id != 0)
            {
                CA_InHabilModel model = (ModelFactory.getModel<CA_InHabilModel>(diasInabil.GetByID(x => x.Id_Dia == Id), new CA_InHabilModel()));
                model.MesLetra = Diccionarios.Meses[model.Mes.Value];
                return View(model);

            }
            return View(new CA_InHabilModel());
        }

        public ActionResult V_DiasInhabiles_Agregar()
        {
            ViewBag.FechaInicio = new DateTime(DateTime.Now.Year, 01, 01).ToShortDateString();
            ViewBag.FechaFin = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            return View(new CA_InHabilModel());
        }

        [HttpPost]
        public ActionResult V_DiasInhabiles_Agregar(CA_InHabilModel model)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    CA_InHabil modelo = new CA_InHabil();
                    modelo.Dia = Convert.ToByte(model.Fecha.Day);
                    modelo.Mes = Convert.ToByte(model.Fecha.Month);
                    modelo.Descrip = model.Descrip.Trim();
                    if (diasInabil.Get().Count() == 0)
                        modelo.Id_Dia = 1;
                    else
                        modelo.Id_Dia = Convert.ToInt16(diasInabil.Get().Max(x => x.Id_Dia) + 1);
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                    modelo.Fecha_Act = DateTime.Now;
                    diasInabil.Insert(modelo);
                    diasInabil.Save();
                    model = ModelFactory.getModel<CA_InHabilModel>(modelo, new CA_InHabilModel());
                    model.MesLetra = Diccionarios.Meses[model.Mes.Value];
                    return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = model });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        public ActionResult V_DiasInhabiles_Editar(Int32 Id)
        {
            CA_InHabilModel model = ModelFactory.getModel<CA_InHabilModel>(diasInabil.GetByID(x => x.Id_Dia == Id), new CA_InHabilModel());
            model.Fecha = new DateTime(DateTime.Now.Year, model.Mes.Value, model.Dia.Value);
            return View(model);
        }
        [HttpPost]
        public ActionResult V_DiasInhabiles_Editar(CA_InHabilModel model)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    CA_InHabil modelo = diasInabil.GetByID(x => x.Id_Dia == model.Id_Dia);
                    modelo.Dia = Convert.ToByte(model.Fecha.Day);
                    modelo.Mes = Convert.ToByte(model.Fecha.Month);
                    modelo.Descrip = model.Descrip.Trim();
                    modelo.Fecha_Act = DateTime.Now;
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                    diasInabil.Update(modelo);
                    diasInabil.Save();
                    model = ModelFactory.getModel<CA_InHabilModel>(modelo, new CA_InHabilModel());
                    model.MesLetra = Diccionarios.Meses[model.Mes.Value];
                    return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Registro = model });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }

        public ActionResult V_DiasInhabiles_Eliminar(Int32 Id)
        {
            try
            {
                diasInabil.Delete(m => m.Id_Dia == Id);
                diasInabil.Save();
                return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Ca_Localidades
        public ActionResult V_Localidades()
        {

            List<Ca_LocalidadesModel> Lst = new List<Ca_LocalidadesModel>();
            IEnumerable<Ca_Localidades> entities = localidades.Get();
            foreach (Ca_Localidades item in entities)
            {
                Ca_LocalidadesModel model = ModelFactory.getModel<Ca_LocalidadesModel>(item, new Ca_LocalidadesModel());
                model.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                model.CA_Municipio = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado), new Ca_MunicipiosModel());
                Lst.Add(model);
            }
            return View(Lst);
        }
        public ActionResult V_LocalidadesDetalles(Int16 IdLocalidad, Int16 IdEstado, Int16 IdMunicipio)
        {
            Ca_LocalidadesModel model = ModelFactory.getModel<Ca_LocalidadesModel>(localidades.GetByID(x => x.Id_Localidad == IdLocalidad && x.Id_Estado == IdEstado && x.Id_Municipio == IdMunicipio), new Ca_LocalidadesModel());
            model.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
            model.CA_Municipio = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado), new Ca_MunicipiosModel());
            return View(model);
        }
        public ActionResult V_Localidad_Guardar()
        {
            Ca_LocalidadesModel model = new Ca_LocalidadesModel();
            model.ListaIdEstado = new SelectList(estados.Get(), "Id_Estado", "Descripcion");
            model.ListaIdMunicipio = new SelectList(municipios.Get(x => x.Id_Estado == model.Id_Estado), "Id_Municipio", "Descripcion");
            return View(model);
        }
        [HttpPost]
        public ActionResult V_Localidad_Guardar(Ca_Localidades model)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;

            try
            {
                if (localidades.Get(x => x.Id_Estado == model.Id_Estado && x.Id_Municipio == model.Id_Municipio).Count() == 0)
                    model.Id_Localidad = 1;
                else
                    model.Id_Localidad = Convert.ToInt16(localidades.Get(x => x.Id_Estado == model.Id_Estado && x.Id_Municipio == model.Id_Municipio).Max(x => x.Id_Localidad) + 1);
                model.Usu_Act = (Int16)appUsuario.IdUsuario;
                model.Fecha_Act = DateTime.Now;
                localidades.Insert(model);
                localidades.Save();
                Ca_LocalidadesModel modelo = ModelFactory.getModel<Ca_LocalidadesModel>(model, new Ca_LocalidadesModel());
                modelo.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                modelo.CA_Municipio = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado), new Ca_MunicipiosModel());
                return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = modelo });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult V_Localidad_Editar(byte IdEstado, byte IdMunicipio, byte IdLocalidad)
        {
            Ca_LocalidadesModel model = ModelFactory.getModel<Ca_LocalidadesModel>(localidades.GetByID(x => x.Id_Estado == IdEstado && x.Id_Municipio == IdMunicipio && x.Id_Localidad == IdLocalidad), new Ca_LocalidadesModel());
            model.ListaIdEstado = new SelectList(estados.Get(), "Id_Estado", "Descripcion", model.Id_Estado);
            model.ListaIdMunicipio = new SelectList(municipios.Get(x => x.Id_Estado == model.Id_Estado), "Id_Municipio", "Descripcion", model.Id_Municipio);
            return View(model);
        }
        [HttpPost]
        public ActionResult V_Localidad_Editar(Ca_Localidades model)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;

            try
            {
                model.Usu_Act = (Int16)appUsuario.IdUsuario;
                model.Fecha_Act = DateTime.Now;
                localidades.Update(model);
                localidades.Save();
                Ca_LocalidadesModel modelo = ModelFactory.getModel<Ca_LocalidadesModel>(model, new Ca_LocalidadesModel());
                modelo.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                modelo.CA_Municipio = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == model.Id_Municipio && x.Id_Estado == model.Id_Estado), new Ca_MunicipiosModel());
                return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = modelo });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult V_Localidad_Eliminar(byte IdEstado, short IdMunicipio, short IdLocalidad)
        {
            try
            {
                localidades.Delete(x => x.Id_Estado == IdEstado && x.Id_Municipio == IdMunicipio && x.Id_Localidad == IdLocalidad);
                diasInabil.Save();
                return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult V_LocalidadGuardar(byte Estado, short Municipio, string Localidad)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;

            try
            {

                Ca_Localidades modelo = new Ca_Localidades();
                modelo.Id_Estado = Estado;
                modelo.Id_Municipio = (short)Municipio;
                modelo.Descripcion = Localidad.Trim();
                if (localidades.Get(x => x.Id_Estado == Estado && x.Id_Municipio == Municipio).Count() == 0)
                    modelo.Id_Localidad = 1;
                else
                    modelo.Id_Localidad = Convert.ToInt16(localidades.Get(x => x.Id_Estado == Estado && x.Id_Municipio == Municipio).Max(x => x.Id_Localidad) + 1);
                modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                modelo.Fecha_Act = DateTime.Now;
                localidades.Insert(modelo);
                localidades.Save();
                return new JsonResult() { Data = new SelectList(localidades.Get(x => x.Id_Estado == Estado && x.Id_Municipio == Municipio), "Id_Localidad", "Descripcion", modelo.Id_Localidad) };
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Ca_Municipios
        public ActionResult V_Municipio()
        {
            List<Ca_MunicipiosModel> Lst = new List<Ca_MunicipiosModel>();
            IEnumerable<Ca_Municipios> entities = municipios.Get();
            foreach (Ca_Municipios item in entities)
            {
                Ca_MunicipiosModel model = ModelFactory.getModel<Ca_MunicipiosModel>(item, new Ca_MunicipiosModel());
                model.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                Lst.Add(model);
            }
            return View(Lst);
        }
        public ActionResult V_Municipio_Detalles(byte? IdEstado, short? IdMunicipio)
        {
            ViewBag.estado = estados.Get().ToList();
            if (IdMunicipio != null && IdMunicipio != 0 && IdEstado != null && IdEstado != 0)
            {
                Ca_MunicipiosModel model = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == IdMunicipio && x.Id_Estado == IdEstado), new Ca_MunicipiosModel());
                model.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                return View(model);
            }
            return View(new Ca_MunicipiosModel());
        }

        public ActionResult V_Municipio_Agregar()
        {
            Ca_MunicipiosModel model = new Ca_MunicipiosModel();
            model.Lista_Estados = new SelectList(estados.Get(), "Id_Estado", "Descripcion", model.Id_Estado);
            return View(model);
        }

        [HttpPost]
        public ActionResult V_Municipio_Agregar(Ca_Municipios modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    modelo.Id_Municipio = 0;
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                    modelo.Fecha_Act = DateTime.Now;
                    municipios.Insert(modelo);
                    municipios.Save();
                    Ca_MunicipiosModel model = ModelFactory.getModel<Ca_MunicipiosModel>(modelo, new Ca_MunicipiosModel());
                    model.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == modelo.Id_Estado), new Ca_EstadosModel());
                    return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = model });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        [HttpPost]
        public ActionResult V_MunicipioGuardar(byte Estado, string Municipio)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;

            try
            {

                Ca_Municipios modelo = new Ca_Municipios();
                modelo.Id_Estado = Estado;
                modelo.Descripcion = Municipio.Trim();
                modelo.Id_Municipio = 0;
                modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                modelo.Fecha_Act = DateTime.Now;
                municipios.Insert(modelo);
                municipios.Save();
                return new JsonResult() { Data = new SelectList(municipios.Get(x => x.Id_Estado == Estado), "Id_Municipio", "Descripcion", modelo.Id_Municipio) };
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult V_Municipio_Editar(byte IdEstado, short IdMunicipio)
        {
            Ca_MunicipiosModel model = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == IdMunicipio && x.Id_Estado == IdEstado), new Ca_MunicipiosModel());
            model.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
            model.Lista_Estados = new SelectList(estados.Get(), "Id_Estado", "Descripcion", model.Id_Estado);
            return View(model);
        }
        [HttpPost]
        public ActionResult V_Municipio_Editar(Ca_Municipios modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    modelo.Fecha_Act = DateTime.Now;
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                    municipios.Update(modelo);
                    municipios.Save();
                    Ca_MunicipiosModel model = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == modelo.Id_Municipio && x.Id_Estado == modelo.Id_Estado), new Ca_MunicipiosModel());
                    model.CA_Estado = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == model.Id_Estado), new Ca_EstadosModel());
                    return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Registro = model });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }

        public ActionResult V_Municipio_Eliminar(byte IdEstado, short IdMunicipio)
        {
            try
            {
                Ca_MunicipiosModel model = new Ca_MunicipiosModel();
                string msn = "";
                Ca_MunicipiosModel modelo = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Municipio == IdMunicipio && x.Id_Estado == IdEstado), new Ca_MunicipiosModel());

                municipios.Delete(m => m.Id_Municipio == IdMunicipio && m.Id_Estado == IdEstado);
                municipios.Save();
                return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente", Registro = modelo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Ca_ObjetoGastos
        public ActionResult V_ObjetoGasto()
        {
            List<Ca_ObjetoGastoModel> Lst = new List<Ca_ObjetoGastoModel>();
            objetogasto.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_ObjetoGastoModel>(x, new Ca_ObjetoGastoModel())); });
            return View(Lst);
        }
        public ActionResult V_ObjetoGastoDetalles(string Id)
        {
            return View(ModelFactory.getModel<Ca_ObjetoGastoModel>(objetogasto.GetByID(x => x.Id_ObjetoG == Id), new Ca_ObjetoGastoModel()));
        }
        #endregion

        #region Ca_Paises
        public ActionResult V_Pais()
        {
            List<Ca_PaisesModel> Lst = new List<Ca_PaisesModel>();
            paises.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_PaisesModel>(x, new Ca_PaisesModel())); });
            return View(Lst);
        }
        public ActionResult V_Pais_Detalles(byte? Id)
        {
            if (Id != null && Id != 0)
                return View(ModelFactory.getModel<Ca_PaisesModel>(paises.GetByID(x => x.Id_Pais == Id), new Ca_PaisesModel()));
            return View(new Ca_PaisesModel());
        }

        public ActionResult V_Pais_Agregar()
        {
            return View(new Ca_PaisesModel());
        }

        [HttpPost]
        public ActionResult V_Pais_Agregar(Ca_Paises modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    modelo.Id_Pais = 0;
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                    modelo.Fecha_Act = DateTime.Now;
                    paises.Insert(modelo);
                    paises.Save();
                    return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Registro = modelo });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        public ActionResult V_Pais_Editar(byte Id)
        {
            return View(ModelFactory.getModel<Ca_PaisesModel>(paises.GetByID(x => x.Id_Pais == Id), new Ca_PaisesModel()));
        }
        [HttpPost]
        public ActionResult V_Pais_Editar(Ca_Paises modelo)
        {
            Ca_PaisesModel model = new Ca_PaisesModel();
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    modelo.Fecha_Act = DateTime.Now;
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                    paises.Update(modelo);
                    paises.Save();
                    return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Registro = modelo });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }

        public ActionResult V_Pais_Eliminar(byte Id)
        {
            try
            {
                paises.Delete(m => m.Id_Pais == Id);
                paises.Save();
                return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Ca_Parametros
        public ActionResult V_Parametros()
        {
            var error = Session["error"];
            Session.Remove("error");
            if (error != null)
            {
                if (!String.IsNullOrEmpty(error.ToString()))
                    ModelState.AddModelError("", error.ToString());
            }
            List<Ca_ParametrosModel> Lst = new List<Ca_ParametrosModel>();
            IEnumerable<CA_Parametros> entities = parametros.Get(x => x.Nombre == "Titulo1" || x.Nombre == "Titulo2" || x.Nombre == "Titulo3" || x.Nombre == "Titulo4" || x.Nombre == "EscudoReportes");
            foreach (CA_Parametros item in entities)
            {
                Ca_ParametrosModel model = ModelFactory.getModel<Ca_ParametrosModel>(item, new Ca_ParametrosModel());
                Lst.Add(model);
            }
            return View(Lst);
        }
        public ActionResult V_Parametros_Editar(Int16 IdParametro)
        {
            Ca_ParametrosModel model = ModelFactory.getModel<Ca_ParametrosModel>(parametros.GetByID(x => x.IdParametro == IdParametro), new Ca_ParametrosModel());
            return View(model);
        }
        [HttpPost]
        public ActionResult V_Parametros_Editar(CA_Parametros modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    modelo.fAct = DateTime.Now;
                    modelo.uAct = (Int16)appUsuario.IdUsuario;
                    parametros.Update(modelo);
                    parametros.Save();
                    Ca_ParametrosModel model = ModelFactory.getModel<Ca_ParametrosModel>(modelo, new Ca_ParametrosModel());
                    return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Registro = model });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        [HttpPost]
        public ActionResult V_Parametros_Editar_File(CA_Parametros modelo, HttpPostedFileBase file)
        {
            Session.Add("error", "");
            Session.Add("exito", false);
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {

                        string extension = System.IO.Path.GetExtension(file.FileName);
                        if (extension != ".jpg" && extension != ".png")
                            throw new ArgumentException("El archivo no es una imagen.");
                        string ruta = "~/Images/reportes/" + modelo.Valor;
                        var path = Path.Combine(Server.MapPath("~/Images/reportes/"), modelo.Valor);
                        file.SaveAs(path);
                        modelo.fAct = DateTime.Now;
                        modelo.uAct = (Int16)appUsuario.IdUsuario;
                        parametros.Update(modelo);
                        parametros.Save();
                        Ca_ParametrosModel model = ModelFactory.getModel<Ca_ParametrosModel>(modelo, new Ca_ParametrosModel());
                        return RedirectToAction("V_Parametros");
                    }
                    else
                        throw new ArgumentException("El archivo llego vacio.");

                }
                catch (Exception ex)
                {

                    Session["error"] = String.Format("Ocurrió un error: {0}", ex.Message);
                    return RedirectToAction("V_Parametros");
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        #endregion

        #region Ca_Personas
        public ActionResult V_Personas()
        {

            Ca_PersonasModel model = new Ca_PersonasModel();
            model.ListaIdEstado = new SelectList(estados.Get(), "Id_Estado", "Descripcion");
            model.ListaIdMunicipio = new SelectList(municipios.Get(x => x.Id_Estado == model.IdEstado), "Id_Municipio", "Descripcion");
            model.ListaIdLocalidad = new SelectList(localidades.Get(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio), "Id_Localidad", "Descripcion");
            model.ListaIdColonia = new SelectList(colonias.Get(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio && x.Id_Localidad == model.IdLocalidad), "id_colonia", "Descripcion");
            model.ListaIdCalle = new SelectList(calles.Get(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio && x.Id_Localidad == model.IdLocalidad), "id_calle", "Descripcion");
            //List<CA_Personas> entities = personas.Get().ToList();
            //List<Ca_PersonasModel> models = new List<Ca_PersonasModel>();
            //entities.ForEach(item => { models.Add(new Llenado().Llenado_CaPersonasBusqueda(item.IdPersona)); });
            ViewBag.ListaPersonas = new List<Ca_PersonasModel>(); //models;
            return View(model);
        }
        [HttpPost]
        public ActionResult V_Personas(Ca_PersonasModel modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            int IdPersona = 0;
            CA_Personas persona = ModelFactory.getModel<CA_Personas>(modelo, new CA_Personas());
            //Empezar a guardar Elementos
            IdPersona = new Ca_PersonasModel().Id_Calculate();
            persona.IdPersona = IdPersona;
            try
            {
                personas.Insert(persona);
                personas.Save();
            }
            catch
            {
                return Json(new { Exito = false, Mensaje = "Ocurió un error al guardar el Contribuyente", IdPersona = IdPersona });
            }
            //----------------------//

            return Json(new { Exito = true, Mensaje = "Guardado Correctamente", IdPersona = IdPersona });
        }
        [HttpPost]
        public ActionResult V_PersonasEdit(Ca_PersonasModel modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            CA_Personas persona = personas.GetByID(x => x.IdPersona == modelo.IdPersona);
            persona = EntityFactory.getEntity<CA_Personas>(modelo, persona);
            try
            {
                personas.Update(persona);
                personas.Save();
                return Json(new { Exito = true, Mensaje = "Guardado correctamente." });
            }
            catch
            {
                return Json(new { Exito = false, Mensaje = "No se guardo adecuadamente la persona" });
            }
        }
        [HttpPost]
        public ActionResult V_Personas_Buscar(int IdPersona)
        {
            return Json(new Llenado().Llenado_CaPersonas(IdPersona));
        }
        [HttpPost]
        public ActionResult V_PersonasDelete(int IdPersona)
        {
            try
            {
                if (beneficiarios.Get(x => x.IdPersona == IdPersona).Count() == 0)
                {
                    if (MaRecibosDAL.Get(x => x.IdContribuyente == IdPersona).Count() == 0)
                    {
                        personas.Delete(x => x.IdPersona == IdPersona);
                        personas.Save();
                        return Json(new { Exito = true, Mensaje = "Se Elimino Correctamente" });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = "No se puede eliminar porque la persona se esta usando Ma recibos." });
                }
                else
                    return Json(new { Exito = false, Mensaje = "No se puede eliminar porque la persona se esta usando en Beneficiarios." });

            }
            catch
            {
                return Json(new { Exito = false, Mensaje = "Error al borrar la persona: " + IdPersona });
            }
        }
        #endregion

        #region Ca_Percep_Deduc

        #endregion

        #region Ca_Programas
        public ActionResult V_Programas()
        {
            List<Ca_ProgramasModel> Lst = new List<Ca_ProgramasModel>();
            programas.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_ProgramasModel>(x, new Ca_ProgramasModel())); });
            return View(Lst);
        }
        public ActionResult V_AgregarProgramas()
        {
            return View(new Ca_ProgramasModel());
        }

        public ActionResult V_ObtenerProgramas(string Id)
        {
            if (Id != null)
                return View(ModelFactory.getModel<Ca_ProgramasModel>(programas.GetByID(x => x.Id_Programa == Id), new Ca_ProgramasModel()));
            return View(new Ca_ProgramasModel());
        }

        [HttpPost]
        public ActionResult EditarProgramas(CA_Programas dataModel)
        {
            try
            {

                Ca_ProgramasModel model = new Ca_ProgramasModel();
                string msn = "";
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                dataModel.Usu_Act = (Int16)appUsuario.IdUsuario;
                dataModel.Fecha_Act = DateTime.Now;
                programas.Update(dataModel);
                programas.Save();
                return Json(new { Exito = true, Mensaje = "Registro guardado correctamente", Registro = dataModel });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult AgregarProgramas(CA_Programas dataModel)
        {
            try
            {
                Ca_ProgramasModel model = new Ca_ProgramasModel();
                string msn = "";
                if (model.Valida(dataModel.Id_Programa, dataModel.Descripcion, ref msn) == true)
                {
                    UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                    dataModel.Usu_Act = (Int16)appUsuario.IdUsuario;
                    dataModel.Descripcion = dataModel.Descripcion.Trim();
                    dataModel.Fecha_Act = DateTime.Now;
                    programas.Insert(dataModel);
                    programas.Save();
                    return Json(new { Exito = true, Mensaje = "Registro guardado correctamente.", Registro = dataModel });
                }
                else
                    return Json(new { Exito = false, Mensaje = msn });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }

        public ActionResult EliminarProgramas(string Id)
        {
            try
            {
                programas.Delete(x => x.Id_Programa == Id);
                programas.Save();
                return Json(new { Exito = true, Mensaje = "Registro eliminado correctamente." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult V_DetallesProgramas(string Id)
        {
            return View(ModelFactory.getModel<Ca_ProgramasModel>(programas.GetByID(x => x.Id_Programa == Id), new Ca_ProgramasModel()));
        }
        #endregion

        #region Ca_Proyecto
        public ActionResult V_Proyectos()
        {
            List<Ca_ProyectoModel> Lst = new List<Ca_ProyectoModel>();
            proyecto.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_ProyectoModel>(x, new Ca_ProyectoModel())); });
            return View(Lst);
        }
        public ActionResult V_AgregarProyectos()
        {
            return View(new Ca_ProyectoModel());
        }

        public ActionResult V_ObtenerProyectos(string Id)
        {
            if (Id != null)
                return View(ModelFactory.getModel<Ca_ProyectoModel>(proyecto.GetByID(x => x.Id_Proceso == Id), new Ca_ProyectoModel()));
            return View(new Ca_ProyectoModel());
        }

        [HttpPost]
        public ActionResult EditarProyectos(Ca_Proyecto dataModel)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                dataModel.Usu_Act = (Int16)appUsuario.IdUsuario;
                dataModel.Fecha_Act = DateTime.Now;
                proyecto.Update(dataModel);
                proyecto.Save();
                return Json(new { Exito = true, Mensaje = "Registro guardado correctamente", Registro = dataModel });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult AgregarProyectos(Ca_Proyecto dataModel)
        {
            try
            {
                Ca_ProyectoModel model = new Ca_ProyectoModel();
                string msn = "";
                if (model.Valida(dataModel.Id_Proceso, dataModel.Descripcion, ref msn) == true)
                {
                    UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                    dataModel.Usu_Act = (Int16)appUsuario.IdUsuario;
                    dataModel.Descripcion = dataModel.Descripcion.Trim();
                    dataModel.Fecha_Act = DateTime.Now;
                    proyecto.Insert(dataModel);
                    proyecto.Save();
                    return Json(new { Exito = true, Mensaje = "Registro guardado correctamente.", Registro = dataModel });
                }
                else
                    return Json(new { Exito = false, Mensaje = msn });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }

        public ActionResult EliminarProyectos(string Id)
        {
            try
            {
                proyecto.Delete(x => x.Id_Proceso == Id);
                proyecto.Save();
                return Json(new { Exito = true, Mensaje = "Registro eliminado correctamente." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult V_DetallesProyectos(string Id)
        {
            return View(ModelFactory.getModel<Ca_ProyectoModel>(proyecto.GetByID(x => x.Id_Proceso == Id), new Ca_ProyectoModel()));
        }
        #endregion



        #region Ca_TipoBeneficiarios
        public ActionResult V_TipoBeneficiario()
        {
            List<Ca_TipoBeneficiariosModel> Lst = new List<Ca_TipoBeneficiariosModel>();
            tipobeneficiario.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoBeneficiariosModel>(x, new Ca_TipoBeneficiariosModel())); });
            return View(Lst);
        }
        public ActionResult V_TipoBeneficiario_Detalles(byte? Id)
        {
            if (Id != null && Id != 0)
                return View(ModelFactory.getModel<Ca_TipoBeneficiariosModel>(tipobeneficiario.GetByID(x => x.Id_TipoBene == Id), new Ca_TipoBeneficiariosModel()));
            return View(new Ca_TipoBeneficiariosModel());
        }

        public ActionResult V_TipoBeneficiario_Agregar()
        {
            return View(new Ca_TipoBeneficiariosModel());
        }

        [HttpPost]
        public ActionResult V_TipoBeneficiario_Agregar(Ca_TipoBeneficiarios modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            Ca_TipoBeneficiariosModel model = new Ca_TipoBeneficiariosModel();
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    modelo.Id_TipoBene = 0;
                    if (model.ValidaDesc(modelo.Descripcion, ref msn) == true)
                    {
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                        modelo.Fecha_Act = DateTime.Now;
                        tipobeneficiario.Insert(modelo);
                        tipobeneficiario.Save();
                        return Json(new { Exito = true, Mensaje = "Registro guardado exitosamente", Registro = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información no válida" });
            }
        }
        public ActionResult V_TipoBeneficiario_Editar(byte Id)
        {
            return View(ModelFactory.getModel<Ca_TipoBeneficiariosModel>(tipobeneficiario.GetByID(x => x.Id_TipoBene == Id), new Ca_TipoBeneficiariosModel()));
        }
        [HttpPost]
        public ActionResult V_TipoBeneficiario_Editar(Ca_TipoBeneficiarios modelo)
        {
            Ca_TipoBeneficiariosModel model = new Ca_TipoBeneficiariosModel();
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    if (model.ValidaEdit(modelo.Id_TipoBene, modelo.Descripcion, ref msn) == true)
                    {
                        modelo.Fecha_Act = DateTime.Now;
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;

                        tipobeneficiario.Update(modelo);
                        tipobeneficiario.Save();
                        return Json(new { Exito = true, Mensaje = "Registro modificado exitosamente", Registro = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información no válida" });
            }
        }

        public ActionResult V_TipoBeneficiario_Eliminar(byte Id)
        {
            try
            {
                Ca_TipoBeneficiariosModel model = new Ca_TipoBeneficiariosModel();
                string msn = "";
                Ca_TipoBeneficiariosModel modelo = ModelFactory.getModel<Ca_TipoBeneficiariosModel>(tipobeneficiario.GetByID(x => x.Id_TipoBene == Id), new Ca_TipoBeneficiariosModel());

                tipobeneficiario.Delete(m => m.Id_TipoBene == Id);
                tipobeneficiario.Save();
                return Json(new { Exito = true, Mensaje = "Registro eliminado exitosamente", Registro = modelo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = code == -2146233087 ? "El registro no puede ser eliminado porque ya ha sido utilizado en otra parte" : new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Ca_TipoCompromisos

        public ActionResult V_TipoCompromiso()
        {
            List<Ca_TipoCompromisosModel> Lst = new List<Ca_TipoCompromisosModel>();
            tipocompromiso.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoCompromisosModel>(x, new Ca_TipoCompromisosModel())); });
            return View("V_TipoCompromiso", Lst);
        }
        public ActionResult V_TipoCompromiso_Detalles(short? Id_TipoCompromiso)
        {
            if (Id_TipoCompromiso != null && Id_TipoCompromiso != 0)
                return View(ModelFactory.getModel<Ca_TipoCompromisosModel>(tipocompromiso.GetByID(x => x.Id_TipoCompromiso == Id_TipoCompromiso), new Ca_TipoCompromisosModel()));
            return View(new Ca_TipoCompromisosModel());
        }

        public ActionResult V_TipoCompromiso_Agregar()
        {
            return View(new Ca_TipoCompromisosModel());
        }

        [HttpPost]
        public ActionResult V_TipoCompromiso_Agregar(Ca_TipoCompromisos modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            Ca_TipoCompromisosModel model = new Ca_TipoCompromisosModel();
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    modelo.Id_TipoCompromiso = (short)new TipoCompromisoBL().getNextId();
                    if (modelo.Num_Dia == null)
                        modelo.Num_Dia = false;
                    if (modelo.Num_Dias == null)
                        modelo.Num_Dias = 0;
                    if (modelo.Num_Semana == null)
                        modelo.Num_Semana = false;
                    if (modelo.Num_Semanas == null)
                        modelo.Num_Semanas = 0;
                    if (modelo.Dia_Semana == null)
                        modelo.Dia_Semana = 0;
                    if (modelo.A_Qna == null)
                        modelo.A_Qna = false;
                    if (modelo.Dia_Antes_Q == null)
                        modelo.Dia_Antes_Q = 0;
                    if (modelo.Dia_Despues_Q == null)
                        modelo.Dia_Despues_Q = 0;
                    if (modelo.Dia_Mes == null)
                        modelo.Dia_Mes = false;
                    if (modelo.Dia1 == null)
                        modelo.Dia1 = 0;
                    if (modelo.Antes == null)
                        modelo.Antes = false;
                    if (modelo.Dia2 == null)
                        modelo.Dia2 = 0;
                    if (modelo.Despues == null)
                        modelo.Despues = false;
                    if (model.ValidaDesc(modelo.Descripcion, ref msn) == true)
                    {
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                        modelo.Fecha_Act = DateTime.Now;
                        modelo.Pagarse = new TipoCompromisoBL().obtenerFechaLetra(modelo);
                        tipocompromiso.Insert(modelo);
                        tipocompromiso.Save();
                        return Json(new { Exito = true, Mensaje = "Registro guardado exitosamente", Registro = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información no válida" });
            }
        }
        public ActionResult V_TipoCompromiso_Editar(short Id_TipoCompromiso)
        {
            return View(ModelFactory.getModel<Ca_TipoCompromisosModel>(tipocompromiso.GetByID(x => x.Id_TipoCompromiso == Id_TipoCompromiso), new Ca_TipoCompromisosModel()));
        }
        [HttpPost]
        public ActionResult V_TipoCompromiso_Editar(Ca_TipoCompromisos modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                Ca_TipoCompromisosModel model = new Ca_TipoCompromisosModel();
                try
                {
                    string msn = "";
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    if (modelo.Num_Dia == null)
                        modelo.Num_Dia = false;
                    if (modelo.Num_Dias == null)
                        modelo.Num_Dias = 0;
                    if (modelo.Num_Semana == null)
                        modelo.Num_Semana = false;
                    if (modelo.Num_Semanas == null)
                        modelo.Num_Semanas = 0;
                    if (modelo.Dia_Semana == null)
                        modelo.Dia_Semana = 0;
                    if (modelo.A_Qna == null)
                        modelo.A_Qna = false;
                    if (modelo.Dia_Antes_Q == null)
                        modelo.Dia_Antes_Q = 0;
                    if (modelo.Dia_Despues_Q == null)
                        modelo.Dia_Despues_Q = 0;
                    if (modelo.Dia_Mes == null)
                        modelo.Dia_Mes = false;
                    if (modelo.Dia1 == null)
                        modelo.Dia1 = 0;
                    if (modelo.Antes == null)
                        modelo.Antes = false;
                    if (modelo.Dia2 == null)
                        modelo.Dia2 = 0;
                    if (modelo.Despues == null)
                        modelo.Despues = false;
                    if (model.ValidaEdit(modelo.Id_TipoCompromiso, modelo.Descripcion, ref msn) == true)
                    {
                        modelo.Fecha_Act = DateTime.Now;
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                        modelo.Pagarse = new TipoCompromisoBL().obtenerFechaLetra(modelo);
                        tipocompromiso.Update(modelo);
                        tipocompromiso.Save();
                        return Json(new { Exito = true, Mensaje = "Registro modificado exitosamente", Registro = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    return Json(new { Exito = false, Mensaje = "Ocurrio un error " + ex });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información no válida" });
            }
        }
        [HttpPost]
        public ActionResult V_TipoCompromiso_Eliminar(short Id_TipoCompromiso)
        {
            try
            {
                Ca_TipoCompromisosModel model = new Ca_TipoCompromisosModel();
                string msn = "";
                Ca_TipoCompromisosModel modelo = ModelFactory.getModel<Ca_TipoCompromisosModel>(tipocompromiso.GetByID(x => x.Id_TipoCompromiso == Id_TipoCompromiso), new Ca_TipoCompromisosModel());

                tipocompromiso.Delete(m => m.Id_TipoCompromiso == Id_TipoCompromiso);
                tipocompromiso.Save();
                return Json(new { Exito = true, Mensaje = "Registro eliminado exitosamente", Datos = modelo });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = "Ocurrió un error " + ex });
            }
        }
        #endregion

        #region Ca_TipoContrarecibos
        public ActionResult V_TipoContrarecibo()
        {
            List<Ca_TipoContrarecibosModel> Lst = new List<Ca_TipoContrarecibosModel>();
            tipocontrarecibo.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoContrarecibosModel>(x, new Ca_TipoContrarecibosModel())); });
            return View("V_TipoContrarecibo", Lst);
        }
        public ActionResult V_TipoContrarecibo_Detalles(byte? Id_TipoContrarecibo)
        {
            if (Id_TipoContrarecibo != null && Id_TipoContrarecibo != 0)
                return View(ModelFactory.getModel<Ca_TipoContrarecibosModel>(tipocontrarecibo.GetByID(x => x.Id_TipoCR == Id_TipoContrarecibo), new Ca_TipoContrarecibosModel()));
            return View(new Ca_TipoContrarecibosModel());
        }

        public ActionResult V_TipoContrarecibo_Agregar()
        {
            return View(new Ca_TipoContrarecibosModel());
        }

        [HttpPost]
        public ActionResult V_TipoContrarecibo_Agregar(Ca_TipoContrarecibos modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            Ca_TipoContrarecibosModel model = new Ca_TipoContrarecibosModel();
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    modelo.Id_TipoCR = 0;
                    if (model.ValidaDesc(modelo.Descripcion, ref msn) == true)
                    {
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                        modelo.Fecha_Act = DateTime.Now;
                        tipocontrarecibo.Insert(modelo);
                        tipocontrarecibo.Save();
                        return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Datos = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    return Json(new { Exito = false, Mensaje = "Ocurrio un Error " + ex });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        public ActionResult V_TipoContrarecibo_Editar(byte Id_TipoContrarecibo)
        {
            return View(ModelFactory.getModel<Ca_TipoContrarecibosModel>(tipocontrarecibo.GetByID(x => x.Id_TipoCR == Id_TipoContrarecibo), new Ca_TipoContrarecibosModel()));
        }
        [HttpPost]
        public ActionResult V_TipoContrarecibo_Editar(Ca_TipoContrarecibos modelo)
        {
            Ca_TipoContrarecibosModel model = new Ca_TipoContrarecibosModel();
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    if (model.ValidaEdit(modelo.Id_TipoCR, modelo.Descripcion, ref msn) == true)
                    {
                        modelo.Fecha_Act = DateTime.Now;
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;

                        tipocontrarecibo.Update(modelo);
                        tipocontrarecibo.Save();
                        return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Datos = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    return Json(new { Exito = false, Mensaje = "Ocurrio un Error " + ex });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        [HttpPost]
        public ActionResult V_TipoContrarecibo_Eliminar(byte Id_TipoContrarecibo)
        {
            try
            {
                Ca_TipoContrarecibosModel model = new Ca_TipoContrarecibosModel();
                string msn = "";
                Ca_TipoContrarecibosModel modelo = ModelFactory.getModel<Ca_TipoContrarecibosModel>(tipocontrarecibo.GetByID(x => x.Id_TipoCR == Id_TipoContrarecibo), new Ca_TipoContrarecibosModel());

                tipocontrarecibo.Delete(m => m.Id_TipoCR == Id_TipoContrarecibo);
                tipocontrarecibo.Save();
                return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente", Datos = modelo });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = "Ocurrio un Error " + ex });
            }
        }
        #endregion

        #region Ca_TipoDoctos
        public ActionResult V_TipoDocto()
        {
            List<Ca_TipoDoctosModel> Lst = new List<Ca_TipoDoctosModel>();
            tipodoctos.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoDoctosModel>(x, new Ca_TipoDoctosModel())); });
            return View("V_TipoDocto", Lst);
        }
        public ActionResult V_TipoDocto_Detalles(byte? Id_TipoDocto)
        {
            if (Id_TipoDocto != null && Id_TipoDocto != 0)
                return View(ModelFactory.getModel<Ca_TipoDoctosModel>(tipodoctos.GetByID(x => x.Id_Tipodocto == Id_TipoDocto), new Ca_TipoDoctosModel()));
            return View(new Ca_TipoDoctosModel());
        }

        public ActionResult V_TipoDocto_Agregar()
        {
            return View(new Ca_TipoDoctosModel());
        }

        [HttpPost]
        public ActionResult V_TipoDocto_Agregar(Ca_TipoDoctos modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            Ca_TipoDoctosModel model = new Ca_TipoDoctosModel();
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    modelo.Id_Tipodocto = 0;
                    if (model.ValidaDesc(modelo.Descripcion, ref msn) == true)
                    {
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                        modelo.Fecha_Act = DateTime.Now;
                        tipodoctos.Insert(modelo);
                        tipodoctos.Save();
                        return Json(new { Exito = true, Mensaje = "Registro Guardado Exitosamente", Datos = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    return Json(new { Exito = false, Mensaje = "Ocurrio un Error " + ex });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        public ActionResult V_TipoDocto_Editar(byte Id_Tipodocto)
        {
            return View(ModelFactory.getModel<Ca_TipoDoctosModel>(tipodoctos.GetByID(x => x.Id_Tipodocto == Id_Tipodocto), new Ca_TipoDoctosModel()));
        }
        [HttpPost]
        public ActionResult V_TipoDocto_Editar(Ca_TipoDoctos modelo)
        {
            Ca_TipoDoctosModel model = new Ca_TipoDoctosModel();
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    if (model.ValidaEdit(modelo.Id_Tipodocto, modelo.Descripcion, ref msn) == true)
                    {
                        modelo.Fecha_Act = DateTime.Now;
                        modelo.Usu_Act = (Int16)appUsuario.IdUsuario;

                        tipodoctos.Update(modelo);
                        tipodoctos.Save();
                        return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Datos = modelo });
                    }
                    else
                        return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    return Json(new { Exito = false, Mensaje = "Ocurrio un Error " + ex });
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        [HttpPost]
        public ActionResult V_TipoDocto_Eliminar(byte Id_Tipodocto)
        {
            try
            {
                Ca_TipoDoctosModel model = new Ca_TipoDoctosModel();
                string msn = "";
                Ca_TipoDoctosModel modelo = ModelFactory.getModel<Ca_TipoDoctosModel>(tipodoctos.GetByID(x => x.Id_Tipodocto == Id_Tipodocto), new Ca_TipoDoctosModel());

                tipodoctos.Delete(m => m.Id_Tipodocto == Id_Tipodocto);
                tipodoctos.Save();
                return Json(new { Exito = true, Mensaje = "Registro Eliminado Exitosamente", Datos = modelo });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = "Ocurrio un Error " + ex });
            }
        }
        #endregion

        #region Ca_TipoFormatoCheques

        #endregion

        #region Ca_TipoGastos
        public ActionResult V_TipoGasto()
        {
            List<Ca_TipoGastosModel> Lst = new List<Ca_TipoGastosModel>();
            tipogasto.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoGastosModel>(x, new Ca_TipoGastosModel())); });
            return View(Lst);
        }
        public ActionResult V_TipoGastoDetalles(string Id)
        {
            return View(ModelFactory.getModel<Ca_TipoGastosModel>(tipogasto.GetByID(x => x.Id_TipoGasto == Id), new Ca_TipoGastosModel()));
        }
        #endregion

        #region Ca_TipoImpuestos
        //public ActionResult V_TipoImpuesto()
        //{
        //    List<Ca_TipoImpuestosModel> Lst = new List<Ca_TipoImpuestosModel>();
        //    tipoimpuesto.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoImpuestosModel>(x, new Ca_TipoImpuestosModel())); });
        //    return View(Lst);
        //}
        public ActionResult V_TipoImpuestoDetalles(string Id)
        {
            return View(ModelFactory.getModel<Ca_TipoImpuestosModel>(tipoimpuesto.GetByID(x => x.Id_TipoImpuesto == Id), new Ca_TipoImpuestosModel()));
        }
        #endregion

        #region Ca_Ingresos

        #endregion

        #region Ca_TipoMeta
        public ActionResult V_TipoMeta()
        {
            List<Ca_TipoMetaModel> Lst = new List<Ca_TipoMetaModel>();
            tipometa.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoMetaModel>(x, new Ca_TipoMetaModel())); });
            return View(Lst);
        }
        public ActionResult V_AgregarTipoMeta()
        {
            return View(new Ca_TipoMetaModel());
        }

        public ActionResult V_ObtenerTipoMeta(string Id)
        {
            if (Id != null)
                return View(ModelFactory.getModel<Ca_TipoMetaModel>(tipometa.GetByID(x => x.Id_TipoMeta == Id), new Ca_TipoMetaModel()));
            return View(new Ca_TipoMetaModel());
        }

        [HttpPost]
        public ActionResult EditarTipoMeta(Ca_TipoMeta dataModel)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                dataModel.Usu_Act = (Int16)appUsuario.IdUsuario;
                dataModel.Fecha_Act = DateTime.Now;
                tipometa.Update(dataModel);
                tipometa.Save();
                return Json(new { Exito = true, Mensaje = "Registro guardado correctamente", Registro = dataModel });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult AgregarTipoMeta(Ca_TipoMeta dataModel)
        {
            try
            {
                Ca_TipoMetaModel model = new Ca_TipoMetaModel();
                string msn = "";
                if (model.Valida(dataModel.Id_TipoMeta, dataModel.Descripcion, ref msn) == true)
                {
                    UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                    dataModel.Usu_Act = (Int16)appUsuario.IdUsuario;
                    dataModel.Descripcion = dataModel.Descripcion.Trim();
                    dataModel.Fecha_Act = DateTime.Now;
                    tipometa.Insert(dataModel);
                    tipometa.Save();
                    return Json(new { Exito = true, Mensaje = "Registro guardado correctamente.", Registro = dataModel });
                }
                else
                    return Json(new { Exito = false, Mensaje = msn });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }

        public ActionResult EliminarTipoMeta(string Id)
        {
            try
            {
                tipometa.Delete(x => x.Id_TipoMeta == Id);
                tipometa.Save();
                return Json(new { Exito = true, Mensaje = "Registro eliminado correctamente." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult V_DetallesTipoMeta(string Id)
        {
            return View(ModelFactory.getModel<Ca_TipoMetaModel>(tipometa.GetByID(x => x.Id_TipoMeta == Id), new Ca_TipoMetaModel()));
        }
        #endregion

        #region Ca_TipoMovBancarios
        public ActionResult V_TipoMovBancarios()
        {
            List<Ca_TipoMovBancariosModel> Lst = new List<Ca_TipoMovBancariosModel>();
            tipomovbancarios.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoMovBancariosModel>(x, new Ca_TipoMovBancariosModel())); });
            return View(Lst);
        }
        public ActionResult V_DetallesTipoMovBancarios(string Id)
        {
            return View(ModelFactory.getModel<Ca_TipoMovBancariosModel>(tipomovbancarios.GetByID(x => x.Id_MovBancario == Id), new Ca_TipoMovBancariosModel()));
        }
        #endregion

        #region Ca_TipoPagos
        public ActionResult V_TipoPago()
        {
            List<Ca_TipoPagosModel> Lst = new List<Ca_TipoPagosModel>();
            tipopagos.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoPagosModel>(x, new Ca_TipoPagosModel())); });
            return View(Lst);
        }
        public ActionResult V_TipoPago_Detalles(byte? Id)
        {
            if (Id != null && Id != 0)
                return View(ModelFactory.getModel<Ca_TipoPagosModel>(tipopagos.GetByID(x => x.Id_TipoPago == Id), new Ca_TipoPagosModel()));
            return View(new Ca_TipoPagosModel());
        }

        public ActionResult V_TipoPago_Agregar()
        {
            return View(new Ca_TipoPagosModel());
        }

        [HttpPost]
        public ActionResult V_TipoPago_Agregar(Ca_TipoPagos modelo)
        {
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            Ca_TipoPagosModel model = new Ca_TipoPagosModel();
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    modelo.Id_TipoPago = 0;
                    //if (model.ValidaDesc(modelo.Descripcion, ref msn) == true)
                    //{
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                    modelo.Fecha_act = DateTime.Now;
                    tipopagos.Insert(modelo);
                    tipopagos.Save();
                    return Json(new { Exito = true, Mensaje = "Registro guardado exitosamente", Registro = modelo });
                    //}
                    //else
                    //    return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información no válida" });
            }
        }
        public ActionResult V_TipoPago_Editar(byte Id)
        {
            return View(ModelFactory.getModel<Ca_TipoPagosModel>(tipopagos.GetByID(x => x.Id_TipoPago == Id), new Ca_TipoPagosModel()));
        }
        [HttpPost]
        public ActionResult V_TipoPago_Editar(Ca_TipoPagos modelo)
        {
            Ca_TipoPagosModel model = new Ca_TipoPagosModel();
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    string msn = "";
                    modelo.Descripcion = modelo.Descripcion.Trim();
                    //if (model.ValidaEdit(modelo.Id_TipoBene, modelo.Descripcion, ref msn) == true)
                    //{
                    modelo.Fecha_act = DateTime.Now;
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;

                    tipopagos.Update(modelo);
                    tipopagos.Save();
                    return Json(new { Exito = true, Mensaje = "Registro modificado exitosamente", Registro = modelo });
                    //}
                    //else
                    //    return Json(new { Exito = false, Mensaje = msn });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información no válida" });
            }
        }

        public ActionResult V_TipoPago_Eliminar(byte Id)
        {
            try
            {
                Ca_TipoPagosModel model = new Ca_TipoPagosModel();
                string msn = "";
                Ca_TipoPagosModel modelo = ModelFactory.getModel<Ca_TipoPagosModel>(tipopagos.GetByID(x => x.Id_TipoPago == Id), new Ca_TipoPagosModel());

                tipopagos.Delete(m => m.Id_TipoPago == Id);
                tipopagos.Save();
                return Json(new { Exito = true, Mensaje = "Registro eliminado exitosamente", Registro = modelo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = (ex.HResult == -2146233087 ? "El registro no puede ser eliminado porque ya se ha usado" : new Errores(ex.HResult, ex.Message).Mensaje) }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Ca_TipoPolizas

        #endregion

        #region Ca_TipoTransferenciasEg
        public ActionResult V_TipoTransferenciasEg()
        {
            List<Ca_TipoTransferenciasEgModel> Lst = new List<Ca_TipoTransferenciasEgModel>();
            tipotransferenciaseg.Get().ToList().ForEach(x => { Lst.Add(ModelFactory.getModel<Ca_TipoTransferenciasEgModel>(x, new Ca_TipoTransferenciasEgModel())); });
            return View(Lst);
        }
        public ActionResult V_TipoTransferenciasEgDetalles(Int16 Id)
        {
            return View(ModelFactory.getModel<Ca_TipoTransferenciasEgModel>(tipotransferenciaseg.GetByID(x => x.Id_Tipotransf == Id), new Ca_TipoTransferenciasEgModel()));
        }
        #endregion

        #region Ca_TipoTransferenciasIng

        #endregion

        #region Ca_UnidadM

        #endregion

        #region Ta_Firmas
        public ActionResult V_Firmas()
        {
            List<TA_FirmasModel> Lst = new List<TA_FirmasModel>();
            IEnumerable<TA_Firmas> entities = firmas.Get(x => x.IdTipo == 2).OrderBy(x => x.Id_Firma);
            foreach (TA_Firmas item in entities)
            {
                TA_FirmasModel model = ModelFactory.getModel<TA_FirmasModel>(item, new TA_FirmasModel());
                if (model.Id_Firma == 1)
                    model.UsoPoliza = "Autoriza el Egreso";
                if (model.Id_Firma == 2)
                    model.UsoPoliza = "Autoriza el Pago";
                Lst.Add(model);
            }
            return View(Lst);
        }
        public ActionResult V_Firmas_Editar(Int16 IdFirma)
        {
            TA_FirmasModel model = ModelFactory.getModel<TA_FirmasModel>(firmas.GetByID(x => x.Id_Firma == IdFirma), new TA_FirmasModel());
            return View(model);
        }
        [HttpPost]
        public ActionResult V_Firmas_Editar(TA_Firmas modelo)
        {

            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    modelo.Fecha_Act = DateTime.Now;
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                    firmas.Update(modelo);
                    firmas.Save();
                    TA_FirmasModel model = ModelFactory.getModel<TA_FirmasModel>(modelo, new TA_FirmasModel());
                    if (model.IdTipo == 2)
                    {
                        if (model.Id_Firma == 1)
                            model.UsoPoliza = "Autoriza el Egreso";
                        if (model.Id_Firma == 2)
                            model.UsoPoliza = "Autoriza el Pago";
                    }
                    return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Registro = model });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }
        public ActionResult V_FirmasFinanciero()
        {
            List<TA_FirmasModel> Lst = new List<TA_FirmasModel>();
            IEnumerable<TA_Firmas> entities = firmas.Get(x => x.IdTipo == 1);
            foreach (TA_Firmas item in entities)
            {
                TA_FirmasModel model = ModelFactory.getModel<TA_FirmasModel>(item, new TA_FirmasModel());
                Lst.Add(model);
            }
            return View(Lst);
        }
        public ActionResult V_FirmasFinanciero_Editar(Int16 IdFirma)
        {
            TA_FirmasModel model = ModelFactory.getModel<TA_FirmasModel>(firmas.GetByID(x => x.Id_Firma == IdFirma), new TA_FirmasModel());
            return View(model);
        }
        [HttpPost]
        public ActionResult V_FirmasFinanciero_Editar(TA_Firmas modelo)
        {

            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            if (ModelState.IsValid)
            {
                try
                {
                    modelo.Fecha_Act = DateTime.Now;
                    modelo.Usu_Act = (Int16)appUsuario.IdUsuario;
                    firmas.Update(modelo);
                    firmas.Save();
                    TA_FirmasModel model = ModelFactory.getModel<TA_FirmasModel>(modelo, new TA_FirmasModel());
                    return Json(new { Exito = true, Mensaje = "Registro Modificado Exitosamente", Registro = model });
                }
                catch (Exception ex)
                {
                    var code = ex.HResult;
                    return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Exito = false, Mensaje = "Información No Válida" });
            }
        }

        #endregion

    }
}

