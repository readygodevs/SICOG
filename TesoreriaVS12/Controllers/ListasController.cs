using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.DAL;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.Controllers
{
    public class ListasController : Controller
    {
        protected PerfilesDAL perfles { get; set; }
        public ListasController()
        {
            if (perfles == null) perfles = new PerfilesDAL();
        }
        
        [HttpPost]
        public JsonResult ListaPerfiles(Int32? Selected)
        {
            if(Selected.HasValue)
                return new JsonResult { Data = new SelectList(perfles.Get(), "IdPerfil", "Descripcion",Selected) };
            return new JsonResult { Data = new SelectList(perfles.Get(), "IdPerfil", "Descripcion") }; 
        }

        public static List<VW_Bases> ListaBases()
        {
            BdTesoreriasDAL bases = new BdTesoreriasDAL();
            return bases.Get().ToList();
        }

        //public static SelectList documentosValidos()
        //{
        //    RESTConsume RestService = new RESTConsume();
        //    Modelos.Apostilla<Modelos.documentos> doctos = RestService.getDocuments();
        //    return new SelectList(doctos.response, "tr_nombre", "do_nombre");
        //}

    }
}
