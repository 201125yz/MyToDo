using MyToDo.Common;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    
    public class LoginViewModel : BindableBase, IDialogAware
    {
        private readonly IEventAggregator aggregator;
        public LoginViewModel(ILoginService loginService, IEventAggregator aggregator) 
        {
            UserDto = new ResgiterUserDto();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.loginService = loginService;
            this.aggregator = aggregator;

            LoadLoginInfo();
        }

        public DialogCloseListener RequestClose {  get; private set; }
        public string Title { get; set; } = "记事本";

        public bool CanCloseDialog()
        {
            return true;
        }

        /// <summary>
        /// 属性
        /// </summary>
        private string account;//账号
        public string Account
        {
            get { return account; }
            set { account = value; RaisePropertyChanged(); }
        }

        private string passWord;//密码
        private readonly ILoginService loginService;

        public string PassWord
        {
            get { return passWord; }
            set {  passWord = value; RaisePropertyChanged(); }
        }

        private string userName;//用户名
        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }

        private bool rememberMe;
        public bool RememberMe
        {
            get { return rememberMe; }
            set { rememberMe = value; RaisePropertyChanged(); }
        }

        private ResgiterUserDto userDto;
        public ResgiterUserDto UserDto
        {
            get { return userDto; }
            set { userDto = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 命令
        /// </summary>
        public DelegateCommand<string> ExecuteCommand {  get; private set; }


        /// <summary>
        /// 方法
        /// </summary>
        public void OnDialogClosed()
        {
            //LoginOutMethod();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "Login":
                    LoginMethod();
                    break;
                case "LoginOut":
                    LoginOutMethod();
                    break;
                case "Go"://跳转注册页面
                    SelectedIndex = 1;
                    break;
                case "Resgiter"://注册账号
                    ResgiterMethod();
                    break;
                case "Return"://返回登录
                    SelectedIndex = 0;
                    break;
            }
                
        }

        //注册账号方法
        private async void ResgiterMethod()
        {
            if(string.IsNullOrWhiteSpace(UserDto.Account)||
                string.IsNullOrWhiteSpace(UserDto.UserName)||
                string.IsNullOrWhiteSpace(UserDto.PassWord)||
                string.IsNullOrWhiteSpace(UserDto.NewpassWord))
                { return; }

            if(UserDto.PassWord != UserDto.NewpassWord)
            {
                aggregator.UpdateMessage("两次密码不一致，请重新输入！", "Login");//事件消息发送
                return;
            }
           var resgiterResult = await loginService.ResgiterAsync(new Shared.Dtos.UserDto()
            {
                Account = UserDto.Account,
                UserName = UserDto.UserName, 
                PassWord = UserDto.PassWord,
            });

            if (resgiterResult != null && resgiterResult.Status)
            {
                aggregator.UpdateMessage("注册成功","Login");//事件消息发送
                SelectedIndex = 0;//注册成功
                return;
            }
            //注册失败
            aggregator.UpdateMessage(resgiterResult.Message, "Login");//事件消息发送
        }

        //private async void LoginMethod()
        //{
        //    if (string.IsNullOrWhiteSpace(Account) || string.IsNullOrWhiteSpace(PassWord))
        //    { return; }

        //    var loginResult = await loginService.LoginAsync(new Shared.Dtos.UserDto()
        //    {
        //        Account = Account,
        //        UserName = UserDto.UserName,
        //        PassWord = PassWord
        //    });

        //    if (loginResult != null && loginResult.Status)
        //    {
        //        //AppSession.UserName = loginResult.Result.UserName;//登录成功更新用户名
        //        RequestClose.Invoke(new DialogResult(ButtonResult.OK));
        //        return;
        //    }
        //    //登录失败
        //    else
        //    {
        //        aggregator.UpdateMessage(loginResult.Message, "Login");//事件消息发送
        //    }   
        //}

        private async void LoginMethod()
        {
            if (string.IsNullOrWhiteSpace(Account) || string.IsNullOrWhiteSpace(PassWord) || string.IsNullOrWhiteSpace(UserName))
                return;

            var loginResult = await loginService.LoginAsync(new UserDto
            {
                Account = Account,
                PassWord = PassWord,
                UserName = UserName
            });

            if (loginResult != null && loginResult.Status)
            {
                // 安全获取 UserDto
                var userDto = loginResult.Result;
                if (userDto != null)
                {
                    AppSession.UserName = UserName;
                    SaveLoginInfo();
                    RequestClose.Invoke(new DialogResult(ButtonResult.OK));
                }
                else
                {
                    // 登录失败显示消息
                    aggregator.UpdateMessage("登录失败，账号、密码或用户名错误", "Login");
                }
            }
            else
            {
                //注册账号提示
                aggregator.UpdateMessage(loginResult?.Message ?? "请先注册账号", "Login");
            }
        }

        //退出登录
        private void LoginOutMethod()
        {
            RequestClose.Invoke(new DialogResult(ButtonResult.No));
        }

        //定义软件启动时自动加载保存的账号、密码与用户名的方法
        private void LoadLoginInfo()
        {
            Account = Properties.Settings.Default.Account;
            PassWord = Properties.Settings.Default.PassWord;
            UserName = Properties.Settings.Default.UserName;

            RememberMe = Properties.Settings.Default.RememberMe;
        }

        //定义记住账号/密码与用户名方法
        private void SaveLoginInfo()
        {
            if (RememberMe)
            {
                Properties.Settings.Default.Account = Account;
                Properties.Settings.Default.PassWord = PassWord;
                Properties.Settings.Default.UserName = UserName;
            }
            else
            {
                Properties.Settings.Default.Account = string.Empty;
                Properties.Settings.Default.PassWord = string.Empty;
                Properties.Settings.Default.UserName = string.Empty;
            }

            Properties.Settings.Default.RememberMe = RememberMe;
            Properties.Settings.Default.Save();
        }
    }
}
