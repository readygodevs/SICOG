using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.Models;

namespace TesoreriaVS12.Models
{
    public class Logueo
    {
        private ControlGeneralContainer db { get; set; }
        public static bool Session_End = false;

        public const String Administrador = "Administrador";
        public const String Firmante = "Firmante";
        public const String appAdmin = "AppAdmin";
        public static String IP = WebConfigurationManager.AppSettings["Ip"];
        public static String User = WebConfigurationManager.AppSettings["User"];
        public static String Pass = WebConfigurationManager.AppSettings["Pass"];

        public Logueo()
        {
            if (db == null) db = new ControlGeneralContainer();
        }

        //public CA_Correos getCorreo(String Correo)
        //{
        //    return db.CA_Correos.SingleOrDefault(x => x.Descripcion.Equals(Correo));
        //}

        public CA_Usuarios getUsr(int usr)
        {
            CA_Usuarios usuario = db.CA_Usuarios.SingleOrDefault(x => x.IdUsuario == usr);
            return usuario;
        }
        public bool Existe(String usr)
        {
            CA_Usuarios usuario = db.CA_Usuarios.SingleOrDefault(x => x.Usuario == usr);
            return (!(usuario == null));
        }

        public byte Intentos(String usr)
        {
            CA_Usuarios usuario = db.CA_Usuarios.SingleOrDefault(x => x.Usuario == usr);
            if (usuario == null)
                return 0;
            return (usuario.Intentos);
        }

        public void SetIntentos(String usr, Byte? Intentos)
        {
            try
            {
                CA_Usuarios usuario = db.CA_Usuarios.SingleOrDefault(x => x.Usuario == usr);
                if (usuario != null)
                {
                    if (Intentos.HasValue)
                        usuario.Intentos = Intentos.Value;
                    else
                        usuario.Intentos++;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var code = ex.HResult;
                new Errores(code, ex.Message);
            }

        }

        public ErrorCustom ValidarUsuario(LoginModel login)
        {
            string psw = ObtenerSha256(login.Password);

            CA_Usuarios usr = db.CA_Usuarios.SingleOrDefault(x => x.Usuario == login.UserName);
            if (usr == null)
                return new ErrorCustom(true, MensajesError.Credencialesinvalidas);
            if (usr.Contrasenia != psw)
                return new ErrorCustom(true, MensajesError.Credencialesinvalidas);
            if (!usr.Activo.Value)
                return new ErrorCustom(true, MensajesError.UsuarioDesactivado);
            ErrorCustom error = new ErrorCustom(false, "");
            error.Usuario = this.ObtenerUsuaro(usr);
            return error;
        }

        System.Web.Mvc.UrlHelper contextoWeb = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);

        public UsuarioLogueado ObtenerUsuaro(CA_Usuarios ciudadano)
        {
            UsuarioLogueado usr = new UsuarioLogueado();
            usr.Username = ciudadano.Usuario.ToUpper();
            usr.NombreCompleto = String.Format("{0} {1} {2}", ciudadano.Nombre, ciudadano.ApellidoPaterno, ciudadano.ApellidoMaterno);
            usr.NombreCompleto = usr.NombreCompleto.ToUpper();
            usr.IdUsuario = ciudadano.IdUsuario;
            usr.IdRol = ciudadano.IdPerfil;
            usr.Ape1 = ciudadano.ApellidoPaterno;
            usr.Ape2 = ciudadano.ApellidoMaterno;
            usr.Nombre = ciudadano.Nombre;
            usr.Rol = ciudadano.CA_Perfiles.Descripcion;
            usr.Intentos = ciudadano.Intentos;
            usr.ChangePass = ciudadano.CambiaContrasenia;
            usr.DefaultPage = ciudadano.CA_Perfiles.DefaultPage;
            if (!string.IsNullOrEmpty(usr.DefaultPage))
            {
                if (usr.DefaultPage.StartsWith("/"))
                {
                    usr.DefaultPage = usr.DefaultPage.Substring(1, usr.DefaultPage.Length - 1);
                }
                string[] accionYControler = usr.DefaultPage.Split('/');
                usr.DefaultPage = contextoWeb.Action(accionYControler[1], accionYControler[0]);
            }
            if (usr.ChangePass == null)
                usr.ChangePass = false;
            return usr;
        }

        public UsuarioLogueado ObtenerUsuario(LoginModel login)
        {
            UsuarioLogueado usr = new UsuarioLogueado();
            string psw = ObtenerSha256(login.Password);
            CA_Usuarios ciudadano = db.CA_Usuarios.SingleOrDefault(x => x.Usuario == login.UserName && x.Contrasenia == psw);
            if (ciudadano != null)
            {
                usr.Username = ciudadano.Usuario.ToUpper();
                usr.NombreCompleto = String.Format("{0} {1} {2}", ciudadano.Nombre, ciudadano.ApellidoPaterno, ciudadano.ApellidoMaterno);
                usr.NombreCompleto = usr.NombreCompleto.ToUpper();
                usr.IdUsuario = ciudadano.IdUsuario;
                usr.IdRol = ciudadano.IdPerfil;
                usr.Ape1 = ciudadano.ApellidoPaterno;
                usr.Ape2 = ciudadano.ApellidoMaterno;
                usr.Nombre = ciudadano.Nombre;
                usr.Rol = ciudadano.CA_Perfiles.Descripcion;
                usr.ChangePass = ciudadano.CambiaContrasenia;
                usr.DefaultPage = ciudadano.CA_Perfiles.DefaultPage;
                if (usr.ChangePass == null)
                    usr.ChangePass = false;
            }
            return usr;
        }

