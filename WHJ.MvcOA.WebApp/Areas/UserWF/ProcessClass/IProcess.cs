using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WHJ.MvcOA.WebApp.Areas.UserWF.ProcessClass
{
    public interface IProcess
    {
        bool ProcessBus(object days);
    }
}