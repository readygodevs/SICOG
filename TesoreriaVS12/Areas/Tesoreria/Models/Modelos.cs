using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;

namespace TesoreriaVS12.Areas.Tesoreria.Models
{
    public class Modelos
    {
        
        public class ImpresionChequesModel
        {
            public ImpresionChequesModel()
            {
                this.Fecha_Pago = null;
                this.Fecha_VenceContraRecibo = null;
                this.NoChequesImprimir = 0;
            }

            [Display(Name="Banco")]
            [Required(ErrorMessage="Debe elegir un banco")]
            public short Id_CtaBancaria { get; set; }
            [Display(Name = "Formato de cheque")]
            [Required(ErrorMessage = "Debe elegir un formato de cheque")]
            public byte Id_Formato { get; set; }
            [Display(Name = "Fecha de vencimiento de contrarecibo")]            
            public DateTime? Fecha_VenceContraRecibo { get; set; }
            [Display(Name = "Fecha de pago")]
            [Required(ErrorMessage = "Debe elegir una fecha de pago")]
            public DateTime? Fecha_Pago { get; set; }
            [Display(Name = "Cantidad de cheques a imprimir")]
            public int NoChequesImprimir { get; set; }
            [Display(Name = "Solo imprimir el primer cheque")]
            public bool? PrimerCheque { get; set; }
            public Control_Fechas CFechas { get; set; }

            /*Listas*/
            public SelectList Lista_Bancos { get; set; }
            public SelectList Lista_FormatosCheques { get; set; }
        }

        public class ChequesModel
        {
            public ChequesModel()
            {
                this.Cheques = new List<Chequestbl>();
                this.PolizasEgresos = new List<Chequestbl>();
                this.PolizasDiario = new List<Chequestbl>();
            }

            public List<Chequestbl> Cheques { get; set; }
            public List<Chequestbl> PolizasEgresos { get; set; }
            public List<Chequestbl> PolizasDiario { get; set; }
            public byte Formato { get; set; }
            public string UsuarioGeneroCheques { get; set; }
        }

        public class DisponibilidadModel
        {
            [Display(Name="Mes")]
            public int Id_Mes { get; set; }
            public SelectList Lista_Meses { get; set; }
        }
        public class EvolucionModel
        {
            [Display(Name = "Mes")]
            public int Id_Mes { get; set; }
            public SelectList Lista_Meses { get; set; }
        }
        

        public class ContraRecibosAnalisisModel
        {            
            [Display(Name="Tipo de Cuenta por Liquidar")]
            public byte Id_TipoCR { get; set; }
            [Display(Name = "Folio")]
            public int Id_FolioCR { get; set; }
            [Display(Name = "Tipo de Compromiso")]
            public Nullable<short> Id_TipoCompromiso { get; set; }
            [Display(Name = "Banco")]
            public Nullable<short> Id_Banco { get; set; }
            [Display(Name = "Cuenta Bancaria")]
            public Nullable<short> Id_CtaBancaria { get; set; }
            [Display(Name = "Desde")]
            public Nullable<System.DateTime> Fecha_Pago_I { get; set; }
            [Display(Name = "Hasta")]
            public Nullable<System.DateTime> Fecha_Pago_F { get; set; }
            [Display(Name = "Desde")]
            public Nullable<decimal> Cargos_I { get; set; }
            [Display(Name = "Hasta")]
            public Nullable<decimal> Cargos_F { get; set; }
            [Display(Name = "Estatus")]
            public Nullable<byte> Id_EstatusCR { get; set; }
            [Display(Name = "Situación Cheque")]
            public Nullable<byte> Id_SituacionCheque { get; set; }
            [Display(Name = "Número de Cheque")]
            public Nullable<int> No_Cheque { get; set; }
            [Display(Name = "Beneficiario")]
            public Nullable<int> Id_Beneficiario { get; set; }
            public string NombreBeneficiario { get; set; }
            [Display(Name = "Establecer Orden del Reporte")]
            public int orden { get; set; }

            public SelectList Lista_Id_TipoCR { get; set; }
            public SelectList Lista_Id_EstatusCR { get; set; }
            public SelectList Lista_Id_Banco { get; set; }
            public SelectList Lista_Id_CtaBancaria { get; set; }
            public SelectList Lista_Id_TipoCompromiso { get; set; }
            public SelectList Lista_Id_SituacionCheque { get; set; }
            public SelectList Lista_orden { get; set; }

            public ContraRecibosAnalisisModel()
            {
                TipoContrarecibosDAL DaltipoContra = new TipoContrarecibosDAL();
                TipoCompromisosDAL DALTipoCompromisos = new TipoCompromisosDAL();
                BancosDAL dalBancos = new BancosDAL();
                CuentasBancariasDAL dalCtaBancarias = new CuentasBancariasDAL();
                Dictionary<byte, string> situacionCheque = new Dictionary<byte, string>{
                    {1, "TODOS"},
                    {2, "CHEQUE SIN IMPRIMIR"},
                    {3, "CHEQUE IMPRESO"}
                };
                Dictionary<byte, string> ordenReporte = new Dictionary<byte, string>{
                    {1, "PROVEEDOR"},
                    {2, "FECHA DE PAGO"},
                    {3, "TIPO Y FOLIO DE CONTRARECIBO"},
                    {4, "NÚMERO DE CHEQUE"}
                };
                this.Lista_Id_TipoCR = new SelectList(DaltipoContra.Get(), "Id_TipoCR", "Descripcion");
                this.Lista_Id_EstatusCR = new SelectList(Diccionarios.Estatus_CR, "Key", "Value");                
                this.Lista_Id_Banco = new SelectList(dalBancos.Get(), "Id_Banco", "Descripcion");
                this.Lista_Id_CtaBancaria = new SelectList(dalCtaBancarias.Get(), "Id_CtaBancaria", "Descripcion");
                this.Lista_Id_SituacionCheque = new SelectList(situacionCheque, "Key", "Value");
                this.Lista_Id_TipoCompromiso = new SelectList(DALTipoCompromisos.Get(), "Id_TipoCompromiso", "Descripcion");
                this.Lista_orden = new SelectList(ordenReporte, "Key", "Value");
            }
        }
        
    }
}