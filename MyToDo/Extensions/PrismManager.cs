using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Extensions
{
    public class PrismManager
    {
        //static表静态成员，不需要实例化，readonly为只读变量，只能在定义时或构造函数中赋值，之后不能修改。
        //MainViewRegionName变量名，存放区域名字，MainViewRegion表明这个区域的表示符。
        public static readonly string MainViewRegionName = "MainViewRegion";

        //设置界面里的区域导航
        public static readonly string SettingViewRegionName = "SettingViewRegion";
    }
}
