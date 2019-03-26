using System.Web.Mvc;

namespace WHJ.MvcOA.WebApp.Areas.UserWF
{
    public class UserWFAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UserWF";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "UserWF_default",
                "UserWF/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
