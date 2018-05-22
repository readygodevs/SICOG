using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Vistas;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;

namespace TesoreriaVS12.Areas.Tesoreria.Models
{
    public class Listas
    {
        protected BD_TesoreriaEntities db { get; set; }
        private CaCajasReceptorasDAL cajasDAL { get; set; }
        protected BancosDAL dalBancos { get; set; }
        protected CuentasBancariasDAL cuentasbancarias { get; set; }
        public Listas()
        {
            if (db == null) db = new BD_TesoreriaEntities();
            if (cajasDAL == null) cajasDAL = new CaCajasReceptorasDAL();
            if (dalBancos == null) dalBancos = new BancosDAL();
            if (cuentasbancarias == null) cuentasbancarias = new CuentasBancariasDAL();
        }
        public SelectList ListaClasi(Int32? IdTipo, bool Manuales)
        {
            if (IdTipo.HasValue && IdTipo > 0)
                return new SelectList(new ClasificaPolizaDAL().Get(x => x.Id_TipoPoliza == IdTipo && x.Id_ClasificaPoliza != 0 && x.Id_SubClasificaPoliza == 0 && ((Manuales == true) ? x.Automatica != true : x.Automatica != null)).ToList(), "Id_ClasificaPoliza", "Descripcion");
            return new SelectList(new ClasificaPolizaDAL().Get().ToList(), "Id_ClasificaPoliza", "Descripcion");
        }

        public SelectList ListaSub(Int16? IdTipo, Int16? IdClasi)
        {
            if (IdTipo.HasValue && IdTipo > 0 && IdClasi.HasValue)
                return new SelectList(new ClasificaPolizaDAL().Get(x => x.Id_TipoPoliza == IdTipo && x.Id_ClasificaPoliza == IdClasi && x.Id_SubClasificaPoliza != 0).ToList(), "Id_SubClasificaPoliza", "Descripcion");
            return new SelectList(db.Ca_ClasificaPolizas, "Id_SubClasificaPoliza", "Descripcion");
        }

        public SelectList ListaFechaVencimiento(Int16 IdTipo)
        {
            //var firmantes = db.vFechasAsignacionFRyGC.GroupBy(a => new { a.Fecha.}).Select(g => new { g.Key }).ToList();
           
            if(IdTipo==1)
            {
                FechaAsignacionPPyPDDAL dal=new FechaAsignacionPPyPDDAL();
                List<vFechaAsignacionPPyPD> entities = dal.Get().ToList();
                List<vFechaAsignacionPPyPDModel> models = new List<vFechaAsignacionPPyPDModel>();

                foreach (vFechaAsignacionPPyPD item in entities)
                {
                    vFechaAsignacionPPyPDModel temp = new vFechaAsignacionPPyPDModel();
                    temp.id = item.id;
                    temp.Fecha = item.Fecha.Value.ToShortDateString();
                    models.Add(temp);
                }
                SelectList select = new SelectList(models, "Fecha", "Fecha");
                return select;
            }
            else
            {

                FechasAsignacionFRyGCDAL dal = new FechasAsignacionFRyGCDAL();
                List<vFechasAsignacionFRyGC> entities = dal.Get().ToList();
                List<vFechasAsignacionFRyGCModel> models = new List<vFechasAsignacionFRyGCModel>();

                foreach (vFechasAsignacionFRyGC item in entities)
                {
                    vFechasAsignacionFRyGCModel temp = new vFechasAsignacionFRyGCModel();
                    temp.id = item.id;
                    temp.Fecha = item.Fecha.Value.ToShortDateString();
                    models.Add(temp);
                }
                SelectList select = new SelectList(models, "Fecha", "Fecha");
                return select;
            }
        }

        public SelectList ListaCajasReceptoras()
        {
            return new SelectList(cajasDAL.Get(), "Id_CajaR", "Descripcion");
        }

        public SelectList ListaBancos()
        {
            return new SelectList(dalBancos.Get(), "Id_Banco", "Descripcion");
        }

        public SelectList ListaCtaBancarias(short? Id_Banco)
        {
            return new SelectList(cuentasbancarias.Get(x => x.Id_Banco == Id_Banco), "Id_CtaBancaria", "Descripcion");
        }
    }
}