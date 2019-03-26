using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.IBLL
{
   public partial interface IUserInfoBLL
    {
        #region 登录方法
        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="user">用户填写的数据</param>
        /// <returns></returns>
        Model.UserInfo Login(Model.ViewModel.LoginUser user); 
        #endregion

        #region 根据id获取用户权限
        /// <summary>
        /// 根据id获取用户权限
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        List<Model.Permission> GetUserPermission(int userId); 
        #endregion
    }
}
