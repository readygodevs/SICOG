using System.Web.Mvc;

namespace TesoreriaVS12.Areas.Tesoreria
{
    public class TesoreriaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Tesoreria";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Tesoreria_default",
                "Tesoreria/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
