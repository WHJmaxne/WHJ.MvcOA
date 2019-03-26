using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHJ.MvcOA.WebApp.Controllers;

namespace WHJ.MvcOA.WebApp.Areas.WorkFlow.Controllers
{
    public class NodeController : BaseController
    {
        #region 1.0 加载工作流节点视图
        /// <summary>
        /// 加载工作流节点视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int id)
        {
            ViewBag.wId = id;
            return View();
        }
        #endregion

        #region 1.1 加载工作流节点列表
        /// <summary>
        /// 加载工作流节点列表
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(int id, FormCollection form)
        {
            //接收页码和页容量
            string pageIndex = Request.Form["page"];
            string pageSize = Request.Form["rows"];
            //创建分页数据模型
            Model.FormatModel.PageData pageData = new Model.FormatModel.PageData();
            //验证接收的页码值
            if (!int.TryParse(pageIndex, out pageData.pageIndex))
            {
                pageData.pageIndex = 1;
            }
            //验证接收的页容量值
            if (!int.TryParse(pageSize, out pageData.pageSize))
            {
                pageData.pageSize = 10;
            }
            //查询分页数据
            context.bll.W_WorkFlowNodeBLL.LoadPageModel(pageData, w => w.wn_WorkFlowID == id && w.wn_IsDel == false, w => w.wn_Order, true, "PrevNode", "NextNode", "W_WorkNodeType");
            pageData.rows = (pageData.rows as IEnumerable<Model.W_WorkFlowNode>).Select(w => w.ToPOCO(true)).ToList();
            return Json(pageData);
        }
        #endregion

        #region 2.0 加载新增子节点视图
        /// <summary>
        /// 加载新增子节点视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add(int id)
        {
            ViewBag.wfId = id;
            //查询工作流已有的节点集合
            var nodeList=context.bll.W_WorkFlowNodeBLL.LoadOrderEntities(n => n.wn_WorkFlowID == id && n.wn_IsDel == false, n => n.wn_Order, true).ToList();
            var prevNode = nodeList.LastOrDefault();
            if (prevNode != null)
            {
                ViewBag.prevNodeId = prevNode.wn_Id;
            }
            else
            {
                ViewBag.prevNodeId = -1;
            }
            ViewBag.nodeList = nodeList.Select(n => new SelectListItem()
             {
                 Value = n.wn_Id.ToString(),
                 Text = n.wn_Name
             });
            //查询节点业务类型
            ViewBag.nodeType = context.bll.W_WorkNodeTypeBLL.LoadEntities(t => true).ToList().Select(t => new SelectListItem()
             {
                 Value = t.wnt_id.ToString(),
                 Text = t.wnt_name
             });
            return View();
        } 
        #endregion

        #region 2.1 执行新增节点
        /// <summary>
        /// 执行新增节点
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost,Common.Attrs.AjaxRequest]
        public ActionResult Add(Model.ViewModel.WoekFlowNode wnModel)
        {
            //try
            //{
                wnModel.wn_Order = 10001;
                wnModel.wn_Addtime = DateTime.Now;
                wnModel.wn_IsDel = false;
                //如果不是业务节点，需要将业务类名命名为空
                if (wnModel.wn_NodeFuncType < 2)
                {
                    wnModel.wn_NodeFuncClassName = string.Empty;
                }
                var nodeModel = wnModel.ToPOCO();
                //新增 工作流 开始节点
                if (wnModel.wn_PrevNode == -1)
                {
                    AddFirstNode(nodeModel);
                }
                //新增 工作流 结束节点
                else if (wnModel.wn_PrevNode == -2)
                {
                    AddListNode(nodeModel);
                }
                //新增工作流运行节点
                else
                {
                    AddRunNode(nodeModel);
                }

                if (wnModel.wn_NodeFuncType==2)
                {
                    AddBranchNode(nodeModel.wn_Id, wnModel);
                }
                return context.JsonMsgOK("添加成功！");
            //}
            //catch
            //{
            //    return context.JsonMsgErr("操作异常！");
            //}
        } 
        #endregion

