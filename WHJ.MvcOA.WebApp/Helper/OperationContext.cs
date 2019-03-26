using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using WHJ.MvcOA.Common;
using WHJ.MvcOA.Model.EasyUIModel;
using WHJ.MvcOA.Model.FormatModel;

namespace WHJ.MvcOA.WebApp
{
    public class OperationContext
    {
        #region 0.0 常量区
        //存入session的键
        const string perSessionKey = "perSession";
        const string userSessionKey = "userSession";
        const string userIdCookieKey = "userIdCookie";
        const string userPerTreeSessionKey = "treeJson";
        #endregion

        #region 1.0 操作上下文属性
        //1.----便捷访问属性 HttpContext/Session/Response/Request--------
        public HttpContext context
        {
            get
            {
                return HttpContext.Current;
            }
        }

        public HttpSessionState session
        {
            get
            {
                return context.Session;
            }
        }

        public HttpResponse Response
        {
            get
            {
                return context.Response;
            }
        }

        public HttpRequest Request
        {
            get
            {
                return context.Request;
            }
        }

        
        #endregion

        #region 2.0 当前登录的用户权限集合存入 session
        /// <summary>
        /// 当前登录的用户权限集合存入 session
        /// </summary>
        public List<Model.Permission> PerSession
        {
            set
            {
                //1.将用户权限存入session
                session[perSessionKey] = value;
                //2.根据 权限集合 里的菜单类型权限，生成权限树 节点集合，并转成json字符串 存入session
                //2.1 找出 菜单树 类型权限
                var listTreePer = value.FindAll(p => p.p_operationtype == 1);
                //2.2 排序
                listTreePer.Sort((x, y) => x.p_order - y.p_order);
                //2.3 将权限转成树节点集合
               List<TreeNode> listNodes= Per2Node(listTreePer,0);
                //3 将树节点集合转成json字符串
               string strTreeNodeJsons = DataHelper.ToJson(listNodes);
               UserPerTreeJsonStr = strTreeNodeJsons;
            }
            get
            {
                return session[perSessionKey] as List<Model.Permission>;
            }
        }
        #endregion

        #region 2.1 将权限 转成 树节点集合
        /// <summary>
        /// 将权限 转成 树节点集合
        /// </summary>
        /// <param name="listPer"></param>
        /// <returns></returns>
        public List<TreeNode> Per2Node(List<Model.Permission> listPer, int parentId)
        {
            //创建节点集合
            List<TreeNode> listNodes = new List<TreeNode>();
            //循环 权限集合
            foreach (var per in listPer)
            {
                //找出 父id为 parentId 的权限
                if (per.p_parent == parentId)
                {
                    //将权限转为 树节点
                    var node = per.ToNode();
                    //将节点加入节点集合
                    listNodes.Add(node);
                    //递归 查找子节点
                    node.children = Per2Node(listPer, node.id);

                }
            }
            //返回节点集合
            return listNodes;
        } 
        #endregion

        #region 3.0 当前登录对象存入 session
        /// <summary>
        /// 将当前登录对象存入 session
        /// </summary>
        public Model.UserInfo UserSession
        {
            set
            {
                session[userSessionKey] = value;
            }
            get
            {
                return session[userSessionKey] as Model.UserInfo;
            }
        } 
        #endregion

        #region 4.0 将当前登录id存入cookie
        /// <summary>
        /// 将当前登录id存入cookie
        /// </summary>
        public int UserIdCookie
        {
            set
            {               
                HttpCookie cookie = new HttpCookie(userIdCookieKey, value.ToString());
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(cookie);
            }
            get
            {
                HttpCookie cookie = Request.Cookies[userIdCookieKey];
                if (cookie!=null&&!cookie.Value.IsNullOrEmpty())
                {
                    return Convert.ToInt32(cookie.Value);
                }
                return -1;
            }
        } 
        #endregion

        #region 5.0 生成ajax消息json字符串
        /// <summary>
        /// 生成ajax消息 json字符串
        /// </summary>
        /// <param name="statu"></param>
        /// <param name="msg"></param>
        /// <param name="backUrl"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ActionResult JsonMsg(AjaxMsgStatu statu=AjaxMsgStatu.OK,string msg="",string backUrl="",object date=null)
        {
            AjaxMsg ajaxMsg = new AjaxMsg()
            {
                Statu=Convert.ToInt32(statu),
                Msg=msg,
                BackUrl=backUrl,
                Data=date
            };
            JsonResult jr = new JsonResult()
            {                
                Data=ajaxMsg,
                JsonRequestBehavior=JsonRequestBehavior.AllowGet//设置为允许 Get请求执行
            };
            return jr;
        }
        #endregion

        #region 5.1 生成ajax消息json字符串便捷方法
        public ActionResult JsonMsgOK(string msg = "", string backUrl = "", object date = null)
        {
            return JsonMsg(AjaxMsgStatu.OK, msg, backUrl, date);
        }
        public ActionResult JsonMsgErr(string msg = "", string backUrl = "", object data = null)
        {
            return JsonMsg(AjaxMsgStatu.Err, msg, backUrl, data);
        }

        public ActionResult JsonMsgNoOK(string msg = "", string backUrl = "", object data = null)
        {
            return JsonMsg(AjaxMsgStatu.NoOK, msg, backUrl, data);
        }
        
        #endregion

        #region 6.0 返回 alert location js标签
        /// <summary>
        /// 返回 alert location js标签
        /// </summary>
        /// <param name="strMsg"></param>
        /// <param name="backUrl"></param>
        /// <returns></returns>
        public ContentResult JsMsg(string strMsg, string backUrl = "")
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("<script>alert('" + strMsg + "');");
            if (!backUrl.IsNullOrEmpty())
            {
                sb.Append("if(window.top)");
                sb.Append("   window.top.location='" + backUrl + "';");
                sb.Append("else");
                sb.Append("   window.location='" + backUrl + "';");
            }
            sb.Append("</script>");
            return new ContentResult()
            {
                Content = sb.ToString(),
                ContentType = "text/html"
            };
        } 
        #endregion

        #region 7.0 获取BLLSession层实例
        /// <summary>
        /// 获取bll层实例
        /// </summary>
        public IBLL.IBLLSession bll
        {
            get
            {
                return BLLFactory.BLLSessionFactory.CreateBllSession();
            }
        } 
        #endregion

        #region 8.0 当前 登录用户的 权限树json字符串
        /// <summary>
        /// 当前 登录用户的 权限树json字符串
        /// </summary>
        public string UserPerTreeJsonStr
        {
            set
            {
                session[userPerTreeSessionKey] = value;
            }
            get
            {
                return session[userPerTreeSessionKey].ToString();
            }
        }
        #endregion

        #region 9.0 退出系统
        /// <summary>
        /// 退出系统
        /// </summary>
        public void LoginOut()
        {
            //清空当前登录用户的session
            session.Abandon();
            //删除当前登录用的的cookie
            if (Request.Cookies[userIdCookieKey] != null)
            {
                HttpCookie cookie = new HttpCookie(userIdCookieKey, "");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
        } 
        #endregion

        #region end 操作以上属性
        /// <summary>
        /// 操作以上属性
        /// </summary>
        public static OperationContext Current
        {
            get
            {
                return new OperationContext();
            }
        }  
        #endregion

    }
}