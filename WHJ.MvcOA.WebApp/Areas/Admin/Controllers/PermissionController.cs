using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHJ.MvcOA.WebApp.Controllers;

namespace WHJ.MvcOA.WebApp.Areas.Admin.Controllers
{
    public class PermissionController : BaseController
    {
        #region 1.0获取easyUI tree 控件请求的 权限树节点 json字符串
        /// <summary>
        /// 获取easyUI tree 控件请求的 权限树节点 json字符串
        /// </summary>
        /// <returns></returns>
        [HttpPost,Common.Attrs.SkipPermission]
        public ActionResult GetPerTreeData()
        {
            return Content(context.UserPerTreeJsonStr);
        } 
        #endregion

        #region 2.0 加载父权限视图
        /// <summary>
        /// 加载权限列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        } 
        #endregion

        #region 2.1 根据 id 加载权限列表数据
        /// <summary>
        /// 根据 id 加载权限列表数据
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(int id)
        {
            //page=1&rows=10
            //接收页码，接收页容量
            string pageIndex = Request.Form["page"];
            string pageSize = Request.Form["rows"];
            Model.FormatModel.PageData pageData = new Model.FormatModel.PageData();
            //验证接收的页码值
            if (!int.TryParse(pageIndex,out pageData.pageIndex))
            {
                pageData.pageIndex = 1;
            }
            //验证接收的页容量值
            if (!int.TryParse(pageSize, out pageData.pageSize))
            {
                pageData.pageSize = 10;
            }
            //1 查询 权限列表数据
            context.bll.PermissionBLL.LoadPageModel(pageData, p => p.p_iddel == false && p.p_parent == id, p => p.p_order);
            pageData.rows = (pageData.rows as IEnumerable<Model.Permission>).Select(p => p.ToPOCO()).ToList();
            return Json(pageData);
        } 
        #endregion

        #region 2.2 加载子权限视图
        /// <summary>
        /// 加载子权限视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult IndexSon(int id)
        {
            ViewBag.id = id;
            return View();
        } 
        #endregion

        #region 3.0 加载新增视图
        /// <summary>
        /// 加载新增视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
        {
            //新增权限增加一条增加父权限
            List<SelectListItem> parentsPers = new List<SelectListItem>(){
                //new SelectListItem(){
                //    Value="1",Text="父权限",Selected=true
                //}
            };
            //查询所有的父权限
            context.bll.PermissionBLL.LoadEntities(p => p.p_parent == 1 && p.p_iddel == false).ToList().ForEach(p => parentsPers.Add(new SelectListItem()
            {
                Value = p.p_id.ToString(),
                Text = p.p_name
            }));
            ViewBag.parentsList = parentsPers;
            //将操作方式类转换成selectitem
            ViewBag.operType = Model.ViewModel.Enumeration.OperationType().ToList().Select(p => new SelectListItem()
            {
                Value=p.Value,
                Text=p.Text
            });
            //将请求方式类转换成selectitem
            ViewBag.requestMethod = Model.ViewModel.Enumeration.RequestMethods().ToList().Select(p => new SelectListItem()
            {
                Value = p.Value,
                Text = p.Text
            });
            return View();
        } 
        #endregion

