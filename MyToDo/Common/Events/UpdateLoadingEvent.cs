using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Events
{
    //创建一个实体类
    public class UpdateModel
    {
        public bool IsOpen {  get; set; }//用于判断是否打开界面加载内容
    }
    public class UpdateLoadingEvent : PubSubEvent<UpdateModel>
    {
    }
}
