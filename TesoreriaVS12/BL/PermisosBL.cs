using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.DAL;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.BL
{
    public class PermisosBL
    {
        protected VWPermisosDAL dalVWPermisos { get; set; }
        protected vwPermisos2DAL dalPermisos2 { get; set; }
        protected OpcionesDAL dalOpciones { get; set; }
        protected PermisosDAL dalPermisos {get; set;}
        
        protected GruposDAL  dalGRupos { get; set; }
        protected DeAccionesPermisosDAL dalDePermisosAcciones { get; set; }
        protected DeOpcionesGruposDAL dalDeOpcionesGrupos { get; set; }
        protected DeGrupoPerfilDAL dalDeGrupoPerfil { get; set; }

        protected DeGruposOpcionesDAL dalDeGrupoOpciones { get; set; }
        private UsuariosDAL dalUsuarios { get; set; }

        public PermisosBL()
        {
            if (dalVWPermisos == null) dalVWPermisos = new VWPermisosDAL();
            if (dalPermisos2 == null) dalPermisos2 = new vwPermisos2DAL();
            if (dalOpciones == null) dalOpciones = new OpcionesDAL();
            if (dalPermisos == null) dalPermisos = new PermisosDAL();
            if (dalGRupos == null) dalGRupos = new GruposDAL();
            if (dalDePermisosAcciones == null) dalDePermisosAcciones = new DeAccionesPermisosDAL();
            if (dalDeOpcionesGrupos == null) dalDeOpcionesGrupos = new DeOpcionesGruposDAL();
            if (dalUsuarios == null) dalUsuarios = new UsuariosDAL();
            if (dalDeGrupoPerfil == null) dalDeGrupoPerfil = new DeGrupoPerfilDAL();
            if (dalDeGrupoOpciones == null) dalDeGrupoOpciones = new DeGruposOpcionesDAL();
        }

        public bool existPermiso(string Controller, string Acction, string sistema)
        {
            CA_Opciones opcion = dalOpciones.GetByID(x => x.Controlador == Controller && x.Accion == Acction && x.Sistema == sistema);
            return (!(opcion == null));
        }

        public bool hasPermiso(string Controller, string Acction, string sistema, byte IdPerfil, string Method)
        {
            #if DEBUG
            if (!this.existPermiso(Controller, Acction, sistema))
            {
                this.registrarAccion(Controller, Acction, sistema, IdPerfil);
                return true;
            }
            #endif
            try
            {
                bool Get = false, Post = false;
                if(Method == "GET") Get = true;
                if(Method == "POST") Post = true;
                CA_Opciones opcion = dalOpciones.GetByID(x => x.Controlador == Controller && x.Accion == Acction && x.Sistema == sistema);
                if (dalDeGrupoOpciones.Get(x => x.IdOpcion == opcion.IdOpcion).Count() == 0) return true;
                short? Grupo = dalDeGrupoOpciones.GetByID(x => x.IdOpcion == opcion.IdOpcion).IdGrupo;
                De_GrupoPerfil permiso = dalDeGrupoPerfil.GetByID(x => x.IdGrupo == Grupo && x.IdPerfil == IdPerfil);
                if(permiso.Activo == true && (permiso.GET == Get || permiso.POST == Post))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            
            
            //if(dalVWPermisos.Get(x => x.Controlador == Controller && x.Accion == Acction && x.Sistema == sistema && x.IdPerfil == IdPerfil && (x.Activo == true || x.ActivoAccion == true)).Count() != 0)
            //    return true;
            //else 
            //    return false;
        }
        
        public bool hasHijos(int IdGrupo)
        {
            return ((dalGRupos.Get(x => x.IdGrupoPadre == IdGrupo).Select(s=> s.IdGrupo).Count() > 0));
        }

        public bool hasAcciones(int IdPermiso)
        {
            return ((dalDePermisosAcciones.Get(x=> x.IdPermiso == IdPermiso).Select(s=> s.IdAccion).Count() > 0));
        }
        
        //Juls
        public List<PermisosModel> fillChildren(System.Linq.Expressions.Expression<Func<CA_Grupos, bool>> filter = null, byte IdPerfil = 1)
        {
            IEnumerable<CA_Grupos> grupos = null;
            List<PermisosModel> PermisosLst = new List<PermisosModel>();
            if (filter == null)
                grupos = dalGRupos.Get(x => x.IdGrupoPadre == null);
            else
                grupos = dalGRupos.Get(filter);
            foreach (CA_Grupos item in grupos)
            {
                PermisosModel _aux = this.fillNode(item.IdGrupo, IdPerfil);
                if (_aux.Childrens.Count == 0)
                    _aux.Childrens = this.fillChildren(x => x.IdGrupoPadre == item.IdGrupo, IdPerfil);
                PermisosLst.Add(_aux);
            }
            return PermisosLst;
        }

        public PermisosModel fillNode(short IdGrupo, byte IdPerfil)
        {
            CA_Grupos grupo = dalGRupos.GetByID(x => x.IdGrupo == IdGrupo);
            PermisosModel aux = new PermisosModel();
            aux.IdGrupo = IdGrupo.ToString();
            aux.IdGrupoPadre = grupo.IdGrupoPadre;
            aux.Nombre = grupo.Nombre;
            aux.Descripcion = grupo.Descripcion;
            aux.Activo = Convert.ToBoolean(dalDeGrupoPerfil.GetByID(x=> x.IdGrupo == IdGrupo && x.IdPerfil == IdPerfil).Activo);
            //if (dalGRupos.Get(x => x.IdGrupoPadre == IdGrupo).Count() == 0)
            //    aux.Childrens = LlenarDeAccionesPermisosModel(IdGrupo, IdPerfil);
            return aux;
        }

        public List<PermisosModel> LlenarDeAccionesPermisosModel(byte IdGrupo, byte IdPerfil)
        {
            List<PermisosModel> aux = new List<PermisosModel>();
            foreach (VW_Permisos item in dalVWPermisos.Get(vw => vw.IdGrupo == IdGrupo && vw.IdPerfil == IdPerfil && vw.IdAccion != null && vw.DescripcionOpcion == null))
            {
                if (aux.Where(x => x.IdGrupo == String.Format("{0}-{1}", item.IdGrupo, item.IdAccion)).Count() == 0)
                {
                    PermisosModel temp = new PermisosModel();
                    temp.Activo = item.ActivoAccion.HasValue ? item.ActivoAccion.Value : false;
                    temp.Descripcion = item.DescripcionAccion;
                    temp.IdGrupo = String.Format("{0}-{1}", item.IdGrupo, item.IdAccion);
                    temp.IdGrupoPadre = item.IdGrupo;
                    temp.Nombre = "";
                    aux.Add(temp);
                }

            }
            return aux;
        }



        public bool ActualizarPermisos(List<PermisosModel> permisos, byte IdPerfil)
        {
            try
            {
                foreach(PermisosModel item in permisos)
                {
                    
                    if (item.Childrens.Count() > 0)
                    {
                        this.ActualizarPermisos(item.Childrens, IdPerfil);
                    }
                    UpdatePermision(Convert.ToInt16(item.IdGrupo), item.Activo, IdPerfil);
                    //if (item.Childrens.Count > 0)
                    //{
                    //    if (item.Activo)
                    //    {
                    //        IEnumerable<DE_Permisos> aux = getPermisionByOptions(this.getOptionsByGroup(Convert.ToInt32(item.IdGrupo)), IdPerfil);
                    //        aux.ToList().ForEach(x => { UpdatePermision(x.IdPermiso, true, IdPerfil); });
                    //    }
                    //    else
                    //        ActualizarPermisos(item.Childrens, IdPerfil);
                    //}
                    //else
                    //{
                    //    IEnumerable<DE_Permisos> aux = getPermisionByOptions(this.getOptionByAction(item.IdGrupo), IdPerfil);
                    //    aux.ToList().ForEach(x => { UpdatePermision(x.IdPermiso, item.Activo, IdPerfil); });
                    //}
                }
                dalDeGrupoPerfil.Save();

                //dalPermisos.Save();
                //dalDePermisosAcciones.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void UpdatePermision(short IdGrupo, bool Activo, byte IdPerfil)
        {
            
            De_GrupoPerfil degrupo = dalDeGrupoPerfil.GetByID(x=> x.IdGrupo == IdGrupo && IdPerfil == x.IdPerfil);
            degrupo.Activo = Activo;
            //DE_Permisos temp = dalPermisos.GetByID(x => x.IdPermiso == IdPermiso && x.IdPerfil == IdPerfil);
            //DE_AccionesPermisos action = dalDePermisosAcciones.GetByID(x => x.IdPermiso == IdPermiso && x.IdPerfil == IdPerfil);
            //temp.Activo = false;
            //if (action != null)
            //    action.Activo = Activo;
        }

        private IEnumerable<DE_Permisos> getPermisionByOptions(short[] IdOpciones, byte IdPerfil)
        {
            return dalPermisos.Get(x => IdOpciones.Contains(x.IdOpcion) && x.IdPerfil == IdPerfil);
        }

        private short[] getOptionsByGroup(int IdGrupo)
        {
            return dalVWPermisos.Get(x => x.IdGrupo == IdGrupo).Select(s=> s.IdOpcion).ToArray<short>();
        }

        private short[] getOptionByAction(string IdGrupo)
        {
            int Grupo = Convert.ToInt32(IdGrupo.Split('-')[0]);
            int IdAccion = Convert.ToInt32(IdGrupo.Split('-')[1]);
            return dalVWPermisos.Get(x => x.IdGrupo == Grupo && x.IdAccion == IdAccion).Select(s => s.IdOpcion).ToArray<short>();
        }
        
        public void registrarAccion(string Controlador, string Accion, string Area, byte Perfil)
        {
            CA_Opciones padre = dalOpciones.GetByID(x => x.Controlador == Controlador && x.Accion == null && x.Sistema == Area);
            try
            {
                if (padre == null)
                {
                    padre = new CA_Opciones();
                    padre.Accion = null;
                    padre.Controlador = Controlador;
                    padre.fAct = DateTime.Now;
                    padre.IdOpcionP = null;
                    padre.usuAct = 1;
                    padre.Descripcion = Controlador;
                    padre.Sistema = Area;
                    padre.SubMenuWidth = 200;
                    dalOpciones.Insert(padre);
                    dalOpciones.Save();
                    DE_Permisos permiso = new DE_Permisos();
                    permiso.Activo = true;
                    permiso.IdOpcion = padre.IdOpcion;
                    permiso.IdPerfil = Perfil;
                    dalPermisos.Insert(permiso);
                    dalPermisos.Save();
                    CA_Opciones hijos = new CA_Opciones();
                    hijos.Accion = Accion;
                    hijos.Sistema = Area;
                    hijos.IdOpcionP = padre.IdOpcion;
                    hijos.Controlador = Controlador;
                    hijos.fAct = DateTime.Now;
                    hijos.Descripcion = Accion;
                    hijos.SubMenuWidth = 200;
                    hijos.usuAct = 1;
                    dalOpciones.Insert(hijos);
                    dalOpciones.Save();
                    permiso = new DE_Permisos();
                    permiso.IdPerfil = Perfil;
                    permiso.IdOpcion = hijos.IdOpcion;
                    permiso.Activo = true;
                    dalPermisos.Insert(permiso);
                    dalPermisos.Save();
                }
                CA_Opciones hijo = dalOpciones.GetByID(x => x.Controlador == Controlador && x.Accion == Accion && x.Sistema == Area);
                if (hijo == null)
                {
                    hijo = new CA_Opciones();
                    hijo.Accion = Accion;
                    hijo.Controlador = Controlador;
                    hijo.fAct = DateTime.Now;
                    hijo.IdOpcionP = padre.IdOpcion;
                    hijo.usuAct = 1;
                    hijo.Sistema = Area;
                    hijo.SubMenuWidth = 200;
                    dalOpciones.Insert(hijo);
                    dalOpciones.Save();
                    DE_Permisos permiso = new DE_Permisos();
                    permiso.IdPerfil = Perfil;
                    permiso.IdOpcion = hijo.IdOpcion;
                    permiso.Activo = true;
                    dalPermisos.Insert(permiso);
                    dalPermisos.Save();
                }
            }
            catch (Exception ex)
            {
                
            }
            
        }

        //Funciones para los usuarios
        public String getUserHash(int idUsuario)
        {
            CA_Usuarios usuario = dalUsuarios.GetByID(x => x.IdUsuario == idUsuario);
            return usuario.Contrasenia;
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
    }

   

}