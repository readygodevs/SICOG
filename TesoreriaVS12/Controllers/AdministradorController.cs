using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TesoreriaVS12.DAL;
using TesoreriaVS12.Filters;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Controllers
{
    [AuthorizeLogin]
    public class AdministradorController : Controller
    {
        //
        // GET: /Administrador/
        private OpcionesDAL dalOpciones;
        private Menu _menu;

        protected Menu menu
        {
            get { return _menu; }
            set { _menu = value; }
        }

        protected OpcionesDAL DalOpciones
        {
            get { return dalOpciones; }
            set { dalOpciones = value; }
        }

        public AdministradorController()
        {
            if (DalOpciones == null) DalOpciones = new OpcionesDAL();
            if (menu == null) menu = new Menu();
        }


        public ActionResult Menu()
        {
            return View();
        }

        [HttpPost]
        public JsonResult jsonMenu(Int32? Padre)
        {
            try
            {
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                if(!Padre.HasValue)
                    return Json( new Menu().fillPadres(appUsuario.IdRol) );
//                return Json(new Menu().fillHijos(appUsuario.IdRol,x=> x.IdOpcionP == Padre.Value && x.Mostrar == true));
                return Json(new Menu().fillHijos(appUsuario.IdRol, x => x.IdOpcionP == Padre.Value));
            }
            catch (Exception ex)
            {
                new Errores(ex.HResult, ex.Message);
                return Json( new { } );
            }
        }

        public ActionResult EditarMenu(Int32 Menu)
        {
            CA_OpcionesModel dataModel = ModelFactory.getModel<CA_OpcionesModel>(DalOpciones.GetByID(x => x.IdOpcion == Menu), new CA_OpcionesModel());
            if (DalOpciones.Get(x => x.MenuPadre == dataModel.IdOpcion).Count() > 0)
                dataModel.hasPadre = true;
            else
                dataModel.hasPadre = false;
            return View(dataModel);
        }

        [HttpPost]
        public ActionResult EditarMenu(CA_Opciones dataModel)
        {
            try
            {
                
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                dataModel.usuAct = appUsuario.IdUsuario;
                dataModel.fAct = DateTime.Now;
                if (String.IsNullOrEmpty(dataModel.Sistema))
                    dataModel.Sistema = String.Empty;
                if (String.IsNullOrEmpty(dataModel.Controlador))
                    dataModel.Controlador = String.Empty;
                if (String.IsNullOrEmpty(dataModel.Accion))
                    dataModel.Accion = String.Empty;
                if (String.IsNullOrEmpty(dataModel.Descripcion))
                    dataModel.Descripcion = String.Empty;
                DalOpciones.Update(dataModel);
                DalOpciones.Save();
                return Json(new { Exito = true, Mensaje = "Ok", Registro = dataModel });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
                    
        }

        [HttpPost]
        public JsonResult checkMenu(Int32 Id, bool hasMenu)
        {
            try
            {
                CA_Opciones op = dalOpciones.GetByID(x => x.IdOpcion == Id);
                if (op != null)
                {
                    UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                    op.Menu = hasMenu;
                    op.fAct = DateTime.Now;
                    op.usuAct = appUsuario.IdUsuario;
                    dalOpciones.Update(op);
                    dalOpciones.Save();
                    Session["Menu"] = JsonConvert.SerializeObject(menu.fillMenu(appUsuario.IdRol));
                    return Json(new { Exito = true, Mensaje = "Ok" });
                }
                else
                    return Json(new { Exito = false, Mensaje = "El elemento no existe" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public JsonResult setParetn(Int32 IdMenu, Int16 IdParent)
        {
            try
            {
                CA_Opciones op = dalOpciones.GetByID(x => x.IdOpcion == IdMenu);
                if (op != null)
                {
                    UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                    op.MenuPadre = IdParent;
                    op.fAct = DateTime.Now;
                    op.usuAct = appUsuario.IdUsuario;
                    dalOpciones.Update(op);
                    dalOpciones.Save();
                    Session["Menu"] = JsonConvert.SerializeObject(menu.fillMenu(appUsuario.IdRol));
                    return Json(new { Exito = true, Mensaje = "Ok" });
                }
                else
                    return Json(new { Exito = false, Mensaje = "El elemento no existe" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        [HttpPost]
        public JsonResult MenuSession()
        {
            try
            {
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                if(appUsuario != null)
                {
                    var json = JsonConvert.SerializeObject(menu.fillMenu(appUsuario.IdRol));
                    Session["Menu"] = json;
                    return Json(new { Exito = true, Mensaje = "Ok", Menu = json });
                }
                else
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    return Json(new { Exito = false, Mensaje = "Se ha terminado tu sesión"});
                }
                    
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

    }
}
