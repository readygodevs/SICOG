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
    public class RecibosBL
    {
        public RecibosBL()
        {
            if (maRecibosDAL == null) maRecibosDAL = new MaRecibosDAL();
            if (deRecibosDAL == null) deRecibosDAL = new DeRecibosDAL();
            if (parametrosDAL == null) parametrosDAL = new ParametrosDAL();
            if (compromisosBL == null) compromisosBL = new CompromisosBL();
        }
        private CompromisosBL compromisosBL { get; set; }
        protected MaRecibosDAL maRecibosDAL { get; set; }
        protected DeRecibosDAL deRecibosDAL { get; set; }
        protected ParametrosDAL parametrosDAL { get; set; }

        public int nextFolio()
        {
            try
            {
                CA_Parametros param = parametrosDAL.GetByID(x => x.Nombre == "UltimoFolioIngresos");
                if (param != null)
                    return Convert.ToInt32(param.Valor) + 1;
                else return 1;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        public bool setNextFolio()
        {
            try
            {
                CA_Parametros param = parametrosDAL.GetByID(x => x.Nombre == "UltimoFolioIngresos");
                if(param != null)
                {
                    param.Valor = (Convert.ToInt32(param.Valor) + 1).ToString();
                    parametrosDAL.Update(param);
                    parametrosDAL.Save();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<object> createBotonera(Ma_ReciboIngresosModel model)
        {
            List<object> botonera = new List<object>() { "bNuevo", "bBuscar", "bDetalles" };
            bool cerrado = compromisosBL.isClosed(model.Fecha);
            if (!cerrado && (!model.Impreso.HasValue || model.Impreso.Value != true) && model.IdEstatus == Diccionarios.Valores_EstatusRecibos.REGISTRADO)
                this.AddBoton(ref botonera, "bEditar");
            if(model.IdEstatus != Diccionarios.Valores_EstatusRecibos.CANCELADO)
            this.AddBoton(ref botonera, "bCancelarGral");
            if (model.IdEstatus == Diccionarios.Valores_EstatusRecibos.REGISTRADO && model.De_ReciboIngresos.Count > 0)
                this.AddBoton(ref botonera, "bImprimir");
            if (model.IdEstatus == Diccionarios.Valores_EstatusRecibos.DEVENGADO && model.De_ReciboIngresos.Count > 0)
                this.AddBoton(ref botonera, "bRecaudar");
            this.AddBoton(ref botonera, "bSalir");
            return botonera;
        }

        public List<object> createDeBotonera(Ma_ReciboIngresos model)
        {
            List<object> botonera = new List<object>();
            if (model.IdEstatus == Diccionarios.Valores_EstatusRecibos.REGISTRADO)
            {
                this.AddBoton(ref botonera, "bNuevo");
                if(model.De_ReciboIngresos.Count > 0)
                {
                    this.AddBoton(ref botonera, "bEditar");
                    this.AddBoton(ref botonera, "bEliminar");
                }
            }
            this.AddBoton(ref botonera, "bSalir");
            return botonera;
        }
        private void AddBoton(ref List<object> botonera, string btn)
        {
            botonera.Add(btn);
        }
        private void AddBoton(ref List<object> botonera, string[] btn)
        {
            botonera.Add(btn);
        }

    }
}