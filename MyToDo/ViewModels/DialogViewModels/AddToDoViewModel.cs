using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyToDo.ViewModels.DialogViewModels
{
    class AddToDoViewModel : BindableBase, IDialogHostAware
    {

        public AddToDoViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public string DialogHostName { get; set; }

        //定义新增或编辑的实体属性
        private ToDoDto model;
        public ToDoDto Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged(); }
        }


        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                Model = parameters.GetValue<ToDoDto>("Value");
            }
            else
            {
                Model = new ToDoDto();
            }
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
        }

        private void Save()
        {
            //如果你不填标题或不填内容，就不让按确定按钮保存
            if(string.IsNullOrWhiteSpace(Model.Title) || string.IsNullOrWhiteSpace(Model.Content))
                return;

            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                var param = new DialogParameters();
                param.Add("Value", Model);
                var result = new DialogResult(ButtonResult.OK) { Parameters = param };// 如果有需要传递的参数
                DialogHost.Close(DialogHostName, result);
            }
        }
    }
}
