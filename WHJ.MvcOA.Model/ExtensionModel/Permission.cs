using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model
{
   public partial class Permission
    {
       /// <summary>
       /// 将权限转成 easyUI Tree节点
       /// </summary>
       /// <returns></returns>
       public EasyUIModel.TreeNode ToNode()
       {
           return new EasyUIModel.TreeNode()
           {
               id=this.p_id,
               text=this.p_name,
               Checked=false,
               attributes = new { href="/"+this.p_areaname+"/"+this.p_controllername+"/"+this.p_actionname}
           };
       }
    }
}
