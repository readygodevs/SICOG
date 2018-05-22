using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Models;
using HiQPdf;

namespace TesoreriaVS12.Models
{
    public class ConvertHtmlToString
    {

        public String TituloSistema { get; set; }
        public String PiePagina { get; set; }
        public String NombreCompleto { get; set; }
        
        public string RenderRazorViewToString(string viewName, object model, ControllerContext ctx)
        {
            ctx.Controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ctx, viewName);
                var viewContext = new ViewContext(ctx, viewResult.View, ctx.Controller.ViewData, ctx.Controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ctx, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public byte[] GenerarPDF(string viewName, object model, ControllerContext ctx)
        {
            String View = RenderRazorViewToString(viewName, model, ctx);
            HiQPdf.HtmlToPdf docpdf = new HtmlToPdf();

            docpdf.Document.Margins.Bottom = 20;
            docpdf.Document.Margins.Top = 20;
            docpdf.Document.Margins.Left = 20;
            docpdf.Document.Margins.Right = 20;
            docpdf.SerialNumber = "s/va4uPX-1f/a0cHS-wcqFnYOT-gpOHk4qL-h5OAgp2C-gZ2KioqK";
            SetFooter(docpdf.Document);
            byte[] archivo = docpdf.ConvertHtmlToPdfDocument(View, "").WriteToMemory();
            return archivo;
        }

        public byte[] GenerarPDF_Blanco(string viewName, object model, ControllerContext ctx)
        {
            String View = RenderRazorViewToString(viewName, model, ctx);
            HiQPdf.HtmlToPdf docpdf = new HtmlToPdf();

            docpdf.Document.Margins.Bottom = 0;
            docpdf.Document.Margins.Top = 0;
            docpdf.Document.Margins.Left = 0;
            docpdf.Document.Margins.Right = 0;
            docpdf.SerialNumber = "s/va4uPX-1f/a0cHS-wcqFnYOT-gpOHk4qL-h5OAgp2C-gZ2KioqK";
            byte[] archivo = docpdf.ConvertHtmlToPdfDocument(View, "").WriteToMemory();
            return archivo;
        }

        public byte[] GenerarPDF_ChequePoliza(string viewName, object model, ControllerContext ctx)
        {
            String View = RenderRazorViewToString(viewName, model, ctx);
            HiQPdf.HtmlToPdf docpdf = new HtmlToPdf();

            docpdf.Document.Margins.Bottom = 20;
            docpdf.Document.Margins.Top = 20;
            docpdf.Document.Margins.Left = 0;
            docpdf.Document.Margins.Right = 0;
            docpdf.SerialNumber = "s/va4uPX-1f/a0cHS-wcqFnYOT-gpOHk4qL-h5OAgp2C-gZ2KioqK";
            byte[] archivo = docpdf.ConvertHtmlToPdfDocument(View, "").WriteToMemory();
            return archivo;
        }

        public byte[] GenerarPDF_Horizontal(string viewName, object model, ControllerContext ctx)
        {
            String View = RenderRazorViewToString(viewName, model, ctx);
            HiQPdf.HtmlToPdf docpdf = new HtmlToPdf();

            docpdf.Document.Margins.Bottom = 20;
            docpdf.Document.Margins.Top = 20;
            docpdf.Document.Margins.Left = 20;
            docpdf.Document.Margins.Right = 20;
            docpdf.Document.PageOrientation = PdfPageOrientation.Landscape;
            docpdf.SerialNumber = "s/va4uPX-1f/a0cHS-wcqFnYOT-gpOHk4qL-h5OAgp2C-gZ2KioqK";
            SetFooter(docpdf.Document);
            byte[] archivo = docpdf.ConvertHtmlToPdfDocument(View, "").WriteToMemory();
            return archivo;
        }

        public byte[] GenerarPDF2()
        {
            string line;
            PdfDocument pdf = new PdfDocument();
            // create the true type fonts that can be used in document
            Font ttfFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
            PdfFont newTimesFont = pdf.CreateFont(ttfFont);
            PdfFont newTimesFontEmbed = pdf.CreateFont(ttfFont, true);

            pdf.SerialNumber = "s/va4uPX-1f/a0cHS-wcqFnYOT-gpOHk4qL-h5OAgp2C-gZ2KioqK";
            var doc = @"C:\Users\ICSIC\Documents\DoctosTrabajo\PruebasRTF\rtfwordpadgenerado.rtf";
            System.IO.StreamReader file = new System.IO.StreamReader(doc);
            // create page 1
            PdfPage page1 = pdf.AddPage();
            /*Escribir en el pdf*/
            while ((line = file.ReadLine()) != null)
            {
                PdfText text1 = new PdfText(10, 10, line, newTimesFontEmbed);
                // layout the text
                page1.Layout(text1);
            }

            file.Dispose();
            file.Close();
            byte[] archivo = pdf.WriteToMemory();            
            return archivo;
        }

        private void SetFooter(PdfDocumentControl htmlToPdfDocument)
        {
            // enable footer display
            htmlToPdfDocument.Footer.Enabled = true;

            // set footer height
            htmlToPdfDocument.Footer.Height = 15;
            // set footer background color
            htmlToPdfDocument.Footer.BackgroundColor = System.Drawing.Color.WhiteSmoke;

            float pdfPageWidth = htmlToPdfDocument.PageOrientation == PdfPageOrientation.Portrait ?
                    htmlToPdfDocument.PageSize.Width : htmlToPdfDocument.PageSize.Height;

            float footerWidth = pdfPageWidth - htmlToPdfDocument.Margins.Left -
                        htmlToPdfDocument.Margins.Right;
            float footerHeight = htmlToPdfDocument.Footer.Height;

            // layout HTML in footer
            if (String.IsNullOrEmpty(TituloSistema)) TituloSistema = "";
            if (String.IsNullOrEmpty(PiePagina)) PiePagina = "{0} {1} {2}";
            if (String.IsNullOrEmpty(NombreCompleto)) NombreCompleto = "";
            PdfHtml footerHtml = new PdfHtml(5, 0,
                    String.Format(PiePagina, TituloSistema, NombreCompleto, DateTime.Now), null);
            footerHtml.FitDestHeight = true;
            htmlToPdfDocument.Footer.Layout(footerHtml);

            // add page numbering
            System.Drawing.Font pageNumberingFont =
                            new System.Drawing.Font(
                            new System.Drawing.FontFamily("Times New Roman"),
                            7, System.Drawing.GraphicsUnit.Point);
            PdfText pageNumberingText = new PdfText(footerWidth - 100, 3,
                            "Página {CrtPage} de {PageCount}", pageNumberingFont);
            pageNumberingText.HorizontalAlign = PdfTextHAlign.Center;
            pageNumberingText.EmbedSystemFont = true;
            pageNumberingText.ForeColor = System.Drawing.Color.Gray;
            htmlToPdfDocument.Footer.Layout(pageNumberingText);
        }

    }
}