using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WHJ.MvcOA.WebApp.Areas.UserWF.ProcessClass
{
    /// <summary>
    /// 总经理审批业务类
    /// </summary>
    public class JLShenPi:IProcess
    {

        public bool ProcessBus(object days)
        {
            //如果请假天数小于10 则返回true
            return Convert.ToInt32(days) < 10;
        }
    }
}