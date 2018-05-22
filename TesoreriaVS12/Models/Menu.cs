using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Script.Serialization;
using TesoreriaVS12.DAL;

namespace TesoreriaVS12.Models
{
    public sealed class Menu
    {
        private OpcionesDAL _opciones;

        public OpcionesDAL dalOpciones
        {
            get { return _opciones; }
            set { _opciones = value; }
        }

        public Menu()
        {
            if (dalOpciones == null) dalOpciones = new OpcionesDAL();
        }
        System.Web.Mvc.UrlHelper contextoWeb = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);
        public NodoMenu fillNodo(CA_Opciones op, Byte? IdPerfil, int[] IdReportes)
        {
            NodoMenu aux = new NodoMenu();
            aux.Accion = op.Accion;
            aux.Area = op.Sistema;
            aux.Controlador = op.Controlador;
            aux.Descripcion = op.Descripcion;
            aux.id = op.IdOpcion;
            aux.Menu = op.Menu;
            aux.MenuPadre = op.MenuPadre;
            if (!op.IdOpcionP.HasValue)
                aux.IdPadre = -1;
            else
                aux.IdPadre = op.IdOpcionP;


            if (dalOpciones.Get(x => x.MenuPadre == op.IdOpcion).Count() > 0)
            {
                aux.HasHijos = true;
                aux.subMenuWidth = String.Format("{0}px", op.SubMenuWidth);
                if (aux.Controlador != "#" && aux.Area != "#" && aux.Accion != "#" && !String.IsNullOrEmpty(aux.Accion))
                    //aux.Url = String.Format("/{0}/{1}/{2}", aux.Area, aux.Controlador, aux.Accion);
                    aux.Url = contextoWeb.Action(aux.Accion, aux.Area + "/" + aux.Controlador);///System.Web.HttpContext.Current.Server.MapPath(String.Format("/{0}/{1}/{2}", aux.Area, aux.Controlador, aux.Accion));
            }
            else
            {
                aux.HasHijos = false;
                if (String.IsNullOrEmpty(aux.Area) && aux.Controlador != "#")
                    aux.Url = contextoWeb.Action(aux.Accion, aux.Controlador);//String.Format("/{0}/{1}", aux.Controlador, aux.Accion);
                else if (aux.Controlador != "#")
                    aux.Url = contextoWeb.Action(aux.Accion, aux.Area + "/" + aux.Controlador);//String.Format("/{0}/{1}/{2}", aux.Area, aux.Controlador, aux.Accion);
            }
            if (IdPerfil.HasValue)
            {
                if (op.DE_Permisos.SingleOrDefault(x => x.Activo == true && x.IdOpcion == op.IdOpcion && x.IdPerfil == IdPerfil.Value) != null)
                    aux.Activo = true;
                else
                    aux.Activo = false;
            }
            if (IdReportes.Contains(aux.id))
                aux.hasReporte = true;
            return aux;
        }

        public List<NodoMenu> fillMenu(Byte? IdPerfil)
        {
            List<NodoMenu> _Menu = new List<NodoMenu>();
            int[] reportes = new JavaScriptSerializer().Deserialize<int[]>(ConfigurationManager.AppSettings.Get("IdReportes"));
            dalOpciones.Get(o => o.Menu == true).OrderBy(x => x.Orden).ToList().ForEach(x => { _Menu.Add(fillNodo(x, IdPerfil, reportes)); });
            return _Menu;
        }

        public List<NodoMenu> fillPadres(Byte? IdPerfil)
        {
            List<NodoMenu> _Nodos = new List<NodoMenu>();
            IEnumerable<CA_Opciones> _CaOpciones = new List<CA_Opciones>();
            //_CaOpciones = dalOpciones.Get(x => x.IdOpcionP == null && x.Controlador != "#" && x.Mostrar == true);
            _CaOpciones = dalOpciones.Get(x => x.IdOpcionP == null && x.Controlador != "#").OrderBy(x => x.Orden);
            int[] reportes = new JavaScriptSerializer().Deserialize<int[]>(ConfigurationManager.AppSettings.Get("IdReportes"));
            foreach (CA_Opciones item in _CaOpciones)
            {
                NodoMenu aux = fillNodo(item, IdPerfil, reportes);
                _Nodos.Add(aux);
            }
            return _Nodos;
        }