        #region 2.1.1 新增工作流开始节点
        /// <summary>
        /// 新增工作流开始节点
        /// </summary>
        /// <param name="nodeModel"></param>
        private void AddFirstNode(Model.W_WorkFlowNode nodeModel)
        {
            //判断当前的新增节点所在的工作流是否有第一个节点
            //查询当前工作流里面的首节点
            var oldFirNode = context.bll.W_WorkFlowNodeBLL.LoadOrderEntities(w => w.wn_WorkFlowID == nodeModel.wn_WorkFlowID && w.wn_IsDel == false, w => w.wn_Order, true).FirstOrDefault();
            //如果工作流中 没有 开始节点，则将当前第一个节点作为第一个节点
            if (oldFirNode == null)
            {
                //查询程序员用的空节点，用来设置开始节点的上一个和下一个节点
                oldFirNode = context.bll.W_WorkFlowNodeBLL.LoadEntities(w => w.wn_Id == 0).FirstOrDefault();
                //因为是开始节点 预先设置 上一个和下一个 节点为0
                nodeModel.wn_PrevNode = oldFirNode.wn_Id;
                nodeModel.wn_NextNode = oldFirNode.wn_Id;
                nodeModel.wn_NodeType = Convert.ToInt32(Model.FormatModel.NodeType.start);
                //保存当前节点
                context.bll.W_WorkFlowNodeBLL.AddEntity(nodeModel);
                context.bll.SaveChanges();
                //将当前节点保存后的id设置给当前节点的两个属性
                nodeModel.wn_PrevNode = nodeModel.wn_Id;
                nodeModel.wn_NextNode = nodeModel.wn_Id;
                context.bll.SaveChanges();
            }
            else
            {
                //因为是开始节点 预先设置 上一个和下一个 节点为0
                nodeModel.wn_PrevNode = oldFirNode.wn_Id;
                nodeModel.wn_NextNode = oldFirNode.wn_Id;
                //设置新节点的序号为原来节点的序号减 1 
                nodeModel.wn_Order = oldFirNode.wn_Order - 1;
                nodeModel.wn_NodeType = Convert.ToInt32(Model.FormatModel.NodeType.start);
                //保存当前节点
                context.bll.W_WorkFlowNodeBLL.AddEntity(nodeModel);
                context.bll.SaveChanges();
                //将当前节点保存后的id设置给当前节点的两个属性
                nodeModel.wn_PrevNode = nodeModel.wn_Id;
                oldFirNode.wn_PrevNode = nodeModel.wn_Id;
                //判断当前节点是否为 最后一个节点
                if (IsLastNode(oldFirNode))
                {
                    oldFirNode.wn_NodeType = Convert.ToInt32(Model.FormatModel.NodeType.end);
                }
                else
                {
                    oldFirNode.wn_NodeType = Convert.ToInt32(Model.FormatModel.NodeType.run);
                }
                context.bll.SaveChanges();
            }
        } 
        #endregion

        #region 2.1.2 新增工作流运行节点
        /// <summary>
        /// 新增工作流运行节点
        /// </summary>
        /// <param name="nodeModel"></param>
        private void AddRunNode(Model.W_WorkFlowNode nodeModel)
        {
            nodeModel.wn_NodeType = Convert.ToInt32(Model.FormatModel.NodeType.run);
            //查询当前新增节点的 上个节点
            var oldPrevNode = context.bll.W_WorkFlowNodeBLL.LoadEntities(n => n.wn_Id == nodeModel.wn_PrevNode).FirstOrDefault();
            //查询当前新增节点的 下个节点
            var oldNextNode = oldPrevNode.NextNode;
            //如果新增节点的上个节点是开始节点且有且只有一个节点，或新增节点的上个节点是结束节点，则新增节点为结束节点
            if (IsFirstNode(oldNextNode) || IsLastNode(oldPrevNode))
            {
                //如果新增节点的上个节点是结束节点，则将上个节点类型改为运行节点
                if (IsLastNode(oldPrevNode) && !IsFirstNode(oldNextNode))
                {
                    oldPrevNode.wn_NodeType = Convert.ToInt32(Model.FormatModel.NodeType.run);
                }
                nodeModel.wn_NodeType = Convert.ToInt32(Model.FormatModel.NodeType.end);
                //设置当前节点的上个节点和下个节点
                nodeModel.wn_PrevNode = oldPrevNode.wn_Id;
                nodeModel.wn_NextNode = oldPrevNode.wn_Id;
                nodeModel.wn_Order = oldPrevNode.wn_Order + 1;
                //将信息添加到数据库
                context.bll.W_WorkFlowNodeBLL.AddEntity(nodeModel);
                context.bll.SaveChanges();
                //设置上个节点的 下个节点id为新增节点id
                oldPrevNode.wn_NextNode = nodeModel.wn_Id;
                //设置新增节点的下个节点为自己
                nodeModel.wn_NextNode = nodeModel.wn_Id;
                context.bll.SaveChanges();
            }
            else
            {
                //设置当前节点的上个节点和下个节点
                nodeModel.wn_PrevNode = oldPrevNode.wn_Id;
                nodeModel.wn_NextNode = oldNextNode.wn_Id;
                //将上一个节点之后的节点序号全部 + 1
                context.bll.W_WorkFlowNodeBLL.ReOrderWFNodes(nodeModel.wn_WorkFlowID, oldPrevNode.wn_Order);
                nodeModel.wn_Order = oldPrevNode.wn_Order + 1;
                //将信息添加到数据库
                context.bll.W_WorkFlowNodeBLL.AddEntity(nodeModel);
                context.bll.SaveChanges();
                //设置上个节点的 下个节点id为新增节点id
                oldPrevNode.wn_NextNode = nodeModel.wn_Id;
                //设置下个节点的 上个节点id为新增节点id
                oldNextNode.wn_PrevNode = nodeModel.wn_Id;
                context.bll.SaveChanges();
            }
        }
        #endregion

