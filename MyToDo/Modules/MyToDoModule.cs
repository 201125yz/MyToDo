using MyToDo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Modules
{
    public class MyToDoModule : IModule
    {
        /*作用:当 Prism 启动完应用、加载完模块之后，会调用这个方法。
          通常在这里执行模块初始化逻辑，比如：
          向某个 Region 注册默认视图；
          执行一些启动逻辑；
          初始化后台服务或全局事件等。*/
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();

            // 把IndexView 这个View自动加载到该区域中显示。
            regionManager.RegisterViewWithRegion("MainViewRegion", typeof(IndexView));
        }

        /*作用:在模块加载时，这个方法最先执行。
          用来把类（例如 View、Service、Repository 等）注册到 Prism 的 IoC 容器中（默认是 DryIoc）。
          这样 Prism 在运行时可以自动解析这些类型（即依赖注入）。*/

        //RegisterTypes 是“把 View 注册到导航系统中”，告诉 Prism 以后可以通过名字 "IndexView" 来导航到这个界面。
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<IndexView>("IndexView");
        }
    }
}
