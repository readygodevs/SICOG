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
    
    public partial class DE_Banco_Cheque
    {
        public short Id_CtaBancaria { get; set; }
        public short Id_Asignacion { get; set; }
        public Nullable<int> Cheque_Ini { get; set; }
        public Nullable<int> Cheque_Fin { get; set; }
        public Nullable<System.DateTime> Fecha_Asigna { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
    }
}
