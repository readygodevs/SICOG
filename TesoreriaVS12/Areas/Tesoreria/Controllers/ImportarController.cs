using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.BL;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Filters;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    [AuthorizeLogin]
    public class ImportarController : Controller
    {
        public string celda;
        public int noCelda;
        bool exito = false;
        public string errorMensaje = "";
        private string ruta = "/ArchivosContabilidad";
        #region Excel de Compromisos
        public ActionResult Compromisos()
        {
            var error = Session["error"];
            Session.Remove("error");
            var exito = Session["Exito"];
            Session.Remove("Exito");
            List<CompromisoNomina> lista = (List<CompromisoNomina>)Session["listaCompromisos"];
            Session.Remove("listaCompromisos");
            var folioCompromiso = Session["FolioCompromiso"];
            Session.Remove("FolioCompromiso");
            if (folioCompromiso != null)
                ViewBag.FolioCompromiso = folioCompromiso;
            if (exito != null)
                ViewBag.Exito = exito;
            if (error != null)
            {
                if (!String.IsNullOrEmpty(error.ToString()))
                    ModelState.AddModelError("", error.ToString());
            }
            if (lista != null && lista.Count() > 0)
                return View(lista.OrderByDescending(x => x.Cuenta));

            return View(new List<CompromisoNomina>());
        }

        [HttpPost]
        public ActionResult Compromisos(FormCollection form)
        {
            try
            {
                Session.Add("error", "");
                Session.Add("exito", false);
                List<CompromisoNomina> listaCompromiso = new List<CompromisoNomina>();
                if (Request.Files["archivoExcel"].ContentLength > 0)
                {
                    Uri url = new Uri(ConfigurationManager.AppSettings.Get("urlFtp"));
                    string usuario = ConfigurationManager.AppSettings.Get("ftpUser");
                    string password = ConfigurationManager.AppSettings.Get("ftpPass");
                    Ftp ftp = new Ftp(url, usuario, password);
                    var file = Request.Files["archivoExcel"];
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    MaCompromisosDAL compromisosDal = new MaCompromisosDAL();
                    if (extension == ".xlsx" || extension == ".xls")
                    {
                        String[] partesArchivo = file.FileName.ToString().Split('-');
                        if (partesArchivo.Count() != 4)
                            throw new ArgumentException("El nombre del archivo no es válido");
                        if (partesArchivo[0].ToUpper() != "COMPRO")
                            throw new ArgumentException("El nombre del archivo no es válido");
                        string noNomina = partesArchivo[3].Split('.')[0];
                        String AnioEjercicio = new ParametrosDAL().GetByID(x => x.Descripcion == "Ejercicio contable").Valor;
                        if (AnioEjercicio != noNomina.Substring(2, 4))
                            throw new ArgumentException("El año no corresponde al ejercicio");
                        string tipoNomina = partesArchivo[2];
                        if (compromisosDal.Get(X => X.No_Nomina == noNomina).ToList().Count() > 0)
                            throw new ArgumentException("El Compromiso para esa Nómina ya había sido generado");
                        ftp.ExisteDirectorio(ruta);
                        string nombreArchivo = ruta +"/"+ file.FileName;
                        if (!ftp.UploadFTP(file.InputStream, nombreArchivo))
                            throw new ArgumentException("No es posible conectar con el servidor");
                        Byte[] archivo;
                        archivo = ftp.DownloadFTPtoByte(nombreArchivo);
                        ProcesarExcel(archivo, noNomina, tipoNomina);
                        if (exito)
                            listaCompromiso = ProcesarExcel(archivo, noNomina, tipoNomina, 1);
                        ftp.EliminarArchivo(nombreArchivo);
                    }
                    else
                        throw new ArgumentException("El archivo no es un documento de Excel");
                }
                Session.Add("listaCompromisos", listaCompromiso);
                return RedirectToAction("Compromisos");
            }
            catch (Exception ex)
            {
                var sesionError = Session["ErrorSQL"];
                Session.Remove("ErrorSQL");
                string error = "";
                if (sesionError != null)
                {
                    error = new Errores(ex.HResult, ex.Message).Mensaje;
                }
                else
                {
                    switch (ex.HResult)
                    {
                        case -2146233079:
                            error = String.Format("El formato de la celda {0}{1} no es válido, debe ser numérico", celda, noCelda);
                            break;
                        default:
                            error = String.Format(ex.Message);
                            break;
                    }
                }

                Session["error"] = String.Format("Ocurrió un error: {0}", error);
                return RedirectToAction("Compromisos");
            }
        }

        /*
         * path1: arreglo con los datos del archivo de Excel
         * noNomina: # de nómina según el nombre del archivo
         * tipoNomina: tipo según nombre del archivo
         * opción: 1->Cuando se recorre el archivo en busca de errores, 2-> Cuando se validó que no hubo errores y se realiza la inserción en la base de datos
         * sección: 1->Compromiso, 2-> Nómina
         */
        public List<CompromisoNomina> ProcesarExcel(Byte[] path1, string noNomina, string tipoNomina, int opcion = 0, int seccion = 1)
        {
            int anio = 0;
            int mes = 0;
            MaCompromisosDAL compromisosDal = new MaCompromisosDAL();
            CompromisosBL cBL = new CompromisosBL();
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            List<CompromisoNomina> listaCompromisos = new List<CompromisoNomina>();
            BeneficiariosCuentasDAL beneficiariosDal = new BeneficiariosCuentasDAL();
            AreasDAL centroDal = new AreasDAL();
            De_CompromisosModel deCompromiso = new De_CompromisosModel();
            Ma_Compromisos ma_compromiso = new Ma_Compromisos();
            DeCompromisosDAL compDal = new DeCompromisosDAL();
            UsuarioLogueado Usuario = Session["appUsuario"] as UsuarioLogueado;
            DateTime Fecha = new DateTime();
            String Observaciones = "";
            bool sinDisponibilidad;
            bool existeSinDisponibilidad = false;
            IWorkbook hssfwb;
            Stream stream = new MemoryStream(path1);
            hssfwb = WorkbookFactory.Create(stream);
            ISheet sheet = hssfwb.GetSheetAt(0);
            IRow encabezado = sheet.GetRow(2);
            int totalCeldas = encabezado.LastCellNum;
            decimal TotalCargos = 0;
            decimal TotalAbonos = 0;
            int idBeneficiario = 0;
            errorMensaje = "1";
            if (totalCeldas >= 4)
            {
                ParametrosDAL paramDal = new ParametrosDAL();
                errorMensaje = "2";
                sinDisponibilidad = paramDal.GetByID(x => x.Nombre == "Sin_Disponibilidad").Valor == "true" ? true : false;
                //Se saca el maestro del compromiso - Inicia en la fila 2
                int totalFilas = sheet.LastRowNum;
                IRow fila = sheet.GetRow(2);
                if (fila != null)
                {
                    if (fila.PhysicalNumberOfCells >= 4)
                    {
                        noCelda = 3;
                        if (fila.GetCell(0) != null)
                        {
                            CA_BeneficiariosCuentas beneficiario = new CA_BeneficiariosCuentas();
                            celda = "A";
                            if (!String.IsNullOrEmpty(fila.GetCell(0).StringCellValue.Trim()))
                            {
                                ma_compromiso.Fecha = Convert.ToDateTime(fila.GetCell(0).StringCellValue);
                                Fecha = ma_compromiso.Fecha.Value;
                                anio = Convert.ToInt16(ma_compromiso.Fecha.Value.Year.ToString().Substring(2, 2));
                                errorMensaje = "3";
                                String AnioEjercicio = new ParametrosDAL().GetByID(x => x.Descripcion == "Ejercicio contable").Valor;
                                if (AnioEjercicio != Fecha.Year.ToString())
                                    throw new ArgumentException("El año no corresponde al ejercicio");
                                mes = ma_compromiso.Fecha.Value.Month;
                                if (seccion == 2)//Se valida que el mes del archivo de Nómina sea igual que el del Compromiso
                                {
                                    errorMensaje = "4";
                                    Ma_Compromisos compromisoExistente = compromisosDal.GetByID(x => x.No_Nomina == noNomina);
                                    if (compromisoExistente != null)
                                    {
                                        if (compromisoExistente.Fecha_Orden.Value.Month != mes)
                                            throw new ArgumentException("El mes de la Nómina no coincide con el Compromiso");
                                    }
                                }
                                errorMensaje = "5";
                                CierreMensualDAL cierreDal = new CierreMensualDAL();
                                if (cierreDal.GetByID(x => x.Id_Mes == mes).Contable == true)
                                    throw new ArgumentException("Fecha inválida. El mes está cerrado");
                            }
                            else
                                throw new ArgumentException(String.Format("La fecha de la celda {0}{1} no debe estar vacía", celda, noCelda));
                            celda = "B";
                            fila.GetCell(1).SetCellType(CellType.String);
                            if (!String.IsNullOrEmpty(fila.GetCell(1).StringCellValue.ToString().Trim()))
                            {
                                string id_beneficiario = fila.GetCell(1).StringCellValue.ToString();
                                ma_compromiso.Id_Beneficiario = Convert.ToInt16(id_beneficiario);

                            }
                            else if (String.IsNullOrEmpty(fila.GetCell(1).StringCellValue.Trim()))
                                throw new ArgumentException("El número del Beneficiario no puede estar vacío");
                            celda = "C";
                            if (!String.IsNullOrEmpty(fila.GetCell(2).StringCellValue.Trim()))
                            {
                                string cuenta = fila.GetCell(2).StringCellValue.Trim();
                                if (cuenta.Length != 20)
                                    throw new ArgumentException("La Cuenta del Beneficiario debe ser de 20 caracteres");
                                idBeneficiario = ma_compromiso.Id_Beneficiario.Value;
                                errorMensaje = "6";
                                beneficiario = beneficiariosDal.GetByID(x => x.Id_Beneficiario == idBeneficiario && x.Id_Cuenta == cuenta);
                                if (beneficiario == null)
                                    throw new ArgumentException("El Beneficiario no se encuentra registrado en el sistema");
                                else
                                {
                                    if (beneficiario.Id_Cuenta.Trim() != cuenta)
                                        throw new ArgumentException(String.Format("La Cuenta {0} del Beneficiario no corresponde a la registrada en el sistema", cuenta));
                                }
                                CuentasDAL cDal = new CuentasDAL();
                                errorMensaje = "7";
                                CA_Cuentas c = cDal.GetByID(x => x.Id_Cuenta == cuenta);
                                if (c.Nivel == false)
                                    throw new ArgumentException(String.Format("La Cuenta {0} no es de último nivel", cuenta));
                                ma_compromiso.Id_Cuenta_Beneficiario = cuenta;

                            }
                            else if (String.IsNullOrEmpty(fila.GetCell(2).NumericCellValue.ToString().Trim()))
                                throw new ArgumentException("La Cuenta del Beneficiario no puede estar vacía");
                            celda = "D";
                            if (String.IsNullOrEmpty(fila.GetCell(3).StringCellValue.Trim()))
                                throw new ArgumentException(String.Format("La Descripción del Compromiso no debe estar vacía"));
                            else
                            {
                                ma_compromiso.Observaciones = fila.GetCell(3).StringCellValue.Trim();
                                Observaciones = ma_compromiso.Observaciones;
                            }
                        }
                    }
                }
            }
            else
                throw new ArgumentException(String.Format("El formato del archivo no es válido"));
            encabezado = sheet.GetRow(8);
            totalCeldas = encabezado.LastCellNum;
            //Verificar el detalle de la nómina
            noCelda = 8;
            FuncionDAL funcionDal = new FuncionDAL();
            if (totalCeldas >= 17)//Se pone eso para saber que al menos hay 17 celdas y son las que tienen valores
            {
                int totalFilas = sheet.LastRowNum;
                bool hayValores = false;
                for (int i = 8; i <= totalFilas - 1; i++)
                {
                    CompromisoNomina tempCompromiso = new CompromisoNomina();
                    noCelda++;
                    IRow fila = sheet.GetRow(i);
                    if (fila != null)
                    {
                        if (fila.GetCell(13) != null)
                        {
                            celda = "N";
                            fila.GetCell(13).SetCellType(CellType.String);
                            string objeto = fila.GetCell(13).StringCellValue.Trim();
                            if (objeto.Length > 0) //Existe un objeto de gasto en la fila
                            {
                                if (objeto.Length != 5)
                                    throw new ArgumentException(String.Format("El Objeto de Gasto {0} de la celda {1}{2} debe ser de 5 caracteres", objeto, celda, noCelda));
                                else
                                {
                                    hayValores = true;
                                    tempCompromiso.Id_ObjetoG = objeto;
                                    celda = "A";
                                    if (fila.GetCell(0) != null)//Centro gestor
                                    {
                                        fila.GetCell(0).SetCellType(CellType.String);
                                        if (!String.IsNullOrEmpty(fila.GetCell(0).StringCellValue.Trim()))
                                        {
                                            errorMensaje = "8";
                                            string centroGestor = fila.GetCell(0).StringCellValue.Trim();
                                            if (centroDal.GetByID(x => x.Id_Area == centroGestor).UltimoNivel.Value == false)
                                                throw new ArgumentException(String.Format("El Centro Gestor {0} no es de último nivel", centroGestor));
                                            tempCompromiso.Id_Area = centroGestor;
                                        }
                                    }
                                    else
                                        throw new ArgumentException(String.Format("El Centro Gestor de la celda {0}{1} no debe estar vacía", celda, noCelda));
                                    celda = "B";
                                    if (fila.GetCell(1) != null)//Función
                                    {
                                        fila.GetCell(1).SetCellType(CellType.String);
                                        if (!String.IsNullOrEmpty(fila.GetCell(1).StringCellValue.Trim()))
                                        {
                                            string funcion = fila.GetCell(1).StringCellValue.Trim();
                                            if (funcion[funcion.Length - 1] == '0')
                                                throw new ArgumentException(String.Format("El último dígito de la Función {0} de la celda {1}{2} debe ser diferente de 0", funcion, celda, noCelda));
                                            tempCompromiso.Id_Funcion = funcion;
                                        }
                                        else
                                            throw new ArgumentException(String.Format("La Función de la celda {0}{1} no debe estar vacía", celda, noCelda));
                                    }
                                    else
                                        throw new ArgumentException(String.Format("La Función de la celda {0}{1} no debe estar vacía", celda, noCelda));
                                    celda = "C";
                                    if (fila.GetCell(2) != null)//Compromiso
                                    {
                                        fila.GetCell(2).SetCellType(CellType.String);
                                        string compromiso = fila.GetCell(2).StringCellValue.Trim();
                                        if (String.IsNullOrEmpty(compromiso))
                                            throw new ArgumentException(String.Format("El Compromiso de la celda {0}{1} no debe estar vacío", celda, noCelda));
                                        tempCompromiso.Id_Actividad = compromiso;
                                    }
                                    celda = "D";
                                    if (fila.GetCell(3) != null)//Clasificación Programática
                                    {
                                        fila.GetCell(3).SetCellType(CellType.String);
                                        string clasificacion = fila.GetCell(3).StringCellValue.Trim();
                                        if (String.IsNullOrEmpty(clasificacion))
                                            throw new ArgumentException(String.Format("La Clasificación Programática de la celda {0}{1} no debe estar vacía", celda, noCelda));
                                        tempCompromiso.Id_ClasificacionP = clasificacion;
                                    }
                                    celda = "E";
                                    if (fila.GetCell(4) != null)//Programa
                                    {
                                        fila.GetCell(4).SetCellType(CellType.String);
                                        string programa = fila.GetCell(4).StringCellValue.Trim();
                                        if (!String.IsNullOrEmpty(programa))
                                        {
                                            errorMensaje = "9";
                                            MaPresupuestoEgDAL presDal = new MaPresupuestoEgDAL();
                                            if (presDal.Get(x => x.Id_Programa == programa).ToList().Count == 0)
                                                throw new ArgumentException(String.Format("El Programa {0} de la celda {1}{2} no cuenta con un Presupuesto de Egresos", programa, celda, noCelda));
                                        }
                                        else
                                            throw new ArgumentException(String.Format("El Programa de la celda {0}{1} no debe estar vacía", celda, noCelda));
                                        tempCompromiso.Id_Programa = programa;
                                    }
                                    celda = "F";
                                    if (fila.GetCell(5) != null)//Programa
                                    {
                                        fila.GetCell(5).SetCellType(CellType.String);
                                        string proyecto = fila.GetCell(5).StringCellValue.Trim();
                                        if (String.IsNullOrEmpty(proyecto))
                                            throw new ArgumentException(String.Format("El Proyecto de la celda {0}{1} no debe estar vacía", celda, noCelda));
                                        tempCompromiso.Id_Proceso = proyecto;
                                    }
                                    celda = "G";
                                    if (fila.GetCell(6) != null)//Tipo Meta
                                    {
                                        fila.GetCell(6).SetCellType(CellType.String);
                                        string tipoMeta = fila.GetCell(6).StringCellValue;
                                        if (String.IsNullOrEmpty(tipoMeta))
                                            throw new ArgumentException(String.Format("El Tipo de Meta de la celda {0}{1} no debe estar vacía", celda, noCelda));
                                        tempCompromiso.Id_TipoMeta = tipoMeta;
                                    }
                                    celda = "H";
                                    if (fila.GetCell(7) != null)//Actividar MIR
                                    {
                                        fila.GetCell(7).SetCellType(CellType.String);
                                        string actividad = fila.GetCell(7).StringCellValue;
                                        if (!String.IsNullOrEmpty(actividad))
                                            tempCompromiso.Id_ActividadMIR = actividad;
                                        else
                                            throw new ArgumentException(String.Format("La Actividad de la celda {0}{1} no debe estar vacía", celda, noCelda));
                                    }
                                    celda = "I";
                                    if (fila.GetCell(8) != null)//Acción u Obra
                                    {
                                        fila.GetCell(8).SetCellType(CellType.String);
                                        string accion = fila.GetCell(8).StringCellValue;
                                        if (!String.IsNullOrEmpty(accion))
                                            tempCompromiso.Id_Accion = accion;
                                        else
                                            throw new ArgumentException(String.Format("La Acción de la celda {0}{1} no debe estar vacía", celda, noCelda));
                                    }
                                    celda = "J";
                                    if (fila.GetCell(9) != null)//Dimensión Geográfica
                                    {
                                        fila.GetCell(9).SetCellType(CellType.String);
                                        string dimension = fila.GetCell(9).StringCellValue;
                                        if (!String.IsNullOrEmpty(dimension))
                                            tempCompromiso.Id_Alcance = dimension;
                                        else
                                            throw new ArgumentException(String.Format("La Dimensión Geográfica de la celda {0}{1} no debe estar vacía", celda, noCelda));
                                    }
                                    celda = "K";
                                    if (fila.GetCell(10) != null)//Tipo de Gasto
                                    {
                                        fila.GetCell(10).SetCellType(CellType.String);
                                        string tipoG = fila.GetCell(10).StringCellValue;
                                        if (!String.IsNullOrEmpty(tipoG))
                                            tempCompromiso.Id_TipoG = tipoG;
                                        else
                                            throw new ArgumentException(String.Format("El Tipo de Gasto de la celda {0}{1} no debe estar vacía", celda, noCelda));
                                    }
                                    celda = "L";
                                    if (fila.GetCell(11) != null)//Fuente de Financiamiento
                                    {
                                        fila.GetCell(11).SetCellType(CellType.String);
                                        string fuente = fila.GetCell(11).StringCellValue;
                                        if (!String.IsNullOrEmpty(fuente))
                                            tempCompromiso.Id_Fuente = fuente;
                                        else
                                            throw new ArgumentException(String.Format("La Fuente de Financiamiento de la celda {0}{1} no debe estar vacía", celda, noCelda));
                                    }
                                    celda = "M";
                                    if (fila.GetCell(12) != null)//Año de Financiamiento
                                    {
                                        fila.GetCell(12).SetCellType(CellType.String);
                                        string AnioFin = fila.GetCell(12).StringCellValue;
                                        if (!String.IsNullOrEmpty(AnioFin))
                                        {
                                            //if (anio.ToString() != AnioFin)
                                            //    throw new ArgumentException(String.Format("El Año de Financiamiento {0} de la celda {1}{2} debe coincidir con la fecha de la Nómina", AnioFin, celda, noCelda));
                                            tempCompromiso.AnioFin = AnioFin;
                                        }
                                        else
                                            throw new ArgumentException(String.Format("El Año de Financiamiento de la celda {0}{1} no puede estar vacío", celda, noCelda));
                                    }
                                    celda = "N";
                                    if (fila.GetCell(13) != null)//Objeto de Gasto
                                    {
                                        fila.GetCell(13).SetCellType(CellType.String);
                                        if (!String.IsNullOrEmpty(objeto))
                                        {
                                            errorMensaje = "10";
                                            DeDisponibilidadDAL deDispDal = new DeDisponibilidadDAL();
                                            if (deDispDal.Get(x => x.Id_ObjetoG == objeto).ToList().Count == 0)
                                                throw new ArgumentException(String.Format("El Objeto de Gasto {0} de la celda {1}{2} no cuenta con disponibilidad", objeto, celda, noCelda));
                                            tempCompromiso.Id_ObjetoG = objeto;
                                            tempCompromiso.Id_ClavePresupuesto = tempCompromiso.Id_Area + tempCompromiso.Id_Funcion +
                                                tempCompromiso.Id_Actividad + tempCompromiso.Id_ClasificacionP + tempCompromiso.Id_Programa +
                                                tempCompromiso.Id_Proceso + tempCompromiso.Id_TipoMeta + tempCompromiso.Id_ActividadMIR + tempCompromiso.Id_Accion +
                                                tempCompromiso.Id_Alcance + tempCompromiso.Id_TipoG + tempCompromiso.Id_Fuente + tempCompromiso.AnioFin + tempCompromiso.Id_ObjetoG;
                                            MaPresupuestoEgDAL pDal = new MaPresupuestoEgDAL();
                                            errorMensaje = "11";
                                            MA_PresupuestoEg presupuesto = pDal.GetByID(x => x.Id_ClavePresupuesto == tempCompromiso.Id_ClavePresupuesto);
                                            if (presupuesto == null)
                                                throw new ArgumentException(String.Format("La Clave Presupuestal {0} de la fila {1} no existe en el presupuesto de egresos", tempCompromiso.Id_ClavePresupuesto, noCelda));

                                        }
                                        else
                                            throw new ArgumentException(String.Format("El Objeto de Gasto de la celda {0}{1} no debe estar vacía", celda, noCelda));
                                        if (fila.GetCell(15) == null)
                                            throw new ArgumentException(String.Format("El Importe del Objeto de Gasto de la celda {0}{1} debe ser un cargo. Revise", celda, noCelda));
                                    }
                                    celda = "O";
                                    if (fila.GetCell(14) != null)//Cuenta de gasto
                                    {
                                        fila.GetCell(14).SetCellType(CellType.String);
                                        string cuentaGasto = fila.GetCell(14).StringCellValue;
                                        if (!String.IsNullOrEmpty(cuentaGasto))
                                        {
                                            errorMensaje = "12";
                                            CuentasDAL cuentasDal = new CuentasDAL();
                                            if (cuentasDal.GetByID(x => x.Id_Cuenta == cuentaGasto && x.Id_ObjetoG == objeto) == null)
                                                throw new ArgumentException(String.Format("El Objeto de Gasto {0} de la celda {1}{2} no se encuentra ligada con la Cuenta de Gasto {3}", objeto, celda, noCelda, cuentaGasto));
                                            tempCompromiso.Cuenta = cuentaGasto;
                                        }
                                        else
                                            throw new ArgumentException(String.Format("El Objeto de Gasto de la celda {0}{1} no puede estar vacío", celda, noCelda));
                                    }
                                    celda = "P";
                                    if (fila.GetCell(15) != null)//Cuenta de gasto
                                    {
                                        fila.GetCell(15).SetCellType(CellType.String);
                                        decimal cargos = Convert.ToDecimal(fila.GetCell(15).StringCellValue);
                                        TotalCargos += cargos;
                                        tempCompromiso.Cargo = cargos;
                                        errorMensaje = "13";
                                        DeDisponibilidadDAL dispDal = new DeDisponibilidadDAL();
                                        DE_Disponibilidad disp = dispDal.GetByID(x => x.Id_ClavePresupuesto == tempCompromiso.Id_ClavePresupuesto && x.Mes == mes);
                                        if (disp != null)
                                        {
                                            if (sinDisponibilidad == false && cargos > disp.Disponible)
                                                throw new ArgumentException(String.Format("El Cargo {0} de la celda {1}{2} no tiene disponibilidad, la Nómina no ha sido importada", cargos, celda, noCelda));
                                            else if (sinDisponibilidad == true && cargos > disp.Disponible)
                                            {
                                                tempCompromiso.Disponibilidad = false;
                                                existeSinDisponibilidad = true;
                                            }
                                            else
                                                tempCompromiso.Disponibilidad = true;
                                        }
                                    }
                                }
                            }
                        }
                        else//No tiene Objeto de Gasto, se busca la Cuenta (Gasto, Retenciones etc...)
                        {
                            celda = "O";
                            if (fila.GetCell(14) != null)//Compromiso
                            {
                                fila.GetCell(14).SetCellType(CellType.String);
                                string cuenta = fila.GetCell(14).StringCellValue.Trim();
                                if (String.IsNullOrEmpty(cuenta))
                                    throw new ArgumentException(String.Format("La Cuenta de la celda {0}{1} no debe estar vacío", celda, noCelda));
                                CuentasDAL cuentasDal = new CuentasDAL();
                                errorMensaje = "14";
                                if (cuentasDal.GetByID(x => x.Id_Cuenta == cuenta) == null)
                                    throw new ArgumentException(String.Format("La Cuenta {0} en la celda {1}{2} no se encuentra registrada en el sistema", cuenta, celda, noCelda));
                                tempCompromiso.Cuenta = cuenta;
                            }
                            celda = "Q";
                            if (fila.GetCell(16) != null)//Compromiso
                            {
                                fila.GetCell(16).SetCellType(CellType.String);
                                string abono = fila.GetCell(16).StringCellValue.Trim();
                                if (String.IsNullOrEmpty(abono))
                                    throw new ArgumentException(String.Format("El Abono de la celda {0}{1} no debe estar vacío", celda, noCelda));
                                tempCompromiso.Abono = Convert.ToDecimal(abono);
                                TotalAbonos += Convert.ToDecimal(abono);
                            }
                        }
                        listaCompromisos.Add(tempCompromiso);
                    }

                }

                /*
                 * Verificar suma de cargos y abonos
                 */
                if (TotalCargos != TotalAbonos)
                    throw new ArgumentException(String.Format("El total de Cargos {0} es diferente que el total de Abonos {1}", TotalCargos, TotalAbonos));
                exito = true;
                if (opcion == 1 && seccion == 1)//Se realiza la inserción en maestros de compromisos
                {
                    errorMensaje = "15";
                    Session.Add("ErrorSQL", 1);
                    //Inserción del maestro de compromiso
                    ma_compromiso.Id_TipoCompromiso = 5;
                    ma_compromiso.Id_FolioCompromiso = cBL.getNextId(5);
                    ma_compromiso.Cargos = TotalCargos;
                    ma_compromiso.Abonos = TotalAbonos;
                    ma_compromiso.No_Nomina = noNomina;
                    ma_compromiso.TipoNomina = tipoNomina;
                    ma_compromiso.Usuario_Orden = Usuario.NombreCompleto;
                    ma_compromiso.Usuario_Act = (short)Usuario.IdUsuario;
                    ma_compromiso.Estatus = (existeSinDisponibilidad == true) ? Convert.ToInt16(5) : Convert.ToInt16(1);
                    ma_compromiso.Fecha_Orden = ma_compromiso.Fecha;
                    ma_compromiso.Historial = true;
                    ma_compromiso.Adquisicion = false;
                    ma_compromiso.Fecha = ma_compromiso.Fecha;
                    compromisosDal.Insert(ma_compromiso);
                    compromisosDal.Save();
                    cBL.updateNextId(ma_compromiso.Id_TipoCompromiso);
                    Session.Add("FolioCompromiso", ma_compromiso.Id_FolioCompromiso);
                    foreach (CompromisoNomina item in listaCompromisos)
                    {
                        errorMensaje = "17";
                        InsertarDetalleCompromiso(ma_compromiso.Id_TipoCompromiso, ma_compromiso.Id_FolioCompromiso, item.Cuenta, item.Id_ClavePresupuesto, item.Cuenta[0] == '2' || item.Cuenta[0] == '1' ? item.Abono : item.Cargo,
                            item.Cuenta[0] == '2' || item.Cuenta[0] == '1' ? Convert.ToByte(2) : Convert.ToByte(1), item.Id_ClavePresupuesto != null ? item.Disponibilidad : true);
                    }
                    if (existeSinDisponibilidad == false)//Si no hubo registros sin disponibilidad se genera el folio comprometido
                    {
                        errorMensaje = "16";
                        ma_compromiso = compromisosDal.GetByID(x => x.Id_TipoCompromiso == ma_compromiso.Id_TipoCompromiso && x.Id_FolioCompromiso == ma_compromiso.Id_FolioCompromiso);
                        //String[] res = new ProceduresDAL().Pa_AutorizarPartidasCompromiso(ma_compromiso.Id_TipoCompromiso, ma_compromiso.Id_FolioCompromiso, (short)Usuario.IdUsuario);
                        String[] res = new ProceduresDAL().Pa_Genera_PolizaOrden_Comprometido(ma_compromiso.Id_TipoCompromiso, ma_compromiso.Id_FolioCompromiso, (short)Usuario.IdUsuario);
                        ma_compromiso.Id_MesPO_Comprometido = Convert.ToByte(res[0]);
                        ma_compromiso.Id_FolioPO_Comprometido = Convert.ToInt16(res[1]);
                        compromisosDal.Update(ma_compromiso);
                        compromisosDal.Save();
                    }
                    else
                    {
                        Session.Add("error", "Existe al menos un compromiso sin disponibilidad");
                    }
                    Session.Add("Exito", true);
                }
                else if (opcion == 1 && seccion == 2) //Se realiza la inserción
                {
                    Session.Add("ErrorSQL", 1);
                    /*
                     * Si no existe el compromiso se registra uno nuevo
                     */
                    errorMensaje = "18";
                    ma_compromiso = compromisosDal.GetByID(x => x.No_Nomina == noNomina);
                    if (ma_compromiso == null)
                    {
                        InsertarMaestroCompromiso(existeSinDisponibilidad, listaCompromisos, TotalCargos, TotalAbonos, noNomina, tipoNomina, Fecha, idBeneficiario);
                    }
                    /*
                     * Si ya existe un compormiso 
                     */
                    if (ma_compromiso != null)
                    {
                        errorMensaje = "19";
                        ma_compromiso.Fecha = Fecha;
                        ma_compromiso.Fecha_Orden = Fecha;
                        ma_compromiso.Cargos = TotalCargos;
                        ma_compromiso.Abonos = TotalAbonos;
                        ma_compromiso.Observaciones = Observaciones;
                        //1.- Con uno que no tenga disponibilidad se afecta el compromiso a estatus 5
                        if (existeSinDisponibilidad)//Si hubo registros sin disponibilidad se pone en autorizacioón
                        {
                            ma_compromiso.Estatus = 5;//Se pone en autorización
                            Session.Add("error", "Existe al menos un compromiso sin disponibilidad");
                        }
                        errorMensaje = "20";
                        if (ma_compromiso.Id_FolioPO_Comprometido != null)
                        {
                            new ProceduresDAL().PA_ActualizaPoliza_Orden_Comprometido(ma_compromiso.Id_TipoCompromiso, ma_compromiso.Id_FolioCompromiso, (short)Usuario.IdUsuario);
                        }
                        else
                        {
                            errorMensaje = "21";
                            String[] res = new ProceduresDAL().Pa_Genera_PolizaOrden_Comprometido(ma_compromiso.Id_TipoCompromiso, ma_compromiso.Id_FolioCompromiso, (short)Usuario.IdUsuario);
                            ma_compromiso.Id_MesPO_Comprometido = Convert.ToByte(res[0]);
                            ma_compromiso.Id_FolioPO_Comprometido = Convert.ToInt16(res[1]);
                        }
                        errorMensaje = "22";
                        compromisosDal.Update(ma_compromiso);
                        compromisosDal.Save();
                        Session.Add("FolioCompromiso", ma_compromiso.Id_FolioCompromiso);
                        //2.- Los detalles del compromiso se eliminan y se insertan los nuevos en el detalle compromiso y h
                        DeCompromisosDAL DALDeCompromiso = new DeCompromisosDAL();
                        errorMensaje = "23";
                        List<De_Compromisos> listaDetalles = DALDeCompromiso.Get(x => x.Id_TipoCompromiso == ma_compromiso.Id_TipoCompromiso && x.Id_FolioCompromiso == ma_compromiso.Id_FolioCompromiso).ToList();
                        foreach (De_Compromisos item in listaDetalles)
                        {
                            errorMensaje = "24";
                            DALDeCompromiso.Delete(x => x.Id_TipoCompromiso == item.Id_TipoCompromiso && x.Id_FolioCompromiso == item.Id_FolioCompromiso && x.Id_Registro == item.Id_Registro);
                            DALDeCompromiso.Save();
                        }
                        foreach (CompromisoNomina item in listaCompromisos)
                        {
                            errorMensaje = "25";
                            InsertarDetalleCompromiso(ma_compromiso.Id_TipoCompromiso, ma_compromiso.Id_FolioCompromiso, item.Cuenta, item.Id_ClavePresupuesto, item.Cuenta[0] == '2' || item.Cuenta[0] == '1' ? item.Abono : item.Cargo,
                                item.Cuenta[0] == '2' || item.Cuenta[0] == '1' ? Convert.ToByte(2) : Convert.ToByte(1), item.Id_ClavePresupuesto != null ? item.Disponibilidad : true);
                        }
                    }
                    Session.Add("Exito", true);
                }
            }
            listaCompromisos.ForEach(x=>x.Cuenta = x.Cuenta.Substring(0,1)+"-"+x.Cuenta.Substring(1,1)+"-"+x.Cuenta.Substring(2,1)+"-"+x.Cuenta.Substring(3,1)+"-"+x.Cuenta.Substring(4,1)
                    + "-" + x.Cuenta.Substring(5, 5) + "-" + x.Cuenta.Substring(10, 4) + "-" + x.Cuenta.Substring(14, 6));
            return listaCompromisos;
        }
        public void InsertarMaestroCompromiso(Boolean existeSinDisponibilidad, List<CompromisoNomina> listaCompromisos, decimal TotalCargos, decimal TotalAbonos, string noNomina, string tipoNomina, DateTime fecha, int Id_Beneficiario)
        {
            errorMensaje = "26";
            MaCompromisosDAL compromisosDal = new MaCompromisosDAL();
            CompromisosBL cBL = new CompromisosBL();
            UsuarioLogueado Usuario = Session["appUsuario"] as UsuarioLogueado;
            Ma_Compromisos ma_compromiso = new Ma_Compromisos();
            //Inserción del maestro de compromiso
            ma_compromiso.Id_TipoCompromiso = 5;
            ma_compromiso.Id_FolioCompromiso = cBL.getNextId(5);
            ma_compromiso.Cargos = TotalCargos;
            ma_compromiso.Abonos = TotalAbonos;
            ma_compromiso.Id_Beneficiario = Id_Beneficiario;
            ma_compromiso.No_Nomina = noNomina;
            ma_compromiso.TipoNomina = tipoNomina;
            ma_compromiso.Usuario_Orden = Usuario.NombreCompleto;
            ma_compromiso.Usuario_Act = (short)Usuario.IdUsuario;
            ma_compromiso.Estatus = (existeSinDisponibilidad == true) ? Convert.ToInt16(5) : Convert.ToInt16(1);
            ma_compromiso.Fecha_Orden = fecha;
            ma_compromiso.Historial = true;
            ma_compromiso.Fecha = fecha;
            ma_compromiso.Fecha_Orden = fecha;
            compromisosDal.Insert(ma_compromiso);
            compromisosDal.Save();
            cBL.updateNextId(ma_compromiso.Id_TipoCompromiso);
            Session.Add("FolioCompromiso", ma_compromiso.Id_FolioCompromiso);
           
            foreach (CompromisoNomina item in listaCompromisos)
            {
                errorMensaje = "28";
                InsertarDetalleCompromiso(ma_compromiso.Id_TipoCompromiso, ma_compromiso.Id_FolioCompromiso, item.Cuenta, item.Id_ClavePresupuesto, item.Cuenta[0] == '2' || item.Cuenta[0] == '1' ? item.Abono : item.Cargo,
                    item.Cuenta[0] == '2' || item.Cuenta[0] == '1' ? Convert.ToByte(2) : Convert.ToByte(1), item.Id_ClavePresupuesto != null ? item.Disponibilidad : true);
            }
            if (existeSinDisponibilidad == false)//Si no hubo registros sin disponibilidad se genera el folio comprometido
            {
                errorMensaje = "27";
                ma_compromiso = compromisosDal.GetByID(x => x.Id_TipoCompromiso == ma_compromiso.Id_TipoCompromiso && x.Id_FolioCompromiso == ma_compromiso.Id_FolioCompromiso);
                String[] res = new ProceduresDAL().Pa_Genera_PolizaOrden_Comprometido(ma_compromiso.Id_TipoCompromiso, ma_compromiso.Id_FolioCompromiso, (short)Usuario.IdUsuario);
                ma_compromiso.Id_MesPO_Comprometido = Convert.ToByte(res[0]);
                ma_compromiso.Id_FolioPO_Comprometido = Convert.ToInt16(res[1]);
                compromisosDal.Update(ma_compromiso);
                compromisosDal.Save();
            }
            else
            {
                Session.Add("error", "Existe al menos un compromiso sin disponibilidad");
            }
        }
        public void RecorrerDetalles(int Id_Compromiso, int Id_FolioCompromiso)
        {
            errorMensaje = "29";
            DeCompromisosDAL compDal = new DeCompromisosDAL();
            List<De_Compromisos> listaCompromisos = compDal.Get(x => x.Id_TipoCompromiso == Id_Compromiso && x.Id_FolioCompromiso == Id_FolioCompromiso).ToList();
            foreach (De_Compromisos item in listaCompromisos)
            {
                item.AfectaCompro = false;
                compDal.Update(item);
                compDal.Save();
            }
            MaCompromisosDAL mDal = new MaCompromisosDAL();
            errorMensaje = "30";
            Ma_Compromisos compromiso = mDal.GetByID(x => x.Id_FolioCompromiso == Id_FolioCompromiso && x.Id_TipoCompromiso == Id_Compromiso);
            if (compromiso != null)
            {
                compromiso.Estatus = 5;
                mDal.Update(compromiso);
                mDal.Save();
            }
        }

        public void InsertarDetalleCompromiso(short Id_TipoCompromiso, int Id_FolioCompromiso, string Cuenta, string Id_ClavePresupuesto, decimal Importe, byte Id_Movimiento, bool Disponibilidad)
        {
            errorMensaje = "31";
            De_Compromisos detalle = new De_Compromisos();
            DeCompromisosBL cBL = new DeCompromisosBL();
            detalle.Id_TipoCompromiso = Id_TipoCompromiso;
            detalle.Id_FolioCompromiso = Id_FolioCompromiso;
            detalle.Id_Registro = (short)cBL.getNextId(Id_TipoCompromiso, Id_FolioCompromiso);
            detalle.Id_Cuenta = Cuenta;
            detalle.Id_ClavePresupuesto = Id_ClavePresupuesto;
            detalle.Importe = Importe;
            detalle.Id_Movimiento = Id_Movimiento;
            detalle.Disponibilidad = Disponibilidad;
            detalle.AfectaCompro = Disponibilidad;
            DeCompromisosDAL compDal = new DeCompromisosDAL();
            compDal.Insert(detalle);
            compDal.Save();
            if (Disponibilidad == false) //Si no hay disponibilidad entonces todos los detalles anteriores se cambia el estado AfectaCompro a falso y el maestro a Autorización
            {
                errorMensaje = "32";
                RecorrerDetalles(detalle.Id_TipoCompromiso, detalle.Id_FolioCompromiso);
            }
            InsertarHistorialCompromiso(Id_TipoCompromiso, Id_FolioCompromiso, detalle.Id_Registro, Cuenta, Id_ClavePresupuesto, Importe, Id_Movimiento, Disponibilidad);
        }

        public void InsertarHistorialCompromiso(short Id_TipoCompromiso, int Id_FolioCompromiso, int Id_Registro, string Cuenta, string Id_ClavePresupuesto, decimal Importe, byte Id_Movimiento, bool Disponibilidad)
        {
            errorMensaje = "33";
            De_Compromisos_H historial = new De_Compromisos_H();
            DeCompromisosHBL cBL = new DeCompromisosHBL();
            historial.Id_TipoCompromiso = Id_TipoCompromiso;
            historial.Id_FolioCompromiso = Id_FolioCompromiso;
            historial.Id_Registro = (short)Id_Registro;
            historial.Id_Historial = (byte)cBL.getNextId(Id_TipoCompromiso, Id_FolioCompromiso, Id_Registro);
            historial.Id_Cuenta = Cuenta;
            historial.Id_ClavePresupuesto = Id_ClavePresupuesto;
            historial.Importe = Importe;
            historial.Id_Movimiento = Id_Movimiento;
            historial.Disponibilidad = Disponibilidad;
            historial.AfectaCompro = Disponibilidad;
            DeCompromisosHDAL compromisoDal = new DeCompromisosHDAL();
            compromisoDal.Insert(historial);
            compromisoDal.Save();
        }
        #endregion
        #region Excel de Nómina
        public ActionResult Nomina()
        {
            var error = Session["error"];
            Session.Remove("error");
            var exito = Session["Exito"];
            Session.Remove("error");
            if (exito != null)
                ViewBag.Exito = exito;
            var folioCompromiso = Session["FolioCompromiso"];
            Session.Remove("FolioCompromiso");
            if (folioCompromiso != null)
                ViewBag.FolioCompromiso = folioCompromiso;
            List<CompromisoNomina> lista = (List<CompromisoNomina>)Session["listaNomina"];
            Session.Remove("listaNomina");
            if (error != null)
            {
                if (!String.IsNullOrEmpty(error.ToString()))
                    ModelState.AddModelError("", error.ToString());
            }

            if (lista != null && lista.Count() > 0)
                return View(lista.OrderByDescending(x => x.Cuenta));
            return View(new List<CompromisoNomina>());
        }
        [HttpPost]
        public ActionResult Nomina(FormCollection form)
        {
            try
            {
                Session.Add("error", "");
                Session.Add("exito", false);
                List<CompromisoNomina> listaNomina = new List<CompromisoNomina>();
                if (Request.Files["archivoExcel"].ContentLength > 0)
                {
                    Uri url = new Uri(ConfigurationManager.AppSettings.Get("urlFtp"));
                    string usuario = ConfigurationManager.AppSettings.Get("ftpUser");
                    string password = ConfigurationManager.AppSettings.Get("ftpPass");
                    Ftp ftp = new Ftp(url, usuario, password);
                    var file = Request.Files["archivoExcel"];
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    MaCompromisosDAL compromisosDal = new MaCompromisosDAL();
                    if (extension == ".xlsx" || extension == ".xls")
                    {
                        String[] partesArchivo = file.FileName.ToString().Split('-');
                        if (partesArchivo.Count() != 4)
                            throw new ArgumentException("El nombre del archivo no es válido");
                        string noNomina = partesArchivo[3].Split('.')[0];
                        String AnioEjercicio = new ParametrosDAL().GetByID(x => x.Descripcion == "Ejercicio contable").Valor;
                        if (AnioEjercicio != noNomina.Substring(2, 4))
                            throw new ArgumentException("El año no corresponde al ejercicio");
                        string tipoNomina = partesArchivo[2];
                        if (partesArchivo[0].ToUpper() != "NOMINA")
                            throw new ArgumentException("El nombre del archivo no es válido");
                        //if (compromisosDal.Get(X => X.No_Nomina == noNomina).ToList().Count() == 0)
                        //    throw new ArgumentException("El Compromiso de esta Nómina no existe. Verifique");
                        Ma_Compromisos compromiso = compromisosDal.GetByID(X => X.No_Nomina == noNomina);
                        if (compromiso != null)
                        {
                            if (compromiso.Estatus != 1 && compromiso.Id_FolioPO_Comprometido == null && compromiso.Id_MesPO_Comprometido == null)
                                throw new ArgumentException("La Nómina aún no ha sido comprometida, no se puede importar el archivo. Revise");
                            MaContrarecibosDAL DALContrarecibos = new MaContrarecibosDAL();
                            Ma_Contrarecibos Contrarecibo = DALContrarecibos.GetByID(x => x.Id_TipoCR == compromiso.Id_TipoCR && x.Id_FolioCR == compromiso.Id_FolioCR);
                            if (Contrarecibo != null)
                                throw new ArgumentException("La Nómina no se puede importar porque cuenta con un Contrarecibo registrado. Revise");
                        }
                        ftp.ExisteDirectorio(ruta);
                        string nombreArchivo = ruta + "/" + file.FileName;
                        if (!ftp.UploadFTP(file.InputStream, nombreArchivo))
                            throw new ArgumentException("No es posible conectar con el servidor");
                        Byte[] archivo;
                        archivo = ftp.DownloadFTPtoByte(nombreArchivo);
                        ProcesarExcel(archivo, noNomina, tipoNomina, 0, 1);
                        if (exito)
                            listaNomina = ProcesarExcel(archivo, noNomina, tipoNomina, 1, 2);
                        ftp.EliminarArchivo(nombreArchivo);
                    }
                    else
                        throw new ArgumentException("El archivo no es un documento de Excel");
                }
                Session.Add("listaNomina", listaNomina);
                return RedirectToAction("Nomina");
            }
            catch (Exception ex)
            {
                var sesionError = Session["ErrorSQL"];
                Session.Remove("ErrorSQL");
                string error = "";
                if (sesionError != null)
                {
                    error = new Errores(ex.HResult, ex.Message).Mensaje;
                }
                else
                {
                    switch (ex.HResult)
                    {
                        case -2146233079:
                            error = String.Format("El formato de la celda {0}{1} no es válido, debe ser numérico", celda, noCelda);
                            break;
                        default:
                            error = String.Format(ex.Message);
                            break;
                    }
                }
                Session["error"] = String.Format("Ocurrió un error: {0}", error);
                return RedirectToAction("Nomina");
            }
        }

        #endregion
        #region Excel de Egresos
        public ActionResult Egresos()
        {
            var error = Session["error"];
            Session.Remove("error");
            var exito = Session["Exito"];
            Session.Remove("Exito");
            if (exito != null)
                ViewBag.Exito = exito;
            var contrarecibo = Session["Contrarecibo"];
            Session.Remove("Contrarecibo");
            if (exito != null)
                ViewBag.Exito = exito;
            if (contrarecibo != null)
                ViewBag.Contrarecibo = contrarecibo;
            List<Egresos> lista = (List<Egresos>)Session["listaEgresos"];
            Session.Remove("listaEgresos");
            if (error != null)
            {
                if (!String.IsNullOrEmpty(error.ToString()))
                    ModelState.AddModelError("", error.ToString());
            }
            if (lista != null && lista.Count() > 0)
                return View(lista.OrderByDescending(x => x.Cuenta));
            return View(new List<Egresos>());
        }

        [HttpPost]
        public ActionResult Egresos(FormCollection form)
        {
            try
            {
                Session.Add("error", "");
                Session.Add("exito", false);
                List<Egresos> listaEgresos = new List<Egresos>();
                if (Request.Files["archivoExcel"].ContentLength > 0)
                {
                    Uri url = new Uri(ConfigurationManager.AppSettings.Get("urlFtp"));
                    string usuario = ConfigurationManager.AppSettings.Get("ftpUser");
                    string password = ConfigurationManager.AppSettings.Get("ftpPass");
                    Ftp ftp = new Ftp(url, usuario, password);
                    var file = Request.Files["archivoExcel"];
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    if (extension == ".xlsx" || extension == ".xls")
                    {
                        String[] partesArchivo = file.FileName.ToString().Split('-');
                        if (partesArchivo.Count() != 4)
                            throw new ArgumentException("El nombre del archivo no es válido");
                        string noNomina = partesArchivo[3].Split('.')[0];
                        String AnioEjercicio = new ParametrosDAL().GetByID(x => x.Descripcion == "Ejercicio contable").Valor;
                        if (AnioEjercicio != noNomina.Substring(2, 4))
                            throw new ArgumentException("El año no corresponde al ejercicio");
                        string tipoNomina = partesArchivo[2];
                        ftp.ExisteDirectorio(ruta);
                        string nombreArchivo = ruta + "/" + file.FileName;
                        if (!ftp.UploadFTP(file.InputStream, nombreArchivo))
                            throw new ArgumentException("No es posible conectar con el servidor");
                        Byte[] archivo;
                        archivo = ftp.DownloadFTPtoByte(nombreArchivo);
                        ProcesarExcelEgresos(archivo, noNomina, tipoNomina);
                        if (exito)
                            listaEgresos = ProcesarExcelEgresos(archivo, noNomina, tipoNomina, 1);
                        ftp.EliminarArchivo(nombreArchivo);
                    }
                    else
                        throw new ArgumentException("El archivo no es un documento de Excel");
                }
                Session.Add("listaEgresos", listaEgresos);
                return RedirectToAction("Egresos");
            }
            catch (Exception ex)
            {
                var sesionError = Session["ErrorSQL"];
                Session.Remove("ErrorSQL");
                string error = "";
                if (sesionError != null)
                {
                    error = new Errores(ex.HResult, ex.Message).Mensaje;
                }
                else
                {
                    switch (ex.HResult)
                    {
                        case -2146233079:
                            error = String.Format("El formato de la celda {0}{1} no es válido, debe ser numérico", celda, noCelda);
                            break;
                        default:
                            error = String.Format(ex.Message);
                            break;
                    }
                }
                Session["error"] = String.Format("Ocurrió un error: {0}", error);
                return RedirectToAction("Egresos");
            }
        }
        #endregion

        /*
         * path1: arreglo con los datos del archivo de Excel
         * noNomina: # de nómina según el nombre del archivo
         * tipoNomina: tipo según nombre del archivo
         * opción: 1->Cuando se recorre el archivo en busca de errores, 2-> Cuando se validó que no hubo errores y se realiza la inserción en la base de datos
         */
        public List<Egresos> ProcesarExcelEgresos(Byte[] path1, string noNomina, string tipoNomina, int opcion = 0)
        {
            List<Egresos> ListaEgresos = new List<Egresos>();
            int anio = 0;
            int mes = 0;
            MaCompromisosDAL compromisosDal = new MaCompromisosDAL();
            CompromisosBL cBL = new CompromisosBL();
            UsuarioLogueado appUsuario = Session["appUsuario"] as UsuarioLogueado;
            BeneficiariosCuentasDAL beneficiariosDal = new BeneficiariosCuentasDAL();
            AreasDAL centroDal = new AreasDAL();
            De_CompromisosModel deCompromiso = new De_CompromisosModel();
            Ma_Compromisos ma_compromiso = compromisosDal.GetByID(x => x.No_Nomina == noNomina);
            MaContrarecibosDAL DALContrarecibos = new MaContrarecibosDAL();
            Ma_Polizas Poliza = new Ma_Polizas();
            MaPolizasDAL DALPoliza = new MaPolizasDAL();
            /*
             *  Buscar Ma_Compromisos (Id_TipoCR, Id_FolioCR) en Ma_Contrarecibos, 
             *  si no existe  -> "No se puede importar este archivo porque la nómina no se ha ejercido" : Buscar en Ma_Contrecibos y validar que tenga Id_MesPO_Ejercido y FolioEjercido y que no tenga los cancelados y el estatus = 1.
             */
            if (ma_compromiso == null)
                throw new ArgumentException(String.Format("No se encuentra registrado un Compromiso para la Nómina {0}.", noNomina));
            if (ma_compromiso.Id_TipoCR == null && ma_compromiso.Id_FolioCR == null)
                throw new ArgumentException(String.Format("No se puede importar este archivo porque la Nómina {0} no se ha ejercido.", noNomina));
            Ma_Contrarecibos Contrarecibo = DALContrarecibos.GetByID(x => x.Id_TipoCR == ma_compromiso.Id_TipoCR && x.Id_FolioCR == ma_compromiso.Id_FolioCR);
            if (Contrarecibo == null)
                throw new ArgumentException(String.Format("No se puede importar este archivo porque la Nómina {0} no se ha ejercido.", noNomina));
            if (Contrarecibo.Id_MesPO_Ejercido == null && Contrarecibo.Id_FolioPO_Ejercido == null)
                throw new ArgumentException(String.Format("No se puede importar este archivo porque la Nómina {0} no se ha ejercido.", noNomina));
            if (Contrarecibo.Id_MesPO_Ejercido_C != null && Contrarecibo.Id_FolioPO_Ejercido_C != null)
                throw new ArgumentException(String.Format("No se puede importar este archivo porque la Nómina {0} está cancelada.", noNomina));
            if (Contrarecibo.Id_EstatusCR == 3)
                throw new ArgumentException(String.Format("No se puede importar este archivo porque la Nómina {0} está cancelada.", noNomina));
            if (Contrarecibo.Id_EstatusCR == 2)
                throw new ArgumentException(String.Format("No se puede importar este archivo porque la Nómina {0} ya fue pagada.", noNomina));
            if (Contrarecibo.Id_EstatusCR != 1)
                throw new ArgumentException(String.Format("No se puede importar la Nómina {0}.", noNomina));
            /***********************************************************************************************************************************/
            DePolizasDAL compDal = new DePolizasDAL();
            UsuarioLogueado Usuario = Session["appUsuario"] as UsuarioLogueado;
            bool sinDisponibilidad;
            IWorkbook hssfwb;
            Stream stream = new MemoryStream(path1);
            hssfwb = WorkbookFactory.Create(stream);
            ISheet sheet = hssfwb.GetSheetAt(0);
            IRow encabezado = sheet.GetRow(2);
            int totalCeldas = encabezado.LastCellNum;
            decimal TotalCargos = 0;
            decimal TotalAbonos = 0;
            if (totalCeldas >= 2)
            {
                //Se saca el maestro - Inicia en la fila 2
                int totalFilas = sheet.LastRowNum;
                IRow fila = sheet.GetRow(2);
                if (fila != null)
                {
                    if (fila.PhysicalNumberOfCells >= 2)
                    {
                        noCelda = 3;
                        if (fila.GetCell(0) != null)
                        {
                            celda = "A";
                            if (!String.IsNullOrEmpty(fila.GetCell(0).StringCellValue.Trim()))
                            {
                                Poliza.Fecha = Convert.ToDateTime(fila.GetCell(0).StringCellValue);
                                anio = Convert.ToInt16(Poliza.Fecha.Year.ToString().Substring(2, 2));
                                mes = Poliza.Fecha.Month;
                                String AnioEjercicio = new ParametrosDAL().GetByID(x => x.Descripcion == "Ejercicio contable").Valor;
                                if (AnioEjercicio != Poliza.Fecha.Year.ToString())
                                    throw new ArgumentException("El año no corresponde al ejercicio");
                                if (ma_compromiso.Fecha.Value.Month < mes)
                                    throw new ArgumentException("El mes de la Nómina no puede ser menor que el Compromiso");
                                CierreMensualDAL cierreDal = new CierreMensualDAL();
                                if (cierreDal.GetByID(x => x.Id_Mes == mes).Contable == true)
                                    throw new ArgumentException("Fecha inválida. El mes está cerrado");
                            }
                            else
                                throw new ArgumentException(String.Format("La fecha de la celda {0}{1} no debe estar vacía", celda, noCelda));
                            celda = "B";
                            if (String.IsNullOrEmpty(fila.GetCell(1).StringCellValue.Trim()))
                                throw new ArgumentException(String.Format("La Descripción no debe estar vacía"));
                            else
                                Poliza.Descripcion = fila.GetCell(1).StringCellValue.Trim();
                        }
                    }
                }
            }
            else
                throw new ArgumentException(String.Format("El formato del archivo no es válido"));
            encabezado = sheet.GetRow(8);
            totalCeldas = encabezado.LastCellNum;
            //Verificar el detalle de la nómina
            noCelda = 8;
            if (totalCeldas >= 3)//Se pone eso para saber que al menos hay 3 celdas y son las que tienen valores
            {
                int totalFilas = sheet.LastRowNum;
                for (int i = 8; i <= totalFilas - 1; i++)
                {
                    Egresos TempEgreso = new Egresos();
                    bool CargoAbono = false;
                    noCelda++;
                    IRow fila = sheet.GetRow(i);
                    if (fila != null)
                    {
                        if (fila.GetCell(0) != null)
                        {
                            celda = "A";
                            fila.GetCell(0).SetCellType(CellType.String);
                            if (!String.IsNullOrEmpty(fila.GetCell(0).StringCellValue.Trim()))
                            {
                                string cuenta = fila.GetCell(0).StringCellValue.Trim();
                                if (cuenta.Length != 20)
                                    throw new ArgumentException(String.Format("La Cuenta {0} de la celda {1}{2} debe debe ser de 20 caracteres", cuenta, celda, noCelda));
                                CuentasDAL cDal = new CuentasDAL();
                                CA_Cuentas c = cDal.GetByID(x => x.Id_Cuenta == cuenta);
                                if (c == null)
                                    throw new ArgumentException(String.Format("La Cuenta {0} de la celda {1}{2} no se encuentra registrada en el sistema", cuenta, celda, noCelda));
                                if (c.Nivel == false)
                                    throw new ArgumentException(String.Format("La Cuenta {0} de la celda {1}{2} no es de último nivel", cuenta, celda, noCelda));
                                if (cuenta[0] == '4' || cuenta[0] == '5' || cuenta[0] == '8')
                                    throw new ArgumentException(String.Format("La Cuenta {0} de la celda {1}{2} no debe ser de Género 4,5 u 8", cuenta, celda, noCelda));
                                TempEgreso.Cuenta = cuenta;
                            }
                            else
                                throw new ArgumentException(String.Format("La Cuenta de la celda {0}{1} no puede estar vacía", celda, noCelda));
                        }
                        if (fila.GetCell(1) != null)
                        {
                            celda = "B";
                            fila.GetCell(1).SetCellType(CellType.String);
                            if (!String.IsNullOrEmpty(fila.GetCell(1).StringCellValue.Trim()))
                            {
                                decimal Cargo = Decimal.Round(Convert.ToDecimal(fila.GetCell(1).StringCellValue), 2);
                                if (Cargo > 0)
                                {
                                    CargoAbono = true;
                                    TempEgreso.Cargos = Cargo;
                                    TotalCargos += Cargo;
                                }
                            }
                        }
                        if (fila.GetCell(2) != null)
                        {
                            fila.GetCell(2).SetCellType(CellType.String);
                            if (!String.IsNullOrEmpty(fila.GetCell(2).StringCellValue.Trim()) && CargoAbono == false)
                            {
                                decimal Abono = Decimal.Round(Convert.ToDecimal(fila.GetCell(2).StringCellValue), 2);
                                if (Abono > 0)
                                {
                                    CargoAbono = true;
                                    TempEgreso.Abonos = Abono;
                                    TotalAbonos += Abono;
                                }
                            }
                            if (CargoAbono == false)
                                throw new ArgumentException(String.Format("La Cuenta de la celda {0}{1} debe tener un Cargo o un Abono", celda, noCelda));
                        }
                        ListaEgresos.Add(TempEgreso);
                    }
                }
                exito = true;
                if (TotalCargos != TotalAbonos)
                    throw new ArgumentException(String.Format("El total de Cargos {0} es diferente al total de Abonos {1}.", TotalCargos, TotalAbonos));
                if (TotalCargos > ma_compromiso.Cargos)
                    throw new ArgumentException(String.Format("El Importe {0} de la Póliza de Egresos no puede ser mayor que el Importe {1} del Compromiso.", TotalCargos, ma_compromiso.Cargos));
                if (opcion == 1)//Indica que todo salió bien y se va a insertar en la base de datos
                {
                    Session.Add("ErrorSQL", 1);
                    //Inserción en el maestro de Pólizas
                    Poliza.Id_TipoPoliza = 2;
                    //Poliza.Cargos = TotalCargos;
                    //Poliza.Abonos = TotalAbonos;
                    Poliza.Id_MesPoliza = (byte)Poliza.Fecha.Month;
                    Poliza.Id_FolioPoliza = new ProceduresDAL().Pa_Genera_FolioPoliza(2, (byte)Poliza.Fecha.Month);//Se regresará de un procedimiento almacenado
                    Poliza.Id_ClasPoliza = Poliza.Id_SubClasificaPol = 1;
                    DALPoliza.Insert(Poliza);
                    DALPoliza.Save();
                    foreach (Egresos item in ListaEgresos)
                    {
                        InsertarDetallePoliza(item, Poliza);
                    }
                    //Actualizar Contrarecibo
                    Contrarecibo.Id_EstatusCR = 2;
                    Contrarecibo.No_Cheque = 0;
                    Contrarecibo.Id_MesPolizaCH = (byte)Poliza.Fecha.Month;
                    Contrarecibo.Id_FolioPolizaCH = Poliza.Id_FolioPoliza;
                    Contrarecibo.Fecha_Pago = Poliza.Fecha;
                    Contrarecibo.Impreso_CH = true;
                    Contrarecibo.Usu_Pago = Usuario.NombreCompleto;
                    DALContrarecibos.Update(Contrarecibo);
                    DALContrarecibos.Save();
                    int Id_MesPoliza = 0;
                    int Id_FolioPoliza = 0;
                    Contrarecibo = DALContrarecibos.GetByID(x => x.Id_TipoCR == ma_compromiso.Id_TipoCR && x.Id_FolioCR == ma_compromiso.Id_FolioCR);
                    new ProceduresDAL().Pa_Genera_PolizaOrden_Pagado(Contrarecibo.Id_TipoCR, Contrarecibo.Id_FolioCR, (short)Usuario.IdUsuario, ref Id_MesPoliza, ref Id_FolioPoliza);
                    Contrarecibo.Id_MesPO_Pagado = (byte)Id_MesPoliza;
                    Contrarecibo.Id_FolioPO_Pagado = Id_FolioPoliza;
                    DALContrarecibos.Update(Contrarecibo);
                    DALContrarecibos.Save();
                    Session.Add("Exito", true);
                    Session.Add("Contrarecibo", Contrarecibo.Id_FolioCR);
                }
            }
            ListaEgresos.ForEach(x => x.Cuenta = x.Cuenta.Substring(0, 1) + "-" + x.Cuenta.Substring(1, 1) + "-" + x.Cuenta.Substring(2, 1) + "-" + x.Cuenta.Substring(3, 1) + "-" + x.Cuenta.Substring(4, 1)
                    + "-" + x.Cuenta.Substring(5, 5) + "-" + x.Cuenta.Substring(10, 4) + "-" + x.Cuenta.Substring(14, 6));
            return ListaEgresos;
        }

        public void InsertarDetallePoliza(Egresos Detalle, Ma_Polizas Maestro)
        {
            DePolizasDAL DALDePolizas = new DePolizasDAL();
            De_Polizas DetallePoliza = new De_Polizas();
            DetallePoliza.Id_TipoPoliza = 2;
            DetallePoliza.Estatus = 1;
            DetallePoliza.Id_Cuenta = Detalle.Cuenta;
            DetallePoliza.Id_Movimiento = Detalle.Cargos != 0 ? (byte)1 : (byte)2;
            DetallePoliza.Id_MesPoliza = Maestro.Id_MesPoliza;
            DetallePoliza.Id_FolioPoliza = Maestro.Id_FolioPoliza;
            DetallePoliza.Id_Registro = new PolizasBL().GetNextRegistroDetalle(2, Maestro.Id_FolioPoliza, Maestro.Id_MesPoliza);
            DetallePoliza.Fecha = Maestro.Fecha;
            DetallePoliza.Importe = Detalle.Cargos != 0 ? Detalle.Cargos : Detalle.Abonos;
            DALDePolizas.Insert(DetallePoliza);
            DALDePolizas.Save();
        }
    }
}
