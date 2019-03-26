using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model
{
   public partial class W_WorkFlow
    {
       public Model.W_WorkFlow ToPOCO(bool isSelf)
       {
           return new Model.W_WorkFlow()
           {
               wf_Id = this.wf_Id,
               wf_Name = this.wf_Name,
               wf_Remark = this.wf_Remark,
               wf_IsDel = this.wf_IsDel,
               wf_Addtime = this.wf_Addtime,
               //此时的this是EF工作流代理实体，通过其外键导航属性 W_WorkFlowNode 自动查询该工作流节点集合
               W_WorkFlowNode = this.W_WorkFlowNode.Where(n=>n.wn_IsDel==false).OrderBy(n => n.wn_Order).Select(node => node.ToPOCO()).ToList()
           };
       }
    }
}
