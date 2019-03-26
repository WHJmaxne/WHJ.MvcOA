using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHJ.MvcOA.WebApp.Controllers;

namespace WHJ.MvcOA.WebApp.Areas.Sys.Controllers
{
    public class OperationController : BaseController
    {
        //
        // GET: /Sys/Operation/

        public ActionResult LoginOut()
        {
            context.LoginOut();
            return context.JsMsg("退出成功！", "/user/login");
        }

    }
}
