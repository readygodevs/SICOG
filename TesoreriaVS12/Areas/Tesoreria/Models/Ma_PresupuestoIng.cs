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
    
    public partial class Ma_PresupuestoIng
    {
        public string Id_CentroRecaudador { get; set; }
        public string Id_Fuente { get; set; }
        public string AnioFin { get; set; }
        public string Id_Alcance { get; set; }
        public string Id_Concepto { get; set; }
        public string Id_ClavePresupuesto { get; set; }
        public Nullable<decimal> Estimado01 { get; set; }
        public Nullable<decimal> Estimado02 { get; set; }
        public Nullable<decimal> Estimado03 { get; set; }
        public Nullable<decimal> Estimado04 { get; set; }
        public Nullable<decimal> Estimado05 { get; set; }
        public Nullable<decimal> Estimado06 { get; set; }
        public Nullable<decimal> Estimado07 { get; set; }
        public Nullable<decimal> Estimado08 { get; set; }
        public Nullable<decimal> Estimado09 { get; set; }
        public Nullable<decimal> Estimado10 { get; set; }
        public Nullable<decimal> Estimado11 { get; set; }
        public Nullable<decimal> Estimado12 { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<byte> Id_MesPO_Estimado { get; set; }
        public Nullable<int> Id_FolioPO_Estimado { get; set; }
        public Nullable<byte> Id_MesPO_Estimado_C { get; set; }
        public Nullable<int> Id_FolioPO_Estimado_C { get; set; }
        public Nullable<System.DateTime> Fecha_Estimado { get; set; }
        public Nullable<System.DateTime> Fecha_CancelaEstimado { get; set; }
        public Nullable<short> Usu_Act { get; set; }
        public Nullable<System.DateTime> Fecha_act { get; set; }
    
        public virtual Ca_AlcanceGeo Ca_AlcanceGeo { get; set; }
    }
}
