using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHJ.MvcOA.DAL;
using WHJ.MvcOA.IDAL;

namespace WHJ.MvcOA.DALFactory
{
   public partial class DBSession:IDBSession
    {
       DbContext db = DbContextFactory.CreateDbContext();
        public bool SaveChanges()
        {
            return db.SaveChanges() > 0;            
        }

    }
}
