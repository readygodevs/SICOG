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
    
    public partial class Ca_Localidades
    {
        public Ca_Localidades()
        {
            this.Ca_Calles = new HashSet<Ca_Calles>();
            this.Ca_Colonias = new HashSet<Ca_Colonias>();
        }
    
        public byte Id_Estado { get; set; }
        public short Id_Municipio { get; set; }
        public short Id_Localidad { get; set; }
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
    
        public virtual ICollection<Ca_Calles> Ca_Calles { get; set; }
        public virtual ICollection<Ca_Colonias> Ca_Colonias { get; set; }
        public virtual Ca_Municipios Ca_Municipios { get; set; }
    }
}
