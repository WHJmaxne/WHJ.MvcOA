//------------------------------------------------------------------------------
// <auto-generated>
// 为实体类 添加 ToPOCO 方法
// </auto-generated>
//------------------------------------------------------------------------------

namespace WHJ.MvcOA.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Category
    {
    	public Model.Category ToPOCO(){
    		return new Model.Category(){
    			c_id=this.c_id,
    			c_type=this.c_type,
    			c_title=this.c_title,
    			parent_id=this.parent_id,
    		};
    	}
    }
}
