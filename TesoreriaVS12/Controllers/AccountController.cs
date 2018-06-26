//prueba editado
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using TesoreriaVS12.Filters;
using TesoreriaVS12.Models;
using TesoreriaVS12.DAL;
using System.Web.Script.Serialization;
using System.Web.Helpers;
using Newtonsoft.Json;
using TesoreriaVS12.BL;
using TesoreriaVS12.Utils;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;

namespace TesoreriaVS12.Controllers
{
    public class AccountController : Controller
    {

        //CtrlGral Branch
        protected GruposDAL opciones { get; set; }
        protected UsuariosDAL usuarios { get; set; }
        protected PermisosDAL permisos { get; set; }
        protected PerfilesDAL perfiles { get; set; }
        protected Logueo Logueo { get; set; }
        protected BdTesoreriasDAL bds { get; set; }
        public AccountController()
        {
            if (opciones == null) opciones = new GruposDAL();
            if (usuarios == null) usuarios = new UsuariosDAL();
            if (permisos == null) permisos = new PermisosDAL();
            if (perfiles == null) perfiles = new PerfilesDAL();
            if (Logueo == null) Logueo = new Logueo();
        }

        [AllowAnonymous]
        public ActionResult LogOn(string returnUrl)
        {
            //string strHostName = System.Net.Dns.GetHostName();
            //System.Net.IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            //System.Net.IPAddress[] addr = ipEntry.AddressList;
            //string ip = addr[4].ToString();

            Session.RemoveAll();
            ViewBag.ReturnUrl = returnUrl;

            //string[] valores = "010|00|00|00|00|00|00|00|00|00".Split('|');

            //Ca_CURDAL CUR = new Ca_CURDAL();
            //CUR.GetByID2(valores[0],
            //    valores[1],
            //    valores[2],
            //    valores[3],
            //    valores[4],
            //    valores[5],
            //    valores[6],
            //    valores[7],
            //    valores[8],
            //    valores[9]);

            return View();
        }


        public ActionResult Cambiar_Color()
        {
            return View();
        }

