using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model.ViewModel
{
   public class UserRole
    {
       /// <summary>
       /// 用户id
       /// </summary>
       [Required(ErrorMessage="不能为空！"),RegularExpression("[0-9]+",ErrorMessage="必须是数字")]
       public int UserId;
       /// <summary>
       /// 用户要设置的角色id
       /// </summary>
       [Required(ErrorMessage="不能为空！"),RegularExpression(@"(\d[,]{0,1})+")]
       public string RoleIds; 
    }
}
