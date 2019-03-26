using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WHJ.MvcOA.WebApp.Areas.UserWF
{
    /// <summary>
    /// 工作流 业务处理引擎
    /// </summary>
    public class WorkFlowEnginee
    {
        private OperationContext context
        {
            get
            {
                return OperationContext.Current;
            }
        }

        #region 1.0 新增申请单
        /// <summary>
        /// 1.0 新增申请单
        /// </summary>
        /// <param name="apply"></param>
        /// <returns></returns>
        public bool AddApply(Model.WR_WorkFlowApply apply)
        {
            //根据申请单 所提交的工作流id，查询工作流的第一个节点
            var firNode = context.bll.W_WorkFlowNodeBLL.LoadOrderEntities(n => n.wn_WorkFlowID == apply.wr_WFId && n.wn_IsDel == false, n => n.wn_Order, true).FirstOrDefault();
            //将 节点 id设置给申请单 当前节点id
            apply.wf_CurWFNId = firNode.wn_Id;
            apply.wf_State = 0;
            //新增申请单 到申请单表
            context.bll.WR_WorkFlowApplyBLL.AddEntity(apply);
            context.bll.SaveChanges();
            //记录申请明细
            AddApplyDetail(apply, ApplyOperation.Commit, apply.wf_Title + "-" + apply.wf_Content, apply.wr_UserId, firNode);
            //自动将申请单转入下一个节点
            AutoToNextNode(apply);
            return true;
        }
        #endregion

        #region 1.1 根据申请单保存 申请明细
        /// <summary>
        /// 根据申请单保存 申请明细
        /// </summary>
        /// <param name="apply">申请单实体</param>
        /// <param name="operation">操作类型</param>
        /// <param name="reason">操作内容</param>
        public void AddApplyDetail(Model.WR_WorkFlowApply apply, ApplyOperation operation, string reason, int userId, Model.W_WorkFlowNode nowNode)
        {
            Model.WR_WrokFlowApplyDetails aplDetail = new Model.WR_WrokFlowApplyDetails()
            {
                wrd_Rid = apply.wr_Id,
                wrd_UserId = userId,
                wrd_NodeId = nowNode.wn_Id,//将申请单所在节点id 设置
                wrd_NodeName = nowNode.wn_Name,
                // wrd_NodeName = context.bll.W_WorkFlowNodeBLL.LoadEntities(n => n.wn_Id == apply.wf_CurWFNId && n.wn_IsDel == false).FirstOrDefault().wn_Name,//根据申请单所在的节点id外键对应的导航属性获取对应的节点名称
                wrd_Operation = (int)operation,//操作类型
                wrd_Reason = reason,//操作内容
                wrd_Addtime = DateTime.Now,
                wrd_IsDel = false
            };
            context.bll.WR_WrokFlowApplyDetailsBLL.AddEntity(aplDetail);
            context.bll.SaveChanges();
        }
        #endregion

        #region 1.2 自动到下一个节点
        /// <summary>
        /// 自动到下一个节点
        /// </summary>
        /// <param name="apply"></param>
        public void AutoToNextNode(Model.WR_WorkFlowApply apply)
        {
            //获取申请单下一个节点
            var curNode = context.bll.W_WorkFlowNodeBLL.LoadEntities(n => n.wn_Id == apply.wf_CurWFNId && n.wn_IsDel == false).FirstOrDefault();
            //修改申请单的当前节点id
            var nextNode = curNode.NextNode;
            apply.wf_CurWFNId = nextNode.wn_Id;
            apply.wf_State = 2;
            context.bll.SaveChanges();
            //更新记录明细
            AddApplyDetail(apply, ApplyOperation.Pass, "自动进入下一节点", apply.wr_UserId, curNode);
            context.bll.SaveChanges();
        }
        #endregion

        #region 1.3 审批申请单
        /// <summary>
        /// 审批申请单
        /// </summary>
        /// <param name="applyId">申请单id</param>
        /// <param name="operation">操作类型</param>
        /// <param name="approveContent">审批意见</param>
        /// <param name="userId">审批人</param>
        public void ApproveApply(int applyId, ApplyOperation operation, string approveContent, int userId)
        {
            //1. 查询审批的申请单
            var apply = context.bll.WR_WorkFlowApplyBLL.LoadEntities(a => a.wr_Id == applyId && a.wf_IsDel == false).FirstOrDefault();
            //判断操作类型
            switch (operation)
            {
                #region 1.0 拒绝
                case ApplyOperation.Refuse:
                    {
                        apply.wf_State = 1;//已拒绝
                        context.bll.SaveChanges();
                        var curNode = apply.W_WorkFlowNode;
                        AddApplyDetail(apply, operation, approveContent, userId, curNode);
                        break;
                    }
                #endregion

                #region 2.0 驳回
                case ApplyOperation.Reject:
                    {
                        var curNode = apply.W_WorkFlowNode;
                        //如果上一个节点是第一个节点
                        if (curNode.PrevNode.wn_NodeType == 1)
                        {
                            //结束申请
                            apply.wf_State = 1;
                            context.bll.SaveChanges();
                            AddApplyDetail(apply, ApplyOperation.Refuse, approveContent, userId, curNode);
                        }
                        //如果上一个节点不是第一个节点
                        else
                        {
                            apply.wf_CurWFNId = apply.W_WorkFlowNode.PrevNode.wn_Id;
                            context.bll.SaveChanges();
                            AddApplyDetail(apply, operation, approveContent, userId, curNode);
                        }
                    }
                    break;
                #endregion

                #region 3.0 通过
                case ApplyOperation.Pass:
                    {
                        var curNode = apply.W_WorkFlowNode;
                        //判断节点的类型是否是业务节点
                        if (curNode.wn_NodeFuncType == 2)//如果是业务节点
                        {
                            //获取业务类名
                            string strClassName = "WHJ.MvcOA.WebApp.Areas.UserWF.ProcessClass." + curNode.wn_NodeFuncClassName;
                            //从当前程序集中 获取业务类 类型对象
                            Type classType = Assembly.GetExecutingAssembly().GetType(strClassName);
                            //创建业务类对象
                            ProcessClass.IProcess proc = Activator.CreateInstance(classType) as ProcessClass.IProcess;
                            //调用业务处理方法,根据申请单申请内容判断
                            bool procResult = proc.ProcessBus(apply.wf_Content);
                            //根据业务返回值查询不同的分支节点
                            var branchNode = context.bll.W_WorkFlowBranchBLL.LoadEntities(b => b.wfb_WnParentId == apply.wf_CurWFNId && b.wfb_IsDel == false && b.wfb_Result == procResult).FirstOrDefault();
                            //判断分支节点是否是最后一个节点
                            if (branchNode.W_WorkFlowNode.wn_NodeType == 3)
                            {
                                apply.wf_State = 3;
                                approveContent = "自动结束-" + approveContent;
                            }
                            //跟新申请单当前节点id为分直节点
                            apply.wf_CurWFNId = branchNode.wfb_WnId.Value;
                        }
                        //如果不是业务节点
                        else
                        {
                            var nextNode = apply.W_WorkFlowNode.NextNode;
                            apply.wf_CurWFNId = nextNode.wn_Id;
                            if (apply.W_WorkFlowNode.wn_NodeType == 3)
                            {
                                apply.wf_State = 3;
                            }
                        }
                        context.bll.SaveChanges();
                        AddApplyDetail(apply, operation, approveContent, userId, curNode);
                        break;
                    }
                    #endregion
            }
        }
        #endregion

    }
}