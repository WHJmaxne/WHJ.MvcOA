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
    
    public partial class WR_WrokFlowApplyDetails
    {
        public int wrd_Id { get; set; }
        public int wrd_Rid { get; set; }
        public int wrd_NodeId { get; set; }
        public string wrd_NodeName { get; set; }
        public int wrd_UserId { get; set; }
        public Nullable<int> wrd_Operation { get; set; }
        public string wrd_Reason { get; set; }
        public bool wrd_IsDel { get; set; }
        public System.DateTime wrd_Addtime { get; set; }
    
        public virtual UserInfo UserInfo { get; set; }
        public virtual W_WorkFlowNode W_WorkFlowNode { get; set; }
        public virtual WR_WorkFlowApply WR_WorkFlowApply { get; set; }
    }
}
