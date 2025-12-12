using MyToDo.Common.Models;
using MyToDo.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class SettingViewModel : BindableBase
    {
        //私有字段声明
        private readonly IRegionManager regionManager;

        //创建构造函数，引入regionManager导航接口
        public SettingViewModel(IRegionManager regionManager)
        {
            //传入区域导航参数对象regionManager
            this.regionManager = regionManager;

            //抽象的概念进行实例化并在“这个下面”引入测试方法
            MenuBars = new ObservableCollection<MenuBar>();
            CreateMenuBar();

            //实例化驱动菜单栏导航命令并引入驱动方法
            NavigateCommand = new DelegateCommand<MenuBar>(NavigateMethod);

        }

        //设置界面菜单栏导航驱动方法, 使用前提是前端界面有区域占位空间ContentControl，要知道页面要导航到哪里。  
        private void NavigateMethod(MenuBar obj)
        {
            //如表标题没有或命名空间为空就无需导航
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;

            //导航
            regionManager.Regions[PrismManager.SettingViewRegionName].RequestNavigate(obj.NameSpace);

        }

        //注册完毕后，在主VM里写入管理整个菜单栏进行驱动导航(即驱动导航的命令)
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }


        //由于菜单栏是一个列表，类型为MenuBar，这里创建一个动态的列表集合类型的集合属性，取名字为menuBars
        private ObservableCollection<MenuBar> menuBars;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }

        //利用建立的属性来创建左侧菜单栏的方法
        void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Palette", Title = "个性化", NameSpace = "SkinView" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "系统设置", NameSpace = "" });
            MenuBars.Add(new MenuBar() { Icon = "Information", Title = "关于更多", NameSpace = "AboutView" });
        }
    }
}
