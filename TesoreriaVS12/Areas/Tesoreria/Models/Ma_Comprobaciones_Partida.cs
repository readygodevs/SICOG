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
    
    public partial class Ma_Comprobaciones_Partida
    {
        public byte Id_TipoCR { get; set; }
        public int Id_FolioCR { get; set; }
        public short No_Comprobacion { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<int> Id_FolioPolizaCR { get; set; }
        public Nullable<byte> Id_MesPolizaCR { get; set; }
        public Nullable<byte> Id_MesPO_Comprometido { get; set; }
        public Nullable<int> Id_FolioPO_Comprometido { get; set; }
        public Nullable<byte> Id_MesPO_Devengado { get; set; }
        public Nullable<int> Id_FolioPO_Devengado { get; set; }
        public Nullable<byte> Id_MesPO_Ejercido { get; set; }
        public Nullable<int> Id_FolioPO_Ejercido { get; set; }
        public Nullable<byte> Id_MesPO_Pagado { get; set; }
        public Nullable<int> Id_FolioPO_Pagado { get; set; }
        public Nullable<short> Usuario_Act { get; set; }
        public Nullable<System.DateTime> Fecha_Act { get; set; }
    }
}
