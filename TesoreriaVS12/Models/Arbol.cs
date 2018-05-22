using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.DAL;

namespace TesoreriaVS12.Models
{
    public sealed class Arbol
    {
        private PermisosDAL _permisos;
        private OpcionesDAL _opciones;

        public OpcionesDAL opciones
        {
            get { return _opciones; }
            set { _opciones = value; }
        }

        public PermisosDAL permisos
        {
            get { return _permisos; }
            set { _permisos = value; }
        }

        public Arbol()
        {
            if (permisos == null) permisos = new PermisosDAL();
            if (opciones == null) opciones = new OpcionesDAL();
        }


        public List<TreeViewPermisos> fillHijos(Byte? IdPerfil, System.Linq.Expressions.Expression<Func<CA_Opciones, bool>> filter = null)
        {
            List<TreeViewPermisos> _Nodos = new List<TreeViewPermisos>();
            IEnumerable<CA_Opciones> _CaOpciones = new List<CA_Opciones>();
            if (filter == null)
                _CaOpciones = opciones.Get(x => x.IdOpcionP == null);
            else
                _CaOpciones = opciones.Get(filter);
            foreach (CA_Opciones item in _CaOpciones)
            {
                TreeViewPermisos aux = this.fillNode(item, IdPerfil);
                aux.Hijos = this.fillHijos(IdPerfil,x => x.IdOpcionP == item.IdOpcion);
                _Nodos.Add(aux);
            }
            return _Nodos;
        }

        public TreeViewPermisos fillNode(CA_Opciones opciones, Byte? IdPerfil)
        {
            TreeViewPermisos aux = new TreeViewPermisos();
            aux.Id = opciones.IdOpcion;
            aux.Accion = opciones.Accion;
            aux.Area = opciones.Sistema;
            aux.Controlador = opciones.Controlador;
            aux.Descripcion = opciones.Descripcion;
            if (IdPerfil.HasValue)
            {
                DE_Permisos temp = opciones.DE_Permisos.SingleOrDefault(x => x.IdPerfil == IdPerfil.Value && x.IdOpcion == aux.Id);
                if (temp != null)
                    aux.Activo = temp.Activo;
                else
                    aux.Activo = false;
            }
            return aux;
        }
    }
}