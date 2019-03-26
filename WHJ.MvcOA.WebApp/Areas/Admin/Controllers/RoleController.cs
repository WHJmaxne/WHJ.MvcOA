using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHJ.MvcOA.WebApp.Controllers;

namespace WHJ.MvcOA.WebApp.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {

        #region 1.0 加载 角色列表视图
        /// <summary>
        /// 加载 角色列表视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        } 
        #endregion

        #region 1.1 查询角色数据
        /// <summary>
        /// 查询角色数据
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            //接收页码和页容量pageIndex=1，pageSize=10
            string pageIndex = Request.Form["page"];
            string pageSize = Request.Form["rows"];
            //创建分页数据模型
            Model.FormatModel.PageData pageData = new Model.FormatModel.PageData()
            {
                pageIndex = int.Parse(pageIndex),
                pageSize = int.Parse(pageSize)
            };
            //查询数据(联查)
            context.bll.RoleBLL.LoadPageModel(pageData, p => p.r_isdel == false, p => p.r_id, true, "Department");
            //将EF对象转成POCO
            pageData.rows = (pageData.rows as IEnumerable<Model.Role>).Select(p => p.ToPOCO(true)).ToList();
            return Json(pageData);
        } 
        #endregion

        #region 2.0 加载新增视图
        /// <summary>
        /// 加载新增视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.depList = context.bll.DepartmentBLL.LoadEntities(p => true).ToList().Select(p => new SelectListItem()
            {
                Value = p.dep_id.ToString(),
                Text = p.dep_name
            });
            return View();
        } 
        #endregion

        #region 2.1 执行新增
        /// <summary>
        /// 执行新增
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(Model.ViewModel.Role role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    role.r_addtime = DateTime.Now;
                    role.r_isdel = false;
                    role.r_isshow = true;
                    context.bll.RoleBLL.AddEntity(role.ToPOCO());
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
                return context.JsonMsgNoOK("请不要关闭浏览器JS!");
            }
        } 
        #endregion

        #region 3.0 加载修改角色视图
        /// <summary>
        /// 加载修改角色视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modify(int id)
        {
            //查询部门
            ViewBag.depList = context.bll.DepartmentBLL.LoadEntities(p => true).ToList().Select(p => new SelectListItem()
            {
                Value = p.dep_id.ToString(),
                Text = p.dep_name
            });
            //查询要修改的实体
            var roModel = context.bll.RoleBLL.LoadEntities(p => p.r_id == id).FirstOrDefault();
            if (roModel == null)
            {
                return context.JsonMsgErr("找不到要修改的对象!");
            }
            //将EF对象转成ViewModel对象
            var roViewModel = Model.ViewModel.Role.ToViewModel(roModel);
            return View(roViewModel);
        } 
        #endregion

        #region 3.1 执行修改
        /// <summary>
        /// 执行修改
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost, Common.Attrs.AjaxRequest]
        public ActionResult Modify(Model.ViewModel.Role role)
        {
            try
            {
                context.bll.RoleBLL.EditEntity(role.ToPOCO(), "r_name", "r_depid", "r_remark");
                context.bll.SaveChanges();
                return context.JsonMsgOK("修改成功！");
            }
            catch
            {
                return context.JsonMsgErr("操作异常！");
            }
        } 
        #endregion

        #region 4.0 执行删除
        /// <summary>
        /// 执行删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Common.Attrs.AjaxRequest]
        public ActionResult delete(int id)
        {
            try
            {
                context.bll.RoleBLL.EditList(p => p.r_id == id, new string[1] { "r_isdel" }, new object[1] { true });
                context.bll.SaveChanges();
                return context.JsonMsgOK("删除成功！");
            }
            catch
            {
                return context.JsonMsgErr("操作异常！");
            }
        } 
        #endregion

        #region 5.0 加载角色权限设置视图
        /// <summary>
        /// 加载角色权限设置视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SetNodeRole(int id)
        {
            //查询指定角色权限id
            var perIds = context.bll.RolePermissionBLL.LoadOrderEntities(p => p.rp_rid == id, p => p.rp_id, true).Select(p => p.rp_pid).ToList();

            Model.ViewModel.RolePerssion perViewModel = new Model.ViewModel.RolePerssion()
            {
                RoleId = id,
                //查询所有权限
                AllPers = context.bll.PermissionBLL.LoadEntities(p => p.p_iddel == false).ToList(),
                //获取权限集合
                UserPers = context.bll.PermissionBLL.LoadEntities(p => perIds.Contains(p.p_id)).ToList()
            };
            return View(perViewModel);
        } 
        #endregion

        #region 5.1 设置角色权限
        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost, Common.Attrs.AjaxRequest]
        public ActionResult SetNodeRole(FormCollection form)
        {
            try
            {
                //获取角色id
                int roleId = int.Parse(form["RoleId"]);
                //获取新的权限id集合
                List<int> listNewIds = form["NewPersIds"].Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(pStr => int.Parse(pStr)).ToList();
                //获取旧的权限id集合
                List<int> listOldIds = context.bll.RolePermissionBLL.LoadEntities(p => p.rp_rid == roleId).Select(p => p.rp_pid).ToList();
                //比较两个集合，去掉里面重复的
                for (var i = listNewIds.Count - 1; i >= 0; i--)
                {
                    var newId = listNewIds[i];
                    if (listOldIds.Contains(newId))
                    {
                        listNewIds.Remove(newId);
                        listOldIds.Remove(newId);
                    }
                }
                //将新id加入数据
                listNewIds.ForEach(newId => context.bll.RolePermissionBLL.AddEntity(new Model.RolePermission()
                {
                    rp_rid = roleId,
                    rp_pid = newId,
                    rp_iddel = false,
                    rp_addtime = DateTime.Now
                }));
                //将不在listNewIds的id移除
                context.bll.RolePermissionBLL.DeleteList(p => listOldIds.Contains(p.rp_pid) && p.rp_rid == roleId);
                context.bll.SaveChanges();
                return context.JsonMsgOK("设置权限成功！");
            }
            catch
            {
                return context.JsonMsgErr("操作异常！");
            }
        } 
        #endregion
    }
}