        public List<NodoMenu> fillHijos(Byte? IdPerfi, System.Linq.Expressions.Expression<Func<CA_Opciones, bool>> filter = null)
        {
            List<NodoMenu> _Nodos = new List<NodoMenu>();
            IEnumerable<CA_Opciones> _CaOpciones = new List<CA_Opciones>();
            int[] reportes = new JavaScriptSerializer().Deserialize<int[]>(ConfigurationManager.AppSettings.Get("IdReportes"));
            if (filter == null)
                //_CaOpciones = dalOpciones.Get(x => x.IdOpcionP == null && x.Mostrar == true);
                _CaOpciones = dalOpciones.Get(x => x.IdOpcionP == null);
            else
                _CaOpciones = dalOpciones.Get(filter);
            foreach (CA_Opciones item in _CaOpciones)
            {
                NodoMenu aux = fillNodo(item, IdPerfi, reportes);
                aux.Hijos = this.fillHijos(IdPerfi, x => x.IdOpcionP == item.IdOpcion);
                _Nodos.Add(aux);
            }
            return _Nodos;
        }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public sealed class NodoMenu
    {
        private String _Descripcion;
        private String _Controlador;
        private String _Accion;
        private String _Area;
        private Int32 _Id;
        private bool _Activo;
        private Int16? _IdPadre;
        private bool _HasHijos;
        private String _url;
        private bool? _menu;
        private Int16? _menuPadre;
        private List<NodoMenu> _Hijos;
        private String _subMenuWidth;

        [JsonProperty(PropertyName = "hasReporte")]
        public bool hasReporte { get; set; }

        [JsonProperty(PropertyName = "subMenuWidth")]
        public String subMenuWidth
        {
            get { return _subMenuWidth; }
            set { _subMenuWidth = value; }
        }

        [JsonProperty(PropertyName = "children")]
        public List<NodoMenu> Hijos
        {
            get { return _Hijos; }
            set { _Hijos = value; }
        }

        public NodoMenu()
        {
            Hijos = new List<NodoMenu>();
        }

        [JsonProperty(PropertyName = "parentid")]
        public Int16? MenuPadre
        {
            get { return _menuPadre; }
            set { _menuPadre = value; }
        }

        [JsonProperty(PropertyName = "checked")]
        public bool? Menu
        {
            get { return _menu; }
            set { _menu = value; }
        }

        [JsonProperty(PropertyName = "href")]
        public String Url
        {
            get { return _url; }
            set { _url = value; }
        }
        [JsonProperty(PropertyName = "parentidOp")]
        public Int16? IdPadre
        {
            get { return _IdPadre; }
            set { _IdPadre = value; }
        }
        [JsonProperty(PropertyName = "activo")]
        public bool Activo
        {
            get { return _Activo; }
            set { _Activo = value; }
        }
        [JsonProperty(PropertyName = "haschildren")]
        public bool HasHijos
        {
            get { return _HasHijos; }
            set { _HasHijos = value; }
        }
        [JsonProperty(PropertyName = "text")]
        public String Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }
        [JsonProperty(PropertyName = "controller")]
        public String Controlador
        {
            get { return _Controlador; }
            set { _Controlador = value; }
        }
        [JsonProperty(PropertyName = "id")]
        public Int32 id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        [JsonProperty(PropertyName = "area")]
        public String Area
        {
            get { return _Area; }
            set { _Area = value; }
        }
        [JsonProperty(PropertyName = "accion")]
        public String Accion
        {
            get { return _Accion; }
            set { _Accion = value; }
        }
    }
}