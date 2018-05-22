using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TesoreriaVS12.BL;
using TesoreriaVS12.DAL;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Filters
{
    public class AuthorizeLogin : System.Web.Mvc.AuthorizeAttribute
    {
        //protected OpcionesDAL opciones { get; set; }
        //protected UsuariosDAL usuarios { get; set; }
        //protected PermisosDAL permisos { get; set; }
        //protected PerfilesDAL perfiles { get; set; }
        protected PermisosBL blPermisos { get; set; }
        public AuthorizeLogin()
        {
            if (blPermisos == null) blPermisos = new PermisosBL();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            HttpContextBase request = filterContext.RequestContext.HttpContext;
            ControllerContext ctx = filterContext.Controller.ControllerContext;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (session["appUsuario"] == null)
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    filterContext.HttpContext.Response.End();
                    if (filterContext.HttpContext.Request.HttpMethod == "POST")
                        filterContext.Result = new JsonResult { Data = "Fin Sesión" };
                    else
                        filterContext.Result = new JsonResult { Data = "Fin Sesión", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else 
                { 
                    UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                    var Area = filterContext.RouteData.DataTokens["area"];
                    if (Area == null)
                        Area = "";
                    if (!blPermisos.hasPermiso(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName, Area.ToString(), appUsuario.IdRol, filterContext.HttpContext.Request.HttpMethod))
                    {
                        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        filterContext.HttpContext.Response.End();
                        if (filterContext.HttpContext.Request.HttpMethod == "POST")
                            filterContext.Result = new JsonResult { Data = "No tiene acceso a esta opción." };
                        else
                            filterContext.Result = new JsonResult { Data = "No tiene acceso a esta opción.", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                }
            }
            else
                if (session["appUsuario"] == null)
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "LogOn", area = "", returnUrl = String.Format("{0}{1}", request.Request.Url.AbsolutePath, request.Request.Url.Query) }));
                else
                {
                    UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                    #if !DEBUG
                        if (String.IsNullOrEmpty(appUsuario.Conexion) && filterContext.ActionDescriptor.ActionName != "SeleccionarBase")
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "SeleccionarBase", area = "", returnUrl = String.Format("{0}{1}", request.Request.Url.AbsolutePath, request.Request.Url.Query) }));
                    #endif
                    //if (appUsuario.ChangePass.Value)
                    //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Catalogos", action = "CambiarContrasenia", area = "", }));
                    var Area = filterContext.RouteData.DataTokens["area"];
                    if (Area == null)
                        Area = "";
                    if (!blPermisos.hasPermiso(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName, Area.ToString(), appUsuario.IdRol, filterContext.HttpContext.Request.HttpMethod))
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "NoAutorizado", area = "", returnUrl = String.Format("{0}{1}", request.Request.Url.AbsolutePath, request.Request.Url.Query) }));
                    
                }
        }

        //public bool TienePermiso(String Accion, String Controlador, byte Perfil, String Area)
        //{
        //    if (!ExistePermiso(Accion, Controlador, Area))
        //    {
        //        RegistrarPermisos(Accion, Controlador, Perfil, Area);
        //        return true;
        //    }
        //    else
        //    {
        //        CA_OpcionesModel padre = ModelFactory.getModel<CA_OpcionesModel>(opciones.GetByID(x => x.Controlador == Controlador && String.IsNullOrEmpty(x.Accion)),new CA_OpcionesModel());
        //        IQueryable<DE_Permisos> permisoslst = permisos.Get(x => x.IdOpcion == padre.IdOpcion).AsQueryable();
        //        if (permisoslst.SingleOrDefault(x => x.IdPerfil == Perfil && x.Activo == true) != null)
        //            return true;
        //        CA_Opciones hijo = opciones.GetByID(x => x.Controlador == Controlador && x.Accion == Accion);
        //        IQueryable<DE_Permisos> permisosh = permisos.Get(x => x.IdOpcion == hijo.IdOpcion).AsQueryable();
        //        if (permisosh.SingleOrDefault(x => x.IdPerfil == Perfil && x.Activo == true) != null)
        //            return true;
        //        return false;
        //    }
        //}

        //public bool ExistePermiso(String Accion, String Controlador, String Area)
        //{
        //    CA_Opciones opcion = opciones.GetByID(x => x.Controlador == Controlador && x.Accion == Accion && x.Sistema == Area);
        //    return (!(opcion == null));
        //}

        //public void RegistrarPermisos(String Accion, String Controlador, Byte Perfil, String Area)
        //{
        //    CA_Opciones padre = opciones.GetByID(x => x.Controlador == Controlador && x.Accion == null && x.Sistema == Area);
        //    if (padre == null)
        //    {
        //        padre = new CA_Opciones();
        //        padre.Accion = null;
        //        padre.Controlador = Controlador;
        //        padre.fAct = DateTime.Now;
        //        padre.IdOpcionP = null;
        //        padre.usuAct = 1;
        //        padre.Descripcion = Controlador;
        //        padre.Sistema = Area;
        //        padre.SubMenuWidth = 200;
        //        opciones.Insert(padre);
        //        //db.CA_Opciones.Add(padre);
        //        //db.SaveChanges();
        //        opciones.Save();
        //        DE_Permisos permiso = new DE_Permisos();
        //        permiso.Activo = true;
        //        permiso.IdOpcion = padre.IdOpcion;
        //        permiso.IdPerfil = Perfil;
        //        permisos.Insert(permiso);
        //        permisos.Save();
        //        //db.DE_Permisos.Add(permiso);
        //        //db.SaveChanges();
        //        CA_Opciones hijos = new CA_Opciones();
        //        hijos.Accion = Accion;
        //        hijos.Sistema = Area;
        //        hijos.IdOpcionP = padre.IdOpcion;
        //        hijos.Controlador = Controlador;
        //        hijos.fAct = DateTime.Now;
        //        hijos.Descripcion = Accion;
        //        hijos.SubMenuWidth = 200;
        //        hijos.usuAct = 1;
        //        opciones.Insert(hijos);
        //        opciones.Save();
        //        //db.CA_Opciones.Add(hijos);
        //        //db.SaveChanges();
        //        permiso = new DE_Permisos();
        //        permiso.IdPerfil = Perfil;
        //        permiso.IdOpcion = hijos.IdOpcion;
        //        permiso.Activo = true;
        //        permisos.Insert(permiso);
        //        permisos.Save();
        //        //db.DE_Permisos.Add(permiso);
        //        //db.SaveChanges();
        //    }
        //    CA_Opciones hijo = opciones.GetByID(x => x.Controlador == Controlador && x.Accion == Accion && x.Sistema == Area);
        //    if (hijo == null)
        //    {
        //        hijo = new CA_Opciones();
        //        hijo.Accion = Accion;
        //        hijo.Controlador = Controlador;
        //        hijo.fAct = DateTime.Now;
        //        hijo.IdOpcionP = padre.IdOpcion;
        //        hijo.usuAct = 1;
        //        hijo.Sistema = Area;
        //        hijo.SubMenuWidth = 200;
        //        opciones.Insert(hijo);
        //        opciones.Save();
        //        //db.CA_Opciones.Add(hijo);
        //        //db.SaveChanges();
        //        DE_Permisos permiso = new DE_Permisos();
        //        permiso.IdPerfil = Perfil;
        //        permiso.IdOpcion = hijo.IdOpcion;
        //        permiso.Activo = true;
        //        permisos.Insert(permiso);
        //        permisos.Save();
        //        //db.DE_Permisos.Add(permiso);
        //        //db.SaveChanges();
        //    }
        //}
    }
}