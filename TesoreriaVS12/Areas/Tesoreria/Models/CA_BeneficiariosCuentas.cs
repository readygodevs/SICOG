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
    
    public partial class CA_BeneficiariosCuentas
    {
        public int Id_Beneficiario { get; set; }
        public byte Id_ClasBeneficiario { get; set; }
        public string Id_Cuenta { get; set; }
        public Nullable<short> Usu_act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
    
        public virtual Ca_Beneficiarios Ca_Beneficiarios { get; set; }
        public virtual Ca_ClasificaBeneficiarios Ca_ClasificaBeneficiarios { get; set; }
    }
}
