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
    
    public partial class Ma_ReciboIngresos
    {
        public Ma_ReciboIngresos()
        {
            this.De_ReciboIngresos = new HashSet<De_ReciboIngresos>();
        }
    
        public int Folio { get; set; }
        public System.DateTime Fecha { get; set; }
        public byte Id_CajaR { get; set; }
        public int IdContribuyente { get; set; }
        public string Observaciones { get; set; }
        public Nullable<decimal> Importe { get; set; }
        public Nullable<byte> IdTipoPDevengado { get; set; }
        public Nullable<int> IdFolioPDevengado { get; set; }
        public Nullable<byte> IdMesPDevengado { get; set; }
        public Nullable<byte> IdTipoPRecaudado { get; set; }
        public Nullable<int> IdFolioPRecaudado { get; set; }
        public Nullable<byte> IdMesPRecaudado { get; set; }
        public Nullable<byte> IdTipoPDevengadoC { get; set; }
        public Nullable<int> IdFolioPDevengadoC { get; set; }
        public Nullable<byte> IdMesPDevengadoC { get; set; }
        public Nullable<byte> IdTipoPRecaudadoC { get; set; }
        public Nullable<int> IdFolioPRecaudadoC { get; set; }
        public Nullable<byte> IdMesPRecaudadoC { get; set; }
        public Nullable<bool> Impreso { get; set; }
        public Nullable<byte> IdEstatus { get; set; }
        public Nullable<System.DateTime> FechaRecaudacion { get; set; }
        public Nullable<System.DateTime> FechaCancelacion { get; set; }
        public Nullable<short> Id_Banco { get; set; }
        public Nullable<short> Id_CtaBancaria { get; set; }
        public string Usuario_Captura { get; set; }
        public string Usuario_Recaudacion { get; set; }
        public string Usuario_Cancelacion { get; set; }
        public Nullable<byte> Id_TipoPIngresos { get; set; }
        public Nullable<int> Id_FolioPIngresos { get; set; }
        public Nullable<byte> Id_MesPIngresos { get; set; }
        public Nullable<byte> Id_TipoPDiario { get; set; }
        public Nullable<int> Id_FolioPDiario { get; set; }
        public Nullable<byte> Id_MesPDiario { get; set; }
        public Nullable<byte> Id_TipoPIngresosC { get; set; }
        public Nullable<int> Id_FolioPIngresosC { get; set; }
        public Nullable<byte> Id_MesPIngresosC { get; set; }
        public Nullable<byte> Id_TipoPDiarioC { get; set; }
        public Nullable<int> Id_FolioPDiarioC { get; set; }
        public Nullable<byte> Id_MesPDiarioC { get; set; }
        public Nullable<short> uAct { get; set; }
        public Nullable<System.DateTime> fAct { get; set; }
    
        public virtual Ca_CajasReceptoras Ca_CajasReceptoras { get; set; }
        public virtual ICollection<De_ReciboIngresos> De_ReciboIngresos { get; set; }
        public virtual Ma_Polizas Ma_Polizas { get; set; }
        public virtual Ma_Polizas Ma_Polizas1 { get; set; }
        public virtual Ma_Polizas Ma_Polizas2 { get; set; }
        public virtual Ma_Polizas Ma_Polizas3 { get; set; }
        public virtual Ca_CuentasBancarias Ca_CuentasBancarias { get; set; }
    }
}
