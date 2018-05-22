using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TesoreriaVS12.Filters
{
    public class PermisosFilter : ActionFilterAttribute, IActionFilter
    {
        public PermisosFilter() { }
        public string Permiso { get; set; }
        public string Proceso { get; set; }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var x = Permiso;
            var y = Proceso;
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var x = Permiso;
            var y = Proceso;
            throw new NotImplementedException();
        }
    }
}