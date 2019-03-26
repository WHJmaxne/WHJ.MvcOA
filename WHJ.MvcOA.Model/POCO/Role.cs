using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model
{
   public partial class Role
    {
       public Model.Role ToPOCO(bool isSelf)
       {
           return new Model.Role()
           {
               r_id = this.r_id,
               r_depid = this.r_depid,
               r_name = this.r_name,
               r_remark = this.r_remark,
               r_isshow = this.r_isshow,
               r_isdel = this.r_isdel,
               r_addtime = this.r_addtime,
               Department=this.Department.ToPOCO()
           };
       }
    }
}
