//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TesoreriaVS12.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CA_Usuarios
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Usuario { get; set; }
        public string Contrasenia { get; set; }
        public string email { get; set; }
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
    
        public virtual CA_Perfiles CA_Perfiles { get; set; }
    }
}