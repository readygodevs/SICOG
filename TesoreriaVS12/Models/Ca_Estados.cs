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
    
    public partial class Ca_Estados
    {
        public Ca_Estados()
        {
            this.Ca_Municipios = new HashSet<Ca_Municipios>();
        }
    
        public byte Id_Estado { get; set; }
        public string Descripcion { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
    
        public virtual ICollection<Ca_Municipios> Ca_Municipios { get; set; }
    }
}
