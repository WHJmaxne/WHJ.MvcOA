//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WHJ.MvcOA.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class News
    {
        public int n_id { get; set; }
        public int category_id { get; set; }
        public int user_id { get; set; }
        public string n_title { get; set; }
        public string n_author { get; set; }
        public string n_form { get; set; }
        public string n_content { get; set; }
        public int n_sort_id { get; set; }
        public int n_click { get; set; }
        public int n_is_lock { get; set; }
        public System.DateTime add_time { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual UserInfo UserInfo { get; set; }
    }
}
