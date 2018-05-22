using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
namespace TesoreriaVS12.Areas.Tesoreria.Controllers
{
    public class ExcelController : Controller
    {
        //
        // GET: /Tesoreria/Excel/

        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ProcesarExcel(FormCollection form)
        {
            List<Valores> listaValores = new List<Valores>();
            if (Request.Files["FileUpload1"].ContentLength > 0)
            {
                string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName);
                string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Archivos"), Request.Files["FileUpload1"].FileName);
                if (System.IO.File.Exists(path1))
                    System.IO.File.Delete(path1);
                Request.Files["FileUpload1"].SaveAs(path1);
                XSSFWorkbook hssfwb;
                using (FileStream file = new FileStream(path1, FileMode.Open, FileAccess.Read))
                {
                    hssfwb = new XSSFWorkbook(file);
                }
                ISheet sheet = hssfwb.GetSheet("prueba");
                IRow encabezado = sheet.GetRow(6);
                int totalCeldas = encabezado.LastCellNum;
                int totalFilas = sheet.LastRowNum;
                //for (int i = encabezado.FirstCellNum; i < totalCeldas-1; i++)
                //{
                //    string a = "";
                //    if (encabezado.GetCell(i) != null && encabezado.GetCell(i).StringCellValue != "")
                //    {
                //        a = encabezado.GetCell(i).StringCellValue;
                //        Response.Write(a + "\n");
                //    }
                //}
                bool hayValores = false;
                for (int i = 7; i <= totalFilas; i++)
                {
                    IRow fila = sheet.GetRow(i);
                    if (fila != null)
                    {
                        Valores v = new Valores();
                        if (fila.GetCell(0) != null && fila.GetCell(0).NumericCellValue.ToString() != "0")
                        {
                            hayValores = true;
                            v.Valor1 = fila.GetCell(0).NumericCellValue.ToString();
                        }
                        if (fila.GetCell(1) != null && fila.GetCell(1).NumericCellValue.ToString() != "0")
                            v.Valor2 = fila.GetCell(1).NumericCellValue.ToString();
                        if (fila.GetCell(2) != null && fila.GetCell(2).NumericCellValue.ToString() != "0")
                            v.Valor3 = fila.GetCell(2).NumericCellValue.ToString();
                        if (fila.GetCell(3) != null && fila.GetCell(3).NumericCellValue.ToString() != "0")
                            v.Valor4 = fila.GetCell(3).NumericCellValue.ToString();
                        if (fila.GetCell(4) != null && fila.GetCell(4).NumericCellValue.ToString() != "0")
                            v.Valor5 = fila.GetCell(4).NumericCellValue.ToString();
                        if (fila.GetCell(5) != null && fila.GetCell(5).NumericCellValue.ToString() != "0")
                            v.Valor6 = fila.GetCell(5).NumericCellValue.ToString();
                        if (hayValores == true)
                        {
                            hayValores = false;
                            listaValores.Add(v);
                        }
                        
                        //for (int j = fila.FirstCellNum; j < totalFilas; j++)
                        //{
                            
                        //    //if (fila.GetCell(j) != null && fila.GetCell(j).NumericCellValue.ToString()!="0")
                        //    //{
                        //    //    Valores v = new Valores();
                        //    //    string f = fila.GetCell(j).NumericCellValue.ToString();
                        //    //    v.Valor1 = f;
                        //    //}
                        //}
                    }
                    
                }
            }

            return View(listaValores);
        }

    }

    public class Encabezados
    {
        public string Titulo { get; set; }
    }

    public class Valores
    {
        public string Valor1 { get; set; }
        public string Valor2 { get; set; }
        public string Valor3 { get; set; }
        public string Valor4 { get; set; }
        public string Valor5 { get; set; }
        public string Valor6 { get; set; }
    }
}
