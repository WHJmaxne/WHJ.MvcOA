using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using WHJ.MvcOA.IBLL;

namespace WHJ.MvcOA.BLLFactory
{
   public class BLLSessionFactory
    {
       public static IBLLSession CreateBllSession()
       {
           IBLLSession bllSession = (IBLLSession)CallContext.GetData("bllSession");
           if (bllSession == null)
           {
               bllSession = DI.SpringHelper.GetObject<IBLLSession>("bllSession");
               CallContext.SetData("bllSession", bllSession);
           }
           return bllSession;
       }
    }
}
