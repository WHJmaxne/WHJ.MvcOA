//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WHJ.MvcOA.Model
{
    using System;
    using System.Collections.Generic;

    public partial class W_WorkFlow
    {
        public W_WorkFlow()
        {
            this.W_WorkFlowNode = new HashSet<W_WorkFlowNode>();
            this.WR_WorkFlowApply = new HashSet<WR_WorkFlowApply>();
        }

        public int wf_Id { get; set; }
        public string wf_Name { get; set; }
        public string wf_Remark { get; set; }
        public bool wf_IsDel { get; set; }
        public System.DateTime wf_Addtime { get; set; }

        public virtual ICollection<W_WorkFlowNode> W_WorkFlowNode { get; set; }
        public virtual ICollection<WR_WorkFlowApply> WR_WorkFlowApply { get; set; }
    }
}