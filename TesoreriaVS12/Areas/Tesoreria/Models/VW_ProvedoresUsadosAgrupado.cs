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
    
    public partial class VW_ProvedoresUsadosAgrupado
    {
        public int IdPersona { get; set; }
        public Nullable<int> IdRepresentanteLegal { get; set; }
        public string Nombre { get; set; }
        public string CURP { get; set; }
        public string RFC { get; set; }
        public Nullable<byte> IdEstado { get; set; }
        public Nullable<short> IdMunicipio { get; set; }
        public Nullable<short> IdLocalidad { get; set; }
        public Nullable<short> IdColonia { get; set; }
        public Nullable<short> IdCalle { get; set; }
        public string NumeroExt { get; set; }
        public string NumeroInt { get; set; }
        public string CP { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public Nullable<decimal> Subtotal { get; set; }
        public Nullable<decimal> IVA { get; set; }
        public Nullable<decimal> Ret_ISR { get; set; }
        public Nullable<decimal> Ret_IVA { get; set; }
        public Nullable<decimal> Ret_Obra { get; set; }
        public Nullable<decimal> Ret_Otras { get; set; }
        public Nullable<decimal> TOTAL { get; set; }
        public Nullable<byte> Id_TipoBeneficiario { get; set; }
        public string TipoBeneficiario { get; set; }
        public Nullable<byte> Id_ClasBeneficiario { get; set; }
        public string ClasificaBeneficiario { get; set; }
    }
}
