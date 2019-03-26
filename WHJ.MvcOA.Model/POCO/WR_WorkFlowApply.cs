using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model
{
    public partial class WR_WorkFlowApply
    {
        public Model.WR_WorkFlowApply ToPOCO(bool isSelf)
        {
            return new Model.WR_WorkFlowApply()
            {
                wr_Id = this.wr_Id,
                wr_UserId = this.wr_UserId,
                wr_WFId = this.wr_WFId,
                wf_CurWFNId = this.wf_CurWFNId,
                wf_Title = this.wf_Title,
                wf_Content = this.wf_Content,
                wf_State = this.wf_State,
                wf_TargetUserId = this.wf_TargetUserId,
                wf_Priority = this.wf_Priority,
                wf_IsDel = this.wf_IsDel,
                wf_Addtime = this.wf_Addtime,
                W_WorkFlow=this.W_WorkFlow.ToPOCO(),
                W_WorkFlowNode=this.W_WorkFlowNode.ToPOCO()
            };
        }
    }
}
