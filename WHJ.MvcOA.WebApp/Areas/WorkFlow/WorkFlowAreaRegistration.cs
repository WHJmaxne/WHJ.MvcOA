using System.Web.Mvc;

namespace WHJ.MvcOA.WebApp.Areas.WorkFlow
{
    public class WorkFlowAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WorkFlow";
            }
        }

        /// <summary>
        /// 注册区域路由
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WorkFlow_default",
                "WorkFlow/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
