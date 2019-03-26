using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model.FormatModel
{
    /// <summary>
    /// Ajax统一消息格式（用来生成统一的json字符串）
    /// </summary>
   public class AjaxMsg
    {
       public int Statu;
       public string Msg;
       public string BackUrl;
       public object Data;
    }

   
}
