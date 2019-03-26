using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.DI
{
   public class SpringHelper
    {
       //1.获取Spring容器（自动读取配置文件）
       public static IApplicationContext GetSpringContains()
       {
           //读取Spring配置文件
           return ContextRegistry.GetContext();
       }

       //2.根据键获取配置好的 对象
       public static T GetObject<T>(string objId)
       {
           return (T)GetSpringContains().GetObject(objId);
       }
    }
}
