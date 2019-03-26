using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHJ.MvcOA.BLLFactory;
using WHJ.MvcOA.IBLL;
using WHJ.MvcOA.Model;

namespace WHJ.MvcOA.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var list =context.bll.UserInfoBLL.LoadEntities(u => true).ToList();
            return View(list);          
        }

        public ActionResult list()
        {
            return View();
        }
    }
}
