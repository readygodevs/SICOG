//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TesoreriaVS12.Areas.Tesoreria.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ca_ClasificaBeneficiarios
    {
        public Ca_ClasificaBeneficiarios()
        {
            this.CA_BeneficiariosCuentas = new HashSet<CA_BeneficiariosCuentas>();
        }
    
        public byte Id_ClasificaBene { get; set; }
        public string Descripcion { get; set; }
        public string Id_Cuenta { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
    
        public virtual ICollection<CA_BeneficiariosCuentas> CA_BeneficiariosCuentas { get; set; }
    }
}
