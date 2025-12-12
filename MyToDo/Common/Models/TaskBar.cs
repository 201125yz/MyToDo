using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    /// <summary>
    /// 首页第二行内容实体
    /// </summary>
    public class TaskBar : BindableBase
    {
        //定义首页第二行的图标属性
        private string icon;
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        //定义首页第二行的标题属性
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        //定义首页第二行的内容属性
        private string concent;
        public string Concent
        {
            get { return concent; }
            set { concent = value; RaisePropertyChanged(); }
        }

        //定义首页第二行border背景的颜色属性
        private string bordercolor;
        public string Bordercolor
        {
            get { return bordercolor; }
            set { bordercolor = value; }
        }

        //定义首页第二行点击会跳转的目标空间属性
        private string targetname;
        public string Targetname
        {
            get { return targetname; }
            set { targetname = value; }
        }
    }
}
