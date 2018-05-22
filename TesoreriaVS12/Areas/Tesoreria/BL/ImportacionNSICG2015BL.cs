using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class ImportacionNSICG2015BL
    {
        #region Variables
        private TipoContrarecibosDAL tipoContrarecibosDal = new TipoContrarecibosDAL();
        private DeContrarecibosDAL deTipoContrareciboDal = new DeContrarecibosDAL();
        private MaCompromisosDAL compromisosDal = new MaCompromisosDAL();
        private MaContrarecibosDAL contrarecibosDal = new MaContrarecibosDAL();
        private CuentasDAL cuentasDal = new CuentasDAL();
        private CuentasBancariasDAL cuentasBDal = new CuentasBancariasDAL();
        private CompromisosBL compromisosBl = new CompromisosBL();
        private DeCompromisosBL deCompromisosBl = new DeCompromisosBL();
        private DeCompromisosDAL detalleCompromisosDal = new DeCompromisosDAL();
        private DeContrarecibosDAL deCrDal = new DeContrarecibosDAL();
        private DeBancoDAL deBancoDal = new DeBancoDAL();
        private List<String> listaErrores;
        private BeneficiariosCuentasDAL beneficiariosDal = new BeneficiariosCuentasDAL();
        private CuentasBancariasDAL cuentasBancariasDal = new CuentasBancariasDAL();
        private DeCRFacturasDAL deCrFacturasDal = new DeCRFacturasDAL();
        private AreasDAL areasDal = new AreasDAL();
        private MaRecibosDAL reciboIngDal = new MaRecibosDAL();
        private CaCajasReceptorasDAL cajasDal = new CaCajasReceptorasDAL();
        private PersonasDAL personasDal = new PersonasDAL();
        private FuenteDAL fuenteFinDal = new FuenteDAL();
        private MaPresupuestoIngDAL presupuestoIngDal = new MaPresupuestoIngDAL();
        private Ca_CURDAL caCurDal = new Ca_CURDAL();
        private DeRecibosDAL deRecibosDal = new DeRecibosDAL();
        #endregion
        public String getCeldaFromSession()
        {
            return HttpContext.Current.Session["celda"].ToString();
        }
        public void setCeldaValue(String value)
        {
            HttpContext.Current.Session["celda"] = value;
        }
        public void setHojaValue(String value)
        {
            HttpContext.Current.Session["hoja"] = value;
        }
        public String getTipoDatoFromSession()
        {
            return HttpContext.Current.Session["tipoDato"].ToString();
        }
        public void setTipoDato(String value)
        {
            HttpContext.Current.Session["tipoDato"] = value;
        }
        ///<summary>
        ///Obtiene el excel con los datos de los pagos de proveedores
        ///</summary>
        /// <param name="totalSheets">Índice de la celda de la cual se desea obtener el valor.</param>
        /// <param name="excelBook">Archivo de Excel.</param>
        /// <returns>ExcelPagoProveedor: listas con las filas del archivo de excel</returns>
        public ExcelPagoProveedor GetExcelPagoProveedores(int totalSheets, IWorkbook excelBook)
        {
            ExcelPagoProveedor datosExcel = new ExcelPagoProveedor();
            List<PagoProveedoresExcel> listaCompromisos = new List<PagoProveedoresExcel>();
            List<DetallesPagoProveedoresExcel> listaDetalles = new List<DetallesPagoProveedoresExcel>();
            List<DocumentosPagoProveedores> listaDocumentos = new List<DocumentosPagoProveedores>();
            listaErrores = new List<String>();
            decimal totalCargos = 0;
            for (int i = 0; i < totalSheets; i++)
            {
                String sheetName = excelBook.GetSheetName(i);
                setHojaValue(sheetName);
                int sheetNameLength = sheetName.Trim().Length;
                ISheet currentSheet = excelBook.GetSheetAt(i);
                if (currentSheet != null)
                {
                    int totalRowsAtSheet = currentSheet.LastRowNum;
                    if (sheetNameLength == 2)//Es la primer hoja
                    {
                        #region Maestro de Compromisos
                        for (int j = 1; j < totalRowsAtSheet + 1; j++)
                        {
                            IRow currentRow = currentSheet.GetRow(j);

                            if (currentRow != null)
                            {
                                int totalCellinRow = currentRow.LastCellNum;
                                if (totalCellinRow >= 9)//Se asegura que existen al menos las 9 columnas con los datos del compromiso
                                {
                                    Celda cell = GetCellValue(0, currentRow, true);
                                    if (Convert.ToInt32(cell.value) > 0)
                                    {
                                        PagoProveedoresExcel compromiso = GetCompromiso(currentRow);
                                        double d = Double.Parse(compromiso.cuentaBeneficiario);
                                        compromiso.cuentaBeneficiario = Math.Round(d, 0).ToString("F");
                                        if (compromiso.cuentaBeneficiario.Contains(".00"))
                                            compromiso.cuentaBeneficiario = compromiso.cuentaBeneficiario.Substring(0, compromiso.cuentaBeneficiario.Length - 3);

                                        if (compromiso.consecutivo > 0)
                                        {
                                            bool tieneCheque = false;
                                            //Si llegamos a esta parte es porque no hay problema con los tipos de dato, entonces se hacen las validaciones de longitud, que sea último nivel y esas. 
                                            if (compromiso.fuenteFinanciamiento.Length == 0)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento de la celda {0} de la hoja {1} no debe estar vacía.", compromiso.celdaFuenteFinanciamiento, sheetName));
                                            if (compromiso.fuenteFinanciamiento.Length != 4)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} debe ser de 4 dígitos.", compromiso.fuenteFinanciamiento, compromiso.celdaFuenteFinanciamiento, sheetName));
                                            if (compromiso.idCuentaBancaria == 0)
                                                listaErrores.Add(String.Format("El ID de la Cuenta Bancaria {0} de la celda {1} de la hoja {2} no puede estar vacío.", compromiso.idCuentaBancaria, compromiso.celdaIdCuentaBancaria, sheetName));
                                            Ca_FuentesFin fuente = new FuenteDAL().GetByID(x => x.Id_Fuente == compromiso.fuenteFinanciamiento);
                                            if (fuente == null)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} no se encuentra registrada en el sistema.", compromiso.fuenteFinanciamiento, compromiso.celdaFuenteFinanciamiento, sheetName));
                                            if (fuente != null && !fuente.UltimoNivel.Value)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} no es de último nivel.", compromiso.fuenteFinanciamiento, compromiso.celdaFuenteFinanciamiento, sheetName));

                                            /*
                                             * Validar que la fuente de financiamiento esté en CA_CuentasBancarias
                                             */
                                            Ca_CuentasBancarias cuenta = cuentasBancariasDal.GetByID(x => x.Id_Fuente == compromiso.fuenteFinanciamiento && x.Id_CtaBancaria == compromiso.idCuentaBancaria);
                                            if (cuenta == null)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} no se encuentra ligada a la Cuenta Bancaria {3}.", compromiso.fuenteFinanciamiento, compromiso.celdaFuenteFinanciamiento, sheetName, compromiso.idCuentaBancaria));
                                            if (compromiso.noCheque > 0)
                                                tieneCheque = true;
                                            if (tieneCheque && compromiso.noCheque.ToString().Length > 7)
                                                listaErrores.Add(String.Format("La longitud del No. de Cheque {0} de la celda {1} de la hoja {2} no puede ser mayor a 7 dígitos.", compromiso.noCheque, compromiso.celdaNoCheque, sheetName));
                                            DE_Bancos chequeBanco;
                                            if (cuenta != null)
                                            {
                                                chequeBanco = deBancoDal.GetByID(x => x.No_Cheque == compromiso.noCheque && x.Id_CtaBancaria == cuenta.Id_CtaBancaria);
                                                if (tieneCheque && chequeBanco == null && cuenta != null)
                                                    listaErrores.Add(String.Format("El No. de Cheque {0} de la Cta. Bancaria {1} de la celda {2} de la hoja {3} no se encuentra registrado en el sistema.", compromiso.noCheque, cuenta.Id_CtaBancaria, compromiso.celdaNoCheque, sheetName));
                                                if (tieneCheque && chequeBanco != null && cuenta != null && chequeBanco.Id_Estatus != 1)
                                                    listaErrores.Add(String.Format("El No. de Cheque {0} de la Cta. Bancaria {1} de la celda {2} de la hoja {3} ya ha sido utilizado.", compromiso.noCheque, cuenta.Id_CtaBancaria, compromiso.celdaNoCheque, sheetName));
                                                if (!tieneCheque && compromiso.spei == 0)
                                                    listaErrores.Add(String.Format("El SPEI {0} de la celda {1} de la hoja {2} debe ser diferente a 0 debido a que este Compromiso no tiene No. de Cheque.", compromiso.spei, compromiso.celdaSpei, sheetName));
                                                if (!tieneCheque && compromiso.spei.ToString().Length > 50)
                                                    listaErrores.Add(String.Format("El SPEI {0} de la celda {1} de la hoja {2} no debe tener más de 50 dígitos.", compromiso.spei, compromiso.celdaSpei, sheetName));
                                                CA_BeneficiariosCuentas beneficiario = beneficiariosDal.GetByID(x => x.Id_Beneficiario == compromiso.noBeneficiario && x.Id_Cuenta == compromiso.cuentaBeneficiario);
                                                if (beneficiario == null)
                                                    listaErrores.Add(String.Format("El Beneficiario No {0} de la celda {1} de la hoja {2} no se encuentra registrado en el sistema.", compromiso.noBeneficiario, compromiso.celdaNoBeneficiario, sheetName));
                                                if (compromiso.cuentaBeneficiario.Length != 20)
                                                    listaErrores.Add(String.Format("La Cuenta del Beneficiario {0} de la celda {1} de la hoja {2} debe ser de 20 dígitos.", compromiso.cuentaBeneficiario, compromiso.celdaCuentaBeneficiario, sheetName));
                                                if (beneficiario != null && beneficiario.Id_Cuenta.Trim() != compromiso.cuentaBeneficiario)
                                                    listaErrores.Add(String.Format("El No. de Cuenta {0} de la celda {1} de la hoja {2} no pertenece al No. de Beneficiario {3}, favor de verificar.", compromiso.cuentaBeneficiario, compromiso.celdaNoBeneficiario, sheetName, compromiso.noBeneficiario));
                                                if (compromiso.descripcion.Length == 0)
                                                    listaErrores.Add(String.Format("La descripción del Compromiso de la celda {0} de la hoja {1} no puede estar vacía.", compromiso.celdaCuentaBeneficiario, sheetName));
                                            }
                                            listaCompromisos.Add(compromiso);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else if (sheetName.StartsWith("CR") && sheetNameLength > 2)//Son las hojas con los detalles del compromiso
                    {
                        #region Detalles de compromiso
                        string[] name = sheetName.Split(' ');
                        int noCompromiso = 0;
                        try
                        {
                            noCompromiso = Convert.ToInt32(name[1]);
                        }
                        catch (Exception)
                        {
                            listaErrores.Add(String.Format("El nombre de la hoja {0} no es válido, favor de verificar.", sheetName));
                        }
                        if (listaCompromisos.Where(x => x.consecutivo == noCompromiso).Count() == 0)
                            listaErrores.Add(String.Format("El consecutivo {0} del detalle de compromiso no se encuentra en la hoja de los compromisos, favor de verificar.", noCompromiso));
                        else
                        {
                            string funcionTemp = listaCompromisos.SingleOrDefault(x => x.consecutivo == noCompromiso).fuenteFinanciamiento;
                            for (int j = 2; j < totalRowsAtSheet + 1; j++)
                            {
                                IRow currentRow = currentSheet.GetRow(j);
                                if (currentRow != null)
                                {
                                    int totalCellinRow = currentRow.LastCellNum;
                                    if (totalCellinRow >= 15)//Se asegura que existen al menos las 15 columnas con los datos de los detalles del compromiso
                                    {
                                        Celda cell = GetCellValue(0, currentRow);
                                        if (!String.IsNullOrEmpty(cell.value))
                                        {
                                            DetallesPagoProveedoresExcel compromiso = GetDetalleCompromiso(currentRow);
                                            if (!String.IsNullOrEmpty(compromiso.centroGestor))
                                            {
                                                compromiso.consecutivo = noCompromiso;
                                                if (funcionTemp != compromiso.tipoFuente)
                                                    listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} debe ser igual que la Fuente de Financiamiento del consecutivo {3}.", compromiso.tipoFuente, compromiso.celdaTipoFuente, sheetName, noCompromiso));
                                                Ca_Areas area = areasDal.GetByID(x => x.Id_Area == compromiso.centroGestor);
                                                if (area == null)
                                                    listaErrores.Add(String.Format("El Centro Gestor {0} de la celda {1} de la hoja {2} no existe.", compromiso.centroGestor, compromiso.celdaCentroGestor, sheetName));
                                                if (area != null && area.UltimoNivel.Value == false)
                                                    listaErrores.Add(String.Format("El Centro Gestor {0} de la celda {1} de la hoja {2} no es de último nivel.", compromiso.centroGestor, compromiso.celdaCentroGestor, sheetName));
                                                if (compromiso.funcion[compromiso.funcion.Length - 1] == '0')
                                                    listaErrores.Add(String.Format("El último dígito de la Función {0} de la celda {1} de la hoja {2} debe ser diferente de 0.", compromiso.funcion, compromiso.celdaFuncion, sheetName));
                                                MaPresupuestoEgDAL presDal = new MaPresupuestoEgDAL();
                                                if (presDal.Get(x => x.Id_Programa == compromiso.programa).ToList().Count == 0)
                                                    listaErrores.Add(String.Format("El Programa {0} de la celda {1} de la hoja {2} no cuenta con un Presupuesto de Egresos.", compromiso.programa, compromiso.celdaPrograma, sheetName));
                                                String clavePresupuestaria = compromiso.centroGestor + compromiso.funcion +
                                                            compromiso.compromiso + compromiso.clasificacion + compromiso.programa +
                                                            compromiso.proyecto + compromiso.tipoMeta + compromiso.actividadMir + compromiso.accion +
                                                            compromiso.dimensionGeografica + compromiso.tipoGasto + compromiso.tipoFuente + compromiso.AnioFin + compromiso.objetoGasto;
                                                MA_PresupuestoEg presupuesto = new MaPresupuestoEgDAL().GetByID(x => x.Id_ClavePresupuesto == clavePresupuestaria);
                                                if (presupuesto == null)
                                                    listaErrores.Add(String.Format("La Clave Presupuestal {0} de la celda {1} de la hoja {2} no existe en el Presupuesto de Egresos", clavePresupuestaria, compromiso.celdaPrograma, sheetName));
                                                totalCargos += compromiso.cargos;
                                                compromiso.consecutivo = noCompromiso;
                                                compromiso.clavePresupuestaria = clavePresupuestaria;
                                            }
                                            listaDetalles.Add(compromiso);
                                        }
                                    }
                                }
                            }
                            listaCompromisos.SingleOrDefault(x => x.consecutivo == noCompromiso).totalCargos = totalCargos;
                            totalCargos = 0;
                        }
                        #endregion
                    }
                    else if (sheetName.StartsWith("DOC"))//Es una hoja de documentos
                    {
                        #region Documentos
                        string[] name = sheetName.Split(' ');
                        int noCompromiso = 0;
                        try
                        {
                            noCompromiso = Convert.ToInt32(name[1]);
                        }
                        catch (Exception)
                        {
                            listaErrores.Add(String.Format("El nombre de la hoja {0} no es válido, favor de verificar.", sheetName));
                        }
                        if (listaCompromisos.Where(x => x.consecutivo == noCompromiso).Count() == 0)
                            listaErrores.Add(String.Format("El consecutivo {0} del detalle de compromiso no se encuentra en la hoja de los compromisos, favor de verificar.", noCompromiso));
                        else
                        {
                            for (int j = 1; j < totalRowsAtSheet + 1; j++)
                            {
                                IRow currentRow = currentSheet.GetRow(j);
                                if (currentRow != null)
                                {
                                    int totalCellinRow = currentRow.LastCellNum;
                                    if (totalCellinRow >= 9)//Se asegura que existen al menos las 9 columnas con los datos del documento
                                    {
                                        Celda cell = GetCellValue(0, currentRow);
                                        if (!String.IsNullOrEmpty(cell.value))
                                        {
                                            DocumentosPagoProveedores documento = GetDocumentoCompromiso(currentRow);
                                            if (documento.noProveedor > 0)
                                            {
                                                documento.consecutivo = noCompromiso;
                                                /*
                                                 * Validar que ID_Proveedor exista en CA_Beneficiarios -> Maestro de compromisos
                                                 */
                                                Ca_Beneficiarios beneficiario = new BeneficiariosDAL().Get(x => x.Id_Beneficiario == documento.noProveedor).FirstOrDefault();
                                                if (beneficiario == null)
                                                    listaErrores.Add(String.Format("El No. de Proveedor {0} de la celda {1} de la hoja {2} no se encuentra en el sistema, favor de verificar.", documento.noProveedor, documento.celdaNoProveedor, sheetName));
                                                if (documento.total == 0)
                                                    listaErrores.Add(String.Format("El importe de la celda {0} de la hoja {1} no puede ser 0, favor de verificar.", documento.celdaTotal, sheetName));
                                                documento.consecutivo = noCompromiso;
                                            }
                                            listaDocumentos.Add(documento);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            datosExcel.listaCompromisos = listaCompromisos;
            datosExcel.listaDetallesCompromiso = listaDetalles;
            datosExcel.listaDocumentos = listaDocumentos;
            datosExcel.listaErrores = listaErrores;
            return datosExcel;
        }
        ///<summary>
        ///Obtiene el excel con los datos de los fondos revolventes o gastos a comprobar
        ///</summary>
        /// <param name="totalSheets">Índice de la celda de la cual se desea obtener el valor.</param>
        /// <param name="excelBook">Archivo de Excel.</param>
        /// <param name="tipo">Si el parámetro es 0 indica que es un archivo de Gastos a Comprobar, si es 1 es de Fondos Revolventes.</param>
        /// <returns>ExcelFondosRevolventes: listas con las filas del archivo de excel</returns>
        public ExcelFondosRevolventes GetExcelFondosRevolventes(IWorkbook excelBook, byte tipo)
        {
            ExcelFondosRevolventes datosExcel = new ExcelFondosRevolventes();
            List<FondosyGastosExcel> listaFondos = new List<FondosyGastosExcel>();
            List<DetallesFondosyGastos> listaDetalles = new List<DetallesFondosyGastos>();
            listaErrores = new List<string>();
            List<DocumentosPagoProveedores> listaDocumentos = new List<DocumentosPagoProveedores>();
            for (int i = 0; i < excelBook.NumberOfSheets; i++)
            {
                decimal totalCargos = 0;
                String nombreHoja = excelBook.GetSheetName(i);
                setHojaValue(nombreHoja);
                int sheetNameLength = nombreHoja.Trim().Length;
                ISheet currentSheet = excelBook.GetSheetAt(i);
                if (currentSheet != null)
                {
                    int totalRowsAtSheet = currentSheet.LastRowNum;
                    if (sheetNameLength == 2)//Es la primer hoja
                    {
                        #region Maestro de Compromisos
                        for (int j = 1; j < totalRowsAtSheet + 1; j++)
                        {
                            IRow currentRow = currentSheet.GetRow(j);
                            if (currentRow != null)
                            {
                                int totalCellinRow = currentRow.LastCellNum;
                                if (totalCellinRow >= 9)//Se asegura que existen al menos las 9 columnas con los datos del compromiso
                                {
                                    FondosyGastosExcel fondo = GetFondoRevolvente(currentRow);
                                    if (fondo.consecutivo > 0)
                                    {
                                        bool tieneCheque = false;
                                        double d = Double.Parse(fondo.cuentaBeneficiario);
                                        fondo.cuentaBeneficiario = Math.Round(d, 0).ToString("F");
                                        if (fondo.cuentaBeneficiario.Contains(".00"))
                                            fondo.cuentaBeneficiario = fondo.cuentaBeneficiario.Substring(0, fondo.cuentaBeneficiario.Length - 3);
                                        //Si llegamos a esta parte es porque no hay problema con los tipos de dato, entonces se hacen las validaciones de longitud, que sea último nivel y esas. 
                                        if (fondo.fuenteFinanciamiento.Length == 0)
                                            listaErrores.Add(String.Format("La Fuente de Financiamiento de la celda {0} de la hoja {1} no debe estar vacía.", fondo.celdaFuenteFinanciamiento, nombreHoja));
                                        if (fondo.fuenteFinanciamiento.Length != 4)
                                            listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} debe ser de 4 dígitos.", fondo.fuenteFinanciamiento, fondo.celdaFuenteFinanciamiento, nombreHoja));
                                        Ca_FuentesFin fuente = new FuenteDAL().GetByID(x => x.Id_Fuente == fondo.fuenteFinanciamiento);
                                        if (fuente == null)
                                            listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} no se encuentra registrada en el sistema.", fondo.fuenteFinanciamiento, fondo.celdaFuenteFinanciamiento, nombreHoja));
                                        if (fuente != null && !fuente.UltimoNivel.Value)
                                            listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} no es de último nivel.", fondo.fuenteFinanciamiento, fondo.celdaFuenteFinanciamiento, nombreHoja));
                                        if (fondo.noCheque > 0)
                                            tieneCheque = true;
                                        if (tieneCheque && fondo.noCheque.ToString().Length > 7)
                                            listaErrores.Add(String.Format("La longitud del No. de Cheque {0} de la celda {1} debe ser de 7 dígitos.", fondo.noCheque, fondo.celdaNoCheque));
                                        if (!tieneCheque && fondo.spei == 0)
                                            listaErrores.Add(String.Format("El SPEI {0} de la celda {1} debe ser diferente a 0 debido a que este Compromiso no tiene No. de Cheque.", fondo.spei, fondo.celdaSpei));
                                        if (!tieneCheque && fondo.spei.ToString().Length > 50)
                                            listaErrores.Add(String.Format("El SPEI {0} de la celda {1} no debe tener más de 50 dígitos.", fondo.spei, fondo.celdaSpei));
                                        if (fondo.fechaPago.Year != 2015)
                                            listaErrores.Add(String.Format("El año {0} de la Fecha de Pago {1} de la celda {2} de la hoja {3} debe ser del año 2015.", fondo.fechaPago.Year, fondo.fechaPago, fondo.celdaFechaPago, nombreHoja));
                                        if (fondo.cuentaBeneficiario.Length != 20)
                                            listaErrores.Add(String.Format("La Cuenta del Beneficiario {0} de la celda {1} de la hoja {2} debe ser de 20 dígitos.", fondo.cuentaBeneficiario, fondo.celdaCuentaBeneficiario, nombreHoja));
                                        CA_Cuentas cuenta = cuentasDal.GetByID(x => x.Id_Cuenta == fondo.cuentaBeneficiario);
                                        if (cuenta == null)
                                            listaErrores.Add(String.Format("La Cuenta del Beneficiario {0} de la celda {1} de la hoja {2} no se encuentra registrada en el sistema.", fondo.cuentaBeneficiario, fondo.celdaCuentaBeneficiario, nombreHoja));
                                        Ca_CuentasBancarias cuentaB = cuentasBDal.GetByID(x => x.Id_CtaBancaria == fondo.idCuentaBancaria && x.Id_Fuente == fondo.fuenteFinanciamiento);
                                        if (cuentaB == null)
                                            listaErrores.Add(String.Format("La Cuenta del Beneficiario {0} de la celda {1} de la hoja {2} no se encuentra registrada en el sistema.", fondo.cuentaBeneficiario, fondo.celdaCuentaBeneficiario, nombreHoja));
                                        else
                                        {
                                            DE_Bancos chequeBanco = deBancoDal.GetByID(x => x.No_Cheque == fondo.noCheque && x.Id_CtaBancaria == fondo.idCuentaBancaria);
                                            if (tieneCheque && chequeBanco == null && cuentaB != null)
                                                listaErrores.Add(String.Format("El No. de Cheque {0} de la Cta. Bancaria {1} de la celda {2} de la hoja {3} no se encuentra registrado en el sistema.", fondo.noCheque, cuentaB.Id_CtaBancaria, fondo.celdaNoCheque, nombreHoja));
                                            if (tieneCheque && chequeBanco != null && cuentaB != null && chequeBanco.Id_Estatus != 1)
                                                listaErrores.Add(String.Format("El No. de Cheque {0} de la Cta. Bancaria {1} de la celda {2} de la hoja {3} ya ha sido utilizado.", fondo.noCheque, cuentaB.Id_Cuenta, fondo.celdaNoCheque, nombreHoja));
                                            if (!tieneCheque && fondo.spei == 0)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} de la hoja {2} debe ser diferente a 0 debido a que este registro no tiene No. de Cheque.", fondo.spei, fondo.celdaSpei, nombreHoja));
                                            if (!tieneCheque && fondo.spei.ToString().Length > 50)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} de la hoja {2} no debe tener más de 50 dígitos.", fondo.spei, fondo.celdaSpei, nombreHoja));
                                        }
                                        if (tipo == 1 && !fondo.cuentaBeneficiario.StartsWith("1122"))
                                            listaErrores.Add(String.Format("La Cuenta del Beneficiario {0} de la celda {1} de la hoja {2} debe empezar con 1122.", fondo.cuentaBeneficiario, fondo.celdaCuentaBeneficiario, nombreHoja));
                                        if (tipo == 0 && !fondo.cuentaBeneficiario.StartsWith("1123"))
                                            listaErrores.Add(String.Format("La Cuenta del Beneficiario {0} de la celda {1} de la hoja {2} debe empezar con 1123.", fondo.cuentaBeneficiario, fondo.celdaCuentaBeneficiario, nombreHoja));
                                        if (fondo.descripcion.Length == 0)
                                            listaErrores.Add(String.Format("La descripción del Compromiso no puede estar vacía.", fondo.celdaCuentaBeneficiario));
                                        //if (fondo.fechaPago.Year != 2015)
                                        //    listaErrores.Add(String.Format("El año {0} de la Fecha de Comprobación {1} de la celda {2} de la hoja {3} debe ser del año 2015.", fondo.fechaComprobacion.Value.Year, fondo.fechaComprobacion, fondo.celdaFechaComprobacion, nombreHoja));
                                        listaFondos.Add(fondo);
                                    }
                                }
                            }
                        }
                        #endregion

                    }
                    else if (nombreHoja.Length == 5 && nombreHoja.StartsWith("DGC") || nombreHoja.StartsWith("DFR"))//Son las hojas con los detalles de los fondos revolventes o gastos a comprobar
                    {
                        #region Detalles de Gastos a Comprobar o Fondos Revolventes
                        string[] name = nombreHoja.Split(' ');
                        int noCompromiso = 0;
                        try
                        {
                            noCompromiso = Convert.ToInt32(name[1]);
                        }
                        catch (Exception)
                        {
                            listaErrores.Add(String.Format("El nombre de la hoja {0} no es válido, favor de verificar.", nombreHoja));
                        }
                        if (listaDocumentos.Where(x => x.consecutivo == noCompromiso).Count() == 0)
                        {
                            string descExcel = tipo == 0 ? "Gastos a Comprobar" : "Fondos Revolventes";
                            listaErrores.Add(String.Format("El consecutivo {0} del detalle de {1} no se encuentra en la hoja de los {2}, favor de verificar.", noCompromiso, nombreHoja, descExcel));
                        }
                        if (listaFondos.SingleOrDefault(x => x.consecutivo == noCompromiso).fechaComprobacion == null)
                            listaErrores.Add(String.Format("El consecutivo {0} no debe tener detalles debido a que no ha sido comprobado, favor de verificar.", noCompromiso, nombreHoja));

                        string funcionTemp = listaFondos.SingleOrDefault(x => x.consecutivo == noCompromiso).fuenteFinanciamiento;
                        for (int j = 2; j < totalRowsAtSheet + 1; j++)
                        {
                            IRow currentRow = currentSheet.GetRow(j);
                            if (currentRow != null)
                            {
                                int totalCellinRow = currentRow.LastCellNum;
                                if (totalCellinRow >= 15)//Se asegura que existen al menos las 15 columnas con los datos de los detalles del compromiso
                                {
                                    Celda cell = GetCellValue(0, currentRow);
                                    if (!String.IsNullOrEmpty(cell.value))
                                    {
                                        DetallesFondosyGastos registro = GetDetalleFondoyGasto(currentRow, tipo);
                                        if (!String.IsNullOrEmpty(registro.centroGestor))
                                        {
                                            if (funcionTemp != registro.tipoFuente)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} debe ser igual que la Fuente de Financiamiento del consecutivo {3}.", registro.tipoFuente, registro.celdaTipoFuente, nombreHoja, noCompromiso));
                                            registro.consecutivo = noCompromiso;
                                            Ca_Areas area = areasDal.GetByID(x => x.Id_Area == registro.centroGestor);
                                            if (area == null)
                                                listaErrores.Add(String.Format("El Centro Gestor {0} de la celda {1} de la hoja {2} no existe.", registro.centroGestor, registro.celdaCentroGestor, nombreHoja));
                                            if (area != null && area.UltimoNivel.Value == false)
                                                listaErrores.Add(String.Format("El Centro Gestor {0} de la celda {1} de la hoja {2} no es de último nivel.", registro.centroGestor, registro.celdaCentroGestor, nombreHoja));
                                            if (registro.funcion[registro.funcion.Length - 1] == '0')
                                                listaErrores.Add(String.Format("El último dígito de la Función {0} de la celda {1} de la hoja {2} debe ser diferente de 0.", registro.funcion, registro.celdaFuncion, nombreHoja));
                                            MaPresupuestoEgDAL presDal = new MaPresupuestoEgDAL();
                                            if (presDal.Get(x => x.Id_Programa == registro.programa).ToList().Count == 0)
                                                listaErrores.Add(String.Format("El Programa {0} de la celda {1} de la hoja {2} no cuenta con un Presupuesto de Egresos.", registro.programa, registro.celdaPrograma, nombreHoja));
                                            String clavePresupuestaria = registro.centroGestor + registro.funcion +
                                                        registro.compromiso + registro.clasificacion + registro.programa +
                                                        registro.proyecto + registro.tipoMeta + registro.actividadMir + registro.accion +
                                                        registro.dimensionGeografica + registro.tipoGasto + registro.tipoFuente + registro.AnioFin + registro.objetoGasto;
                                            MA_PresupuestoEg presupuesto = new MaPresupuestoEgDAL().GetByID(x => x.Id_ClavePresupuesto == clavePresupuestaria);
                                            if (presupuesto == null)
                                                listaErrores.Add(String.Format("La Clave Presupuestal {0} de la celda {1} de la hoja {2} no existe en el Presupuesto de Egresos", clavePresupuestaria, registro.celdaPrograma, nombreHoja));
                                            totalCargos += registro.cargos;
                                            registro.consecutivo = noCompromiso;
                                            registro.clavePresupuestaria = clavePresupuestaria;
                                        }
                                        listaDetalles.Add(registro);
                                    }
                                }
                            }
                        }

                        if (listaFondos.SingleOrDefault(x => x.consecutivo == noCompromiso).fechaComprobacion != null && listaDetalles.Where(x => x.consecutivo == noCompromiso).Count() == 0)
                            listaErrores.Add(String.Format("El consecutivo {0} no tiene detalles, favor de verificar.", noCompromiso));
                        listaFondos.SingleOrDefault(x => x.consecutivo == noCompromiso).totalCargos = totalCargos;
                        decimal importeCheque = listaFondos.SingleOrDefault(x => x.consecutivo == noCompromiso).importeCheque;

                        /*
                         - Si la suma de los detalles es mayor que el importe del cheque debe tener cuenta por pagar.
                         - Que la cuenta empiece con 2119 y de último nivel.
                         - Abonar la diferencia de la sumatoria de los detalles con el importe del cheque de la carátula.
                         */
                        String cuentaPorPagar = listaFondos.SingleOrDefault(x => x.consecutivo == noCompromiso).cuentaPorPagar;
                        if (totalCargos > importeCheque)
                        {
                            if (String.IsNullOrEmpty(cuentaPorPagar))
                                listaErrores.Add(String.Format("El consecutivo {0} debe tener Cuenta por Pagar debido a que el total de los detalles excede el importe del cheque.", noCompromiso));
                            else if (!cuentaPorPagar.StartsWith("2119"))
                                listaErrores.Add(String.Format("La Cuenta por Pagar {0} del consecutivo {1} debe ser una Cuenta por Pagar, favor de verificar.", cuentaPorPagar, noCompromiso));
                            CA_Cuentas cuenta = cuentasDal.GetByID(x => x.Id_Cuenta == cuentaPorPagar);
                            if (cuenta == null)
                                listaErrores.Add(String.Format("La Cuenta por Pagar {0} del consecutivo {1} no se encuentra registrada en el sistema, favor de verificar.", cuentaPorPagar, noCompromiso));
                            if (cuenta != null && !cuenta.Nivel)
                                listaErrores.Add(String.Format("La Cuenta por Pagar {0} del consecutivo {1} debe ser de Último Nivel, favor de verificar.", cuentaPorPagar, noCompromiso));
                        }
                        else if (!String.IsNullOrEmpty(cuentaPorPagar))
                            listaErrores.Add(String.Format("El consecutivo {0} no debe tener Cuenta por Pagar debido a que el total de los detalles no excede el importe del cheque.", noCompromiso));
                        totalCargos = 0;
                        #endregion
                    }
                    else if (nombreHoja.Length == 5 && nombreHoja.StartsWith("DOC"))//Es una hoja de documentos
                    {
                        #region Documentos
                        string[] name = nombreHoja.Split(' ');
                        int noCompromiso = 0;
                        try
                        {
                            noCompromiso = Convert.ToInt32(name[1]);
                        }
                        catch (Exception)
                        {
                            listaErrores.Add(String.Format("El nombre de la hoja {0} no es válido, favor de verificar.", nombreHoja));
                        }
                        if (listaDocumentos.Where(x => x.consecutivo == noCompromiso).Count() == 0)
                        {
                            string descExcel = tipo == 0 ? "Gastos a Comprobar" : "Fondos Revolventes";
                            listaErrores.Add(String.Format("El consecutivo {0} de los documentos de {1} no se encuentra en la hoja de los {2}, favor de verificar.", noCompromiso, nombreHoja, descExcel));
                        }
                        if (listaFondos.SingleOrDefault(x => x.consecutivo == noCompromiso).fechaComprobacion == null)
                            listaErrores.Add(String.Format("El consecutivo {0} no debe tener detalles debido a que no ha sido comprobado, favor de verificar.", noCompromiso, nombreHoja));

                        for (int j = 1; j < totalRowsAtSheet + 1; j++)
                        {
                            IRow currentRow = currentSheet.GetRow(j);
                            if (currentRow != null)
                            {
                                int totalCellinRow = currentRow.LastCellNum;
                                if (totalCellinRow >= 9)//Se asegura que existen al menos las 9 columnas con los datos del documento
                                {
                                    Celda cell = GetCellValue(0, currentRow);
                                    if (!String.IsNullOrEmpty(cell.value))
                                    {
                                        DocumentosPagoProveedores documento = GetDocumentoCompromiso(currentRow);
                                        if (documento.noProveedor > 0)
                                        {
                                            documento.consecutivo = noCompromiso;
                                            /*
                                             * Validar que ID_Proveedor exista en CA_Beneficiarios -> Maestro de compromisos
                                             */
                                            Ca_Beneficiarios beneficiario = new BeneficiariosDAL().Get(x => x.Id_Beneficiario == documento.noProveedor).FirstOrDefault();
                                            if (beneficiario == null)
                                                listaErrores.Add(String.Format("El No. de Proveedor {0} de la celda {1} de la hoja {2} no se encuentra en el sistema, favor de verificar.", documento.noProveedor, documento.celdaNoProveedor, nombreHoja));
                                            if (documento.total == 0)
                                                listaErrores.Add(String.Format("El importe de la celda {0} de la hoja {1} no puede ser 0, favor de verificar.", documento.celdaTotal, nombreHoja));
                                            documento.consecutivo = noCompromiso;
                                        }
                                        listaDocumentos.Add(documento);
                                    }
                                }
                            }
                        }
                        if (listaFondos.SingleOrDefault(x => x.consecutivo == noCompromiso).fechaComprobacion != null && listaDocumentos.Where(x => x.consecutivo == noCompromiso).Count() == 0)
                            listaErrores.Add(String.Format("El consecutivo {0} no tiene documentos, favor de verificar.", noCompromiso));
                        #endregion
                    }
                }
            }

            foreach (var item in listaFondos)
            {
                int i = item.consecutivo;
                if (item.fechaComprobacion != null && listaDetalles.Where(x => x.consecutivo == i).Count() == 0)
                    listaErrores.Add(String.Format("El consecutivo {0} no tiene detalles, favor de verificar.", i));
                if (item.fechaComprobacion != null && listaDocumentos.Where(x => x.consecutivo == i).Count() == 0)
                    listaErrores.Add(String.Format("El consecutivo {0} no tiene documentos, favor de verificar.", i));
            }
            datosExcel.listaFondosRevolventes = listaFondos;
            datosExcel.listaDetallesFondos = listaDetalles;
            datosExcel.listaDocumentosFondos = listaDocumentos;
            datosExcel.listaErrores = listaErrores;
            return datosExcel;
        }
        ///<summary>
        ///Obtiene el excel con los datos de los fondos revolventes o gastos a comprobar
        ///</summary>
        /// <param name="totalSheets">Índice de la celda de la cual se desea obtener el valor.</param>
        /// <param name="excelBook">Archivo de Excel.</param>
        /// <param name="tipo">Si el parámetro es 0 indica que es un archivo de Gastos a Comprobar, si es 1 es de Fondos Revolventes.</param>
        /// <returns>ExcelFondosRevolventes: listas con las filas del archivo de excel</returns>
        public ExcelReciboIngresos GetExcelReciboIngresos(IWorkbook excelBook)
        {
            ExcelReciboIngresos datosExcel = new ExcelReciboIngresos();
            List<ReciboIngresosExcel> recibos = new List<ReciboIngresosExcel>();
            List<DetalleReciboIngreso> detalles = new List<DetalleReciboIngreso>();
            listaErrores = new List<String>();
            for (int i = 0; i < excelBook.NumberOfSheets; i++)
            {
                decimal totalImporte = 0;
                String sheetName = excelBook.GetSheetName(i);
                setHojaValue(sheetName);
                int sheetNameLength = sheetName.Trim().Length;
                ISheet currentSheet = excelBook.GetSheetAt(i);
                if (currentSheet != null)
                {
                    int totalRowsAtSheet = currentSheet.LastRowNum;
                    if (sheetName.ToLower() == "ingresos")
                    {
                        #region Maestro de Ingresos
                        for (int j = 1; j < totalRowsAtSheet + 1; j++)
                        {
                            IRow currentRow = currentSheet.GetRow(j);

                            if (currentRow != null)
                            {
                                int totalCellinRow = currentRow.LastCellNum;
                                if (totalCellinRow >= 7)//Se asegura que existen al menos las 9 columnas con los datos del compromiso
                                {
                                    Celda cell = GetCellValue(0, currentRow);
                                    if (!String.IsNullOrEmpty(cell.value))
                                    {
                                        ReciboIngresosExcel recibo = GetRecibo(currentRow);
                                        if (recibo.noRecibo > 0)
                                        {
                                            if (reciboIngDal.GetByID(x => x.Folio == recibo.noRecibo) != null)
                                                listaErrores.Add(String.Format("El No. de Recibo {0} de la celda {1} ya se encuentra registrado en el sistema, favor de verificar.", recibo.noRecibo, recibo.celdaNoRecibo));
                                            if (cajasDal.GetByID(x => x.Id_CajaR == recibo.noCajaReceptora) == null)
                                                listaErrores.Add(String.Format("La Caja Receptora {0} de la celda {1} no se encuentra registrado en el sistema, favor de verificar.", recibo.noCajaReceptora, recibo.celdaNoCajaReceptora));
                                            if (personasDal.GetByID(x => x.IdPersona == recibo.noContribuyente) == null)
                                                listaErrores.Add(String.Format("El contribuyente {0} de la celda {1} no se encuentra registrado en el sistema, favor de verificar.", recibo.noContribuyente, recibo.celdaNoContribuyente));
                                            if (fuenteFinDal.GetByID(x => x.Id_Fuente == recibo.fuenteFinanciamiento) == null)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} no se encuentra registrado en el sistema, favor de verificar.", recibo.fuenteFinanciamiento, recibo.celdaFuenteFinanciamiento));
                                            if (fuenteFinDal.GetByID(x => x.Id_Fuente == recibo.fuenteFinanciamiento) == null)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} no se encuentra registrado en el sistema, favor de verificar.", recibo.fuenteFinanciamiento, recibo.celdaFuenteFinanciamiento));
                                            if (cuentasBancariasDal.GetByID(x => x.Id_CtaBancaria == recibo.idCtaBancaria) == null)
                                                listaErrores.Add(String.Format("La Cuenta Bancaria {0} de la celda {1} no se encuentra registrado en el sistema, favor de verificar.", recibo.idCtaBancaria, recibo.celdaIdCtaBancaria));
                                            if (String.IsNullOrEmpty(recibo.descripcion))
                                                listaErrores.Add(String.Format("La descripción de la celda {0} del recibo {1} no debe estar vacía, favor de verificar.", recibo.celdaDescripcion, recibo.noRecibo));
                                            recibos.Add(recibo);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    if (sheetName.Trim().StartsWith("RI")) //Es la hoja con los detalles del recibo de ingresos
                    {
                        #region Detalles de los Recibos de Ingresos
                        string[] name = sheetName.Split(' ');
                        int noRecibo = 0;
                        try
                        {
                            noRecibo = Convert.ToInt32(name[1]);
                        }
                        catch (Exception)
                        {
                            listaErrores.Add(String.Format("El nombre de la hoja {0} no es válido, favor de verificar.", sheetName));
                        }
                        if (recibos.SingleOrDefault(x => x.noRecibo == noRecibo) == null)
                        {
                            listaErrores.Add(String.Format("El No. de Recibo {0} del detalle {1} no se encuentra en la hoja de los Recibos de Ingresos, favor de verificar.", noRecibo, sheetName));
                        }

                        for (int j = 2; j < totalRowsAtSheet + 1; j++)
                        {
                            IRow currentRow = currentSheet.GetRow(j);
                            if (currentRow != null)
                            {
                                int totalCellinRow = currentRow.LastCellNum;
                                if (totalCellinRow >= 7)//Se asegura que existen al menos las 7 columnas con los datos de los detalles del Recibo de Ingresos
                                {
                                    Celda cell = GetCellValue(0, currentRow);
                                    if (!String.IsNullOrEmpty(cell.value))
                                    {
                                        DetalleReciboIngreso detalle = GetDetalleReciboIngreso(currentRow);
                                        if (!String.IsNullOrEmpty(detalle.centroRecaudador))
                                        {
                                            string clavePresupuestaria = String.Format("{0}{1}{2}{3}{4}", detalle.centroRecaudador, detalle.fuenteFinanciamiento, detalle.anioFinanciamiento, detalle.dimension, detalle.cri);
                                            if (presupuestoIngDal.GetByID(x => x.Id_ClavePresupuesto == clavePresupuestaria) == null)
                                                listaErrores.Add(String.Format("La Clave Presupuestaria {0} del número de folio {1} de la hoja {2} no se encuentra registrada en el sistema, favor de verificar.", clavePresupuestaria, noRecibo, sheetName));
                                            if (caCurDal.GetByID(x => x.IdCUR == detalle.cur) == null)
                                                listaErrores.Add(String.Format("EL CUR {0} de la celda {1} de la hoja {2} no se encuentra registrada en el sistema, favor de verificar.", detalle.cur, detalle.celdaCur, sheetName));
                                            totalImporte += detalle.importe;
                                            detalle.clavePresupuestaria = clavePresupuestaria;
                                            detalle.noRecibo = noRecibo;
                                            detalles.Add(detalle);
                                        }
                                    }
                                }
                            }
                        }
                        decimal importeTotal = 0;
                        if (recibos.SingleOrDefault(x => x.noRecibo == noRecibo) != null)
                        {
                            importeTotal = recibos.SingleOrDefault(x => x.noRecibo == noRecibo).importe;
                            if (totalImporte != importeTotal)
                                listaErrores.Add(String.Format("El total {0} de los detalles del número de recibo {1} de la hoja {2} debe ser igual que el importe {3}.", totalImporte, noRecibo, sheetName, importeTotal));
                        }

                        totalImporte = 0;
                        #endregion
                    }
                }
            }
            datosExcel.listaReciboIngresos = recibos;
            datosExcel.listaDetallesReciboIngresos = detalles;
            datosExcel.listaErrores = listaErrores;
            return datosExcel;
        }
        ///<summary>
        ///Obtiene el excel con los datos de las cancelaciones de pasivos
        ///</summary>
        /// <param name="excelBook">Archivo de Excel.</param>
        /// /// <param name="tipo">Si el tipo es 0 es Cancelación de Pasivos, si es 1 es Anticipos y Préstamos.</param>
        /// <returns>ExcelCancelacionPasivos: listas con las filas del archivo de excel</returns>
        public ExcelCancelacionPasivos GetExcelCancelacionPasivos(IWorkbook excelBook, byte tipo)
        {
            ExcelCancelacionPasivos datosExcel = new ExcelCancelacionPasivos();
            List<CancelacionPasivosExcel> cancelaciones = new List<CancelacionPasivosExcel>();
            listaErrores = new List<String>();
            for (int i = 0; i < excelBook.NumberOfSheets; i++)
            {
                String sheetName = excelBook.GetSheetName(i).Trim();
                setHojaValue(sheetName);
                int sheetNameLength = sheetName.Trim().Length;
                ISheet currentSheet = excelBook.GetSheetAt(i);
                if (currentSheet != null)
                {
                    int totalRowsAtSheet = currentSheet.LastRowNum;
                    if (sheetName.ToLower() == "cp")
                    {
                        #region Maestro de Ingresos
                        for (int j = 1; j < totalRowsAtSheet + 1; j++)
                        {
                            IRow currentRow = currentSheet.GetRow(j);
                            if (currentRow != null)
                            {
                                int totalCellinRow = currentRow.LastCellNum;
                                if (totalCellinRow >= 7)//Se asegura que existen al menos las 9 columnas con los datos del compromiso
                                {
                                    Celda cell = GetCellValue(0, currentRow);
                                    if (!String.IsNullOrEmpty(cell.value))
                                    {
                                        CancelacionPasivosExcel cancelacion = GetCancelacionPasivo(currentRow);
                                        if (cancelacion.consecutivo > 0)
                                        {
                                            bool tieneCheque = false;
                                            double d = Double.Parse(cancelacion.cuentaBeneficiario);
                                            cancelacion.cuentaBeneficiario = Math.Round(d, 0).ToString("F");
                                            if (cancelacion.cuentaBeneficiario.Contains(".00"))
                                                cancelacion.cuentaBeneficiario = cancelacion.cuentaBeneficiario.Substring(0, cancelacion.cuentaBeneficiario.Length - 3);
                                            if (cancelacion.fuenteFinanciamiento.Length == 0)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento de la celda {0} de la hoja {1} no debe estar vacía.", cancelacion.celdaFuenteFinanciamiento, sheetName));
                                            if (cancelacion.fuenteFinanciamiento.Length != 4)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} debe ser de 4 dígitos.", cancelacion.fuenteFinanciamiento, cancelacion.celdaFuenteFinanciamiento, sheetName));
                                            Ca_FuentesFin fuente = new FuenteDAL().GetByID(x => x.Id_Fuente == cancelacion.fuenteFinanciamiento);
                                            if (fuente == null)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} no se encuentra registrada en el sistema.", cancelacion.fuenteFinanciamiento, cancelacion.celdaFuenteFinanciamiento, sheetName));
                                            if (fuente != null && !fuente.UltimoNivel.Value)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} no es de último nivel.", cancelacion.fuenteFinanciamiento, cancelacion.celdaFuenteFinanciamiento, sheetName));
                                            if (cancelacion.noCheque > 0)
                                                tieneCheque = true;
                                            if (tieneCheque && cancelacion.noCheque.ToString().Length > 7)
                                                listaErrores.Add(String.Format("La longitud del No. de Cheque {0} de la celda {1} debe ser de 7 dígitos.", cancelacion.noCheque, cancelacion.celdaNoCheque));
                                            if (!tieneCheque && cancelacion.spei == 0)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} debe ser diferente a 0 debido a que este Compromiso no tiene No. de Cheque.", cancelacion.spei, cancelacion.celdaSpei));
                                            if (!tieneCheque && cancelacion.spei.ToString().Length > 50)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} no debe tener más de 50 dígitos.", cancelacion.spei, cancelacion.celdaSpei));
                                            if (cancelacion.fechaPago.Year != 2015)
                                                listaErrores.Add(String.Format("El año {0} de la Fecha de Pago {1} de la celda {2} de la hoja {3} debe ser del año 2015.", cancelacion.fechaPago.Year, cancelacion.fechaPago, cancelacion.celdaFechaPago, sheetName));
                                            if (cancelacion.cuentaBeneficiario.Length != 20)
                                                listaErrores.Add(String.Format("La Cuenta del Beneficiario {0} de la celda {1} de la hoja {2} debe ser de 20 dígitos.", cancelacion.cuentaBeneficiario, cancelacion.celdaCuentaBeneficiario, sheetName));
                                            CA_Cuentas cuenta = cuentasDal.GetByID(x => x.Id_Cuenta == cancelacion.cuentaBeneficiario);
                                            if (cuenta == null)
                                                listaErrores.Add(String.Format("La Cuenta del Beneficiario {0} de la celda {1} de la hoja {2} no se encuentra registrada en el sistema.", cancelacion.cuentaBeneficiario, cancelacion.celdaCuentaBeneficiario, sheetName));
                                            //Ca_CuentasBancarias cuentaB = cuentasBDal.GetByID(x => x.Id_CtaBancaria == cancelacion.noCuentaBancaria);
                                            //if (cuentaB == null)
                                            //    listaErrores.Add(String.Format("La Cuenta del Beneficiario {0} de la celda {1} de la hoja {2} no se encuentra registrada en el sistema.", cancelacion.cuentaBeneficiario, cancelacion.celdaCuentaBeneficiario, sheetName));
                                            //else
                                            //{
                                            DE_Bancos chequeBanco = deBancoDal.GetByID(x => x.No_Cheque == cancelacion.noCheque && x.Id_CtaBancaria == cancelacion.noCuentaBancaria);
                                            if (tieneCheque && chequeBanco == null)
                                                listaErrores.Add(String.Format("El No. de Cheque {0} de la Cta. Bancaria {1} de la celda {2} de la hoja {3} no se encuentra registrado en el sistema.", cancelacion.noCheque, cancelacion.noCuentaBancaria, cancelacion.celdaNoCheque, sheetName));
                                            if (tieneCheque && chequeBanco != null && chequeBanco.Id_Estatus != 1)
                                                listaErrores.Add(String.Format("El No. de Cheque {0} de la Cta. Bancaria {1} de la celda {2} de la hoja {3} ya ha sido utilizado.", cancelacion.noCheque, cancelacion.noCuentaBancaria, cancelacion.celdaNoCheque, sheetName));
                                            if (!tieneCheque && cancelacion.spei == 0)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} de la hoja {2} debe ser diferente a 0 debido a que este Compromiso no tiene No. de Cheque.", cancelacion.spei, cancelacion.celdaSpei, sheetName));
                                            if (!tieneCheque && cancelacion.spei.ToString().Length > 50)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} de la hoja {2} no debe tener más de 50 dígitos.", cancelacion.spei, cancelacion.celdaSpei, sheetName));
                                            //DE_Bancos chequeBanco = deBancoDal.GetByID(x => x.No_Cheque == cancelacion.noCheque && x.Id_CtaBancaria == cancelacion.noCuentaBancaria);
                                            //if (tieneCheque && chequeBanco == null && cuentaB != null)
                                            //    listaErrores.Add(String.Format("El No. de Cheque {0} de la Cta. Bancaria {1} de la celda {2} de la hoja {3} no se encuentra registrado en el sistema.", cancelacion.noCheque, cuentaB.Id_CtaBancaria, cancelacion.celdaNoCheque, sheetName));
                                            //if (tieneCheque && chequeBanco != null && cuentaB != null && chequeBanco.Id_Estatus != 1)
                                            //    listaErrores.Add(String.Format("El No. de Cheque {0} de la Cta. Bancaria {1} de la celda {2} de la hoja {3} ya ha sido utilizado.", cancelacion.noCheque, cuentaB.Id_Cuenta, cancelacion.celdaNoCheque, sheetName));
                                            //if (!tieneCheque && cancelacion.spei == 0)
                                            //    listaErrores.Add(String.Format("El SPEI {0} de la celda {1} de la hoja {2} debe ser diferente a 0 debido a que este Compromiso no tiene No. de Cheque.", cancelacion.spei, cancelacion.celdaSpei, sheetName));
                                            //if (!tieneCheque && cancelacion.spei.ToString().Length > 50)
                                            //    listaErrores.Add(String.Format("El SPEI {0} de la celda {1} de la hoja {2} no debe tener más de 50 dígitos.", cancelacion.spei, cancelacion.celdaSpei, sheetName));
                                            //}
                                            string startsWith = tipo == 0 ? "2" : "1";
                                            if (!cancelacion.cuentaBeneficiario.StartsWith(startsWith))
                                                listaErrores.Add(String.Format("La Cuenta del Beneficiario {0} de la celda {1} de la hoja {2} debe set de tipo {3}.", cancelacion.cuentaBeneficiario, cancelacion.celdaCuentaBeneficiario, sheetName, startsWith));
                                            if (String.IsNullOrEmpty(cancelacion.descripcion))
                                                listaErrores.Add(String.Format("La desscripción de la cancelación de pasivos de la celda {0} de la hoja {1} no debe estar vacía.", cancelacion.celdaDescripcion, sheetName));
                                            cancelaciones.Add(cancelacion);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            datosExcel.errores = listaErrores;
            datosExcel.cancelaciones = cancelaciones;
            return datosExcel;
        }
        public ExcelBeneficiaros GetExcelBeneficiarios(IWorkbook excelBook, byte tipo)
        {
            ExcelBeneficiaros datosExcel = new ExcelBeneficiaros();
            List<BeneficiariosExcel> beneficiarios = new List<BeneficiariosExcel>();
            listaErrores = new List<String>();
            ISheet currentSheet = excelBook.GetSheetAt(0);
            if (currentSheet != null)
            {
                IRow currentRow1 = currentSheet.GetRow(4);//row del numero de cuenta
                datosExcel.NoCuenta = GetCuentaBeneficiario(currentRow1);
                int totalRowsAtSheet = currentSheet.LastRowNum;
                #region Beneficiarios
                for (int j = 5; j < totalRowsAtSheet + 1; j++)
                {
                    IRow currentRow = currentSheet.GetRow(j);
                    if (currentRow != null)
                    {
                        int totalCellinRow = currentRow.LastCellNum;
                        if (totalCellinRow >= 6)//Se asegura que existen al menos las 9 columnas con los datos del compromiso
                        {
                            //Celda cell = GetCellValue(0, currentRow);
                            //if (!String.IsNullOrEmpty(cell.value))
                            //{
                            BeneficiariosExcel beneficiario = GetBeneficiario(currentRow);
                            if (!String.IsNullOrEmpty(beneficiario.nombre))
                            {
                                //bool tieneCheque = false;
                                //double d = Double.Parse(cancelacion.cuentaBeneficiario);
                                //cancelacion.cuentaBeneficiario = Math.Round(d, 0).ToString("F");
                                //if (cancelacion.cuentaBeneficiario.Contains(".00"))
                                //    cancelacion.cuentaBeneficiario = cancelacion.cuentaBeneficiario.Substring(0, cancelacion.cuentaBeneficiario.Length - 3);
                                //if (cancelacion.fuenteFinanciamiento.Length == 0)
                                //    listaErrores.Add(String.Format("La Fuente de Financiamiento de la celda {0} de la hoja {1} no debe estar vacía.", cancelacion.celdaFuenteFinanciamiento, sheetName));
                                //if (cancelacion.fuenteFinanciamiento.Length != 4)
                                //    listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} debe ser de 4 dígitos.", cancelacion.fuenteFinanciamiento, cancelacion.celdaFuenteFinanciamiento, sheetName));
                                //Ca_FuentesFin fuente = new FuenteDAL().GetByID(x => x.Id_Fuente == cancelacion.fuenteFinanciamiento);
                                beneficiarios.Add(beneficiario);
                            }
                            //}
                        }
                    }
                }
                #endregion
            }
            datosExcel.errores = listaErrores;
            datosExcel.beneficiarios = beneficiarios;
            return datosExcel;
        }
        ///<summary>
        ///Obtiene el excel con los datos de los Egresos No Presupuestales
        ///</summary>
        ///<param name="excelBook">Archivo de Excel.</param>
        ///<returns>ExcelEgresosNoPresupuestarios: listas con las filas del archivo de excel.</returns>
        public ExcelEgresosNoPresupuestarios GetExcelEgresosNoPresupuestales(IWorkbook excelBook)
        {
            ExcelEgresosNoPresupuestarios datosExcel = new ExcelEgresosNoPresupuestarios();
            List<EgresosNoPresupuestalesExcel> egresos = new List<EgresosNoPresupuestalesExcel>();
            List<DetallesEgresosNoPresupuestales> detalles = new List<DetallesEgresosNoPresupuestales>();
            listaErrores = new List<String>();
            decimal totalCargos = 0;
            for (int i = 0; i < excelBook.NumberOfSheets; i++)
            {
                String sheetName = excelBook.GetSheetName(i).Trim();
                setHojaValue(sheetName);
                int sheetNameLength = sheetName.Trim().Length;
                ISheet currentSheet = excelBook.GetSheetAt(i);
                if (currentSheet != null)
                {
                    int totalRowsAtSheet = currentSheet.LastRowNum;
                    if (sheetName.ToLower() == "enp")
                    {
                        #region Maestro de Egresos
                        for (int j = 1; j < totalRowsAtSheet + 1; j++)
                        {
                            IRow currentRow = currentSheet.GetRow(j);
                            if (currentRow != null)
                            {
                                int totalCellinRow = currentRow.LastCellNum;
                                if (totalCellinRow >= 7)//Se asegura que existen al menos las 9 columnas con los datos del registro.
                                {
                                    Celda cell = GetCellValue(0, currentRow);
                                    if (!String.IsNullOrEmpty(cell.value))
                                    {
                                        EgresosNoPresupuestalesExcel egreso = GetEgresoNoPresupuestal(currentRow);
                                        if (egreso.consecutivo > 0)
                                        {
                                            bool tieneCheque = false;
                                            if (egreso.fuenteFinanciamiento.Length == 0)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento de la celda {0} de la hoja {1} no debe estar vacía.", egreso.celdaFuenteFinanciamiento, sheetName));
                                            if (egreso.fuenteFinanciamiento.Length != 4)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} debe ser de 4 dígitos.", egreso.fuenteFinanciamiento, egreso.celdaFuenteFinanciamiento, sheetName));
                                            Ca_FuentesFin fuente = new FuenteDAL().GetByID(x => x.Id_Fuente == egreso.fuenteFinanciamiento);
                                            if (fuente == null)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} no se encuentra registrada en el sistema.", egreso.fuenteFinanciamiento, egreso.celdaFuenteFinanciamiento, sheetName));
                                            if (fuente != null && !fuente.UltimoNivel.Value)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} no es de último nivel.", egreso.fuenteFinanciamiento, egreso.celdaFuenteFinanciamiento, sheetName));
                                            if (egreso.noCheque > 0)
                                                tieneCheque = true;
                                            if (tieneCheque && egreso.noCheque.ToString().Length > 7)
                                                listaErrores.Add(String.Format("La longitud del No. de Cheque {0} de la celda {1} debe ser de 7 dígitos.", egreso.noCheque, egreso.celdaNoCheque));
                                            if (!tieneCheque && egreso.spei == 0)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} debe ser diferente a 0 debido a que este Compromiso no tiene No. de Cheque.", egreso.spei, egreso.celdaSpei));
                                            if (!tieneCheque && egreso.spei.ToString().Length > 50)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} no debe tener más de 50 dígitos.", egreso.spei, egreso.celdaSpei));
                                            if (egreso.fechaPago.Year != 2015)
                                                listaErrores.Add(String.Format("El año {0} de la Fecha de Pago {1} de la celda {2} de la hoja {3} debe ser del año 2015.", egreso.fechaPago.Year, egreso.fechaPago, egreso.celdaFechaPago, sheetName));
                                            Ca_CuentasBancarias cuentaB = cuentasBDal.GetByID(x => x.Id_CtaBancaria == egreso.idCuentaBancaria);
                                            if (cuentaB == null)
                                                listaErrores.Add(String.Format("La Cuenta del Beneficiario {0} de la celda {1} de la hoja {2} no se encuentra registrada en el sistema.", egreso.idCuentaBancaria, egreso.celdaCuentaBeneficiario, sheetName));
                                            else
                                            {
                                                DE_Bancos chequeBanco = deBancoDal.GetByID(x => x.No_Cheque == egreso.noCheque && x.Id_CtaBancaria == egreso.idCuentaBancaria);
                                                if (tieneCheque && chequeBanco == null && cuentaB != null)
                                                    listaErrores.Add(String.Format("El No. de Cheque {0} de la Cta. Bancaria {1} de la celda {2} de la hoja {3} no se encuentra registrado en el sistema.", egreso.noCheque, cuentaB.Id_CtaBancaria, egreso.celdaNoCheque, sheetName));
                                                if (tieneCheque && chequeBanco != null && cuentaB != null && chequeBanco.Id_Estatus != 1)
                                                    listaErrores.Add(String.Format("El No. de Cheque {0} de la Cta. Bancaria {1} de la celda {2} de la hoja {3} ya ha sido utilizado.", egreso.noCheque, cuentaB.Id_Cuenta, egreso.celdaNoCheque, sheetName));
                                                if (!tieneCheque && egreso.spei == 0)
                                                    listaErrores.Add(String.Format("El SPEI {0} de la celda {1} de la hoja {2} debe ser diferente a 0 debido a que este Compromiso no tiene No. de Cheque.", egreso.spei, egreso.celdaSpei, sheetName));
                                                if (!tieneCheque && egreso.spei.ToString().Length > 50)
                                                    listaErrores.Add(String.Format("El SPEI {0} de la celda {1} de la hoja {2} no debe tener más de 50 dígitos.", egreso.spei, egreso.celdaSpei, sheetName));
                                            }
                                            CA_Personas persona = new PersonasDAL().GetByID(x => x.IdPersona == egreso.noBeneficiario.Value);
                                            if (persona == null)
                                                listaErrores.Add(String.Format("El No. de Beneficiario {0} de la celda {1} de la hoja {2} no se encuentra registrado en el sistema.", egreso.noBeneficiario, egreso.celdaDescripcion, sheetName));
                                            if (String.IsNullOrEmpty(egreso.descripcion))
                                                listaErrores.Add(String.Format("La desscripción de la cancelación de pasivos de la celda {0} de la hoja {1} no debe estar vacía.", egreso.celdaDescripcion, sheetName));
                                            egresos.Add(egreso);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else if (sheetName.Trim().Length == 5)//Son detalles de los egresos
                    {
                        #region Detalles de Egresos
                        string[] name = sheetName.Split(' ');
                        int consecutivo = 0;
                        try
                        {
                            consecutivo = Convert.ToInt32(name[1]);
                        }
                        catch (Exception)
                        {
                            listaErrores.Add(String.Format("El nombre de la hoja {0} no es válido, favor de verificar.", sheetName));
                        }
                        if (egresos.Where(x => x.consecutivo == consecutivo) == null)
                            listaErrores.Add(String.Format("El consecutivo {0} del detalle de compromiso no se encuentra en la hoja de los compromisos, favor de verificar.", consecutivo));
                        for (int j = 1; j < totalRowsAtSheet + 1; j++)
                        {
                            IRow currentRow = currentSheet.GetRow(j);
                            if (currentRow != null)
                            {
                                int totalCellinRow = currentRow.LastCellNum;
                                if (totalCellinRow >= 2)//Se asegura que existen al menos las 15 columnas con los datos de los detalles del compromiso
                                {
                                    Celda cell = GetCellValue(0, currentRow);
                                    if (!String.IsNullOrEmpty(cell.value))
                                    {
                                        DetallesEgresosNoPresupuestales detalle = GetDetalleEgresoNoPresupuestal(currentRow);
                                        if (!String.IsNullOrEmpty(detalle.cuenta))
                                        {
                                            detalle.consecutivo = consecutivo;
                                            double d = Double.Parse(detalle.cuenta);
                                            detalle.cuenta = Math.Round(d, 0).ToString("F");
                                            if (detalle.cuenta.Contains(".00"))
                                                detalle.cuenta = detalle.cuenta.Substring(0, detalle.cuenta.Length - 3);
                                            totalCargos += detalle.importe;
                                        }
                                        detalles.Add(detalle);
                                    }
                                }
                            }
                        }
                        if (detalles.Where(x => x.consecutivo == consecutivo).Count() == 0)
                            listaErrores.Add(String.Format("El consecutivo {0} no tiene detalles, favor de verificar.", consecutivo));
                        else
                        {
                            EgresosNoPresupuestalesExcel egreso = egresos.SingleOrDefault(x => x.consecutivo == consecutivo);
                            if (egreso.importeCheque != totalCargos)
                            {
                                string descripcion = egreso.noCheque == 0 ? "SPEI" : "cheque";
                                listaErrores.Add(String.Format("El total {0} de los detalles del consecutivo {1} no puede ser diferente al importe del {2} {3}, favor de verificar.", totalCargos, consecutivo, descripcion, egreso.importeCheque));
                                egresos.SingleOrDefault(x => x.consecutivo == consecutivo).totalCargos = totalCargos;
                            }
                        }
                        totalCargos = 0;
                        #endregion
                    }
                }
            }
            datosExcel.egresos = egresos;
            datosExcel.detalles = detalles;
            datosExcel.errores = listaErrores;
            return datosExcel;
        }
        ///<summary>
        ///Obtiene el excel con los datos de los Arrendamientos y Honorarios
        ///</summary>
        ///<param name="excelBook">Archivo de Excel.</param>
        ///<param name="tipo">Tipo del archivo -> 0.- Arrendamientos, 1.- Honorarios, 2.- Cancelación de activos, 3.- Honorarios Asimilables.</param>
        ///<returns>ExcelArrendamientosyHonorarios: listas con las filas del archivo de excel</returns>
        public ExcelArrendamientosyHonorarios GetExcelArrendamientosyHonorarios(IWorkbook excelBook, byte tipo)
        {
            ExcelArrendamientosyHonorarios datosExcel = new ExcelArrendamientosyHonorarios();
            List<ArrendamientosyHonorariosExcel> arrendamientos = new List<ArrendamientosyHonorariosExcel>();
            List<DetalleArrendamientos> detalles = new List<DetalleArrendamientos>();
            List<DocumentosPagoProveedores> documentos = new List<DocumentosPagoProveedores>();
            listaErrores = new List<String>();
            decimal totalCargos = 0;
            decimal totalAbonos = 0;
            for (int i = 0; i < excelBook.NumberOfSheets; i++)
            {
                String sheetName = excelBook.GetSheetName(i).Trim();
                setHojaValue(sheetName);
                int sheetNameLength = sheetName.Trim().Length;
                ISheet currentSheet = excelBook.GetSheetAt(i);
                if (currentSheet != null)
                {
                    int totalRowsAtSheet = currentSheet.LastRowNum;
                    if (sheetName.ToLower() == "carátula")
                    {
                        #region Maestro de Egresos
                        for (int j = 1; j < totalRowsAtSheet + 1; j++)
                        {
                            IRow currentRow = currentSheet.GetRow(j);
                            if (currentRow != null)
                            {
                                int totalCellinRow = currentRow.LastCellNum;
                                if (totalCellinRow >= 11)//Se asegura que existen al menos las 11 columnas con los datos del registro.
                                {
                                    Celda cell = GetCellValue(0, currentRow);
                                    if (!String.IsNullOrEmpty(cell.value))
                                    {
                                        ArrendamientosyHonorariosExcel arrendamiento = GetArrendamientoyHonorario(currentRow, tipo);
                                        if (arrendamiento.consecutivo > 0)
                                        {
                                            bool tieneCheque = false;
                                            double d = Double.Parse(arrendamiento.cuentaBeneficiario);
                                            arrendamiento.cuentaBeneficiario = Math.Round(d, 0).ToString("F");
                                            if (arrendamiento.cuentaBeneficiario.Contains(".00"))
                                                arrendamiento.cuentaBeneficiario = arrendamiento.cuentaBeneficiario.Substring(0, arrendamiento.cuentaBeneficiario.Length - 3);
                                            if (arrendamiento.fuenteFinanciamiento.Length == 0)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento de la celda {0} de la hoja {1} no debe estar vacía.", arrendamiento.celdaFuenteFinanciamiento, sheetName));
                                            if (arrendamiento.fuenteFinanciamiento.Length != 4)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} debe ser de 4 dígitos.", arrendamiento.fuenteFinanciamiento, arrendamiento.celdaFuenteFinanciamiento, sheetName));
                                            Ca_FuentesFin fuente = new FuenteDAL().GetByID(x => x.Id_Fuente == arrendamiento.fuenteFinanciamiento);
                                            if (fuente == null)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} no se encuentra registrada en el sistema.", arrendamiento.fuenteFinanciamiento, arrendamiento.celdaFuenteFinanciamiento, sheetName));
                                            if (fuente != null && !fuente.UltimoNivel.Value)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} no es de último nivel.", arrendamiento.fuenteFinanciamiento, arrendamiento.celdaFuenteFinanciamiento, sheetName));
                                            if (arrendamiento.noCheque > 0)
                                                tieneCheque = true;
                                            if (tieneCheque && arrendamiento.noCheque.ToString().Length > 7)
                                                listaErrores.Add(String.Format("La longitud del No. de Cheque {0} de la celda {1} debe ser de 7 dígitos.", arrendamiento.noCheque, arrendamiento.celdaNoCheque));
                                            if (!tieneCheque && arrendamiento.spei == 0)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} debe ser diferente a 0 debido a que este Compromiso no tiene No. de Cheque.", arrendamiento.spei, arrendamiento.celdaSpei));
                                            if (!tieneCheque && arrendamiento.spei.ToString().Length > 50)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} no debe tener más de 50 dígitos.", arrendamiento.spei, arrendamiento.celdaSpei));
                                            if (arrendamiento.fechaPago.Year != 2015)
                                                listaErrores.Add(String.Format("El año {0} de la Fecha de Pago {1} de la celda {2} de la hoja {3} debe ser del año 2015.", arrendamiento.fechaPago.Year, arrendamiento.fechaPago, arrendamiento.celdaFechaPago, sheetName));
                                            CA_Cuentas cuenta = null;
                                            Ca_CuentasBancarias cuentaB = null;
                                            cuenta = cuentasDal.GetByID(x => x.Id_Cuenta == arrendamiento.cuentaBeneficiario);
                                            if (cuenta == null)
                                                listaErrores.Add(String.Format("La cuenta {0} de la cuenta {1} de la hoja {2} no se encuentra registrada en el sistema.", arrendamiento.cuentaBeneficiario, arrendamiento.celdaCuentaBeneficiario, sheetName));
                                            else if (!cuenta.Nivel)
                                                listaErrores.Add(String.Format("La cuenta {0} de la cuenta {1} de la hoja {2} no es de último nivel.", arrendamiento.cuentaBeneficiario, arrendamiento.celdaCuentaBeneficiario, sheetName));
                                            cuentaB = cuentasBancariasDal.GetByID(x => x.Id_Fuente == arrendamiento.fuenteFinanciamiento && x.Id_CtaBancaria == arrendamiento.idCuentaBancaria);
                                            if (cuentaB == null)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} no se encuentra ligada a la Cuenta Bancaria {3}.", arrendamiento.fuenteFinanciamiento, arrendamiento.celdaFuenteFinanciamiento, sheetName, arrendamiento.idCuentaBancaria));
                                            DE_Bancos chequeBanco = null;
                                            if (cuentaB != null)
                                            {
                                                chequeBanco = deBancoDal.GetByID(x => x.No_Cheque == arrendamiento.noCheque && x.Id_CtaBancaria == cuentaB.Id_CtaBancaria);
                                                if (tieneCheque && chequeBanco == null && cuentaB != null)
                                                    listaErrores.Add(String.Format("El No. de Cheque {0} de la Cta. Bancaria {1} de la celda {2} de la hoja {3} no se encuentra registrado en el sistema.", arrendamiento.noCheque, cuentaB.Id_CtaBancaria, arrendamiento.celdaNoCheque, sheetName));
                                                if (tieneCheque && chequeBanco != null && cuenta != null && chequeBanco.Id_Estatus != 1)
                                                    listaErrores.Add(String.Format("El No. de Cheque {0} de la Cta. Bancaria {1} de la celda {2} de la hoja {3} ya ha sido utilizado.", arrendamiento.noCheque, cuentaB.Id_CtaBancaria, arrendamiento.celdaNoCheque, sheetName));
                                            }

                                            if (!tieneCheque && arrendamiento.spei == 0)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} de la hoja {2} debe ser diferente a 0 debido a que este Compromiso no tiene No. de Cheque.", arrendamiento.spei, arrendamiento.celdaSpei, sheetName));
                                            if (!tieneCheque && arrendamiento.spei.ToString().Length > 50)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} de la hoja {2} no debe tener más de 50 dígitos.", arrendamiento.spei, arrendamiento.celdaSpei, sheetName));
                                            CA_BeneficiariosCuentas beneficiario = beneficiariosDal.GetByID(x => x.Id_Beneficiario == arrendamiento.noBeneficiario && x.Id_Cuenta == arrendamiento.cuentaBeneficiario);
                                            if (beneficiario == null)
                                                listaErrores.Add(String.Format("El Beneficiario No {0} de la celda {1} de la hoja {2} no se encuentra registrado en el sistema.", arrendamiento.noBeneficiario, arrendamiento.celdaNoBeneficiario, sheetName));
                                            if (arrendamiento.cuentaBeneficiario.Length != 20)
                                                listaErrores.Add(String.Format("La Cuenta  {0} de la celda {1} de la hoja {2} debe ser de 20 dígitos.", arrendamiento.cuentaBeneficiario, arrendamiento.celdaCuentaBeneficiario, sheetName));
                                            if (tipo == 0 && arrendamiento.cuentaBeneficiario.StartsWith("2"))
                                                listaErrores.Add(String.Format("La Cuenta  {0} de la celda {1} de la hoja {2} debe ser una cuenta de activo.", arrendamiento.cuentaBeneficiario, arrendamiento.celdaCuentaBeneficiario, sheetName));
                                            if (tipo == 0 &&  arrendamiento.TipoMovimiento == 2)
                                                listaErrores.Add(String.Format("El tipo de movimiento {0} de la celda {1} de la hoja {2} debe ser  2.", arrendamiento.TipoMovimiento, arrendamiento.celdaTipoMovimiento, sheetName));

                                            
                                            if (tipo == 1 && (!arrendamiento.cuentaBeneficiario.StartsWith("1") && !arrendamiento.cuentaBeneficiario.StartsWith("2")))
                                                listaErrores.Add(String.Format("La Cuenta  {0} de la celda {1} de la hoja {2} debe ser una cuenta de pasivo o activo.", arrendamiento.cuentaBeneficiario, arrendamiento.celdaCuentaBeneficiario, sheetName));
                                            if (tipo == 1 && (!(arrendamiento.TipoMovimiento == 1) && !(arrendamiento.TipoMovimiento ==2)) )
                                                listaErrores.Add(String.Format("El tipo de movimiento {0} de la celda {1} de la hoja {2} debe ser  1 o 2.", arrendamiento.TipoMovimiento, arrendamiento.celdaTipoMovimiento, sheetName));

                                            if (beneficiario != null && beneficiario.Id_Cuenta.Trim() != arrendamiento.cuentaBeneficiario)
                                                listaErrores.Add(String.Format("El No. de Cuenta {0} de la celda {1} de la hoja {2} no pertenece al No. de Beneficiario {3}, favor de verificar.", arrendamiento.cuentaBeneficiario, arrendamiento.celdaNoBeneficiario, sheetName, arrendamiento.noBeneficiario));
                                            if (arrendamiento.descripcion.Length == 0)
                                                listaErrores.Add(String.Format("La descripción de la celda {0} de la hoja {1} no puede estar vacía.", arrendamiento.celdaCuentaBeneficiario, sheetName));
                                            if (String.IsNullOrEmpty(arrendamiento.descripcion))
                                                listaErrores.Add(String.Format("La descripción de la cancelación de pasivos de la celda {0} de la hoja {1} no debe estar vacía.", arrendamiento.celdaDescripcion, sheetName));
                                            if (tipo == 3)//Si es un honorario asimilable se debe validar el tipo del movimiento
                                            {
                                                switch (arrendamiento.TipoMovimiento)
                                                {
                                                    case 0:
                                                        listaErrores.Add(String.Format("El tipo de movimiento de la celda {0} de la hoja {1} no debe estar vacío.", arrendamiento.celdaTipoMovimiento, sheetName));
                                                        break;
                                                    case 1:
                                                    case 2:
                                                        break;
                                                    default:
                                                        listaErrores.Add(String.Format("El tipo de movimiento {0} de la celda {1} de la hoja {2} debe ser un tipo de movimiento válido (1 = Cargo, 2 = Abono).", arrendamiento.TipoMovimiento, arrendamiento.celdaTipoMovimiento, sheetName));
                                                        break;
                                                }
                                            }
                                            arrendamientos.Add(arrendamiento);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else if (sheetName.Trim().StartsWith("D") && sheetName.Trim().Length == 3)//Son detalles de los arrendamientos
                    {
                        #region Detalles de Egresos
                        string[] name = sheetName.Split(' ');
                        int consecutivo = 0;
                        try
                        {
                            consecutivo = Convert.ToInt32(name[1]);
                        }
                        catch (Exception)
                        {
                            listaErrores.Add(String.Format("El nombre de la hoja {0} no es válido, favor de verificar.", sheetName));
                        }
                        if (arrendamientos.Where(x => x.consecutivo == consecutivo) == null)
                            listaErrores.Add(String.Format("El consecutivo {0} del detalle del arrendamiento no se encuentra en la hoja de los arrendamientos, favor de verificar.", consecutivo));
                        string funcionTemp = arrendamientos.SingleOrDefault(x => x.consecutivo == consecutivo).fuenteFinanciamiento;
                        for (int j = 2; j < totalRowsAtSheet + 1; j++)
                        {
                            IRow currentRow = currentSheet.GetRow(j);
                            if (currentRow != null)
                            {
                                int totalCellinRow = currentRow.LastCellNum;
                                if (totalCellinRow >= 9)//Se asegura que existen al menos las 15 columnas con los datos de los detalles del compromiso
                                {
                                    DetalleArrendamientos detalle = GetDetalleArrendamiento(currentRow);
                                    if (!String.IsNullOrEmpty(detalle.cuenta))
                                    {
                                        detalle.consecutivo = consecutivo;
                                        double d = Double.Parse(detalle.cuenta);
                                        detalle.cuenta = Math.Round(d, 0).ToString("F");
                                        if (detalle.cuenta.Contains(".00"))
                                            detalle.cuenta = detalle.cuenta.Substring(0, detalle.cuenta.Length - 3);
                                        if (detalle.cuenta.StartsWith("5"))//Si inicia con 5 se tiene que validar la clave presupuestaria y no debe tener abonos
                                        {
                                            if (funcionTemp != detalle.tipoFuente)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} debe ser igual que la Fuente de Financiamiento del consecutivo {3}.", detalle.tipoFuente, detalle.celdaTipoFuente, sheetName, consecutivo));
                                            Ca_Areas area = areasDal.GetByID(x => x.Id_Area == detalle.centroGestor);
                                            if (area == null)
                                                listaErrores.Add(String.Format("El Centro Gestor {0} de la celda {1} de la hoja {2} no existe.", detalle.centroGestor, detalle.celdaCentroGestor, sheetName));
                                            if (area != null && area.UltimoNivel.Value == false)
                                                listaErrores.Add(String.Format("El Centro Gestor {0} de la celda {1} de la hoja {2} no es de último nivel.", detalle.centroGestor, detalle.celdaCentroGestor, sheetName));
                                            if (detalle.funcion[detalle.funcion.Length - 1] == '0')
                                                listaErrores.Add(String.Format("El último dígito de la Función {0} de la celda {1} de la hoja {2} debe ser diferente de 0.", detalle.funcion, detalle.celdaFuncion, sheetName));
                                            MaPresupuestoEgDAL presDal = new MaPresupuestoEgDAL();
                                            if (presDal.Get(x => x.Id_Programa == detalle.programa).ToList().Count == 0)
                                                listaErrores.Add(String.Format("El Programa {0} de la celda {1} de la hoja {2} no cuenta con un Presupuesto de Egresos.", detalle.programa, detalle.celdaPrograma, sheetName));
                                            String clavePresupuestaria = detalle.centroGestor + detalle.funcion +
                                                        detalle.compromiso + detalle.clasificacion + detalle.programa +
                                                        detalle.proyecto + detalle.tipoMeta + detalle.actividadMir + detalle.accion +
                                                        detalle.dimensionGeografica + detalle.tipoGasto + detalle.tipoFuente + detalle.AnioFin + detalle.objetoGasto;
                                            MA_PresupuestoEg presupuesto = new MaPresupuestoEgDAL().GetByID(x => x.Id_ClavePresupuesto == clavePresupuestaria);
                                            if (presupuesto == null)
                                                listaErrores.Add(String.Format("La Clave Presupuestal {0} de la celda {1} de la hoja {2} no existe en el Presupuesto de Egresos", clavePresupuestaria, detalle.celdaPrograma, sheetName));
                                            if (detalle.cargos == 0)
                                                listaErrores.Add(String.Format("El detalle del consecutivo {0} de la hoja {1} debe tener un cargo.", detalle.consecutivo, sheetName));
                                            if (detalle.abonos > 0)
                                                listaErrores.Add(String.Format("El detalle del consecutivo {0} de la hoja {1} no debe tener abonos.", detalle.consecutivo, sheetName));
                                        }
                                        else
                                        {
                                            if (detalle.cargos > 0)
                                                listaErrores.Add(String.Format("El detalle del consecutivo {0} de la hoja {1} no debe tener un cargo.", detalle.consecutivo, sheetName));
                                            if (detalle.abonos == 0)
                                                listaErrores.Add(String.Format("El detalle del consecutivo {0} de la hoja {1} debe tener abonos.", detalle.consecutivo, sheetName));
                                        }
                                        totalCargos += detalle.cargos;
                                        totalAbonos += detalle.abonos;
                                        detalles.Add(detalle);
                                    }
                                }
                            }
                        }
                        if (detalles.Where(x => x.consecutivo == consecutivo).Count() == 0)
                            listaErrores.Add(String.Format("El consecutivo {0} no tiene detalles, favor de verificar.", consecutivo));
                        else
                        {
                            decimal cargos = detalles.Where(x => x.consecutivo == consecutivo).Sum(x => x.cargos);
                            decimal abonos = detalles.Where(x => x.consecutivo == consecutivo).Sum(x => x.abonos);
                                    
                            ArrendamientosyHonorariosExcel arrendamiento = arrendamientos.SingleOrDefault(x => x.consecutivo == consecutivo);
                            if (arrendamiento.TipoMovimiento == null)
                                listaErrores.Add(String.Format("El consecutivo {0} no tiene tipo de movimiento, favor de verificar.", consecutivo));

                            if (arrendamiento == null)
                            {
                                listaErrores.Add(String.Format("No se encontró un registro para el consecutivo {1}.", consecutivo));
                            }
                            else
                            {
                                if (tipo == 1 || tipo == 0)
                                {
                                    if (arrendamiento.TipoMovimiento != null)
                                    {
                                        if (arrendamiento.TipoMovimiento == 2)
                                        {
                                            abonos = abonos + arrendamiento.importeActivoP + arrendamiento.importeCheque;
                                        }
                                        if (arrendamiento.TipoMovimiento == 1)
                                        {
                                            abonos = abonos + arrendamiento.importeCheque;
                                            cargos = cargos + arrendamiento.importeActivoP;

                                        }

                                    }
                                }


                                if (cargos != abonos)
                                    listaErrores.Add(String.Format("El total de cargos {0} del detalle del consecutivo {1} no puede ser diferente que el total de abonos {2}.",string.Format("{0:0.00}", cargos), consecutivo, string.Format("{0:0.00}",abonos)));


                                if (detalles.Where(x => x.consecutivo == consecutivo && x.cuenta == arrendamiento.cuentaBeneficiario).Count() == 0)
                                    listaErrores.Add(String.Format("La cuenta {0} del consecutivo {1} no se encuentra en los detalles de la hoja {2}, favor de verificar.", arrendamiento.cuentaBeneficiario, consecutivo, sheetName));
                                else
                                {
                                    string cuenta = arrendamientos.SingleOrDefault(x => x.consecutivo == consecutivo).cuentaBeneficiario;
                                    decimal sumatoriaCuenta = detalles.Where(x => x.consecutivo == consecutivo && x.cuenta == cuenta).Sum(x => x.abonos);
                                    if (tipo == 3)//Honorarios asimilables
                                    {
                                        if (arrendamiento.cuentaActivoPasivo.StartsWith("2"))//Si el abonado es de pasivo la suma de los detalles debe ser mayor que el importe del cheque
                                        {
                                            if (!(sumatoriaCuenta > arrendamiento.importeCheque))
                                                listaErrores.Add(String.Format("La sumatoria de los detalles {0} del consecutivo {1} debe ser mayor que el importe del cheque {2}, favor de verificar.", sumatoriaCuenta, consecutivo,string.Format("{0:0.00}",  arrendamiento.importeCheque)));
                                        }
                                        else//Si es cargo el importe del cheque debe ser mayor que la suma de los detalles
                                        {
                                            if (!(arrendamiento.importeCheque > sumatoriaCuenta))
                                                listaErrores.Add(String.Format("El importe del cheque {0} del consecutivo {1} debe ser mayor que la sumatoria de los detalles {2}, favor de verificar.", string.Format("{0:0.00}", arrendamiento.importeCheque), consecutivo, sumatoriaCuenta));
                                        }
                                    }
                                    else
                                    {
                                        if (!(sumatoriaCuenta > arrendamiento.importeCheque))
                                            listaErrores.Add(String.Format("La sumatoria de los detalles {0} del consecutivo {1} debe ser mayor que el importe del cheque {2}, favor de verificar.", sumatoriaCuenta, consecutivo, string.Format("{0:0.00}", arrendamiento.importeCheque)));
                                    }
                                    //else
                                    //{
                                    //    if (arrendamiento.importeCheque != sumatoriaCuenta)
                                    //    {
                                    //        listaErrores.Add(String.Format("El total {0} de los detalles de la cuenta {1} del consecutivo {2} no puede ser diferente al importe del cheque {3}, favor de verificar.", sumatoriaCuenta, cuenta, consecutivo, arrendamiento.importeCheque));
                                    //        arrendamientos.SingleOrDefault(x => x.consecutivo == consecutivo).totalCargos = totalCargos;
                                    //    }
                                    //}
                                }
                            }
                        }
                        totalCargos = 0;
                        #endregion
                    }
                    else if (sheetName.Trim().StartsWith("DOC"))//Son documentos de los arrendamientos
                    {
                        #region Documentos de arrendamientos
                        string[] name = sheetName.Split(' ');
                        int consecutivo = 0;
                        try
                        {
                            consecutivo = Convert.ToInt32(name[1]);
                        }
                        catch (Exception)
                        {
                            listaErrores.Add(String.Format("El nombre de la hoja {0} no es válido, favor de verificar.", sheetName));
                        }
                        if (arrendamientos.Where(x => x.consecutivo == consecutivo) == null)
                            listaErrores.Add(String.Format("El consecutivo {0} de los documentos del arrendamiento no se encuentra en la hoja de los arrendamientos, favor de verificar.", consecutivo));
                        for (int j = 1; j < totalRowsAtSheet + 1; j++)
                        {
                            IRow currentRow = currentSheet.GetRow(j);
                            if (currentRow != null)
                            {
                                int totalCellinRow = currentRow.LastCellNum;
                                if (totalCellinRow >= 9)//Se asegura que existen al menos las 9 columnas con los datos del documento
                                {
                                    Celda cell = GetCellValue(0, currentRow);
                                    if (!String.IsNullOrEmpty(cell.value))
                                    {
                                        DocumentosPagoProveedores documento = GetDocumentoCompromiso(currentRow);
                                        if (documento.noProveedor > 0)
                                        {
                                            documento.consecutivo = consecutivo;
                                            /*
                                             * Validar que ID_Proveedor exista en CA_Beneficiarios -> Maestro de compromisos
                                             */
                                            Ca_Beneficiarios beneficiario = new BeneficiariosDAL().Get(x => x.Id_Beneficiario == documento.noProveedor).FirstOrDefault();
                                            if (beneficiario == null)
                                                listaErrores.Add(String.Format("El No. de Proveedor {0} de la celda {1} de la hoja {2} no se encuentra en el sistema, favor de verificar.", documento.noProveedor, documento.celdaNoProveedor, sheetName));
                                            if (documento.total == 0)
                                                listaErrores.Add(String.Format("El importe de la celda {0} de la hoja {1} no puede ser 0, favor de verificar.", documento.celdaTotal, sheetName));
                                        }
                                        documentos.Add(documento);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            datosExcel.arrendamientos = arrendamientos;
            datosExcel.detalles = detalles;
            datosExcel.documentos = documentos;
            datosExcel.errores = listaErrores;
            return datosExcel;
        }


        ///<returns>ExcelArrendamientosyHonorarios: listas con las filas del archivo de excel</returns>
        public ExcelArrendamientosyHonorarios GetExcelHonorarios(IWorkbook excelBook, byte tipo)
        {
            ExcelArrendamientosyHonorarios datosExcel = new ExcelArrendamientosyHonorarios();
            List<ArrendamientosyHonorariosExcel> arrendamientos = new List<ArrendamientosyHonorariosExcel>();
            List<DetalleArrendamientos> detalles = new List<DetalleArrendamientos>();
            List<DocumentosPagoProveedores> documentos = new List<DocumentosPagoProveedores>();
            listaErrores = new List<String>();
            decimal totalCargos = 0;
            decimal totalAbonos = 0;
            for (int i = 0; i < excelBook.NumberOfSheets; i++)
            {
                String sheetName = excelBook.GetSheetName(i).Trim();
                setHojaValue(sheetName);
                int sheetNameLength = sheetName.Trim().Length;
                ISheet currentSheet = excelBook.GetSheetAt(i);
                if (currentSheet != null)
                {
                    int totalRowsAtSheet = currentSheet.LastRowNum;
                    if (sheetName.ToLower() == "carátula")
                    {
                        #region Maestro de Egresos
                        for (int j = 1; j < totalRowsAtSheet + 1; j++)
                        {
                            IRow currentRow = currentSheet.GetRow(j);
                            if (currentRow != null)
                            {
                                int totalCellinRow = currentRow.LastCellNum;
                                if (totalCellinRow >= 11)//Se asegura que existen al menos las 11 columnas con los datos del registro.
                                {
                                    Celda cell = GetCellValue(0, currentRow);
                                    if (!String.IsNullOrEmpty(cell.value))
                                    {
                                        ArrendamientosyHonorariosExcel arrendamiento = GetHonorario(currentRow, tipo);
                                        if (arrendamiento.consecutivo > 0)
                                        {
                                            bool tieneCheque = false;
                                            double d = Double.Parse(arrendamiento.cuentaBeneficiario);
                                            arrendamiento.cuentaBeneficiario = Math.Round(d, 0).ToString("F");
                                            if (arrendamiento.cuentaBeneficiario.Contains(".00"))
                                                arrendamiento.cuentaBeneficiario = arrendamiento.cuentaBeneficiario.Substring(0, arrendamiento.cuentaBeneficiario.Length - 3);
                                            if (arrendamiento.fuenteFinanciamiento.Length == 0)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento de la celda {0} de la hoja {1} no debe estar vacía.", arrendamiento.celdaFuenteFinanciamiento, sheetName));
                                            if (arrendamiento.fuenteFinanciamiento.Length != 4)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} debe ser de 4 dígitos.", arrendamiento.fuenteFinanciamiento, arrendamiento.celdaFuenteFinanciamiento, sheetName));
                                            Ca_FuentesFin fuente = new FuenteDAL().GetByID(x => x.Id_Fuente == arrendamiento.fuenteFinanciamiento);
                                            if (fuente == null)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} no se encuentra registrada en el sistema.", arrendamiento.fuenteFinanciamiento, arrendamiento.celdaFuenteFinanciamiento, sheetName));
                                            if (fuente != null && !fuente.UltimoNivel.Value)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} no es de último nivel.", arrendamiento.fuenteFinanciamiento, arrendamiento.celdaFuenteFinanciamiento, sheetName));
                                            //if (arrendamiento.)
                                            if (arrendamiento.noCheque > 0)
                                                tieneCheque = true;
                                            if (tieneCheque && arrendamiento.noCheque.ToString().Length > 7)
                                                listaErrores.Add(String.Format("La longitud del No. de Cheque {0} de la celda {1} debe ser de 7 dígitos.", arrendamiento.noCheque, arrendamiento.celdaNoCheque));
                                            if (!tieneCheque && arrendamiento.spei == 0)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} debe ser diferente a 0 debido a que este Compromiso no tiene No. de Cheque.", arrendamiento.spei, arrendamiento.celdaSpei));
                                            if (!tieneCheque && arrendamiento.spei.ToString().Length > 50)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} no debe tener más de 50 dígitos.", arrendamiento.spei, arrendamiento.celdaSpei));
                                            if (arrendamiento.fechaPago.Year != 2015)
                                                listaErrores.Add(String.Format("El año {0} de la Fecha de Pago {1} de la celda {2} de la hoja {3} debe ser del año 2015.", arrendamiento.fechaPago.Year, arrendamiento.fechaPago, arrendamiento.celdaFechaPago, sheetName));
                                            CA_Cuentas cuenta = null;
                                            Ca_CuentasBancarias cuentaB = null;
                                            cuenta = cuentasDal.GetByID(x => x.Id_Cuenta == arrendamiento.cuentaBeneficiario);
                                            if (cuenta == null)
                                                listaErrores.Add(String.Format("La cuenta {0} de la cuenta {1} de la hoja {2} no se encuentra registrada en el sistema.", arrendamiento.cuentaBeneficiario, arrendamiento.celdaCuentaBeneficiario, sheetName));
                                            else if (!cuenta.Nivel)
                                                listaErrores.Add(String.Format("La cuenta {0} de la cuenta {1} de la hoja {2} no es de último nivel.", arrendamiento.cuentaBeneficiario, arrendamiento.celdaCuentaBeneficiario, sheetName));
                                            cuentaB = cuentasBancariasDal.GetByID(x => x.Id_Fuente == arrendamiento.fuenteFinanciamiento && x.Id_CtaBancaria == arrendamiento.idCuentaBancaria);
                                            if (cuentaB == null)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} no se encuentra ligada a la Cuenta Bancaria {3}.", arrendamiento.fuenteFinanciamiento, arrendamiento.celdaFuenteFinanciamiento, sheetName, arrendamiento.idCuentaBancaria));
                                            DE_Bancos chequeBanco = null;
                                            if (cuentaB != null)
                                            {
                                                chequeBanco = deBancoDal.GetByID(x => x.No_Cheque == arrendamiento.noCheque && x.Id_CtaBancaria == cuentaB.Id_CtaBancaria);
                                                if (tieneCheque && chequeBanco == null && cuentaB != null)
                                                    listaErrores.Add(String.Format("El No. de Cheque {0} de la Cta. Bancaria {1} de la celda {2} de la hoja {3} no se encuentra registrado en el sistema.", arrendamiento.noCheque, cuentaB.Id_CtaBancaria, arrendamiento.celdaNoCheque, sheetName));
                                                if (tieneCheque && chequeBanco != null && cuenta != null && chequeBanco.Id_Estatus != 1)
                                                    listaErrores.Add(String.Format("El No. de Cheque {0} de la Cta. Bancaria {1} de la celda {2} de la hoja {3} ya ha sido utilizado.", arrendamiento.noCheque, cuentaB.Id_CtaBancaria, arrendamiento.celdaNoCheque, sheetName));
                                            }

                                            if (!tieneCheque && arrendamiento.spei == 0)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} de la hoja {2} debe ser diferente a 0 debido a que este Compromiso no tiene No. de Cheque.", arrendamiento.spei, arrendamiento.celdaSpei, sheetName));
                                            if (!tieneCheque && arrendamiento.spei.ToString().Length > 50)
                                                listaErrores.Add(String.Format("El SPEI {0} de la celda {1} de la hoja {2} no debe tener más de 50 dígitos.", arrendamiento.spei, arrendamiento.celdaSpei, sheetName));
                                            CA_BeneficiariosCuentas beneficiario = beneficiariosDal.GetByID(x => x.Id_Beneficiario == arrendamiento.noBeneficiario && x.Id_Cuenta == arrendamiento.cuentaBeneficiario);
                                            if (beneficiario == null)
                                                listaErrores.Add(String.Format("El Beneficiario No {0} de la celda {1} de la hoja {2} no se encuentra registrado en el sistema.", arrendamiento.noBeneficiario, arrendamiento.celdaNoBeneficiario, sheetName));
                                            if (arrendamiento.cuentaBeneficiario.Length != 20)
                                                listaErrores.Add(String.Format("La Cuenta del Beneficiario {0} de la celda {1} de la hoja {2} debe ser de 20 dígitos.", arrendamiento.cuentaBeneficiario, arrendamiento.celdaCuentaBeneficiario, sheetName));
                                            if (tipo == 0 && !arrendamiento.cuentaBeneficiario.StartsWith("2"))
                                                listaErrores.Add(String.Format("La Cuenta del Beneficiario {0} de la celda {1} de la hoja {2} debe ser una cuenta de activo.", arrendamiento.cuentaBeneficiario, arrendamiento.celdaCuentaBeneficiario, sheetName));
                                            if (tipo == 1 && !arrendamiento.cuentaBeneficiario.StartsWith("1"))
                                                listaErrores.Add(String.Format("La Cuenta del Beneficiario {0} de la celda {1} de la hoja {2} debe ser una cuenta de pasivo.", arrendamiento.cuentaBeneficiario, arrendamiento.celdaCuentaBeneficiario, sheetName));
                                            if (beneficiario != null && beneficiario.Id_Cuenta.Trim() != arrendamiento.cuentaBeneficiario)
                                                listaErrores.Add(String.Format("El No. de Cuenta {0} de la celda {1} de la hoja {2} no pertenece al No. de Beneficiario {3}, favor de verificar.", arrendamiento.cuentaBeneficiario, arrendamiento.celdaNoBeneficiario, sheetName, arrendamiento.noBeneficiario));
                                            if (arrendamiento.descripcion.Length == 0)
                                                listaErrores.Add(String.Format("La descripción de la celda {0} de la hoja {1} no puede estar vacía.", arrendamiento.celdaCuentaBeneficiario, sheetName));
                                            if (String.IsNullOrEmpty(arrendamiento.descripcion))
                                                listaErrores.Add(String.Format("La descripción de la cancelación de pasivos de la celda {0} de la hoja {1} no debe estar vacía.", arrendamiento.celdaDescripcion, sheetName));
                                            if (tipo == 3)//Si es un honorario asimilable se debe validar el tipo del movimiento
                                            {
                                                switch (arrendamiento.TipoMovimiento)
                                                {
                                                    case 0:
                                                        listaErrores.Add(String.Format("El tipo de movimiento de la celda {0} de la hoja {1} no debe estar vacío.", arrendamiento.celdaTipoMovimiento, sheetName));
                                                        break;
                                                    case 1:
                                                    case 2:
                                                        break;
                                                    default:
                                                        listaErrores.Add(String.Format("El tipo de movimiento {0} de la celda {1} de la hoja {2} debe ser un tipo de movimiento válido (1 = Cargo, 2 = Abono).", arrendamiento.TipoMovimiento, arrendamiento.celdaTipoMovimiento, sheetName));
                                                        break;
                                                }
                                            }
                                            arrendamientos.Add(arrendamiento);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else if (sheetName.Trim().StartsWith("D") && sheetName.Trim().Length == 3)//Son detalles de los arrendamientos
                    {
                        #region Detalles de Egresos
                        string[] name = sheetName.Split(' ');
                        int consecutivo = 0;
                        try
                        {
                            consecutivo = Convert.ToInt32(name[1]);
                        }
                        catch (Exception)
                        {
                            listaErrores.Add(String.Format("El nombre de la hoja {0} no es válido, favor de verificar.", sheetName));
                        }
                        if (arrendamientos.Where(x => x.consecutivo == consecutivo) == null)
                            listaErrores.Add(String.Format("El consecutivo {0} del detalle del arrendamiento no se encuentra en la hoja de los arrendamientos, favor de verificar.", consecutivo));
                        string funcionTemp = arrendamientos.SingleOrDefault(x => x.consecutivo == consecutivo).fuenteFinanciamiento;
                        for (int j = 2; j < totalRowsAtSheet + 1; j++)
                        {
                            IRow currentRow = currentSheet.GetRow(j);
                            if (currentRow != null)
                            {
                                int totalCellinRow = currentRow.LastCellNum;
                                if (totalCellinRow >= 9)//Se asegura que existen al menos las 15 columnas con los datos de los detalles del compromiso
                                {
                                    DetalleArrendamientos detalle = GetDetalleArrendamiento(currentRow);
                                    if (!String.IsNullOrEmpty(detalle.cuenta))
                                    {
                                        detalle.consecutivo = consecutivo;
                                        double d = Double.Parse(detalle.cuenta);
                                        detalle.cuenta = Math.Round(d, 0).ToString("F");
                                        if (detalle.cuenta.Contains(".00"))
                                            detalle.cuenta = detalle.cuenta.Substring(0, detalle.cuenta.Length - 3);
                                        if (detalle.cuenta.StartsWith("5"))//Si inicia con 5 se tiene que validar la clave presupuestaria y no debe tener abonos
                                        {
                                            if (funcionTemp != detalle.tipoFuente)
                                                listaErrores.Add(String.Format("La Fuente de Financiamiento {0} de la celda {1} de la hoja {2} debe ser igual que la Fuente de Financiamiento del consecutivo {3}.", detalle.tipoFuente, detalle.celdaTipoFuente, sheetName, consecutivo));
                                            Ca_Areas area = areasDal.GetByID(x => x.Id_Area == detalle.centroGestor);
                                            if (area == null)
                                                listaErrores.Add(String.Format("El Centro Gestor {0} de la celda {1} de la hoja {2} no existe.", detalle.centroGestor, detalle.celdaCentroGestor, sheetName));
                                            if (area != null && area.UltimoNivel.Value == false)
                                                listaErrores.Add(String.Format("El Centro Gestor {0} de la celda {1} de la hoja {2} no es de último nivel.", detalle.centroGestor, detalle.celdaCentroGestor, sheetName));
                                            if (detalle.funcion[detalle.funcion.Length - 1] == '0')
                                                listaErrores.Add(String.Format("El último dígito de la Función {0} de la celda {1} de la hoja {2} debe ser diferente de 0.", detalle.funcion, detalle.celdaFuncion, sheetName));
                                            MaPresupuestoEgDAL presDal = new MaPresupuestoEgDAL();
                                            if (presDal.Get(x => x.Id_Programa == detalle.programa).ToList().Count == 0)
                                                listaErrores.Add(String.Format("El Programa {0} de la celda {1} de la hoja {2} no cuenta con un Presupuesto de Egresos.", detalle.programa, detalle.celdaPrograma, sheetName));
                                            String clavePresupuestaria = detalle.centroGestor + detalle.funcion +
                                                        detalle.compromiso + detalle.clasificacion + detalle.programa +
                                                        detalle.proyecto + detalle.tipoMeta + detalle.actividadMir + detalle.accion +
                                                        detalle.dimensionGeografica + detalle.tipoGasto + detalle.tipoFuente + detalle.AnioFin + detalle.objetoGasto;
                                            MA_PresupuestoEg presupuesto = new MaPresupuestoEgDAL().GetByID(x => x.Id_ClavePresupuesto == clavePresupuestaria);
                                            if (presupuesto == null)
                                                listaErrores.Add(String.Format("La Clave Presupuestal {0} de la celda {1} de la hoja {2} no existe en el Presupuesto de Egresos", clavePresupuestaria, detalle.celdaPrograma, sheetName));
                                            if (detalle.cargos == 0)
                                                listaErrores.Add(String.Format("El detalle del consecutivo {0} de la hoja {1} debe tener un cargo.", detalle.consecutivo, sheetName));
                                            if (detalle.abonos > 0)
                                                listaErrores.Add(String.Format("El detalle del consecutivo {0} de la hoja {1} no debe tener abonos.", detalle.consecutivo, sheetName));
                                        }
                                        else
                                        {
                                            if (detalle.cargos > 0)
                                                listaErrores.Add(String.Format("El detalle del consecutivo {0} de la hoja {1} no debe tener un cargo.", detalle.consecutivo, sheetName));
                                            if (detalle.abonos == 0)
                                                listaErrores.Add(String.Format("El detalle del consecutivo {0} de la hoja {1} debe tener abonos.", detalle.consecutivo, sheetName));
                                        }
                                        totalCargos += detalle.cargos;
                                        totalAbonos += detalle.abonos;
                                        detalles.Add(detalle);
                                    }
                                }
                            }
                        }
                        if (detalles.Where(x => x.consecutivo == consecutivo).Count() == 0)
                            listaErrores.Add(String.Format("El consecutivo {0} no tiene detalles, favor de verificar.", consecutivo));
                        else
                        {
                            decimal cargos = detalles.Where(x => x.consecutivo == consecutivo).Sum(x => x.cargos);
                            decimal abonos = detalles.Where(x => x.consecutivo == consecutivo).Sum(x => x.abonos);


                            ArrendamientosyHonorariosExcel arrendamiento = arrendamientos.SingleOrDefault(x => x.consecutivo == consecutivo);
                            if (arrendamiento == null)
                            {
                                listaErrores.Add(String.Format("No se encontró un registro para el consecutivo {1}.", consecutivo));
                            }
                            else
                            {
                                if (arrendamiento.TipoMovimiento != null)
                                {
                                    if (arrendamiento.TipoMovimiento == 2)
                                    {
                                        abonos = abonos + arrendamiento.importeActivoP + arrendamiento.importeActivoP2 + arrendamiento.importeActivoP3 + arrendamiento.importeActivoP4 + arrendamiento.importeCheque;
                                    }
                                    if (arrendamiento.TipoMovimiento == 1)
                                    {
                                        abonos = abonos + arrendamiento.importeCheque;
                                        cargos = cargos + arrendamiento.importeActivoP + arrendamiento.importeActivoP2 + arrendamiento.importeActivoP3 + arrendamiento.importeActivoP4;

                                    }

                                }

                                if (cargos != abonos)
                                    listaErrores.Add(String.Format("El total de cargos {0} del detalle del consecutivo {1} no puede ser diferente que el total de abonos {2}.", cargos, consecutivo, abonos));

                                if (detalles.Where(x => x.consecutivo == consecutivo && x.cuenta == arrendamiento.cuentaBeneficiario).Count() == 0)
                                    listaErrores.Add(String.Format("La cuenta {0} del consecutivo {1} no se encuentra en los detalles de la hoja {2}, favor de verificar.", arrendamiento.cuentaBeneficiario, consecutivo, sheetName));
                                else
                                {
                                    string cuenta = arrendamientos.SingleOrDefault(x => x.consecutivo == consecutivo).cuentaBeneficiario;
                                    decimal sumatoriaCuenta = detalles.Where(x => x.consecutivo == consecutivo && x.cuenta == cuenta).Sum(x => x.abonos);
                                    if (tipo == 3)//Honorarios asimilables
                                    {
                                        if (arrendamiento.cuentaActivoPasivo.StartsWith("2"))//Si el abonado es de pasivo la suma de los detalles debe ser mayor que el importe del cheque
                                        {
                                            if (!(sumatoriaCuenta > arrendamiento.importeCheque))
                                                listaErrores.Add(String.Format("La sumatoria de los detalles {0} del consecutivo {1} debe ser mayor que el importe del cheque {2}, favor de verificar.", sumatoriaCuenta, consecutivo, arrendamiento.importeCheque));
                                        }
                                        else//Si es cargo el importe del cheque debe ser mayor que la suma de los detalles
                                        {
                                            if (!(arrendamiento.importeCheque > sumatoriaCuenta))
                                                listaErrores.Add(String.Format("El importe del cheque {0} del consecutivo {1} debe ser mayor que la sumatoria de los detalles {2}, favor de verificar.", arrendamiento.importeCheque, consecutivo, sumatoriaCuenta));
                                        }

                                        //Decimal SumaTotalP = arrendamientos.SingleOrDefault(x => x.consecutivo == consecutivo).importeActivoP + arrendamientos.SingleOrDefault(x => x.consecutivo == consecutivo).importeActivoP2 + arrendamientos.SingleOrDefault(x => x.consecutivo == consecutivo).importeActivoP3 + arrendamientos.SingleOrDefault(x => x.consecutivo == consecutivo).importeActivoP4;
                                        //if (SumaTotalP != arrendamiento.totalCargos)
                                        //{
                                        //    listaErrores.Add(String.Format("La cuentas del consecutivo {0}  no cuadran, favor de verificar.", arrendamiento.consecutivo));

                                        //}
                                    }
                                    else
                                    {
                                        if (!(sumatoriaCuenta > arrendamiento.importeCheque))
                                            listaErrores.Add(String.Format("La sumatoria de los detalles {0} del consecutivo {1} debe ser mayor que el importe del cheque {2}, favor de verificar.", sumatoriaCuenta, consecutivo, arrendamiento.importeCheque));
                                    }
                                    //else
                                    //{
                                    //    if (arrendamiento.importeCheque != sumatoriaCuenta)
                                    //    {
                                    //        listaErrores.Add(String.Format("El total {0} de los detalles de la cuenta {1} del consecutivo {2} no puede ser diferente al importe del cheque {3}, favor de verificar.", sumatoriaCuenta, cuenta, consecutivo, arrendamiento.importeCheque));
                                    //        arrendamientos.SingleOrDefault(x => x.consecutivo == consecutivo).totalCargos = totalCargos;
                                    //    }
                                    //}

                                }
                            }
                        }
                        totalCargos = 0;
                        #endregion
                    }
                    else if (sheetName.Trim().StartsWith("DOC"))//Son documentos de los arrendamientos
                    {
                        #region Documentos de arrendamientos
                        string[] name = sheetName.Split(' ');
                        int consecutivo = 0;
                        try
                        {
                            consecutivo = Convert.ToInt32(name[1]);
                        }
                        catch (Exception)
                        {
                            listaErrores.Add(String.Format("El nombre de la hoja {0} no es válido, favor de verificar.", sheetName));
                        }
                        if (arrendamientos.Where(x => x.consecutivo == consecutivo) == null)
                            listaErrores.Add(String.Format("El consecutivo {0} de los documentos del arrendamiento no se encuentra en la hoja de los arrendamientos, favor de verificar.", consecutivo));
                        for (int j = 1; j < totalRowsAtSheet + 1; j++)
                        {
                            IRow currentRow = currentSheet.GetRow(j);
                            if (currentRow != null)
                            {
                                int totalCellinRow = currentRow.LastCellNum;
                                if (totalCellinRow >= 9)//Se asegura que existen al menos las 9 columnas con los datos del documento
                                {
                                    Celda cell = GetCellValue(0, currentRow);
                                    if (!String.IsNullOrEmpty(cell.value))
                                    {
                                        DocumentosPagoProveedores documento = GetDocumentoCompromiso(currentRow);
                                        if (documento.noProveedor > 0)
                                        {
                                            documento.consecutivo = consecutivo;
                                            /*
                                             * Validar que ID_Proveedor exista en CA_Beneficiarios -> Maestro de compromisos
                                             */
                                            Ca_Beneficiarios beneficiario = new BeneficiariosDAL().Get(x => x.Id_Beneficiario == documento.noProveedor).FirstOrDefault();
                                            if (beneficiario == null)
                                                listaErrores.Add(String.Format("El No. de Proveedor {0} de la celda {1} de la hoja {2} no se encuentra en el sistema, favor de verificar.", documento.noProveedor, documento.celdaNoProveedor, sheetName));
                                            if (documento.total == 0)
                                                listaErrores.Add(String.Format("El importe de la celda {0} de la hoja {1} no puede ser 0, favor de verificar.", documento.celdaTotal, sheetName));
                                        }
                                        documentos.Add(documento);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            datosExcel.arrendamientos = arrendamientos;
            datosExcel.detalles = detalles;
            datosExcel.documentos = documentos;
            datosExcel.errores = listaErrores;
            return datosExcel;
        }

        ///<summary>
        ///Obtiene el valor de la celda según el índice indicado
        ///</summary>
        /// <param name="cellIndex">Índice de la celda de la cual se desea obtener el valor.</param>
        /// <param name="row">Fila de la que se va a obtener la celda.</param>
        /// <param name="isIntNullable">Indica si el valor al que se le va a asignar el contenido de la celda puede ser un entero nulable (para poner 0 y que no falle la conversión).</param>
        /// <param name="isDate">Indica si el valor al que se le va a asignar el contenido de la celda es una fecha (se evalúa el tipo de dato numérico porque si el formato de Celda es Fecha la librería lo reconoce como un tipo de dato especial).</param>
        public Celda GetCellValue(int cellIndex, IRow row, bool isIntNullable = false, bool isDate = false)
        {
            ICell currentCell = row.GetCell(cellIndex);
            Celda cell = new Celda();
            String cellValue = "";
            if (currentCell != null)
            {
                CellReference cr = new CellReference(currentCell);
                if ((currentCell.CellType == CellType.Numeric) && isDate)
                {
                    DateTime fecha = currentCell.DateCellValue;
                    cellValue = fecha.ToShortDateString();
                }
                else
                {
                    currentCell.SetCellType(CellType.String);
                    cellValue = currentCell.StringCellValue.Trim();
                }
                cell.cell = cr.FormatAsString();
                setCeldaValue(cell.cell);
                cell.value = !String.IsNullOrEmpty(cellValue) ? cellValue : isIntNullable ? "0" : "";
            }
            else if (isIntNullable)
            {
                var celdaSesion = getCeldaFromSession();
                cell.cell = incrementCharacter(celdaSesion[0]).ToString() + celdaSesion.Substring(1, celdaSesion.Length - 1);
                cell.value = "0";
            }

            return cell;
        }
        ///<summary>
        ///Obtiene el compromiso de una fila determinada
        ///</summary>
        /// <param name="currentRow">Fila de la cual se van a sacar los datos de las celdas necesarias para el compromiso.</param>
        private PagoProveedoresExcel GetCompromiso(IRow currentRow)
        {
            PagoProveedoresExcel compromiso = new PagoProveedoresExcel();
            Celda cell = new Celda();
            setTipoDato("numérico");
            cell = GetCellValue((int)PagoProveedores.tipoCompromiso, currentRow);
            compromiso.tipoCompromiso = Convert.ToInt16(cell.value);
            compromiso.celdatipoCompromiso = cell.cell;
            cell = GetCellValue((int)PagoProveedores.consecutivo, currentRow);
            compromiso.consecutivo = Convert.ToInt32(cell.value);
            compromiso.celdaConsecutivo = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)PagoProveedores.idCuentaBancaria, currentRow, true);
            compromiso.idCuentaBancaria = Convert.ToInt32(cell.value);
            compromiso.celdaIdCuentaBancaria = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)PagoProveedores.fuenteFinanciamiento, currentRow);
            compromiso.fuenteFinanciamiento = cell.value;
            compromiso.celdaFuenteFinanciamiento = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)PagoProveedores.noCheque, currentRow);
            compromiso.noCheque = Convert.ToInt32(GetCellValue((int)PagoProveedores.noCheque, currentRow, true).value);
            compromiso.celdaNoCheque = cell.cell;
            cell = GetCellValue((int)PagoProveedores.spei, currentRow, true);
            compromiso.spei = Convert.ToInt32(cell.value);
            compromiso.celdaSpei = cell.cell;
            setTipoDato("fecha");
            cell = GetCellValue((int)PagoProveedores.fechaPago, currentRow, false, true);
            compromiso.fechaPago = Convert.ToDateTime(cell.value);
            compromiso.celdaFechaPago = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)PagoProveedores.noBeneficiario, currentRow);
            compromiso.noBeneficiario = Convert.ToInt32(cell.value);
            compromiso.celdaNoBeneficiario = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)PagoProveedores.cuentaBeneficiario, currentRow);
            compromiso.cuentaBeneficiario = cell.value;
            compromiso.celdaCuentaBeneficiario = cell.cell;
            cell = GetCellValue((int)PagoProveedores.descripcion, currentRow);
            compromiso.descripcion = cell.value;
            compromiso.celdaDescripcion = cell.cell;
            return compromiso;
        }
        ///<summary>
        ///Obtiene el detalle del compromiso de una fila determinada
        ///</summary>
        ///<param name="currentRow">Fila de la cual se van a sacar los datos de las celdas necesarias para el compromiso.</param>
        private DetallesPagoProveedoresExcel GetDetalleCompromiso(IRow currentRow)
        {
            DetallesPagoProveedoresExcel compromiso = new DetallesPagoProveedoresExcel();
            Celda cell = new Celda();
            setTipoDato("cadena");
            cell = GetCellValue((int)PagoProveedoresDetalle.centroGestor, currentRow);
            compromiso.centroGestor = cell.value;
            compromiso.celdaCentroGestor = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.funcion, currentRow);
            compromiso.funcion = cell.value;
            compromiso.celdaFuncion = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.compromiso, currentRow);
            compromiso.compromiso = cell.value;
            compromiso.celdaCompromiso = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.clasificacion, currentRow);
            compromiso.clasificacion = cell.value;
            compromiso.celdaClasificacion = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.programa, currentRow);
            if (cell.value.Length == 1)//El programa puede empezar con 0, pero si la persona que hizo el excel no le puso el formato a texto se borra el 0
                cell.value = "0" + cell.value;
            compromiso.programa = cell.value;
            compromiso.celdaPrograma = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.proyecto, currentRow);
            compromiso.proyecto = cell.value;
            compromiso.celdaProyecto = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.tipoMeta, currentRow);
            compromiso.tipoMeta = cell.value;
            compromiso.celdaTipoMeta = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.actividadMir, currentRow);
            compromiso.actividadMir = cell.value;
            compromiso.celdaActividadMir = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.accion, currentRow);
            compromiso.accion = cell.value;
            compromiso.celdaAccion = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.dimensionGeografica, currentRow);
            compromiso.dimensionGeografica = cell.value;
            compromiso.celdaDimensionGeografica = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.tipoGasto, currentRow);
            compromiso.tipoGasto = cell.value;
            compromiso.celdaTipoGasto = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.tipoFuente, currentRow);
            compromiso.tipoFuente = cell.value;
            compromiso.celdaTipoFuente = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.AnioFin, currentRow);
            compromiso.AnioFin = cell.value;
            compromiso.celdaAnioFinanciamiento = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.objetoGasto, currentRow);
            compromiso.objetoGasto = cell.value;
            compromiso.celdaObjetoGasto = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.cargos, currentRow, true);
            compromiso.cargos = Convert.ToDecimal(cell.value);
            compromiso.celdaCargos = cell.cell;
            return compromiso;
        }
        ///<summary>
        ///Obtiene el documento de un compromiso en una fila determinada
        ///</summary>
        /// <param name="currentRow">Fila de la cual se van a sacar los datos de las celdas necesarias para el documento del compromiso.</param>
        private DocumentosPagoProveedores GetDocumentoCompromiso(IRow currentRow)
        {
            DocumentosPagoProveedores documento = new DocumentosPagoProveedores();
            Celda cell = new Celda();
            setTipoDato("numérico");
            cell = GetCellValue((int)PagoProveedoresDocumentos.noProveedor, currentRow);
            documento.noProveedor = Convert.ToInt32(cell.value);
            documento.celdaNoProveedor = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDocumentos.noFactura, currentRow);
            documento.noFactura = cell.value;
            documento.celdaNoFactura = cell.cell;
            setTipoDato("fecha");
            cell = GetCellValue((int)PagoProveedoresDocumentos.fechaFactura, currentRow, false, true);
            documento.fechaFactura = Convert.ToDateTime(cell.value);
            documento.celdaFechaFactura = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)PagoProveedoresDocumentos.subtotal, currentRow, true);
            documento.subtotal = Convert.ToDecimal(cell.value);
            documento.celdaSubtotal = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDocumentos.iva, currentRow, true);
            documento.iva = Convert.ToDecimal(cell.value);
            documento.celdaIva = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDocumentos.retencionIva, currentRow, true);
            documento.retencionIva = Convert.ToDecimal(cell.value);
            documento.celdaRetencionIva = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDocumentos.retencionIsr, currentRow, true);
            documento.retencionIsr = Convert.ToDecimal(cell.value);
            documento.celdaRetencionIsr = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDocumentos.retencionObra, currentRow, true);
            documento.retencionObra = Convert.ToDecimal(cell.value);
            documento.celdaRetencionObra = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDocumentos.total, currentRow, true);
            documento.total = Convert.ToDecimal(cell.value);
            documento.celdaTotal = cell.cell;
            return documento;
        }
        ///<summary>
        ///Obtiene el fondo revolvente de una fila determinada
        ///</summary>
        /// <param name="currentRow">Fila de la cual se van a sacar los datos de las celdas necesarias para el fondo revolvente o gastos a comprobar.</param>
        private FondosyGastosExcel GetFondoRevolvente(IRow currentRow)
        {
            FondosyGastosExcel fondo = new FondosyGastosExcel();
            Celda cell = new Celda();
            cell = GetCellValue((int)FondosRevolvente.consecutivo, currentRow);
            fondo.consecutivo = Convert.ToInt32(cell.value);
            fondo.celdaConsecutivo = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)FondosRevolvente.idCuentaBancaria, currentRow, true);
            fondo.idCuentaBancaria = Convert.ToInt32(cell.value);
            fondo.celdaIdCuentaBancaria = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)FondosRevolvente.fuenteFinanciamiento, currentRow);
            fondo.fuenteFinanciamiento = cell.value;
            fondo.celdaFuenteFinanciamiento = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)FondosRevolvente.noCheque, currentRow, true);
            fondo.noCheque = Convert.ToInt32(cell.value);
            fondo.celdaNoCheque = cell.cell;
            cell = GetCellValue((int)FondosRevolvente.spei, currentRow, true);
            fondo.spei = Convert.ToInt32(cell.value);
            fondo.celdaSpei = cell.cell;
            setTipoDato("fecha");
            cell = GetCellValue((int)FondosRevolvente.fechaPago, currentRow, false, true);
            fondo.fechaPago = Convert.ToDateTime(cell.value);
            fondo.celdaFechaPago = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)FondosRevolvente.cuentaBeneficiario, currentRow);
            fondo.cuentaBeneficiario = cell.value;
            fondo.celdaCuentaBeneficiario = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)FondosRevolvente.descripcion, currentRow);
            fondo.descripcion = cell.value;
            fondo.celdaDescripcion = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)FondosRevolvente.importe, currentRow, true);
            fondo.importeCheque = Convert.ToDecimal(cell.value);
            fondo.celdaImporte = cell.cell;
            setTipoDato("fecha");
            cell = GetCellValue((int)FondosRevolvente.fechaComprobacion, currentRow, false, true);
            if (String.IsNullOrEmpty(cell.value))
            {
                fondo.fechaComprobacion = null;
            }
            else
            {
                fondo.fechaComprobacion = Convert.ToDateTime(cell.value);
            }
            fondo.celdaFechaComprobacion = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)FondosRevolvente.cuentaPorPagar, currentRow);
            fondo.cuentaPorPagar = cell.value;
            fondo.celdaCuentaPorPagar = cell.cell;
            return fondo;
        }
        ///<summary>
        ///Obtiene el recibo de ingresos de una fila determinada
        ///</summary>
        /// <param name="currentRow">Fila de la cual se van a sacar los datos de las celdas necesarias para el recibo de ingresos.</param>
        private ReciboIngresosExcel GetRecibo(IRow currentRow)
        {
            ReciboIngresosExcel recibo = new ReciboIngresosExcel();
            setTipoDato("numérico");
            Celda cell = GetCellValue((int)ReciboIngresosEnum.consecutivo, currentRow);
            recibo.noRecibo = Convert.ToInt32(cell.value);
            recibo.celdaNoRecibo = cell.cell;
            setTipoDato("fecha");
            cell = GetCellValue((int)ReciboIngresosEnum.fechaRecaudacion, currentRow, false, true);
            recibo.fechaRecaudacion = Convert.ToDateTime(cell.value);
            recibo.celdaFechaRecaudacion = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)ReciboIngresosEnum.noCajaReceptora, currentRow);
            recibo.noCajaReceptora = Convert.ToInt32(cell.value);
            recibo.celdaNoCajaReceptora = cell.cell;
            cell = GetCellValue((int)ReciboIngresosEnum.noContribuyente, currentRow);
            recibo.noContribuyente = Convert.ToInt32(cell.value);
            recibo.celdaNoContribuyente = cell.cell;
            cell = GetCellValue((int)ReciboIngresosEnum.importeTotal, currentRow);
            recibo.importe = Convert.ToInt32(cell.value);
            recibo.celdaImporte = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)ReciboIngresosEnum.idCtaBancaria, currentRow);
            recibo.idCtaBancaria = Convert.ToInt32(cell.value);
            recibo.celdaIdCtaBancaria = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)ReciboIngresosEnum.fuenteFinanciamiento, currentRow);
            recibo.fuenteFinanciamiento = cell.value;
            recibo.celdaFuenteFinanciamiento = cell.cell;
            cell = GetCellValue((int)ReciboIngresosEnum.descripcion, currentRow);
            recibo.descripcion = cell.value;
            recibo.celdaDescripcion = cell.cell;
            return recibo;
        }
        ///<summary>
        ///Obtiene el recibo de ingresos de una fila determinada
        ///</summary>
        /// <param name="currentRow">Fila de la cual se van a sacar los datos de las celdas necesarias para el recibo de ingresos.</param>
        private CancelacionPasivosExcel GetCancelacionPasivo(IRow currentRow)
        {
            CancelacionPasivosExcel cancelacion = new CancelacionPasivosExcel();
            setTipoDato("numérico");
            Celda cell = GetCellValue((int)CancelacionPasivosEnum.consecutivo, currentRow);
            cancelacion.consecutivo = Convert.ToInt32(cell.value);
            cancelacion.celdaConsecutivo = cell.cell;
            cell = GetCellValue((int)CancelacionPasivosEnum.noCuentaBancaria, currentRow);
            cancelacion.noCuentaBancaria = Convert.ToInt32(cell.value);
            cancelacion.celdaNoCtaBancaria = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)CancelacionPasivosEnum.fuenteFinanciamiento, currentRow);
            cancelacion.fuenteFinanciamiento = cell.value;
            cancelacion.celdaFuenteFinanciamiento = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)CancelacionPasivosEnum.noCheque, currentRow, true);
            cancelacion.noCheque = Convert.ToInt32(cell.value);
            cancelacion.celdaNoCheque = cell.cell;
            cell = GetCellValue((int)CancelacionPasivosEnum.spei, currentRow, true);
            cancelacion.spei = Convert.ToInt32(cell.value);
            cancelacion.celdaSpei = cell.cell;
            setTipoDato("fecha");
            cell = GetCellValue((int)CancelacionPasivosEnum.fechaPago, currentRow, false, true);
            cancelacion.fechaPago = Convert.ToDateTime(cell.value);
            cancelacion.celdaFechaPago = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)CancelacionPasivosEnum.cuentaBeneficiario, currentRow);
            cancelacion.cuentaBeneficiario = cell.value;
            cancelacion.celdaCuentaBeneficiario = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)CancelacionPasivosEnum.importeCheque, currentRow);
            cancelacion.importe = Convert.ToDecimal(cell.value);
            cancelacion.celdaImporte = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)CancelacionPasivosEnum.descripcion, currentRow);
            cancelacion.descripcion = cell.value;
            cancelacion.celdaDescripcion = cell.cell;
            return cancelacion;
        }
        private BeneficiariosExcel GetBeneficiario(IRow currentRow)
        {
            BeneficiariosExcel beneficiario = new BeneficiariosExcel();
            setTipoDato("cadena");
            Celda cell = GetCellValue((int)ProveedoresEnum.nombreCompleto, currentRow);
            beneficiario.nombreCompleto = cell.value;
            beneficiario.celdaNombreCompleto = cell.cell;
            cell = GetCellValue((int)ProveedoresEnum.nombre, currentRow);
            beneficiario.nombre = cell.value;
            beneficiario.celdaNombre = cell.cell;
            cell = GetCellValue((int)ProveedoresEnum.apellido1, currentRow);
            beneficiario.apellido1 = cell.value;
            beneficiario.celdaApellido1 = cell.cell;
            cell = GetCellValue((int)ProveedoresEnum.apellido2, currentRow);
            beneficiario.apellido2 = cell.value;
            beneficiario.celdaApellido2 = cell.cell;
            return beneficiario;
        }
        private string GetCuentaBeneficiario(IRow currentRow)
        {
            string cuenta = "";
            setTipoDato("cadena");
            Celda cell = GetCellValue((int)ProveedoresEnum.cuenta, currentRow);
            cuenta = cell.value;
            return cuenta;
        }
        ///<summ
        ///ary>
        ///Obtiene el siguiente caracter
        ///</summary>
        /// <param name="input">Caracter del cuál se desea obtener el siguiente. [Necesario para cuando existen celdas sin valores en el excel]</param>
        private char incrementCharacter(char input)
        {
            return (input == 'z' ? 'a' : (char)(input + 1));
        }
        ///<summary>
        ///Obtiene el detalle del compromiso de una fila determinada
        ///</summary>
        ///<param name="currentRow">Fila de la cual se van a sacar los datos de las celdas necesarias para el compromiso.</param>
        ///<param name="tipo">Si el parámetro es 0 indica que es un archivo de Gastos a Comprobar, si es 1 es de Fondos Revolventes.</param>
        private DetallesFondosyGastos GetDetalleFondoyGasto(IRow currentRow, byte tipo)
        {
            DetallesFondosyGastos compromiso = new DetallesFondosyGastos();
            Celda cell = new Celda();
            setTipoDato("cadena");
            cell = GetCellValue((int)PagoProveedoresDetalle.centroGestor, currentRow);
            compromiso.centroGestor = cell.value;
            compromiso.celdaCentroGestor = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.funcion, currentRow);
            compromiso.funcion = cell.value;
            compromiso.celdaFuncion = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.compromiso, currentRow);
            compromiso.compromiso = cell.value;
            compromiso.celdaCompromiso = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.clasificacion, currentRow);
            compromiso.clasificacion = cell.value;
            compromiso.celdaClasificacion = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.programa, currentRow);
            compromiso.programa = cell.value;
            compromiso.celdaPrograma = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.proyecto, currentRow);
            compromiso.proyecto = cell.value;
            compromiso.celdaProyecto = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.tipoMeta, currentRow);
            compromiso.tipoMeta = cell.value;
            compromiso.celdaTipoMeta = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.actividadMir, currentRow);
            compromiso.actividadMir = cell.value;
            compromiso.celdaActividadMir = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.accion, currentRow);
            compromiso.accion = cell.value;
            compromiso.celdaAccion = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.dimensionGeografica, currentRow);
            compromiso.dimensionGeografica = cell.value;
            compromiso.celdaDimensionGeografica = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.tipoGasto, currentRow);
            compromiso.tipoGasto = cell.value;
            compromiso.celdaTipoGasto = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.tipoFuente, currentRow);
            compromiso.tipoFuente = cell.value;
            compromiso.celdaTipoFuente = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.AnioFin, currentRow);
            compromiso.AnioFin = cell.value;
            compromiso.celdaAnioFinanciamiento = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.objetoGasto, currentRow);
            compromiso.objetoGasto = cell.value;
            compromiso.celdaObjetoGasto = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.cargos, currentRow, true);
            compromiso.cargos = Convert.ToDecimal(cell.value);
            compromiso.celdaCargos = cell.cell;
            return compromiso;
        }
        ///<summary>
        ///Obtiene el egreso no presupuestal de una fila determinada
        ///</summary>
        ///<param name="currentRow">Fila de la cual se van a sacar los datos de las celdas necesarias para el egreso. Se usa PagoProveedoresExcel porque tiene los campos necesarios para el egreso no presupuestal</param>
        private EgresosNoPresupuestalesExcel GetEgresoNoPresupuestal(IRow currentRow)
        {
            EgresosNoPresupuestalesExcel egreso = new EgresosNoPresupuestalesExcel();
            Celda cell = new Celda();
            setTipoDato("numérico");
            cell = GetCellValue((int)EgresosNoPEnum.consecutivo, currentRow);
            egreso.consecutivo = Convert.ToInt32(cell.value);
            egreso.celdaConsecutivo = cell.cell;
            cell = GetCellValue((int)EgresosNoPEnum.noCuentaBancaria, currentRow, true);
            egreso.idCuentaBancaria = Convert.ToInt32(cell.value);
            egreso.celdaIdCuentaBancaria = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)EgresosNoPEnum.fuenteFinanciamiento, currentRow);
            egreso.fuenteFinanciamiento = cell.value;
            egreso.celdaFuenteFinanciamiento = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)EgresosNoPEnum.noCheque, currentRow);
            egreso.noCheque = Convert.ToInt32(GetCellValue((int)EgresosNoPEnum.noCheque, currentRow, true).value);
            egreso.celdaNoCheque = cell.cell;
            cell = GetCellValue((int)EgresosNoPEnum.spei, currentRow, true);
            egreso.spei = Convert.ToInt32(cell.value);
            egreso.celdaSpei = cell.cell;
            setTipoDato("fecha");
            cell = GetCellValue((int)EgresosNoPEnum.fechaPago, currentRow, false, true);
            egreso.fechaPago = Convert.ToDateTime(cell.value);
            egreso.celdaFechaPago = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)EgresosNoPEnum.noBeneficiario, currentRow);
            egreso.noBeneficiario = Convert.ToInt32(cell.value);
            egreso.celdaNoBeneficiario = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)EgresosNoPEnum.importeCheque, currentRow);
            egreso.importeCheque = Convert.ToDecimal(cell.value);
            egreso.celdaImporteCheque = cell.cell;
            cell = GetCellValue((int)EgresosNoPEnum.descripcion, currentRow);
            egreso.descripcion = cell.value;
            egreso.celdaDescripcion = cell.cell;
            return egreso;
        }
        ///<summary>
        ///Obtiene el detalle del egreso no presupuestal de una fila determinada
        ///</summary>
        ///<param name="currentRow">Fila de la cual se van a sacar los datos de las celdas necesarias para el detalle de los egresos no presupuestales.</param>
        private DetallesEgresosNoPresupuestales GetDetalleEgresoNoPresupuestal(IRow currentRow)
        {
            DetallesEgresosNoPresupuestales detalle = new DetallesEgresosNoPresupuestales();
            Celda cell = new Celda();
            setTipoDato("cadena");
            cell = GetCellValue((int)PagoProveedoresDetalle.centroGestor, currentRow);
            detalle.cuenta = cell.value;
            detalle.celdaCuenta = cell.cell;
            cell = GetCellValue((int)PagoProveedoresDetalle.funcion, currentRow);
            detalle.importe = Convert.ToDecimal(cell.value);
            detalle.celdaImporte = cell.cell;
            return detalle;
        }
        ///<summary>
        ///Obtiene el detalle del compromiso de una fila determinada
        ///</summary>
        ///<param name="currentRow">Fila de la cual se van a sacar los datos de las celdas necesarias para el compromiso.</param>
        private DetalleArrendamientos GetDetalleArrendamiento(IRow currentRow)
        {
            DetalleArrendamientos detalle = new DetalleArrendamientos();
            Celda cell = new Celda();
            setTipoDato("cadena");
            cell = GetCellValue((int)ArrendamientosDetalle.centroGestor, currentRow);
            detalle.centroGestor = cell.value;
            detalle.celdaCentroGestor = cell.cell;
            cell = GetCellValue((int)ArrendamientosDetalle.funcion, currentRow);
            detalle.funcion = cell.value;
            detalle.celdaFuncion = cell.cell;
            cell = GetCellValue((int)ArrendamientosDetalle.compromiso, currentRow);
            detalle.compromiso = cell.value;
            detalle.celdaCompromiso = cell.cell;
            cell = GetCellValue((int)ArrendamientosDetalle.clasificacion, currentRow);
            detalle.clasificacion = cell.value;
            detalle.celdaClasificacion = cell.cell;
            cell = GetCellValue((int)ArrendamientosDetalle.programa, currentRow);
            if (cell.value.Length == 1)//El programa puede empezar con 0, pero si la persona que hizo el excel no le puso el formato a texto se borra el 0
                cell.value = "0" + cell.value;
            detalle.programa = cell.value;
            detalle.celdaPrograma = cell.cell;
            cell = GetCellValue((int)ArrendamientosDetalle.proyecto, currentRow);
            detalle.proyecto = cell.value;
            detalle.celdaProyecto = cell.cell;
            cell = GetCellValue((int)ArrendamientosDetalle.tipoMeta, currentRow);
            detalle.tipoMeta = cell.value;
            detalle.celdaTipoMeta = cell.cell;
            cell = GetCellValue((int)ArrendamientosDetalle.actividadMir, currentRow);
            detalle.actividadMir = cell.value;
            detalle.celdaActividadMir = cell.cell;
            cell = GetCellValue((int)ArrendamientosDetalle.accion, currentRow);
            detalle.accion = cell.value;
            detalle.celdaAccion = cell.cell;
            cell = GetCellValue((int)ArrendamientosDetalle.dimensionGeografica, currentRow);
            detalle.dimensionGeografica = cell.value;
            detalle.celdaDimensionGeografica = cell.cell;
            cell = GetCellValue((int)ArrendamientosDetalle.tipoGasto, currentRow);
            detalle.tipoGasto = cell.value;
            detalle.celdaTipoGasto = cell.cell;
            cell = GetCellValue((int)ArrendamientosDetalle.tipoFuente, currentRow);
            detalle.tipoFuente = cell.value;
            detalle.celdaTipoFuente = cell.cell;
            cell = GetCellValue((int)ArrendamientosDetalle.AnioFin, currentRow);
            detalle.AnioFin = cell.value;
            detalle.celdaAnioFinanciamiento = cell.cell;
            cell = GetCellValue((int)ArrendamientosDetalle.objetoGasto, currentRow);
            detalle.objetoGasto = cell.value;
            detalle.celdaObjetoGasto = cell.cell;
            cell = GetCellValue((int)ArrendamientosDetalle.cuenta, currentRow);
            detalle.cuenta = cell.value;
            detalle.celdaCuenta = cell.cell;
            if (!String.IsNullOrEmpty(detalle.cuenta))
            {
                setTipoDato("numérico");
                cell = GetCellValue((int)ArrendamientosDetalle.cargos, currentRow, true);
                detalle.cargos = Convert.ToDecimal(cell.value);
                detalle.celdaCargos = cell.cell;
                //cell = GetCellValue((int)ArrendamientosDetalle.abonos, currentRow, true);
                //detalle.abonos = Convert.ToDecimal(cell.value);
                //detalle.celdaAbonos = cell.cell;
            }
            return detalle;
        }
        ///<summary>
        ///Obtiene el arrendamiento y honorario de una fila determinada
        ///</summary>
        ///<param name="currentRow">Fila de la cual se van a sacar los datos de las celdas necesarias para el arrendamiento.</param>
        ///<param name="tipo">Tipo del archivo -> 0.- Arrendamientos, 1.- Honorarios, 2.- Cancelación de activos, 3.- Honorarios asimilables.</param>
        private ArrendamientosyHonorariosExcel GetArrendamientoyHonorario(IRow currentRow, byte tipo)
        {
            ArrendamientosyHonorariosExcel arrendamiento = new ArrendamientosyHonorariosExcel();
            Celda cell = new Celda();
            setTipoDato("numérico");
            //cell = GetCellValue((int)ArrendamientoyHonorariosEnum.tipoCompromiso, currentRow);
            //arrendamiento.tipoCompromiso = Convert.ToInt16(cell.value);
            //arrendamiento.celdatipoCompromiso = cell.cell;
            cell = GetCellValue((int)ArrendamientoyHonorariosEnum.consecutivo, currentRow);
            arrendamiento.consecutivo = Convert.ToInt32(cell.value);
            arrendamiento.celdaConsecutivo = cell.cell;
            cell = GetCellValue((int)ArrendamientoyHonorariosEnum.idCuentaBancaria, currentRow, true);
            arrendamiento.idCuentaBancaria = Convert.ToInt32(cell.value);
            arrendamiento.celdaIdCuentaBancaria = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)ArrendamientoyHonorariosEnum.fuenteFinanciamiento, currentRow);
            arrendamiento.fuenteFinanciamiento = cell.value;
            arrendamiento.celdaFuenteFinanciamiento = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)ArrendamientoyHonorariosEnum.noCheque, currentRow);
            arrendamiento.noCheque = Convert.ToInt32(GetCellValue((int)ArrendamientoyHonorariosEnum.noCheque, currentRow, true).value);
            arrendamiento.celdaNoCheque = cell.cell;
            cell = GetCellValue((int)ArrendamientoyHonorariosEnum.spei, currentRow, true);
            arrendamiento.spei = Convert.ToInt32(cell.value);
            arrendamiento.celdaSpei = cell.cell;
            setTipoDato("fecha");
            cell = GetCellValue((int)ArrendamientoyHonorariosEnum.fechaPago, currentRow, false, true);
            arrendamiento.fechaPago = Convert.ToDateTime(cell.value);
            arrendamiento.celdaFechaPago = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)ArrendamientoyHonorariosEnum.noBeneficiario, currentRow);
            arrendamiento.noBeneficiario = Convert.ToInt32(cell.value);
            arrendamiento.celdaNoBeneficiario = cell.cell;
            cell = GetCellValue((int)ArrendamientoyHonorariosEnum.cuentaBeneficiario, currentRow);
            arrendamiento.cuentaBeneficiario = cell.value;
            arrendamiento.celdaCuentaBeneficiario = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)ArrendamientoyHonorariosEnum.importeCheque, currentRow);
            arrendamiento.importeCheque = Convert.ToDecimal(cell.value);
            arrendamiento.celdaImporteCheque = cell.cell;
            cell = GetCellValue((int)ArrendamientoyHonorariosEnum.cuentaPA, currentRow);
            arrendamiento.cuentaActivoPasivo = cell.value;
            arrendamiento.celdaCuentaActivoP = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)ArrendamientoyHonorariosEnum.importePA, currentRow);
            arrendamiento.importeActivoP = Convert.ToDecimal(cell.value);
            arrendamiento.celdaImporteActivoP = cell.cell;
            cell = GetCellValue((int)ArrendamientoyHonorariosEnum.descripcion, currentRow);
            arrendamiento.descripcion = cell.value;
            arrendamiento.celdaDescripcion = cell.cell;
                setTipoDato("numérico");
                cell = GetCellValue((int)ArrendamientoyHonorariosEnum.tipoMovimiento, currentRow, true);
                arrendamiento.TipoMovimiento = Convert.ToInt32(cell.value);
                arrendamiento.celdaTipoMovimiento = cell.cell;

            return arrendamiento;
        }

        ///<param name="tipo">Tipo del archivo -> 0.- Arrendamientos, 1.- Honorarios, 2.- Cancelación de activos, 3.- Honorarios asimilables.</param>
        private ArrendamientosyHonorariosExcel GetHonorario(IRow currentRow, byte tipo)
        {
            ArrendamientosyHonorariosExcel arrendamiento = new ArrendamientosyHonorariosExcel();
            Celda cell = new Celda();
            setTipoDato("numérico");
            //cell = GetCellValue((int)ArrendamientoyHonorariosEnum.tipoCompromiso, currentRow);
            //arrendamiento.tipoCompromiso = Convert.ToInt16(cell.value);
            //arrendamiento.celdatipoCompromiso = cell.cell;
            cell = GetCellValue((int)HonorariosEnum.consecutivo, currentRow);
            arrendamiento.consecutivo = Convert.ToInt32(cell.value);
            arrendamiento.celdaConsecutivo = cell.cell;
            cell = GetCellValue((int)HonorariosEnum.idCuentaBancaria, currentRow, true);
            arrendamiento.idCuentaBancaria = Convert.ToInt32(cell.value);
            arrendamiento.celdaIdCuentaBancaria = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)HonorariosEnum.fuenteFinanciamiento, currentRow);
            arrendamiento.fuenteFinanciamiento = cell.value;
            arrendamiento.celdaFuenteFinanciamiento = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)HonorariosEnum.noCheque, currentRow);
            arrendamiento.noCheque = Convert.ToInt32(GetCellValue((int)ArrendamientoyHonorariosEnum.noCheque, currentRow, true).value);
            arrendamiento.celdaNoCheque = cell.cell;
            cell = GetCellValue((int)HonorariosEnum.spei, currentRow, true);
            arrendamiento.spei = Convert.ToInt32(cell.value);
            arrendamiento.celdaSpei = cell.cell;
            setTipoDato("fecha");
            cell = GetCellValue((int)HonorariosEnum.fechaPago, currentRow, false, true);
            arrendamiento.fechaPago = Convert.ToDateTime(cell.value);
            arrendamiento.celdaFechaPago = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)HonorariosEnum.noBeneficiario, currentRow);
            arrendamiento.noBeneficiario = Convert.ToInt32(cell.value);
            arrendamiento.celdaNoBeneficiario = cell.cell;
            cell = GetCellValue((int)HonorariosEnum.cuentaBeneficiario, currentRow);
            arrendamiento.cuentaBeneficiario = cell.value;
            arrendamiento.celdaCuentaBeneficiario = cell.cell;
            setTipoDato("cadena");
            cell = GetCellValue((int)HonorariosEnum.importeCheque, currentRow);
            arrendamiento.importeCheque = Convert.ToDecimal(cell.value);
            arrendamiento.celdaImporteCheque = cell.cell;
            cell = GetCellValue((int)HonorariosEnum.cuentaPA, currentRow);
            arrendamiento.cuentaActivoPasivo = cell.value;
            arrendamiento.celdaCuentaActivoP = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)HonorariosEnum.importePA, currentRow);
            arrendamiento.importeActivoP = cell.value != "" ? Convert.ToDecimal(cell.value) : 0;
            arrendamiento.celdaImporteActivoP = cell.cell;
            /***   cuentas de pasivo **/


            cell = GetCellValue((int)HonorariosEnum.cuentaPA2, currentRow);
            arrendamiento.cuentaActivoPasivo2 = cell.value;
            arrendamiento.celdaCuentaActivoP2 = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)HonorariosEnum.importePA2, currentRow);
            arrendamiento.importeActivoP2 = cell.value != "" ? Convert.ToDecimal(cell.value) : 0;
            arrendamiento.celdaImporteActivoP2 = cell.cell;

            cell = GetCellValue((int)HonorariosEnum.cuentaPA3, currentRow);
            arrendamiento.cuentaActivoPasivo3 = cell.value;
            arrendamiento.celdaCuentaActivoP3 = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)HonorariosEnum.importePA3, currentRow);
            arrendamiento.importeActivoP3 = cell.value != "" ? Convert.ToDecimal(cell.value) : 0;
            arrendamiento.celdaImporteActivoP3 = cell.cell;


            cell = GetCellValue((int)HonorariosEnum.cuentaPA4, currentRow);
            arrendamiento.cuentaActivoPasivo4 = cell.value;
            arrendamiento.celdaCuentaActivoP4 = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)HonorariosEnum.importePA4, currentRow);
            arrendamiento.importeActivoP4 = cell.value != "" ?  Convert.ToDecimal(cell.value): 0;
            arrendamiento.celdaImporteActivoP4 = cell.cell;



            /** **/
            cell = GetCellValue((int)HonorariosEnum.descripcion, currentRow);
            arrendamiento.descripcion = cell.value;
            arrendamiento.celdaDescripcion = cell.cell;
            if (tipo == 3)
            {
                setTipoDato("numérico");
                cell = GetCellValue((int)HonorariosEnum.tipoMovimiento, currentRow, true);
                arrendamiento.TipoMovimiento = Convert.ToInt32(cell.value);
                arrendamiento.celdaTipoMovimiento = cell.cell;
            }
            return arrendamiento;
        }
        private DetalleReciboIngreso GetDetalleReciboIngreso(IRow currentRow)
        {
            DetalleReciboIngreso detalle = new DetalleReciboIngreso();
            Celda cell = new Celda();
            setTipoDato("cadena");
            cell = GetCellValue((int)DetalleReciboIngresos.centroRecaudador, currentRow);
            detalle.centroRecaudador = cell.value;
            detalle.celdaCentroRecaudador = cell.cell;
            cell = GetCellValue((int)DetalleReciboIngresos.dimensionGeografica, currentRow);
            detalle.dimension = cell.value;
            detalle.celdaDimension = cell.cell;
            cell = GetCellValue((int)DetalleReciboIngresos.fuenteFinanciamiento, currentRow);
            detalle.fuenteFinanciamiento = cell.value;
            detalle.celdaFuenteFinanciamiento = cell.cell;
            setTipoDato("numérico");
            cell = GetCellValue((int)DetalleReciboIngresos.anioFinanciamiento, currentRow);
            detalle.anioFinanciamiento = cell.value;
            detalle.celdaAnioFinanciamiento = cell.cell;
            cell = GetCellValue((int)DetalleReciboIngresos.cri, currentRow);
            detalle.cri = cell.value;
            detalle.celdaCri = cell.cell;
            cell = GetCellValue((int)DetalleReciboIngresos.cur, currentRow);
            detalle.cur = cell.value;
            detalle.celdaCur = cell.cell;
            cell = GetCellValue((int)DetalleReciboIngresos.importe, currentRow);
            detalle.importe = Convert.ToDecimal(cell.value);
            detalle.celdaImporte = cell.cell;
            return detalle;
        }
        public int getNextIdContrarecibo(short tipoContrarecibo)
        {
            try
            {
                return tipoContrarecibosDal.Get(reg => reg.Id_TipoCR == tipoContrarecibo).Max(max => max.UltimoCR).Value + 1;
            }
            catch (Exception)
            {
                return 1;
            }
        }
        public void updateNextIdContrarecibo(short tipoContrarecibo)
        {
            Ca_TipoContrarecibos entity = tipoContrarecibosDal.GetByID(reg => reg.Id_TipoCR == tipoContrarecibo);
            entity.UltimoCR = entity.UltimoCR + 1;
            tipoContrarecibosDal.Update(entity);
            tipoContrarecibosDal.Save();
        }
        public int getNextIdDetalleContrarecibo(short tipoContrarecibo, int folioContrarecibo)
        {
            try
            {
                return deTipoContrareciboDal.Get(reg => reg.Id_TipoCR == tipoContrarecibo && reg.Id_FolioCR == folioContrarecibo).Max(max => max.Id_Registro) + 1;
            }
            catch (Exception)
            {
                return 1;
            }
        }
        public int getNextIdDetalleRecibo()
        {
            try
            {
                return deRecibosDal.Get().Max(max => max.IdRegistro) + 1;
            }
            catch (Exception)
            {
                return 1;
            }
        }
        public Ma_Compromisos insertaCompromiso(short idTipoCompromiso, decimal sumatoria, byte idTipoCr, short estatus, DateTime fecha, int idBeneficiario, String cuentaBeneficiario, String observaciones, String usuarioOrden,
            short idUsuarioAct, int idFolioCR)
        {
            int idFolioCompromiso = compromisosBl.getNextId(idTipoCompromiso);
            compromisosBl.updateNextId((short)idTipoCompromiso);
            Ma_Compromisos maestro = new Ma_Compromisos();
            maestro.Cargos = maestro.Abonos = sumatoria;
            maestro.Id_TipoCR = idTipoCr;
            maestro.Id_FolioCR = idFolioCR;
            maestro.Estatus = estatus;
            maestro.Id_FolioCompromiso = idFolioCompromiso;
            maestro.Fecha = fecha;
            maestro.Id_Beneficiario = idBeneficiario;
            maestro.Id_Cuenta_Beneficiario = cuentaBeneficiario;
            maestro.Observaciones = observaciones;
            maestro.Usuario_Orden = usuarioOrden;
            maestro.Fecha_Orden = maestro.Fecha_Devengado = fecha;
            maestro.Adquisicion = false;
            maestro.Id_TipoCompromiso = idTipoCompromiso;
            maestro.Usuario_Act = (short)idUsuarioAct;
            compromisosDal.Insert(maestro);
            compromisosDal.Save();

            return maestro;
        }
        public De_Compromisos insertaDetalleCompromiso(String clavePresupuestaria, byte idMovimiento, short idTipoCompromiso, short idUsuario, int idFolioCompromiso, String objetoGasto, decimal cargos, String cuentaBeneficiario)
        {
            De_Compromisos detalle = new De_Compromisos();
            detalle.Id_ClavePresupuesto = clavePresupuestaria;
            detalle.Id_Movimiento = idMovimiento;
            detalle.Id_TipoCompromiso = (short)idTipoCompromiso;
            detalle.Id_Cuenta = String.IsNullOrEmpty(objetoGasto) ? cuentaBeneficiario : cuentasDal.Get(x => x.Id_ObjetoG == objetoGasto).FirstOrDefault().Id_Cuenta;
            detalle.Usuario_Act = (short)idUsuario;
            detalle.Fecha_Act = DateTime.Now;
            detalle.Importe = cargos;
            detalle.Id_FolioCompromiso = idFolioCompromiso;
            detalle.Id_Registro = (short)deCompromisosBl.getNextId((short)idTipoCompromiso, idFolioCompromiso);
            detalleCompromisosDal.Insert(detalle);
            detalleCompromisosDal.Save();
            return detalle;
        }
        public Ma_Contrarecibos insertaContrareciboPago(PagoProveedoresExcel item, short idCuentaBancaria, int idFolioCR, decimal total, int noCheque, string usuario, short tipoCR,
            String cuentaFR, byte? estatusGC, DateTime? fechaCierre, short tipoCompromiso)
        {
            Ma_Contrarecibos contrarecibo = new Ma_Contrarecibos();
            contrarecibo.Id_TipoCR = (byte)tipoCR;
            contrarecibo.FechaCR = contrarecibo.FechaVen = item.fechaPago;
            contrarecibo.Descripcion = item.descripcion;
            contrarecibo.Id_Beneficiario = item.noBeneficiario;
            contrarecibo.Id_FolioCR = idFolioCR;
            contrarecibo.Impreso_CR = contrarecibo.Impreso_CH = true;
            contrarecibo.Id_CtaBancaria = idCuentaBancaria;
            contrarecibo.Id_EstatusCR = 2;
            contrarecibo.Id_TipoCompromiso = tipoCompromiso;
            contrarecibo.Id_Fuente = item.fuenteFinanciamiento;
            contrarecibo.Cargos = contrarecibo.Abonos = total;
            contrarecibo.Spei = item.spei == 0 ? null : item.spei.ToString();
            contrarecibo.No_Cheque = noCheque;
            contrarecibo.Usu_Pago = contrarecibo.Usu_CR = usuario;
            contrarecibo.Fecha_Pago = item.fechaPago;
            contrarecibo.Id_CuentaFR = cuentaFR;
            contrarecibo.Estatus_GC = estatusGC;
            contrarecibo.FechaCierreGC = fechaCierre;
            contrarecibosDal.Insert(contrarecibo);
            contrarecibosDal.Save();
            updateNextIdContrarecibo(tipoCR);
            return contrarecibo;
        }
        public Ma_Contrarecibos insertarContrarecibo(DateTime fechaPago, string descripcion, int? noBeneficiario, string fuenteFinanciamiento, int spei,
            short? idCuentaBancaria, int idFolioCR, decimal total, int noCheque, string usuario, short tipoCR, String cuentaFR, byte? estatusGC,
            DateTime? fechaCierre, short? tipoCompromiso, short? usuAct, decimal? importe_ah, string cuenta_ah, int? idPersonaENP = null)
        {
            Ma_Contrarecibos contrarecibo = new Ma_Contrarecibos();
            contrarecibo.Id_TipoCR = (byte)tipoCR;
            contrarecibo.FechaCR = contrarecibo.FechaVen = contrarecibo.Fecha_Pago = fechaPago;
            contrarecibo.Descripcion = descripcion;
            contrarecibo.Id_Beneficiario = noBeneficiario;
            contrarecibo.Id_FolioCR = idFolioCR;
            contrarecibo.Impreso_CR = contrarecibo.Impreso_CH = true;
            contrarecibo.Id_CtaBancaria = idCuentaBancaria;
            contrarecibo.Id_EstatusCR = 2;
            contrarecibo.Id_TipoCompromiso = tipoCompromiso;
            contrarecibo.Id_Fuente = fuenteFinanciamiento;
            contrarecibo.Cargos = contrarecibo.Abonos = total;
            contrarecibo.Spei = spei == 0 ? null : spei.ToString();
            contrarecibo.No_Cheque = noCheque;
            contrarecibo.Usu_Pago = contrarecibo.Usu_CR = usuario;
            contrarecibo.Id_CuentaFR = cuentaFR;
            contrarecibo.Estatus_GC = estatusGC;
            contrarecibo.Usuario_Act = usuAct;
            contrarecibo.Fecha_Act = DateTime.Now;
            contrarecibo.Importe_AH = importe_ah;
            contrarecibo.Id_Cuenta_AH = cuenta_ah;
            contrarecibo.IdPersona_ENP = idPersonaENP;
            contrarecibo.FechaCierreGC = fechaCierre;
            contrarecibosDal.Insert(contrarecibo);
            contrarecibosDal.Save();
            updateNextIdContrarecibo(tipoCR);
            return contrarecibo;
        }


        //Insertar hornorarios
        public Ma_Contrarecibos insertarContrareciboH(DateTime fechaPago, string descripcion, int? noBeneficiario, string fuenteFinanciamiento, int spei,
           short? idCuentaBancaria, int idFolioCR, decimal total, int noCheque, string usuario, short tipoCR, String cuentaFR, byte? estatusGC,
           DateTime? fechaCierre, short? tipoCompromiso, short? usuAct, decimal? importe_ah, string cuenta_ah, decimal? importe_ah2, string cuenta_ah2, decimal? importe_ah3, string cuenta_ah3, decimal? importe_ah4, string cuenta_ah4, decimal importeCheque, int? idPersonaENP = null)
        {
            Ma_Contrarecibos contrarecibo = new Ma_Contrarecibos();
            contrarecibo.Id_TipoCR = (byte)tipoCR;
            contrarecibo.FechaCR = contrarecibo.FechaVen = contrarecibo.Fecha_Pago = fechaPago;
            contrarecibo.Descripcion = descripcion;
            contrarecibo.Id_Beneficiario = noBeneficiario;
            contrarecibo.Id_FolioCR = idFolioCR;
            contrarecibo.Impreso_CR = contrarecibo.Impreso_CH = true;
            contrarecibo.Id_CtaBancaria = idCuentaBancaria;
            contrarecibo.Id_EstatusCR = 2;
            contrarecibo.Id_TipoCompromiso = tipoCompromiso;
            contrarecibo.Id_Fuente = fuenteFinanciamiento;
            contrarecibo.Cargos = contrarecibo.Abonos = total;
            contrarecibo.Spei = spei == 0 ? null : spei.ToString();
            contrarecibo.No_Cheque = noCheque;
            contrarecibo.Usu_Pago = contrarecibo.Usu_CR = usuario;
            contrarecibo.Id_CuentaFR = cuentaFR;
            contrarecibo.Estatus_GC = estatusGC;
            contrarecibo.Usuario_Act = usuAct;
            contrarecibo.Fecha_Act = DateTime.Now;
            contrarecibo.Importe_AH = importe_ah;
            contrarecibo.Id_Cuenta_AH = cuenta_ah;
            /* */
            contrarecibo.Importe_AH2 = importe_ah2;
            contrarecibo.Id_Cuenta_AH2 = cuenta_ah2;
            contrarecibo.Importe_AH3 = importe_ah3;
            contrarecibo.Id_Cuenta_AH3 = cuenta_ah3;
            contrarecibo.Importe_AH4 = importe_ah4;
            contrarecibo.Id_Cuenta_AH4 = cuenta_ah4;
            contrarecibo.Importe_CH = importeCheque;
            /* */
            contrarecibo.IdPersona_ENP = idPersonaENP;
            contrarecibo.FechaCierreGC = fechaCierre;
            contrarecibosDal.Insert(contrarecibo);
            contrarecibosDal.Save();
            updateNextIdContrarecibo(tipoCR);
            return contrarecibo;
        }

        ///<summary>
        ///Actualiza el contrarecibo
        ///</summary>
        ///<param name="tipoCR">ID del tipo del Contrarecibo.</param>
        ///<param name="idFolioCR">Folio del contrarecibo.</param>
        ///<param name="tipo">Tipo del archivo -> 0.- Arrendamientos, 1.- Honorarios Puros, 2.- Cancelación de activos,3.- Honorarios Asimilables.</param>
        public Ma_Contrarecibos actualizarContrarecibo(byte tipoCR, int idFolioCR, decimal? importe_ah, byte tipo, int tipoMovimiento)
        {
            Ma_Contrarecibos contrarecibo = contrarecibosDal.GetByID(x => x.Id_TipoCR == tipoCR && x.Id_FolioCR == idFolioCR);
            decimal? importeCh = 0;
            switch (tipo)
            {
                case 0:
                case 1:
                case 2:
                    importeCh = contrarecibo.Cargos - importe_ah;
                    break;
                case 3:
                    importeCh = tipoMovimiento == 1 ? contrarecibo.Cargos + importe_ah : contrarecibo.Cargos - importe_ah;
                    break;
            }
            contrarecibo.Importe_CH = importeCh;
            contrarecibosDal.Update(contrarecibo);
            contrarecibosDal.Save();
            return contrarecibo;
        }
        public DE_Bancos actualizaDetalleBanco(PagoProveedoresExcel item, int idUsuario, decimal sumatoria, short idCtaBancaria)
        {
            DE_Bancos detalleBancos = deBancoDal.GetByID(x => x.Id_CtaBancaria == idCtaBancaria && x.No_Cheque == item.noCheque);
            detalleBancos.Id_Estatus = 2;
            detalleBancos.Id_Beneficiario = item.noBeneficiario;
            detalleBancos.Importe = sumatoria;
            detalleBancos.Observa = item.descripcion;
            detalleBancos.Fecha = item.fechaPago;
            detalleBancos.Usu_Act = (short)idUsuario;
            detalleBancos.Fecha_Act = DateTime.Now;
            deBancoDal.Update(detalleBancos);
            deBancoDal.Save();
            return detalleBancos;
        }
        public DE_Bancos actualizaDetalleBancoCancelaciones(CancelacionPasivosExcel item, int idUsuario, decimal importe, short idCtaBancaria)
        {
            DE_Bancos detalleBancos = deBancoDal.GetByID(x => x.Id_CtaBancaria == idCtaBancaria && x.No_Cheque == item.noCheque);
            detalleBancos.Id_Estatus = 2;
            detalleBancos.Id_Beneficiario = item.noCuentaBancaria;
            detalleBancos.Importe = importe;
            detalleBancos.Observa = item.descripcion;
            detalleBancos.Fecha = item.fechaPago;
            detalleBancos.Usu_Act = (short)idUsuario;
            detalleBancos.Fecha_Act = DateTime.Now;
            deBancoDal.Update(detalleBancos);
            deBancoDal.Save();
            return detalleBancos;
        }
        public De_Contrarecibos insertarDetalleContrarecibo(String clavePresupuestaria, String objetoGasto, decimal total, int idUsuario, int idFolioCr, String cuentaBeneficiario,
            byte tipoMovimiento, short tipoContrarecibo, bool excedido = false)
        {
            De_Contrarecibos detalle = new De_Contrarecibos();
            detalle.Id_ClavePresupuesto = clavePresupuestaria;
            detalle.Id_Movimiento = tipoMovimiento;
            detalle.Id_TipoCR = (byte)tipoContrarecibo;
            if (excedido)
                detalle.Id_Cuenta = cuentaBeneficiario;
            else
                detalle.Id_Cuenta = String.IsNullOrEmpty(objetoGasto) ? cuentaBeneficiario : cuentasDal.Get(x => x.Id_ObjetoG == objetoGasto).FirstOrDefault().Id_Cuenta;
            detalle.Usuario_Act = (short)idUsuario;
            detalle.Fecha_Act = DateTime.Now;
            detalle.Importe = total;
            detalle.Id_FolioCR = idFolioCr;
            detalle.Id_Registro = (short)getNextIdDetalleContrarecibo(tipoContrarecibo, detalle.Id_FolioCR);
            deCrDal.Insert(detalle);
            deCrDal.Save();
            return detalle;
        }
        public void insertarDetallesCrFactura(byte tipoCr, int nextFolioContrarecibo, int idProveedor, byte? tipoDocto, string noDocto, DateTime? fechaFactura, decimal? subtotal, decimal? iva,
            decimal? retIva, decimal? retISR, decimal? retObra, decimal? total)
        {
            De_CR_Facturas deFactura = new De_CR_Facturas();
            deFactura.Id_TipoCR = tipoCr;
            deFactura.Id_FolioCR = nextFolioContrarecibo;
            deFactura.Id_Proveedor = idProveedor;
            deFactura.Id_TipoDocto = tipoDocto;
            deFactura.No_docto = noDocto;
            deFactura.Fecha = fechaFactura;
            deFactura.SubTotal = subtotal;
            deFactura.IVA = iva;
            deFactura.Ret_IVA = retIva;
            deFactura.Ret_ISR = retISR;
            deFactura.Ret_Obra = retObra;
            deFactura.TOTAL = total;
            deCrFacturasDal.Insert(deFactura);
            deCrFacturasDal.Save();
        }
        public void insertaReciboIngresos(int folio, DateTime fecha, byte idCaja, int idContribuyente, string observaciones, decimal importe, string usuario, short? idUsuario, short idCtaBancaria)
        {
            Ma_ReciboIngresos recibo = new Ma_ReciboIngresos();
            recibo.Folio = folio;
            recibo.Fecha = fecha;
            recibo.Id_CtaBancaria = idCtaBancaria;
            recibo.Id_CajaR = idCaja;
            recibo.IdContribuyente = idContribuyente;
            recibo.Observaciones = observaciones;
            recibo.Importe = importe;
            recibo.IdEstatus = 2;
            recibo.Id_Banco = cuentasBancariasDal.GetByID(x => x.Id_CtaBancaria == idCtaBancaria).Id_Banco;
            recibo.Impreso = true;
            recibo.FechaRecaudacion = fecha;
            recibo.Usuario_Recaudacion = recibo.Usuario_Captura = usuario;
            recibo.uAct = idUsuario;
            recibo.fAct = DateTime.Now;
            reciboIngDal.Insert(recibo);
            reciboIngDal.Save();
        }
        public void insertaDetalleRecibo(int folio, string cur, string clavePresupuestaria, decimal? importe, string idConcepto, byte? idMovimiento, short? idUsuario)
        {
            De_ReciboIngresos detalle = new De_ReciboIngresos();
            detalle.Folio = folio;
            detalle.IdRegistro = (byte)getNextIdDetalleRecibo();
            detalle.IdCur = cur;
            detalle.Id_ClavePresupuestoIng = clavePresupuestaria;
            detalle.Importe = importe;
            detalle.Id_Cuenta = idConcepto.Length == 3 ? cuentasDal.GetByID(x => x.Id_Concepto == idConcepto).Id_Cuenta : idConcepto;
            detalle.Id_Movimiento = idMovimiento;
            detalle.uAct = idUsuario;
            detalle.fAct = DateTime.Now;
            deRecibosDal.Insert(detalle);
            deRecibosDal.Save();
        }

        public void actualizarFolioIngresos(int noRecibo, short idUsuario)
        {
            ParametrosDAL parametrosDal = new ParametrosDAL();
            CA_Parametros parametro = parametrosDal.GetByID(x => x.Nombre == "UltimoFolioIngresos");
            parametro.Valor = noRecibo.ToString();
            parametro.uAct = idUsuario;
            parametro.fAct = DateTime.Now;
            parametrosDal.Update(parametro);
            parametrosDal.Save();
        }
    }
}