using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.Model.FormatModel
{
   public class PageData
    {
       public int pageIndex;
       public int pageSize;
       public int total;//RowCount;为easyUI datagrid组件而改
       public object rows;//pageList;为easyUI datagrid组件而改

       public int pageCount
       {
           get
           {
               return Convert.ToInt32(Math.Ceiling(total * 1.0 / pageSize));
           }
       }
    }
}
