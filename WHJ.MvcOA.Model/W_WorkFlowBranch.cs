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
    
    public partial class W_WorkFlowBranch
    {
        public int wfb_Id { get; set; }
        public Nullable<int> wfb_WnParentId { get; set; }
        public bool wfb_Result { get; set; }
        public Nullable<int> wfb_WnId { get; set; }
        public bool wfb_IsDel { get; set; }
        public System.DateTime wfb_Addtime { get; set; }
    
        public virtual W_WorkFlowNode W_WorkFlowNode { get; set; }
        public virtual W_WorkFlowNode W_WorkFlowNode1 { get; set; }
    }
}