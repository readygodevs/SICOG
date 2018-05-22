using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class Ma_Comprobaciones_PartidaModel : Ma_Comprobaciones_Partida
    {
        public decimal? Importe { get; set; }
    }

    public class ReintegrosModel
    {
        public byte TipoCREfectivo { get; set; }
        public int FolioCREfectivo { get; set; }
        public int FolioGCEfectivo { get; set; }
        [Required(ErrorMessage="*")]
        [Display(Name = "Cuenta")]
        public string CtaEfectivo { get; set; }
        [Required(ErrorMessage="*")]
        [Display(Name = "Fecha")]
        public DateTime FechaEfectivo { get; set; }
        [Required(ErrorMessage="*")]
        [Display(Name = "No de Recibo")]
        public int? NoReciboEfectivo { get; set; }
        [Required(ErrorMessage="*")]
        [Display(Name = "Importe")]
        public decimal ImporteEfectivo { get; set; }

        [Required(ErrorMessage="*")]
        [Display(Name = "Banco")]
        public short? CtaBancariaReintegro { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Fecha")]
        public DateTime FechaReintegro { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "No de Recibo")]
        public int? NoReciboReintegro { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Importe")]
        public decimal ImporteReintegro { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Fecha")]
        public DateTime FechaCvePresupuestal { get; set; }

        public List<De_Comprobaciones> De_Comprobaciones { get; set; }
        public List<Ma_Comprobaciones_PartidaModel> Ma_Comprobaciones_Partida { get; set; }

        public ReintegrosModel()
        {
        }
        
        public ReintegrosModel(int FolioGC)
        {
            DeComprobacionesDAL _dalDeCompro = new DeComprobacionesDAL();
            MaComprobacionesPartidasDAL _dalComproPartidas = new MaComprobacionesPartidasDAL();
            MaContrarecibosDAL _dalContra = new MaContrarecibosDAL();
            DeContrarecibosDAL _dalDeContra = new DeContrarecibosDAL();

            De_Comprobaciones = new List<De_Comprobaciones>();
            Ma_Comprobaciones_Partida = new List<Ma_Comprobaciones_PartidaModel>();
            De_Comprobaciones = _dalDeCompro.Get(x => x.Id_FolioGC == FolioGC).ToList();
            Ma_Contrarecibos ma = _dalContra.GetByID(x=>x.Id_FolioGC == FolioGC);
            foreach (Ma_Comprobaciones_Partida item in _dalComproPartidas.Get(x => x.Id_TipoCR == ma.Id_TipoCR && x.Id_FolioCR == ma.Id_FolioCR))
            {
                Ma_Comprobaciones_PartidaModel aux = ModelFactory.getModel<Ma_Comprobaciones_PartidaModel>(item, new Ma_Comprobaciones_PartidaModel());
                aux.Importe = _dalDeContra.Get(x=> x.Id_Movimiento == Diccionarios.ValorMovimientos.CARGO && x.Id_TipoCR == item.Id_TipoCR && x.Id_FolioCR == item.Id_FolioCR && x.No_Comprobacion == item.No_Comprobacion).Sum(x=> x.Importe);
                Ma_Comprobaciones_Partida.Add(aux);
            }
        }
    }

    public class Fondos_GastosBL
    {
        private MaContrarecibosDAL _dalContra;
        private DeContrarecibosDAL _dalDeContra;
        private DeComprobacionesDAL _dalDeCompro;
        private DeFacturasDAL _dalFacturas;
        private CompromisosBL _dalCompromisosBL;
        private ProceduresDAL _dalSP;
        private MaComprobacionesDAL _dalCompro;
        private MaComprobacionesPartidasDAL _dalComproPartidas;
        private ParametrosDAL dalParametros;
        private CuentasDAL dalCuentas;
        private DePolizasDAL _dalDePolizas;


        public Fondos_GastosBL()
        {
            if (_dalContra == null) _dalContra = new MaContrarecibosDAL();
            if (_dalDeContra == null) _dalDeContra = new DeContrarecibosDAL();
            if (_dalDeCompro == null) _dalDeCompro = new DeComprobacionesDAL();
            if (_dalFacturas == null) _dalFacturas = new DeFacturasDAL();
            if (_dalCompromisosBL == null) _dalCompromisosBL = new CompromisosBL();
            if (_dalSP == null) _dalSP = new ProceduresDAL();
            if (_dalCompro == null) _dalCompro = new MaComprobacionesDAL();
            if (_dalComproPartidas == null) _dalComproPartidas = new MaComprobacionesPartidasDAL();
            if (dalParametros == null) dalParametros = new ParametrosDAL();
            if (dalCuentas == null) dalCuentas = new CuentasDAL();
            if (_dalDePolizas == null) _dalDePolizas = new DePolizasDAL();
        }


        public void AddBoton(ref List<object> botonera, string btn)
        {
            botonera.Add(btn);
        }

        public void CreateBotonera(ref List<object> Botones, byte IdTipoCR, int IdFolio)
        {
            byte? estatus = _dalContra.GetByID(x=> x.Id_TipoCR == IdTipoCR && x.Id_FolioCR == IdFolio).Id_EstatusCR;
            if (estatus.HasValue && estatus != Diccionarios.Valores_Estatus_CR.Cancelado)
            {
                this.AddBoton(ref Botones, "bNuevo");
                if (this.stateEdit(IdTipoCR, IdFolio)) this.AddBoton(ref Botones, "bEditar");
                if (this.stateCancel(IdTipoCR, IdFolio)) this.AddBoton(ref Botones, "bCancelarGral");
                this.AddBoton(ref Botones, "bBuscar");
                if (this.statePrint(IdTipoCR, IdFolio)) this.AddBoton(ref Botones, "bImprimir");
                if (this.stateDetails(IdTipoCR, IdFolio)) this.AddBoton(ref Botones, "bDetalles");
                if (this.stateFactura(IdTipoCR, IdFolio)) this.AddBoton(ref Botones, "bDocumentos");
                if (this.stateCloseComprobation(IdTipoCR, IdFolio)) this.AddBoton(ref Botones, "bCerrarComprobacion");
                this.AddBoton(ref Botones, "bSalir");
            }
            else
            {
                this.AddBoton(ref Botones, "bNuevo");
                this.AddBoton(ref Botones, "bBuscar");
                this.AddBoton(ref Botones, "bSalir");
            }
        }

        public bool stateEdit(byte Tipo, int Folio)
        {
            Control_Fechas fecha = new Control_Fechas();
            Ma_Contrarecibos ma = _dalContra.GetByID(x=> x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio);
            if (_dalCompromisosBL.isClosed(Convert.ToDateTime(ma.FechaCR)))
                return false;
            return true;
        }

        public bool stateCancel(byte Tipo, int Folio)
        {
            Ma_Contrarecibos ma = _dalContra.GetByID(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio);
            if (ma.Id_EstatusCR == 2 && ma.Estatus_GC == 1 || ma.Id_EstatusCR == 1 && ma.No_Cheque == null) 
                return true; 
            else 
                return false;
        }

        public bool stateDetails(byte Tipo, int Folio)
        {
            bool? impreso = _dalContra.GetByID(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio).Impreso_CH;
            if (impreso == true)
                return true;
            else
                return false;
        }

        public bool stateFactura(byte Tipo, int Folio)
        {
            bool? impreso = _dalContra.GetByID(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio).Impreso_CH;
            if (impreso == true)
                return true;
            else
                return false;
        }

        public bool statePrint(byte Tipo, int Folio)
        {
            bool? impreso = _dalContra.GetByID(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio).Impreso_CR;
            if (impreso == false)
                return true;
            else
                return false;
        }

        public bool stateCloseComprobation(byte Tipo, int Folio)
        {
            Ma_Contrarecibos ma = _dalContra.GetByID(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio);
            if (ma.Estatus_GC != 2 && ma.Id_EstatusCR == 2 && ma.Impreso_CR == true && hasDetails(Tipo, Folio) == true && hasFacturas(Tipo, Folio) == true)
                return true;
            else
                return false;
        }

        public bool hasDetails(byte Tipo, int Folio)
        {
            if (_dalDeContra.Get(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio).Count() > 0)
                return true;
            else
                return false;
        }

        public bool hasFacturas(byte Tipo, int Folio)
        {
            if (_dalFacturas.Get(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio).Count() > 0)
                return true;
            else
                return false;
        }

        public bool hasReintegro(byte Tipo, int Folio, int? FolioGC)
        {
            if (!FolioGC.HasValue) return false;
            decimal? reintego = _dalContra.GetByID(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio).Cargos;
            decimal? cargosdetalles = _dalDeContra.Get(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio && x.Id_Movimiento == 1).Sum(x => x.Importe);
            if (cargosdetalles == null) cargosdetalles = 0;
            decimal? cargosComprobaciones = 0;
            int ccom = 0;
            if (FolioGC.HasValue)
            {
                cargosComprobaciones = _dalDeCompro.Get(m => m.Id_FolioGC == FolioGC && m.Id_FolioPoliza_C == null).Sum(m => m.Importe);
                ccom = _dalDeCompro.Get(m => m.Id_FolioGC == FolioGC && m.Id_FolioPoliza_C == null).Count();
            }
            if (cargosComprobaciones == null) cargosComprobaciones = 0;
            decimal? suma = cargosComprobaciones + cargosdetalles;
            int dcom = _dalDeContra.Get(m => m.Id_TipoCR == Tipo && m.Id_FolioCR == Folio && m.Id_Registro > 1).Count();
            decimal? Total = reintego - suma;
            if (Total > 0)
                return true;
            else
                if (ccom > 0 || dcom > 0)
                    return true;
                else
                    return false;
        }

        //-----------------------------------------------------------------------------------------------------------//

        public short? getNoComprobacion(byte Tipo, int Folio)
        {
            try
            {
                short? c = _dalDeContra.Get(x=> x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio).Max(x=> x.No_Comprobacion);
                c++;
                return c;
            }
            catch
            {
                return 1;
            }
        }

        public int getFolioGC()
        {
            try
            {
                return _dalCompro.Get().Max(x => x.Id_FolioGC) + 1;
            }
            catch
            {
                return 1;
            }
        }

        public short getConsecutivo(int FolioGC)
        {
            try
            {
                short c = _dalDeCompro.Get(x=> x.Id_FolioGC == FolioGC).Max(x=> x.Id_Consecutivo);
                c++;
                return c;
            }
            catch
            {
                return 1;
            }
        }

        //-------------------------------Acciones de los Botones -----------------------------------------//
        public void CancelarCRFG(byte tipo, int folio, short usuario, DateTime? Fecha)
        {
            Ma_Contrarecibos ma = _dalContra.GetByID(x=> x.Id_TipoCR == tipo && x.Id_FolioCR == folio);
            if (ma.Impreso_CH == true) // si ya tiene poliza de pagado
            {
                _dalSP.PA_Cancelacion_Cheque_GCyFR(tipo, folio, usuario, Fecha);
            }

            //CAMBIAR ESTATUS DEL CR A CANCELADO
            ma.Id_EstatusCR = Diccionarios.Valores_Estatus_CR.Cancelado;
            _dalContra.Save();

            //ELIMINAR DETALLES DEL CR SI TIENE Y TMB LAS FACTURAS
            List<De_Contrarecibos> de = _dalDeContra.Get(m => m.Id_TipoCR == tipo && m.Id_FolioCR == folio).ToList();
            foreach (var item in de)
            {
                _dalDeContra.Delete(m => m.Id_TipoCR == item.Id_TipoCR && m.Id_FolioCR == item.Id_FolioCR && m.Id_Registro == item.Id_Registro);
            }
            _dalDeContra.Save();

            List<De_CR_Facturas> def = _dalFacturas.Get(m => m.Id_TipoCR == tipo && m.Id_FolioCR == folio).ToList();
            foreach (var item in def)
            {
                _dalFacturas.Delete(m => m.Id_TipoCR == item.Id_TipoCR && m.Id_FolioCR == item.Id_FolioCR && m.Id_Proveedor == item.Id_Proveedor && m.Id_Factura == item.Id_Factura);
            }
            _dalFacturas.Save();

        }


        public decimal? cargosDetails(byte Tipo, int Folio)
        {
            try
            {
                return _dalDeContra.Get(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio && x.Id_Movimiento == 1).Sum(m => m.Importe);
            }
            catch
            {
                return 0;
            }
        }

        public decimal? ImporteOriginal(byte Tipo, int Folio)
        {
            try
            {
                return _dalDeContra.Get(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio && x.Id_Movimiento == 1 && x.No_Comprobacion == null).Sum(m => m.Importe);
            }
            catch
            {
                return 0;
            }
        }

        //---------------------------------------Calculos ---------------------------------------------//

        public decimal? Calcula_Reintegros(byte Tipo, int Folio, int? FolioGC)
        {
            //Calculo de los Detalles
            decimal? reintego = _dalContra.GetByID(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio).Cargos;
            decimal? cargosdetalles = _dalDeContra.Get(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio && x.Id_Movimiento == 1).Sum(x => x.Importe);
            decimal? cargosComprobaciones = 0;
            if (FolioGC.HasValue)
                if (_dalDeCompro.Get(x => x.Id_FolioGC == FolioGC && x.Id_FolioPoliza_C == null).Count() > 0)
                    cargosComprobaciones = _dalDeCompro.Get(x => x.Id_FolioGC == FolioGC && x.Id_FolioPoliza_C == null).Sum(x => x.Importe);
            if (cargosdetalles == null) cargosdetalles = 0;
            if (cargosComprobaciones == null) cargosComprobaciones = 0;
            decimal? suma = cargosComprobaciones + cargosdetalles;
            if (suma == null) suma = 0;
            return reintego - suma;
        }

        public decimal? Calcula_TotalReintegros(byte Tipo, int Folio, int? FolioGC)
        {
            decimal? cargosdetalles = _dalDeContra.Get(m=> m.Id_TipoCR == Tipo && m.Id_FolioCR == Folio && m.Id_Movimiento == 1 && m.No_Comprobacion != null).Sum(m=>m.Importe);
            decimal? cargosComprobaciones = 0;
            if (FolioGC.HasValue)
                if (_dalDeCompro.Get(m => m.Id_FolioGC == FolioGC && m.Id_FolioPoliza_C == null).ToList().Count() > 0)
                    cargosComprobaciones =  _dalDeCompro.Get(m=> m.Id_FolioGC == FolioGC && m.Id_FolioPoliza_C == null).Sum(m=> m.Importe);
            if (cargosdetalles == null) cargosdetalles = 0;
            if (cargosComprobaciones == null) cargosComprobaciones = 0;
            return cargosComprobaciones + cargosdetalles;

        }


        //------------------------------------------------------------------------------------------------//
        public void GuardarDatosSobrantesReintegros(byte tipo, int folio, DateTime FechaCierre, decimal? Importe_Reintegro, decimal? Importe_Sobrante, byte? Mes, int? Id_FolioPO_Comprometido, int? Id_FolioPO_Devengado, int? Id_FolioPO_Ejercido, int? Id_FolioPO_Pagado, int? Id_FolioPolizaCR, short? Usuario)
        {
            //Referenciando Para guardar
            Ma_Comprobaciones compro = new Ma_Comprobaciones();
            compro.Id_FolioGC = this.getFolioGC(); 
            compro.Importe_Reintegro = Importe_Reintegro;
            compro.Importe_Sobrante = Importe_Sobrante;
            compro.Id_MesPO_Comprometido = Mes;
            compro.Id_FolioPO_Comprometido = Id_FolioPO_Comprometido;
            compro.Id_MesPO_Devengado = Mes;
            compro.Id_FolioPO_Devengado = Id_FolioPO_Devengado;
            compro.Usuario_Act = Usuario;
            compro.Fecha_Act = DateTime.Now;
            _dalCompro.Insert(compro);
            _dalCompro.Save();

            //Se vincula con el Ma Contrarecibos
            Ma_Contrarecibos maestro = _dalContra.GetByID(x=> x.Id_TipoCR == tipo && x.Id_FolioCR == folio);
            maestro.Id_MesPO_Ejercido = Mes;
            maestro.Id_FolioPO_Ejercido = Id_FolioPO_Ejercido;
            maestro.Id_MesPO_Pagado = Mes;
            maestro.Id_FolioPO_Pagado = Id_FolioPO_Pagado;
            maestro.Id_FolioGC = compro.Id_FolioGC;
            maestro.Estatus_GC = 2;
            maestro.FechaCierreGC = FechaCierre;
            maestro.Id_MesPolizaCR = Mes;
            maestro.Id_FolioPolizaCR = Id_FolioPolizaCR;
            _dalContra.Save();
        }

        public void Pa_Reintegros(int FolioGC, string Cuenta, decimal Importe, DateTime Fecha, short usuario, int? Recibo, short? CtaBancaria)
        {
            Ma_Contrarecibos macontra = _dalContra.GetByID(x=> x.Id_FolioGC == FolioGC);
            string [] resul = _dalSP.PA_Genera_Poliza_Diario_Reintegro(macontra.Id_TipoCR, macontra.Id_FolioCR, Cuenta, Importe, Fecha, usuario);
            De_Comprobaciones de = new De_Comprobaciones();
            de.Id_FolioGC = FolioGC;
            de.Id_Consecutivo = this.getConsecutivo(FolioGC);
            de.Importe = Importe;
            de.Fecha = DateTime.Now;
            de.Id_TipoPoliza = 1;
            de.Id_FolioPoliza = Convert.ToInt32(resul[1]);
            de.Id_MesPoliza = Convert.ToByte(resul[0]);
            de.Id_ReciboIng = Recibo;
            de.Fecha_ReciboIng = Fecha;
            de.Id_CuentaBancaria = CtaBancaria;
            de.Id_Cuenta = Cuenta;
            de.Usuario_Act = usuario;
            de.Fecha_Act = DateTime.Now;
            _dalDeCompro.Insert(de);
            _dalDeCompro.Save();
        }

        public bool Pa_Reintegros2(byte tipo, int folio, short no_comprobacion, DateTime fecha, short usu)
        {
            try
            {
                string[] result = _dalSP.PA_Genera_Polizas_GC_FR_Comprobaciones(tipo, folio, no_comprobacion, fecha, usu);
                Ma_Comprobaciones_Partida macom = new Ma_Comprobaciones_Partida();
                macom.Id_TipoCR = tipo;
                macom.Id_FolioCR = folio;
                macom.No_Comprobacion = no_comprobacion;
                macom.Fecha = fecha;
                macom.Id_MesPolizaCR = Convert.ToByte(result[0]);
                macom.Id_FolioPolizaCR = Convert.ToInt32(result[1]);
                macom.Id_MesPO_Pagado = Convert.ToByte(result[0]);
                macom.Id_FolioPO_Pagado = Convert.ToInt32(result[2]);
                macom.Id_MesPO_Ejercido = Convert.ToByte(result[0]);
                macom.Id_FolioPO_Ejercido = Convert.ToInt32(result[3]);
                macom.Id_MesPO_Devengado = Convert.ToByte(result[0]);
                macom.Id_FolioPO_Devengado = Convert.ToInt32(result[4]);
                macom.Id_MesPO_Comprometido = Convert.ToByte(result[0]);
                macom.Id_FolioPO_Comprometido = Convert.ToInt32(result[5]);
                macom.Usuario_Act = usu;
                macom.Fecha_Act = DateTime.Now;
                _dalComproPartidas.Insert(macom);
                _dalComproPartidas.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public decimal? ReintegrosSobrantes(byte tipo, int folio)
        {

            decimal? Cargos = _dalContra.GetByID(x=> x.Id_TipoCR == tipo && x.Id_FolioCR == folio).Cargos;
            decimal? DeCargos = _dalDeContra.Get(x => x.Id_TipoCR == tipo && x.Id_FolioCR == folio && x.Id_Movimiento == Diccionarios.ValorMovimientos.CARGO && x.No_Comprobacion == null).Sum(x => x.Importe);
            if (Cargos == null) Cargos = 0;
            if (DeCargos == null) DeCargos = 0;
            return Cargos - DeCargos;
        }

        public bool isAdd(string IdCuenta)
        {
            if (Convert.ToBoolean(dalParametros.GetByID(x => x.Nombre == "Con_Saldo_GCFR").Valor))
                return true;
            else
            {
                CA_Cuentas cta = dalCuentas.GetByID(x => x.Id_Cuenta == IdCuenta);
                if ((cta.Cargo_Final - cta.Abono_Final) > 0)
                    return false;
            }
            return true;
        }

        public bool VerificaNoComprobacion(byte Tipo, int Folio, short NoComprobacion)
        {
            try
            {
                Ma_Comprobaciones_Partida ma = _dalComproPartidas.GetByID(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio && x.No_Comprobacion == NoComprobacion);
                if (ma != null)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool CancelacionPolizaReintegro(byte tipo, int folio, DateTime FechaC, short usuario, int Cosecutivo)
        {
            try
            {
                int? FolioGC = _dalContra.GetByID(m => m.Id_TipoCR == tipo && m.Id_FolioCR == folio).Id_FolioGC;
                De_Comprobaciones de = _dalDeCompro.GetByID(m => m.Id_FolioGC == FolioGC && m.Id_Consecutivo == Cosecutivo);
                string cuenta = _dalDePolizas.GetByID(m => m.Id_TipoPoliza == de.Id_TipoPoliza && m.Id_FolioPoliza == de.Id_FolioPoliza && m.Id_MesPoliza == de.Id_MesPoliza && m.Id_Movimiento == 1).Id_Cuenta;
                decimal? Importe = _dalDeCompro.GetByID(m => m.Id_FolioGC == FolioGC && m.Id_Consecutivo == Cosecutivo).Importe;
                string[] resultado = _dalSP.PA_Genera_Poliza_Diario_Reintegro_Cancela(tipo, folio, cuenta, Importe.Value, FechaC, usuario);
                de.Id_TipoPoliza_C = Convert.ToByte(resultado[0]);
                de.Id_MesPoliza_C = Convert.ToByte(resultado[1]);
                de.Id_FolioPoliza_C = Convert.ToInt32(resultado[2]);
                _dalDeCompro.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class Fondos_GastosDetallesBL
    {
        private MaContrarecibosDAL _dalContra;
        private DeContrarecibosDAL _dalDeContra;
        private MaComprobacionesPartidasDAL _dalComprobacionPartida;

        public Fondos_GastosDetallesBL()
        {
            if (_dalContra == null) _dalContra = new MaContrarecibosDAL();
            if (_dalDeContra == null) _dalDeContra = new DeContrarecibosDAL();
            if (_dalComprobacionPartida == null) _dalComprobacionPartida = new MaComprobacionesPartidasDAL();
        }

        public void AddBoton(ref List<object> botonera, string btn)
        {
            botonera.Add(btn);
        }

        public void CreateBotonera(ref List<object> Botones, byte IdTipoCR, int IdFolio)
        {
            if (this.isNew(IdTipoCR, IdFolio)) this.AddBoton(ref Botones, "bNuevo");
            if (this.isDelete(IdTipoCR, IdFolio)) this.AddBoton(ref Botones, "bEliminar");
            if (this.isSaldar(IdTipoCR, IdFolio)) this.AddBoton(ref Botones, "bSaldar");
            if (this.isExit(IdTipoCR, IdFolio, null)) this.AddBoton(ref Botones, "bSalir");
        }

        public void CreateBotoneraComprobacion(ref List<object> Botones, byte IdTipoCR, int IdFolio, short NoComprobacion)
        {
            if (this.isNewComprobation(IdTipoCR, IdFolio, NoComprobacion)) this.AddBoton(ref Botones, "bNuevo");
            if (this.isDeleteComprobation(IdTipoCR, IdFolio, NoComprobacion)) this.AddBoton(ref Botones, "bEliminar");
            if (this.isSaldarComprobation(IdTipoCR, IdFolio, NoComprobacion)) this.AddBoton(ref Botones, "bSaldar");
            if (this.isExit(IdTipoCR, IdFolio, NoComprobacion)) this.AddBoton(ref Botones, "bSalir");
        }

        public bool isNew(byte Tipo, int Folio)
        {
            Ma_Contrarecibos ma = _dalContra.GetByID(x=> x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio);
            if (ma.Estatus_GC == 2) return false;
            return true;
        }

        public bool isNewComprobation(byte Tipo, int Folio, short NoComprobacion)
        {
            if (hasComprobacionPartida(Tipo, Folio, NoComprobacion)) return false;
            if (!hasDetails(Tipo, Folio, NoComprobacion)) return true;
            if (equalImport(Tipo, Folio, NoComprobacion)) return false;
            return true;
        }

        public bool isDelete(byte Tipo, int Folio)
        {
            Ma_Contrarecibos ma = _dalContra.GetByID(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio);
            if (!hasDetails(Tipo, Folio, null)) return false;
            if (ma.Estatus_GC == 2) return false;
            return true;
        }

        public bool isDeleteComprobation(byte Tipo, int Folio, short NoComprobacion)
        {
            if (hasComprobacionPartida(Tipo, Folio, NoComprobacion)) return false;
            if (!hasDetails(Tipo, Folio, NoComprobacion)) return false;
            return true;
        }


        public bool isSaldar(byte Tipo, int Folio)
        {
            Ma_Contrarecibos ma = _dalContra.GetByID(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio);
            if (ma.Estatus_GC == 2) return false;
            if (!hasDetails(Tipo, Folio, null)) return false;
            if (equalImport(Tipo, Folio, null)) return false; else return true;
        }

        public bool isSaldarComprobation(byte Tipo, int Folio, short NoComprobacion)
        {
            Ma_Contrarecibos ma = _dalContra.GetByID(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio);
            if (hasComprobacionPartida(Tipo, Folio, NoComprobacion)) return false;
            if (!hasDetails(Tipo, Folio, NoComprobacion)) return false;
            if (equalImport(Tipo, Folio, NoComprobacion)) return false; else return true;
        }

        public bool isExit(byte Tipo, int Folio, short? NoComprobacion)
        {
            if (!hasDetails(Tipo, Folio, NoComprobacion)) return true;
            if (equalImport(Tipo, Folio, NoComprobacion)) return true;
            return false;
        }


        public short? getNoComprobacion(byte Tipo,int Folio)
        {
            try
            {
                short? c = _dalDeContra.Get(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio).Max(x => x.No_Comprobacion);
                if (c == null) c = 0;
                c++;
                return c;
            }
            catch
            {
                return 1;
            }
        }

        public bool hasComprobacionPartida(byte Tipo, int Folio, short NoComprobacion)
        {
            if (_dalComprobacionPartida.Get(x=> x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio && x.No_Comprobacion == NoComprobacion).Count() > 0) return true; else return false;
        }

        public bool hasDetails(byte Tipo, int Folio, short? NoComprobacion)
        {
            if (_dalDeContra.Get(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio && (NoComprobacion.HasValue ? x.No_Comprobacion == NoComprobacion : x.No_Comprobacion == null)).Count() > 0) return true; else return false;
        }

        public bool equalImport(byte Tipo, int Folio, short? NoComprobacion)
        {
            decimal? cargos = _dalDeContra.Get(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio && (NoComprobacion.HasValue ? x.No_Comprobacion == NoComprobacion : x.No_Comprobacion == null) && x.Id_Movimiento == Diccionarios.ValorMovimientos.CARGO).Sum(x => x.Importe);
            decimal? abonos = _dalDeContra.Get(x => x.Id_TipoCR == Tipo && x.Id_FolioCR == Folio && (NoComprobacion.HasValue ? x.No_Comprobacion == NoComprobacion : x.No_Comprobacion == null) && x.Id_Movimiento == Diccionarios.ValorMovimientos.ABONO).Sum(x => x.Importe);
            if (cargos == abonos) return true; else return false;
        }
    }

    public class ComprobacionesBL
    {

    }


}