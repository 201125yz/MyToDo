using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class MsgViewModel : BindableBase, IDialogHostAware
    {

        public MsgViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public string DialogHostName { get; set; } = "RootDialog";
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        /// <summary>
        /// 属性定义
        /// </summary>
        private string title;
        private string message;
        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }
        public string Message
        {
            get { return message; }
            set { message = value; RaisePropertyChanged(); }
        }

        public void OnDialogOpend(IDialogParameters parameters)
        {
            if(parameters.ContainsKey("Title"))
                Title = parameters.GetValue<string>("Title");
            if(parameters.ContainsKey("Message"))
                Message = parameters.GetValue<string>("Message");
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName,new DialogResult(ButtonResult.No));
        }

        private void Save()
        {
            var param = new DialogParameters();
            var result = new DialogResult(ButtonResult.OK) { Parameters = param };// 如果有需要传递的参数
            DialogHost.Close(DialogHostName, result);
        }
    }
}
