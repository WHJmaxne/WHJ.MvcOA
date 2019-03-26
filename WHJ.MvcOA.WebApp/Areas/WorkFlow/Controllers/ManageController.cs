using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHJ.MvcOA.WebApp.Controllers;

namespace WHJ.MvcOA.WebApp.Areas.WorkFlow.Controllers
{
    public class ManageController : BaseController
    {
        #region 1.0 加载工作流列表视图
        /// <summary>
        /// 加载工作流列表视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        } 
        #endregion

        #region 1.1 加载工作流列表
        /// <summary>
        /// 加载工作流列表
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
            Model.FormatModel.PageData pageData = new Model.FormatModel.PageData();
            //验证传入的页码值
            if (!int.TryParse(pageIndex, out pageData.pageIndex))
            {
                pageData.pageIndex = 1;
            }
            //验证传入的页容量值
            if (!int.TryParse(pageSize,out pageData.pageSize))
            {
                pageData.pageSize = 10;
            }
            //查询分页数据
            context.bll.W_WorkFlowBLL.LoadPageModel(pageData, p => p.wf_IsDel == false, p => p.wf_Id, true);
            //将数据转为POCO
            pageData.rows = (pageData.rows as IEnumerable<Model.W_WorkFlow>).Select(p => p.ToPOCO(true)).ToList();
            return Json(pageData);
        }
        #endregion

        #region 2.0 加载添加视图
        /// <summary>
        /// 加载添加视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        } 
        #endregion

        #region 2.1 执行添加
        /// <summary>
        /// 执行添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, Common.Attrs.AjaxRequest]
        public ActionResult Add(Model.ViewModel.W_WorkFlow workf)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    workf.wf_IsDel = false;
                    workf.wf_Addtime = DateTime.Now;
                    context.bll.W_WorkFlowBLL.AddEntity(workf.ToPOCO());
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
                return context.JsonMsgNoOK("请不要禁用浏览器JS!");
            }
        } 
        #endregion

        #region 3.0 执行删除
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
                context.bll.W_WorkFlowBLL.EditList(w => w.wf_Id == id, new string[1] { "wf_IsDel" }, new object[1] { true });
                context.bll.SaveChanges();
                return context.JsonMsgOK("删除成功！");
            }
            catch
            {
                return context.JsonMsgErr("操作异常！");
            }

        } 
        #endregion

        #region 4.0 加载修改视图
        /// <summary>
        /// 加载修改视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns
        [HttpGet]
        public ActionResult Modify(int id)
        {
            var wfModel = context.bll.W_WorkFlowBLL.LoadEntities(w => w.wf_Id == id).FirstOrDefault();
            if (wfModel == null)
            {
                return context.JsonMsgNoOK("找不到删除的对象！");
            }
            var wfViewModel = Model.ViewModel.W_WorkFlow.ToViewModel(wfModel);
            return View(wfViewModel);
        } 
        #endregion

        #region 4.1 执行修改
        /// <summary>
        /// 执行修改
        /// </summary>
        /// <param name="wf"></param>
        /// <returns></returns>
        [HttpPost, Common.Attrs.AjaxRequest]
        public ActionResult Modify(Model.ViewModel.W_WorkFlow wf)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.bll.W_WorkFlowBLL.EditEntity(wf.ToPOCO(), "wf_Name", "wf_Remark");
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
                return context.JsonMsgNoOK("请不要禁用浏览器JS！");
            }
        } 
        #endregion

    }
}
