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
    
    public partial class PA_WSConsultaDetalleCompromiso_Result
    {
        public short Id_TipoCompromiso { get; set; }
        public int Id_FolioCompromiso { get; set; }
        public short Id_Registro { get; set; }
        public string Id_Cuenta { get; set; }
        public string Id_ClavePresupuesto { get; set; }
        public Nullable<decimal> Importe { get; set; }
        public Nullable<byte> Id_Movimiento { get; set; }
        public Nullable<bool> Disponibilidad { get; set; }
        public Nullable<bool> AfectaCompro { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
    }
}
