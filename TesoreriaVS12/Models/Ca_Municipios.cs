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
    
    public partial class Ca_Municipios
    {
        public Ca_Municipios()
        {
            this.Ca_Localidades = new HashSet<Ca_Localidades>();
        }
    
        public byte Id_Estado { get; set; }
        public short Id_Municipio { get; set; }
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
    
        public virtual Ca_Estados Ca_Estados { get; set; }
        public virtual ICollection<Ca_Localidades> Ca_Localidades { get; set; }
    }
}
