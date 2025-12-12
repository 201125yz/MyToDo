using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyToDo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

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
            butClose.Click += (s, e) =>
            {
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
        }

    }
}