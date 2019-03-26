using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model.ViewModel
{
   public partial class Permission
    {
        public int p_id { get; set; }
        [DisplayName("父权限")]
        public int p_parent { get; set; }
       [DisplayName("权限名称"),Required(ErrorMessage="权限名不能为空！")]
        public string p_name { get; set; }
       [DisplayName("区域名"), Required(ErrorMessage = "区域名不能为空！")]
        public string p_areaname { get; set; }
       [DisplayName("控制器名"), Required(ErrorMessage = "控制器名不能为空！")]
        public string p_controllername { get; set; }
       [DisplayName("方法名"), Required(ErrorMessage = "方法名不能为空！")]
        public string p_actionname { get; set; }
       [DisplayName("请求方式")]
        public int p_formmethod { get; set; }
       [DisplayName("权限类型")]
        public int p_operationtype { get; set; }
        public string p_ico { get; set; }
       [DisplayName("序号"), Required(ErrorMessage = "序号名不能为空！")]
        public int p_order { get; set; }
        public bool p_islink { get; set; }
       [DisplayName("按钮权限js方法名"),RegularExpression("[a-zA-Z]+",ErrorMessage="只能输入字母！")]
        public string p_linkurl { get; set; }
       [DisplayName("是否显示")]
        public bool p_isshow { get; set; }
       [DisplayName("备注")]
        public string p_remark { get; set; }
        public bool p_iddel { get; set; }
        public System.DateTime p_addtime { get; set; }

        #region 1.0 将Model转成POCO
        /// <summary>
        /// 将Model转成POCO
        /// </summary>
        /// <returns></returns>
        public Model.Permission ToPOCO()
        {
            return new Model.Permission()
            {
                p_id = this.p_id,
                p_parent = this.p_parent,
                p_name = this.p_name,
                p_areaname = this.p_areaname,
                p_controllername = this.p_controllername,
                p_actionname = this.p_actionname,
                p_formmethod = this.p_formmethod,
                p_operationtype = this.p_operationtype,
                p_ico = this.p_ico,
                p_order = this.p_order,
                p_islink = this.p_islink,
                p_linkurl = this.p_linkurl,
                p_isshow = this.p_isshow,
                p_remark = this.p_remark,
                p_iddel = this.p_iddel,
                p_addtime = this.p_addtime,
            };
        } 
        #endregion

        #region 2.0 将POCO转成ViewModel
        /// <summary>
        /// 将POCO转成ViewModel
        /// </summary>
        /// <param name="perModel"></param>
        /// <returns></returns>
        public static ViewModel.Permission ToViewModel(Model.Permission perModel)
        {
            return new Permission()
            {
                p_id = perModel.p_id,
                p_parent = perModel.p_parent,
                p_name = perModel.p_name,
                p_areaname = perModel.p_areaname,
                p_controllername = perModel.p_controllername,
                p_actionname = perModel.p_actionname,
                p_formmethod = perModel.p_formmethod,
                p_operationtype = perModel.p_operationtype,
                p_ico = perModel.p_ico,
                p_order = perModel.p_order,
                p_islink = perModel.p_islink,
                p_linkurl = perModel.p_linkurl,
                p_isshow = perModel.p_isshow,
                p_remark = perModel.p_remark,
                p_iddel = perModel.p_iddel,
                p_addtime = perModel.p_addtime,
            };
        } 
        #endregion
    }
}
