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
    
    public partial class vListaContraRecibosAsignacionCheques12
    {
        public byte Id_TipoCR { get; set; }
        public int Id_FolioCR { get; set; }
        public Nullable<short> Id_CtaBancaria { get; set; }
        public Nullable<System.DateTime> FechaVen { get; set; }
        public string Descripcion { get; set; }
        public Nullable<System.DateTime> Fecha_Ejercido { get; set; }
        public Nullable<decimal> Importe { get; set; }
        public Nullable<int> No_Cheque { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
    }
}
