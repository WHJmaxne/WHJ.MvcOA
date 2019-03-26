using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model.ViewModel
{
   public partial class W_WorkFlow
    {
        public int wf_Id { get; set; }
        [Required(ErrorMessage = "工作流名称不能为空！"), DisplayName("工作流名称")]
        public string wf_Name { get; set; }
        [DisplayName("工作流说明")]
        public string wf_Remark { get; set; }
        public bool wf_IsDel { get; set; }
        [DisplayName("添加时间")]
        public System.DateTime wf_Addtime { get; set; }

        public Model.W_WorkFlow ToPOCO()
        {
            return new Model.W_WorkFlow()
            {
                wf_Addtime = this.wf_Addtime,
                wf_Id = this.wf_Id,
                wf_IsDel = this.wf_IsDel,
                wf_Name = this.wf_Name,
                wf_Remark = this.wf_Remark
            };
        }

        public static ViewModel.W_WorkFlow ToViewModel(Model.W_WorkFlow wf)
        {
            return new ViewModel.W_WorkFlow()
            {
                wf_Addtime = wf.wf_Addtime,
                wf_Id = wf.wf_Id,
                wf_Remark = wf.wf_Remark,
                wf_IsDel = wf.wf_IsDel,
                wf_Name = wf.wf_Name
            };
        }
    }
}
