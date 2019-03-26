using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model.ViewModel
{
   public class Enumeration
    {
       public string Value;
       public string Text;

       #region 1.0 获取权限操作类型
       private static List<Enumeration> _operType = null;
       /// <summary>
       /// 获取权限操作类型
       /// 1-treenode 2-formbtn 3-datagrid 4-jqueryAjax 5-treeAjax
       /// </summary>
       /// <returns></returns>
       public static List<Enumeration> OperationType()
       {
           if (_operType == null)
           {
               _operType = new List<Enumeration>()
               {
                   new Enumeration(){Value="1",Text="树菜单节点"},
                   new Enumeration(){Value="2",Text="操作按钮"},
                   new Enumeration(){Value="3",Text="DataGrid组件"},
                   new Enumeration(){Value="4",Text="手动Ajax"},
                   new Enumeration(){Value="5",Text="树菜单请求"}
               };
           }
           return _operType;
       } 
       #endregion

       #region 2.0 获取HttpMethod方式
       private static List<Enumeration> _reqMethods = null;
       /// <summary>’；
       /// 获取HttpMethod方式
       /// </summary>
       /// <returns></returns>
       public static List<Enumeration> RequestMethods()
       {
           if (_reqMethods == null)
           {
               _reqMethods = new List<Enumeration>()
               {
                   new Enumeration(){Value="1",Text="GET"},
                   new Enumeration(){Value="2",Text="POST"},
                   new Enumeration(){Value="3",Text="BOTH"}
               };
           }
           return _reqMethods;
       } 
       #endregion

       #region 3.0 获取申请单状态
       private static List<Enumeration> _wfState = null;
       /// <summary>’；
       /// 获取申请单状态
       /// </summary>
       /// <returns></returns>
       public static List<Enumeration> GetWFState()
       {
           if (_wfState == null)
           {
               _wfState = new List<Enumeration>()
               {
                   new Enumeration(){Value="0",Text="已提交"},
                   new Enumeration(){Value="1",Text="已拒绝"},
                   new Enumeration(){Value="2",Text="审批中"},
                   new Enumeration(){Value="3",Text="已结束"}
               };
           }
           return _wfState;
       }
       #endregion

       #region 4.0 获取工作流申请表优先级
       private static List<Enumeration> _priority = null;
       /// <summary>
       /// 获取工作流申请表优先级
       /// </summary>
       /// <returns></returns>
       public static List<Enumeration> GetPriority()
       {
           if (_priority == null)
           {
               _priority = new List<Enumeration>() {
               new Enumeration() { Value="1",Text="高"},
               new Enumeration(){Value="2",Text="中"},
               new Enumeration(){Value="3",Text="低"}
               };
           }
           return _priority;
       } 
       #endregion

       #region 4.0 获取工作流操作类型
       private static List<Enumeration> _wfoperation = null;
       /// <summary>
       /// 获取工作流操作类型
       /// </summary>
       /// <returns></returns>
       public static List<Enumeration> GetWFOperation()
       {
           if (_wfoperation == null)
           {
               _wfoperation = new List<Enumeration>() {
               new Enumeration() { Value="0",Text="提交"},
               new Enumeration() { Value="1",Text="通过"},
               new Enumeration(){Value="2",Text="驳回"},
               new Enumeration(){Value="3",Text="拒绝"}
               };
           }
           return _wfoperation;
       }
       #endregion
    }
}
