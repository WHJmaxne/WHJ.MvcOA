//------------------------------------------------------------------------------
// <auto-generated>
// 为实体类 添加 ToPOCO 方法
// </auto-generated>
//------------------------------------------------------------------------------

namespace WHJ.MvcOA.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Menus
    {
    	public Model.Menus ToPOCO(){
    		return new Model.Menus(){
    			m_id=this.m_id,
    			m_parent_mid=this.m_parent_mid,
    			m_name=this.m_name,
    			m_url=this.m_url,
    			m_sortid=this.m_sortid,
    			m_status=this.m_status,
    			m_picname=this.m_picname,
    			m_level=this.m_level,
    			m_remark=this.m_remark,
    			m_exp1=this.m_exp1,
    			m_exp2=this.m_exp2,
    			m_add_time=this.m_add_time,
    		};
    	}
    }
}