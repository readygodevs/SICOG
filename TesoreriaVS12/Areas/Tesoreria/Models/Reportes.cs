using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesoreriaVS12.Areas.Tesoreria.Models
{


    public class Reportes
    {

        


    }
    public partial class tblRepCuentasModel
    {
        public byte Genero { get; set; }
        public byte Grupo { get; set; }
        public byte Rubro { get; set; }
        public string Descripcion { get; set; }
        public Nullable<decimal> Ejercicio1 { get; set; }
        public Nullable<decimal> Ejercicio2 { get; set; }
        public string Fecha { get; set; }
    }
}