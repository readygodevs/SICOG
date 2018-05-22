using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class CompromisosBL
    {
        MaCompromisosDAL DALCompromisos = new MaCompromisosDAL();
        private CierreMensualDAL DALCierreMensual { get; set; }
        private DeCompromisosDAL DALDeCompromisos { get; set; }
        private TipoCompromisosDAL DALTipoCompromisos { get; set; }
        private BeneficiariosCuentasDAL DALBeneficiarioCuenta { get; set; }
        private DeCompromisosBL BLDeCompromisos { get; set; }

        public CompromisosBL()
        {
            if (DALCierreMensual == null) DALCierreMensual = new CierreMensualDAL();
            if (DALDeCompromisos == null) DALDeCompromisos = new DeCompromisosDAL();
            if (DALTipoCompromisos == null) DALTipoCompromisos = new TipoCompromisosDAL();
            if (DALBeneficiarioCuenta == null) DALBeneficiarioCuenta = new BeneficiariosCuentasDAL();
            if (BLDeCompromisos == null) BLDeCompromisos = new DeCompromisosBL();
            if (DALCompromisos == null) DALCompromisos = new MaCompromisosDAL();
        }

        public bool isAdquisisciones(bool adquisisciones)
        {
            return adquisisciones;
        }

        public bool isEditable(DateTime fecha, short estatus, bool adquisisciones)
        {
            if (isAdquisisciones(adquisisciones))
                return false;
            if(this.isClosed(fecha))
                return false;
            if (!(estatus == 1 || estatus == 5 || estatus == 6))
                return false;

            return true;
        }

        public bool isClosed(DateTime fecha)
        {
            byte mes = Byte.Parse(fecha.Month.ToString());
            return DALCierreMensual.GetByID(reg => reg.Id_Mes == mes).Contable.Value;
        }

        public bool hasCommittedPolicy(short tipoCompromiso, int folio)
        {
            
            
            return true;
        }

        public int getNextId(short tipoCompromiso)
        {
            try
            {
                if (DALTipoCompromisos.Get(reg => reg.Id_TipoCompromiso == tipoCompromiso).Count() > 0)
                    return DALTipoCompromisos.Get(reg => reg.Id_TipoCompromiso == tipoCompromiso).Max(max => max.UltimoComp).Value + 1;
                else
                    return 1;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public void updateNextId(short tipoCompromiso)
        {
            Ca_TipoCompromisos entity = DALTipoCompromisos.GetByID(reg => reg.Id_TipoCompromiso == tipoCompromiso);
            entity.UltimoComp = entity.UltimoComp + 1;
            DALTipoCompromisos.Update(entity);
            DALTipoCompromisos.Save();
        }

        public bool canEditar(short tipoCompromiso, int folio, short estatus)
        {
            if (!BLDeCompromisos.hasRows(tipoCompromiso, folio) && estatus == Diccionarios.ValorEstatus.COMPROMETIDO)
                return true;
            return false;
        }

        public bool canRecibir(short tipoCompromiso, int folio, short estatus)
        {
            if(BLDeCompromisos.hasRows(tipoCompromiso, folio) && BLDeCompromisos.isSaldado(tipoCompromiso, folio) && Diccionarios.ValorEstatus.COMPROMETIDO == estatus)
                return true;
            return false;
        }

        public bool canCancelar(short tipoCompromiso, int folio, short estatus)
        {
            Ma_Compromisos entity = DALCompromisos.GetByID(reg => reg.Id_TipoCompromiso == tipoCompromiso && reg.Id_FolioCompromiso == folio);
            if (estatus == Diccionarios.ValorEstatus.COMPROMETIDO || estatus ==  Diccionarios.ValorEstatus.AUTORIZACION)
                return true;
            else if (estatus == Diccionarios.ValorEstatus.RECIBIDO && entity.Id_TipoCR == null && entity.Id_FolioCR == null)
                return true;
            else
                return false;
        }

        public List<object> createBotonera(short tipoCompromiso, int folio, short estatus, bool? Adquisiciones = null)
        {
            List<object> botonera = new List<object>();

            Ma_Compromisos entity = DALCompromisos.GetByID(reg => reg.Id_TipoCompromiso == tipoCompromiso && reg.Id_FolioCompromiso == folio);

            if (Adquisiciones.HasValue && Adquisiciones.Value)
            {
                this.AddBoton(ref botonera, "bNuevo");

                this.AddBoton(ref botonera, "bBuscar");

                if (entity != null)
                    this.AddBoton(ref botonera, "bDetalles");

                this.AddBoton(ref botonera, "bSalir");
                return botonera;
            }

            this.AddBoton(ref botonera, "bNuevo");

            if (!entity.Id_FolioCR.HasValue && canEditar(tipoCompromiso, folio, estatus))
                this.AddBoton(ref botonera, "bEditar");
                        
            this.AddBoton(ref botonera, "bBuscar");
            
            if (entity != null)
                this.AddBoton(ref botonera, "bDetalles");

            if (!entity.Id_FolioCR.HasValue && canCancelar(tipoCompromiso, folio, estatus))
                this.AddBoton(ref botonera, "bCancelarGral");

            if (!entity.Id_FolioCR.HasValue && canRecibir(tipoCompromiso, folio, estatus))
                this.AddBoton(ref botonera, "bRecibido");

            this.AddBoton(ref botonera, "bSalir");

            return botonera;
        }

        private void AddBoton(ref List<object> botonera, string btn)
        {
            botonera.Add(btn);
        }

        public string getCuentaBeneficiario(int beneficiario, byte clasificacion)
        {
            return DALBeneficiarioCuenta.GetByID(reg => reg.Id_Beneficiario == beneficiario && reg.Id_ClasBeneficiario == clasificacion).Id_Cuenta;
        }

        public byte getUltimoMesCerrado()
        {
            try
            {
                return DALCierreMensual.Get(reg => reg.Contable == true).Last().Id_Mes;
            }
            catch (Exception)
            {
                return 0;               
            }
        }

        /*
         * Búsqueda de Compromisos [Análisis de Compromisos]
         */
        public List<Ma_CompromisosModel> BuscarCompromisos(CompromisosBusqueda compromiso)
        {
            List<Ma_Compromisos> listaCompromisos = DALCompromisos.Get(
                x => (compromiso.Id_TipoCompromiso.HasValue ? x.Id_TipoCompromiso == compromiso.Id_TipoCompromiso.Value : x.Id_FolioCompromiso > 0) &&
                (compromiso.Estatus.HasValue ? x.Estatus == compromiso.Estatus.Value:x.Id_FolioCompromiso>0) &&
                (compromiso.Id_Beneficiario.HasValue ? x.Id_Beneficiario == compromiso.Id_Beneficiario.Value : x.Id_FolioCompromiso > 0) &&
                //Búsqueda de fechas
                /*Buscar por Fecha de Orden*/
                /*&& compromiso.FechaHastaOrden.HasValue ? x.Fecha_Orden >= compromiso.FechaDesdeOrden.Value && x.Fecha_Orden<=compromiso.FechaHastaOrden.Value : 
                compromiso.FechaDesdeOrden.HasValue && !compromiso.FechaHastaOrden.HasValue? x.Fecha_Orden >= compromiso.FechaDesdeOrden.Value:!compromiso.FechaDesdeOrden.HasValue &&
                compromiso.FechaHastaOrden.HasValue ? x.Fecha_Orden <= compromiso.FechaHastaOrden.Value */
                (compromiso.FechaDesdeOrden.HasValue? x.Fecha_Orden>=compromiso.FechaDesdeOrden && x.Fecha_Orden<=compromiso.FechaHastaOrden : x.Id_FolioCompromiso > 0) &&
                /*Buscar por Fecha de Devengado*/
                /*
                 *  && compromiso.FechaHastaDevengado.HasValue ? x.Fecha_Devengado >= compromiso.FechaDesdeDevengado.Value && x.Fecha_Devengado <= compromiso.FechaHastaDevengado.Value :
                compromiso.FechaDesdeDevengado.HasValue && !compromiso.FechaHastaDevengado.HasValue ? x.Fecha_Devengado >= compromiso.FechaDesdeDevengado.Value : !compromiso.FechaDesdeDevengado.HasValue &&
                compromiso.FechaHastaDevengado.HasValue ? x.Fecha_Devengado <= compromiso.FechaHastaDevengado.Value 
                 */
                (compromiso.FechaDesdeDevengado.HasValue? x.Fecha_Devengado>=compromiso.FechaDesdeDevengado && x.Fecha_Devengado<=compromiso.FechaHastaDevengado: x.Id_FolioCompromiso > 0)  &&
                /*Buscar por Rangos de importes*/
                (compromiso.ImporteDesde.HasValue && compromiso.ImporteHasta.HasValue? x.Cargos >= compromiso.ImporteDesde.Value && x.Cargos <= compromiso.ImporteHasta : 
                compromiso.ImporteDesde.HasValue && !compromiso.ImporteHasta.HasValue ? x.Cargos>=compromiso.ImporteDesde : !compromiso.ImporteDesde.HasValue && compromiso.ImporteHasta.HasValue ? x.Cargos<=compromiso.ImporteHasta:x.Id_FolioCompromiso > 0) &&
                /*Buscar por Adquisición*/
                (compromiso.BusquedaAdquisiciones.HasValue ? compromiso.BusquedaAdquisiciones.Value == true ? x.Adquisicion == true : x.Id_FolioCompromiso > 0 : x.Id_FolioCompromiso > 0)
                ).ToList();
            
            List<Ma_CompromisosModel> lista = new List<Ma_CompromisosModel>();
            foreach (Ma_Compromisos item in listaCompromisos)
            {
                lista.Add(new Llenado().LLenado_ReporteMaCompromisos(item.Id_TipoCompromiso, item.Id_FolioCompromiso));
            }
            if (compromiso.Orden.HasValue)
            {
                switch (compromiso.Orden)
                {
                    case 1:
                        lista = lista.OrderBy(x => x.Id_TipoCompromiso).ThenBy(x => x.Id_FolioCompromiso).ToList();
                        break;
                    case 2:
                        lista = lista.OrderBy(x => x.Id_Beneficiario).ThenBy(x => x.Id_FolioCompromiso).ToList();
                        break;
                    case 3:
                        lista = lista.OrderBy(x => x.Fecha_Orden).ToList();
                        break;
                    case 4:
                        lista = lista.OrderBy(x => x.Fecha_Devengado).ToList();
                        break;
                    default:
                        lista = lista.OrderBy(x => x.Id_TipoCompromiso).ThenBy(x => x.Id_FolioCompromiso).ToList();
                        break;
                }
            }
            return lista;
            //|| (compromiso.FechaHasta.HasValue?x.Fecha_Orden<=compromiso.FechaHasta:x.Fecha_Orden !=null)
        }
    }
}