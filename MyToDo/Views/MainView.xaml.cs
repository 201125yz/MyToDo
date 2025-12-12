using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyToDo.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {

        private readonly IDialogHostService dialogHostService;
        public MainView(IEventAggregator aggregator, IDialogHostService dialogHostService)
        {
            InitializeComponent();
            //注册已完成的消息提示
            aggregator.ResgiterMessage(arg =>
            {
                Snackbar.MessageQueue.Enqueue(arg.Message);
            });

            //注册等待消息窗口
            aggregator.Resgiter(arg =>
            {
                dialogHost.IsOpen = arg.IsOpen;
                if (dialogHost.IsOpen)
                    dialogHost.DialogContent = new ProgressView();
            });
            //定义主页面最小化事件
            butMin.Click += (s, e) =>
            {
                this.WindowState = WindowState.Minimized;
            };

            //定义主页面最大化事件
            butMax.Click += (s, e) =>
            {
                //如果当前主页面已经最大化了，点击最大化变为正常页面大小，否则最大化。
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            };

            //定义关闭事件
            butClose.Click += async (s, e) =>
            {
                var dialogResult = await dialogHostService.Question("温馨提示", "确定退出系统？");
                if (dialogResult.Result != ButtonResult.OK)
                    return;
                this.Close();
            };

            //定义鼠标左键点击拖动页面导航栏可移动事件
            colorZone.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };

            //定义鼠标双击导航栏可最大化或变换为正常页面大小的事件
            colorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else
                    this.WindowState = WindowState.Normal;
            };

            //定义导航菜单栏页自动关闭事件
            menuBar.SelectionChanged += (s, e) =>
            {
                drawerHost.IsLeftDrawerOpen = false;
            };
            this.dialogHostService = dialogHostService;
        }
    }
}
