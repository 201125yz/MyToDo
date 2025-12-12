using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common
{
    public interface IDialogHostAware 
    {
        //所属弹窗名称
        string DialogHostName { get; set; }

        //打开过程中执行
        void OnDialogOpend(IDialogParameters parameters);

        //弹窗确定与取消命令
        DelegateCommand SaveCommand { get; set; }
        DelegateCommand CancelCommand { get; set; }
    }
}
