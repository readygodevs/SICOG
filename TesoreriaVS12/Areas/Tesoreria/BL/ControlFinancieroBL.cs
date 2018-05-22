using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Utils;
using TesoreriaVS12.Models;
using TesoreriaVS12.Filters;
using System.Web.Script.Serialization;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.DAL;
using TesoreriaVS12.Areas.Tesoreria.BL;
using System.Configuration;
using System.IO;
using TesoreriaVS12.Areas.Tesoreria.DAL.Vistas;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class ControlFinancieroBL
    {
         protected CierreBancoDAL DALCierreBanco { get; set; }
        protected CuentasBancariasDAL DALCuentasBancarias { get; set; }
        protected ParametrosDAL DALParametros { get; set; }
        protected ProceduresDAL DALProcedures { get; set; }
        protected VW_ConciliacionDAL DALVConciliacion { get; set; }
        protected TipoMovBancariosDAL DALTipoMovBancarios { get; set; }
        protected MaEstadosCuentaDAL DALEstadosCuenta { get; set; }

        protected MaMovimientosConciliacionDAL DALMaMovimientosConciliacion { get; set; }

        public ControlFinancieroBL()
        {
            if (DALCierreBanco == null) DALCierreBanco = new CierreBancoDAL();
            if (DALParametros == null) DALParametros = new ParametrosDAL();
            if (DALProcedures == null) DALProcedures = new ProceduresDAL();
            if (DALVConciliacion == null) DALVConciliacion = new VW_ConciliacionDAL();
            if (DALTipoMovBancarios == null) DALTipoMovBancarios = new TipoMovBancariosDAL();
            if (DALCuentasBancarias == null) DALCuentasBancarias = new CuentasBancariasDAL();
            if (DALMaMovimientosConciliacion == null) DALMaMovimientosConciliacion = new MaMovimientosConciliacionDAL();
            if (DALEstadosCuenta == null) DALEstadosCuenta = new MaEstadosCuentaDAL();
        }
        public void ValidarCuentas(string[] elementos,Int16 mes)
        {
            for (int i = 0; i < elementos.Length - 2; i++)
            {
                Ma_EstadosCuenta temp = new Ma_EstadosCuenta();
                temp.No_CuentaBancaria = elementos[i].Substring(0, 16);
                Ca_CuentasBancarias cuenta = DALCuentasBancarias.GetByID(x => x.Id_Cuenta == temp.No_CuentaBancaria);
                if (cuenta == null)
                    throw new ArgumentException(String.Format("La cuenta {0} no existe", temp.No_CuentaBancaria));
                else
                {
                    if(DALCierreBanco.GetByID(x=>x.Id_CtaBancaria==cuenta.Id_CtaBancaria && x.Id_Mes==mes).Cerrado_Conciliado.Value)
                        throw new ArgumentException(String.Format("El mes esta cerrado para la cuenta {0}.", temp.No_CuentaBancaria));
                }
                
            }
        }
        public void CleanEstadosCuenta(Int16 banco,Int16 mes)
        {
            List<Ca_CuentasBancarias> listaCuentas = DALCuentasBancarias.Get(x => x.Id_Banco == banco).ToList();
            foreach (Ca_CuentasBancarias item in listaCuentas)
            {
                if(DALEstadosCuenta.Get(x => x.Id_CuentaBancaria == item.Id_CtaBancaria && x.Conciliado==true && x.IdMes==mes).Count()>0)
                    throw new ArgumentException(String.Format("Hay registros ya conciliados para la cuenta {0}.",item.NoCuenta));
                if (DALCierreBanco.GetByID(x => x.Id_CtaBancaria == item.Id_CtaBancaria && x.Id_Mes == mes).Cerrado_Conciliado.Value)
                    throw new ArgumentException(String.Format("El mes esta cerrado para la cuenta {0}.", item.Id_Cuenta));
            }
            foreach (Ca_CuentasBancarias item in listaCuentas)
            {
                DALEstadosCuenta.DeleteAll(DALEstadosCuenta.Get(x => x.Id_CuentaBancaria == item.Id_CtaBancaria && x.IdMes==mes).ToList());
                DALEstadosCuenta.Save();
                
            }
        }
        public List<Ma_EstadosCuenta> ProcesarSantander(string[] elementos,Int16 IdMes)
        {
            List<Ma_EstadosCuenta> lista = new List<Ma_EstadosCuenta>();
            for (int i = 0; i < elementos.Length - 2; i++)
            {
                Ma_EstadosCuenta temp = new Ma_EstadosCuenta();
                Ca_CuentasBancarias cuenta = DALCuentasBancarias.GetByID(x => x.Id_Cuenta == elementos[i].Substring(0, 16));
                if(cuenta==null)
                    throw new ArgumentException(String.Format("La cuenta {0} no existe", elementos[i].Substring(0, 16)));
                temp.Id_CuentaBancaria = 1;
                string nocuenta = elementos[i].Substring(0, 16);
                string anio = elementos[i].Substring(20, 4);
                string mes = elementos[i].Substring(18, 2);
                string dia = elementos[i].Substring(16, 2);
                temp.No_CuentaBancaria = elementos[i].Substring(0, 16);
                DateTime fecha = new DateTime(Convert.ToInt32(elementos[i].Substring(20, 4)), Convert.ToInt32(elementos[i].Substring(16, 2)), Convert.ToInt32(elementos[i].Substring(18, 2)));
                temp.Fecha = fecha;
                temp.Sucursal = elementos[i].Substring(28, 4);
                temp.Descripcion = elementos[i].Substring(36, 40);
                string tipo = elementos[i].Substring(76, 1);
                if (tipo == "+")
                    temp.Abonos = Convert.ToDecimal(elementos[i].Substring(77, 14));
                else
                    temp.Cargos = Convert.ToDecimal(elementos[i].Substring(77, 14));
                temp.Saldos = Convert.ToDecimal(elementos[i].Substring(91, 14));
                temp.Referencia = elementos[i].Substring(105, 8);
                temp.Concepto = elementos[i].Substring(113, 40);
                temp.IdMes =Convert.ToByte(IdMes);
                if(ValidaInsercion(temp))
                    lista.Add(temp);
            }
            return lista;
        }
        public bool ValidaInsercion(Ma_EstadosCuenta entity)
        {
            Ma_EstadosCuenta temp = DALEstadosCuenta.GetByID(x=>x.Referencia==entity.Referencia&& x.Id_CuentaBancaria==entity.Id_CuentaBancaria && x.Fecha==entity.Fecha && x.IdMes==entity.IdMes &&
                                    (entity.Cargos.HasValue)? x.Cargos==entity.Cargos : x.Abonos==entity.Abonos);
            if (temp == null)
            {
                entity.Conciliado = false;
                DALEstadosCuenta.Insert(entity);
                DALEstadosCuenta.Save();
                return true;
            }
            else
            {
                if (!temp.Conciliado.Value)
                    return true;
            }
            return false;
        }
        public void validarCierreMes(int? idMes,int? IdCta)
        {
            if (DALCierreBanco.GetByID(x => x.Id_CtaBancaria == IdCta && x.Id_Mes == idMes).Cerrado_Conciliado.Value)
                throw new ArgumentException(String.Format("El mes esta cerrado para esta cuenta"));
        }
        public void startConciliacion(BusquedaConciliacionAuto busqueda)
        {
            validarCierreMes(busqueda.IdMes, busqueda.IdCtaBancaria);
            List<Ma_EstadosCuenta> lista = DALEstadosCuenta.Get(x => x.Conciliado == false && x.Id_CuentaBancaria==busqueda.IdCtaBancaria && x.IdMes==busqueda.IdMes).ToList();
            foreach (Ma_EstadosCuenta item in lista)
            {
                Int32 cheque=Convert.ToInt32(item.Referencia);
                Ma_MovimientosConciliacion temp = DALMaMovimientosConciliacion.GetByID(x => x.Id_CtaBancaria == item.Id_CuentaBancaria && x.Fecha<=item.Fecha && 
                                                                                       x.No_Cheque==cheque &&
                                                                                       (x.Estatus==1 || x.Estatus==3 ) &&
                                                                                       x.Cancelado != true &&
                                                                                       (item.Cargos.HasValue) ? x.Importe == item.Cargos && x.Id_Movimiento == 1 : x.Importe == item.Abonos && x.Id_Movimiento == 2);
                if (temp != null)
                {
                    temp.Estatus = 2;
                    temp.Fecha_Conciliacion = DateTime.Now;
                    DALMaMovimientosConciliacion.Update(temp);
                    DALMaMovimientosConciliacion.Save();
                    item.Conciliado = true;
                    DALEstadosCuenta.Update(item);
                    DALEstadosCuenta.Save();
                }
            }
        }
        public void startConciliacionDepositos(BusquedaConciliacionAuto busqueda)
        {
            validarCierreMes(busqueda.IdMes, busqueda.IdCtaBancaria);
            List<Ma_EstadosCuenta> lista = DALEstadosCuenta.Get(x => x.Conciliado == false && x.Id_CuentaBancaria == busqueda.IdCtaBancaria && x.IdMes == busqueda.IdMes && (x.Referencia==null || x.Referencia=="00000000")).ToList();
            foreach (Ma_EstadosCuenta item in lista)
            {
                Int32 cheque = Convert.ToInt32(item.Referencia);
                Ma_MovimientosConciliacion temp = DALMaMovimientosConciliacion.GetByID(x => x.Id_CtaBancaria == item.Id_CuentaBancaria && x.Fecha <= item.Fecha &&
                                                                                       x.No_Cheque == null &&
                                                                                       (x.Estatus == 1 || x.Estatus == 3) &&
                                                                                       x.Cancelado != true &&
                                                                                       (item.Cargos.HasValue) ? x.Importe == item.Cargos && x.Id_Movimiento == 1 : x.Importe == item.Abonos && x.Id_Movimiento == 2);
                if (temp != null)
                {
                    temp.Estatus = 2;
                    temp.Fecha_Conciliacion = DateTime.Now;
                    DALMaMovimientosConciliacion.Update(temp);
                    DALMaMovimientosConciliacion.Save();
                    item.Conciliado = true;
                    DALEstadosCuenta.Update(item);
                    DALEstadosCuenta.Save();
                }
            }
        }
        public void revertConciliacion(BusquedaConciliacionAuto busqueda)
        {
            validarCierreMes(busqueda.IdMes, busqueda.IdCtaBancaria);
            List<Ma_EstadosCuenta> lista = DALEstadosCuenta.Get(x => x.Conciliado == true && x.Id_CuentaBancaria == busqueda.IdCtaBancaria && x.IdMes == busqueda.IdMes).ToList();
            foreach (Ma_EstadosCuenta item in lista)
            {
                Int32 cheque = Convert.ToInt32(item.Referencia);
                Ma_MovimientosConciliacion temp = DALMaMovimientosConciliacion.Get(x => x.Id_CtaBancaria == item.Id_CuentaBancaria && x.Fecha <= item.Fecha &&
                                                                                       x.No_Cheque == cheque &&
                                                                                       x.Estatus == 2 &&
                                                                                       (item.Cargos.HasValue) ? x.Importe == item.Cargos : x.Importe == item.Abonos).FirstOrDefault();
                if (temp != null)
                {
                    temp.Estatus = temp.Estatus=1;
                    temp.Id_FolioMovimienotBancario = temp.Id_FolioMovimiento_Original;
                    temp.Id_TipoMovimientoBancario = temp.Id_TipoMovimiento_Original;
                    temp.Fecha_Conciliacion = null;
                    DALMaMovimientosConciliacion.Update(temp);
                    DALMaMovimientosConciliacion.Save();
                    item.Conciliado = false;
                    DALEstadosCuenta.Update(item);
                    DALEstadosCuenta.Save();
                }
            }
        }
    }
}