using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WHJ.MvcOA.WebApp.Areas.UserWF
{
    /// <summary>
    /// 工作流操作类型
    /// </summary>
    public enum ApplyOperation
    {
        /// <summary>
        /// 提交
        /// </summary>
        Commit,
        /// <summary>
        /// 通过
        /// </summary>
        Pass,
        /// <summary>
        /// 驳回
        /// </summary>
        Reject,
        /// <summary>
        /// 拒绝
        /// </summary>
        Refuse
    }
}