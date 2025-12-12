using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    /// <summary>
    /// 系统导航菜单实体类
    /// </summary>
    //创建主页面左侧弹出的菜单栏页面属性
    public class MenuBar : BindableBase
    {
        //定义图标属性
        private string icon;
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        //定义菜单名称属性
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        //定义命名空间属性
        private string nameSpace;
        public string NameSpace
        {
            get { return nameSpace; }
            set { nameSpace = value; }
        }
    }
}
