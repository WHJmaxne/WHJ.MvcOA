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
    
    public partial class W_WorkNodeType
    {
        public W_WorkNodeType()
        {
            this.W_WorkFlowNode = new HashSet<W_WorkFlowNode>();
        }
    
        public int wnt_id { get; set; }
        public string wnt_name { get; set; }
    
        public virtual ICollection<W_WorkFlowNode> W_WorkFlowNode { get; set; }
    }
}