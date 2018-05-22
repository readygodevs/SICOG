using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.DAL;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Models
{
    public class MensajesError
    {
        public const String Credencialesinvalidas = "El Usuario o la contraseña son inválidos";
        public const String UsuarioDesactivado = "Su usuario se encuentra deshabilitado, para habilitarlo es necesario acudir con el administrador del sistema";
        public const String BloquearUsuario = "Su usuario ha sido bloqueado por sobrepasar los intentos de inicio de sesión";
    }
    
    public class UsuarioLogueado
    {
        [Display(Name = "Nombre Usuario")]
        public String NombreCompleto { get; set; }
        [Display(Name = "Usuario")]
        public String Username { get; set; }
        public String IdFirma { get; set; }
        public String Rol { get; set; }
        public Int32 IdUsuario { get; set; }
        public byte Intentos { get; set; }
        public Byte IdRol { get; set; }
        public bool? ChangePass { get; set; }
        public bool Activo { get; set; }
        public String Nombre { get; set; }
        public String Ape1 { get; set; }
        public String Ape2 { get; set; }
        public String DefaultPage { get; set; }
        public String Conexion { get; set; }
    }
    
    public class CA_UsuariosModel
    {        
       
        public int IdUsuario { get; set; }
        [Required(ErrorMessage="El {0} es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El {0} es requerido", AllowEmptyStrings = true)]
        [Display(Name = "Primer apellido")]
        public string ApellidoPaterno { get; set; }
        [Required(ErrorMessage = "El {0} es requerido", AllowEmptyStrings = true)]
        [Display(Name = "Segundo apellido")]
        public string ApellidoMaterno { get; set; }
        [Required(ErrorMessage = "El {0} es requerido", AllowEmptyStrings = true)]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La {0} es requerida")]
        [Display(Name = "Contraseña")]
        public string Contrasenia { get; set; }
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage="Debe ser un correo")]
        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "e-mail")]
        public string email { get; set; }
        [Required(ErrorMessage="Debes seleccionar un perfil")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Selecciona un perfil")]
        [Display(Name="Perfil")]
        public byte IdPerfil { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<bool> CambiaContrasenia { get; set; }
        public Nullable<System.DateTime> FechaRegistro { get; set; }
        public byte Intentos { get; set; }
        public Nullable<bool> GeneradoAutomatico { get; set; }
        public string Titulo { get; set; }
        public string Cargo { get; set; }
        public Nullable<int> usuAct { get; set; }
        public Nullable<System.DateTime> fAct { get; set; }
        public CA_PerfilesModel CA_Perfiles { get; set; }

        public SelectList Perfiles { get; set; }
        public CA_UsuariosModel()
        {
            this.Perfiles = new SelectList(new PerfilesDAL().Get(), "IdPerfil", "Descripcion");
        }
        public CA_UsuariosModel(int IdPerfil)
        {
            this.Perfiles = new SelectList(new PerfilesDAL().Get(), "IdPerfil", "Descripcion",IdPerfil);
        }
    }

    public class CA_PerfilesModel
    {
        public byte IdPerfil { get; set; }
        [Required(ErrorMessage="*")]
        public string Descripcion { get; set; }
        public Nullable<int> usuAct { get; set; }
        public Nullable<System.DateTime> fAct { get; set; }

        public List<De_PermisosModel> DE_Permisos { get; set; }
    }

    public class De_PermisosModel
    {
        public int IdPermiso { get; set; }
        public short IdOpcion { get; set; }
        public byte IdPerfil { get; set; }
        public bool Activo { get; set; }

        public CA_OpcionesModel CA_Opciones { get; set; }
    }

    public class CA_OpcionesModel
    {
        public short IdOpcion { get; set; }
        public Nullable<short> IdOpcionP { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public string Sistema { get; set; }
        public string Descripcion { get; set; }
        public bool Barra { get; set; }
        public short BarraPadre { get; set; }
        public bool Menu { get; set; }
        public short MenuPadre { get; set; }
        [Display(Name="Ancho de submenús")]
        public Nullable<short> SubMenuWidth { get; set; }
        public Nullable<int> usuAct { get; set; }
        public Nullable<System.DateTime> fAct { get; set; }
        public bool hasPadre { get; set; }
        public Nullable<short> Orden { get; set; }
        public Nullable<bool> Mostrar { get; set; }
        
    }

    public class VW_BasesModel
    {
        public short dbid { get; set; }
        public string Nombre { get; set; }
    }

    public class ErrorCustom
    {
        /// <summary>
        /// true = error, false = éxito
        /// </summary>
        public bool Error { get; set; }
        public String Mensaje { get; set; }
        public UsuarioLogueado Usuario { get; set; }
        public ErrorCustom(bool E, String M)
        {
            this.Error = E;
            this.Mensaje = M;
            Usuario = new UsuarioLogueado();
        }
        public ErrorCustom()
        {
            Usuario = new UsuarioLogueado();
        }
    }

    public class Permisos
    {
        public String Perfil { get; set; }
        public byte? IdPerfil { get; set; }
        public List<TreeViewPermisos> PermisosLst { get; set; }
        public Permisos()
        {
            PermisosLst = new List<TreeViewPermisos>();
        }
    }
    
    public class TreeViewPermisos
    {
        public TreeViewPermisos()
        {
            Hijos = new HashSet<TreeViewPermisos>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public string Url { get; set; }
        public string Area { get; set; }
        public ICollection<TreeViewPermisos> Hijos { get; set; }
    }

    public class PermisosModel
    {
        public string IdGrupo { get; set; }
        public string Descripcion { get; set; }
        [JsonProperty(PropertyName = "checked")]
        public bool Activo { get; set; }
        public short? IdGrupoPadre { get; set; }
        public string Nombre { get; set; }
        public short?[] IdOpciones { get; set; }

        [JsonProperty(PropertyName = "children")]
        public List<PermisosModel> Childrens { get; set; }
        public PermisosModel()
        {
            this.Childrens = new List<PermisosModel>();
        }
    }

    public class DE_AccionesPermisosModel
    {
        public int IdRegistro { get; set; }
        public short IdAccion { get; set; }
        public int IdPermiso { get; set; }
        public bool Activo { get; set; }
        public string Descripcion { get; set; }
    }

    #region
    /*Botonera*/
    public class Ca_BotonesModel
    {
        public short IdBoton { get; set; }
        public string Nombre { get; set; }
        public string clase { get; set; }
        public string iconClass { get; set; }
        public string descripcion { get; set; }
        public Nullable<bool> habilitado { get; set; }
    }
    public class Botonera
    {
        public const int Compromisos = 1;

        //private Boton nuevo = new Boton { clase = "js_MNuevo", Nombre = "bNuevo", Descripcion = "Nuevo", Habilitado = true, iconClass = "fa fa-plus-circle" };
        //private Boton eliminar = new Boton { clase = "js_MEliminar", Nombre = "bEliminar", Descripcion = "Eliminar", Habilitado = true, iconClass = "fa fa-trash-o" };
        //private Boton cancelar = new Boton { clase = "js_MCancelar", Nombre = "bCancelar", Descripcion = "Cancelar", Habilitado = true, iconClass = "fa fa-ban" };
        //private Boton detalles = new Boton { clase = "js_MDetalles", Nombre = "bDetalles", Descripcion = "Detalles", Habilitado = true, iconClass = "fa fa-file-text-o" };
        //private Boton buscar = new Boton { clase = "js_MBuscar", Nombre = "bBuscar", Descripcion = "Buscar", Habilitado = true, iconClass = "fa fa-search" };
        //private Boton salir = new Boton { clase = "js_MSalir", Nombre = "bSalir", Descripcion = "Salir", Habilitado = true, iconClass = "fa fa-power-off" };
        //private Boton guardar = new Boton { clase = "js_MGuardar", Nombre = "bGuardar", Descripcion = "Guardar", Habilitado = true, iconClass = "fa fa-save" };
        //private Boton compromisos = new Boton { clase = "js_MCompromisos", Nombre = "bCompromisos", Descripcion = "Compromisos", Habilitado = true, iconClass = "fa fa-file-text" };

        public Botonera()
        { }

        public Botonera(List<Ca_BotonesModel> botones)
        {
            botones.ForEach(item => { this.Botones.Add(item); });
        }
        
        public Botonera(List<short> listaIdBotones)
        {
            this.Botones = new List<Ca_BotonesModel>();
            listaIdBotones.ForEach(item => { this.Botones.Add(ModelFactory.getModel<Ca_BotonesModel>(db.CA_Botones.First(reg => reg.IdBoton.Equals(item)), new Ca_BotonesModel())); });
        }

        public Botonera(List<string> listaNombresBotones)
        {
            this.Botones = new List<Ca_BotonesModel>();
            listaNombresBotones.ForEach(item => { this.Botones.Add(ModelFactory.getModel<Ca_BotonesModel>(db.CA_Botones.FirstOrDefault(reg => reg.Nombre.Equals(item)), new Ca_BotonesModel())); });
        }

        private ControlGeneralContainer db = new ControlGeneralContainer();
        public List<Ca_BotonesModel> Botones { get; set; }
    }
    public class Boton
    {
        public Boton()
        { 
        }

        public Boton(string _id, string _Nombre, string _Descripcion, string _iconClass, bool _Habilitado)
        {
            this.clase = _id;
            this.Nombre = _Nombre;
            this.Descripcion = _Descripcion;
            this.iconClass = _iconClass;
            this.Habilitado = _Habilitado;
        }

        public string clase { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string iconClass { get; set; }
        public bool Habilitado { get; set; }
    }
    #endregion

    public class Enums
    {
        public enum Format
        {
            xml,
            json,
            html,
        }

        public enum Action
        {
            borrar,
            obtener
        }

        public enum Methods
        {
            compromiso,
            devengado,
            requisicion,
            requisicion_cancelada,
            recepcion,
            orden,
            solicitud
        }
    }

    public class Headers
    {
        public String headerName { get; set; }
        public String headerValue { get; set; }
    }

    public class RestResponseListObject<T>
    {
        public REST_Service REST_Service { get; set; }
        public List<T> response { get; set; }
    }

    public class RestResponseObject<T>
    {
        public REST_Service REST_Service { get; set; }
        public T response { get; set; }
    }

    public class REST_Service
    {
        public string status_response { get; set; }
        public string message { get; set; }
        public string fecha { get; set; }
        public string hora { get; set; }
        public string response_key { get; set; }
        public string response_time { get; set; }
    }

    public class ResponseCancelacion
    {
        public bool error { get; set; }
        public string mensaje { get; set; }        
    }

    public class ResponseCancelada
    {
        public bool error { get; set; }
        public bool cancelado { get; set; }
        public string mensaje { get; set; }
    }

    public partial class VW_BeneficiariosModel
    {
        public int Id_Beneficiario { get; set; }
        public string IdBeneficiario { get; set; }
        public string Nombre { get; set; }
        public Nullable<byte> Id_ClasificaBene { get; set; }
        public string Clasificacion { get; set; }
        public string Id_Cuenta { get; set; }
        public string Id_CuentaFormato { get; set; }
    }
}