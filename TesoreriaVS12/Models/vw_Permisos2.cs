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
    
    public partial class vw_Permisos2
    {
        public long id { get; set; }
        public short IdOpcion { get; set; }
        public Nullable<byte> IdGrupo { get; set; }
        public Nullable<byte> IdGrupoPadre { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public string Descripcion { get; set; }
        public Nullable<byte> IdPerfil { get; set; }
        public Nullable<int> IdPermiso { get; set; }
        public Nullable<bool> ActivoPermiso { get; set; }
        public Nullable<short> IdAccion { get; set; }
        public string DescripcionAccion { get; set; }
    }
}
