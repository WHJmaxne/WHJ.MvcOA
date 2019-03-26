using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.IBLL
{
   public partial interface IW_WorkFlowNodeBLL
    {
       void ReOrderWFNodes(int wfId, int beginOrder);

       void delReOrderWFNodes(int wfId, int delBeginOrder);
    }
}
