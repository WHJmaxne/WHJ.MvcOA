using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WHJ.MvcOA.WebApp
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            ClearViewEnginee();
        }
        /// <summary>
        /// 清理当前网站下 不需要使用视图引擎，并优化视图查找路径
        /// </summary>
        public void ClearViewEnginee()
        {
            
            //移除当前视图网站 默认视图引擎 集合里的第一个 .aspx移除
            ViewEngines.Engines.RemoveAt(0);
            //优化路径
            //获取集合中的 Razor 视图引擎
            var razor = ViewEngines.Engines[0] as RazorViewEngine;
            //指定视图查找的文件后缀
            razor.FileExtensions = new string[1] { "cshtml" };

            //修改区域查找路径
            string[] areaViewLocationFormats = new string[2] {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml"};
            razor.AreaViewLocationFormats = areaViewLocationFormats;
            razor.AreaPartialViewLocationFormats = areaViewLocationFormats;
            razor.AreaMasterLocationFormats = areaViewLocationFormats;

            //2.3.2修改 视图 默认位置 格式
            string[] viewLocationFormats = new string[2] {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"  };
            razor.ViewLocationFormats = viewLocationFormats;
            razor.PartialViewLocationFormats = viewLocationFormats;
            razor.MasterLocationFormats = viewLocationFormats;
        }
    }
}