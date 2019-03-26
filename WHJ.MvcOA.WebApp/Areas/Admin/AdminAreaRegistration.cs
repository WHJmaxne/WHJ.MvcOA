using System.Web.Mvc;

namespace WHJ.MvcOA.WebApp.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// 指定区域视图位置
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller="Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
