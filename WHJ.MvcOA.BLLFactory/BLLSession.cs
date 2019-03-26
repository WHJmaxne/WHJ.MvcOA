using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHJ.MvcOA.DALFactory;
using WHJ.MvcOA.IBLL;
using WHJ.MvcOA.IDAL;

namespace WHJ.MvcOA.BLLFactory
{
   public partial class BLLSession:IBLLSession
    {
       /// <summary>
       /// 从线程中获取数据仓储
       /// </summary>
       /// <returns></returns>
        public bool SaveChanges()
        {
            IDBSession dbSession = DBSessionFactory.CreateDbSession();
            return dbSession.SaveChanges();
        }
    }
}
