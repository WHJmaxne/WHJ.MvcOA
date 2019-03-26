using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHJ.MvcOA.Common;

namespace WHJ.MvcOA.BLL
{
    public partial class UserInfoBLL
    {
        #region 1.0 登录验证 + Login()
        /// <summary>
        /// 登陆方法
        /// </summary>
        /// <param name="user">用户填写的数据</param>
        /// <returns></returns>
        public Model.UserInfo Login(Model.ViewModel.LoginUser user)
        {
            var model = this.CreateDbSession.UserInfoDAL.LoadEntities(u => u.u_name == user.uLoginName).FirstOrDefault();
            if (model != null && model.u_pwd == user.uPwd.MD5())
            {
                return model;
            }
            return null;
        }
        #endregion

        #region 2.0 登录成功后查询用户权限
        /// <summary>
        /// 登录成功后查询用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Model.Permission> GetUserPermission(int userId)
        {
            //一、查询 用户 角色 权限
            //1.根据用户id查询用户角色id
            var roleIds = this.CreateDbSession.UserRoleDAL.LoadEntities(u => u.ur_uid == userId).Select(u => u.ur_rid).ToList();

            //2.根据 角色id们 查询 权限id们
            var perIds = this.CreateDbSession.RolePermissionDAL.LoadEntities(u => roleIds.Contains(u.rp_rid)).Select(u => u.rp_pid);

            //3.查询权限id查询权限集合
            var userPers = this.CreateDbSession.PermissionDAL.LoadEntities(u => perIds.Contains(u.p_id)).ToList();

            //二、查询 用户 特权
            //1.查询特权id
            var vipIds = this.CreateDbSession.UserVipPermissionDAL.LoadEntities(u => u.vip_userid == userId).Select(u => u.vip_permission).ToList();
            //2.查询特权集合
            var vipPers = this.CreateDbSession.PermissionDAL.LoadEntities(u => vipIds.Contains(u.p_id)).ToList();

            //三、合并角色和特权权限
            vipPers.ForEach(u =>
            {
               //如果特权 id 不在 角色权限中，则将 该条特权id加入到 角色权限集合中
               if (!perIds.Contains(u.p_id))
                {
                    userPers.Add(u);
                }
            });
            //四、返回 合并后的 权限集合
            return userPers;
        }
        #endregion
    }
}
