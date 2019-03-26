using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHJ.MvcOA.Model;
using WHJ.MvcOA.Model.ViewModel;
using WHJ.MvcOA.Common;

namespace WHJ.MvcOA.WebApp.Controllers
{    
    public class UserController : BaseController
    {
        #region 1.0 加载登录视图  Get
        /// <summary>
        /// 1.0 加载登录视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        } 
        #endregion

        #region 2.0 执行登录 Post
        /// <summary>
        /// 执行登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginUser loginUser)
        {
            //判断验证码是否正确
            if (Session["code"] != null && Session["code"].ToString().IsSameStr(loginUser.uVCode))
            {
                Session["code"] = null;

                //查询登录用户
                var userModel =context.bll.UserInfoBLL.Login(loginUser);

                //如果不等于空，则登录成功
                if (userModel != null)
                {
                    //登录成功，加载用户权限，并存入session
                    context.PerSession = context.bll.UserInfoBLL.GetUserPermission(userModel.user_id).Select(u => u.ToPOCO()).ToList();

                    //将当前登录的用户对象存到 session 中
                    context.UserSession = userModel.ToPOCO();

                    //判断是否七天免登陆
                    if (loginUser.uIsCookie)
                    {
                        context.UserIdCookie = userModel.user_id;
                    }
                    //生成返回给Ajax请求的消息

                    return context.JsonMsgOK("登录成功！", "/Admin/Manage/Index");
                }

                return context.JsonMsgErr("用户名或密码错误！");
            }
            else
            {
                return context.JsonMsgNoOK("验证码不正确");
            }
        } 
        #endregion
    }
}
