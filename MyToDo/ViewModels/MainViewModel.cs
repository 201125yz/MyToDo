using Microsoft.Win32;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MyToDo.ViewModels
{
    public class MainViewModel : BindableBase, IConfigureService
    {
        //私有字段声明
        private readonly IRegionManager regionManager;
        private readonly IContainerProvider containerProvider;

        //定义导航日志
        private IRegionNavigationJournal journal;

        //创建构造函数，引入regionManager导航接口
        public MainViewModel(IContainerProvider containerProvider,IRegionManager regionManager)
        {
            //抽象的概念进行实例化并在“这个下面”引入测试方法
            MenuBars = new ObservableCollection<MenuBar>();
           // CreateMenuBar();

            //实例化驱动菜单栏导航命令并引入驱动方法
            NavigateCommand = new DelegateCommand<MenuBar>(NavigateMethod);

            //实例化上一步与下一步的命令
            GoBackCommand = new DelegateCommand(() => 
            { 
                if(journal != null && journal.CanGoBack)
                    journal.GoBack();
            });
            GoForwardCommand = new DelegateCommand(() =>
            {
                if(journal != null && journal.CanGoForward)
                    journal.GoForward();
            });

            HomeCommand = new DelegateCommand(NavigateHome);

            LoginOutCommand = new DelegateCommand(() =>
            {
                //回到登录页面并隐藏软件界面
                App.LoginOut(containerProvider);
            });

            UpdateImageCommand = new DelegateCommand(UpdateImage);
            LoadUserImage();
            this.regionManager = regionManager;
            this.containerProvider = containerProvider;
        }

        //跳转到主页面方法
        private void NavigateHome()
        {
            // 导航到 IndexView
            regionManager.RequestNavigate("MainViewRegion", "IndexView");
        }

        //菜单栏导航驱动方法, 使用前提是前端界面有区域占位空间ContentControl，要知道页面要导航到哪里。  
        private void NavigateMethod(MenuBar obj)
        {
            //如表标题没有或命名空间为空就无需导航
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;

            //导航
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;//上一步与下一步的导航日志的功能 
            });
            
        }

        //注册完毕后，在主VM里写入管理整个菜单栏进行驱动导航(即驱动导航的命令)
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }

        //这里定义home的主页命令
        public DelegateCommand HomeCommand {  get; private set; }

        //定义上一步命令
        public DelegateCommand GoBackCommand { get; private set; }

        //定义下一步命令
        public DelegateCommand GoForwardCommand {  get; private set; }

        //注销命令
        public DelegateCommand LoginOutCommand { get; private set; }

        //更换头像命令
        public DelegateCommand UpdateImageCommand { get; private set; }

        /// <summary>
        /// 属性
        /// </summary>
        //由于菜单栏是一个列表，类型为MenuBar，这里创建一个动态的列表集合类型的集合属性，取名字为menuBars
        private ObservableCollection<MenuBar> menuBars;
        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        private BitmapImage userImage;
        public BitmapImage UserImage
        {
            get { return userImage; }
            set { userImage = value; RaisePropertyChanged(); }
        }

        //利用建立的属性来创建左侧菜单栏的方法
        void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookEditOutline", Title = "待办事项", NameSpace = "ToDoView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title = "备忘录", NameSpace = "MemoView" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "设置", NameSpace = "SettingView" });
        }

        /// <summary>
        /// 配置首页初始化参数
        /// </summary>
        public void Configure()
        {
            UserName = AppSession.UserName;
            CreateMenuBar();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");
        }

        //更新头像方法
        private void UpdateImage()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp";
            dialog.Title = "选择头像";

            if(dialog.ShowDialog() == true)
            {
                //加载图片
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(dialog.FileName);
                bitmap.EndInit();

                // 更新头像
                UserImage = bitmap;
                // 保存到本地（确保下次启动头像仍然存在）
                SaveUserImage(bitmap);
            }
        }

        //将头像图片保存到本地
        private void SaveUserImage(BitmapImage image)
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MyApp");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, "avatar.png");

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(stream);
            }
        }
        //软件启动加载图片
        private void LoadUserImage()
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MyApp",
                "avatar.png");

            if (File.Exists(path))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;   // 关键：加载后释放文件占用
                bitmap.UriSource = new Uri(path, UriKind.Absolute);
                bitmap.EndInit();
                bitmap.Freeze();  // 可选，提高 WPF 性能
                UserImage = bitmap;
            }
        }
    }
}
