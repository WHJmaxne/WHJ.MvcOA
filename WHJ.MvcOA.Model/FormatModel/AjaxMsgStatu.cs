using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model.FormatModel
{
   public enum AjaxMsgStatu
    {     
           OK = 1,
           NoOK = 2,//处理不成功
           Err = 3,//异常
           NoLogin = 4,//没有登录
           NoPer = 5//没有权限     
    }
}
