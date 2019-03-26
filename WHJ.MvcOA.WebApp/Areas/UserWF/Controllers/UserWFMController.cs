using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHJ.MvcOA.WebApp.Controllers;

namespace WHJ.MvcOA.WebApp.Areas.UserWF.Controllers
{
    public class UserWFMController : BaseController
    {
        //调用工作流引擎
        WorkFlowEnginee wfEnginee = new WorkFlowEnginee();

        #region 1.0 加载用户可申请工作流视图
        /// <summary>
        /// 加载用户可申请工作流视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WFList()
        {
            return View();
        } 
        #endregion

        #region 1.1 加载用户工作流列表
        /// <summary>
        /// 加载用户工作流列表
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost, Common.Attrs.AjaxRequest]
        public ActionResult WFList(FormCollection form)
        {
            //接收页码值和页容量值
            string pageIndex = Request.Form["page"];
            string pageSize = Request.Form["rows"];
            //创建分页数据模型
            Model.FormatModel.PageData pageData = new Model.FormatModel.PageData();
            //验证接收到的页码值
            if (!int.TryParse(pageIndex, out pageData.pageIndex))
            {
                pageData.pageIndex = 1;
            }
            //验证接收到的页容量值
            if (!int.TryParse(pageSize, out pageData.pageSize))
            {
                pageData.pageSize = 10;
            }
            //查询分页数据
            context.bll.W_WorkFlowBLL.LoadPageModel(pageData, w => w.wf_IsDel == false, w => w.wf_Id, true, "W_WorkFlowNode");
            //将分页数据转为POCO
            pageData.rows = (pageData.rows as IEnumerable<Model.W_WorkFlow>).Select(w => w.ToPOCO(true)).ToList();
            return Json(pageData);
        } 
        #endregion

        #region 2.0 加载用户填写申请视图
        /// <summary>
        /// 加载用户填写申请视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add(int id)
        {
            ViewBag.wfId=id;
            return View();
        } 
        #endregion

        #region 2.1 执行用户提交申请
        /// <summary>
        /// 执行用户提交申请
        /// </summary>
        /// <param name="id"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost, Common.Attrs.AjaxRequest]
        public ActionResult Add(Model.ViewModel.WorkFlowApply aplModel)
        {
            if (ModelState.IsValid)
            {
                var addModel = aplModel.ToPOCO();
                addModel.wr_UserId = context.UserSession.user_id;
                addModel.wf_Addtime = DateTime.Now;
                addModel.wf_IsDel = false;
                if (wfEnginee.AddApply(addModel))
                {
                    return context.JsonMsgOK("提交成功！");
                }
                else
                {
                    return context.JsonMsgNoOK("参数异常！");
                }
            }
            else
            {
                return context.JsonMsgErr("请不要禁用浏览器JS！");
            }
        } 
        #endregion

        #region 3.0 加载 查看自己的申请单 视图
        /// <summary>
        /// 加载 查看自己的申请单 视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApplyList()
        {
            return View();
        } 
        #endregion

        #region 3.1 查看自己的申请单 列表
        /// <summary>
        /// 查看自己的申请单 列表
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApplyList(FormCollection form)
        {
            //接收页码值和页容量值
            string pageIndex = Request.Form["page"];
            string pageSize = Request.Form["rows"];
            //创建分页数据模型
            Model.FormatModel.PageData pageData = new Model.FormatModel.PageData();
            //验证接受的页码值
            if (!int.TryParse(pageIndex, out pageData.pageIndex))
            {
                pageData.pageIndex = 1;
            }
            //验证接收的页容量值
            if (!int.TryParse(pageSize, out pageData.pageSize))
            {
                pageData.pageSize = 10;
            }
            //查询数据
            context.bll.WR_WorkFlowApplyBLL.LoadPageModel(pageData, a => a.wr_UserId == context.UserSession.user_id && a.wf_IsDel == false, a => a.wf_Addtime, false, "W_WorkFlow", "W_WorkFlowNode");
            pageData.rows = (pageData.rows as IEnumerable<Model.WR_WorkFlowApply>).Select(a => a.ToPOCO(true)).ToList();
            return Json(pageData);
        } 
        #endregion

        #region 3.2 加载申请单明细列表
        /// <summary>
        /// 加载申请单明细列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewApply(int id)
        {
            //查询当前申请单
            var aplModel = context.bll.WR_WorkFlowApplyBLL.LoadIncludeEntities(a => a.wr_Id == id && a.wf_IsDel == false, "W_WorkFlow", "W_WorkFlowNode").FirstOrDefault();
            if (aplModel == null)
            {
                return context.JsonMsgNoOK("对不起！您查询的申请单不存在！");
            }
            ViewBag.aplModel = aplModel;
            //查询申请单明细
            var aplDetailModel = context.bll.WR_WrokFlowApplyDetailsBLL.LoadEntities(d => d.wrd_Rid == id && d.wrd_IsDel == false).ToList();
            ViewBag.aplDetailModel = aplDetailModel;
            return View();
        } 
        #endregion

