using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Models;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Utils;
using System.Configuration;
using TesoreriaVS12.Areas.Tesoreria.BL;
using TesoreriaVS12.Filters;
namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    [AuthorizeLogin]
    public class NominaController : Controller
    {
        public string celda;
        public int noCelda;
        bool exito = false;

        public ActionResult CatalogoPyD()
        {
            List<Ca_Percep_Deduc> lista = (List<Ca_Percep_Deduc>)Session["catalogo"];
            Session.Remove("catalogo");
            if (lista != null)
            {
                String error = Session["error"].ToString();
                bool exito = (bool)Session["exito"];
                if (exito)
                    ViewBag.Exito = "El catálogo de Percepciones y Deducciones ha sido importado exitosamente";
                Session.Remove("error");
                Session.Remove("exito");
                if (!String.IsNullOrEmpty(error))
                    ModelState.AddModelError("", error);
                return View(lista);
            }
            else
            {
                PercepDeducDAL percDal = new PercepDeducDAL();
                List<Ca_Percep_Deduc> catalogo = percDal.Get().ToList();
                return View(catalogo);
            }
        }

        /*
         * Procesa el archivo de Catálogos de Percepciones y Deducciones Excel)
         */

        [HttpPost]
        public ActionResult ProcesarCatalogoPyD()
        {
            Session.Add("error", "");
            Session.Add("exito", false);
            int noCelda = 0;
            string celda = "";
            try
            {
                PercepDeducDAL percDal = new PercepDeducDAL();
                if (Request.Files["archivoExcel"].ContentLength > 0)
                {
                    Uri url = new Uri(ConfigurationManager.AppSettings.Get("urlFtp"));
                    string usuario = ConfigurationManager.AppSettings.Get("ftpUser");
                    string password = ConfigurationManager.AppSettings.Get("ftpPass");
                    Ftp ftp = new Ftp(url, usuario, password);
                    var file = Request.Files["archivoExcel"];
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    if (extension != ".xlsx" && extension != ".xls")
                        throw new ArgumentException("El archivo no es un documento de Excel");
                    ftp.UploadFTP(file.InputStream, file.FileName);
                    Byte[] archivo;
                    archivo = ftp.DownloadFTPtoByte(file.FileName);
                    IWorkbook hssfwb;
                    Stream stream = new MemoryStream(archivo);
                    hssfwb = WorkbookFactory.Create(stream);
                    ISheet sheet = hssfwb.GetSheetAt(0);
                    IRow encabezado = sheet.GetRow(0);
                    int totalCeldas = encabezado.LastCellNum;
                    if (totalCeldas == 4)
                    {
                        int totalFilas = sheet.LastRowNum;
                        bool hayValores = false;
                        for (int i = 1; i <= totalFilas; i++)
                        {
                            noCelda = i + 1;
                            IRow fila = sheet.GetRow(i);
                            if (fila != null)
                            {
                                if (fila.PhysicalNumberOfCells == 4)
                                {
                                    CatalogoPyD v = new CatalogoPyD();
                                    celda = "A";
                                    if (fila.GetCell(0) != null)
                                    {
                                        if (!String.IsNullOrEmpty(fila.GetCell(0).StringCellValue))
                                        {
                                            hayValores = true;
                                            v.Clave = fila.GetCell(0).StringCellValue;
                                        }
                                    }
                                    else
                                        throw new ArgumentException(String.Format("La clave de la celda {0}{1} no debe estar vacía", celda, noCelda));
                                    celda = "B";
                                    if (fila.GetCell(1) != null)
                                    {
                                        if (!String.IsNullOrEmpty(fila.GetCell(1).StringCellValue))
                                            v.Descripcion = fila.GetCell(1).StringCellValue.ToString();
                                    }
                                    else
                                        throw new ArgumentException(String.Format("La descripción de la celda {0}{1} no debe estar vacía", celda, noCelda));
                                    celda = "C";
                                    if (fila.GetCell(2) != null)
                                    {
                                        if (!String.IsNullOrEmpty(fila.GetCell(2).NumericCellValue.ToString()))
                                        {
                                            v.ObjetoGasto = Convert.ToDouble(fila.GetCell(2).NumericCellValue).ToString();
                                            if (v.ObjetoGasto.Length > 5)
                                                throw new ArgumentException(String.Format("El valor del Objeto de Gasto de la celda {0}{1} debe ser de 5 dígitos", celda, noCelda));
                                        }
                                    }
                                    celda = "D";
                                    if (fila.GetCell(3) != null)
                                    {
                                        if (!String.IsNullOrEmpty(fila.GetCell(3).NumericCellValue.ToString()))
                                        {
                                            v.Cuenta = Convert.ToDecimal(fila.GetCell(3).NumericCellValue).ToString();
                                            if (v.Cuenta.Length > 20)
                                                throw new ArgumentException(String.Format("El valor de la Cuenta de la celda {0}{1} debe ser de 20 dígitos", celda, noCelda));
                                        }
                                    }
                                    if (hayValores == true)
                                    {
                                        hayValores = false;
                                        Ca_Percep_Deduc caP = percDal.GetByID(x => x.Percep_Deduc == v.Clave);
                                        if (caP != null)
                                        {
                                            bool seActualiza = false;
                                            if (v.ObjetoGasto != null)//Se busca en el catálogo, en caso de no existir se muestra el error
                                            {
                                                ObjetoGDAL gastoDal = new ObjetoGDAL();
                                                CA_ObjetoGasto objeto = gastoDal.GetByID(x => x.Id_ObjetoG == v.ObjetoGasto);
                                                if (objeto == null)
                                                    throw new ArgumentException(String.Format("El Objeto de Gasto {0} de la celda {1}{2} no existe en el sistema", v.ObjetoGasto, celda, noCelda));
                                            }
                                            if (v.Cuenta != null)
                                            {
                                                CuentasDAL cuentasDal = new CuentasDAL();
                                                CA_Cuentas cuenta = cuentasDal.GetByID(x => x.Id_Cuenta == v.Cuenta);
                                                if (cuenta == null)
                                                    throw new ArgumentException(String.Format("La Cuenta {0} de la celda {1}{2} no existe en el sistema", v.Cuenta, celda, noCelda));
                                            }
                                            if (v.Cuenta != null && (caP.Id_Cuenta != v.Cuenta))
                                            {
                                                caP.Id_Cuenta = v.Cuenta;
                                                seActualiza = true;
                                            }
                                            if (v.ObjetoGasto != null && (caP.Id_ObjetoG.ToString() != v.ObjetoGasto))
                                            {
                                                caP.Id_ObjetoG = v.ObjetoGasto;
                                                seActualiza = true;
                                            }
                                            if (seActualiza)
                                                percDal.Save();
                                        }
                                        else
                                        {
                                            string tipo = "";
                                            if (v.Clave[0] == 'P') tipo = "Percepción";
                                            if (v.Clave[0] == 'D') tipo = "Deducción";
                                            throw new ArgumentException(String.Format("La {0} de la celda {1}{2} no existe en el sistema", tipo, celda, noCelda));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("La estructura del archivo no es válida"));
                    }
                }
                List<Ca_Percep_Deduc> catalogo = percDal.Get().ToList();
                Session.Add("catalogo", catalogo);
                Session["exito"] = true;
                return RedirectToAction("CatalogoPyD");
            }
            catch (Exception ex)
            {
                string error = "";
                switch (ex.HResult)
                {
                    case -2146233079:
                        error = String.Format("El formato de la celda {0}{1} no es válido, debe ser numérico", celda, noCelda);
                        break;
                    default:
                        error = String.Format(ex.Message);
                        break;
                }
                PercepDeducDAL percDal = new PercepDeducDAL();
                List<Ca_Percep_Deduc> catalogo = percDal.Get().ToList();
                Session["error"] = String.Format("Ocurrió un error: {0}", error);
                Session.Add("catalogo", catalogo);
                return RedirectToAction("CatalogoPyD");
            }
        }
        [HttpGet]
        public ActionResult ImportarNomina()
        {
            List<Ma_NominaModel> lista = (List<Ma_NominaModel>)Session["listaNomina"];
            Session.Remove("listaNomina");
            var error = Session["error"];
            Session.Remove("error");
            if (error != null)
            {
                if (!String.IsNullOrEmpty(error.ToString()))
                    ModelState.AddModelError("", error.ToString());
            }
            if (lista != null)
                return View(lista);
            return View(new List<Ma_NominaModel>());
        }

        [HttpPost]
        public ActionResult ProcesarNomina(Ma_NominaModel nominaForm)
        {
            Session.Add("error", "");
            Session.Add("exito", false);
            List<Ma_NominaModel> listaNomina = new List<Ma_NominaModel>();
            Byte[] archivo;
            try
            {
                PercepDeducDAL percDal = new PercepDeducDAL();
                if (Request.Files["archivoExcel"].ContentLength > 0)
                {
                    Uri url = new Uri(ConfigurationManager.AppSettings.Get("urlFtp"));
                    string usuario = ConfigurationManager.AppSettings.Get("ftpUser");
                    string password = ConfigurationManager.AppSettings.Get("ftpPass");
                    Ftp ftp = new Ftp(url, usuario, password);
                    var file = Request.Files["archivoExcel"];
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    if (extension != ".xlsx" || extension != ".xls")
                        throw new ArgumentException("El archivo no es un documento de Excel");
                    ftp.UploadFTP(file.InputStream, file.FileName);
                    archivo = ftp.DownloadFTPtoByte(file.FileName);
                    //string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Archivos"), Request.Files["archivoExcel"].FileName);
                    //if (System.IO.File.Exists(path1))
                    //    System.IO.File.Delete(path1);
                    //Request.Files["archivoExcel"].SaveAs(path1);
                    ProcesarExcelNomina(archivo, nominaForm.No_Nomina);
                    if (exito)//Si no ocurre error en el Excel se insertan las nóminas
                    {
                        ProcesarExcelNomina(archivo, nominaForm.No_Nomina, 1);
                    }
                }
                Session["exito"] = true;
                return RedirectToAction("ImportarNomina");
            }
            catch (Exception ex)
            {
                string error = "";
                switch (ex.HResult)
                {
                    case -2146233079:
                        error = String.Format("El formato de la celda {0}{1} no es válido, debe ser numérico", celda, noCelda);
                        break;
                    case -2146233033:
                        error = String.Format("El formato de la celda {0}{1} no es válido", celda, noCelda);
                        break;
                    default:
                        error = String.Format(ex.Message);
                        break;
                }
                Session["error"] = String.Format("Ocurrió un error: {0}", error);
                return RedirectToAction("ImportarNomina");
            }
        }

        public void ProcesarExcelNomina(Byte[] path1,string no_nomina ,int opcion = 0)
        {
            
            List<Ma_NominaModel> listaNomina = new List<Ma_NominaModel>();
            PercepDeducDAL dalPD = new PercepDeducDAL();
            IWorkbook hssfwb;
            Stream stream = new MemoryStream(path1);
            hssfwb = WorkbookFactory.Create(stream);
            ISheet sheet = hssfwb.GetSheetAt(0);
            IRow encabezado = sheet.GetRow(0);
            int totalCeldas = encabezado.LastCellNum;
            MaNominaDAL nominaDal = new MaNominaDAL();
            DeNominaDAL deNominaDal = new DeNominaDAL();
            CuentasDAL cuentasDal = new CuentasDAL();
            ObjetoGDAL objetoDal = new ObjetoGDAL();
            if (totalCeldas == 13)
            {
                int totalFilas = sheet.LastRowNum;
                bool hayValores = false;
                for (int i = 1; i <= totalFilas; i++)
                {
                    noCelda = i + 1;
                    IRow fila = sheet.GetRow(i);
                    if (fila != null)
                    {
                        if (fila.PhysicalNumberOfCells == 13)
                        {
                            Ma_NominaModel nomina = new Ma_NominaModel();
                            celda = "A";
                            if (fila.GetCell(0) != null)
                            {

                                if (!String.IsNullOrEmpty(fila.GetCell(0).StringCellValue.Trim()))
                                {
                                    hayValores = true;
                                    nomina.Fecha = Convert.ToDateTime(fila.GetCell(0).StringCellValue);
                                }
                            }
                            else
                                throw new ArgumentException(String.Format("La fecha de la celda {0}{1} no debe estar vacía", celda, noCelda));
                            celda = "B";//No. de Control
                            if (fila.GetCell(1) != null)
                            {
                                if (!String.IsNullOrEmpty(fila.GetCell(1).NumericCellValue.ToString()))
                                    nomina.No_Control = fila.GetCell(1).NumericCellValue.ToString();
                            }
                            else
                                throw new ArgumentException(String.Format("El número de control de la celda {0}{1} no debe estar vacía", celda, noCelda));
                            celda = "C";
                            if (fila.GetCell(2) != null)
                            {
                                if (!String.IsNullOrEmpty(fila.GetCell(2).StringCellValue.ToString()))
                                {
                                    nomina.CURP = fila.GetCell(2).StringCellValue;
                                }
                            }
                            celda = "D";
                            if (fila.GetCell(3) != null)
                            {
                                nomina.Nombre = fila.GetCell(3).StringCellValue;
                            }
                            celda = "E";
                            if (fila.GetCell(4) != null)
                            {
                                nomina.Apellido_Pat = fila.GetCell(4).StringCellValue;
                            }
                            celda = "F";
                            if (fila.GetCell(5) != null)
                            {
                                nomina.Apellido_Mat = fila.GetCell(5).StringCellValue;
                            }
                            celda = "G";
                            if (fila.GetCell(6) != null)
                            {
                                nomina.Tipo_Nomina = fila.GetCell(6).StringCellValue;
                            }
                            else
                                throw new ArgumentException(String.Format("El Tipo de Nómina de la celda {0}{1} no debe estar vacío", celda, noCelda));
                            celda = "H";
                            if (fila.GetCell(7) != null)
                            {
                                nomina.Id_Banco_RH = Convert.ToInt16(fila.GetCell(7).NumericCellValue);
                            }
                            else
                                throw new ArgumentException(String.Format("La Clave de Banco de la celda {0}{1} no debe estar vacía", celda, noCelda));
                            celda = "I";
                            if (fila.GetCell(8) != null)
                            {
                                nomina.No_Tarjeta = fila.GetCell(8).StringCellValue;
                            }
                            else
                                throw new ArgumentException(String.Format("La Cuenta de Banco de la celda {0}{1} no debe estar vacía", celda, noCelda));
                            celda = "J";
                            if (fila.GetCell(9) != null)
                            {
                                nomina.Tipo_Pago = fila.GetCell(9).StringCellValue;
                            }
                            else
                                throw new ArgumentException(String.Format("El Tipo de Pago de la celda {0}{1} no debe estar vacío", celda, noCelda));
                            celda = "K";
                            //Percepciones 
                            List<Percepcion> listaPercepciones = new List<Percepcion>();
                            List<Deduccion> listaDeducciones = new List<Deduccion>();
                            if (fila.GetCell(10) != null)
                            {
                                nomina.Percepciones = fila.GetCell(10).StringCellValue;
                                String[] stringPercepciones = nomina.Percepciones.ToString().Split('/');
                                foreach (var l in stringPercepciones)
                                {
                                    string[] percepcionValor = l.Split('-');
                                    if (percepcionValor[0].Length != 4)
                                        throw new ArgumentException(String.Format("La Percepción {0} de la celda {1}{2} no es válida, debe ser de 4 caracteres", percepcionValor[0], celda, noCelda));
                                    string clavePercepcion = percepcionValor[0];
                                    Ca_Percep_Deduc percepcion = dalPD.GetByID(x=>x.Percep_Deduc == clavePercepcion);
                                    if (percepcion == null)
                                        throw new ArgumentException(String.Format("La Percepción {0} de la celda {1}{2} no se encuentra registrada en el sistema", percepcionValor[0], celda, noCelda));
                                    else if (percepcion.Id_ObjetoG == null)
                                        throw new ArgumentException(String.Format("La Percepción {0} de la celda {1}{2} no tiene un Objeto de Gasto registrado", percepcionValor[0], celda, noCelda));
                                    else
                                    {
                                        CA_ObjetoGasto objeto = objetoDal.GetByID(x => x.Id_ObjetoG == percepcion.Id_ObjetoG);
                                        if (objeto == null)
                                            throw new ArgumentException(String.Format("El Objeto de Gasto de la Percepción {0} en la celda {1}{2} no debe estar vacío", percepcion.Percep_Deduc, celda, noCelda));
                                        else if (objeto.UltimoNivel.Value == false)
                                            throw new ArgumentException(String.Format("El Objeto de Gasto {0} de la Percepción {1} en la celda {2}{3} no es de último nivel", percepcion.Id_ObjetoG, percepcion.Percep_Deduc, celda, noCelda));
                                        
                                    }
                                    Percepcion p = new Percepcion();
                                    p.Clave = percepcionValor[0];
                                    p.Valor = Convert.ToDecimal(percepcionValor[1]);
                                    listaPercepciones.Add(p);
                                }
                            }
                            else
                                throw new ArgumentException(String.Format("Las Percepciones de la celda {0}{1} no deben estar vacías", celda, noCelda));
                            celda = "L";
                            //Percepciones 
                            if (fila.GetCell(11) != null)
                            {
                                nomina.Deducciones = fila.GetCell(11).StringCellValue;
                                String[] stringDeducciones = nomina.Deducciones.ToString().Split('/');
                                foreach (var l in stringDeducciones)
                                {
                                    string[] deduccionValor = l.Split('-');
                                    if (deduccionValor[0].Length != 4)
                                        throw new ArgumentException(String.Format("La Deducción {0} de la celda {1}{2} no es válida, debe ser de 4 caracteres", deduccionValor[0], celda, noCelda));
                                    string 
                                        claveDeduccion= deduccionValor[0];
                                    Ca_Percep_Deduc deduccion = dalPD.GetByID(x => x.Percep_Deduc == claveDeduccion);
                                    if (deduccion == null)
                                        throw new ArgumentException(String.Format("La Deducción {0} de la celda {1}{2} no se encuentra registrada en el sistema", deduccionValor[0], celda, noCelda));
                                    else if (deduccion.Id_Cuenta == null)
                                        throw new ArgumentException(String.Format("La Deducción {0} de la celda {1}{2} no tiene una Cuenta registrada", deduccionValor[0], celda, noCelda));
                                    else
                                    {
                                        CA_Cuentas cuenta = cuentasDal.GetByID(x => x.Id_Cuenta == deduccion.Id_Cuenta);
                                        if (cuenta == null)
                                            throw new ArgumentException(String.Format("La Cuenta de la Percepción {0} en la celda {1}{2} no debe estar vacía", deduccion.Percep_Deduc, celda, noCelda));
                                        else if (cuenta.Nivel == false)
                                            throw new ArgumentException(String.Format("La Cuenta {0} de la Percepción {1} en la celda {2}{3} no es de Nivel 1", deduccion.Id_Cuenta, deduccion.Percep_Deduc, celda, noCelda));
                                    }
                                    Deduccion d = new Deduccion();
                                    d.Clave = deduccionValor[0];
                                    d.Valor= Convert.ToDecimal(deduccionValor[1]);
                                    listaDeducciones.Add(d);
                                }
                            }
                            else
                                throw new ArgumentException(String.Format("Las Deducciones de la celda {0}{1} no deben estar vacías", celda, noCelda));
                            celda = "M";
                            if (fila.GetCell(12) != null)
                            {
                                nomina.Neto = Convert.ToDecimal(fila.GetCell(12).NumericCellValue);
                            }
                            else
                                throw new ArgumentException(String.Format("El valor Neto a Pagar de la celda {0}{1} no debe estar vacío", celda, noCelda));
                            nomina.No_Nomina = no_nomina;
                            if (opcion == 1)// se ejecuta el método con opcion para que se procesa a insertar la nómina
                            {
                                List<Ma_Nomina> nominasPagadas = nominaDal.Get(x=>x.No_Nomina == no_nomina && x.Pagado == true).ToList();
                                if (nominasPagadas.Count==0)
                                {
                                    List<De_Nomina> listaN = deNominaDal.Get(x => x.No_Nomina == no_nomina && x.No_Control == nomina.No_Control).ToList();
                                    foreach (De_Nomina l in listaN)
                                    {
                                        De_Nomina nTemp = l;
                                        deNominaDal.Delete(x => x.IdRegistro == nTemp.IdRegistro && x.No_Nomina == l.No_Nomina && x.No_Control == nomina.No_Control && x.Tipo_Nomina == nomina.Tipo_Nomina);
                                    }
                                    deNominaDal.Save();
                                    Ma_Nomina nominaExistente = nominaDal.GetByID(x=>x.No_Nomina == no_nomina && x.No_Control == nomina.No_Control);
                                    if(nominaExistente !=null)  nominaDal.Delete(x => x.No_Nomina == no_nomina && x.No_Control == nomina.No_Control);
                                    nominaDal.Save();
                                    Ma_Nomina registroNomina = EntityFactory.getEntity<Ma_Nomina>(nomina, new Ma_Nomina());
                                    registroNomina.Pagado = false;
                                    nominaDal.Insert(registroNomina);
                                    nominaDal.Save();
                                    NominaBL blNomina = new NominaBL();
                                    foreach (var l in listaPercepciones)
                                    {
                                        De_Nomina n = new De_Nomina();
                                        n.No_Nomina = nomina.No_Nomina;
                                        n.No_Control = nomina.No_Control;
                                        n.Tipo_Nomina = nomina.Tipo_Nomina;
                                        n.Clave_PD = l.Clave;
                                        n.Importe = l.Valor;
                                        n.IdRegistro = blNomina.GetNextID(nomina.No_Nomina, nomina.No_Control, nomina.Tipo_Nomina);
                                        deNominaDal.Insert(n);
                                        deNominaDal.Save();
                                    }
                                    foreach (var l in listaDeducciones)
                                    {
                                        De_Nomina n = new De_Nomina();
                                        n.No_Nomina = nomina.No_Nomina;
                                        n.No_Control = nomina.No_Control;
                                        n.Tipo_Nomina = nomina.Tipo_Nomina;
                                        n.Clave_PD = l.Clave;
                                        n.Importe = l.Valor;
                                        n.IdRegistro = blNomina.GetNextID(nomina.No_Nomina, nomina.No_Control, nomina.Tipo_Nomina);
                                        deNominaDal.Insert(n);
                                        deNominaDal.Save();
                                    }
                                }
                                else
                                    throw new ArgumentException(String.Format("El pago de la Nómina no. {0} ya ha sido realizada",nomina.No_Nomina));
                                listaNomina.Add(nomina);
                            }
                        }
                    }
                }
                exito = true;
                Session.Add("listaNomina", listaNomina);
            }
            else
                throw new ArgumentException(String.Format("La estructura del archivo no es válido"));
        }

    }

    public class CatalogoPyD
    {
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public string Cuenta { get; set; }
        public string ObjetoGasto { get; set; }
    }

    public class Percepcion
    {
        public string Clave { get; set; }
        public decimal Valor { get; set; }
    }

    public class Deduccion
    {
        public string Clave { get; set; }
        public decimal Valor { get; set; }
    }
}
