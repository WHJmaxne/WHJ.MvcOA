using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WHJ.MvcOA.Common
{
   public class DataHelper
    {
       public static JavaScriptSerializer jss = new JavaScriptSerializer();

       #region 将object对象转成 json字符串
       /// <summary>
       /// 将object对象转成 json字符串
       /// </summary>
       /// <param name="obj"></param>
       /// <returns></returns>
       public static string ToJson(object obj)
       {
           return jss.Serialize(obj);
       } 
       #endregion

    }
}