        #region 4.0 加载待审批列表视图
        /// <summary>
        /// 加载待审批列表视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProcessList()
        {
            return View();
        } 
        #endregion

        #region 4.1 加载待审批列表
        /// <summary>
        /// 加载待审批列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProcessList(FormCollection form)
        {
            //接收页码值和页容量值
            string pageIndex = Request.Form["page"];
            string pageSize = Request.Form["rows"];
            //创建分页数据模型
            Model.FormatModel.PageData pageData = new Model.FormatModel.PageData();
            //验证接收的验码值
            if (!int.TryParse(pageIndex, out pageData.pageIndex))
            {
                pageData.pageIndex = 1;
            }
            //验证接收的页容量值
            if (!int.TryParse(pageSize, out pageData.pageSize))
            {
                pageData.pageSize = 10;
            }
            var userId = context.UserSession.user_id;
            //根据用户id 查询用户角色id
            var userRoleIds = context.bll.UserRoleBLL.LoadEntities(u => u.ur_uid == userId && u.ur_IsDel == false).Select(u => u.ur_rid).ToList();
            //查询用户角色对应的节点id
            var nodeIds = context.bll.W_WrokFlowRoleBLL.LoadEntities(n => userRoleIds.Contains(n.wfr_Role)).Select(n => n.wfr_WFNode).ToList();
            //根据节点id集合查询 用户可审批申请单集合
            context.bll.WR_WorkFlowApplyBLL.LoadPageModel(pageData, a => nodeIds.Contains(a.wf_CurWFNId) && a.wr_UserId != userId && a.wf_IsDel == false, a => a.wf_Priority, true, "W_WorkFlow", "W_WorkFlowNode");
            //将分页后的EF代理类 对象集合转成 POCO集合
            pageData.rows = (pageData.rows as IEnumerable<Model.WR_WorkFlowApply>).Select(a => a.ToPOCO(true)).ToList();
            return Json(pageData);
        } 
        #endregion

        #region 4.2 加载待审批明细
        /// <summary>
        /// 加载待审批明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApproveApply(int id)
        {
            var userId = context.UserSession.user_id;
            //根据用户id 查询用户角色id
            var userRoleIds = context.bll.UserRoleBLL.LoadEntities(u => u.ur_uid == userId && u.ur_IsDel == false).Select(u => u.ur_rid).ToList();
            //查询用户角色对应的节点id
            var nodeIds = context.bll.W_WrokFlowRoleBLL.LoadEntities(n => userRoleIds.Contains(n.wfr_Role)).Select(n => n.wfr_WFNode).ToList();
            //查询当前申请单
            var aplModel = context.bll.WR_WorkFlowApplyBLL.LoadIncludeEntities(a => a.wr_Id == id && nodeIds.Contains(a.wf_CurWFNId) && a.wf_IsDel == false, "W_WorkFlow", "W_WorkFlowNode").FirstOrDefault();
            if (aplModel == null)
            {
                return context.JsonMsgNoOK("对不起！您查询的申请单不存在！");
            }
            ViewBag.aplModel = aplModel;
            //查询申请单明细
            var aplDetailModel = context.bll.WR_WrokFlowApplyDetailsBLL.LoadEntities(d => d.wrd_Rid == id && d.wrd_IsDel == false).ToList();
            ViewBag.aplDetailModel = aplDetailModel;
            return View();
        } 
        #endregion

        #region 4.3 申请单审批
        /// <summary>
        /// 申请单审批
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApproveApply()
        {
            //接收提交的表单数据
            int txtAId = int.Parse(Request.Form["txtAId"]);
            string btnId = Request.Form["btnId"];
            string txtContent = Request.Form["txtContent"];
            //获取操作类型
            ApplyOperation operation = ApplyOperation.Commit;
            switch (btnId)
            {
                case "btnPass":
                    operation = ApplyOperation.Pass;
                    break;
                case "btnReject":
                    operation = ApplyOperation.Reject;
                    break;
                default:
                    operation = ApplyOperation.Refuse;
                    break;
            }
            wfEnginee.ApproveApply(txtAId, operation, txtContent, context.UserSession.user_id);
            return context.JsonMsgOK("审批成功！");
        } 
        #endregion
    }
}
