using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model.ViewModel
{
    public partial class Role
    {
        public int r_id { get; set; }
        [DisplayName("所属部门")]
        public int r_depid { get; set; }
        [Required(ErrorMessage = "不能空"), DisplayName("角色名称")]
        public string r_name { get; set; }
        [DisplayName("角色备注")]
        public string r_remark { get; set; }
        [DisplayName("是否显示")]
        public bool r_isshow { get; set; }
        public bool r_isdel { get; set; }
        public System.DateTime r_addtime { get; set; }

        public Model.Role ToPOCO()
        {
            return new Model.Role()
            {
                r_id = this.r_id,
                r_depid = this.r_depid,
                r_name = this.r_name,
                r_remark = this.r_remark,
                r_isshow = this.r_isshow,
                r_isdel = this.r_isdel,
                r_addtime = this.r_addtime
            };
        }

        public static ViewModel.Role ToViewModel(Model.Role role)
        {
            return new ViewModel.Role()
            {
                r_addtime=role.r_addtime,
                r_depid=role.r_depid,
                r_id=role.r_id,
                r_isshow=role.r_isshow,
                r_isdel=role.r_isdel,
                r_name=role.r_name,
                r_remark=role.r_remark
            };
        }
    }
}