        #region 2.1.3 新增工作流结束节点
        /// <summary>
        /// 新增工作流结束节点
        /// </summary>
        /// <param name="nodeModel"></param>
        private void AddListNode(Model.W_WorkFlowNode nodeModel)
        {
            //查询最后一个节点是否存在
            var oldLastNode = context.bll.W_WorkFlowNodeBLL.LoadOrderEntities(n => n.wn_WorkFlowID == nodeModel.wn_WorkFlowID && n.wn_IsDel == false, n => n.wn_Order, false).FirstOrDefault();
            //设置新节点的上一个节点和下一个节点
            nodeModel.wn_PrevNode = oldLastNode.wn_Id;
            nodeModel.wn_NextNode = oldLastNode.wn_Id;
            //将当前节点的额序号设置为上一个节点序号加一
            nodeModel.wn_Order = oldLastNode.wn_Order + 1;
            //将当前节点类型设置为最后一个节点
            nodeModel.wn_NodeType = Convert.ToInt32(Model.FormatModel.NodeType.end);
            context.bll.W_WorkFlowNodeBLL.AddEntity(nodeModel);
            context.bll.SaveChanges();
            //设置新增结束节点的下一个节点为自己
            nodeModel.wn_NextNode = nodeModel.wn_Id;
            //将oldLastNode的下一个节点设置为当前新增的节点
            oldLastNode.wn_NextNode = nodeModel.wn_Id;
            //判断oldLastNode是否为第一个节点
            if (IsFirstNode(oldLastNode))
            {
                oldLastNode.wn_NodeType = Convert.ToInt32(Model.FormatModel.NodeType.start);
            }
            else
            {
                oldLastNode.wn_NodeType = Convert.ToInt32(Model.FormatModel.NodeType.run);
            }
            context.bll.SaveChanges();
        } 
        #endregion

        #region 2.1.4 新增工作流分支节点
        /// <summary>
        /// 新增工作流分支节点
        /// </summary>
        /// <param name="newNodeId"></param>
        /// <param name="viewNode"></param>
        private void AddBranchNode(int newNodeId, Model.ViewModel.WoekFlowNode viewNode)
        {
            //将true节点分支添加到 W_WorkFlowBranch 表中
            Model.W_WorkFlowBranch branchNode = new Model.W_WorkFlowBranch()
            {
                wfb_WnParentId = newNodeId,
                wfb_Result = true,
                wfb_WnId = viewNode.trueNode,
                wfb_Addtime = DateTime.Now,
                wfb_IsDel = false
            };
            //将false节点分支添加到 W_WorkFlowBranch 表中
            Model.W_WorkFlowBranch falseBranchNode = new Model.W_WorkFlowBranch()
            {
                wfb_WnParentId = newNodeId,
                wfb_Result = false,
                wfb_WnId = viewNode.falseNode,
                wfb_Addtime = DateTime.Now,
                wfb_IsDel = false
            };
            context.bll.W_WorkFlowBranchBLL.AddEntity(branchNode);
            context.bll.W_WorkFlowBranchBLL.AddEntity(falseBranchNode);
            context.bll.SaveChanges();
        } 
        #endregion

