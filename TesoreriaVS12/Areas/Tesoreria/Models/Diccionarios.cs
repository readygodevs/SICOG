using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesoreriaVS12.Areas.Tesoreria.Models
{
    public class Diccionarios
    {
        public class ValorEstatus
        {
            public const int COMPROMETIDO = 1;
            public const int ASIGANADO_CR = 2;
            public const int PAGADO = 3;
            public const int CANCELADO = 4;
            public const int AUTORIZACION = 5;
            public const int RECIBIDO = 6;
        }

        public class ValorMovimientos
        {
            public const byte CARGO = 1;
            public const byte ABONO = 2;
        }

        public class Valores_EstatusRecibos
        {
            public const byte REGISTRADO = 1;
            public const byte RECAUDADO = 2;
            public const byte CANCELADO = 3;
            public const byte DEVENGADO = 4;
        }

        public class TiposCR
        {
            public const byte FondosRevolventes = 3;
            public const byte GastosComprobar = 4;
            public const byte CancelacionPasivos = 5;
            public const byte AnticiposPrestamos = 6;
            public const byte EgresosNoPresupuestales = 7;
            public const byte ContraRecibos = 1;
            public const byte Arrendamientos = 8;
            public const byte Honorarios = 9;//Honorarios puros
            public const byte CancelacionActivos = 10;
            public const byte HonorariosAsimilables = 11;
        }

        public class Valores_Estatus_CR
        {
            public const byte Programado = 1;
            public const byte Pagado = 2;
            public const byte Cancelado = 3;
            public const byte Transferido = 4;
            public const byte Comprobado = 5;
            public const byte Autorización = 6;
        }
        public static readonly Dictionary<int, string> ListaTiposContrarecibos= new Dictionary<int, string>
        {
            {3,"Fondos Revolventes"},{4,"Gastos a Comprobar"},{5,"Cancelación de Pasivos"},{1,"Proveedores"}
        };
        
        public static readonly Dictionary<int, string> EstatusCompromisos = new Dictionary<int, string>
        {
            {1,"Comprometido"},{2,"Asignado a CxL"},{3,"Pagado"},{4,"Cancelado"},{5,"Autorización"},{6,"Devengado"}
        };
        public static readonly Dictionary<byte, string> EstatusRecibos = new Dictionary<byte, string>
        {
            {1,"REGISTRADO"},{2,"RECAUDADO"},{3,"CANCELADO"},{4,"DEVENGADO"}
        };

        public static readonly Dictionary<int, string> OrdenCompromisos = new Dictionary<int, string>
        {
            {1,"Orden de Compra"},{2,"Orden de Servicio"}
        };
        public static readonly Dictionary<Int16, string> TipoPago = new Dictionary<short, string>
        {
            {1,"Pago con deposito"}, {2, "Pago con efectivo"},{3,"Pago en especie"},{4,"Pago por cheque"}
        };

        public static readonly Dictionary<Int16, string> TipoPersona = new Dictionary<short, string>
        {
            {1,"Fisica"}, {2, "Moral"}
        };

        public static readonly Dictionary<Int16, string> Movimiento = new Dictionary<short, string>
        {
            {1,"Cargo"}, {2, "Abono"}
        };

        public static readonly Dictionary<Int16, string> Estatus_CR = new Dictionary<short, string>
        {
            {1,"Programado"}, {2, "Pagado"}, {3, "Cancelado"}, {4, "Transferido"}, {5, "Comprobado"}, {6, "Autorización"}
        };

        public static readonly Dictionary<Int16, string> Estatus_GC = new Dictionary<short, string>
        {
            {1,"Sin Comprobar"}, {2, "Comprobado"}
        };

        public static readonly Dictionary<int, string> Meses = new Dictionary<int, string>
        {
           {1,"ENERO"}, {2,"FEBRERO"}, {3,"MARZO"}, {4,"ABRIL"}, {5,"MAYO"}, {6,"JUNIO"}, {7,"JULIO"}, {8,"AGOSTO"}, {9,"SEPTIEMBRE"}, {10,"OCTUBRE"}, {11,"NOVIEMBRE"}, {12,"DICIEMBRE"}
        };

        public static readonly Dictionary<int, string> Naturaleza = new Dictionary<int, string>
        {
            { 1, "Acredora"}, { 2, "Deudora"}
        };

        public static readonly Dictionary<int, string> Tipo_Tiempo_Depreciacion = new Dictionary<int, string>
        {
            { 1, "Año"}, { 2, "Mes"}
        };

        public static readonly Dictionary<int, string> EstatusTransferencia = new Dictionary<int, string>
        {
            {1,"SIN AFECTAR"},{2,"AFECTADO"},{3,"CANCELADO"}
        };

        public static readonly Dictionary<int, string> DiasSemana = new Dictionary<int, string>
        {
            {0,"SELECCIONE"},{1,"LUNES"},{2,"MARTES"},{3,"MIÉRCOLES"},{4,"JUEVES"},{5,"VIERNES"}
        };

        public static readonly Dictionary<int, string> ListaSemanas= new Dictionary<int, string>
        {
            {0,"SELECCIONE"},{1,"PRIMERA"},{2,"SEGUNDA"},{3,"TERCERA"},{4,"CUARTA"}
        };

        public static readonly Dictionary<int, string> ListaTiposImpuestoDed = new Dictionary<int, string>
        {
            {0,"SELECCIONE"},{1,"IMPUESTO"},{2,"DEDUCCIÓN"}
        };

        public static readonly Dictionary<int, string> TipoPoliza = new Dictionary<int, string>
        {
           {1,"STATUS 1"},{2,"STATUS 2"},{3,"STATUS 3"},{4,"STATUS 4"}
        };
        public static readonly Dictionary<int, string> TipoMovimientoBancario = new Dictionary<int, string>
        {
           {1,"DEPOSITOS"},{2,"RETIROS"},{3,"CONCILIACION"}
        };
        public static readonly Dictionary<int, string> EstatusMovimientoBancario = new Dictionary<int, string>
        {
           {1,"POR REVISAR"},{2,"CONCILIADO CORRECTO"},{3,"EN CONCILIACIÓN"}
        };
        public static readonly Dictionary<bool, string> EstatusEstadosCuenta = new Dictionary<bool, string>
        {
           {true,"CONCILIADO"},{false,"SIN CONCILIAR"}
        };

    }
} 