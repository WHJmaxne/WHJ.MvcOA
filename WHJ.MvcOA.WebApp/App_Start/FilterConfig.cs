using System.Web;
using System.Web.Mvc;

namespace WHJ.MvcOA.WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new Filters.PermissionValidateAttribute());
        }
    }
}