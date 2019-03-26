using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHJ.MvcOA.Common;
using WHJ.MvcOA.Model.FormatModel;

namespace WHJ.MvcOA.WebApp.Filters
{
    /// <summary>
    /// 权限过滤器
    /// </summary>
    public class PermissionValidateAttribute:AuthorizeAttribute
    {

        #region 1.0 权限过滤
        /// <summary>
        /// 权限过滤
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        
        {
            //1.创建要验证的区域名单
            List<string> blackList = new List<string>() { "workflow", "admin", "userwf", "sys" };

            //2.验证当前请求的 url区域名是否在 名单中
            //2.1判断当前请求的url中是否包含区域
            if (filterContext.RouteData.DataTokens.ContainsKey("area"))
            {
                //2.1获取区域名
                string strAreaName = filterContext.RouteData.DataTokens["area"].ToString();
                //3.如果区域名不为空，并且在黑名单中，就要验证权限
                if (!strAreaName.IsNullOrEmpty())
                {
                    //判断当前区域是否在黑名单中
                    if (!blackList.Contains(strAreaName,StringComparer.CurrentCultureIgnoreCase))
                    {
                        ProcessResult(filterContext, Model.FormatModel.AjaxMsgStatu.NoPer, "您没有访问权限！", "/user/login");
                    }
                    //判断当前 被请求 的 方法 是否有贴 跳过登录验证标签
                    //如果没有贴 则验证 登录和权限
                    if (!DoseSticAttr<Common.Attrs.SkipLoginAttribute>(filterContext.ActionDescriptor))
                    {
                        //1.验证是否 登录（验证是否有session 或者cookie）
                        if (IsLogin())
                        {
                            //2. 验证 登录用户是否有访问页面的权限
                            //2.1获取当前url中的各种参数
                            //获取控制器名
                            string strController = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                            //获取方法名
                            string strAction = filterContext.ActionDescriptor.ActionName;
                            //获取请求方式
                            string strMethod = filterContext.HttpContext.Request.HttpMethod;

                            //验证被请求的方法和类上 是否贴有 跳过权限验证 标签
                            //如果没有贴 则验证权限
                            if (!DoseSticAttr<Common.Attrs.SkipPermissionAttribute>(filterContext.ActionDescriptor))
                            {
                                //检查权限，如果登录用户没有权限，则返回错误消息
                                if (!HasPermission(strAreaName, strController, strAction, strMethod))
                                {
                                    //返回没有权限消息
                                    //filterContext.Result = context.JsonMsg(Model.FormatModel.AjaxMsgStatu.NoPer, "您没有访问权限！", "/user/login");
                                    ProcessResult(filterContext, Model.FormatModel.AjaxMsgStatu.NoPer, "您没有访问权限！", "/user/login");
                                }
                            }
                        }
                        else
                        {
                            //返回没有登录消息
                            //filterContext.Result = context.JsonMsg(Model.FormatModel.AjaxMsgStatu.NoPer, "您没有登录！", "/user/login");
                            ProcessResult(filterContext, Model.FormatModel.AjaxMsgStatu.NoPer, "您没有登录！", "/user/login");
                        }
                    }
                }
            }
        } 
        #endregion

        #region 2.0 验证是否登录(自动登录)
        /// <summary>
        /// 验证是否登录(自动登录)
        /// </summary>
        /// <returns></returns>
        private bool IsLogin()
        {
            bool isLogin = true;
            //1.是否有session
            //1.1如果session中没有登录对象
            if (context.UserSession == null)
            {
                //2.验证cookie是否存在
                if (context.UserIdCookie < 0)
                {
                    //2.1返回没有登录的错误消息
                    context.JsonMsgErr("登录信息已过期，请重新登录", "/User/Login");
                    isLogin = false;
                }
                //2.2如果id大于0 说明登录有效
                else
                {
                    //2.3查询登录用户
                    var userModel = context.bll.UserInfoBLL.LoadEntities(u => u.user_id == context.UserIdCookie).FirstOrDefault();
                    //2.4如果不等于空，则登录成功
                    if (userModel != null)
                    {
                        //2.5登录成功，加载用户权限，并存入session
                        context.PerSession = context.bll.UserInfoBLL.GetUserPermission(userModel.user_id).Select(u => u.ToPOCO()).ToList();
                        //2.6将当前登录的用户对象存到 session 中
                        context.UserSession = userModel.ToPOCO();
                    }
                    else
                    {
                        isLogin = false;
                    }
                }
            }
            return isLogin;
        } 
        #endregion

        #region 3.0 检查当前url是否在用户登录权限中
        /// <summary>
        /// 检查当前url是否在用户登录权限中
        /// </summary>
        /// <param name="strArea"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private bool HasPermission(string strArea, string controller, string action, string method)
        {
            var perList = context.PerSession;
            //var perNow = perList.Find(p => p.p_areaname.IsSameStr(strArea) 
            //    && p.p_controllername.IsSameStr(controller) 
            //    && p.p_actionname.IsSameStr(action) 
            //    && method.IsSameStr("get") ? (p.p_formmethod == 1 || p.p_formmethod == 3) : (p.p_formmethod == 2 || p.p_formmethod == 3)
            //     );
            var perNow = perList.Find(delegate(Model.Permission per) { return per.p_areaname.IsSameStr(strArea) && per.p_controllername.IsSameStr(controller) && per.p_actionname.IsSameStr(action) && (method.IsSameStr("get") ? (per.p_formmethod == 1 || per.p_formmethod == 3) : (per.p_formmethod == 2 || per.p_formmethod == 3)); });
            if (perNow != null)
            {
                return true;
            }
            return false;
        } 
        #endregion

        #region 4.0 根据方法被请求的方式，返回不同的 js 代码（json/js）
        /// <summary>
        /// 根据方法被请求的方式，返回不同的 js 代码（json/js）
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="statu"></param>
        /// <param name="msg"></param>
        /// <param name="backUrl"></param>
        /// <param name="date"></param>
        private void ProcessResult(AuthorizationContext filterContext, AjaxMsgStatu statu = AjaxMsgStatu.OK, string msg = "", string backUrl = "", object date = null)
        {
            //如果被请求的方法或类 贴了AjaxRequest 标签特性，则代表浏览器是 Ajax请求，返回json字符串
            if (DoseSticAttr<Common.Attrs.AjaxRequestAttribute>(filterContext.ActionDescriptor))
            {
                filterContext.Result = context.JsonMsg(statu, msg, backUrl);
            }
            //如果没有贴 AjaxResult 标签，则代表是浏览器直接请求（跳转），返回 js 代码
            else
            {
                filterContext.Result = context.JsMsg(msg, backUrl);
            }
        } 
        #endregion

        #region 5.0 判断方法和类 是否有贴指定的标记
        /// <summary>
        /// 判断方法和类 是否有贴指定的标记
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        private bool DoseSticAttr<T>(ActionDescriptor action)
        {
            Type t = typeof(T);
            return action.IsDefined(t, false) || action.ControllerDescriptor.IsDefined(t, false);
        } 
        #endregion
        public OperationContext context
        {
            get
            {
                return OperationContext.Current;
            }
        }        
    }
}