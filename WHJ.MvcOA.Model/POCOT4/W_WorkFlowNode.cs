//------------------------------------------------------------------------------
// <auto-generated>
// 为实体类 添加 ToPOCO 方法
// </auto-generated>
//------------------------------------------------------------------------------

namespace WHJ.MvcOA.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class W_WorkFlowNode
    {
    	public Model.W_WorkFlowNode ToPOCO(){
    		return new Model.W_WorkFlowNode(){
    			wn_Id=this.wn_Id,
    			wn_WorkFlowID=this.wn_WorkFlowID,
    			wn_Name=this.wn_Name,
    			wn_PrevNode=this.wn_PrevNode,
    			wn_NextNode=this.wn_NextNode,
    			wn_NodeType=this.wn_NodeType,
    			wn_NodeFuncType=this.wn_NodeFuncType,
    			wn_NodeFuncClassName=this.wn_NodeFuncClassName,
    			wn_Order=this.wn_Order,
    			wn_IsDel=this.wn_IsDel,
    			wn_Addtime=this.wn_Addtime,
    		};
    	}
    }
}