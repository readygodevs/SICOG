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
    
    public partial class DE_AccionesPermisos
    {
        public int IdRegistro { get; set; }
        public short IdAccion { get; set; }
        public int IdPermiso { get; set; }
        public bool Activo { get; set; }
        public Nullable<byte> IdPerfil { get; set; }
        public Nullable<bool> POST { get; set; }
        public Nullable<bool> GET { get; set; }
    
        public virtual CA_Acciones CA_Acciones { get; set; }
        public virtual DE_Permisos DE_Permisos { get; set; }
        public virtual CA_Perfiles CA_Perfiles { get; set; }
    }
}