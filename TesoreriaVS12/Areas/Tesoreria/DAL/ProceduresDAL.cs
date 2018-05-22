using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;
using TesoreriaVS12.Models;
using System.Data.Objects;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos
{

    public class ProceduresDAL : DALGeneric
    {
        public string[] Pa_Genera_PolizaOrden_Comprometido(short? TipoCompromiso, int? FolioCompromiso, short? usuario)
        {
            ObjectParameter Mes = new ObjectParameter("id_MesPol", typeof(int));
            ObjectParameter FolioPoliza = new ObjectParameter("id_FolioPol", typeof(int));
            int i =Db.Pa_Genera_PolizaOrden_Comprometido(TipoCompromiso, FolioCompromiso, usuario, Mes, FolioPoliza);
            return new string[] { Mes.Value.ToString(), FolioPoliza.Value.ToString() };
        }

        public string[] Pa_Genera_PolizaOrden_Devengado(short? TipoCompromiso, int? FolioCompromiso, short? usuario)
        {
            ObjectParameter Mes = new ObjectParameter("id_MesPol", typeof(int));
            ObjectParameter FolioPoliza = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Devengado(TipoCompromiso, FolioCompromiso, usuario, Mes, FolioPoliza);
            return new string[] { Mes.Value.ToString(), FolioPoliza.Value.ToString() };
        }

        public string[] PA_Genera_Poliza_Diario_CR(short? TipoCompromiso, int? FolioCompromiso, short? usuario)
        {
            ObjectParameter Mes = new ObjectParameter("id_MesPol", typeof(int));
            ObjectParameter FolioPoliza = new ObjectParameter("id_FolioPol", typeof(int));
            Db.PA_Genera_Poliza_Diario_CR(TipoCompromiso, FolioCompromiso, usuario, Mes, FolioPoliza);
            return new string[] { Mes.Value.ToString(), FolioPoliza.Value.ToString() };
        }


        public int PA_Estado_Cuenta_Banco(string IdCuentaBancaria)
        {
            ObjectParameter cargos = new ObjectParameter("cargos", typeof(int));
            ObjectParameter abonos = new ObjectParameter("abonos", typeof(int));
            ObjectParameter saldo = new ObjectParameter("saldo", typeof(int));
            short? mes = (short?)DateTime.Now.Month;
            Db.PA_Estado_Cuenta_Banco(IdCuentaBancaria.ToString(), mes, cargos, abonos, saldo);
            return Convert.ToInt32(saldo.Value);
        }
        public int Pa_Asigna_Cheque(byte? Id_TipoCR, Int32? Id_FolioCR, short? IdCuentaBancaria, int NoCheque,short? idUsuario)
        {
            ObjectParameter No_Cheque = new ObjectParameter("No_Cheque", typeof(int));
            No_Cheque.Value = NoCheque;
            Db.Pa_Asigna_Cheque(Id_TipoCR, Id_FolioCR, IdCuentaBancaria, idUsuario, No_Cheque);
            return Convert.ToInt32(No_Cheque.Value);
        }
        public int Pa_Asigna_Cheque_TE(byte? Id_TipoCR, Int32? Id_FolioCR, short? IdCuentaBancaria, short? idUsuario,DateTime? FechaPago)
        {
            ObjectParameter No_Cheque = new ObjectParameter("No_Cheque", typeof(int));
            No_Cheque.Value = 0;
            Db.Pa_Asigna_Cheque_TE(Id_TipoCR, Id_FolioCR, IdCuentaBancaria, idUsuario,FechaPago,No_Cheque);
            return Convert.ToInt32(No_Cheque.Value);
        }
        public void PA_DesasignaciondeCheques(short IdCuenta)
        {
            ObjectParameter w_Leyenda = new ObjectParameter("w_Leyenda", typeof(string));
            Db.PA_DesasignaciondeCheques(IdCuenta, w_Leyenda);
        }
        public void PA_DesasignaciondeCheques_Individual(short IdCuenta, int? NoCheque)
        {
            ObjectParameter w_Leyenda = new ObjectParameter("w_Leyenda", typeof(string));
            Db.PA_DesasignaciondeCheques_Individual(IdCuenta, NoCheque.Value, w_Leyenda);
        }
        public void Pa_Genera_PolizaOrden_Devengado_Cancela(short? id_tipoCompromiso,int? Id_FolioCompromiso, DateTime? FechaC,short? usuario,ref byte id_mes_d, ref int id_folio_d)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(int));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Devengado_Cancela(id_tipoCompromiso,Id_FolioCompromiso,FechaC,usuario,id_MesPol,id_FolioPol);
            id_mes_d = Convert.ToByte(id_MesPol.Value);
            id_folio_d = Convert.ToInt32(id_FolioPol.Value);
        }

        public void Pa_Genera_PolizaOrden_DevengadoIng_Cancela(int? Folio,DateTime? Fecha, short? uAct, ref byte id_mes_d, ref int id_folio_d)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(int));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Devengado_Ing_CANCELA(Folio,Fecha,uAct,id_MesPol, id_FolioPol);
            id_mes_d = Convert.ToByte(id_MesPol.Value);
            id_folio_d = Convert.ToInt32(id_FolioPol.Value);
        }

        public void Pa_Genera_PolizaOrden_Recaudado_Cancela(int? Folio,DateTime? Fecha, short? uAct, ref byte id_mes_d, ref int id_folio_d)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(int));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Recaudado_Cancela(Folio, Fecha, uAct, id_MesPol, id_FolioPol);
            id_mes_d = Convert.ToByte(id_MesPol.Value);
            id_folio_d = Convert.ToInt32(id_FolioPol.Value);
        }

        public void PA_Genera_Polizas_DiarioIngresos_Cancela(int? Folio,short?uAct, DateTime FechaCancela, ref byte id_mes_d, ref int id_folio_d)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(int));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.PA_Genera_Polizas_DiarioIngresos_CANCELA(Folio,FechaCancela, uAct,id_MesPol, id_FolioPol);
            id_mes_d = Convert.ToByte(id_MesPol.Value);
            id_folio_d = Convert.ToInt32(id_FolioPol.Value);
        }

        public void PA_Genera_PolizaIngresos_Cancela(int? Folio, short? uAct, DateTime FechaCancela, ref byte id_mes_d, ref int id_folio_d)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(int));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.PA_Genera_PolizaIngresos_Cancela(Folio,FechaCancela, uAct,id_MesPol, id_FolioPol);
            id_mes_d = Convert.ToByte(id_MesPol.Value);
            id_folio_d = Convert.ToInt32(id_FolioPol.Value);
        }

        /*Nuevos*/

        public void PA_Genera_Polizas_CANCELA_ReciboIng(int? Folio, short? uAct, DateTime FechaCancela, ref byte id_mes_d, ref int id_folio_d)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(int));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.PA_Genera_Polizas_CANCELA_ReciboIng(Folio, FechaCancela, uAct, id_MesPol, id_FolioPol);
            id_mes_d = Convert.ToByte(id_MesPol.Value);
            id_folio_d = Convert.ToInt32(id_FolioPol.Value);
        }

        public void Pa_Genera_PolizaOrden_Comprometido_Cancela(short? id_tipoCompromiso,int? Id_FolioCompromiso, DateTime? FechaC,short? usuario,ref byte id_mes_c, ref int id_folio_c)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(int));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Comprometido_Cancela(id_tipoCompromiso,Id_FolioCompromiso,FechaC,usuario,id_MesPol,id_FolioPol);
            id_mes_c = Convert.ToByte(id_MesPol.Value);
            id_folio_c = Convert.ToInt32(id_FolioPol.Value);
        }
        public void Pa_Genera_PolizaOrden_Aprobado(string Id_ObjetoG, decimal? Importe, DateTime FechaAprobado, ref byte Id_MesPol, ref int Id_FolioP, short? Id_Usuario)
        {
            ObjectParameter id_MesPol = new ObjectParameter("Id_MesPol", typeof(int));
            ObjectParameter id_FolioPol = new ObjectParameter("Id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Aprobado(Id_ObjetoG, Importe, FechaAprobado, Id_Usuario, id_MesPol, id_FolioPol);
            Id_MesPol = Convert.ToByte(id_MesPol.Value);
            Id_FolioP = Convert.ToInt32(id_FolioPol.Value);
        }
        public void Pa_Genera_PolizaOrden_Estimado(string Id_Concepto, decimal? Importe, DateTime FechaEstimado, ref byte Id_MesPol, ref int Id_FolioP, short? Id_Usuario)
        {
            ObjectParameter id_MesPol = new ObjectParameter("Id_MesPol", typeof(int));
            ObjectParameter id_FolioPol = new ObjectParameter("Id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Estimado(Id_Concepto, Importe, FechaEstimado, Id_Usuario, id_MesPol, id_FolioPol);
            Id_MesPol = Convert.ToByte(id_MesPol.Value);
            Id_FolioP = Convert.ToInt32(id_FolioPol.Value);
        }
        public void Pa_Cancelacion_Poliza_Diario_Devengo(short? id_tipoCompromiso, int? Id_FolioCompromiso, DateTime? FechaC, short? usuario, ref byte id_mes_d, ref int id_folio_d)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPolD", typeof(int));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPolD", typeof(int));
            Db.PA_Cancelacion_Poliza_Diario_Devengo(id_tipoCompromiso, Id_FolioCompromiso, FechaC, usuario, id_MesPol, id_FolioPol);
            id_mes_d = Convert.ToByte(id_MesPol.Value);
            id_folio_d = Convert.ToInt32(id_FolioPol.Value);
        }

        public void Pa_Genera_PolizaOrden_Ejercido_Compromiso_Cancela(byte? TipoCR,int? FolioCR, DateTime? FechaCancela, short? IdUsuario,ref byte IdMes, ref int IdFolio)
        { 
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(byte));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Ejercido_Compromiso_Cancela(TipoCR,FolioCR, FechaCancela, IdUsuario,id_MesPol, id_FolioPol);
            IdMes = Convert.ToByte(id_MesPol.Value);
            IdFolio = Convert.ToInt32(id_FolioPol.Value);
        }
        public void Pa_Genera_PolizaOrden_Pagado_Cancela(byte? TipoCR, int? FolioCR, DateTime? FechaCancela, short? IdUsuario, ref byte IdMes, ref int IdFolio)
        { 
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(byte));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Ejercido_Compromiso_Cancela(TipoCR,FolioCR, FechaCancela, IdUsuario,id_MesPol, id_FolioPol);
            IdMes = Convert.ToByte(id_MesPol.Value);
            IdFolio = Convert.ToInt32(id_FolioPol.Value);
        }
        public void Pa_Genera_PoilizaOrden_Ejercido_Compromiso(byte? TipoCR, int? FolioCR,short? IdUsuario, ref byte IdMes, ref int IdFolio)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(byte));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Ejercido_Compromiso(TipoCR, FolioCR,IdUsuario, id_MesPol, id_FolioPol);
            IdMes = Convert.ToByte(id_MesPol.Value);
            IdFolio = Convert.ToInt32(id_FolioPol.Value);
        }
        public bool PA_VerificaCierreTransferencia(int? idTrans)
        {
            ObjectParameter mescerrado = new ObjectParameter("mescerrado", typeof(bool));
            Db.PA_VerificaCierreTransferencia(idTrans,mescerrado);
            return Convert.ToBoolean(mescerrado.Value);
		}
        public void Pa_Genera_PolizaOrden_Pagado(byte? Id_TipoCR, int? Id_FolioCR, short? Id_Usuario, ref int Id_MesPoliza, ref int Id_FolioPoliza)
        {
            ObjectParameter id_MesPol = new ObjectParameter("Id_MesPol", typeof(int));
            ObjectParameter id_FolioPol = new ObjectParameter("Id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Pagado(Id_TipoCR, Id_FolioCR, Id_Usuario, id_MesPol,id_FolioPol);
            Id_MesPoliza = Convert.ToByte(id_MesPol.Value);
            Id_FolioPoliza = Convert.ToInt32(id_FolioPol.Value);
        }

        public decimal[] PA_BalanzaPorCuenta(string IdCuenta, byte? MesIni, byte? MesFin)
        {
            ObjectParameter SaldoAnteriorDebe = new ObjectParameter("SaldoAnteriorDebe", typeof(string));
            ObjectParameter SaldoAnteriorHaber = new ObjectParameter("SaldoAnteriorHaber", typeof(string));
            ObjectParameter MovimientoDebe = new ObjectParameter("MovimientoDebe", typeof(string));
            ObjectParameter MovimientoHaber = new ObjectParameter("MovimientoHaber", typeof(string));
            ObjectParameter SaldoActualDebe = new ObjectParameter("SaldoActualDebe", typeof(string));
            ObjectParameter SaldoActualHaber = new ObjectParameter("SaldoActualHaber", typeof(string));
            Db.PA_BalanzaPorCuenta(IdCuenta, MesIni, MesFin, SaldoAnteriorDebe, SaldoAnteriorHaber, MovimientoDebe, MovimientoHaber, SaldoActualDebe, SaldoActualHaber);
            return new decimal[] { 
                Convert.ToDecimal(SaldoAnteriorDebe.Value), 
                Convert.ToDecimal(SaldoAnteriorHaber.Value), 
                Convert.ToDecimal(MovimientoDebe.Value), 
                Convert.ToDecimal(MovimientoHaber.Value), 
                Convert.ToDecimal(SaldoActualDebe.Value), 
                Convert.ToDecimal(SaldoActualHaber.Value)
            };
        }
        public void Pa_AutorizarPartidasCompromiso(short Id_TipoCompromiso, int? Id_FolioCompromiso, short? Id_Usuario)
        {
            Db.Pa_AutorizarPartidasCompromiso(Id_TipoCompromiso, Id_FolioCompromiso, Id_Usuario);
        }

        public int Pa_Genera_FolioPoliza(byte? Id_TipoPol,byte? Id_MesPol)
        {
            ObjectParameter Id_FolioPol= new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_FolioPoliza(Id_TipoPol, Id_MesPol, Id_FolioPol);
            return Convert.ToInt32(Id_FolioPol.Value);
        }

        public void PA_Cancelar_Compromiso(short? Id_TipoCompromiso, int? Id_FolioCompromiso, short? Id_Usuario)
        {
            Db.PA_Cancelar_Compromiso(Id_TipoCompromiso, Id_FolioCompromiso, Id_Usuario);
        }
        public void Pa_Genera_PolizaOrden_Modificado_Amp(int? Id_Transferencia, DateTime? Fecha, short? Id_Usuario,ref byte IdMes, ref int IdFolio)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(byte));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Modificado_Amp(Id_Transferencia,Fecha,Id_Usuario,id_MesPol,id_FolioPol);
            IdMes = Convert.ToByte(id_MesPol.Value);
            IdFolio = Convert.ToInt32(id_FolioPol.Value);
        }
        public void Pa_Genera_PolizaOrden_Modificado_Red(int? Id_Transferencia, DateTime? Fecha, short? Id_Usuario, ref byte IdMes, ref int IdFolio)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(byte));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Modificado_Red(Id_Transferencia, Fecha, Id_Usuario, id_MesPol, id_FolioPol);
            IdMes = Convert.ToByte(id_MesPol.Value);
            IdFolio = Convert.ToInt32(id_FolioPol.Value);
        }
        public void Pa_Genera_PolizaOrden_Modificado_Amp_Cancela(int? Id_Transferencia, DateTime? Fecha, short? Id_Usuario, ref byte IdMes, ref int IdFolio)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(byte));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Modificado_Amp_Cancela(Id_Transferencia, Fecha, Id_Usuario, id_MesPol, id_FolioPol);
            IdMes = Convert.ToByte(id_MesPol.Value);
            IdFolio = Convert.ToInt32(id_FolioPol.Value);
        }
        public void Pa_Genera_PolizaOrden_Modificado_Red_Cancela(int? Id_Transferencia, DateTime? Fecha, short? Id_Usuario, ref byte IdMes, ref int IdFolio)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(byte));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Modificado_Red_Cancela(Id_Transferencia, Fecha, Id_Usuario, id_MesPol, id_FolioPol);
            IdMes = Convert.ToByte(id_MesPol.Value);
            IdFolio = Convert.ToInt32(id_FolioPol.Value);
		}

        public int PA_ActualizaPoliza_Orden_Comprometido(short? Id_TipoCompromiso, int? Id_FolioCompromiso, short? Id_Usuario)
        {
            return Db.PA_ActualizaPoliza_Orden_Comprometido(Id_TipoCompromiso, Id_FolioCompromiso, Id_Usuario);
        }

        public string PA_FechaCompromiso(short? IdCompromiso, string fechaCad)
        {
            ObjectParameter fechaVencimiento = new ObjectParameter("fechaVencimiento", typeof(string));
            Db.PA_FechaCompromiso(IdCompromiso, fechaCad, fechaVencimiento);
            return fechaVencimiento.Value.ToString();
        }
        public decimal[] PA_Genera_Detalle_Transferencia_Mensual(Int32? IdTransferencia, byte? mes, string centroGestor, Int32? id_OGInicial, Int32? id_OGFinal, short? Id_Usuario)
        {
            ObjectParameter totalRequerido = new ObjectParameter("totalRequerido", typeof(int));
            ObjectParameter totalACubrir = new ObjectParameter("totalACubrir", typeof(int));
            Db.PA_Genera_Detalle_Transferencia_Mensual(IdTransferencia,mes,centroGestor,id_OGInicial,id_OGFinal,Id_Usuario,totalRequerido,totalACubrir);
            return new decimal[] { 
                Convert.ToDecimal(totalRequerido.Value), 
                Convert.ToDecimal(totalACubrir.Value)
            };
        }
        public decimal[] PA_Genera_Detalle_Transferencia_EntreMeses(Int32? IdTransferencia, byte? mesOrigen, byte? mesDestino, string centroGestor, string id_OGInicial, string id_OGFinal, short? Id_Usuario)
        {
            ObjectParameter totalRequerido = new ObjectParameter("totalRequerido", typeof(decimal));
            ObjectParameter totalACubrir = new ObjectParameter("totalACubrir", typeof(decimal));
            ObjectParameter error = new ObjectParameter("error", typeof(bool));
            ObjectParameter mensaje = new ObjectParameter("mensaje", typeof(string));
            Db.PA_Genera_Detalle_Transferencia_EntreMeses(IdTransferencia, mesOrigen, mesDestino, centroGestor, id_OGInicial, id_OGFinal, Id_Usuario, totalRequerido, totalACubrir, error, mensaje);
            return new decimal[] { 
                Convert.ToDecimal(totalRequerido.Value), 
                Convert.ToDecimal(totalACubrir.Value)
            };
        }
        public void PA_Genera_PolizasManuales(byte TipoPoliza, int FolioPoliza, byte MesPoliza, short uAct)
        {
            Db.PA_Genera_Polizas_Manual(TipoPoliza, FolioPoliza, MesPoliza, uAct);
        }
        public void PA_Genera_PolizasManualesC(byte TipoPoliza, int FolioPoliza, byte MesPoliza, short uAct, DateTime fCancela)
        {
            Db.PA_Genera_Polizas_Manual_Cancela(TipoPoliza, FolioPoliza, MesPoliza, uAct);
            //Db.PA_Genera_Polizas_Manual(TipoPoliza, FolioPoliza, MesPoliza, uAct);
        }
        public List<Chequestbl> PA_Generar_Cheques(short? usuario, short? chequeImpreso, short Id_CtaBancaria, string fechaVencimiento, string fechaPago, short? noCheques, short? formato)
        {
           return Db.PA_Generar_Cheques(usuario, chequeImpreso, Id_CtaBancaria, fechaVencimiento, fechaPago, noCheques, formato).ToList<Chequestbl>();    
        }
        public int[] Pa_Genera_PolizaOrden_Modificado_Amp_Ing(int? Id_Transferencia, DateTime? Fecha, short? Id_Usuario)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(byte));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_ModificadoIng(Id_Transferencia, Fecha, Id_Usuario, id_MesPol, id_FolioPol);
            return new int[] { 
                Convert.ToInt32(id_MesPol.Value), 
                Convert.ToInt32(id_FolioPol.Value)
            };
        }
        public int[] Pa_Genera_PolizaOrden_Modificado_Red_Ing(int? Id_Transferencia, DateTime? Fecha, short? Id_Usuario)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(byte));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_ModificadoREDIng(Id_Transferencia, Fecha, Id_Usuario, id_MesPol, id_FolioPol);
            return new int[] { 
                Convert.ToInt32(id_MesPol.Value), 
                Convert.ToInt32(id_FolioPol.Value)
            };
        }
        public int[] Pa_Genera_PolizaOrden_Modificado_Amp_Cancela_Ing(int? Id_Transferencia, DateTime? Fecha, short? Id_Usuario)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(byte));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_ModificadoIng_Cancela(Id_Transferencia, Fecha, Id_Usuario, id_MesPol, id_FolioPol);
            return new int[] { 
                Convert.ToInt32(id_MesPol.Value), 
                Convert.ToInt32(id_FolioPol.Value)
            };
        }
        public int[] Pa_Genera_PolizaOrden_Modificado_Red_Cancela_Ing(int? Id_Transferencia, DateTime? Fecha, short? Id_Usuario)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(byte));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_ModificadoREDIng_CANCELA(Id_Transferencia, Fecha, Id_Usuario, id_MesPol, id_FolioPol);
            return new int[] { 
                Convert.ToInt32(id_MesPol.Value), 
                Convert.ToInt32(id_FolioPol.Value)
            };
		}
        public string[] PA_Genera_Poliza_Egresos_Cancela(byte TipoCR, int? FolioCR, short? Id_CtaBancaria, int? NoCheque, DateTime? fechaPago, short Usuario)
        {
            ObjectParameter Mes = new ObjectParameter("id_MesPoliza", typeof(byte));
            ObjectParameter FolioPoliza = new ObjectParameter("id_FolioPoliza", typeof(int));
            Db.PA_Genera_Poliza_Egresos_Cancela(TipoCR, FolioCR, Id_CtaBancaria, NoCheque, fechaPago, Usuario, Mes, FolioPoliza);
            return new string[] { 
                Mes.Value.ToString(), 
                FolioPoliza.Value.ToString()
            };
        }
        
        public void PA_Cancelacion_Cheque_GCyFR(byte TipoCR, int FolioCR, short usuario, DateTime? FechaC)
        {
            Db.PA_Cancelacion_Cheque_GCyFR(TipoCR, FolioCR, usuario, FechaC);
        }

        public string[] PA_Genera_Polizas_GC_FR(byte TipoCR, int FolioCR, DateTime FechaC, short usuario)
        {
            ObjectParameter mesR = new ObjectParameter("id_MesPol", typeof(byte));
            ObjectParameter folioR = new ObjectParameter("id_FolioPol", typeof(int));
            ObjectParameter folioPagado = new ObjectParameter("id_FolioPol_Pagado", typeof(byte));
            ObjectParameter folioEjercido = new ObjectParameter("id_FolioPol_Ejercido", typeof(int));
            ObjectParameter folioDevengado = new ObjectParameter("id_FolioPol_Devengado", typeof(int));
            ObjectParameter folioCompromiso = new ObjectParameter("id_FolioPol_Compromiso", typeof(int));
            Db.PA_Genera_Polizas_GC_FR(TipoCR, FolioCR, FechaC, usuario, mesR, folioR, folioPagado, folioEjercido, folioDevengado, folioCompromiso);
            return new string[] { Convert.ToString(mesR.Value), Convert.ToString(folioR.Value), Convert.ToString(folioPagado.Value), Convert.ToString(folioEjercido.Value),
                Convert.ToString(folioDevengado.Value), Convert.ToString(folioCompromiso.Value) };
        }

        public string[] PA_Genera_Poliza_Diario_Reintegro(byte TipoCR, int FolioCR, string Cuenta, decimal Importe, DateTime FechaR, short usuario)
        {
            ObjectParameter mesR = new ObjectParameter("id_MesPoliza", typeof(byte));
            ObjectParameter folioR = new ObjectParameter("id_FolioPoliza", typeof(int));
            Db.PA_Genera_Poliza_Diario_Reintegro(TipoCR, FolioCR, Cuenta, Importe, FechaR, usuario, mesR, folioR);
            return new string[] { Convert.ToString(mesR.Value), Convert.ToString(folioR.Value) };
        }

        public string[] PA_Genera_Poliza_Diario_Reintegro_Cancela(byte TipoCR, int FolioCR, string Cuenta, decimal Importe, DateTime FechaR, short usuario)
        {
            ObjectParameter tipR = new ObjectParameter("id_TipoPoliza", typeof(byte));
            ObjectParameter mesR = new ObjectParameter("id_MesPoliza", typeof(byte));
            ObjectParameter folioR = new ObjectParameter("id_FolioPoliza", typeof(int));
            Db.PA_Genera_Poliza_Diario_Reintegro_Cancela(TipoCR, FolioCR, Cuenta, Importe, FechaR, usuario,tipR, mesR, folioR);
            return new string[] { Convert.ToString(tipR.Value), Convert.ToString(mesR.Value), Convert.ToString(folioR.Value) };
        }

        public string[] PA_Genera_Polizas_GC_FR_Comprobaciones(byte TipoCR, int FolioCR, int NoComprobacion, DateTime FechaC, short usuario)
        {
            ObjectParameter mesR = new ObjectParameter("id_MesPol", typeof(byte));
            ObjectParameter folioR = new ObjectParameter("id_FolioPol", typeof(int));
            ObjectParameter folioPagado = new ObjectParameter("id_FolioPol_Pagado", typeof(byte));
            ObjectParameter folioEjercido = new ObjectParameter("id_FolioPol_Ejercido", typeof(int));
            ObjectParameter folioDevengado = new ObjectParameter("id_FolioPol_Devengado", typeof(int));
            ObjectParameter folioCompromiso = new ObjectParameter("id_FolioPol_Compromiso", typeof(int));
            Db.PA_Genera_Polizas_GC_FR_Comprobaciones(TipoCR, FolioCR, NoComprobacion, FechaC, usuario, mesR, folioR, folioPagado, folioEjercido, folioDevengado, folioCompromiso);
            return new string[] { Convert.ToString(mesR.Value), Convert.ToString(folioR.Value), Convert.ToString(folioPagado.Value), Convert.ToString(folioEjercido.Value),
                Convert.ToString(folioDevengado.Value), Convert.ToString(folioCompromiso.Value) };
        }

        internal void Pa_Genera_PolizaOrden_DevengadoIng(int p1, DateTime FechaRecaudacion, short p2, ref byte Mes, ref int folio)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(int));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Devengado_Ing(p1, FechaRecaudacion, p2, id_MesPol, id_FolioPol);
            Mes = Convert.ToByte(id_MesPol.Value);
            folio = Convert.ToInt32(id_FolioPol.Value);
        }

        internal void Pa_Genera_PolizaOrden_Recaudado(int p1, DateTime FechaRecaudacion, short p2, ref byte Mes, ref int folio)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(int));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.Pa_Genera_PolizaOrden_Recaudado(p1, FechaRecaudacion, p2, id_MesPol, id_FolioPol);
            Mes = Convert.ToByte(id_MesPol.Value);
            folio = Convert.ToInt32(id_FolioPol.Value);
        }

        internal void PA_Genera_PolizaIngresos(int p1, short p2,DateTime FechaRecaudacion, ref byte Mes, ref int folio)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(int));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.PA_Genera_PolizaIngresos(p1,FechaRecaudacion, p2, id_MesPol, id_FolioPol);
            Mes = Convert.ToByte(id_MesPol.Value);
            folio = Convert.ToInt32(id_FolioPol.Value);
        }

        internal void PA_Genera_Polizas_DiarioIngresos(int p1, short p2,DateTime FechaRecaudacion, ref byte Mes, ref int folio)
        {
            ObjectParameter id_MesPol = new ObjectParameter("id_MesPol", typeof(int));
            ObjectParameter id_FolioPol = new ObjectParameter("id_FolioPol", typeof(int));
            Db.PA_Genera_Polizas_DiarioIngresos(p1, FechaRecaudacion, p2, id_MesPol, id_FolioPol);
            Mes = Convert.ToByte(id_MesPol.Value);
            folio = Convert.ToInt32(id_FolioPol.Value);
        }


        public List<tblRepCuentas> PA_ReporteCuentasEjercicios(short? ejercicio1,short? ejercicio2,DateTime? FechaInicio,DateTime? FechaFin)
        {
            try
            {
                return Db.Pa_ReporteCuentasEjercicios(ejercicio1, ejercicio2, FechaInicio, FechaFin).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public void Pa_ConciliaAct(byte? mes, int? idCta, int? Id_Usuario,bool? borrado )
        {
            Db.PA_ConciliaAct(mes, idCta, Id_Usuario, borrado);
        }
        public List<tblRepConciliacion> PA_ConciliacionReporte(int? IdCtaBancaria, int? IdMes)
        {
            try
            {
                return new List<tblRepConciliacion>(); //Db.PA_ConciliacionReporte(IdMes, IdCtaBancaria);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}



