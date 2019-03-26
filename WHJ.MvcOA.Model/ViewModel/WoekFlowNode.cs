using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model.ViewModel
{
   public partial class WoekFlowNode
    {
        public int wn_Id { get; set; }
        public int wn_WorkFlowID { get; set; }
       [Required(ErrorMessage="节点名称不能为空"),DisplayName("节点名称")]
        public string wn_Name { get; set; }
        public Nullable<int> wn_PrevNode { get; set; }
        public Nullable<int> wn_NextNode { get; set; }
        public int wn_NodeType { get; set; }
        public int wn_NodeFuncType { get; set; }
        public string wn_NodeFuncClassName { get; set; }
        public int wn_Order { get; set; }
        public bool wn_IsDel { get; set; }
        public System.DateTime wn_Addtime { get; set; }

        public Nullable<int> trueNode { get; set; }
        public Nullable<int> falseNode { get; set; }

        public Model.W_WorkFlowNode ToPOCO()
        {
            return new Model.W_WorkFlowNode()
            {
                wn_Id = this.wn_Id,
                wn_WorkFlowID = this.wn_WorkFlowID,
                wn_Name = this.wn_Name,
                wn_PrevNode = this.wn_PrevNode,
                wn_NextNode = this.wn_NextNode,
                wn_NodeType = this.wn_NodeType,
                wn_NodeFuncType = this.wn_NodeFuncType,
                wn_NodeFuncClassName = this.wn_NodeFuncClassName,
                wn_Order = this.wn_Order,
                wn_IsDel = this.wn_IsDel,
                wn_Addtime = this.wn_Addtime,
            };
        }
    }
}
