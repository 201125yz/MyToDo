using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyToDo.Extensions
{
    //设置附加属性让密码能够在前端绑定
    public class PassWordExtensions
    {

        public static string GetPassWord(DependencyObject obj)
        {
            return (string)obj.GetValue(PassWordProperty);
        }

        public static void SetPassWord(DependencyObject obj, string value)
        {
            obj.SetValue(PassWordProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PassWordProperty =
            DependencyProperty.RegisterAttached("PassWord", typeof(string), typeof(PassWordExtensions), new PropertyMetadata(string.Empty,OnPassWordPropertyChanged));

        //属性变更方法
        static void OnPassWordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passWord = d as PasswordBox;
            string password = (string)e.NewValue;//新值

            if (passWord != null && passWord.Password != password)
            {
                passWord.Password = password;
            }
        }

        //写绑定行为
        public class PasswordBehavior: Behavior<PasswordBox>
        {
            //重写行为的两个方法
            protected override void OnAttached()
            {
                base.OnAttached();
                AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
            }

            private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
            {
                PasswordBox passwordBox = sender as PasswordBox;
                string password = PassWordExtensions.GetPassWord(passwordBox);
                if (password != null && passwordBox.Password != password)
                {
                    PassWordExtensions.SetPassWord(passwordBox, passwordBox.Password);
                }
            }

            protected override void OnDetaching()
            {
                base.OnDetaching();
                AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
            }
        }
    }
}
