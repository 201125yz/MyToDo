using MyToDo.Common;
using MyToDo.Service;
using MyToDo.ViewModels;
using MyToDo.ViewModels.DialogViewModels;
using MyToDo.Views;
using MyToDo.Views.Dialogs;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace MyToDo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        //创建启动页
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();//从容器中取出启动窗口
        }

        //注销
        public static void LoginOut(IContainerProvider containerProvider)
        {
            //隐藏软件界面（主页面）
            Current.MainWindow.Hide();
            //从容器中取出Dialogservice
            var dialog = containerProvider.Resolve<IDialogService>();
            dialog.ShowDialog("LoginView", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    //Environment.Exit(0);
                    Application.Current.Shutdown();
                    return;
                }
               Current.MainWindow.Show();

            });
        }

        //初始化
        protected override void OnInitialized()
        {
            // 1️⃣ 启动后台 API.exe
            string apiPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "API", "MyToDo.Api.exe");
            if (File.Exists(apiPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = apiPath,
                    Arguments = "",
                    UseShellExecute = false,
                    CreateNoWindow = true // 隐藏窗口
                };
                Process.Start(startInfo);
            }

            //从容器中取出对话服务
            var dialog = Container.Resolve<IDialogService>();
            dialog.ShowDialog("LoginView", callback =>
            {
                if(callback.Result != ButtonResult.OK)
                {
                    Application.Current.Shutdown();
                    return;
                }
                var service = App.Current.MainWindow.DataContext as IConfigureService;
                if (service != null)
                {
                    service.Configure();
                }
                base.OnInitialized();

            });  
        }

        //依赖注入方法
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //注册http-API
            containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));
            containerRegistry.GetContainer().RegisterInstance(@"http://localhost:3333", serviceKey: "webUrl");

            //注册服务
            containerRegistry.Register<ITodoService, ToDoService>();
            containerRegistry.Register<IMemoService, MemoService>();
            containerRegistry.Register<IDialogHostService, DialogHostService>();
            containerRegistry.Register<ILoginService, LoginService>();

            //注册登录弹窗
            containerRegistry.RegisterDialog<LoginView, LoginViewModel>();

            //依赖容器注入为导航
            containerRegistry.RegisterForNavigation<AddToDoView, AddToDoViewModel>(); //弹窗
            containerRegistry.RegisterForNavigation<AddMemoView, AddMemoViewModel>();
            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();

            containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();//主页
            containerRegistry.RegisterForNavigation<ToDoView, ToDoViewModel>();//待办
            containerRegistry.RegisterForNavigation<MemoView, MemoViewModel>();//备忘录
            containerRegistry.RegisterForNavigation<SettingView, SettingViewModel>();//设置

            containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();//个性化
            containerRegistry.RegisterForNavigation<AboutView, AboutViewModel>();//关于更多
        }
    }
}
