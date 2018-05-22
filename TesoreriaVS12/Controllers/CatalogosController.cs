using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TesoreriaVS12.BL;
using TesoreriaVS12.DAL;
using TesoreriaVS12.Filters;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Controllers
{
    [AuthorizeLogin]
    public class CatalogosController : Controller
    {
        protected UsuariosDAL repoUsuarios { get; set; }
        protected PerfilesDAL repoPerfiles { get; set; }
        protected PermisosDAL repoPermisos { get; set; }
        protected GruposDAL repoOpciones { get; set; }
        private PermisosBL blPermisos { get; set; }
        public CatalogosController()
        {
            if (repoUsuarios == null) repoUsuarios = new UsuariosDAL();
            if (repoPerfiles == null) repoPerfiles = new PerfilesDAL();
            if (repoPermisos == null) repoPermisos = new PermisosDAL();
            if (repoOpciones == null) repoOpciones = new GruposDAL();
            if (blPermisos == null) blPermisos = new PermisosBL();
        }
        #region Usuarios

        //[PermisosFilter(Permiso="Polizas,GastosComprobar", Proceso="Guardar")]
        public ActionResult Usuarios()
        {
            List<CA_UsuariosModel> usrLst = new List<CA_UsuariosModel>();
            foreach(CA_Usuarios item in repoUsuarios.Get())
            {
                CA_UsuariosModel temp = ModelFactory.getModel<CA_UsuariosModel>(item, new CA_UsuariosModel());
                if(item.CA_Perfiles != null)
                    temp.CA_Perfiles = ModelFactory.getModel<CA_PerfilesModel>(item.CA_Perfiles, new CA_PerfilesModel());
                usrLst.Add(temp);

            }
            return View(usrLst);
        }
        //[PermisosFilter(Permiso = "Polizas", Proceso = "Guardar")]
        public ActionResult AgregarUsuario(Int32? Id)
        {
            if (Id.HasValue)
                return View(ModelFactory.getModel<CA_UsuariosModel>(repoUsuarios.GetByID(x=> x.IdUsuario == Id.Value), new CA_UsuariosModel()));
            return View(new CA_UsuariosModel());
        }

        [HttpPost]
        //[PermisosFilter(Permiso = "Polizas", Proceso = "[Guardar,detalles,listar]")]
        public ActionResult AgregarUsuario(CA_Usuarios dataModel)
        {
            try
            {
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                CA_UsuariosModel modelUsuario = new CA_UsuariosModel();
                bool Nuevo = false;
                if (dataModel.IdUsuario != 0)
                {
                    dataModel.usuAct = appUsuario.IdUsuario;
                    dataModel.fAct = DateTime.Now;
                    repoUsuarios.Update(dataModel);
                    repoUsuarios.Save();
                }
                else
                {
                    string password = new PermisosBL().GenerarContrasenia(8);
                    Logueo log = new Logueo();
                    dataModel.usuAct = appUsuario.IdUsuario;
                    dataModel.FechaRegistro = DateTime.Now;
                    dataModel.fAct = DateTime.Now;
                    dataModel.Activo = true;
                    dataModel.Contrasenia = log.ObtenerSha256(password);
                    dataModel.CambiaContrasenia = true;
                    dataModel.GeneradoAutomatico = false;
                    dataModel.Intentos = 0;
                    repoUsuarios.Insert(dataModel);
                    repoUsuarios.Save();
                    EnviaCorreo.EnviodeCorreos enviaCorreo = new EnviaCorreo.EnviodeCorreos();
                    string res = enviaCorreo.Envia_Mail("NSICG2015", "Registro de nuevo usuario", String.Format("{0} {1} {2} se te ha asignado el siguiente acceso.\n Usuario: {3} <br/> Contraseña: {4} <br/>", 
                        dataModel.Nombre, dataModel.ApellidoPaterno,dataModel.ApellidoMaterno,dataModel.Usuario
                        , password), dataModel.email);
                    dataModel.CA_Perfiles = repoPerfiles.GetByID(x => x.IdPerfil == dataModel.IdPerfil);
                    modelUsuario = ModelFactory.getModel<CA_UsuariosModel>(dataModel,new CA_UsuariosModel());
                    modelUsuario.CA_Perfiles = ModelFactory.getModel<CA_PerfilesModel>(dataModel.CA_Perfiles,new CA_PerfilesModel());
                }
                return Json(new { Exito = true, Mensaje = "Ok", Nuevo = Nuevo, Registro = modelUsuario });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }
        [HttpPost]
        public ActionResult RestablecerPassword(int idUsuario){
            try
            {
                CA_Usuarios usuario = repoUsuarios.GetByID(x=>x.IdUsuario == idUsuario);
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                if (usuario != null)
                {
                    string password = new PermisosBL().GenerarContrasenia(8);
                    Logueo log = new Logueo();
                    usuario.Contrasenia = log.ObtenerSha256(password);
                    usuario.Intentos = 0;
                    usuario.Activo = true;
                    usuario.fAct = DateTime.Now;
                    usuario.CambiaContrasenia = false;
                    usuario.usuAct = appUsuario.IdUsuario;
                    repoUsuarios.Update(usuario);
                    repoUsuarios.Save();
                    EnviaCorreo.EnviodeCorreos enviaCorreo = new EnviaCorreo.EnviodeCorreos();
                    string res = enviaCorreo.Envia_Mail("NSICG2015", "Restablecimiento de contraseña - Nuevo Sistema Integral de Contabilidad Gubernamental",
                        String.Format("Estimado <b>{0} {1} {2}</b>, se ha restablecido su contraseña con éxito <br/> Usuario: <b>{3}</b><br/> Nueva Contraseña: <b>{4}</b> <br/> Para ingresar al NSICG haz click <a href='http://www.icm-sicg.col.gob.mx/Account/LogOn'>aquí</a>", 
                        usuario.Nombre, usuario.ApellidoPaterno, usuario.ApellidoMaterno,usuario.Usuario,password), usuario.email);
                    return Json(new { Exito = true, Mensaje = "La contraseña se ha restablecido correctamente" });
                }
                return Json(new { Exito = true, Mensaje = "Usuario inválido" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }
        
        [HttpGet]
        public ActionResult EditarUsuario(Int32 id)
        {
            CA_Usuarios usuario = repoUsuarios.GetByID(x=>x.IdUsuario == id);
            if (usuario != null)
                return View(ModelFactory.getModel<CA_UsuariosModel>(usuario, new CA_UsuariosModel(usuario.IdPerfil)));
            return View(ModelFactory.getModel<CA_UsuariosModel>(usuario, new CA_UsuariosModel()));
        }

        [HttpPost]
        public ActionResult EditarUsuario(CA_Usuarios dataModel)
        {
            try
            {
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                CA_UsuariosModel modelUsuario = new CA_UsuariosModel();
                if (dataModel.IdUsuario != 0)
                {
                    dataModel.usuAct = appUsuario.IdUsuario;
                    dataModel.fAct = DateTime.Now;
                    dataModel.Contrasenia = new PermisosBL().getUserHash(dataModel.IdUsuario);
                    repoUsuarios.Update(dataModel);
                    repoUsuarios.Save();
                    dataModel.CA_Perfiles = repoPerfiles.GetByID(x => x.IdPerfil == dataModel.IdPerfil);
                    modelUsuario = ModelFactory.getModel<CA_UsuariosModel>(dataModel, new CA_UsuariosModel());
                    modelUsuario.CA_Perfiles = ModelFactory.getModel<CA_PerfilesModel>(dataModel.CA_Perfiles, new CA_PerfilesModel());
                }
                return Json(new { Exito = true, Mensaje = "Usuario actualizado correctamente", Registro = modelUsuario });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }

        public ActionResult EliminarUsuario(Int32 Id)
        {
            try
            {
                CA_Usuarios usr = repoUsuarios.GetByID(x => x.IdUsuario == Id);
                usr.Activo = false;
                repoUsuarios.Update(usr);
                repoUsuarios.Save();
                return Json(new { Exito = true, Mensaje = "Ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DetallesUsuario(Int32 Id)
        {
            return View(ModelFactory.getModel<CA_UsuariosModel>(repoUsuarios.GetByID(x => x.IdUsuario == Id), new CA_UsuariosModel()));
        }

        public ActionResult CambiarContrasenia()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Bloquear(int Id, bool bloqueo)
        {
            try
            {
                CA_Usuarios usr = repoUsuarios.GetByID(x => x.IdUsuario == Id);
                if (bloqueo)
                    usr.Activo = false;
                else
                {
                    usr.Activo = true;
                    usr.Intentos = 0;
                }
                repoUsuarios.Update(usr);
                repoUsuarios.Save();
                return Json(new { Exito = true, Mensaje = "Proceso Exitoso" });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
            
        }

        #endregion

        #region Perfiles
        public ActionResult Perfiles()
        {
            List<CA_PerfilesModel> usrLst = new List<CA_PerfilesModel>();
            repoPerfiles.Get().ToList().ForEach(x => { usrLst.Add(ModelFactory.getModel<CA_PerfilesModel>(x, new CA_PerfilesModel())); });
            return View(usrLst);
        }

        public ActionResult AgregarPerfil(Int32? Id)
        {
            if (Id.HasValue)
                return View(ModelFactory.getModel<CA_PerfilesModel>(repoPerfiles.GetByID(x=> x.IdPerfil == Id.Value), new CA_PerfilesModel()));
            return View(new CA_PerfilesModel());
        }

        public ActionResult EditarPerfil(Int32? Id)
        {
            if (Id.HasValue)
                return View(ModelFactory.getModel<CA_PerfilesModel>(repoPerfiles.GetByID(x => x.IdPerfil == Id.Value), new CA_PerfilesModel()));
            return View(new CA_PerfilesModel());
        }

        [HttpPost]
        public ActionResult EditarPerfil(CA_Perfiles dataModel)
        {
            try
            {
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                dataModel.usuAct = appUsuario.IdUsuario;
                dataModel.fAct = DateTime.Now;
                repoPerfiles.Update(dataModel);
                repoPerfiles.Save();
                return Json(new { Exito = true, Mensaje = "Perfil actualizado con éxito", Registro = dataModel });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }

        [HttpPost]
        public ActionResult AgregarPerfil(CA_Perfiles dataModel)
        {
            try
            {
                UsuarioLogueado appUsuario = Logueo.GetUsrLogueado();
                dataModel.usuAct = appUsuario.IdUsuario;
                dataModel.Descripcion = dataModel.Descripcion.Trim();
                dataModel.fAct = DateTime.Now;
                dataModel.fAct = DateTime.Now;
                repoPerfiles.Insert(dataModel);
                repoPerfiles.Save();
                return Json(new { Exito = true, Mensaje = "Ok", Registro = dataModel });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }

        public ActionResult EliminarPerfil(Int32 Id)
        {
            try
            {
                repoPerfiles.Delete(x => x.IdPerfil == Id);
                repoPerfiles.Save();
                return Json(new { Exito = true, Mensaje = "Ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DetallesPerfil(Int32 IdPerfil)
        {
            return View(ModelFactory.getModel<CA_PerfilesModel>(repoPerfiles.GetByID(x => x.IdPerfil == IdPerfil), new CA_PerfilesModel()));
        }
        #endregion

        #region Permisos

        [HttpPost]
        public ActionResult Permisos(Byte? IdPerfil)
        {
            Permisos per = new Permisos();
            per.IdPerfil = IdPerfil;
            return View(per);
        }

        [HttpPost]
        public ActionResult PermisosJson(byte? IdPerfil)
        {
            try
            {
                List<PermisosModel> permisos = blPermisos.fillChildren(null, IdPerfil.Value);
                return Json(new { Exito = true, Mensaje = "OK", Permisos = JsonConvert.SerializeObject(permisos) });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }
        }


        [HttpPost]
        public JsonResult GuardarPermisos(string frm, byte IdPerfil)
        {
            try
            {
                List<PermisosModel> Lista = new JavaScriptSerializer().Deserialize<List<PermisosModel>>(frm);
                if(blPermisos.ActualizarPermisos(Lista,IdPerfil))
                    return Json(new { Exito = true, Mensaje = "OK" });
                else
                    return Json(new { Exito = false, Mensaje = "No se pudo actualizar" });
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                return Json(new { Exito = false, Mensaje = new Errores(code, ex.Message).Mensaje });
            }
        }

        #endregion

    }
}
