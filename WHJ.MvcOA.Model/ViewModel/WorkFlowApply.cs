using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model.ViewModel
{
    /// <summary>
    /// 申请单视图模型
    /// </summary>
    public class WorkFlowApply
    {
        public int wr_Id { get; set; }
        public int wr_UserId { get; set; }
        public int wr_WFId { get; set; }
        public int wf_CurWFNId { get; set; }
        [Required(ErrorMessage="申请名称不能为空！")]
        public string wf_Title { get; set; }
        [Required(ErrorMessage="申请内容不能为空！")]
        public string wf_Content { get; set; }
        public int wf_State { get; set; }
        public Nullable<int> wf_TargetUserId { get; set; }
        public int wf_Priority { get; set; }
        public bool wf_IsDel { get; set; }
        public System.DateTime wf_Addtime { get; set; }

        public Model.WR_WorkFlowApply ToPOCO()
        {
            return new WR_WorkFlowApply()
            {
                wf_State = this.wf_State,
                wf_Addtime = this.wf_Addtime,
                wf_Content = this.wf_Content,
                wf_IsDel = this.wf_IsDel,
                wf_CurWFNId = this.wf_CurWFNId,
                wf_Priority = this.wf_Priority,
                wf_Title = this.wf_Title,
                wr_Id = this.wr_Id,
                wf_TargetUserId = this.wf_TargetUserId,
                wr_UserId = this.wr_UserId,
                wr_WFId = this.wr_WFId
            };
        }
    }
}
