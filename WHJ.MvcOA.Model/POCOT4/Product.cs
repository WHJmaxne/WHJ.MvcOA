//------------------------------------------------------------------------------
// <auto-generated>
// 为实体类 添加 ToPOCO 方法
// </auto-generated>
//------------------------------------------------------------------------------

namespace WHJ.MvcOA.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
    	public Model.Product ToPOCO(){
    		return new Model.Product(){
    			p_id=this.p_id,
    			p_photo_no=this.p_photo_no,
    			p_title=this.p_title,
    			category_id=this.category_id,
    			img_url=this.img_url,
    			p_content=this.p_content,
    			sort_id=this.sort_id,
    			click=this.click,
    			add_time=this.add_time,
    			is_lock=this.is_lock,
    		};
    	}
    }
}