        #region 3.1 执行新增
        /// <summary>
        /// 执行新增
        /// </summary>
        /// <returns></returns>
        [HttpPost,Common.Attrs.AjaxRequest]
        public ActionResult Add(Model.ViewModel.Permission perModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //补充必填的三个个值
                    perModel.p_iddel = false;
                    perModel.p_addtime = DateTime.Now;
                    perModel.p_isshow = true;
                    //提交到EF容器
                    context.bll.PermissionBLL.AddEntity(perModel.ToPOCO());
                    context.bll.SaveChanges();
                    return context.JsonMsgOK("添加成功！");
                }
                catch (Exception ex)
                {
                    return context.JsonMsgErr(ex.Message);
                }    
            }
            else
            {
                return context.JsonMsgErr("请不要警用浏览器JS!");
            }
            
        } 
        #endregion

        #region 4.0 加载修改视图
        /// <summary>
        /// 加载修改视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Modify(int id)
        {
            //查询要修改的实体
            var model = context.bll.PermissionBLL.LoadEntities(p => p.p_id == id).FirstOrDefault();
            if (model == null)
            {
                return context.JsonMsgErr("找不到要修改的对象！");
            }
            var viewModel = Model.ViewModel.Permission.ToViewModel(model);
            ViewBag.parentsList = context.bll.PermissionBLL.LoadEntities(p => p.p_parent == 1 && p.p_iddel == false).ToList().Select(p => new SelectListItem()
            {
                Value = p.p_id.ToString(),
                Text = p.p_name
            });
            ViewBag.operType = Model.ViewModel.Enumeration.OperationType().Select(p => new SelectListItem()
            {
                Value = p.Value,
                Text = p.Text
            });
            ViewBag.requestMethod = Model.ViewModel.Enumeration.RequestMethods().ToList().Select(p => new SelectListItem()
            {
                Value = p.Value,
                Text = p.Text
            });
            return View(viewModel);
        } 
        #endregion

        #region 4.1 执行修改
        /// <summary>
        /// 执行修改
        /// </summary>
        /// <param name="per"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Modify(Model.ViewModel.Permission per)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.bll.PermissionBLL.EditEntity(per.ToPOCO(), "p_name", "p_parent", "p_areaname", "p_controllername", "p_actionname", "p_formmethod", "p_operationtype", "p_order", "p_linkurl", "p_remark");
                    context.bll.SaveChanges();
                    return context.JsonMsgOK("修改成功！");
                }
                catch (Exception ex)
                {
                    return context.JsonMsgErr(ex.Message);
                }
            }
            else
            {
                return context.JsonMsgNoOK("请不要禁用浏览器JS！");
            }
        } 
        #endregion

        #region 5.0 删除当前权限和子权限
        /// <summary>
        /// 删除当前权限和子权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Common.Attrs.AjaxRequest]
        public ActionResult delete(int id)
        {
            try
            {
                //1.删除当前父权限和子权限
                context.bll.PermissionBLL.EditList(p => p.p_parent == id || p.p_id == id, new string[1] { "p_iddel" }, new object[1] { true });
                context.bll.SaveChanges();
                return context.JsonMsgOK("删除成功！");
            }
            catch
            {
                return context.JsonMsgErr("删除异常！");
            }
        } 
        #endregion

        #region 5.1 查询权限是否有子权限，如果没有则直接删除
        /// <summary>
        /// 查询权限是否有子权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost,Common.Attrs.AjaxRequest]
        public ActionResult IfExsitSons(int id)
        {
            //删除当前权限的子权限
            var model = context.bll.PermissionBLL.LoadEntities(p => p.p_parent == id).FirstOrDefault();
            //如果有子权限，则返回提示是否删除子权限
            if (model != null)
            {
                return context.JsonMsgNoOK("存在子权限！您确定要删除吗?");
            }
            //如果没有子权限，则直接删除
            else
            {
               return delPerSon(id);
            }
        } 
        #endregion

        #region 5.1 执行删除子权限
        /// <summary>
        /// 执行删除子权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Common.Attrs.AjaxRequest]
        public ActionResult delPerSon(int id)
        {
            try
            {
                context.bll.PermissionBLL.EditList(p => p.p_id == id, new string[1] { "p_iddel" }, new object[1] { true });
                context.bll.SaveChanges();
                return context.JsonMsgOK("删除成功!");
            }
            catch
            {
                return context.JsonMsgErr("删除异常！");
            }
        } 
        #endregion

        #region 6.0 加载新增子权限视图
        /// <summary>
        /// 加载新增子权限视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddPerSon(int id)
        {
            var perModel = context.bll.PermissionBLL.LoadEntities(p => p.p_id == id && p.p_iddel == false).FirstOrDefault().ToPOCO();
            ViewBag.permodel = perModel;
            //将操作方式类转换成selectitem
            ViewBag.operType = Model.ViewModel.Enumeration.OperationType().ToList().Select(p => new SelectListItem()
            {
                Value = p.Value,
                Text = p.Text
            });
            //将请求方式类转换成selectitem
            ViewBag.requestMethod = Model.ViewModel.Enumeration.RequestMethods().ToList().Select(p => new SelectListItem()
            {
                Value = p.Value,
                Text = p.Text
            });
            return View();
        } 
        #endregion
    }
}
