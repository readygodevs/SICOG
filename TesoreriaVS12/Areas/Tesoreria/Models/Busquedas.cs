using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TesoreriaVS12.Areas.Tesoreria.Models
{
    public class BusquedaCompromisos
    {
        [Display(Name="Folio")]
        public int Folio { get; set; }
        [Display(Name = "Tipo de Compromiso")]
        public short TipoCompromiso { get; set; }
        [Display(Name = "Orden")]
        public short Orden { get; set; }
        [Display(Name = "Estatus del Compromiso")]
        public short Estatus { get; set; }
        [Display(Name = "No. Requisición")]
        public int NoRequisicion { get; set; }
        [Display(Name = "Beneficiario")]
        public int IdBeneficiario { get; set; }
        [Display(Name = "No. de Orden de Compra")]
        public int NoOrdenCompra{ get; set; }
        [Display(Name = "")]
        public string DescripcionBeneficiario { get; set; }
        [Display(Name = "Fecha de Fincamiento")]
        public DateTime? Fecha_Fincamiento { get; set; }
        [Display(Name = "Procede de Adquisiciones")]
        public bool isAdquisiciones { get; set; }

        /*Listas*/
        public SelectList ListTipoCompromiso { get; set; }
        public SelectList ListEstatus { get; set; }
    }

    public sealed class FiltrosCuentas
    {
        /// <summary>
        /// Lista de datos tipo Byte que indican los generos en que buscará la cuenta
        /// </summary>
        public List<Int32> Genero { get; set; }
        /// <summary>
        /// Indica el grupo para filtar la tabla
        /// </summary>
        public Byte Grupo { get; set; }
        /// <summary>
        /// Lista de datos tipo Byte que indican los grupos en que buscará la cuenta
        /// </summary>
        public List<Int32> Grupos { get; set; }
        /// <summary>
        /// indica el Rubro en que buscará la cuenta
        /// </summary>
        public Byte Rubro { get; set; }
        /// <summary>
        /// Indica la cuenta para filtrar la tabla
        /// </summary>
        public Byte Cuenta { get; set; }
        /// <summary>
        /// Indica la cuenta O1 para filtar la tabla
        /// </summary>
        public Byte SubcuentaO1 { get; set; }

        /// <summary>
        /// Mostrar o no las cuentas de último Nivel: True = Mostrar ultimo nivel
        /// </summary>
        public bool UltimoNivel { get; set; }
        /// <summary>
        /// <summary>
        /// Mostrar o no la tabla completa, es decir, todas la cuentas. True = Todas;
        /// </summary>
        public bool? Completa { get; set; }
        /// <summary>
        /// Permite o no seleccionar solamente las cuentas que no son ultimo nivel. True = solo no último Nivel
        /// </summary>
        public bool? selectNoUltimoNivel { get; set; }

        /// <summary>
        /// Permite o no seleccionar solamente las cuentas de ultimo nivel. True = solo último Nivel
        /// </summary>
        public bool? selectUltimoNivel { get; set; }

        public String IdCri { get; set; }

        public String IdCuenta { get; set; }

        public String Descripcion { get; set; }

        public List<String> IdCuentasRestrictivas { get; set; }
        public bool? RestringirCuentas { get; set; }
        public string ParametroCuentas { get; set; }

        public String GeneroStr { get; set; }
        public String GrupoStr { get; set; }
        public FiltrosCuentas()
        {
            IdCuentasRestrictivas = new List<String>();
            Genero = new List<int>();
            Grupos=new List<int>();
        }
        
    }

    public sealed class BusquedaContrarecibos
    {
        [Display(Name = "Nombre de Beneficiario")]
        public String NombreBeneficiario { get; set; }
        [Display(Name = "Folio de contrarecibo")]
        public Int32? FolioContrarecibo { get; set; }

        public Int32 TipoCR { get; set; }
        [Display(Name = "No. de Compromiso")]
        public Int32? NoCompromiso { get; set; }
        [Display(Name = "No. de Requisicion")]
        public Int32? NoRequisicion { get; set; }
        [Display(Name = "No. de Orden de Compra")]
        public Int32? NoOrdenCompra { get; set; }
        [Display(Name = "No. Cheque")]
        public Int32? NoCheque { get; set; }
        public BusquedaContrarecibos()
        {
        }

        public BusquedaContrarecibos(byte Tipo)
        {
            this.TipoCR = Tipo;
        }
    }
}