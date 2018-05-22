using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Areas.Tesoreria.Models;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class PolizasBL
    {
        private MaPolizasDAL DALPolizas;
        private DePolizasDAL DALDetallePolizas;
        private DeReferenciasPolizasDAL referenciasDAL { get; set; }
        private CompromisosBL compromisosBL { get; set; }
        protected DeDisponibilidadDAL DALDeDisponiblidad { get; set; }
        protected DeEvolucionDAL DALDeEvolucion { get; set; }

        public PolizasBL()
        {
            DALPolizas = new MaPolizasDAL();
            DALDetallePolizas = new DePolizasDAL();
            if (referenciasDAL == null) referenciasDAL = new DeReferenciasPolizasDAL();
            if (compromisosBL == null) compromisosBL = new CompromisosBL();
            if (DALDeDisponiblidad == null) DALDeDisponiblidad = new DeDisponibilidadDAL();
            if (DALDeEvolucion == null) DALDeEvolucion = new DeEvolucionDAL();
        }

        public int GetNextFolio(byte Id_TipoPoliza, byte Id_MesPoliza)
        {
            try
            {
                return DALPolizas.Get(reg => reg.Id_TipoPoliza == Id_TipoPoliza && reg.Id_MesPoliza == Id_MesPoliza).Max(max => max.Id_FolioPoliza) + 1;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public short GetNextRegistroDetalle(byte Id_TipoPoliza,int Id_FolioPoliza,  byte Id_MesPoliza)
        {
            try
            {
                return Convert.ToInt16(DALDetallePolizas.Get(reg => reg.Id_TipoPoliza == Id_TipoPoliza && reg.Id_MesPoliza == Id_MesPoliza && reg.Id_FolioPoliza == Id_FolioPoliza).Max(max => max.Id_Registro) + 1);
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public bool hasPolizas(byte Id_TipoPoliza,int Id_FolioPoliza,  byte Id_MesPoliza)
        {
            if(referenciasDAL.GetByID(x=> x.IdFolioPoliza == Id_FolioPoliza && x.IdMesPoliza == Id_MesPoliza && x.IdTipoPoliza == Id_TipoPoliza) != null)
                return true;
            return false;
        }

        public List<object> createBotonera(Ma_PolizasModel model)
        {
            List<object> botonera = new List<object>() { "bNuevo", "bBuscar", "bDetalles","bImprimir" };
            bool cerrado = compromisosBL.isClosed(model.Fecha);
            if (!cerrado && String.IsNullOrEmpty(model.Poliza_Comprometido) && model.Id_ClasPoliza == 2)
                this.AddBoton(ref botonera, "bEditar");
            if ((String.IsNullOrEmpty(model.Poliza_DiarioC) || model.Poliza_DiarioC == "00000-") && model.De_Polizas.Count > 0 && model.Id_ClasPoliza == 2)
                this.AddBoton(ref botonera, "bCancelarGral");
            this.AddBoton(ref botonera, "bSalir");
            return botonera;
        }

        public List<object> createDeBotonera(Ma_PolizasModel model)
        {
            List<object> botonera = new List<object>();
            bool cerrado = compromisosBL.isClosed(model.Fecha);
            if (!cerrado && String.IsNullOrEmpty(model.Poliza_Comprometido) && model.Id_ClasPoliza == 2)
                return new List<object>() { "bNuevo", "bSalir" };
            this.AddBoton(ref botonera, "bSalir");
            return botonera;
        }

        private void AddBoton(ref List<object> botonera, string btn)
        {
            botonera.Add(btn);
        }

        public decimal hasPagado(string cve_presupuesto, decimal importe, DateTime? Fecha)
        {
            byte mes = Fecha.HasValue ? Convert.ToByte(Fecha.Value.Month) : Convert.ToByte("0");
            DE_Disponibilidad entity = DALDeDisponiblidad.GetByID(reg => reg.Id_ClavePresupuesto == cve_presupuesto && reg.Mes == mes);
            return entity.Pagado.HasValue ? entity.Pagado.Value - importe : 0 - importe;
        }

        public decimal hasRecaudado(string cve_presupuesto, decimal importe, DateTime? Fecha)
        {
            byte mes = Fecha.HasValue ? Convert.ToByte(Fecha.Value.Month) : Convert.ToByte("0");
            DE_Evolucion entity = DALDeEvolucion.GetByID(reg => reg.Id_ClavePresupuesto == cve_presupuesto && reg.Mes == mes);
            //DE_Disponibilidad entity = DALDeDisponiblidad.GetByID(reg => reg.Id_ClavePresupuesto == cve_presupuesto && reg.Mes == mes);
            return entity.Recaudado.HasValue ? entity.Recaudado.Value - importe : 0 - importe;
        }

        public List<Ma_PolizasModel> BuscarCompromisos(AnaliticoPolizas busqueda)
        {
            List<Ma_Polizas> listaPolizas = DALPolizas.Get(
                x => (busqueda.Id_TipoPoliza.HasValue && busqueda.Id_TipoPoliza>0 ? x.Id_TipoPoliza == busqueda.Id_TipoPoliza.Value : x.Id_TipoPoliza > 0) &&
                (busqueda.Id_TipoCR.HasValue && busqueda.Id_TipoCR>0 ? x.Id_TipoCR == busqueda.Id_TipoCR.Value : x.Id_TipoCR > 0) &&
                (busqueda.Id_Beneficiario.HasValue && busqueda.Id_Beneficiario>0 ? x.Id_Beneficiario == busqueda.Id_Beneficiario.Value : x.Id_TipoPoliza > 0) &&
                (busqueda.FechaDesde.HasValue && busqueda.FechaDesde>DateTime.MinValue ? x.Fecha >= busqueda.FechaDesde && x.Fecha <= busqueda.FechaHasta : x.Id_TipoPoliza > 0) &&
                (busqueda.MesInicio.HasValue && busqueda.MesFin.HasValue ? x.Id_MesPoliza >= busqueda.MesInicio && x.Id_MesPoliza <= busqueda.MesFin :
                busqueda.MesInicio.HasValue && !busqueda.MesFin.HasValue ? x.Id_MesPoliza >= busqueda.MesInicio : !busqueda.MesInicio.HasValue && busqueda.MesFin.HasValue ? x.Id_MesPoliza <= busqueda.MesFin : x.Id_TipoPoliza > 0)&&
                (!String.IsNullOrEmpty(busqueda.Descripcion) ? x.Descripcion.Contains(busqueda.Descripcion) : x.Id_TipoPoliza > 0) &&
                (busqueda.Id_CtaBancaria.HasValue && busqueda.Id_CtaBancaria > 0 ? x.Id_CtaBancaria == busqueda.Id_CtaBancaria.Value : x.Id_TipoPoliza > 0) &&
                (busqueda.ImporteDesde.HasValue && busqueda.ImporteHasta.HasValue ? x.Cargos >= busqueda.ImporteDesde.Value && x.Cargos <= busqueda.ImporteHasta :
                busqueda.ImporteDesde.HasValue && !busqueda.ImporteHasta.HasValue ? x.Cargos >= busqueda.ImporteDesde : !busqueda.ImporteDesde.HasValue && busqueda.ImporteHasta.HasValue ? x.Cargos <= busqueda.ImporteHasta : x.Id_TipoPoliza > 0)&&
                (busqueda.ChequeInicial.HasValue && busqueda.ChequeFinal.HasValue ? x.No_Cheque >= busqueda.ChequeInicial.Value && x.No_Cheque <= busqueda.ChequeFinal :
                busqueda.ChequeInicial.HasValue && !busqueda.ChequeFinal.HasValue ? x.No_Cheque >= busqueda.ChequeInicial : !busqueda.ChequeInicial.HasValue && busqueda.ChequeFinal.HasValue ? x.No_Cheque <= busqueda.ChequeFinal : x.Id_TipoPoliza > 0) 
                ).ToList();

            List<Ma_PolizasModel> lista = new List<Ma_PolizasModel>();
            foreach (Ma_Polizas item in listaPolizas)
            {
                lista.Add(new Llenado().LLenado_MaPolizas(item.Id_TipoPoliza,item.Id_FolioPoliza,item.Id_MesPoliza));
            }
            return lista;
        }
    }
}