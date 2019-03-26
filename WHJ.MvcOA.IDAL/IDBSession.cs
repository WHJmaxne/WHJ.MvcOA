using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.IDAL
{
    /// <summary>
    /// 数据会话层
    /// 1.为了统一各种数据对象的访问方式（以属性方式提供访问），只要拿到他就可以拿到各个数据子接口对象
    /// 2.统一数据的保存方式
    /// </summary>
   public partial interface IDBSession
    {
       /// <summary>
       /// 统一为处理 当前浏览器请求的 线程执行过程中 所有的 通过EF容器，提交数据的操作，更新到数据库
       /// </summary>
       /// <returns></returns>
       bool SaveChanges();
    }
}
