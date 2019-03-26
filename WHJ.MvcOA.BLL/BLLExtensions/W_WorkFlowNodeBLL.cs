using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.BLL
{
    public partial class W_WorkFlowNodeBLL:IBLL.IW_WorkFlowNodeBLL
    {
        #region 1.0 对工作流节点重新排序方法
        /// <summary>
        /// 对工作流节点重新排序方法
        /// </summary>
        /// <param name="wfId">工作流id</param>
        public void ReOrderWFNodes(int wfId,int beginOrder)
        {
            this.CurrentDAL.ExecuteSQL("update W_WorkFlowNode set wn_Order=wn_Order+1 where wn_WorkFlowID=" + wfId + "and wn_Order>" + beginOrder);
        }
        #endregion

        #region 2.0 删除工作流时对工作流重新排序
        /// <summary>
        /// 删除工作流时对工作流重新排序
        /// </summary>
        /// <param name="wnId">工作流id</param>
        /// <param name="delBeginOrder">当前删除的工作流的序号</param>
        public void delReOrderWFNodes(int wfId, int delBeginOrder)
        {
            this.CurrentDAL.ExecuteSQL("update W_WorkFlowNode set wn_Order=wn_Order-1 where wn_WorkFlowID=" + wfId + "and wn_Order>" + delBeginOrder);
        } 
        #endregion
    }
}
