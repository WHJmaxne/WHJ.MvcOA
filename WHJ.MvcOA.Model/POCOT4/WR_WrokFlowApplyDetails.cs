//------------------------------------------------------------------------------
// <auto-generated>
// 为实体类 添加 ToPOCO 方法
// </auto-generated>
//------------------------------------------------------------------------------

namespace WHJ.MvcOA.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class WR_WrokFlowApplyDetails
    {
    	public Model.WR_WrokFlowApplyDetails ToPOCO(){
    		return new Model.WR_WrokFlowApplyDetails(){
    			wrd_Id=this.wrd_Id,
    			wrd_Rid=this.wrd_Rid,
    			wrd_NodeId=this.wrd_NodeId,
    			wrd_NodeName=this.wrd_NodeName,
    			wrd_UserId=this.wrd_UserId,
    			wrd_Operation=this.wrd_Operation,
    			wrd_Reason=this.wrd_Reason,
    			wrd_IsDel=this.wrd_IsDel,
    			wrd_Addtime=this.wrd_Addtime,
    		};
    	}
    }
}