        #region 3.0 删除工作流节点
        /// <summary>
        /// 删除工作流节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost,Common.Attrs.AjaxRequest]
        public ActionResult delete(int id)
        {
            try
            {
                var branchNode = context.bll.W_WorkFlowBranchBLL.LoadEntities(b => b.wfb_WnId == id && b.wfb_IsDel == false).ToList();
                var delModel = context.bll.W_WorkFlowNodeBLL.LoadEntities(n => n.wn_Id == id).FirstOrDefault();
                var prevNode = delModel.PrevNode;
                var nextNode = delModel.NextNode;
                //当删除的节点是开始节点时
                if (IsFirstNode(delModel))
                {
                    //如果开始节点的下一个节点是业务节点
                    if (nextNode.wn_NodeFuncType==2)
                    {
                        return context.JsonMsgNoOK("开始节点的下一个节点是业务节点，无法删除！");
                    }
                    //如果开始节点是业务分支节点
                    else if (branchNode.Count>0)
                    {
                        return context.JsonMsgNoOK("当前节点为分支节点,将与业务节点一并删除,确认删除吗？");
                    }
                    DelFirstNode(delModel);
                }
                //当删除的节点是结束节点时
                else if (IsLastNode(delModel))
                {
                    //如果结束节点的上一个节点是业务节点
                    if (prevNode.wn_NodeFuncType == 2)
                    {
                        return context.JsonMsgNoOK("结束节点的上一个节点是业务节点，无法删除！");
                    }
                    //如果结束节点是业务分支节点
                    else if (branchNode.Count > 0)
                    {
                        return context.JsonMsgNoOK("当前节点为分支节点,将与业务节点一并删除,确认删除吗？");
                    }
                    DelLastNode(delModel);
                }
                //当删除的节点是运行节点时
                else
                {
                    //如果当前节点是业务分支节点
                    if (branchNode.Count > 0)
                    {
                        return context.JsonMsgNoOK("当前节点为分支节点,将与业务节点一并删除,确认删除吗？");
                    }
                    else
                    {
                        DelRunNode(delModel);
                    }
                }
                return context.JsonMsgOK("删除成功！");
            }
            catch
            {
                return context.JsonMsgErr("操作异常！");
            }
        } 
        #endregion

        #region 3.0.1 删除工作流分支节点
        /// <summary>
        /// 删除工作流分支节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult delBranch(int id)
        {
            //查询分支表中的分支节点
            var branchModel = context.bll.W_WorkFlowBranchBLL.LoadEntities(b => b.wfb_WnId == id && b.wfb_IsDel == false).FirstOrDefault();
            //查询要删除的两个实体
            var nodeModel = context.bll.W_WorkFlowNodeBLL.LoadEntities(n => n.wn_Id == id).FirstOrDefault();
            var nodeBranchModel = context.bll.W_WorkFlowNodeBLL.LoadEntities(n => n.wn_Id == branchModel.wfb_WnParentId).FirstOrDefault();
            //直接删除分支表中的分支节点
            context.bll.W_WorkFlowBranchBLL.EditList(b => b.wfb_WnParentId == branchModel.wfb_WnParentId, new string[1] { "wfb_IsDel" }, new object[1] { true });
            context.bll.SaveChanges();
            //删除节点表中的节点
            if (IsFirstNode(nodeModel))
            {
                DelFirstNode(nodeModel);
            }else if (IsLastNode(nodeModel))
            {
                DelLastNode(nodeModel);
            }
            else
            {
                DelRunNode(nodeModel);
            }
            DelRunNode(nodeBranchModel);
            return context.JsonMsgOK("删除成功！");
        } 
        #endregion

        #region 3.1 删除开始节点
        /// <summary>
        /// 删除开始节点
        /// </summary>
        /// <param name="nodeModel"></param>
        private void DelFirstNode(Model.W_WorkFlowNode nodeModel)
        {
            //查询开始节点的下一个节点
            var nextNode = nodeModel.NextNode;
            //如果下一个节点不为空
            if (!IsFirstNode(nextNode))
            {
                //则将下一个节点id赋值给下一个节点的父节点id
                nextNode.wn_PrevNode = nextNode.wn_Id;
                //将节点的状态改为开始节点
                nextNode.wn_NodeType = Convert.ToInt32(Model.FormatModel.NodeType.start);
                context.bll.SaveChanges();
            }
            //查询程序员用的空节点
            var oldFirNode = context.bll.W_WorkFlowNodeBLL.LoadEntities(n => n.wn_Id == 0).FirstOrDefault();
            //将要删除的节点的父节点id和子节点id设置为空节点的id
            nodeModel.wn_NextNode = oldFirNode.wn_Id;
            nodeModel.wn_PrevNode = oldFirNode.wn_Id;
            //执行删除
            nodeModel.wn_IsDel = true;
            context.bll.SaveChanges();
        } 
        #endregion

