using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHJ.MvcOA.WebApp.Controllers;
using WHJ.MvcOA.Common;
using System.Text.RegularExpressions;

namespace WHJ.MvcOA.WebApp.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {

        #region 1.0 加载用户列表视图
        /// <summary>
        /// 加载用户列表视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        } 
        #endregion

        #region 1.1 查询用户信息列表
        /// <summary>
        /// 查询用户信息列表
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            //接收页码值和页容量
            string pageIndex = Request.Form["page"];
            string pageSize = Request.Form["rows"];
            //创建分页数据模型
            Model.FormatModel.PageData pageData = new Model.FormatModel.PageData()
            {
                pageIndex = int.Parse(pageIndex),
                pageSize = int.Parse(pageSize)
            };
            //查询出用户列表
            context.bll.UserInfoBLL.LoadPageModel(pageData, p => p.u_isdel == false, p => p.user_id, true, "Department");
            //将查询数据转成POCO
            pageData.rows = (pageData.rows as IEnumerable<Model.UserInfo>).Select(p => p.ToPOCO(true)).ToList();
            return Json(pageData);
        } 
        #endregion

        #region 2.0 加载新增用户视图
        /// <summary>
        /// 加载新增用户视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
        {
            
            ViewBag.department = context.bll.DepartmentBLL.LoadEntities(p => true).ToList().Select(p => new SelectListItem()
            {
                Value = p.dep_id.ToString(),
                Text = p.dep_name
            });
            return View();
        } 
        #endregion

        #region 2.1 执行新增用户
        /// <summary>
        /// 执行新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost, Common.Attrs.AjaxRequest]
        public ActionResult Add(Model.ViewModel.UserInfo user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    user.u_add_time = DateTime.Now;
                    user.u_is_lock = false;
                    user.u_isdel = false;
                    context.bll.UserInfoBLL.AddEntity(user.ToPOCO());
                    context.bll.SaveChanges();
                    return context.JsonMsgOK("添加成功！");
                }
                catch
                {
                    return context.JsonMsgErr("操作异常！");
                }
            }
            else
            {
                return context.JsonMsgNoOK("请不要禁用浏览器JS！");
            }
        } 
        #endregion

        #region 3.0 执行用户删除
        /// <summary>
        /// 执行用户删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Common.Attrs.AjaxRequest]
        public ActionResult delete(int id)
        {
            try
            {
                context.bll.UserInfoBLL.EditList(u => u.user_id == id, new string[1] { "u_isdel" }, new object[1] { true });
                context.bll.SaveChanges();
                return context.JsonMsgOK("删除成功！");
            }
            catch
            {
                return context.JsonMsgErr("操作异常！");
            }
        } 
        #endregion

        #region 4.0 加载修改用户视图
        /// <summary>
        /// 加载修改用户视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modify(int id)
        {
            //查询出用户信息
            var uModel = context.bll.UserInfoBLL.LoadEntities(u => u.user_id == id).FirstOrDefault();
            if (uModel == null)
            {
                return context.JsonMsgErr("找不到要修改的对象！");
            }
            //将poco转成viewmodel
            var viewUModel = Model.ViewModel.UserInfo.ToViewModel(uModel);
            //查询出有哪些部门
            ViewBag.department = context.bll.DepartmentBLL.LoadEntities(p => true).ToList().Select(p => new SelectListItem()
            {
                Value = p.dep_id.ToString(),
                Text = p.dep_name
            });
            return View(viewUModel);
        } 
        #endregion

        #region 4.1 执行用户修改
        /// <summary>
        /// 执行用户修改
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost, Common.Attrs.AjaxRequest]
        public ActionResult Modify(Model.ViewModel.UserInfo user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.bll.UserInfoBLL.EditEntity(user.ToPOCO(), "u_name", "u_pwd", "u_depid", "real_name", "u_email", "u_telephone");
                    context.bll.SaveChanges();
                    return context.JsonMsgOK("修改成功！");
                }
                catch
                {
                    return context.JsonMsgErr("操作异常！");
                }
            }
            else
            {
                return context.JsonMsgNoOK("请不要禁用浏览器JS!");
            }
        } 
        #endregion

        #region 5.0 加载 设置用户角色视图
        /// <summary>
        /// 加载 设置用户角色视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SetRole(int id)
        {
            //得到用户id
            ViewBag.uId = id;
            //通过用户id，查询相应的部门有哪些角色
            var roList = context.bll.UserInfoBLL.LoadEntities(u => u.user_id == id).FirstOrDefault().Department.Role;
            ViewBag.depRole = roList;
            return View();
        } 
        #endregion

        #region 5.1 设置用户角色
        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="uRole"></param>
        /// <returns></returns>
        [HttpPost, Common.Attrs.AjaxRequest]
        public ActionResult SetRole(Model.ViewModel.UserRole uRole)
        {
            try
            {
                //接收用户id和新角色id字符串
                string strId = Request.Form["UserId"];
                string roleIdStrs = Request.Form["RoleIds"];
                //将用户id转为int类型
                int id = 0;
                if (!int.TryParse(strId, out id))
                {
                    return context.JsonMsgNoOK("参数格式不正确！");
                }
                //正则表达式验证新角色id字符串
                Regex reg = new Regex(@"(\d[,]{0,1})+");
                if (!reg.IsMatch(roleIdStrs))
                {
                    return context.JsonMsgNoOK("参数格式不正确！");
                }
                //将新角色id字符串转为List<int>集合
                List<int> roleIds = roleIdStrs.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(s => int.Parse(s)).ToList();
                //查询出原来的角色id集合
                var roleOldIds = context.bll.UserRoleBLL.LoadEntities(r => r.ur_uid == id).Select(r => r.ur_rid).ToList();
                //比较两个集合，去掉里面重复的
                for (var i = roleIds.Count - 1; i >= 0; i--)
                {
                    var newId = roleIds[i];
                    if (roleOldIds.Contains(newId))
                    {
                        roleIds.Remove(newId);
                        roleOldIds.Remove(newId);
                    }
                }
                //将新的角色id添加到数据库中
                roleIds.ForEach(newId => context.bll.UserRoleBLL.AddEntity(new Model.UserRole()
                {
                    ur_addtime = DateTime.Now,
                    ur_IsDel = false,
                    ur_uid = id,
                    ur_rid = newId
                }));
                //将不在新角色id中的角色id删除
                context.bll.UserRoleBLL.DeleteList(r => roleOldIds.Contains(r.ur_rid) && r.ur_uid == id);
                context.bll.SaveChanges();
                return context.JsonMsgOK("设置成功！");
            }
            catch
            {
                return context.JsonMsgErr("操作异常！");
            }
        } 
        #endregion
    }
}
