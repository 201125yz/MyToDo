using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    public class ResgiterUserDto : BaseDto
    {
        //用于注册
        //定义用户名
        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }

        //定义账号
        private string account;
        public string Account
        {
            get { return account; }
            set { account = value; OnPropertyChanged(); }
        }

        //定义密码
        private string passWord;
        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; OnPropertyChanged(); }
        }

        private string newpassWord;
        public string NewpassWord
        {
            get { return newpassWord; }
            set { newpassWord = value; OnPropertyChanged(); }
        }
    }
}