        #region 3.2 删除运行节点
        /// <summary>
        /// 删除运行节点
        /// </summary>
        private void DelRunNode(Model.W_WorkFlowNode node)
        {
            //查询要删除节点的父节点
            var prevNode = node.PrevNode;
            //查询要删除节点的子节点
            var nextNode = node.NextNode;
            //将父节点的子节点id等于子节点的id
            prevNode.wn_NextNode = nextNode.wn_Id;
            //将子节点的父节点id等于父节点的id
            nextNode.wn_PrevNode = prevNode.wn_Id;
            //将要删除的节点之后的序号全部 - 1
            context.bll.W_WorkFlowNodeBLL.delReOrderWFNodes(node.wn_WorkFlowID, node.wn_Order);
            //查询程序员用的空节点
            var oldFirNode = context.bll.W_WorkFlowNodeBLL.LoadEntities(n => n.wn_Id == 0).FirstOrDefault();
            //将要删除的节点的父节点id和子节点id设置为空节点的id
            node.wn_NextNode = oldFirNode.wn_Id;
            node.wn_PrevNode = oldFirNode.wn_Id;
            //执行删除
            node.wn_IsDel = true;
            context.bll.SaveChanges();
        } 
        #endregion

        #region 3.3 删除结束节点
        /// <summary>
        /// 删除结束节点
        /// </summary>
        /// <param name="nodeModel"></param>
        private void DelLastNode(Model.W_WorkFlowNode nodeModel)
        {
            //查询要删除节点的父节点
            var prevNode = nodeModel.PrevNode;
            if (!IsFirstNode(prevNode))
            {
                prevNode.wn_NodeType = Convert.ToInt32(Model.FormatModel.NodeType.end);
            }
            //将父节点的子节点id等于父节点的父节点id
            prevNode.wn_NextNode = prevNode.wn_Id;
            context.bll.SaveChanges();
            //查询程序员用的空节点
            var oldFirNode = context.bll.W_WorkFlowNodeBLL.LoadEntities(n => n.wn_Id == 0).FirstOrDefault();
            //将要删除的节点的父节点id和子节点id设置为空节点的id
            nodeModel.wn_NextNode = oldFirNode.wn_Id;
            nodeModel.wn_PrevNode = oldFirNode.wn_Id;
            //执行删除
            nodeModel.wn_IsDel = true;
            context.bll.SaveChanges();
            
        } 
        #endregion

        #region Extension 1.0 判断当前节点是否为 最后一个节点
        /// <summary>
        /// 判断当前节点是否为 最后一个节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        bool IsLastNode(Model.W_WorkFlowNode node)
        {
            return node.wn_NextNode == node.wn_Id;
        } 
        #endregion

        #region Extension 1.1 判断当前节点是否为 第一个节点
        /// <summary>
        /// 判断当前节点是否为 第一个节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        bool IsFirstNode(Model.W_WorkFlowNode node)
        {
            return node.wn_PrevNode == node.wn_Id;
        } 
        #endregion

        #region 4.0 加载设置审批角色页面
        /// <summary>
        /// 加载设置审批角色页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SetNodeRole(int id)
        {
            ViewBag.depList = context.bll.DepartmentBLL.LoadEntities(d => d.dep_isdel == false).ToList();
            ViewBag.wnId = id;
            return View();
        } 
        #endregion

        #region 4.1 根据选择的部门加载部门角色
        /// <summary>
        /// 根据选择的部门加载部门角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRoles(int id)
        {
            var roleList = context.bll.RoleBLL.LoadOrderEntities(r => r.r_depid == id, r => r.r_id).Select(r => r.ToPOCO());
            return Json(roleList);
        } 
        #endregion

        #region 4.2 设置审批角色
        /// <summary>
        /// 设置审批角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Common.Attrs.AjaxRequest]
        public ActionResult SetNodeRole(FormCollection form,int id)
        {
            try
            {
                //接收角色字符串
                string roleIds = form["rolesId"];
                //将字符串转换为int集合
                List<int> roleId = roleIds.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(r => int.Parse(r)).ToList();
                //查询原来的角色id
                var oldRoleId = context.bll.W_WrokFlowRoleBLL.LoadEntities(w => w.wfr_WFNode == id).Select(w => w.wfr_Role).ToList();
                //循环将添加的角色id在旧角色id中存在的id去掉
                for (int i = 0; i < roleId.Count; i++)
                {
                    var newId = roleId[i];
                    if (oldRoleId.Contains(newId))
                    {
                        oldRoleId.Remove(newId);
                        roleId.Remove(newId);
                    }
                }
                //将新角色id加入数据库中
                roleId.ForEach(newId => context.bll.W_WrokFlowRoleBLL.AddEntity(new Model.W_WrokFlowRole()
                {
                    wfr_Role = newId,
                    wfr_WFNode = id,
                }));
                //将不在新角色id中的角色id删除
                context.bll.W_WrokFlowRoleBLL.DeleteList(r => oldRoleId.Contains(r.wfr_Role) && r.wfr_WFNode == id);
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
