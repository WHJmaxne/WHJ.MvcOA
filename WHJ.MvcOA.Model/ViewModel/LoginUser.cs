using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model.ViewModel
{
    public partial class LoginUser
    {
        [Required(ErrorMessage = "登录名不能为空")]
        public string uLoginName { get; set; }
        [Required(ErrorMessage = "密码不能为空")]
        public string uPwd { get; set; }
        [Required(ErrorMessage = "验证码不能为空")]
        public string uVCode { get; set; }
        public bool uIsCookie { get; set; }
    }
}
