using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model.EasyUIModel
{
    /// <summary>
    /// easyUI的 tree 组件的 节点模型
    /// </summary>
   public class TreeNode
    {
       /// <summary>
       /// 节点id
       /// </summary>
       public int id;

       /// <summary>
       /// 节点显示文本
       /// </summary>
       public string text;

       /// <summary>
       /// 节点状态："open","close"
       /// </summary>
       public string state = "open";

       /// <summary>
       /// 表示该节点是否被选中(只有复选框树才用)
       /// </summary>
       public bool Checked;

       /// <summary>
       /// 被添加到节点的自定义属性
       /// </summary>
       public object attributes;

       /// <summary>
       /// 一个节点数组声明了若干节点
       /// </summary>
       public List<TreeNode> children;
    }
}
