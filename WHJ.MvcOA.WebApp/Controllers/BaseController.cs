using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WHJ.MvcOA.WebApp.Controllers
{
    /// <summary>
    /// 控制器父类
    /// </summary>
    public class BaseController : Controller
    {
        #region 1.0 便捷操作
        public OperationContext context
        {
            get
            {
                return OperationContext.Current;
            }
        } 
        #endregion
    }
   
}
