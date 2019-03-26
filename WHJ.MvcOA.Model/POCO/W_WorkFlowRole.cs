using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model
{
    public partial class W_WrokFlowRole
    {
        public Model.W_WrokFlowRole ToPOCO(bool isSelf)
        {
            return new Model.W_WrokFlowRole()
            {
                wfr_Id = this.wfr_Id,
                wfr_WFNode = this.wfr_WFNode,
                wfr_Role = this.wfr_Role,
                Role = this.Role.ToPOCO()
            };
        }
    }
}
