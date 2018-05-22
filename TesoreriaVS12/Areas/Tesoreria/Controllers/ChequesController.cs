using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.BL;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Vistas;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Filters;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    [AuthorizeLogin]
    public class ChequesController : Controller
    {
        private BancosImpresionChequesDAL DALvBancosImpresion { get; set; }
        private ListaChequesImpresionDAL DALvListaCheques { get; set; }
        private TipoFormatoChequesDAL DALTiposFormatosCheques { get; set; }
        private ChequesBL BLCheques { get; set; }
        private ProceduresDAL DALProcedures { get; set; }
        private ConvertHtmlToString reports { get; set; }

        public ChequesController()
        {
            if (DALvBancosImpresion == null) DALvBancosImpresion = new BancosImpresionChequesDAL();
            if (DALTiposFormatosCheques == null) DALTiposFormatosCheques = new TipoFormatoChequesDAL();
            if (DALvListaCheques == null) DALvListaCheques = new ListaChequesImpresionDAL();
            if (BLCheques == null) BLCheques = new ChequesBL();
            if (DALProcedures == null) DALProcedures = new ProceduresDAL();
            if (reports == null) reports = new ConvertHtmlToString();
        }
        //
        // GET: /Tesoreria/Cheques/

        public ActionResult Impresion()
        {
            Modelos.ImpresionChequesModel model = new Modelos.ImpresionChequesModel();
            List<vBancosImpresionChequesModel> listaBancos = new List<vBancosImpresionChequesModel>();
            List<Ca_TipoFormatosChequesModel> listaFormatos = new List<Ca_TipoFormatosChequesModel>();
            DALvBancosImpresion.Get().ToList().ForEach(item => { listaBancos.Add(ModelFactory.getModel<vBancosImpresionChequesModel>(item, new vBancosImpresionChequesModel())); });
            DALTiposFormatosCheques.Get().ToList().ForEach(item => { listaFormatos.Add(ModelFactory.getModel<Ca_TipoFormatosChequesModel>(item, new Ca_TipoFormatosChequesModel())); });
            model.Lista_Bancos = new SelectList(listaBancos, "Id_CtaBancaria", "NombreCuentaBancaria");
            model.Lista_FormatosCheques = new SelectList(listaFormatos, "Id_Formato", "Descripcion");
            //DALProcedures.PA_Generar_Cheques(1,0,4,"18/12/2014","18/12/2014",3,1);
            model.CFechas = new Control_Fechas();
            return View(model);
        }

        [HttpPost]
        public ActionResult TblCheques(Modelos.ImpresionChequesModel dataModel)
        {
            try
            {
                List<vListaChequesImpresionModel> models = new List<vListaChequesImpresionModel>();
                List<vListaChequesImpresion> entities = new List<vListaChequesImpresion>();                
                /*if (dataModel.Id_CtaBancaria > 0)
                    entities = DALvListaCheques.Get(reg => reg.Id_CtaBancaria == dataModel.Id_CtaBancaria).ToList();
                if (dataModel.Fecha_VenceContraRecibo != null)
                   entities = entities.Where(reg => reg.FechaVen == dataModel.Fecha_VenceContraRecibo).ToList();*/

                if (dataModel.Fecha_VenceContraRecibo.HasValue)
                    entities = DALvListaCheques.Get(reg => reg.Id_CtaBancaria == dataModel.Id_CtaBancaria && reg.FechaVen == dataModel.Fecha_VenceContraRecibo && reg.Fecha_Pago.HasValue == false).ToList();
                else
                    entities = DALvListaCheques.Get(reg => reg.Id_CtaBancaria == dataModel.Id_CtaBancaria && reg.Fecha_Pago.HasValue == false).ToList();

                entities = entities.OrderBy(reg => reg.No_Cheque).ToList();
                foreach (var item in entities)
                {
                    if (!item.Fecha_Pago.HasValue)
                    {
                        vListaChequesImpresionModel model = new vListaChequesImpresionModel();
                        model = ModelFactory.getModel<vListaChequesImpresionModel>(item, new vListaChequesImpresionModel());
                        models.Add(model);
                    }
                }
                return View(models);
            }
            catch (Exception)
            {
                return View(new List<vListaChequesImpresionModel>());
            }
        }

        [HttpPost]
        public ActionResult GenerarCheques(Modelos.ImpresionChequesModel dataModel)
        {
            try
            {
                List<vListaChequesImpresion> entities = new List<vListaChequesImpresion>();

                if (dataModel.Id_CtaBancaria == 0)
                    return Json(new { Exito = false, Mensaje = "Debe elegir una cuenta bancaria." });
                if (!dataModel.Fecha_Pago.HasValue)
                    return Json(new { Exito = false, Mensaje = "Debe elegir una fecha de pago." });

                if (dataModel.Fecha_VenceContraRecibo.HasValue)
                    entities = DALvListaCheques.Get(reg => reg.Id_CtaBancaria == dataModel.Id_CtaBancaria && reg.FechaVen == dataModel.Fecha_VenceContraRecibo && reg.Fecha_Pago.HasValue == false).ToList();
                else
                    entities = DALvListaCheques.Get(reg => reg.Id_CtaBancaria == dataModel.Id_CtaBancaria && reg.Fecha_Pago.HasValue == false).ToList();

                if(entities.Count == 0)
                    return Json(new { Exito = false, Mensaje = "No se encontraron cheques. Por favor revice su consulta." });

                entities = entities.OrderBy(reg => reg.No_Cheque).ToList();

                foreach (var item in entities)
                {
                    if(item.Fecha_Ejercido > dataModel.Fecha_Pago.Value)
                        return Json(new { Exito = false, Mensaje = "Se encontró al menos un Cuenta por Liquidar, cuya fecha es mayor a la fecha de pago." });

                    if (dataModel.PrimerCheque.HasValue && dataModel.PrimerCheque.Value)
                        break;
                }
                
                if (dataModel.PrimerCheque.HasValue && dataModel.PrimerCheque.Value)
                {
                    vListaChequesImpresion priemero = entities.First();
                    Session["ListaCheques"] = DALProcedures.PA_Generar_Cheques((short)Logueo.GetUsrLogueado().IdUsuario, 0, (short)priemero.Id_CtaBancaria, priemero.FechaVen.Value.ToShortDateString(), dataModel.Fecha_Pago.Value.ToShortDateString(), 1, byte.Parse(dataModel.Id_Formato.ToString()));
                    return Json(new { Exito = true, UrlRpt = "ImprimirCheques", Formato = dataModel.Id_Formato, Mensaje = "OK" });
                }

                short no_cheques = dataModel.NoChequesImprimir > 0 ? (short)dataModel.NoChequesImprimir : (short)entities.Count;
                if (no_cheques > entities.Count)
                    return Json(new { Exito = false, Mensaje = "La impresión no se puede llevar a cabo, porque el número de cheques que intenta imprimir es mayor al número de cheques para impresión." });
                if (!BLCheques.areConsecutive(dataModel.Id_CtaBancaria, dataModel.Fecha_Pago.Value, entities, no_cheques))
                    return Json(new { Exito = false, Mensaje = "La impresión no se puede llevar a cabo, porque los cheques no son consecutivos." });

                Session["ListaCheques"] = DALProcedures.PA_Generar_Cheques((short)Logueo.GetUsrLogueado().IdUsuario, 0, (short)dataModel.Id_CtaBancaria, dataModel.Fecha_VenceContraRecibo.HasValue ? dataModel.Fecha_VenceContraRecibo.Value.ToShortDateString() : null, dataModel.Fecha_Pago.Value.ToShortDateString(), no_cheques, byte.Parse(dataModel.Id_Formato.ToString()));
                return Json(new { Exito = true, UrlRpt = "ImprimirCheques", Formato = dataModel.Id_Formato, Mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Exito = false, Mensaje = new Errores(ex.HResult, ex.Message).Mensaje });
            }            
        }
        string NormalizeLength(string value, int maxLength)
        {
            return value.PadRight(maxLength).Substring(0, maxLength);
        }
        [HttpGet]
        public ActionResult ImprimirCheques(byte Formato)
        {
            FirmasDAL DALFirmas = new FirmasDAL();
            String NombreFirmante1,NombreFirmante2, CargoFirmante1,CargoFirmante2;
            TA_Firmas f1 = DALFirmas.GetByID(x=>x.Id_Firma == 1);
            if (f1 != null) {
                NombreFirmante1 = f1.Nombre;
                CargoFirmante1 = f1.Cargo;
            }
            else
            {
                NombreFirmante1 = "";
                CargoFirmante1 = "";
            }
            TA_Firmas f2 = DALFirmas.GetByID(x => x.Id_Firma == 2);
            if (f2 != null)
            {
                NombreFirmante2 = f2.Nombre;
                CargoFirmante2 = f2.Cargo;
            }
            else
            {
                NombreFirmante2 = "";
                CargoFirmante2 = "";
            }
            ViewBag.NombreFirmante2 = NormalizeLength(NombreFirmante2,25);
            ViewBag.CargoFirmante2 = CargoFirmante2;
            ViewBag.NombreFirmante1 = NormalizeLength(NombreFirmante1,25);
            ViewBag.CargoFirmante1 = CargoFirmante1;
            List<Chequestbl> cheques = Session["ListaCheques"] as List<Chequestbl>;
            Modelos.ChequesModel model = new Modelos.ChequesModel();
            model.Formato = Formato;
            model.UsuarioGeneroCheques = Logueo.GetUsrLogueado().NombreCompleto;
            List<int> no_cheques = (from reg in cheques select reg.No_Cheque).Distinct().ToList();
            foreach (int item in no_cheques)
	        {
                model.Cheques.Add(cheques.Where(reg => reg.No_Cheque == item).First());
                model.PolizasDiario = cheques.Where(reg => reg.No_Cheque == item && reg.Id_TipoPoliza == Ca_TipoPolizasModel.DIARIO).ToList();
                model.PolizasEgresos = cheques.Where(reg => reg.No_Cheque == item && reg.Id_TipoPoliza == Ca_TipoPolizasModel.EGRESOS).ToList();
	        }            
            
            //#define DEBUG            
            #if !DEBUG
                Session["ListaCheques"] = null;
            #endif            
            return File(reports.GenerarPDF_Blanco("ImprimirCheques", model, this.ControllerContext), "Application/PDF");
            /*
            var no_cheques = (from reg in Diccionarios.EstatusCompromisos select reg.Value).Distinct().ToList();
            return Json(no_cheques);*/
        }
    }
}
