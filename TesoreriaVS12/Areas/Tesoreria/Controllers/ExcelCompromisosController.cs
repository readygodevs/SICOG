using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.BL;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    public class ExcelCompromisosController : Controller
    {
        private String celda = "";
        private String tipoDato = "";
        private const String MENSAJEERROR = "Ocurrieron errores, favor de revisar";
        private ConvertHtmlToString GenerarPDF = new ConvertHtmlToString();
        private ImportacionNSICG2015BL importacion = new ImportacionNSICG2015BL();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult PagoProveedores()
        {
            var error = Session["error"];
            Session.Remove("error");
            var exito = Session["Exito"];
            Session.Remove("Exito");
            if (exito != null)
                ViewBag.Exito = exito;
            if (error != null)
            {
                if (!String.IsNullOrEmpty(error.ToString()))
                    ModelState.AddModelError("", error.ToString());
            }
            return View();
        }


        [HttpPost]
        public ActionResult validacion(FormCollection form, byte tipo, bool options)
        {
            try
            {
                Session.Add("celda", "");
                Session.Add("tipoDato", "");
                if (Request.Files["archivoExcel"].ContentLength > 0)
                {
                    HttpPostedFileBase file = Request.Files["archivoExcel"];
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    if (extension == ".xlsx" || extension == ".xls")
                    {
                        IWorkbook excel = null;
                        //varios
                        if (tipo == 0 ||tipo == 1 || tipo == 2 )
                        {
                            if (options)
                            {
                                excel = ProcesarExcelArrendamientosHonorariosyActivos(GetByteArrFromFrile(file), tipo);
                            }
                            else
                            {
                                excel = ValidarProcesarExcelArrendamientosHonorariosyActivos(GetByteArrFromFrile(file), tipo);

                            }
                        }

                        if (tipo == 3)
                        {
                            if (options)
                            {
                                excel = ProcesarExcelHonorarios(GetByteArrFromFrile(file), tipo);
                            }
                            else
                            {
                                excel = ValidarProcesarExcelHonorarios(GetByteArrFromFrile(file), tipo);

                            }
                        }
                        

                        //Anticipos y prestamos Sueldos no revolventes -Activo
                        if (tipo == 4)
                        {
                            if (options)
                            {
                                
                                 excel = ProcesarExcelCancelacionPasivosYAnticipos(GetByteArrFromFrile(file), 1);
                            }
                            else {
                                excel = ValidarProcesarExcelCancelacionPasivosYAnticipos(GetByteArrFromFrile(file), 1);

                            
                            }
                        }
                        //Cancelación de pasivos
                        if (tipo == 5)
                        {
                            if (options)
                            {
                                excel = ProcesarExcelCancelacionPasivosYAnticipos(GetByteArrFromFrile(file), 0);

                            }
                            else
                            {
                                 excel = ValidarProcesarExcelCancelacionPasivosYAnticipos(GetByteArrFromFrile(file), 0);

                            }
                        }
                        //Egresos no presupuestales
                        if (tipo == 6)
                        {
                            if (options)
                            {
                                 excel = ProcesarExcelEgresosNoPresupuestales(GetByteArrFromFrile(file));

                            }
                            else
                            {
                                excel = ValidarProcesarExcelEgresosNoPresupuestales(GetByteArrFromFrile(file));

                            }
                        }
                        // Fondos resolventes
                        if (tipo == 7)
                        {
                            if (options)
                            {
                                excel = ProcesarExcelGastosYFondos(GetByteArrFromFrile(file), 1);

                            }
                            else
                            {
                                excel = ValidarProcesarExcelGastosYFondos(GetByteArrFromFrile(file), 1);

                            }
                        }
                        // Gastos a comprobar -activo
                        if (tipo == 8)
                        {
                            if (options)
                            {
                                excel = ProcesarExcelGastosYFondos(GetByteArrFromFrile(file), 0);

                            }
                            else
                            {
                                excel = ValidarProcesarExcelGastosYFondos(GetByteArrFromFrile(file), 0);

                            }
                        }
                        
                        //Pago a proveedores
                        if (tipo == 9)
                        {
                            if (options)
                            {
                                 excel = ProcesarExcelPagoProveedores(GetByteArrFromFrile(file));

                            }
                            else
                            {
                                 excel = ValidarProcesarExcelPagoProveedores(GetByteArrFromFrile(file));

                            }
                        }
                        //Recibo de ingresos
                        if (tipo == 10)
                        {
                            if (options)
                            {
                                 excel = ProcesarExcelReciboIngresos(GetByteArrFromFrile(file));

                            }
                            else
                            {
                                 excel = ValidarProcesarExcelReciboIngresos(GetByteArrFromFrile(file));

                            }
                        }
                        //Honoarios puros 
                        if (tipo == 11)
                        { }
                        if (tipo == 12)
                        { }

                        MemoryStream output = new MemoryStream();
                        excel.Write(output);
                        return File(output.ToArray(),
                         "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", file.FileName + ".xlsx");
                    }
                    else
                    {
                        AddErrorToList("El archivo no es un documento de Excel");
                        ThrowError();
                    }
                }
                return RedirectToAction("PagoProveedores");
            }
            catch (Exception ex)
            {
                celda = Session["celda"].ToString();
                tipoDato = Session["tipoDato"].ToString();
                Session.Remove("celda");
                Session.Remove("tipoDato");
                string error = String.Empty;
                switch (ex.HResult)
                {
                    case -2146233079:
                    case -2146233033:
                        error = String.Format("El formato de la celda {0} de la hoja {1} no es válido, debe ser {2}", celda, GetHojaSession(), tipoDato);
                        break;
                    //case -2147024809:
                    //    error = "La validación de este archivo no generó ningún error.";
                    //    break;
                    default:
                        error = String.Format(ex.Message);
                        break;
                }
                if (MENSAJEERROR != error)
                    AddErrorToList(error);
                return RedirectToAction("ErroresExcel");
            }
        }


        //Validaciones
        //Opcion0-3
        private IWorkbook ValidarProcesarExcelCancelacionPasivosYAnticipos(byte[] fileData, byte tipo)
        {
            ClearErrorList();
            IWorkbook excelBook = WorkbookFactory.Create(new MemoryStream(fileData));
            ExcelCancelacionPasivos datosExcel = importacion.GetExcelCancelacionPasivos(excelBook, tipo);
            CuentasBancariasDAL cuentasBancariasDal = new CuentasBancariasDAL();
            UsuarioLogueado usuarioLog = Session["appUsuario"] as UsuarioLogueado;
            String nombreUsuario = usuarioLog.NombreCompleto;
            int idUsuario = usuarioLog.IdUsuario;
            short tipoContrarecibo = tipo == 0 ? (short)5 : (short)6;
            short tipoCompromiso = tipo == 0 ? (short)41 : (short)39;
            if (datosExcel.errores.Count() > 0)
            {
                SetListaErrores(datosExcel.errores);
                ThrowError();
            }
            else
            {
                datosExcel.errores.Add(String.Format("La validación de este archivo no generó ningún error."));
                SetListaErrores(datosExcel.errores);
                ThrowError();
            }

            return excelBook;
        }
        // opcion 4-5 
        ///<param name="fileData">Arreglo de Byte con el contenido del archivo. Ya no es necesario FTP.</param>
        ///<param name="tipo">Tipo del archivo -> 0.- Arrendamientos, 1.- Honorarios Puros, 2.- Cancelación de activos,3.- Honorarios Asimilables.</param>
        private IWorkbook ValidarProcesarExcelArrendamientosHonorariosyActivos(byte[] fileData, byte tipo)
        {
            ClearErrorList();
            IWorkbook excelBook = WorkbookFactory.Create(new MemoryStream(fileData));
            ExcelArrendamientosyHonorarios datosExcel = importacion.GetExcelArrendamientosyHonorarios(excelBook, tipo);
            UsuarioLogueado usuarioLog = Session["appUsuario"] as UsuarioLogueado;
            String nombreUsuario = usuarioLog.NombreCompleto;
            int idUsuario = usuarioLog.IdUsuario;
            if (datosExcel.errores.Count() > 0)
            {
                SetListaErrores(datosExcel.errores);
                ThrowError();
            }
            else
            {
                datosExcel.errores.Add(String.Format("La validación de este archivo no generó ningún error."));

                SetListaErrores(datosExcel.errores);
                ThrowError();
            }
        
            return excelBook;
        }
        ///<param name="tipo">Tipo del archivo -> 0.- Arrendamientos, 1.- Honorarios Puros, 2.- Cancelación de activos,3.- Honorarios Asimilables.</param>
        private IWorkbook ValidarProcesarExcelHonorarios(byte[] fileData, byte tipo)
        {
            ClearErrorList();
            IWorkbook excelBook = WorkbookFactory.Create(new MemoryStream(fileData));
            ExcelArrendamientosyHonorarios datosExcel = importacion.GetExcelHonorarios(excelBook, tipo);
            UsuarioLogueado usuarioLog = Session["appUsuario"] as UsuarioLogueado;
            String nombreUsuario = usuarioLog.NombreCompleto;
            int idUsuario = usuarioLog.IdUsuario;
            if (datosExcel.errores.Count() > 0)
            {
                SetListaErrores(datosExcel.errores);
                ThrowError();
            }
            else
            {
                datosExcel.errores.Add(String.Format("La validación de este archivo no generó ningún error."));

                SetListaErrores(datosExcel.errores);
                ThrowError();
            }

            return excelBook;
        }

        //Opcion
        private IWorkbook ValidarProcesarExcelGastosYFondos(byte[] fileData, byte tipo)
        {
            ClearErrorList();
            IWorkbook excelBook = WorkbookFactory.Create(new MemoryStream(fileData));
            ExcelFondosRevolventes datosExcel = importacion.GetExcelFondosRevolventes(excelBook, tipo);
            UsuarioLogueado usuarioLog = Session["appUsuario"] as UsuarioLogueado;
            String nombreUsuario = usuarioLog.NombreCompleto;
            int idUsuario = usuarioLog.IdUsuario;
            if (datosExcel.listaErrores.Count() > 0)
            {
                SetListaErrores(datosExcel.listaErrores);
                ThrowError();
            }
            else
            {
                datosExcel.listaErrores.Add(String.Format("La validación de este archivo no generó ningún error."));

                SetListaErrores(datosExcel.listaErrores);
                ThrowError();
            }

            return excelBook;
        }
        private IWorkbook ValidarProcesarExcelEgresosNoPresupuestales(byte[] fileData)
        {
            ClearErrorList();
            IWorkbook excelBook = WorkbookFactory.Create(new MemoryStream(fileData));
            ExcelEgresosNoPresupuestarios datosExcel = importacion.GetExcelEgresosNoPresupuestales(excelBook);
            UsuarioLogueado usuarioLog = Session["appUsuario"] as UsuarioLogueado;
            String nombreUsuario = usuarioLog.NombreCompleto;
            int idUsuario = usuarioLog.IdUsuario;
            if (datosExcel.errores.Count() > 0)
            {
                SetListaErrores(datosExcel.errores);
                ThrowError();
            }
            else
            {
                datosExcel.errores.Add(String.Format("La validación de este archivo no generó ningún error."));

                SetListaErrores(datosExcel.errores);
                ThrowError();
            }
           
            return excelBook;
        }

        ///</summary>
        ///<param name="fileData">Arreglo de Byte con el contenido del archivo. Ya no es necesario FTP.</param>
        private IWorkbook ValidarProcesarExcelPagoProveedores(byte[] fileData)
        {
            ClearErrorList();
            #region Variables
            ExcelPagoProveedor datosExcel = new ExcelPagoProveedor();
            UsuarioLogueado usuarioLog = Session["appUsuario"] as UsuarioLogueado;
            String nombreUsuario = usuarioLog.NombreCompleto;
            int idUsuario = usuarioLog.IdUsuario;
            CuentasBancariasDAL cuentasBancariasDal = new CuentasBancariasDAL();
            IWorkbook excelBook = WorkbookFactory.Create(new MemoryStream(fileData));
            #endregion
            datosExcel = importacion.GetExcelPagoProveedores(excelBook.NumberOfSheets, excelBook);
            if (datosExcel.listaErrores.Count() > 0)
            {
                SetListaErrores(datosExcel.listaErrores);
                ThrowError();
            }
            else
            {
                datosExcel.listaErrores.Add(String.Format("La validación de este archivo no generó ningún error."));

                SetListaErrores(datosExcel.listaErrores);
                ThrowError();
            }
          
            return excelBook;
        }
        /// <param name="fileData">Arreglo de Byte con el contenido del archivo. Ya no es necesario FTP.</param>
        private IWorkbook ValidarProcesarExcelReciboIngresos(byte[] fileData)
        {
            ClearErrorList();
            IWorkbook excelBook = WorkbookFactory.Create(new MemoryStream(fileData));
            ExcelReciboIngresos datosExcel = importacion.GetExcelReciboIngresos(excelBook);
            UsuarioLogueado usuarioLog = Session["appUsuario"] as UsuarioLogueado;
            String nombreUsuario = usuarioLog.NombreCompleto;
            MaRecibosDAL recibosDal = new MaRecibosDAL();
            int idUsuario = usuarioLog.IdUsuario;
            if (datosExcel.listaErrores.Count() > 0)
            {
                SetListaErrores(datosExcel.listaErrores);
                ThrowError();
            }
            else
            {
                datosExcel.listaErrores.Add(String.Format("La validación de este archivo no generó ningún error."));

                SetListaErrores(datosExcel.listaErrores);
                ThrowError();
            }
            
            return excelBook;
        }
        //Inserciones
        private IWorkbook ProcesarExcelCancelacionPasivosYAnticipos(byte[] fileData,byte tipo)
        {
            ClearErrorList();
            IWorkbook excelBook = WorkbookFactory.Create(new MemoryStream(fileData));
            ExcelCancelacionPasivos datosExcel = importacion.GetExcelCancelacionPasivos(excelBook, tipo);
            CuentasBancariasDAL cuentasBancariasDal = new CuentasBancariasDAL();
            UsuarioLogueado usuarioLog = Session["appUsuario"] as UsuarioLogueado;
            String nombreUsuario = usuarioLog.NombreCompleto;
            int idUsuario = usuarioLog.IdUsuario;
            short tipoContrarecibo = tipo == 0 ? (short)5 : (short)6;
            short tipoCompromiso = tipo == 0 ? (short)41 : (short)39;
            //if (datosExcel.errores.Count() > 0)
            //{
            //    SetListaErrores(datosExcel.errores);
            //    ThrowError();
            //}
            #region inserciones
            ISheet sheet = excelBook.GetSheetAt(0);
            IRow row = sheet.GetRow(0);
            ICell celda1 = row.CreateCell(10, CellType.String);
            celda1.SetCellValue("Folio Contrarecibo");
            int line = 1;
            foreach (var item in datosExcel.cancelaciones)
            {
                int nextFolioContrarecibo = importacion.getNextIdContrarecibo(tipoContrarecibo);
                importacion.insertarContrarecibo(item.fechaPago, item.descripcion, null, item.fuenteFinanciamiento, item.spei, (short)item.noCuentaBancaria
                    , nextFolioContrarecibo, item.importe, item.noCheque, nombreUsuario, tipoContrarecibo, item.cuentaBeneficiario, null, null, tipoCompromiso, (short)idUsuario, null, null);
                row = sheet.GetRow(line);
                row.CreateCell(10, CellType.String).SetCellValue(nextFolioContrarecibo);
                line++;
                if (item.noCheque > 0)
                    importacion.actualizaDetalleBancoCancelaciones(item, idUsuario, item.importe, (short)item.noCuentaBancaria);
            }
            #endregion
            return excelBook;
        } 
        //opcion 4-5
        ///<param name="fileData">Arreglo de Byte con el contenido del archivo. Ya no es necesario FTP.</param>
        ///<param name="tipo">Tipo del archivo -> 0.- Arrendamientos, 1.- Honorarios Puros, 2.- Cancelación de activos,3.- Honorarios Asimilables.</param>
        private IWorkbook ProcesarExcelArrendamientosHonorariosyActivos(byte[] fileData, byte tipo)
        {
            ClearErrorList();
            IWorkbook excelBook = WorkbookFactory.Create(new MemoryStream(fileData));
            ExcelArrendamientosyHonorarios datosExcel = importacion.GetExcelArrendamientosyHonorarios(excelBook, tipo);
            UsuarioLogueado usuarioLog = Session["appUsuario"] as UsuarioLogueado;
            String nombreUsuario = usuarioLog.NombreCompleto;
            int idUsuario = usuarioLog.IdUsuario;
            //if (datosExcel.errores.Count() > 0)
            //{
            //    SetListaErrores(datosExcel.errores);
            //    ThrowError();
            //}
            #region inserciones
            ISheet sheet = excelBook.GetSheetAt(0);
            IRow row = sheet.GetRow(0);
            ICell celda1 = row.CreateCell(10, CellType.String);
            ICell celda2 = row.CreateCell(11, CellType.String);
            celda1.SetCellValue("Folio Compromiso");
            celda2.SetCellValue("Folio Contrarecibo");
            int line = 1;
            short tipoCr = tipo == 0 ? Diccionarios.TiposCR.Arrendamientos : tipo == 1 ? Diccionarios.TiposCR.Honorarios : Diccionarios.TiposCR.CancelacionActivos;
            short tipoCompromiso = tipo == 0 ? (short)12 : tipo == 1 ? (short)29 : tipo == 2 ? (short)54 : (short)59;
            foreach (var item in datosExcel.arrendamientos)
            {
                decimal sumatoria = datosExcel.detalles.Where(x => x.consecutivo == item.consecutivo).Sum(x => x.cargos);
                int nextFolioContrarecibo = 0;
                nextFolioContrarecibo = importacion.getNextIdContrarecibo(tipoCr);
                importacion.insertarContrarecibo(item.fechaPago, item.descripcion, null, item.fuenteFinanciamiento, item.spei, (short)item.idCuentaBancaria
                    , nextFolioContrarecibo, item.importeCheque, item.noCheque, nombreUsuario, tipoCr, null, null, null, tipoCompromiso, (short)idUsuario, item.importeActivoP, item.cuentaActivoPasivo);
                Ma_Compromisos maestro = importacion.insertaCompromiso((short)item.tipoCompromiso, sumatoria, (byte)tipoCr, (short)Diccionarios.ValorEstatus.RECIBIDO, item.fechaPago, item.noBeneficiario.Value, item.cuentaBeneficiario, item.descripcion, nombreUsuario, (short)idUsuario, nextFolioContrarecibo);
                row = sheet.GetRow(line);
                row.CreateCell(10, CellType.String).SetCellValue(maestro.Id_FolioCompromiso);
                row.CreateCell(11, CellType.String).SetCellValue(nextFolioContrarecibo);
                line++;
                foreach (var itemDetalle in datosExcel.detalles.Where(x => x.consecutivo == item.consecutivo))
                {
                    importacion.insertaDetalleCompromiso(itemDetalle.clavePresupuestaria, (byte)1, (short)item.tipoCompromiso, (short)idUsuario, maestro.Id_FolioCompromiso, itemDetalle.objetoGasto, itemDetalle.cargos, "");
                    importacion.insertarDetalleContrarecibo(itemDetalle.clavePresupuestaria, itemDetalle.objetoGasto, itemDetalle.cargos == 0 ? itemDetalle.abonos : itemDetalle.cargos, idUsuario, nextFolioContrarecibo, itemDetalle.cuenta, 1, tipoCr);
                }
                importacion.insertarDetalleContrarecibo(null, null, item.importeCheque, idUsuario, nextFolioContrarecibo, item.cuentaBeneficiario, 2, tipoCr);
                importacion.insertaDetalleCompromiso(null, 2, (short)item.tipoCompromiso, (short)idUsuario, maestro.Id_FolioCompromiso, null, sumatoria, item.cuentaBeneficiario);
                foreach (var itemDocto in datosExcel.documentos.Where(x => x.consecutivo == item.consecutivo))
                    importacion.insertarDetallesCrFactura(1, nextFolioContrarecibo, itemDocto.noProveedor, 1, itemDocto.noFactura, itemDocto.fechaFactura, itemDocto.subtotal,
                        itemDocto.iva, itemDocto.retencionIva, itemDocto.retencionIsr, itemDocto.retencionObra, itemDocto.total);
                if (item.noCheque > 0)
                    importacion.actualizaDetalleBanco(item, idUsuario, sumatoria, (short)item.idCuentaBancaria);
                importacion.actualizarContrarecibo((byte)tipoCr, nextFolioContrarecibo, item.importeActivoP, tipo, item.TipoMovimiento);
            }
            #endregion
            return excelBook;
        }

        ///<param name="tipo">3.- Honorarios Asimilables.</param>
        private IWorkbook ProcesarExcelHonorarios(byte[] fileData, byte tipo)
        {
            ClearErrorList();
            IWorkbook excelBook = WorkbookFactory.Create(new MemoryStream(fileData));
            ExcelArrendamientosyHonorarios datosExcel = importacion.GetExcelHonorarios(excelBook, tipo);
            UsuarioLogueado usuarioLog = Session["appUsuario"] as UsuarioLogueado;
            String nombreUsuario = usuarioLog.NombreCompleto;
            int idUsuario = usuarioLog.IdUsuario;
            //if (datosExcel.errores.Count() > 0)
            //{
            //    SetListaErrores(datosExcel.errores);
            //    ThrowError();
            //}
            #region inserciones
            ISheet sheet = excelBook.GetSheetAt(0);
            IRow row = sheet.GetRow(0);
            ICell celda1 = row.CreateCell(10, CellType.String);
            ICell celda2 = row.CreateCell(11, CellType.String);
            celda1.SetCellValue("Folio Compromiso");
            celda2.SetCellValue("Folio Contrarecibo");
            int line = 1;
            short tipoCr = tipo == 0 ? Diccionarios.TiposCR.Arrendamientos : tipo == 1 ? Diccionarios.TiposCR.Honorarios : Diccionarios.TiposCR.CancelacionActivos;
            short tipoCompromiso = tipo == 0 ? (short)12 : tipo == 1 ? (short)29 : tipo == 2 ? (short)54 : (short)59;
            foreach (var item in datosExcel.arrendamientos)
            {
                decimal sumatoria = datosExcel.detalles.Where(x => x.consecutivo == item.consecutivo).Sum(x => x.cargos);
                int nextFolioContrarecibo = 0;
                nextFolioContrarecibo = importacion.getNextIdContrarecibo(tipoCr);
                importacion.insertarContrareciboH(item.fechaPago, item.descripcion, null, item.fuenteFinanciamiento, item.spei, (short)item.idCuentaBancaria, nextFolioContrarecibo, item.totalCargos, item.noCheque, 
                    nombreUsuario, tipoCr, null, null, null, tipoCompromiso, (short)idUsuario, item.importeActivoP, item.cuentaActivoPasivo, item.importeActivoP2, item.cuentaActivoPasivo2, item.importeActivoP3, item.cuentaActivoPasivo3, item.importeActivoP4, item.cuentaActivoPasivo4, item.importeCheque);
                Ma_Compromisos maestro = importacion.insertaCompromiso((short)item.tipoCompromiso, sumatoria, (byte)tipoCr, (short)Diccionarios.ValorEstatus.RECIBIDO, item.fechaPago, item.noBeneficiario.Value, item.cuentaBeneficiario, item.descripcion, nombreUsuario, (short)idUsuario, nextFolioContrarecibo);
                row = sheet.GetRow(line);
                row.CreateCell(10, CellType.String).SetCellValue(maestro.Id_FolioCompromiso);
                row.CreateCell(11, CellType.String).SetCellValue(nextFolioContrarecibo);
                line++;
                foreach (var itemDetalle in datosExcel.detalles.Where(x => x.consecutivo == item.consecutivo))
                {
                    importacion.insertaDetalleCompromiso(itemDetalle.clavePresupuestaria, (byte)1, (short)item.tipoCompromiso, (short)idUsuario, maestro.Id_FolioCompromiso, itemDetalle.objetoGasto, itemDetalle.cargos, "");
                    importacion.insertarDetalleContrarecibo(itemDetalle.clavePresupuestaria, itemDetalle.objetoGasto, itemDetalle.cargos == 0 ? itemDetalle.abonos : itemDetalle.cargos, idUsuario, nextFolioContrarecibo, itemDetalle.cuenta, 1, tipoCr);
                }
                importacion.insertarDetalleContrarecibo(null, null, item.importeCheque, idUsuario, nextFolioContrarecibo, item.cuentaBeneficiario, 2, tipoCr);
                importacion.insertaDetalleCompromiso(null, 2, (short)item.tipoCompromiso, (short)idUsuario, maestro.Id_FolioCompromiso, null, sumatoria, item.cuentaBeneficiario);
                foreach (var itemDocto in datosExcel.documentos.Where(x => x.consecutivo == item.consecutivo))
                    importacion.insertarDetallesCrFactura(1, nextFolioContrarecibo, itemDocto.noProveedor, 1, itemDocto.noFactura, itemDocto.fechaFactura, itemDocto.subtotal,
                        itemDocto.iva, itemDocto.retencionIva, itemDocto.retencionIsr, itemDocto.retencionObra, itemDocto.total);
                if (item.noCheque > 0)
                    importacion.actualizaDetalleBanco(item, idUsuario, sumatoria, (short)item.idCuentaBancaria);
                importacion.actualizarContrarecibo((byte)tipoCr, nextFolioContrarecibo, item.importeActivoP, tipo, item.TipoMovimiento);
            }
            #endregion
            return excelBook;
        }
        ///<param name="fileData">Arreglo de Byte con el contenido del archivo. Ya no es necesario FTP.</param>
        ///<param name="tipo">Tipo del contrarecibo, si es 0 es Cancelación de Pasivos, si es 1 es Anticipos y Préstamos.</param>
        ///
        private IWorkbook ProcesarExcelEgresosNoPresupuestales(byte[] fileData)
        {
            ClearErrorList();
            IWorkbook excelBook = WorkbookFactory.Create(new MemoryStream(fileData));
            ExcelEgresosNoPresupuestarios datosExcel = importacion.GetExcelEgresosNoPresupuestales(excelBook);
            UsuarioLogueado usuarioLog = Session["appUsuario"] as UsuarioLogueado;
            String nombreUsuario = usuarioLog.NombreCompleto;
            int idUsuario = usuarioLog.IdUsuario;
            //if (datosExcel.errores.Count() > 0)
            //{
            //    SetListaErrores(datosExcel.errores);
            //    ThrowError();
            //}
            //#region inserciones
            ISheet sheet = excelBook.GetSheetAt(0);
            IRow row = sheet.GetRow(0);
            ICell celda1 = row.CreateCell(10, CellType.String);
            celda1.SetCellValue("Folio Contrarecibo");
            int line = 1;
            foreach (var item in datosExcel.egresos)
            {
                decimal sumatoria = datosExcel.detalles.Where(x => x.consecutivo == item.consecutivo).Sum(x => x.importe);
                int nextFolioContrarecibo = 0;
                nextFolioContrarecibo = importacion.getNextIdContrarecibo(7);
                importacion.insertarContrarecibo(item.fechaPago, item.descripcion, null, item.fuenteFinanciamiento, item.spei, null
                    , nextFolioContrarecibo, item.importeCheque, item.noCheque, nombreUsuario, 7, null, null, null, 40, (short)idUsuario, Convert.ToInt32(item.noBeneficiario), null);
                row = sheet.GetRow(line);
                row.CreateCell(10, CellType.String).SetCellValue(nextFolioContrarecibo);
                line++;
                foreach (var itemDetalle in datosExcel.detalles.Where(x => x.consecutivo == item.consecutivo))
                    importacion.insertarDetalleContrarecibo(null, null, itemDetalle.importe, idUsuario, nextFolioContrarecibo, itemDetalle.cuenta, 1, 7);
                if (item.noCheque > 0)
                    importacion.actualizaDetalleBanco(item, idUsuario, sumatoria, (short)item.idCuentaBancaria);
                //importacion.insertarDetalleContrarecibo(null, null, item.importeCheque, idUsuario, nextFolioContrarecibo, null, 2, 7);
            }
            //#endregion
            return excelBook;
        }


        private IWorkbook ProcesarExcelGastosYFondos(byte[] fileData, byte tipo)
        {
            ClearErrorList();
            IWorkbook excelBook = WorkbookFactory.Create(new MemoryStream(fileData));
            ExcelFondosRevolventes datosExcel = importacion.GetExcelFondosRevolventes(excelBook, tipo);
            UsuarioLogueado usuarioLog = Session["appUsuario"] as UsuarioLogueado;
            String nombreUsuario = usuarioLog.NombreCompleto;
            int idUsuario = usuarioLog.IdUsuario;
            //if (datosExcel.listaErrores.Count() > 0)
            //{
            //    SetListaErrores(datosExcel.listaErrores);
            //    ThrowError();
            //}
            //Si llega aquí es porque no hubo ningún error y se hacen las inserciones
            #region Inserciones
            ISheet sheet = excelBook.GetSheetAt(0);
            IRow row = sheet.GetRow(0);
            ICell celda1 = row.CreateCell(10, CellType.String);
            celda1.SetCellValue("Folio Contrarecibo");
            int line = 1;
            decimal sumatoria = 0;
            short tipoContrarecibo = tipo == 0 ? (short)4 : (short)3;
            short tipoCompromiso = tipo == 0 ? (short)38 : (short)3;
            foreach (var item in datosExcel.listaFondosRevolventes)//Inserción de compromisos
            {
                int nextFolioContrarecibo = 0;
                Ma_Contrarecibos contrarecibo;
                using (TransactionScope txScope = new TransactionScope())
                {
                    sumatoria = datosExcel.listaDetallesFondos.Where(x => x.consecutivo == item.consecutivo).Sum(x => x.cargos);
                    nextFolioContrarecibo = importacion.getNextIdContrarecibo(tipoContrarecibo);
                    contrarecibo = importacion.insertaContrareciboPago(item, (short)item.idCuentaBancaria, nextFolioContrarecibo, item.importeCheque, item.noCheque, nombreUsuario, tipoContrarecibo,
                        item.cuentaBeneficiario, item.fechaComprobacion == null ? (byte)1 : (byte)2, item.fechaComprobacion, tipoCompromiso);
                    row = sheet.GetRow(line);
                    row.CreateCell(10, CellType.String).SetCellValue(nextFolioContrarecibo);
                    line++;
                    txScope.Complete();
                }
                foreach (var itemDetalle in datosExcel.listaDetallesFondos.Where(x => x.consecutivo == item.consecutivo))
                    importacion.insertarDetalleContrarecibo(itemDetalle.clavePresupuestaria, itemDetalle.objetoGasto, itemDetalle.cargos, idUsuario, nextFolioContrarecibo, item.cuentaBeneficiario, 1, tipoContrarecibo);
                if (datosExcel.listaDetallesFondos.Where(x => x.consecutivo == item.consecutivo).Count() > 0)
                {

                    /*
                     * Si tiene cuenta por pagar es porque se excedio
                     */
                    if (!String.IsNullOrEmpty(item.cuentaPorPagar))
                    {
                        decimal diferencia = datosExcel.listaDetallesFondos.Where(x => x.consecutivo == item.consecutivo).Sum(x => x.cargos) - item.importeCheque;
                        importacion.insertarDetalleContrarecibo(null, null, item.importeCheque, idUsuario, nextFolioContrarecibo, item.cuentaBeneficiario, 2, tipoContrarecibo);
                        importacion.insertarDetalleContrarecibo(null, null, diferencia, idUsuario, nextFolioContrarecibo, item.cuentaPorPagar, 2, tipoContrarecibo, true);
                    }
                    else
                    {
                        importacion.insertarDetalleContrarecibo(null, null, sumatoria, idUsuario, nextFolioContrarecibo, item.cuentaBeneficiario, 2, tipoContrarecibo);
                    }
                    if (item.noCheque > 0)
                        importacion.actualizaDetalleBanco(item, idUsuario, sumatoria, (short)item.idCuentaBancaria);
                    foreach (var itemDocto in datosExcel.listaDocumentosFondos.Where(x => x.consecutivo == item.consecutivo))
                        importacion.insertarDetallesCrFactura((byte)tipoContrarecibo, nextFolioContrarecibo, itemDocto.noProveedor, 1, itemDocto.noFactura, itemDocto.fechaFactura, itemDocto.subtotal, itemDocto.subtotal,
                            itemDocto.retencionIva, itemDocto.retencionIsr, itemDocto.retencionObra, itemDocto.total);
                }
            }
            #endregion
            return excelBook;
        }

        ///</summary>
        ///<param name="fileData">Arreglo de Byte con el contenido del archivo. Ya no es necesario FTP.</param>
        private IWorkbook ProcesarExcelPagoProveedores(byte[] fileData)
        {
            ClearErrorList();
            #region Variables
            ExcelPagoProveedor datosExcel = new ExcelPagoProveedor();
            UsuarioLogueado usuarioLog = Session["appUsuario"] as UsuarioLogueado;
            String nombreUsuario = usuarioLog.NombreCompleto;
            int idUsuario = usuarioLog.IdUsuario;
            CuentasBancariasDAL cuentasBancariasDal = new CuentasBancariasDAL();
            IWorkbook excelBook = WorkbookFactory.Create(new MemoryStream(fileData));
            #endregion
            datosExcel = importacion.GetExcelPagoProveedores(excelBook.NumberOfSheets, excelBook);
            //if (datosExcel.listaErrores.Count() > 0)
            //{
            //    SetListaErrores(datosExcel.listaErrores);
            //    ThrowError();
            //}
            #region inserciones
            ISheet sheet = excelBook.GetSheetAt(0);
            IRow row = sheet.GetRow(0);
            ICell celda1 = row.CreateCell(10, CellType.String);
            celda1.SetCellValue("Folio Compromiso");
            celda1 = row.CreateCell(11, CellType.String);
            celda1.SetCellValue("Folio Contrarecibo");
            int line = 1;
            decimal sumatoria = 0;
            foreach (var item in datosExcel.listaCompromisos)//Inserción de compromisos
            {
                int nextFolioContrarecibo = 0;
                using (TransactionScope txScope = new TransactionScope())
                {
                    sumatoria = datosExcel.listaDetallesCompromiso.Where(x => x.consecutivo == item.consecutivo).Sum(x => x.cargos);
                    nextFolioContrarecibo = importacion.getNextIdContrarecibo(1);
                    Ma_Contrarecibos contrarecibo = importacion.insertaContrareciboPago(item, cuentasBancariasDal.GetByID(x => x.Id_Fuente == item.fuenteFinanciamiento).Id_CtaBancaria,
                        nextFolioContrarecibo, sumatoria, item.noCheque, nombreUsuario, Diccionarios.TiposCR.ContraRecibos, null, null, null, 1);
                    Ma_Compromisos maestro = importacion.insertaCompromiso((short)item.tipoCompromiso, sumatoria, (byte)1, (short)Diccionarios.ValorEstatus.RECIBIDO, item.fechaPago, item.noBeneficiario.Value, item.cuentaBeneficiario, item.descripcion, nombreUsuario, (short)idUsuario, nextFolioContrarecibo);
                    row = sheet.GetRow(line);
                    row.CreateCell(10, CellType.String).SetCellValue(maestro.Id_FolioCompromiso);
                    row.CreateCell(11, CellType.String).SetCellValue(nextFolioContrarecibo);
                    line++;
                    foreach (var itemDetalle in datosExcel.listaDetallesCompromiso.Where(x => x.consecutivo == item.consecutivo))
                        importacion.insertaDetalleCompromiso(itemDetalle.clavePresupuestaria, (byte)1, (short)item.tipoCompromiso, (short)idUsuario, maestro.Id_FolioCompromiso, itemDetalle.objetoGasto, itemDetalle.cargos, "");
                    importacion.insertaDetalleCompromiso(null, 2, (short)item.tipoCompromiso, (short)idUsuario, maestro.Id_FolioCompromiso, null, sumatoria, item.cuentaBeneficiario);
                    if (item.noCheque > 0)
                        importacion.actualizaDetalleBanco(item, idUsuario, sumatoria, contrarecibo.Id_CtaBancaria.Value);
                    txScope.Complete();
                }
                foreach (var itemDetalle in datosExcel.listaDetallesCompromiso.Where(x => x.consecutivo == item.consecutivo))
                    importacion.insertarDetalleContrarecibo(itemDetalle.clavePresupuestaria, itemDetalle.objetoGasto, itemDetalle.cargos, idUsuario, nextFolioContrarecibo, item.cuentaBeneficiario, 1, 1);
                importacion.insertarDetalleContrarecibo(null, null, sumatoria, idUsuario, nextFolioContrarecibo, item.cuentaBeneficiario, 2, 1);
                foreach (var itemDocto in datosExcel.listaDocumentos.Where(x => x.consecutivo == item.consecutivo))
                    importacion.insertarDetallesCrFactura(1, nextFolioContrarecibo, itemDocto.noProveedor, 1, itemDocto.noFactura, itemDocto.fechaFactura, itemDocto.subtotal,
                        itemDocto.iva, itemDocto.retencionIva, itemDocto.retencionIsr, itemDocto.retencionObra, itemDocto.total);
            }
            #endregion
            return excelBook;
        }

        /// <param name="fileData">Arreglo de Byte con el contenido del archivo. Ya no es necesario FTP.</param>
        private IWorkbook ProcesarExcelReciboIngresos(byte[] fileData)
        {
            ClearErrorList();
            IWorkbook excelBook = WorkbookFactory.Create(new MemoryStream(fileData));
            ExcelReciboIngresos datosExcel = importacion.GetExcelReciboIngresos(excelBook);
            UsuarioLogueado usuarioLog = Session["appUsuario"] as UsuarioLogueado;
            String nombreUsuario = usuarioLog.NombreCompleto;
            MaRecibosDAL recibosDal = new MaRecibosDAL();
            int idUsuario = usuarioLog.IdUsuario;
            //if (datosExcel.listaErrores.Count() > 0)
            //{
            //    SetListaErrores(datosExcel.listaErrores);
            //    ThrowError();
            //}

            foreach (var item in datosExcel.listaReciboIngresos)
            {
                importacion.insertaReciboIngresos(item.noRecibo, item.fechaRecaudacion, (byte)item.noCajaReceptora, item.noContribuyente, item.descripcion, item.importe, nombreUsuario, (short)idUsuario, (short)item.idCtaBancaria);
                foreach (var detalle in datosExcel.listaDetallesReciboIngresos.Where(x => x.noRecibo == item.noRecibo))
                {
                    importacion.insertaDetalleRecibo(item.noRecibo, detalle.cur, detalle.clavePresupuestaria, detalle.importe, detalle.cri, (byte)2, (short)idUsuario);
                }
                //importacion.insertaDetalleRecibo(item.noRecibo, null, null, datosExcel.listaDetallesReciboIngresos.Where(x => x.noRecibo == item.noRecibo).Sum(x => x.importe), "11241000010000000000", (byte)1, (short)idUsuario);
                importacion.actualizarFolioIngresos(item.noRecibo, (short)idUsuario);
            }
            return excelBook;


        }
        //Errores excel 
        public ActionResult ErroresExcel()
        {
            UsuarioLogueado Usuario = Session["appUsuario"] as UsuarioLogueado;
            GenerarPDF.NombreCompleto = Usuario.NombreCompleto;
            ListaErrores errores = new ListaErrores();
            errores.lista = GetListaErrores();
            byte[] PDF = GenerarPDF.GenerarPDF("PagoProveedoresErrores", errores, this.ControllerContext);
            return File(PDF, "application/pdf");
        }
        //Funciones Básicas
        private List<String> GetListaErrores()
        {
            var lista = Session["listaErrores"];
            List<String> listaErrores = new List<String>();
            if (lista != null)
            {
                listaErrores = lista as List<String>;
            }
            return listaErrores;
        }
        private void SetListaErrores(List<String> errores)
        {
            Session["listaErrores"] = errores;
        }
        private void AddErrorToList(String error)
        {
            List<String> errores = GetListaErrores();
            errores.Add(error);
            SetListaErrores(errores);
        }
        private void ClearErrorList()
        {
            Session.Remove("listaErrores");
        }
        private String GetHojaSession()
        {
            return Session["hoja"].ToString();
        }
        /// <summary>
        /// Obtiene el arreglo de bytes a partir de un archivo
        /// </summary>
        /// <param name="file">Archivo que se desea convertir</param>
        /// <returns>Arreglo de bytes del archivo</returns>
        private Byte[] GetByteArrFromFrile(HttpPostedFileBase file)
        {
            Byte[] fileData;
            using (Stream inputStream = file.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                fileData = memoryStream.ToArray();
            }
            return fileData;
        }
        private void ThrowError()
        {
            throw new ArgumentException(MENSAJEERROR);
        }
    }
}
