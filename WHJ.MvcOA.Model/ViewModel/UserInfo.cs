using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHJ.MvcOA.Common;

namespace WHJ.MvcOA.Model.ViewModel
{
   public partial class UserInfo
    {
        public int user_id { get; set; }
        [DisplayName("所属部门")]
        public int u_depid { get; set; }
        [DisplayName("登录名"), Required(ErrorMessage = "登录名不能为空")]
        public string u_name { get; set; }
       [DisplayName("真实姓名"), Required(ErrorMessage = "真实不能为空")]
        public string real_name { get; set; }
        [
        DisplayName("登录密码"),
        Required(ErrorMessage = "密码不能为空"),
        StringLength(40, MinimumLength = 5, ErrorMessage = "长度必须在5-12之间")
        ]
        public string u_pwd { get; set; }

        [DisplayName("Email"), Required(ErrorMessage = "Email不能为空"), RegularExpression(@"^\w+@\w+.(com|COM)$", ErrorMessage = "请输入正确Email地址")]
        public string u_email { get; set; }
       [DisplayName("手机号"), Required(ErrorMessage = "手机号不能为空"), RegularExpression(@"^1[34578]\d{9}$", ErrorMessage="请输入正确的手机号")]
        public string u_telephone { get; set; }
        public bool u_isdel { get; set; }
        public bool u_is_lock { get; set; }
        public System.DateTime u_add_time { get; set; }

        public Model.UserInfo ToPOCO()
        {
            return new Model.UserInfo()
            {
                user_id = this.user_id,
                real_name=this.real_name,
                u_add_time = this.u_add_time,
                u_depid=this.u_depid,
                u_email=this.u_email,
                u_is_lock=this.u_is_lock,
                u_isdel=this.u_isdel,
                u_name=this.u_name,
                u_pwd=this.u_pwd.MD5(),
                u_telephone=this.u_telephone
            };
        }

        public static ViewModel.UserInfo ToViewModel(Model.UserInfo user)
        {
            return new ViewModel.UserInfo()
            {
                real_name=user.real_name,
                u_add_time=user.u_add_time,
                u_depid=user.u_depid,
                u_email=user.u_email,
                u_is_lock=user.u_is_lock,
                u_isdel=user.u_isdel,
                u_name=user.u_name,
                u_pwd=user.u_pwd,
                u_telephone=user.u_telephone,
                user_id=user.user_id
            };
        }
    }
}
