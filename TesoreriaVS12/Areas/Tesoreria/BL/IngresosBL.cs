using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Vistas;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class IngresosBL
    {
        private vReciboIngresosDAL DALReciboIngresos {get;set;}
        public IngresosBL()
        {
            if (this.DALReciboIngresos == null) this.DALReciboIngresos = new vReciboIngresosDAL();
        }

        public List<VW_DetalleReciboIngModel> BusquedaReciboIngresos(BusquedaAnaliticoIngresos busqueda)
        {
            List<VW_DetalleReciboIng> listaRecibos = DALReciboIngresos.Get(x=>(busqueda.IdContribuyente>0? x.IdContribuyente == busqueda.IdContribuyente:x.Folio>0)
                && (busqueda.ImporteDesde.HasValue && busqueda.ImporteHasta.HasValue ? x.Importe >= busqueda.ImporteDesde.Value && x.Importe <= busqueda.ImporteHasta :
                busqueda.ImporteDesde.HasValue && !busqueda.ImporteHasta.HasValue ? x.Importe >= busqueda.ImporteDesde : !busqueda.ImporteDesde.HasValue && busqueda.ImporteHasta.HasValue ? x.Importe <= busqueda.ImporteHasta : x.Folio> 0) &&
                (busqueda.FechaDesdeRecibo.HasValue? x.Fecha >= busqueda.FechaDesdeRecibo.Value && x.Fecha<=busqueda.FechaHastaRecibo.Value:x.Folio>0) &&
                (busqueda.FechaDesdeRecaudacion.HasValue ? x.FechaRecaudacion >= busqueda.FechaDesdeRecaudacion.Value && x.FechaRecaudacion <= busqueda.FechaHastaRecaudacion.Value : x.Folio > 0) &&
                (busqueda.CuentaBancaria>0 ? x.Id_CtaBancaria == busqueda.CuentaBancaria:x.Folio>0) &&
                (busqueda.Estatus>0 ? x.IdEstatus == busqueda.Estatus : x.Folio>0)).ToList();
            List<VW_DetalleReciboIngModel> listaBusqueda = new List<VW_DetalleReciboIngModel>();
            listaRecibos.ForEach(x => listaBusqueda.Add(ModelFactory.getModel<VW_DetalleReciboIngModel>(x, new VW_DetalleReciboIngModel())));
            listaBusqueda.ForEach(x=>x.DescripcionEstatus = Diccionarios.EstatusRecibos[x.IdEstatus.Value]);
            switch (busqueda.Orden)
            {
                case 1:
                    listaBusqueda = listaBusqueda.OrderBy(x=>x.Folio).ToList();
                    break;
                case 2:
                    listaBusqueda = listaBusqueda.OrderBy(x => x.IdContribuyente).ToList();
                    break;
                case 3:
                    listaBusqueda = listaBusqueda.OrderBy(x => x.FechaRecaudacion).ToList();
                    break;
                default:
                    break;
            }
            return listaBusqueda;
        }

        public List<VW_DetalleReciboIngModel> BusquedaCorteCaja(BusquedaCorteCaja busqueda)
        {
            List<VW_DetalleReciboIng> listaResultados = DALReciboIngresos.Get(x=>(busqueda.FechaDesdeRecaudacion.HasValue ? 
                x.FechaRecaudacion>=busqueda.FechaDesdeRecaudacion.Value && x.FechaRecaudacion<= busqueda.FechaHastaRecaudacion.Value: x.Folio>0) &&
                (busqueda.CajaReceptora>0 ? x.Id_CajaR == busqueda.CajaReceptora : x.Folio>0) &&
                (busqueda.Estatus>0 ? x.IdEstatus == busqueda.Estatus : x.Folio>0)).ToList();
            List<VW_DetalleReciboIngModel> lista = new List<VW_DetalleReciboIngModel>();
            listaResultados.ForEach(x => lista.Add(ModelFactory.getModel<VW_DetalleReciboIngModel>(x, new VW_DetalleReciboIngModel())));
            foreach (var item in lista)
            {
                item.DescripcionEstatus = Diccionarios.EstatusRecibos[item.IdEstatus.Value];
            }
            //lista.ForEach(x=>x.DescripcionEstatus = Diccionarios.EstatusRecibos[Convert.ToByte(x.IdEstatus)]);
            switch (busqueda.Orden)
            {
                case 1:
                    lista = lista.OrderBy(x => x.Folio).ToList();
                    break;
                case 2:
                    lista = lista.OrderBy(x => x.NombreContribuyente).ToList();
                    break;
                case 3:
                    lista = lista.OrderBy(x => x.FechaRecaudacion).ToList();
                    break;
                default:
                    break;
            }
            return lista;
        }

        public List<ReporteCorteCaja> ReporteCaja(List<VW_DetalleReciboIngModel> busqueda)
        {
            List<ReporteCorteCaja> reporte = new List<ReporteCorteCaja>();
            List<String> conceptos = (from reg in busqueda select reg.Id_Concepto).Distinct().ToList();
            String tempConcepto = "";
            foreach (string item in conceptos)
            {
                if (tempConcepto!=item)
                {
                    ReporteCorteCaja tempCorte = new ReporteCorteCaja();
                    tempCorte.Id_Concepto = item;
                    tempCorte.Id_Estatus = busqueda.First(x => x.Id_Concepto == item).IdEstatus;
                    tempCorte.DescripcionEstatus= Diccionarios.EstatusRecibos[Convert.ToByte(busqueda.First(x => x.Id_Concepto == item).IdEstatus)];
                    tempCorte.CUR = busqueda.First(x => x.Id_Concepto == item).CUR;
                    tempCorte.CRI = busqueda.First(x => x.Id_Concepto == item).CRI;
                    tempCorte.detalleCUR = busqueda.Where(x => x.Id_Concepto == item).ToList();
                    reporte.Add(tempCorte);
                }
            }
            return reporte;
        }
    }
}