using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Parameters
{
    public class ToDoParameter : QueryParameter
    {
        //定义一个状态属性，用于待办界面下拉选项
        public int? Status {  get; set; }
    }
}
