using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Filters;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.Controllers
{
    public class HomeController : Controller
    {
        [AuthorizeLogin]
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            bool Autenticado = Request.IsAuthenticated;
            String usuario = User.Identity.Name;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Botonera(List<object> ids)
        {
            short p;
            short.TryParse(ids[0].ToString(), out p);
            if (p != 0)
            {
                List<short> list = new List<short>();
                ids.ForEach(item => { list.Add(short.Parse(item.ToString())); });
                return View(new Botonera(list));
            }
            else
            {
                List<string> list = new List<string>();
                ids.ForEach(item => { list.Add(item.ToString()); });
                return View(new Botonera(list));
            }
        }
    }
}
