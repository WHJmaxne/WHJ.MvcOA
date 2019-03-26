using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using WHJ.MvcOA.IDAL;

namespace WHJ.MvcOA.DALFactory
{
   public class DBSessionFactory
    {
       public static IDBSession CreateDbSession()
       {          
           IDBSession dbSession = (IDBSession)CallContext.GetData("dbSession");
           if (dbSession==null)
           {
               dbSession = DI.SpringHelper.GetObject<IDBSession>("dbSession");
               CallContext.SetData("dbSession", dbSession);
           }
           return dbSession;
       }
    }
}