        [HttpPost]
        public ActionResult cambiar_tema(string color)
        {
            try
            {
                itemCss = "#header {";
                //Color header
                reemplazar_linea(Server.MapPath(@"~\Content\layout.css"), "background-color:", color);

                itemCss = "#menu-lateral li a {";
                //Color botonera izq
                reemplazar_linea(Server.MapPath(@"~\Content\layout.css"), "color:", color);
                itemCss = "#menu-lateral li a:hover {";
                reemplazar_linea(Server.MapPath(@"~\Content\layout.css"), "color:", color);

                itemCss = ".iconos_wraper .acciones a {";
                //Color botones grid
                reemplazar_linea(Server.MapPath(@"~\Content\layout.css"), "color:", color);

                return Json(new { Error = false, Mensaje = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Error = true, Mensaje = ex.Message });
            }
        }
        string itemCss = "";
        public void reemplazar_linea(string path, string propiedad, string valor)
        {
            string[] lineas = System.IO.File.ReadAllLines(path);


            var indexHeader = lineas.Select((text, index) => new { text, lineNumber = index })
                      .Where(x => x.text.Trim().Equals(itemCss)).FirstOrDefault();

            for (int i = indexHeader.lineNumber; i < lineas.Length; i++)
            {
                if (lineas[i].Trim().StartsWith(propiedad))
                {
                    lineas[i] = propiedad + valor + ";";//#119548
                    i = lineas.Length;
                }
            }

            System.IO.File.WriteAllLines(path, lineas);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOn(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ErrorCustom error = Logueo.ValidarUsuario(model);
                if (error.Error)
                {
                    ModelState.AddModelError("", error.Mensaje);
                    Logueo.SetIntentos(model.UserName, null);
                    return View(model);
                }
                else
                {
                    if (error.Usuario.Intentos < 3)
                    {
                        Logueo.SetIntentos(model.UserName, 0);
                        HttpContext.Session["appUsuario"] = error.Usuario;
                        UsuarioLogueado appUsuario = error.Usuario;
                        FormsAuthentication.SetAuthCookie(appUsuario.NombreCompleto, model.RememberMe);
                        if (String.IsNullOrEmpty(returnUrl) || returnUrl == "/")
                        {
                            Menu _menu = new Menu();
                            var json = JsonConvert.SerializeObject(_menu.fillMenu(appUsuario.IdRol));
                            Session["Menu"] = json;
                            return RedirectToAction("SeleccionarBase", "Account");
                            //return Redirect(appUsuario.DefaultPage);
                        }
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", MensajesError.BloquearUsuario);
                        Logueo.BloquearUsuario(model.UserName);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CambiarContraseniaAjax(LocalPasswordModel usuarioModel)
        {
            try
            {
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                CA_Usuarios usuario = usuarios.GetByID(x => x.IdUsuario == appUsuario.IdUsuario);
                if (usuario != null)
                {
                    Logueo log = new Logueo();
                    if (log.ObtenerSha256(usuarioModel.OldPassword) == usuario.Contrasenia)
                    {
                        usuario.Contrasenia = log.ObtenerSha256(usuarioModel.ConfirmPassword);
                        usuario.usuAct = appUsuario.IdUsuario;
                        usuario.fAct = DateTime.Now;
                        usuarios.Update(usuario);
                        usuarios.Save();
                        return Json(new { Exito = true });
                    }
                    return Json(new { Exito = false, Mensaje = "La contraseña actual no coincide, favor de revisar" });

                }
                return Json(new { Exito = false, Mensaje = "Usuario no válido" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        //
        // POST: /Account/LogOff

        public ActionResult LogOff()
        {
            UsuarioLogueado usr = HttpContext.Session["appUsuario"] as UsuarioLogueado;

            FormsAuthentication.SignOut();
            Session.RemoveAll();
            return RedirectToAction("LogOn", "Account", new { area = "" });
        }

        [AuthorizeLogin]
        public ActionResult SeleccionarBase()
        {
            if (bds == null) bds = new BdTesoreriasDAL();
            return View(bds.Get().OrderBy(a => a.Nombre).ToList());

        }

        UrlHelper contextoWeb =
            new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);

        [AuthorizeLogin]
        [HttpPost]
        public JsonResult SetConection(String ConexionBase)
        {
            try
            {
                UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
                appUsuario.Conexion = ConexionBase;
                Session["appUsuario"] = appUsuario;
                String[] nombreBase = ConexionBase.Split('_');
                Session["Ejercicio"] = nombreBase[nombreBase.Length - 1];

                if (appUsuario.DefaultPage == null) appUsuario.DefaultPage = contextoWeb.Action("Index", "Home");
                return Json(new { Exito = true, DefaultUrl = appUsuario.DefaultPage });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, DefaultUrl = "", Mensaje = new Errores(code, ex.Message).Mensaje });
            }

        }

        #region Aplicaciones auxiliares
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Vaya a http://go.microsoft.com/fwlink/?LinkID=177550 para
            // obtener una lista completa de códigos de estado.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "El nombre de usuario ya existe. Escriba un nombre de usuario diferente.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Ya existe un nombre de usuario para esa dirección de correo electrónico. Escriba una dirección de correo electrónico diferente.";

                case MembershipCreateStatus.InvalidPassword:
                    return "La contraseña especificada no es válida. Escriba un valor de contraseña válido.";

                case MembershipCreateStatus.InvalidEmail:
                    return "La dirección de correo electrónico especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "La respuesta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "La pregunta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.InvalidUserName:
                    return "El nombre de usuario especificado no es válido. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.ProviderError:
                    return "El proveedor de autenticación devolvió un error. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";

                case MembershipCreateStatus.UserRejected:
                    return "La solicitud de creación de usuario se ha cancelado. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";

                default:
                    return "Error desconocido. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";
            }
        }
        #endregion
    }
}