        public static string getNombreUsr(short IdUsario)
        {
            ControlGeneralContainer data = new ControlGeneralContainer();
            CA_Usuarios usr = data.CA_Usuarios.SingleOrDefault(x => x.IdUsuario == IdUsario);
            return usr != null ? String.Format("{0} {1} {2}", usr.Nombre, usr.ApellidoPaterno, usr.ApellidoMaterno) : "";
        }

        public static void SetCookie(string name, string value)
        {
            HttpCookie myCookie = new HttpCookie(name);
            myCookie.Value = value;
            myCookie.Expires = DateTime.Now.AddMonths(1);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        public static HttpCookie GetCookie(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }

        public static UsuarioLogueado GetUsrLogueado()
        {
            return HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
        }

        public static String GetParametro(String _Name)
        {
            ParametrosDAL param = new ParametrosDAL();
            CA_Parametros parametro = param.GetByID(x => x.Nombre == _Name);
            if (parametro != null)
                return parametro.Valor;
            else
                return "";
        }

        public ErrorCustom CambiarContrasenia(String Usr, String PswOld, String PswNew)
        {
            try
            {
                String psw = ObtenerSha256(PswOld);
                String nueva = ObtenerSha256(PswNew);
                CA_Usuarios Usuario = db.CA_Usuarios.SingleOrDefault(x => x.Usuario == Usr && x.Contrasenia == psw);
                if (nueva == psw)
                    return new ErrorCustom(true, "La contraseña anterior y contraseña nueva no deben coincidir, favor de realizar el cambio");
                if (Usuario != null)
                {
                    Usuario.Contrasenia = nueva;
                    Usuario.CambiaContrasenia = false;
                    db.SaveChanges();
                    return new ErrorCustom(false, "");
                }
                else
                    return new ErrorCustom(true, "No coincide  la contraseña, ingrese correctamente los campos");
            }
            catch (Exception ex)
            {
                return new ErrorCustom(true, ex.Message);
            }

        }

        public void BloquearUsuario(String Usr)
        {
            CA_Usuarios usuario = db.CA_Usuarios.SingleOrDefault(x => x.Usuario == Usr);
            if (usuario != null)
                usuario.Activo = false;
            db.SaveChanges();
        }

        //public CA_Parametros getVersion()
        //{
        //    return db.CA_Parametros.FirstOrDefault(reg => reg.Nombre == "Version");
        //}

        //public List<CA_Parametros> getParametros()
        //{
        //    return db.CA_Parametros.ToList();
        //}

        public IQueryable<CA_Opciones> ListaOpciones()
        {
            return db.CA_Opciones;
        }

        public IQueryable<CA_Perfiles> ListaPerfiles()
        {
            return db.CA_Perfiles;
        }
        //public IQueryable<CA_Parametros> ListParametros()
        //{
        //    return db.CA_Parametros;
        //}
        public IQueryable<object> ListaAutorizados(byte Perfil)
        {
            return from P in db.DE_Permisos
                   where
                     P.IdPerfil == Perfil
                   select new
                   {
                       IdPermiso = P.IdPermiso,
                       IdOpcion = P.IdOpcion,
                       IdPerfil = P.IdPerfil,
                       Activo = P.Activo
                   };
        }
        public void OtorgarPermisos(byte Perfil, Int16 Opcion, bool Activo)
        {
            DE_Permisos permiso = db.DE_Permisos.SingleOrDefault(x => x.IdOpcion == Opcion && x.IdPerfil == Perfil);
            if (permiso != null)
                permiso.Activo = Activo;
            else
            {
                permiso = new DE_Permisos();
                permiso.Activo = Activo;
                permiso.IdOpcion = Opcion;
                permiso.IdPerfil = Perfil;
                db.DE_Permisos.Add(permiso);
            }
        }
        public void QuitarPermisos(byte Perfil)
        {
            IQueryable<DE_Permisos> permisos = db.DE_Permisos.Where(x => x.IdPerfil == Perfil);
            foreach (var item in permisos)
            {
                item.Activo = false;
            }
            db.SaveChanges();
        }

        public void GuardarCambios()
        {
            db.SaveChanges();
        }

        public string ObtenerSha256(string str)
        {
            // Instanciamos el Crypto  
            SHA256 sha = SHA256CryptoServiceProvider.Create();
            //MD5 md5 = MD5CryptoServiceProvider.Create();
            // Instaciamos la Decodificación
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            // generamos el Hahs del array de bytes codificados.
            stream = sha.ComputeHash(encoding.GetBytes(str));
            // anexamos 0 para cifrar la cadena
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            // retornamos el HASH del la cadena
            return sb.ToString();
        }

        public String GenerarContrasenia(Int32 PasswordLength)
        {
            string _allowedChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ0123456789";
            Byte[] randomBytes = new Byte[PasswordLength];
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < PasswordLength; i++)
            {
                Random randomObj = new Random();
                randomObj.NextBytes(randomBytes);
                chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
            }

            return new string(chars);
        }

        public static Dictionary<int, String> Meses = new Dictionary<int, string>()
        {
            {1, "ENE"},
            {2, "FEB"},
            {3, "MAR"},
            {4, "ABR"},
            {5, "MAY"},
            {6, "JUN"},
            {7, "JUL"},
            {8, "AGO"},
            {9, "SEP"},
            {10,"OCT"},
            {11,"NOV"},
            {12,"DIC"}
        };
    }
}