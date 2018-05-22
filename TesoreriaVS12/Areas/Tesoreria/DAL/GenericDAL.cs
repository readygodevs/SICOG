using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.Areas.Tesoreria.DAL
{
    public class DALGeneric
    {
        private BD_TesoreriaEntities _db;

        protected BD_TesoreriaEntities Db
        {
            get { return _db; }
            set { _db = value; }
        }

        //public BD_TesoreriaEntities db { get; set; }

        public DALGeneric()
        {
            if (Db == null) Db = new BD_TesoreriaEntities();
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            if (appUsuario != null)
                Db.ChangeDatabase(appUsuario.Conexion, Logueo.IP, Logueo.User, Logueo.Pass, false, "");
        }
    }
}