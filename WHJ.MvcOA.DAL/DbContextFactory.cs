using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.DAL
{
   public class DbContextFactory
    {
       public static DbContext CreateDbContext()
       {
           DbContext dbContext = (DbContext)CallContext.GetData("dbContext");
           if (dbContext==null)
           {
               dbContext = new kyprintMVCEntities();
               CallContext.SetData("dbContext", dbContext);
           }
           return dbContext;
       }
    }
}
