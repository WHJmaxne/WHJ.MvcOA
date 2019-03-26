using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model
{
   public partial class UserInfo
    {
       public Model.UserInfo ToPOCO(bool isSelf)
       {
           return new Model.UserInfo()
           {
               user_id = this.user_id,
               u_name = this.u_name,
               u_pwd = this.u_pwd,
               real_name = this.real_name,
               u_telephone = this.u_telephone,
               u_email = this.u_email,
               u_is_lock = this.u_is_lock,
               u_add_time = this.u_add_time,
               u_depid = this.u_depid,
               u_isdel = this.u_isdel,
               Department=this.Department.ToPOCO()
           };
       }
    }
}
