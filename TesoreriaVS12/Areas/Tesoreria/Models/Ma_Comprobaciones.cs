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
    
    public partial class Ma_Comprobaciones
    {
        public Ma_Comprobaciones()
        {
            this.De_Comprobaciones = new HashSet<De_Comprobaciones>();
            this.Ma_Contrarecibos = new HashSet<Ma_Contrarecibos>();
        }
    
        public int Id_FolioGC { get; set; }
        public Nullable<decimal> Importe_Reintegro { get; set; }
        public Nullable<decimal> Importe_Sobrante { get; set; }
        public string Id_Cuenta_CtaXPagar { get; set; }
        public Nullable<byte> Id_MesPO_Comprometido { get; set; }
        public Nullable<int> Id_FolioPO_Comprometido { get; set; }
        public Nullable<byte> Id_MesPO_Comprometido_C { get; set; }
        public Nullable<int> Id_FolioPO_Comprometido_C { get; set; }
        public Nullable<byte> Id_MesPO_Devengado { get; set; }
        public Nullable<int> Id_FolioPO_Devengado { get; set; }
        public Nullable<byte> Id_MesPO_Devengado_C { get; set; }
        public Nullable<int> Id_FolioPO_Devengado_C { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
    
        public virtual ICollection<De_Comprobaciones> De_Comprobaciones { get; set; }
        public virtual ICollection<Ma_Contrarecibos> Ma_Contrarecibos { get; set; }
    }
}