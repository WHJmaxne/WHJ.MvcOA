using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model
{
   public partial class W_WorkFlowNode
    {
       public Model.W_WorkFlowNode ToPOCO(bool isSelf)
       {
           return new Model.W_WorkFlowNode()
           {
               wn_Id = this.wn_Id,
               wn_WorkFlowID = this.wn_WorkFlowID,
               wn_Name = this.wn_Name,
               wn_PrevNode = this.wn_PrevNode,
               wn_NextNode = this.wn_NextNode,
               wn_NodeType = this.wn_NodeType,
               wn_NodeFuncType = this.wn_NodeFuncType,
               wn_NodeFuncClassName = this.wn_NodeFuncClassName,
               wn_Order = this.wn_Order,
               wn_IsDel = this.wn_IsDel,
               wn_Addtime = this.wn_Addtime,
               NextNode = this.NextNode.ToPOCO(),
               PrevNode = this.PrevNode.ToPOCO(),
               W_WorkNodeType = this.W_WorkNodeType.ToPOCO(),
               W_WrokFlowRole = this.W_WrokFlowRole.Select(w => w.ToPOCO(true)).ToList()
           };
       }
    }
}
