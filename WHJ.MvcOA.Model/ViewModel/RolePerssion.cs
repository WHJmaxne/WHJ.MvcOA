using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model.ViewModel
{
    /// <summary>
    /// 用户权限设置 视图 实体类
    /// </summary>
   public class RolePerssion
    {
       /// <summary>
       /// 角色id
       /// </summary>
       public int RoleId;

       /// <summary>
       /// 所有权限
       /// </summary>
       public List<Model.Permission> AllPers;

       /// <summary>
       /// 当前角色权限
       /// </summary>
       public List<Model.Permission> UserPers;
    }
}
