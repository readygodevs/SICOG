using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.DAL;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Areas.Tesoreria.Models
{



    public class Ca_AccionesModel
    {
        [Display(Name = "Proyecto")]
        [Required(ErrorMessage = "*")]
        public string Id_Proceso { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Actividad")]
        public string Id_ActividadMIR2 { get; set; }
        [Required(ErrorMessage = "*")]
        public short Id_Accion { get; set; }
        [Required(ErrorMessage = "*")]
        public string Id_Accion2 { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_ProyectoModel CA_Proyecto { get; set; }
        public Ca_ActividadModel CA_Actividad { get; set; }

        public Ca_AccionesModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_ActEconomicaModel
    {
        public short Sector { get; set; }
        public short Actividad { get; set; }
        [Display(Name = "Número Actividad")]
        public string Id_ActEconomica { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_SectoresModel CA_Sector { get; set; }

        public Ca_ActEconomicaModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_ActividadModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Proyecto/Proceso")]
        public string Id_Proceso { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Descripción Proyecto/Proceso")]
        public string DescripcionProceso { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(2, ErrorMessage = "Máximo dos caracteres.", MinimumLength = 1)]
        [Display(Name = "Número de actividad")]
        public byte Id_ActividadMIR { get; set; }
        [Display(Name = "Actividad MIR")]
        public string Id_ActividadMIR2 { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Descripción Actividad MIR")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_ProyectoModel CA_Proyecto { get; set; }

        public Ca_ActividadModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
        public bool Valida(Int16 id, string descripcion, ref string msn)
        {
            try
            {
                Ca_Actividad clas = new Ca_Actividad();
                ActividadMIRDAL clasModel = new ActividadMIRDAL();

                //Si esta repetido el ID
                clas = clasModel.GetByID(x => x.Id_ActividadMIR == id);
                if (clas != null)
                {
                    msn = "Ya existe está clave de actividad";
                    return false;
                }
                else
                {
                    //return ValidaDesc(descripcion, ref msn);
                    return true;
                }
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaDesc(string descripcion, ref string msn)
        {
            try
            {
                /*ClasProgramaticaDAL actividadModel = new ClasProgramaticaDAL();
                Ca_ClasProgramatica funcion = actividadModel.GetByID(x => x.Descripcion == descripcion);
                if (funcion != null)
                {
                    msn = StringID.Yaexiste + "Funcion";
                    return false;
                }
                msn = "Validación Correcta";*/
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
    }

    public class Ca_ActividadesInstModel
    {
        [Required(ErrorMessage = "*")]
        [StringLength(2, ErrorMessage = "Máximo dos caracteres.", MinimumLength = 1)]
        [Display(Name = "Compromiso")]
        public string Id_Actividad { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_ActividadesInstModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
        public bool Valida(string idActividad, string descripcion, ref string msn)
        {
            try
            {
                string Id = "";
                Ca_ActividadesInst actividad = new Ca_ActividadesInst();
                ActividadDAL actividadModel = new ActividadDAL();

                //Si esta repetido el ID
                actividad = actividadModel.GetByID(x => x.Id_Actividad == idActividad);
                if (actividad != null)
                {
                    msn = "Ya existe está clave actividad";
                    return false;
                }
                else
                {
                    return ValidaDesc(descripcion, ref msn);
                }
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaDesc(string descripcion, ref string msn)
        {
            try
            {
                ActividadDAL actividadModel = new ActividadDAL();
                Ca_ActividadesInst funcion = actividadModel.GetByID(x => x.Descripcion == descripcion);
                if (funcion != null)
                {
                    msn = StringID.Yaexiste + "Funcion";
                    return false;
                }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
    }

    public class Ca_AlcanceGeoModel
    {
        [Display(Name = "Dimensión Geográfica")]
        public string Id_AlcanceGeo { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_AlcanceGeoModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_AlmacenesModel
    {
        public short Id_Almacen { get; set; }
        public string Descripcion { get; set; }
        public string Id_Area { get; set; }
        public string Id_Programa { get; set; }
        public Nullable<bool> Almacen_Activo { get; set; }
        public string Domicilio_Almacen { get; set; }
        public string Responsable_Almacen { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_AreasModel Ca_Areas { get; set; }
        public Ca_ProgramasModel Ca_Programas { get; set; }

        public Ca_AlmacenesModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_AreasModel
    {

        [Required(ErrorMessage = "*")]
        [MaxLength(2)]
        [Display(Name = "Unidad Presupuestal")]
        public byte Id_UP { get; set; }
        [Required(ErrorMessage = "*")]
        [MaxLength(2)]
        [Display(Name = "Unidad Responsable")]
        public byte Id_UR { get; set; }
        [Required(ErrorMessage = "*")]
        [MaxLength(2)]
        [Display(Name = "Unidad Ejecutora")]
        public byte Id_UE { get; set; }
        [Display(Name = "Id Area")]
        public string Id_Area { get; set; }
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "*")]
        [MinLength(1, ErrorMessage = "*")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Último Nivel")]
        public Nullable<bool> UltimoNivel { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_AreasModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            UltimoNivel = false;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }

        public bool ValidaAdd(byte Id_UP, byte Id_UR, byte Id_UE, string Descripcion, bool? Nivel, ref string msn)
        {
            try
            {
                string Id = "";
                Ca_Areas areas = new Ca_Areas();
                AreasDAL areasmodel = new AreasDAL();

                //Si esta repetido el ID
                Id = StringID.IdArea(Id_UP, Id_UR, Id_UE);
                areas = areasmodel.GetByID(x => x.Id_Area == Id);
                if (areas != null)
                {
                    msn = "Ya existe éste Centro Gestor";
                    return false;
                }
                else
                {
                    if (!ValidaIdArea(Id_UP, Id_UR, Id_UE, Nivel, ref msn))
                        return false;
                    //return ValidaDesc(Descripcion, ref msn);
                }
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        //public bool ValidaDesc(string Descripcion, ref string msn)
        //{
        //    try
        //    {
        //        AreasDAL areasmodel = new AreasDAL();
        //        Ca_Areas areas = areasmodel.GetByID(x => x.Descripcion == Descripcion);
        //        if (areas != null)
        //        {
        //            msn = StringID.Yaexiste +"Centro Gestor";
        //            return false;
        //        }
        //        msn = "Validación Correcta";
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        msn = StringID.Exceptions + ex.Message; 
        //        return false;
        //    }
        //}
        public bool ValidaEdit(byte Id_UP, byte Id_UR, byte Id_UE, string Descripcion, bool? Nivel, ref string msn)
        {
            try
            {
                AreasDAL areasmodel = new AreasDAL();
                Ca_Areas areas = areasmodel.GetByID(x => x.Id_UP == Id_UP && x.Id_UR == Id_UR && x.Id_UE == Id_UE);
                //if(areas.Descripcion != Descripcion)
                //    if (!ValidaDesc(Descripcion, ref msn)) return false;
                if (areas.UltimoNivel != Nivel)
                    return ValidaIdAreaEdit(Id_UP, Id_UR, Id_UE, ref msn);
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaDelete(byte Id_UP, byte Id_UR, byte Id_UE, ref string msn)
        {
            try
            {
                AreasDAL areasmodel = new AreasDAL();
                int c = 0;
                if (Id_UP > 0 && Id_UR == 0 && Id_UE == 0)
                {
                    c = areasmodel.Get(x => x.Id_UP == Id_UP).Count();
                    if (c > 1)
                    {
                        msn = "No se puede eliminar porque ese Centro Gestor tiene registros descendentes";
                        return false;
                    }
                }
                else
                    if (Id_UP > 0 && Id_UR > 0 && Id_UE == 0)
                    {
                        c = areasmodel.Get(x => x.Id_UP == Id_UP && x.Id_UR == Id_UR).Count();
                        if (c > 1)
                        {
                            msn = "No se puede eliminar porque ese Centro Gestor tiene registros descendentes";
                            return false;
                        }
                    }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaIdAreaEdit(byte Id_UP, byte Id_UR, byte Id_UE, ref string msn)
        {
            try
            {
                AreasDAL areasmodel = new AreasDAL();
                int c = 0;
                if (Id_UP > 0 && Id_UR == 0 && Id_UE == 0)
                {
                    c = areasmodel.Get(x => x.Id_UP == Id_UP).Count();
                    if (c > 1)
                    {
                        msn = StringID.EditarUltimoNivel("descendentes");
                        return false;
                    }
                }
                else
                    if (Id_UP > 0 && Id_UR > 0 && Id_UE == 0)
                    {
                        c = areasmodel.Get(x => x.Id_UP == Id_UP && x.Id_UR == Id_UR).Count();
                        if (c > 1)
                        {
                            msn = StringID.EditarUltimoNivel("descendentes");
                            return false;
                        }
                    }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaIdArea(byte Id_UP, byte Id_UR, byte Id_UE, bool? Nivel, ref string msn)
        {
            try
            {
                AreasDAL areasmodel = new AreasDAL();
                Ca_Areas areas = null;
                string Id = "";
                if (Id_UP > 0 && Id_UR == 0 && Id_UE == 0)
                {
                }
                else
                {
                    if (Id_UP > 0 && Id_UR > 0 && Id_UE == 0)
                        Id = StringID.IdArea(Id_UP, 0, 0);
                    else
                        if (Id_UP > 0 && Id_UR > 0 && Id_UE > 0)
                            Id = StringID.IdArea(Id_UP, Id_UR, 0);
                    areas = areasmodel.GetByID(x => x.Id_Area == Id);
                    if (areas == null)
                    {
                        msn = "El Centro Gestor que usted intenta guardar no tiene un registro ascendente, favor de verificarlo";
                        return false;
                    }
                    else
                    {
                        if (areas.UltimoNivel == true)
                        {
                            msn = "El Centro Gestor que usted intenta guardar tiene un registro ascendente de último nivel, favor de verificarlo";
                            return false;
                        }
                    }

                }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
    }


    public class Ca_BancosModel
    {
        public short Id_Banco { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Número de Cuenta")]
        public string Id_Cuenta { get; set; }
        public string BancoFormato { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        [Display(Name = "Banco RH")]
        [Required(ErrorMessage = "*")]
        public Nullable<short> Id_BancoRH { get; set; }
        public Ca_CuentasModel Ca_Cuentas { get; set; }
        public SelectList ListaIdBancoRH { get; set; }

        public Ca_BancosModel()
        {
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_BancosRHModel
    {
        public short Id_BancoRH { get; set; }
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_BancosRHModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_PersonasModel
    {
        public int IdPersona { get; set; }
        public Nullable<int> IdRepresentanteLegal { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "*")]
        public string Nombre { get; set; }
        [Display(Name = "Apellido paterno")]
        [Required(ErrorMessage = "*")]
        public string ApellidoPaterno { get; set; }
        [Display(Name = "Apellido materno")]
        [Required(ErrorMessage = "*")]
        public string ApellidoMaterno { get; set; }
        [Display(Name = "R.F.C.")]
        [Required(ErrorMessage = "*")]
        public string RFC { get; set; }
        [Display(Name = "C.U.R.P.")]
        [Required(ErrorMessage = "*")]
        public string CURP { get; set; }
        [Required(ErrorMessage = "*")]
        public Nullable<bool> PersonaFisica { get; set; }
        [Display(Name = "Razón social")]
        [Required(ErrorMessage = "*")]
        public string RazonSocial { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "País")]
        public Nullable<byte> IdPais { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Estado")]
        public Nullable<byte> IdEstado { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Municipio")]
        public Nullable<short> IdMunicipio { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Localidad")]
        public Nullable<short> IdLocalidad { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Colonia")]
        public Nullable<short> IdColonia { get; set; }
        public string Colonia { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Calle")]
        public Nullable<short> IdCalle { get; set; }
        public string Calle { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Número exterior")]
        public string NumeroExt { get; set; }
        [Display(Name = "Número interior")]
        public string NumeroInt { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Código postal")]
        [DataType(DataType.PostalCode)]
        public string CP { get; set; }
        [Display(Name = "Teléfono(s)")]
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Fax { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Nombre Completo")]

        public string NombreCompleto { get; set; }
        public string Domicilio { get; set; }
        public bool Resultado { get; set; }

        public Ca_PaisesModel Ca_PaisesModel { get; set; }
        public Ca_EstadosModel Ca_EstadosModel { get; set; }
        public Ca_MunicipiosModel Ca_MunicipiosModel { get; set; }
        public Ca_LocalidadesModel Ca_LocalidadesModel { get; set; }
        public Ca_ColoniasModel Ca_ColoniasModel { get; set; }
        public Ca_CallesModel Ca_CallesModel { get; set; }

        public SelectList ListaIdEstado { get; set; }
        public SelectList ListaIdMunicipio { get; set; }
        public SelectList ListaIdLocalidad { get; set; }
        public SelectList ListaIdCalle { get; set; }
        public SelectList ListaIdColonia { get; set; }

        public int Id_Calculate()
        {
            try
            {
                int c = new PersonasDAL().Get().Max(x => x.IdPersona);
                c++;
                return c;
            }
            catch
            {
                return 1;
            }
        }
    }

    public class Ca_BeneficiariosModel : Ca_PersonasModel
    {
        public int Id_Beneficiario { get; set; }
        public int IdPersona { get; set; }
        public Nullable<byte> Id_TipoBeneficiario { get; set; }
        public Nullable<byte> Id_TipoPago { get; set; }
        public Nullable<short> Id_CtaBancaria { get; set; }
        public string Observaciones { get; set; }
        public string ClasificaProvedor { get; set; }
        public string Referencia { get; set; }
        public Nullable<byte> IdCamara { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        //[Required(ErrorMessage = "*")]
        [Display(Name = "Clasificaciones del Beneficiario")]
        public string listaidclasificacion { get; set; }
        [Display(Name = "Giros del Beneficiario")]
        public string listaidgiros { get; set; }
        [Display(Name = "Lista de Contactos")]
        public string listacontactos { get; set; }

        public Ca_CamaraComercioModel CA_CamaraComercioModel { get; set; }
        public Ca_CuentasBancariasModel Ca_CuentasBancariasModel { get; set; }
        public Ca_TipoBeneficiariosModel Ca_TipoBeneficiariosModel { get; set; }
        public SelectList ListaIdEstado { get; set; }
        public SelectList ListaIdMunicipio { get; set; }
        public SelectList ListaIdLocalidad { get; set; }
        public string Colonia { get; set; }
        public SelectList ListaIdColonia { get; set; }
        public string Calle { get; set; }
        public SelectList ListaIdCalle { get; set; }
        public List<Ca_BeneficiariosCuentasModel> ListaCa_BeneficiariosCuentasModel { get; set; }
        public List<De_BeneficiarioContactosModel> ListaDE_BeneficiarioContactosModel { get; set; }
        public List<De_Beneficiarios_GirosModel> ListaDe_Beneficiarios_GirosModel { get; set; }

        public Ca_BeneficiariosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            this.ListaCa_BeneficiariosCuentasModel = new List<Ca_BeneficiariosCuentasModel>();
            this.ListaDE_BeneficiarioContactosModel = new List<De_BeneficiarioContactosModel>();
            this.ListaDe_Beneficiarios_GirosModel = new List<De_Beneficiarios_GirosModel>();
            Fecha_Act = DateTime.Now;
        }

        public int Id_Calculate()
        {
            try
            {
                int c = new BeneficiariosDAL().Get().Max(x => x.Id_Beneficiario);
                c++;
                return c;
            }
            catch
            {
                return 1;
            }
        }
    }

    public class Ca_BeneficiariosCuentasModel
    {
        public int Id_Beneficiario { get; set; }
        public byte Id_ClasBeneficiario { get; set; }
        public string Id_Cuenta { get; set; }
        public Nullable<short> Usu_act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_CuentasModel Ca_Cuentas { get; set; }
        public Ca_ClasificaBeneficiariosModel Ca_ClasificaBeneficiarios { get; set; }

        public Ca_BeneficiariosCuentasModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_CallesModel
    {
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "*")]
        public byte Id_Estado { get; set; }
        [Display(Name = "Municipio")]
        [Required(ErrorMessage = "*")]
        public short Id_Municipio { get; set; }
        [Display(Name = "Localidad")]
        [Required(ErrorMessage = "*")]
        public short Id_Localidad { get; set; }
        [Display(Name = "Calle")]
        public short id_calle { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_EstadosModel CA_Estado { get; set; }
        public Ca_MunicipiosModel CA_Municipio { get; set; }
        public Ca_LocalidadesModel CA_Localidad { get; set; }
        public SelectList ListaIdEstado { get; set; }
        public SelectList ListaIdMunicipio { get; set; }
        public SelectList ListaIdLocalidad { get; set; }
        public Ca_CallesModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }

        public short Id_Calculate(byte IdEstado, short IdMunicipio, short IdLocalidad)
        {
            CallesDAL calles = new CallesDAL();
            try
            {
                short c = calles.Get(x => x.Id_Estado == IdEstado && x.Id_Municipio == IdMunicipio && x.Id_Localidad == IdLocalidad).Max(x => x.id_calle);
                c++;
                return c;
            }
            catch
            {
                return 1;
            }
        }
    }

    public class Ca_CausaCancelacionModel
    {
        [Display(Name = "Tipo de Causa")]
        public short Id_TipoCausa { get; set; }
        [Display(Name = "Causa")]
        public short Id_Causa { get; set; }
        [Display(Name = "Descripción")]
        public string Descrip { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_CausaCancelacionModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_CierreBancoModel
    {
        [Display(Name = "Cuenta Bancaria")]

        public short Id_CtaBancaria { get; set; }
        [Display(Name = "Mes")]

        public short Id_Mes { get; set; }
        public Nullable<bool> Cerrado_Contable { get; set; }
        public Nullable<bool> Cerrado_Conciliado { get; set; }
        [Display(Name = "Saldo Inicial")]

        public Nullable<decimal> Saldo_Inicial { get; set; }
        public Nullable<decimal> Retiros { get; set; }
                [Display(Name = "Depósitos")]

        public Nullable<decimal> Depositos { get; set; }
        [Display(Name = "Saldo Final")]

        public Nullable<decimal> Saldo_Final { get; set; }
        public Nullable<decimal> Cargos_Conciliacion { get; set; }
        public Nullable<decimal> Abonos_Conciliacion { get; set; }
        public Nullable<decimal> Saldo_Conciliado { get; set; }
        public Nullable<decimal> Saldo_EdoCta { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_CuentasBancariasModel Ca_CuentasBancarias { get; set; }

        public Ca_CierreBancoModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usuario_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_CierreMensualModel
    {
        public byte Id_Mes { get; set; }
        public Nullable<bool> Contable { get; set; }
        public Nullable<bool> Conciliado { get; set; }
        public Nullable<bool> Bloqueado { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public string desc { get; set; }

        public Ca_CierreMensualModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
        public bool validaMes(Int16 idMes)
        {
            CierreMensualDAL dal = new CierreMensualDAL();
            Ca_CierreMensual ca = dal.GetByID(x => x.Id_Mes == idMes);
            if (ca != null)
                return ca.Contable.Value;
            return false;
        }
        public int ObtenerMes()
        {
            CierreMensualDAL DALCierre = new CierreMensualDAL();
            Ca_CierreMensual ca = DALCierre.Get(x => x.Contable == true).OrderByDescending(x => x.Id_Mes).FirstOrDefault();
            if (ca != null)
            {
                return ca.Id_Mes + 1;
            }
            else
                return 0;
        }

        public int ObtenerMesCierre()
        {
            CierreMensualDAL DALCierre = new CierreMensualDAL();
            Ca_CierreMensual ca = DALCierre.GetByID(x => x.Contable == true);
            if (ca != null)
                return ca.Id_Mes + 1;
            else
                return 0;
        }

    }

    public class Ca_ClasificaBeneficiariosModel
    {
        [Display(Name = "Clasificación del Beneficiario")]
        public byte Id_ClasificaBene { get; set; }
        [Display(Name = "Descripción"), MaxLength(100, ErrorMessage = "Máximo 100 caracteres"), Required]
        public string Descripcion { get; set; }
        [Display(Name = "Cuenta")]
        [Required(ErrorMessage = "*")]
        public string Id_Cuenta { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_CuentasModel Ca_Cuentas { get; set; }
        public Ca_ClasificaBeneficiariosModel Ca_ClasificaBeneficiarios { get; set; }

        public Ca_ClasificaBeneficiariosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_ClasificaPolizasModel
    {
        [Display(Name = "Tipo Póliza")]
        public byte Id_TipoPoliza { get; set; }
        [Display(Name = "Clasificación")]
        public byte Id_ClasificaPoliza { get; set; }
        [Display(Name = "Subclasificación")]
        public byte Id_SubClasificaPoliza { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Automática")]
        public bool Automatica { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_ClasificaPolizasModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_ClasProgramaticaModel
    {
        [Required(ErrorMessage = "*")]
        [StringLength(1, ErrorMessage = "Máximo un caracteres.", MinimumLength = 1)]
        [Display(Name = "Clasificación")]
        public string Id_ClasificacionP { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_ClasProgramaticaModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
        public bool Valida(string idCla, string descripcion, ref string msn)
        {
            try
            {
                Ca_ClasProgramatica clas = new Ca_ClasProgramatica();
                ClasProgramaticaDAL clasModel = new ClasProgramaticaDAL();

                //Si esta repetido el ID
                clas = clasModel.GetByID(x => x.Id_ClasificacionP == idCla);
                if (clas != null)
                {
                    msn = "Ya existe está clave actividad";
                    return false;
                }
                else
                {
                    //return ValidaDesc(descripcion, ref msn);
                    return true;
                }
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaDesc(string descripcion, ref string msn)
        {
            try
            {
                /*ClasProgramaticaDAL actividadModel = new ClasProgramaticaDAL();
                Ca_ClasProgramatica funcion = actividadModel.GetByID(x => x.Descripcion == descripcion);
                if (funcion != null)
                {
                    msn = StringID.Yaexiste + "Funcion";
                    return false;
                }
                msn = "Validación Correcta";*/
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
    }

    public class Ca_ColoniasModel
    {
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "*")]
        public byte Id_Estado { get; set; }
        [Display(Name = "Municipio")]
        [Required(ErrorMessage = "*")]
        public short Id_Municipio { get; set; }
        [Display(Name = "Localidad")]
        [Required(ErrorMessage = "*")]
        public short Id_Localidad { get; set; }
        public short id_colonia { get; set; }
        [Display(Name = "Colonia")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_EstadosModel CA_Estado { get; set; }
        public Ca_MunicipiosModel CA_Municipio { get; set; }
        public Ca_LocalidadesModel CA_Localidad { get; set; }
        public SelectList ListaIdEstado { get; set; }
        public SelectList ListaIdMunicipio { get; set; }
        public SelectList ListaIdLocalidad { get; set; }
        public Ca_ColoniasModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }

        public short Id_Calculate(byte IdEstado, short IdMunicipio, short IdLocalidad)
        {
            ColoniasDAL colonias = new ColoniasDAL();
            try
            {
                short c = colonias.Get(x => x.Id_Estado == IdEstado && x.Id_Municipio == IdMunicipio && x.Id_Localidad == IdLocalidad).Max(x => x.id_colonia);
                c++;
                return c;
            }
            catch
            {
                return 1;
            }
        }
    }

    public class Ca_ConceptoRubroIngresosModel
    {
        public string Rubro { get; set; }
        public string Tipo { get; set; }
        public string Clase { get; set; }
        public string Concepto { get; set; }
        public string Descripcion { get; set; }
        public string Id_Ingreso { get; set; }
        public string Ultimo_nivel { get; set; }
        public int id { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_ConceptoRubroIngresosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_ConceptosIngresosModel
    {
        public byte Rubro { get; set; }
        public byte Tipo { get; set; }
        public byte Clase { get; set; }
        public byte Concepto { get; set; }
        public byte SubCuenta { get; set; }
        [Display(Name = "CRI")]
        public string Id_Concepto { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Último Nivel")]
        public Nullable<bool> UltimoNivel { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_ConceptosIngresosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
        public bool ValidaAdd(byte Rubro, byte Tipo, byte Clase, string Descripcion, bool? Nivel, ref string msn)
        {
            try
            {
                string Id = "";
                Ca_ConceptosIngresos concepto = new Ca_ConceptosIngresos();
                ConceptosIngresosDAL conceptomodel = new ConceptosIngresosDAL();

                //Si esta repetido el ID
                Id = StringID.IdConceptoIngreso(Rubro, Tipo, Clase);
                concepto = conceptomodel.GetByID(x => x.Id_Concepto == Id);
                if (concepto != null)
                {
                    msn = StringID.Yaexiste + "Concepto Ingreso";
                    return false;
                }
                else
                {
                    if (!ValidaIdArea(Rubro, Tipo, Clase, Nivel, ref msn))
                        return false;
                    return ValidaDesc(Descripcion, ref msn);
                }
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }

        public bool ValidaDesc(string Descripcion, ref string msn)
        {
            try
            {
                ConceptosIngresosDAL conceptomodel = new ConceptosIngresosDAL();
                Ca_ConceptosIngresos concepto = conceptomodel.GetByID(x => x.Descripcion == Descripcion);
                if (concepto != null)
                {
                    msn = StringID.Yaexiste + "Concepto Ingreso";
                    return false;
                }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }

        public bool ValidaEdit(byte Rubro, byte Tipo, byte Clase, string Descripcion, bool? Nivel, ref string msn)
        {
            try
            {
                ConceptosIngresosDAL conceptomodel = new ConceptosIngresosDAL();
                Ca_ConceptosIngresos concepto = conceptomodel.GetByID(x => x.Rubro == Rubro && x.Tipo == Tipo && x.Clase == Clase);
                if (concepto.Descripcion != Descripcion)
                    if (!ValidaDesc(Descripcion, ref msn)) return false;
                if (concepto.UltimoNivel != Nivel)
                    return ValidaIdConceptoEdit(Rubro, Tipo, Clase, ref msn);
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }

        public bool ValidaDelete(byte Rubro, byte Tipo, byte Clase, ref string msn)
        {
            try
            {
                ConceptosIngresosDAL conceptomodel = new ConceptosIngresosDAL();
                int c = 0;
                if (Rubro > 0 && Tipo == 0 && Clase == 0)
                {
                    c = conceptomodel.Get(x => x.Rubro == Rubro).Count();
                    if (c > 1)
                    {
                        msn = "No se puede eliminar porque ese Concepto Ingreso tiene registros descendentes";
                        return false;
                    }
                }
                else
                    if (Rubro > 0 && Tipo > 0 && Clase == 0)
                    {
                        c = conceptomodel.Get(x => x.Rubro == Rubro && x.Tipo == Tipo).Count();
                        if (c > 1)
                        {
                            msn = "No se puede eliminar porque ese  Concepto Ingreso tiene registros descendentes";
                            return false;
                        }
                    }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaIdConceptoEdit(byte Rubro, byte Tipo, byte Clase, ref string msn)
        {
            try
            {
                ConceptosIngresosDAL conceptomodel = new ConceptosIngresosDAL();
                int c = 0;
                if (Rubro > 0 && Tipo == 0 && Clase == 0)
                {
                    c = conceptomodel.Get(x => x.Rubro == Rubro).Count();
                    if (c > 1)
                    {
                        msn = StringID.EditarUltimoNivel("descendentes");
                        return false;
                    }
                }
                else
                    if (Rubro > 0 && Tipo > 0 && Clase == 0)
                    {
                        c = conceptomodel.Get(x => x.Rubro == Rubro && x.Tipo == Tipo).Count();
                        if (c > 1)
                        {
                            msn = StringID.EditarUltimoNivel("descendentes");
                            return false;
                        }
                    }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }

        public bool ValidaIdArea(byte Rubro, byte Tipo, byte Clase, bool? Nivel, ref string msn)
        {
            try
            {
                Ca_ConceptosIngresos concepto = new Ca_ConceptosIngresos();
                ConceptosIngresosDAL conceptomodel = new ConceptosIngresosDAL();
                string Id = "";
                if (Rubro > 0 && Tipo == 0 && Clase == 0)
                {
                }
                else
                {
                    if (Rubro > 0 && Tipo > 0 && Clase == 0)
                        Id = StringID.IdConceptoIngreso(Rubro, 0, 0);
                    else
                        if (Rubro > 0 && Tipo > 0 && Clase > 0)
                            Id = StringID.IdConceptoIngreso(Rubro, Tipo, 0);
                    concepto = conceptomodel.GetByID(x => x.Id_Concepto == Id);
                    if (concepto == null)
                    {
                        msn = "El Concepto Ingreso que usted intenta guardar no tiene un registro ascendente, favor de verificarlo";
                        return false;
                    }
                    else
                    {
                        if (concepto.UltimoNivel == true)
                        {
                            msn = "El Concepto Ingreso que usten intenta guardar tiene un registro ascendente de ultimo nivel, favor de verificarlo";
                            return false;
                        }
                    }
                }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
    }

    public class Ca_CuentasModel
    {
        [Display(Name = "Genero")]
        [Required(ErrorMessage = "*")]
        public byte Genero { get; set; }
        [Display(Name = "Grupo")]
        [Required(ErrorMessage = "*")]
        public byte Grupo { get; set; }
        [Display(Name = "Rubro")]
        [Required(ErrorMessage = "*")]
        public byte Rubro { get; set; }
        [Display(Name = "Cuenta")]
        [Required(ErrorMessage = "*")]
        public byte Cuenta { get; set; }
        [Display(Name = "SubCuentaO1")]
        [Required(ErrorMessage = "*")]
        public byte SubCuentaO1 { get; set; }
        [Display(Name = "SubCuentaO2")]
        [Required(ErrorMessage = "*")]
        public int SubCuentaO2 { get; set; }
        [Display(Name = "SubCuentaO3")]
        [Required(ErrorMessage = "*")]
        public short SubCuentaO3 { get; set; }
        [Display(Name = "SubCuentaO4")]
        [Required(ErrorMessage = "*")]
        public int SubCuentaO4 { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        public string Descripcion2 { get; set; }

        public string Descripcion3 { get; set; }

        public string Descripcion4 { get; set; }

        public string Id_Cuenta { get; set; }
        [Display(Name = "Cuenta")]
        public string Id_CuentaFormato { get; set; }
        [Display(Name = "COG")]
        public string Id_ObjetoG { get; set; }
        [Display(Name = "CRI")]
        public string Id_Concepto { get; set; }
        [Display(Name = "Último Nivel")]
        public bool Nivel { get; set; }
        public Nullable<decimal> cargo01 { get; set; }
        public Nullable<decimal> cargo02 { get; set; }
        public Nullable<decimal> cargo03 { get; set; }
        public Nullable<decimal> cargo04 { get; set; }
        public Nullable<decimal> cargo05 { get; set; }
        public Nullable<decimal> cargo06 { get; set; }
        public Nullable<decimal> cargo07 { get; set; }
        public Nullable<decimal> cargo08 { get; set; }
        public Nullable<decimal> cargo09 { get; set; }
        public Nullable<decimal> cargo10 { get; set; }
        public Nullable<decimal> cargo11 { get; set; }
        public Nullable<decimal> cargo12 { get; set; }
        public Nullable<decimal> abono01 { get; set; }
        public Nullable<decimal> abono02 { get; set; }
        public Nullable<decimal> abono03 { get; set; }
        public Nullable<decimal> abono04 { get; set; }
        public Nullable<decimal> abono05 { get; set; }
        public Nullable<decimal> abono06 { get; set; }
        public Nullable<decimal> abono07 { get; set; }
        public Nullable<decimal> abono08 { get; set; }
        public Nullable<decimal> abono09 { get; set; }
        public Nullable<decimal> abono10 { get; set; }
        public Nullable<decimal> abono11 { get; set; }
        public Nullable<decimal> abono12 { get; set; }
        public Nullable<decimal> Cargo_Inicial { get; set; }
        public Nullable<decimal> Abono_Inicial { get; set; }
        public Nullable<byte> Naturaleza { get; set; }
        public string Uso { get; set; }
        public string Observaciones { get; set; }
        [Display(Name = "U. Nivel Armonizado")]
        public bool UNivel_Armonizado { get; set; }
        [Display(Name = "Porcentaje")]
        public Nullable<decimal> Porcentaje_Depreciacion { get; set; }
        [Display(Name = "Tiempo")]
        public Nullable<short> Tiempo_Depreciacion { get; set; }
        public Nullable<byte> Tipo_Tiempo_Depreciacion { get; set; }
        public string Id_Cuenta_Original { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public bool? SelectUltimoNivel { get; set; }
        public bool? SelectNoUltimoNivel { get; set; }
        public bool? BtnAgregar { get; set; }
        public bool? BtnDetalles { get; set; }
        public bool? BtnEditar { get; set; }
        public bool? BtnEliminar { get; set; }


        public Ca_ObjetoGastoModel Ca_ObjetoGastos { get; set; }
        public Ca_ConceptosIngresosModel Ca_ConceptosIngresos { get; set; }
        public SelectList Di_Naturaleza { get; set; }
        //public SelectList Di_TipoTiempoDeapreciacion { get; set; }

        public Ca_CuentasModel()
        {
            Fecha_Act = DateTime.Now;
            Di_Naturaleza = new SelectList(Diccionarios.Naturaleza, "Key", "Value");
            //Di_TipoTiempoDeapreciacion = new SelectList(Diccionarios.Tipo_Tiempo_Depreciacion, "Key", "Value");
        }

        public bool ValidaIdCuenta(byte Genero, byte Grupo, byte Rubro, byte Cuenta, byte SubCuentaO1, int SubCuentaO2, short SubCuentaO3, int SubCuentaO4, ref string msn)
        {
            try
            {
                CuentasDAL cuentasmodel = new CuentasDAL();
                string Id = StringID.IdCuenta(Genero, Grupo, Rubro, Cuenta, SubCuentaO1, SubCuentaO2, SubCuentaO3, SubCuentaO4);
                CA_Cuentas cuenta = cuentasmodel.GetByID(x => x.Id_Cuenta == Id);
                if (cuenta != null)
                {
                    msn = StringID.Yaexista + "Cuenta";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }

        public bool ValidaDescripcion(string Descripcion, ref string msn)
        {
            try
            {
                CuentasDAL cuentasmodel = new CuentasDAL();
                CA_Cuentas cuenta = cuentasmodel.GetByID(x => x.Descripcion == Descripcion);
                if (cuenta != null)
                {
                    msn = StringID.Yaexista + "Cuenta";
                }
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }

        public bool ValidaIdAscendente2(byte Genero, byte Grupo, byte Rubro, byte Cuenta, byte SubCuentaO1, int SubCuentaO2, short SubCuentaO3, int SubCuentaO4, ref string msn)
        {
            try
            {

                CuentasDAL cuentasmodel = new CuentasDAL();
                CA_Cuentas cuentas = null;
                string Id = "";
                Id = StringID.IdCuenta(Genero, Grupo, Rubro, Cuenta, SubCuentaO1, SubCuentaO2, SubCuentaO3, SubCuentaO4);

                if (Genero > 0 && Grupo == 0 && Rubro == 0 && Cuenta == 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                {

                }
                else
                    if (Genero > 0 && Grupo > 0 && Rubro == 0 && Cuenta == 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                        Id = StringID.IdCuenta(Genero, 0, 0, 0, 0, 0, 0, 0);
                    else
                        if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta == 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                            Id = StringID.IdCuenta(Genero, Grupo, 0, 0, 0, 0, 0, 0);
                        else
                            if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                                Id = StringID.IdCuenta(Genero, Grupo, Rubro, 0, 0, 0, 0, 0);
                            else
                                if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 > 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                                    Id = StringID.IdCuenta(Genero, Grupo, Rubro, Cuenta, 0, 0, 0, 0);
                                else
                                    if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 > 0 && SubCuentaO2 > 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                                        Id = StringID.IdCuenta(Genero, Grupo, Rubro, Cuenta, SubCuentaO1, 0, 0, 0);
                                    else
                                        if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 > 0 && SubCuentaO2 > 0 && SubCuentaO3 > 0 && SubCuentaO4 == 0)
                                            Id = StringID.IdCuenta(Genero, Grupo, Rubro, Cuenta, SubCuentaO1, SubCuentaO2, 0, 0);
                                        else
                                            Id = StringID.IdCuenta(Genero, Grupo, Rubro, Cuenta, SubCuentaO1, SubCuentaO2, SubCuentaO3, 0);
                cuentas = cuentasmodel.GetByID(x => x.Id_Cuenta == Id);
                if (cuentas == null)
                {
                    msn = StringID.YaAsendente("La Cuenta", "ascendente");
                    return false;
                }
                else
                    if (cuentas.Nivel == true)
                    {
                        msn = StringID.YaAsendentelvl("La Cuenta", "ascendente");
                        return false;
                    }

                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }

        public bool ValidaDelete(byte Genero, byte Grupo, byte Rubro, byte Cuenta, byte SubCuentaO1, int SubCuentaO2, short SubCuentaO3, int SubCuentaO4, ref string msn)
        {
            try
            {
                if (Genero == 8 || Genero == 4 || Genero == 5) return false;
                CuentasDAL cuentasmodel = new CuentasDAL();
                int c = 0;
                bool Armonizado = false;

                if (Genero > 0 && Grupo == 0 && Rubro == 0 && Cuenta == 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                {
                    c = cuentasmodel.Get(x => x.Genero == Genero && x.Grupo > 0 && x.Rubro == 0).Count();
                    Armonizado = cuentasmodel.GetByID(x => x.Genero == Genero && x.Grupo == 0).UNivel_Armonizado;
                }
                else
                    if (Genero > 0 && Grupo > 0 && Rubro == 0 && Cuenta == 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                    {
                        c = cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro > 0 && x.Cuenta == 0).Count();
                        Armonizado = cuentasmodel.GetByID(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == 0).UNivel_Armonizado;
                    }
                    else
                        if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta == 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                        {
                            c = cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta > 0 && x.SubCuentaO1 == 0).Count();
                            Armonizado = cuentasmodel.GetByID(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == 0).UNivel_Armonizado;
                        }
                        else
                            if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                            {
                                c = cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 > 0 && x.SubCuentaO2 == 0).Count();
                                Armonizado = cuentasmodel.GetByID(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == 0).UNivel_Armonizado;
                            }
                            else
                                if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 > 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                                {
                                    c = cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 > 0 && x.SubCuentaO3 == 0).Count();
                                    Armonizado = cuentasmodel.GetByID(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 == 0).UNivel_Armonizado;
                                }
                                else
                                    if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 > 0 && SubCuentaO2 > 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                                    {
                                        c = cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 == SubCuentaO2 && x.SubCuentaO3 > 0).Count();
                                        Armonizado = cuentasmodel.GetByID(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 == SubCuentaO2 && x.SubCuentaO3 == 0).UNivel_Armonizado;
                                    }
                                    else
                                        if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 > 0 && SubCuentaO2 > 0 && SubCuentaO3 > 0 && SubCuentaO4 == 0)
                                        {
                                            c = cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 == SubCuentaO2 && x.SubCuentaO3 == SubCuentaO3 && x.SubCuentaO4 > 0).Count();
                                            Armonizado = cuentasmodel.GetByID(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 == SubCuentaO2 && x.SubCuentaO3 == SubCuentaO3 && x.SubCuentaO4 == 0).UNivel_Armonizado;
                                        }
                                        else
                                            return true;

                if (Armonizado == true)
                {
                    msn = "No se puede eliminar porque esa cuenta es del CONAC";
                    return false;
                }
                if (c > 0)
                {
                    msn = "No se puede eliminar porque esa cuenta tiene registros descendentes";
                    return false;
                }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }

            try
            {
                CuentasDAL cuentasmodel = new CuentasDAL();
                int c = 0;
                c = cuentasmodel.Get(x =>
                    (Genero > 0 ? x.Genero == Genero : x.Genero != null) &&
                    (Grupo > 0 ? x.Grupo == Grupo : x.Grupo != null) &&
                    (Rubro > 0 ? x.Rubro == Rubro : x.Rubro != null) &&
                    (Cuenta > 0 ? x.Cuenta == Cuenta : x.Cuenta != null) &&
                    (SubCuentaO1 > 0 ? x.SubCuentaO1 == SubCuentaO1 : x.SubCuentaO1 != null) &&
                    (SubCuentaO2 > 0 ? x.SubCuentaO2 == SubCuentaO2 : x.SubCuentaO2 != null) &&
                    (SubCuentaO3 > 0 ? x.SubCuentaO3 == SubCuentaO3 : x.SubCuentaO3 != null) &&
                    (SubCuentaO4 > 0 ? x.SubCuentaO4 == SubCuentaO4 : x.SubCuentaO4 != null)
                    ).Count();
                if (c > 1)
                {
                    msn = "No se puede eliminar porque esa Cuenta tiene registros descendentes";
                    return false;
                }

                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }

        public bool ValidaNoHijos(byte Genero, byte Grupo, byte Rubro, byte Cuenta, byte SubCuentaO1, int SubCuentaO2, short SubCuentaO3, int SubCuentaO4)
        {
            int NoGrupo = 10, NoRubro = 10, NoCuenta = 10, NoSubCeuntaO1 = 10, NoSubCeuntaO2 = 100000, NoSubCeuntaO3 = 10000, NoSubCeuntaO4 = 1000000;

            CuentasDAL cuentasmodel = new CuentasDAL();

            if (Genero > 0 && Grupo == 0 && Rubro == 0 && Cuenta == 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0) //Lvl 1
                if (cuentasmodel.Get(x => x.Genero == Genero && x.Grupo > 0 && x.Rubro == 0).Count() < NoGrupo &&
                    cuentasmodel.Get(x => x.Genero == Genero && x.Grupo > 0 && x.Rubro == 0).Where(x => x.UNivel_Armonizado == true).Count() == 0) return true;
                else return false;
            else
                if (Genero > 0 && Grupo > 0 && Rubro == 0 && Cuenta == 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                    if (cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro > 0 && x.Cuenta == 0).Count() < NoRubro &&
                        cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro > 0 && x.Cuenta == 0).Where(x => x.UNivel_Armonizado == true).Count() == 0) return true;
                    else return false;

                else
                    if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta == 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                        if (cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta > 0 && x.SubCuentaO1 == 0).Count() < NoCuenta &&
                            cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta > 0 && x.SubCuentaO1 == 0).Where(x => x.UNivel_Armonizado == true).Count() == 0) return true;
                        else return false;

                    else
                        if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                            if (cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 > 0 && x.SubCuentaO2 == 0).Count() < NoSubCeuntaO1 &&
                                cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 > 0 && x.SubCuentaO2 == 0).Where(x => x.UNivel_Armonizado == true).Count() == 0) return true;
                            else return false;

                        else
                            if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 > 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                                if (cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 > 0 && x.SubCuentaO3 == 0).Count() < NoSubCeuntaO2 &&
                                    cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 > 0 && x.SubCuentaO3 == 0).Where(x => x.UNivel_Armonizado == true).Count() == 0) return true;
                                else return false;

                            else
                                if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 > 0 && SubCuentaO2 > 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                                    if (cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 == SubCuentaO2 && x.SubCuentaO3 > 0 && x.SubCuentaO4 == 0).Count() < NoSubCeuntaO3 &&
                                        cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 == SubCuentaO2 && x.SubCuentaO3 > 0 && x.SubCuentaO4 == 0).Where(x => x.UNivel_Armonizado == true).Count() == 0) return true;
                                    else return false;

                                else
                                    if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 > 0 && SubCuentaO2 > 0 && SubCuentaO3 > 0 && SubCuentaO4 == 0)
                                        if (cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 == SubCuentaO2 && x.SubCuentaO3 == SubCuentaO3 && x.SubCuentaO4 > 0).Count() < NoSubCeuntaO4 &&
                                            cuentasmodel.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 == SubCuentaO2 && x.SubCuentaO3 == SubCuentaO3 && x.SubCuentaO4 > 0).Where(x => x.UNivel_Armonizado == true).Count() == 0) return true;
                                        else return false;
            return false;
        }

        public void Consecutivo(byte Genero, byte Grupo, byte Rubro, byte Cuenta, byte SubCuentaO1, int SubCuentaO2, short SubCuentaO3, int SubCuentaO4,
            ref byte GeneroR, ref byte GrupoR, ref byte RubroR, ref byte CuentaR, ref byte SubCuentaO1R, ref int SubCuentaO2R, ref short SubCuentaO3R, ref int SubCuentaO4R)
        {
            CuentasDAL cuentas = new CuentasDAL();
            int c = 0;
            GeneroR = Genero;
            GrupoR = Grupo;
            RubroR = Rubro;
            CuentaR = Cuenta;
            SubCuentaO1R = SubCuentaO1;
            SubCuentaO2R = SubCuentaO2;
            SubCuentaO3R = SubCuentaO3;
            SubCuentaO4R = SubCuentaO4;
            if (Genero > 0 && Grupo == 0 && Rubro == 0 && Cuenta == 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0) //Lvl 1
                if (cuentas.Get(x => x.Genero == Genero && x.Grupo > 0 && x.Rubro == 0).GroupBy(x => x.Grupo).Count() > 0)
                {
                    c = cuentas.Get(x => x.Genero == Genero && x.Grupo > 0 && x.Rubro == 0).Max(x => x.Grupo);
                    GrupoR = (byte)c++;
                }
                else
                    GrupoR = 1;
            else
                if (Genero > 0 && Grupo > 0 && Rubro == 0 && Cuenta == 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                    if (cuentas.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro > 0 && x.Cuenta == 0).GroupBy(x => x.Rubro).Count() > 0)
                    {
                        c = cuentas.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro > 0 && x.Cuenta == 0).Max(x => x.Rubro);
                        c++;
                        RubroR = (byte)c;
                    }
                    else
                        RubroR = 1;
                else
                    if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta == 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                        if (cuentas.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta > 0 && x.SubCuentaO1 == 0).GroupBy(x => x.Cuenta).Count() > 0)
                        {
                            c = cuentas.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta > 0 && x.SubCuentaO1 == 0).Max(x => x.Cuenta);
                            c++;
                            CuentaR = (byte)c;
                        }
                        else
                            CuentaR = 1;
                    else
                        if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                            if (cuentas.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 > 0 && x.SubCuentaO2 == 0).GroupBy(x => x.SubCuentaO1).Count() > 0)
                            {
                                c = cuentas.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 > 0 && x.SubCuentaO2 == 0).Max(x => x.SubCuentaO1);
                                c++;
                                SubCuentaO1R = (byte)c;
                            }
                            else
                                SubCuentaO1R = 1;
                        else
                            if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 > 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                                if (cuentas.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 > 0 && x.SubCuentaO3 == 0).GroupBy(x => x.SubCuentaO2).Count() > 0)
                                {
                                    c = cuentas.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 > 0 && x.SubCuentaO3 == 0).Max(x => x.SubCuentaO2);
                                    c++;
                                    SubCuentaO2R = c;
                                }
                                else
                                    SubCuentaO2R = 1;
                            else
                                if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 > 0 && SubCuentaO2 > 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                                    if (cuentas.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 == SubCuentaO2 && x.SubCuentaO3 > 0 && x.SubCuentaO4 == 0).GroupBy(x => x.SubCuentaO3).Count() > 0)
                                    {
                                        c = cuentas.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 == SubCuentaO2 && x.SubCuentaO3 > 0 && x.SubCuentaO4 == 0).Max(x => x.SubCuentaO3);
                                        c++;
                                        SubCuentaO3R = (short)c;
                                    }
                                    else
                                        SubCuentaO3R = 1;
                                else
                                    if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 > 0 && SubCuentaO2 > 0 && SubCuentaO3 > 0 && SubCuentaO4 == 0)
                                        if (cuentas.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 == SubCuentaO2 && x.SubCuentaO3 == SubCuentaO3 && x.SubCuentaO4 > 0).GroupBy(x => x.SubCuentaO4).Count() > 0)
                                        {
                                            c = cuentas.Get(x => x.Genero == Genero && x.Grupo == Grupo && x.Rubro == Rubro && x.Cuenta == Cuenta && x.SubCuentaO1 == SubCuentaO1 && x.SubCuentaO2 == SubCuentaO2 && x.SubCuentaO3 == SubCuentaO3 && x.SubCuentaO4 > 0).Max(x => x.SubCuentaO4);
                                            c++;
                                            SubCuentaO4R = c;
                                        }
                                        else
                                            SubCuentaO4R = 1;
        }

        public byte? ValidaNaturaleza(byte Genero, byte Grupo, byte Rubro, byte Cuenta, byte SubCuentaO1, int SubCuentaO2, short SubCuentaO3, int SubCuentaO4)
        {
            CuentasDAL cuentasmodel = new CuentasDAL();
            CA_Cuentas cuentas = null;
            string Id = "";
            Id = StringID.IdCuenta(Genero, Grupo, Rubro, Cuenta, SubCuentaO1, SubCuentaO2, SubCuentaO3, SubCuentaO4);

            if (Genero > 0 && Grupo == 0 && Rubro == 0 && Cuenta == 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
            {

            }
            else
                if (Genero > 0 && Grupo > 0 && Rubro == 0 && Cuenta == 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                    Id = StringID.IdCuenta(Genero, 0, 0, 0, 0, 0, 0, 0);
                else
                    if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta == 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                        Id = StringID.IdCuenta(Genero, Grupo, 0, 0, 0, 0, 0, 0);
                    else
                        if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 == 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                            Id = StringID.IdCuenta(Genero, Grupo, Rubro, 0, 0, 0, 0, 0);
                        else
                            if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 > 0 && SubCuentaO2 == 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                                Id = StringID.IdCuenta(Genero, Grupo, Rubro, Cuenta, 0, 0, 0, 0);
                            else
                                if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 > 0 && SubCuentaO2 > 0 && SubCuentaO3 == 0 && SubCuentaO4 == 0)
                                    Id = StringID.IdCuenta(Genero, Grupo, Rubro, Cuenta, SubCuentaO1, 0, 0, 0);
                                else
                                    if (Genero > 0 && Grupo > 0 && Rubro > 0 && Cuenta > 0 && SubCuentaO1 > 0 && SubCuentaO2 > 0 && SubCuentaO3 > 0 && SubCuentaO4 == 0)
                                        Id = StringID.IdCuenta(Genero, Grupo, Rubro, Cuenta, SubCuentaO1, SubCuentaO2, 0, 0);
                                    else
                                        Id = StringID.IdCuenta(Genero, Grupo, Rubro, Cuenta, SubCuentaO1, SubCuentaO2, SubCuentaO3, 0);
            cuentas = cuentasmodel.GetByID(x => x.Id_Cuenta == Id);
            return cuentas.Naturaleza;
        }
    }

    public class Ca_CuentasBancariasModel
    {
        public short Id_CtaBancaria { get; set; }
        [Display(Name = "Banco")]
        [Required(ErrorMessage = "*")]
        public Nullable<short> Id_Banco { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "No Cuenta")]
        public string NoCuenta { get; set; }
        [Required(ErrorMessage = "*")]
        public string Sucursal { get; set; }
        [Display(Name = "Tipo Foliador")]
        public Nullable<byte> TipoFoliador { get; set; }
        [Display(Name = "Cuenta")]
        [Required(ErrorMessage = "*")]
        public string Id_Cuenta { get; set; }
        [Display(Name = "No cheque inicial")]
        [Required(ErrorMessage = "*")]
        public Nullable<int> NoChequeIni { get; set; }
        [Display(Name = "No cheque final")]
        [Required(ErrorMessage = "*")]
        public Nullable<int> NoChequeFin { get; set; }
        public Nullable<bool> ChequePoliza { get; set; }
        public Nullable<decimal> SaldoInicial { get; set; }
        public Nullable<decimal> Depositos { get; set; }
        public Nullable<decimal> Retiros { get; set; }
        public Nullable<decimal> SaldoFinal { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public Nullable<short> Id_Banco_Anterior { get; set; }
        public string Id_Cuenta_Anterior { get; set; }
        [Display(Name = "No Sucursal")]
        [Required(ErrorMessage = "*")]
        [StringLength(4, ErrorMessage = "Máximo 4 caracteres")]
        public string No_Sucursal { get; set; }
        public Nullable<bool> Exportable { get; set; }
        public string CuentaBancaria { get; set; }
        [Display(Name = "Fuente Financiamiento")]
        [Required(ErrorMessage = "*")]

        public string Id_Fuente { get; set; }

        public virtual Ca_BancosModel Ca_Bancos { get; set; }
        public virtual Ca_CuentasModel Ca_Cuentas { get; set; }
        public virtual Ca_FuentesFinModel Ca_Fuentes { get; set; }

        public SelectList ListaFuentes { get; set; }

        public Ca_CuentasBancariasModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }

        public Ca_CuentasBancariasModel(short IdCtaBancaria)
        {

        }
    }

    public class Ca_EstadosModel
    {
        public byte Id_Estado { get; set; }
        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_EstadosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_EstatusRequisicionesModel
    {
        public short Id_Estatus { get; set; }
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_EstatusRequisicionesModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_FuentesFinModel
    {
        public string Id_Fuente { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public Nullable<bool> UltimoNivel { get; set; }

        public Ca_FuentesFinModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_FuncionesModel
    {
        [Required]
        public byte Finalidad { get; set; }
        [Required]
        [Display(Name = "Función")]
        public byte Funcion { get; set; }
        [Required]
        public byte Subfuncion { get; set; }
        [Display(Name = "Número función")]
        public string Id_Funcion { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_FuncionesModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
        public bool ValidaAdd(byte finalidad, byte funcion, byte subfuncion, string Descripcion, ref string msn)
        {
            try
            {
                string Id = "";
                Ca_Funciones Funcion = new Ca_Funciones();
                FuncionDAL funcionModel = new FuncionDAL();

                //Si esta repetido el ID
                Id = StringID.IdFuncion(finalidad, funcion, subfuncion);
                Funcion = funcionModel.GetByID(x => x.Id_Funcion == Id);
                if (Funcion != null)
                {
                    msn = "Ya existe está función";
                    return false;
                }
                else
                {
                    if (!ValidaIdFuncion(finalidad, funcion, subfuncion, ref msn))
                        return false;
                    return ValidaDesc(Descripcion, ref msn);
                }
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }

        public bool ValidaDesc(string Descripcion, ref string msn)
        {
            try
            {
                FuncionDAL funcionModel = new FuncionDAL();
                Ca_Funciones funcion = funcionModel.GetByID(x => x.Descripcion == Descripcion);
                if (funcion != null)
                {
                    msn = StringID.Yaexiste + "Funcion";
                    return false;
                }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }

        public bool ValidaEdit(byte finalidad, byte funcion, byte subfuncion, string Descripcion, ref string msn)
        {
            try
            {
                FuncionDAL funcionModel = new FuncionDAL();
                Ca_Funciones Funcion = funcionModel.GetByID(x => x.Finalidad == finalidad && x.Funcion == funcion && x.Subfuncion == subfuncion);
                if (Funcion.Descripcion != Descripcion)
                    if (!ValidaDesc(Descripcion, ref msn)) return false;
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }

        public bool ValidaDelete(byte finalidad, byte funcion, byte subfuncion, ref string msn)
        {
            try
            {
                FuncionDAL funcionesmodel = new FuncionDAL();
                int c = 0;
                if (finalidad > 0 && funcion == 0 && subfuncion == 0)
                {
                    c = funcionesmodel.Get(x => x.Finalidad == finalidad).Count();
                    if (c > 1)
                    {
                        msn = "No se puede eliminar porque la Función tiene registros ascendentes";
                        return false;
                    }
                }
                else
                    if (finalidad > 0 && funcion > 0 && subfuncion == 0)
                    {
                        c = funcionesmodel.Get(x => x.Finalidad == finalidad && x.Funcion == funcion).Count();
                        if (c > 1)
                        {
                            msn = "No se puede eliminar porque la Función tiene registros descendentes";
                            return false;
                        }
                    }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }

        public bool ValidaIdFuncion(byte finalidad, byte funcion, byte subfuncion, ref string msn)
        {
            try
            {
                Ca_Funciones Funcion = new Ca_Funciones();
                FuncionDAL funcionModel = new FuncionDAL();
                string Id = "";
                if (finalidad > 0 && funcion == 0 && subfuncion == 0)
                {
                }
                else
                {
                    if (finalidad > 0 && funcion > 0 && subfuncion == 0)
                        Id = StringID.IdFuncion(finalidad, 0, 0);
                    else
                        if (finalidad > 0 && funcion > 0 && subfuncion > 0)
                            Id = StringID.IdFuncion(finalidad, funcion, 0);
                    Funcion = funcionModel.GetByID(x => x.Id_Funcion == Id);
                    if (Funcion == null)
                    {
                        msn = StringID.YaAsendente("La Función", "ascendente");
                        return false;
                    }
                }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
    }

    public class Ca_Giros_ComercialesModel
    {
        public short Id_GiroComercial { get; set; }
        public string Descripcion { get; set; }
        public Nullable<byte> Id_Tipo { get; set; }
        public Nullable<byte> Inventariable { get; set; }
        public bool Proponer_Area { get; set; }
        public Nullable<byte> Id_Tipo_Bien { get; set; }
        public Nullable<byte> Id_Tipo_Consumo { get; set; }
        public Nullable<short> Rsist { get; set; }
        public Nullable<short> Rmod { get; set; }
        public Nullable<short> Rno { get; set; }
        public Nullable<short> Vigencia_eCompras { get; set; }
        public bool Activo { get; set; }
        public bool Requerir_Descripcion { get; set; }
        public bool Proponer_Precio_Cotizaciones { get; set; }
        public bool Ingresar_Almacen { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_Giros_ComercialesModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_Impuestos_DeduccionModel
    {
        [Display(Name = "Tipo")]
        [Required, Range(1, 2, ErrorMessage = "El Campo Tipo es obligatorio.")]
        public short Id_Tipo_ImpDed { get; set; }
        public short Id_ImpDed { get; set; }
        [Display(Name = "Descripción")]
        [Required]
        public string Descripcion { get; set; }
        public string Id_ImpDed2 { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public SelectList ListaTipos { get; set; }
        public Ca_Impuestos_DeduccionModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
            this.ListaTipos = new SelectList(Diccionarios.ListaTiposImpuestoDed, "Key", "Value");
        }
    }

    public class Ca_LocalidadesModel
    {
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "*")]
        public byte Id_Estado { get; set; }
        [Display(Name = "Municipio")]
        [Required(ErrorMessage = "*")]
        public short Id_Municipio { get; set; }
        public short Id_Localidad { get; set; }
        [Display(Name = "Localidad")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_EstadosModel CA_Estado { get; set; }
        public Ca_MunicipiosModel CA_Municipio { get; set; }
        public SelectList ListaIdEstado { get; set; }
        public SelectList ListaIdMunicipio { get; set; }

        public Ca_LocalidadesModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_MunicipiosModel
    {
        [Display(Name = "Estado:")]
        [Required(ErrorMessage = "*")]
        public byte Id_Estado { get; set; }
        [Display(Name = "Municipio:")]
        public short Id_Municipio { get; set; }
        [Display(Name = "Descripción:")]
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        [Display(Name = "Estado")]
        public Ca_EstadosModel CA_Estado { get; set; }
        public SelectList Lista_Estados { get; set; }
        public Ca_MunicipiosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_ObjetoGastoModel
    {
        public byte Capitulo { get; set; }
        public byte Concepto { get; set; }
        public byte PartidaGen { get; set; }
        public byte PartidaEsp { get; set; }
        public string Id_ObjetoG { get; set; }
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        public Nullable<bool> UltimoNivel { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_ObjetoGastoModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_PaisesModel
    {
        [Display(Name = "Pais:")]
        public byte Id_Pais { get; set; }
        [Display(Name = "Descripción:"), MaxLength(50, ErrorMessage = "Máximo 50 caracteres"), Required]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_PaisesModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_ParametrosModel
    {
        public byte IdParametro { get; set; }
        public string Nombre { get; set; }
        [Display(Name = "Descripción:")]
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "*")]
        public string Valor { get; set; }
        public Nullable<short> uAct { get; set; }
        public Nullable<System.DateTime> fAct { get; set; }

        public Ca_ParametrosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            uAct = (Int16)appUsuario.IdUsuario;
            fAct = DateTime.Now;
        }
    }

    public class Ca_Percep_DeducModel
    {
        public string Tipo_PD { get; set; }
        public string Clave_PD { get; set; }
        public string Descripcion { get; set; }
        public string Percep_Deduc { get; set; }
        public string Id_ObjetoG { get; set; }
        public string Id_Cuenta { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_Percep_DeducModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_ProgramasModel
    {
        [Required(ErrorMessage = "*")]
        [StringLength(2, ErrorMessage = "Máximo dos caracteres.", MinimumLength = 1)]
        [Display(Name = "Programa")]
        public string Id_Programa { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_ProgramasModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
        public bool Valida(string idCla, string descripcion, ref string msn)
        {
            try
            {
                CA_Programas clas = new CA_Programas();
                ProgramaDAL clasModel = new ProgramaDAL();

                //Si esta repetido el ID
                clas = clasModel.GetByID(x => x.Id_Programa == idCla);
                if (clas != null)
                {
                    msn = "Ya existe está clave actividad";
                    return false;
                }
                else
                {
                    //return ValidaDesc(descripcion, ref msn);
                    return true;
                }
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaDesc(string descripcion, ref string msn)
        {
            try
            {
                /*ClasProgramaticaDAL actividadModel = new ClasProgramaticaDAL();
                Ca_ClasProgramatica funcion = actividadModel.GetByID(x => x.Descripcion == descripcion);
                if (funcion != null)
                {
                    msn = StringID.Yaexiste + "Funcion";
                    return false;
                }
                msn = "Validación Correcta";*/
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
    }

    public class Ca_ProyectoModel
    {
        [Required(ErrorMessage = "*")]
        [StringLength(2, ErrorMessage = "Máximo dos caracteres.", MinimumLength = 1)]
        [Display(Name = "Proyecto")]
        public string Id_Proceso { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_ProyectoModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
        public bool Valida(string idCla, string descripcion, ref string msn)
        {
            try
            {
                Ca_Proyecto clas = new Ca_Proyecto();
                ProcesoDAL clasModel = new ProcesoDAL();

                //Si esta repetido el ID
                clas = clasModel.GetByID(x => x.Id_Proceso == idCla);
                if (clas != null)
                {
                    msn = "Ya existe está clave de proyecto";
                    return false;
                }
                else
                {
                    //return ValidaDesc(descripcion, ref msn);
                    return true;
                }
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaDesc(string descripcion, ref string msn)
        {
            try
            {
                /*ClasProgramaticaDAL actividadModel = new ClasProgramaticaDAL();
                Ca_ClasProgramatica funcion = actividadModel.GetByID(x => x.Descripcion == descripcion);
                if (funcion != null)
                {
                    msn = StringID.Yaexiste + "Funcion";
                    return false;
                }
                msn = "Validación Correcta";*/
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
    }

    public class Ca_SectoresModel
    {
        [Display(Name = "Sector:")]
        public short Id_Sector { get; set; }
        [Display(Name = "Descripción:"), MaxLength(50, ErrorMessage = "Máximo 50 caracteres"), Required]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_SectoresModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_TipoBeneficiariosModel
    {
        [Display(Name = "Tipo de beneficiario")]
        public byte Id_TipoBene { get; set; }
        [Display(Name = "Descripción:"), MaxLength(30, ErrorMessage = "Máximo 30 caracteres"), Required]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_TipoBeneficiariosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
        /// <summary>
        /// Valida si la descripción cambió y en tal caso si no se ingresó una descripción repetida
        /// </summary>
        /// <param name="Id_TipoBeneficiario"></param>
        /// <param name="Descripcion"></param>
        /// <param name="msn"></param>
        /// <returns>true si procede la edición y false en caso contrario</returns>
        public bool ValidaEdit(byte Id_TipoBeneficiario, string Descripcion, ref string msn)
        {
            try
            {
                TipoBeneficiariosDAL TipoBeneficiariomodel = new TipoBeneficiariosDAL();
                Ca_TipoBeneficiarios TipoBeneficiario = TipoBeneficiariomodel.GetByID(x => x.Id_TipoBene == Id_TipoBeneficiario);
                if (TipoBeneficiario.Descripcion != Descripcion)
                    if (!ValidaDesc(Descripcion, ref msn)) return false;
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// Valida que no se ingrese una descripción repetida
        /// </summary>
        /// <param name="Descripcion"></param>
        /// <param name="msn"></param>
        /// <returns>true si la descripción ingresada es válida y false si se detecta una repetición</returns>
        public bool ValidaDesc(string Descripcion, ref string msn)
        {
            try
            {
                TipoBeneficiariosDAL TipoBeneficiariomodel = new TipoBeneficiariosDAL();
                Ca_TipoBeneficiarios TipoBeneficiario = TipoBeneficiariomodel.GetByID(x => x.Descripcion == Descripcion);
                if (TipoBeneficiario != null)
                {
                    msn = "Ya existe éste tipo de compromiso";
                    return false;
                }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
    }

    public class Ca_TipoCompromisosModel
    {
        [Display(Name = "Tipo de compromiso")]
        public short Id_TipoCompromiso { get; set; }
        [Display(Name = "Descripción"), MaxLength(50, ErrorMessage = "No debe exceder 50 caracteres"), Required]
        public string Descripcion { get; set; }
        [Display(Name = "Último compromiso")]
        public Nullable<int> UltimoComp { get; set; }
        [Display(Name = "Imprimir fecha")]
        public Nullable<bool> ImprimirFecha { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        [Display(Name = "Folio")]
        public Nullable<int> Folio { get; set; }
        [Display(Name = "Sin fecha")]
        public Nullable<bool> Sin_Fecha { get; set; }
        [Display(Name = "Número de día")]
        public Nullable<bool> Num_Dia { get; set; }
        [Display(Name = "Cantidad de días")]
        //[Required, Range(1, 31, ErrorMessage = "*")]
        public Nullable<int> Num_Dias { get; set; }
        [Display(Name = "Número de semana")]
        public Nullable<bool> Num_Semana { get; set; }
        [Display(Name = "Cantidad de semanas")]
        //[Required, Range(1, 31, ErrorMessage = "*")]
        public Nullable<int> Num_Semanas { get; set; }
        [Display(Name = "Día de la semana")]
        //[Required, Range(1, 31, ErrorMessage = "*")]
        public Nullable<int> Dia_Semana { get; set; }
        [Display(Name = "Pagar antes de la quincena")]
        public Nullable<bool> A_Qna { get; set; }
        [Display(Name = "Días antes de la quincena")]
        //[Required, Range(1, 31, ErrorMessage = "*")]
        public Nullable<int> Dia_Antes_Q { get; set; }
        [Display(Name = "Días despues de la quincena")]
        //[Required, Range(1, 31, ErrorMessage = "*")]
        public Nullable<int> Dia_Despues_Q { get; set; }
        [Display(Name = "Día del mes")]
        public Nullable<bool> Dia_Mes { get; set; }
        [Display(Name = "Día 1")]
        //[Required, Range(1, 31, ErrorMessage = "*")]
        public Nullable<int> Dia1 { get; set; }
        public Nullable<bool> Antes { get; set; }
        [Display(Name = "Día 2")]
        //[Required, Range(1, 31, ErrorMessage = "*")]
        public Nullable<int> Dia2 { get; set; }
        public Nullable<bool> Despues { get; set; }
        [Display(Name = "Fecha calculada")]
        public string Fecha_Calculada { get; set; }
        [Display(Name = "Pagarse")]
        public string Pagarse { get; set; }
        public SelectList ListaDias { get; set; }
        public SelectList ListaSemanas { get; set; }
        public Ca_TipoCompromisosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
            this.ListaDias = new SelectList(Diccionarios.DiasSemana, "Key", "Value");
            this.ListaSemanas = new SelectList(Diccionarios.ListaSemanas, "Key", "Value");
        }
        /// <summary>
        /// Valida si la descripción cambió y en tal caso si no se ingresó una descripción repetida
        /// </summary>
        /// <param name="Id_TipoCompromiso"></param>
        /// <param name="Descripcion"></param>
        /// <param name="msn"></param>
        /// <returns>true si procede la edición y false en caso contrario</returns>
        public bool ValidaEdit(short Id_TipoCompromiso, string Descripcion, ref string msn)
        {
            try
            {
                TipoCompromisosDAL TipoCompromisomodel = new TipoCompromisosDAL();
                Ca_TipoCompromisos TipoCompromiso = TipoCompromisomodel.GetByID(x => x.Id_TipoCompromiso == Id_TipoCompromiso);
                if (TipoCompromiso.Descripcion != Descripcion)
                    if (!ValidaDesc(Descripcion, ref msn)) return false;
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// Valida que no se ingrese una descripción repetida
        /// </summary>
        /// <param name="Descripcion"></param>
        /// <param name="msn"></param>
        /// <returns>true si la descripción ingresada es válida y false si se detecta una repetición</returns>
        public bool ValidaDesc(string Descripcion, ref string msn)
        {
            try
            {
                TipoCompromisosDAL TipoCompromisomodel = new TipoCompromisosDAL();
                Ca_TipoCompromisos TipoCompromiso = TipoCompromisomodel.GetByID(x => x.Descripcion == Descripcion);
                if (TipoCompromiso != null)
                {
                    msn = "Ya existe éste tipo de compromiso";
                    return false;
                }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
    }

    public class Ca_TipoContrarecibosModel
    {
        [Display(Name = "Tipo de contrarecibo:")]
        public byte Id_TipoCR { get; set; }
        [Display(Name = "Descripción:"), MaxLength(20, ErrorMessage = "Máximo 20 caracteres"), Required]
        public string Descripcion { get; set; }
        [Display(Name = "Último contrarecibo:")]
        public Nullable<int> UltimoCR { get; set; }
        [Display(Name = "Facturas diversas:")]
        public Nullable<bool> Facturas_Diversas { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_TipoContrarecibosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
        /// <summary>
        /// Valida si la descripción cambió y en tal caso si no se ingresó una descripción repetida
        /// </summary>
        /// <param name="Id_TipoContrarecibo"></param>
        /// <param name="Descripcion"></param>
        /// <param name="msn"></param>
        /// <returns>true si procede la edición y false en caso contrario</returns>
        public bool ValidaEdit(byte Id_TipoContrarecibo, string Descripcion, ref string msn)
        {
            try
            {
                TipoContrarecibosDAL TipoContrarecibomodel = new TipoContrarecibosDAL();
                Ca_TipoContrarecibos TipoContrarecibo = TipoContrarecibomodel.GetByID(x => x.Id_TipoCR == Id_TipoContrarecibo);
                if (TipoContrarecibo.Descripcion != Descripcion)
                    if (!ValidaDesc(Descripcion, ref msn)) return false;
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// Valida que no se ingrese una descripción repetida
        /// </summary>
        /// <param name="Descripcion"></param>
        /// <param name="msn"></param>
        /// <returns>true si la descripción ingresada es válida y false si se detecta una repetición</returns>
        public bool ValidaDesc(string Descripcion, ref string msn)
        {
            try
            {
                TipoContrarecibosDAL TipoContrarecibomodel = new TipoContrarecibosDAL();
                Ca_TipoContrarecibos TipoContrarecibo = TipoContrarecibomodel.GetByID(x => x.Descripcion == Descripcion);
                if (TipoContrarecibo != null)
                {
                    msn = "Ya existe éste tipo de contrarecibo";
                    return false;
                }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
    }

    public class Ca_TipoDoctosModel
    {
        [Display(Name = "Tipo de documento")]
        public byte Id_Tipodocto { get; set; }
        [Display(Name = "Descripción"), Required, MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_TipoDoctosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
        /// <summary>
        /// Valida si la descripción cambió y en tal caso si no se ingresó una descripción repetida
        /// </summary>
        /// <param name="Id_TipoDocto"></param>
        /// <param name="Descripcion"></param>
        /// <param name="msn"></param>
        /// <returns>true si procede la edición y false en caso contrario</returns>
        public bool ValidaEdit(byte Id_Tipodocto, string Descripcion, ref string msn)
        {
            try
            {
                TipoDoctosDAL TipoDoctomodel = new TipoDoctosDAL();
                Ca_TipoDoctos TipoDocto = TipoDoctomodel.GetByID(x => x.Id_Tipodocto == Id_Tipodocto);
                if (TipoDocto.Descripcion != Descripcion)
                    if (!ValidaDesc(Descripcion, ref msn)) return false;
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// Valida que no se ingrese una descripción repetida
        /// </summary>
        /// <param name="Descripcion"></param>
        /// <param name="msn"></param>
        /// <returns>true si la descripción ingresada es válida y false si se detecta una repetición</returns>
        public bool ValidaDesc(string Descripcion, ref string msn)
        {
            try
            {
                TipoDoctosDAL TipoDoctomodel = new TipoDoctosDAL();
                Ca_TipoDoctos TipoDocto = TipoDoctomodel.GetByID(x => x.Descripcion == Descripcion);
                if (TipoDocto != null)
                {
                    msn = "Ya existe éste tipo de documento";
                    return false;
                }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
    }

    public class Ca_TipoFormatosChequesModel
    {
        public const byte POLIZA_CHEQUE = 1;
        public const byte CONTINUO = 2;
        public const byte CHEQUE_POLIZA = 3;
        public const byte CHEQUE_POLIZA_DIF = 4;

        public byte Id_Formato { get; set; }
        public string Descripcion { get; set; }
        public Nullable<short> No_Filas_PD { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_TipoFormatosChequesModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_TipoGastosModel
    {
        [Display(Name = "Tipo de gasto")]
        public string Id_TipoGasto { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_TipoGastosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_TipoImpuestosModel
    {
        [Display(Name = "Tipo Impuesto")]
        public byte TipoImpuesto { get; set; }
        [Display(Name = "Folio")]
        public byte FolioImpuesto { get; set; }
        [Display(Name = "Número de Tipo Impuesto")]
        public string Id_TipoImpuesto { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_TipoImpuestosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_TipoIngresosModel
    {
        public string Id_TipoIngreso { get; set; }
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_TipoIngresosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_TipoMetaModel
    {
        [Required(ErrorMessage = "*")]
        [StringLength(1, ErrorMessage = "Máximo un caracter.", MinimumLength = 1)]
        [Display(Name = "Tipo meta")]
        public string Id_TipoMeta { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_TipoMetaModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
        public bool Valida(string idCla, string descripcion, ref string msn)
        {
            try
            {
                Ca_TipoMeta clas = new Ca_TipoMeta();
                TipoMetaDAL clasModel = new TipoMetaDAL();

                //Si esta repetido el ID
                clas = clasModel.GetByID(x => x.Id_TipoMeta == idCla);
                if (clas != null)
                {
                    msn = "Ya existe está clave de tipo de meta";
                    return false;
                }
                else
                {
                    //return ValidaDesc(descripcion, ref msn);
                    return true;
                }
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaDesc(string descripcion, ref string msn)
        {
            try
            {
                /*ClasProgramaticaDAL actividadModel = new ClasProgramaticaDAL();
                Ca_ClasProgramatica funcion = actividadModel.GetByID(x => x.Descripcion == descripcion);
                if (funcion != null)
                {
                    msn = StringID.Yaexiste + "Funcion";
                    return false;
                }
                msn = "Validación Correcta";*/
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
    }

    public class Ca_TipoMovBancariosModel
    {
        public byte Id_TipoMovB { get; set; }
        public byte Id_FolioMovB { get; set; }
        [Display(Name = "Tipo de Movimiento")]
        public string Id_MovBancario { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Movimiento")]
        public Nullable<byte> TipoMov { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_TipoMovBancariosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_TipoPagosModel
    {
        [Display(Name = "Tipo de pago")]
        public byte Id_TipoPago { get; set; }
        [Display(Name = "Descripción"), MaxLength(20, ErrorMessage = "Máximo 20 caracteres")]
        [Required(ErrorMessage = "El Campo Descripción es obligatorio")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_act { get; set; }

        public Ca_TipoPagosModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_act = DateTime.Now;
        }
    }

    public class Ca_TipoPolizasModel
    {
        public const byte INGRESOS = 1;
        public const byte EGRESOS = 2;
        public const byte DIARIO = 3;
        public const byte ORDEN = 4;

        public byte Id_TipoPoliza { get; set; }
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_TipoPolizasModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_TipoTransferenciasEgModel
    {
        [Display(Name = "Tipo transferencia")]
        public byte Id_Tipotransf { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Automática")]
        public Nullable<bool> Automatica { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_TipoTransferenciasEgModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_TipoTransferenciasIngModel
    {
        public byte Id_TipoTransf { get; set; }
        public string Descripcion { get; set; }
        public Nullable<bool> Automatica { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_TipoTransferenciasIngModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_UnidadMModel
    {
        public short Id_UnidadM { get; set; }
        public string Descrip { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_UnidadMModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class Ca_GirosModel
    {
        public short Id_GiroComercial { get; set; }
        public string Descripcion { get; set; }
        public Nullable<short> uAct { get; set; }
        public Nullable<System.DateTime> fAct { get; set; }
    }

    public class Ca_CamaraComercioModel
    {
        public byte IdCamara { get; set; }
        public string Descripcion { get; set; }
        public Nullable<short> uAct { get; set; }
        public Nullable<System.DateTime> fAct { get; set; }
    }

    public class Ca_CentroRecaudadorModel
    {
        [Display(Name = "URI")]
        [StringLength(2, ErrorMessage = "Máximo dos caracteres.")]
        [Required(ErrorMessage = "*")]
        public byte Id_URI { get; set; }
        [Display(Name = "UEI")]
        [StringLength(2, ErrorMessage = "Máximo dos caracteres.")]
        [Required(ErrorMessage = "*")]
        public byte Id_UEI { get; set; }
        [Display(Name = "UPI")]
        [StringLength(2, ErrorMessage = "Máximo dos caracteres.")]
        [Required(ErrorMessage = "*")]
        public byte Id_UPI { get; set; }
        [Display(Name = "Centro Recaudador")]
        public string Id_CRecaudador { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        [Display(Name = "Último Nivel")]
        public Nullable<bool> UltimoNivel { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public bool ValidaAdd(byte IdUPI, byte IdURI, byte IdUEI, string Descripcion, bool? Nivel, ref string msn)
        {
            try
            {
                string Id = "";
                Ca_CentroRecaudador entities = new Ca_CentroRecaudador();
                CentroRecaudadorDAL DAL = new CentroRecaudadorDAL();

                //Si esta repetido el ID
                Id = StringID.IdArea(IdUPI, IdURI, IdUEI);
                entities = DAL.GetByID(x => x.Id_CRecaudador == Id);
                if (entities != null)
                {
                    msn = StringID.Yaexiste + "Centro Recaudador";
                    return false;
                }
                else
                {
                    if (!ValidaIdArea(IdUPI, IdURI, IdUEI, Nivel, ref msn))
                        return false;
                    return ValidaDesc(Descripcion, ref msn);
                }
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaIdArea(byte IdUPI, byte IdURI, byte IdUEI, bool? Nivel, ref string msn)
        {
            try
            {
                Ca_CentroRecaudador concepto = new Ca_CentroRecaudador();
                CentroRecaudadorDAL DAL = new CentroRecaudadorDAL();
                string Id = "";
                if (IdUPI > 0 && IdURI == 0 && IdUEI == 0)
                {
                }
                else
                {
                    if (IdUPI > 0 && IdURI > 0 && IdUEI == 0)
                        Id = StringID.IdCRecaudador(IdUPI, 0, 0);
                    else
                        if (IdUPI > 0 && IdURI > 0 && IdUEI > 0)
                            Id = StringID.IdCRecaudador(IdUPI, IdURI, 0);
                    concepto = DAL.GetByID(x => x.Id_CRecaudador == Id);
                    if (concepto == null)
                    {
                        msn = "El Centro Recaudador que usted intenta guardar no tiene un registro ascendente, favor de verificarlo";
                        return false;
                    }
                    else
                    {
                        if (concepto.UltimoNivel == true)
                        {
                            msn = "El Centro Recaudador que usten intenta guardar tiene un registro ascendente de ultimo nivel, favor de verificarlo";
                            return false;
                        }
                    }
                }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaDesc(string Descripcion, ref string msn)
        {
            try
            {
                CentroRecaudadorDAL DAL = new CentroRecaudadorDAL();
                Ca_CentroRecaudador concepto = DAL.GetByID(x => x.Descripcion == Descripcion);
                if (concepto != null)
                {
                    msn = StringID.Yaexiste + "Centro Recaudador";
                    return false;
                }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaEdit(byte IdUPI, byte IdURI, byte IdUEI, string Descripcion, bool? Nivel, ref string msn)
        {
            try
            {
                CentroRecaudadorDAL DAL = new CentroRecaudadorDAL();
                Ca_CentroRecaudador concepto = DAL.GetByID(x => x.Id_URI == IdURI && x.Id_UEI == IdUEI && x.Id_UPI == IdUPI);
                if (concepto.Descripcion != Descripcion)
                    if (!ValidaDesc(Descripcion, ref msn)) return false;
                if (concepto.UltimoNivel != Nivel)
                    return ValidaIdConceptoEdit(IdUPI, IdURI, IdUEI, ref msn);
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }

        public bool ValidaDelete(byte IdUPI, byte IdURI, byte IdUEI, ref string msn)
        {
            try
            {
                CentroRecaudadorDAL DAL = new CentroRecaudadorDAL();
                int c = 0;
                if (IdUPI > 0 && IdURI == 0 && IdUEI == 0)
                {
                    c = DAL.Get(x => x.Id_UPI == IdUPI).Count();
                    if (c > 1)
                    {
                        msn = "No se puede eliminar porque ese Centro Recaudador tiene registros descendentes";
                        return false;
                    }
                }
                else
                    if (IdUPI > 0 && IdURI > 0 && IdUEI == 0)
                    {
                        c = DAL.Get(x => x.Id_UPI == IdUPI && x.Id_URI == IdURI).Count();
                        if (c > 1)
                        {
                            msn = "No se puede eliminar porque ese Centro Recaudador tiene registros descendentes";
                            return false;
                        }
                    }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaIdConceptoEdit(byte IdUPI, byte IdURI, byte IdUEI, ref string msn)
        {
            try
            {
                CentroRecaudadorDAL DAL = new CentroRecaudadorDAL();
                int c = 0;
                if (IdUPI > 0 && IdURI == 0 && IdUEI == 0)
                {
                    c = DAL.Get(x => x.Id_UPI == IdUPI).Count();
                    if (c > 1)
                    {
                        msn = StringID.EditarUltimoNivel("descendentes");
                        return false;
                    }
                }
                else
                    if (IdUPI > 0 && IdURI > 0 && IdUEI == 0)
                    {
                        c = DAL.Get(x => x.Id_UPI == IdUPI && x.Id_URI == IdURI).Count();
                        if (c > 1)
                        {
                            msn = StringID.EditarUltimoNivel("descendentes");
                            return false;
                        }
                    }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
    }
    public class Ca_FuentesFin_IngModel
    {
        [Display(Name = "Fuente")]
        [StringLength(1, ErrorMessage = "Máximo un caracter.")]
        [Required(ErrorMessage = "*")]
        public byte Id_Fuente { get; set; }
        [Display(Name = "Aportación")]
        [StringLength(1, ErrorMessage = "Máximo un caracter.")]
        [Required(ErrorMessage = "*")]
        public byte Id_Aportacion { get; set; }
        [Display(Name = "Convenio")]
        [StringLength(2, ErrorMessage = "Máximo dos caracteres.")]
        [Required(ErrorMessage = "*")]
        public byte Id_Convenio { get; set; }
        [Display(Name = "Fuente Financiamiento")]
        public string Id_FuenteFinancia { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        public Nullable<bool> UltimoNivel { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public bool ValidaAdd(byte IdFuente, byte IdAportacion, byte IdConvenio, string Descripcion, bool? Nivel, ref string msn)
        {
            try
            {
                string Id = "";
                Ca_FuentesFin_Ing entities = new Ca_FuentesFin_Ing();
                FuenteIngDAL DAL = new FuenteIngDAL();

                //Si esta repetido el ID
                Id = StringID.IdFuenteFin(IdFuente, IdAportacion, IdConvenio);
                entities = DAL.GetByID(x => x.Id_FuenteFinancia == Id);
                if (entities != null)
                {
                    msn = StringID.Yaexiste + "Concepto Ingreso";
                    return false;
                }
                else
                {
                    if (!ValidaIdArea(IdFuente, IdAportacion, IdConvenio, Nivel, ref msn))
                        return false;
                    return ValidaDesc(Descripcion, ref msn);
                }
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaIdArea(byte IdFuente, byte IdAportacion, byte IdConvenio, bool? Nivel, ref string msn)
        {
            try
            {
                Ca_FuentesFin_Ing concepto = new Ca_FuentesFin_Ing();
                FuenteIngDAL DAL = new FuenteIngDAL();
                string Id = "";
                if (IdFuente > 0 && IdAportacion == 0 && IdConvenio == 0)
                {
                }
                else
                {
                    if (IdFuente > 0 && IdAportacion > 0 && IdConvenio == 0)
                        Id = StringID.IdFuenteFin(IdFuente, 0, 0);
                    else
                        if (IdFuente > 0 && IdAportacion > 0 && IdConvenio > 0)
                            Id = StringID.IdFuenteFin(IdFuente, IdAportacion, 0);
                    concepto = DAL.GetByID(x => x.Id_FuenteFinancia == Id);
                    if (concepto == null)
                    {
                        msn = "La Fuente que usted intenta guardar no tiene un registro ascendente, favor de verificarlo";
                        return false;
                    }
                    else
                    {
                        if (concepto.UltimoNivel == true)
                        {
                            msn = "La Fuente que usten intenta guardar tiene un registro ascendente de ultimo nivel, favor de verificarlo";
                            return false;
                        }
                    }
                }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaDesc(string Descripcion, ref string msn)
        {
            try
            {
                FuenteIngDAL DAL = new FuenteIngDAL();
                Ca_FuentesFin_Ing concepto = DAL.GetByID(x => x.Descripcion == Descripcion);
                if (concepto != null)
                {
                    msn = StringID.Yaexiste + "Fuente";
                    return false;
                }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaEdit(byte IdFuente, byte IdAportacion, byte IdConvenio, string Descripcion, bool? Nivel, ref string msn)
        {
            try
            {
                FuenteIngDAL DAL = new FuenteIngDAL();
                Ca_FuentesFin_Ing concepto = DAL.GetByID(x => x.Id_Fuente == IdFuente && x.Id_Aportacion == IdAportacion && x.Id_Convenio == IdConvenio);
                if (concepto.Descripcion != Descripcion)
                    if (!ValidaDesc(Descripcion, ref msn)) return false;
                if (concepto.UltimoNivel != Nivel)
                    return ValidaIdConceptoEdit(IdFuente, IdAportacion, IdConvenio, ref msn);
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }

        public bool ValidaDelete(byte IdFuente, byte IdAportacion, byte IdConvenio, ref string msn)
        {
            try
            {
                FuenteIngDAL DAL = new FuenteIngDAL();
                int c = 0;
                if (IdFuente > 0 && IdAportacion == 0 && IdConvenio == 0)
                {
                    c = DAL.Get(x => x.Id_Fuente == IdFuente).Count();
                    if (c > 1)
                    {
                        msn = "No se puede eliminar porque esa Fuente tiene registros descendentes";
                        return false;
                    }
                }
                else
                    if (IdFuente > 0 && IdAportacion > 0 && IdConvenio == 0)
                    {
                        c = DAL.Get(x => x.Id_Fuente == IdFuente && x.Id_Aportacion == IdAportacion).Count();
                        if (c > 1)
                        {
                            msn = "No se puede eliminar porque esa Fuente tiene registros descendentes";
                            return false;
                        }
                    }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
        public bool ValidaIdConceptoEdit(byte IdFuente, byte IdAportacion, byte IdConvenio, ref string msn)
        {
            try
            {
                FuenteIngDAL DAL = new FuenteIngDAL();
                int c = 0;
                if (IdFuente > 0 && IdAportacion == 0 && IdConvenio == 0)
                {
                    c = DAL.Get(x => x.Id_Fuente == IdFuente).Count();
                    if (c > 1)
                    {
                        msn = StringID.EditarUltimoNivel("descendentes");
                        return false;
                    }
                }
                else
                    if (IdFuente > 0 && IdAportacion > 0 && IdConvenio == 0)
                    {
                        c = DAL.Get(x => x.Id_Fuente == IdFuente && x.Id_Aportacion == IdAportacion).Count();
                        if (c > 1)
                        {
                            msn = StringID.EditarUltimoNivel("descendentes");
                            return false;
                        }
                    }
                msn = "Validación Correcta";
                return true;
            }
            catch (Exception ex)
            {
                msn = StringID.Exceptions + ex.Message;
                return false;
            }
        }
    }

    public class CA_CURModel
    {
        [Display(Name = "CRI")]
        public string Id_Concepto { get; set; }
        [Display(Name = "Nivel 4")]
        public string nivel4 { get; set; }
        [Display(Name = "Nivel 5")]
        public string nivel5 { get; set; }
        [Display(Name = "Nivel 6")]
        public string nivel6 { get; set; }
        [Display(Name = "Nivel 7")]
        public string nivel7 { get; set; }
        [Display(Name = "Nivel 8")]
        public string nivel8 { get; set; }
        [Display(Name = "Nivel 9")]
        public string nivel9 { get; set; }
        [Display(Name = "Nivel 10")]
        public string nivel10 { get; set; }
        [Display(Name = "Nivel 11")]
        public string nivel11 { get; set; }
        [Display(Name = "Nivel 12")]
        public string Nivel12 { get; set; }
        [Display(Name = "CUR")]
        public string IdCUR { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Úlitmo Nivel")]
        public Nullable<bool> UltimoNivel { get; set; }
        public Nullable<short> uAct { get; set; }
        public Nullable<System.DateTime> fAct { get; set; }
    }

    public class Ca_CajasReceptorasModel
    {
        [Display(Name = "Id Caja")]
        public byte Id_CajaR { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }

        public Ca_CajasReceptorasModel()
        {
            UsuarioLogueado appUsuario = HttpContext.Current.Session["appUsuario"] as UsuarioLogueado;
            Usu_Act = (Int16)appUsuario.IdUsuario;
            Fecha_Act = DateTime.Now;
        }
    }

    public class CA_InHabilModel
    {
        public short Id_Dia { get; set; }
        public Nullable<byte> Dia { get; set; }
        public Nullable<byte> Mes { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "*")]
        public string Descrip { get; set; }
        public string MesLetra { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public System.DateTime Fecha_Act { get; set; }
        public System.DateTime Fecha { get; set; }
    }

    public class TA_FirmasModel
    {
        public string UsoPoliza { get; set; }
        public short Id_Firma { get; set; }
        [Required(ErrorMessage = "*")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "*")]
        public string Cargo { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
        public Nullable<byte> IdTipo { get; set; }
    }
}