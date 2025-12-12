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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyToDo.Views
{
    /// <summary>
    /// ToDoView.xaml 的交互逻辑
    /// </summary>
    public partial class ToDoView : UserControl
    {
        public ToDoView()
        {
            InitializeComponent();
            // UserControl 加载完成时播放视频
            this.Loaded += ToDoView_Loaded;
        }

        // 页面加载时启动视频
        private void ToDoView_Loaded(object sender, RoutedEventArgs e)
        {
            MyMediaElement.Play();
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            MyMediaElement.Position = TimeSpan.Zero;
            MyMediaElement.Play();
        }
    }
}
