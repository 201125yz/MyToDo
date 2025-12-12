
using MaterialDesignColors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class ToDoViewModel : NavigationViewModel
    {
        private readonly IDialogHostService dialogHost;
        public ToDoViewModel(ITodoService service, IContainerProvider provider):base(provider)  
        {
            //实例化TodoDtos
            TodoDtos = new ObservableCollection<ToDoDto>();

            //实例化待办命令与方法
            ExcuteTodoCommand = new DelegateCommand<string>(ExcuteTodo);

            //实例化待办内容选中命令
            SelectedCommand = new DelegateCommand<ToDoDto>(SelectedToDoMethod);

            //实例化待办内容删除命令
            DeleteCommand = new DelegateCommand<ToDoDto>(DeleteToDo);
            dialogHost = provider.Resolve<IDialogHostService>();//依赖注入
            this.service = service; 
        }

 
        //定义右侧弹窗属性
        private bool isRightDrawerOpen;
        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }
        
        //打开右侧弹窗界面发方法（添加待办）
        private void AddToDo()
        {
            CurrentDto = new ToDoDto();
            IsRightDrawerOpen = true;
        }

        //定义DoToDto类型的集合属性
        private ObservableCollection<ToDoDto> todoDtos;
        private readonly ITodoService service;

        public ObservableCollection<ToDoDto> TodoDtos
        {
            get { return todoDtos; }
            set { todoDtos = value;  RaisePropertyChanged(); }
        }

        //定义编辑/新增操作的选中待办内容的属性状态
        private ToDoDto currentDto;
        public ToDoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }

        //定义待办内容搜索属性
        private string search;
        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }

        //定义待办界面筛选下拉列表选项属性
        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }

        //定义添加待办命令
        public DelegateCommand<string> ExcuteTodoCommand { get; private set; }

        //定义待办内容选中命令
        public DelegateCommand<ToDoDto>SelectedCommand { get; private set; }

        //定义待办内容删除命令
        public DelegateCommand<ToDoDto> DeleteCommand {  get; private set; }

        /// <summary>
        /// 获取待办内容信息显示在待办界面里
        /// </summary>
        /// 查询数据库方法
        async void GetDataAsync()
        {
            UpdateLoading(true);//打开等待窗口

            int? Status = SelectedIndex == 0 ? null : SelectedIndex == 2 ? 1 : 0;

            var todoResult = await service.GetAllFilterAsync(new ToDoParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
                Status = Status
            });

            if (todoResult.Status )
            {
                TodoDtos.Clear();
                foreach (var item in todoResult.Result.Items) 
                {
                    TodoDtos.Add(item);
                }
            }

            UpdateLoading(false);
        }
        //调用GetDataAsync查询数据库的加载方法
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            if(navigationContext.Parameters.ContainsKey("Value"))
                SelectedIndex = navigationContext.Parameters.GetValue<int>("Value");//拿到IndexVM传递的值进行已完成数据跳转
            else
            {
                SelectedIndex = 0;
            }

            GetDataAsync();
        }

        //待办内容选中更改/新增功能方法
        private async void SelectedToDoMethod(ToDoDto dto)
        {
            try
            {
                UpdateLoading(true);
                //加载数据库中的数据
                var todoResule = await service.GetFirstofDefaultAsync(dto.Id);
                if (todoResule.Status)
                {
                    CurrentDto = todoResule.Result;
                    IsRightDrawerOpen = true;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                UpdateLoading(false);
            }
            
        }

        //定义带参方法，包含搜索待办内容与添加待办操作
        private void ExcuteTodo(string obj)
        {
            switch (obj)
            {
                case "新增":
                    AddToDo();
                    break;
                case "搜索":
                    Query();
                    break;
                case "保存":
                    Save();
                    break;
            }
        }

        //定义搜索方法
        private void Query()
        {
            GetDataAsync();
        }

        //定义右侧弹出栏中的添加待办按钮的保存含义方法
        private async void Save()
        {
            //判断输入的标题或者内容不能为空，否则不能保存，即不能添加待办
            if (string.IsNullOrWhiteSpace(CurrentDto.Title) || string.IsNullOrWhiteSpace(CurrentDto.Content))
                return;

            UpdateLoading(true);
            try
            {
                if (CurrentDto.Id > 0)//id大于0为更新数据
                {
                    var updateResult = await service.UpdateAsync(CurrentDto);
                    if (updateResult.Status)
                    {
                        //更新成功后，在ToDoDtos集合里查找对应数据
                        var todo = TodoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (todo != null)
                        {
                            todo.Title = CurrentDto.Title;
                            todo.Content = CurrentDto.Content;
                            todo.Status = CurrentDto.Status;
                        }
                    }
                    IsRightDrawerOpen = false;
                }
                else//id等于0为新增数据
                {
                    var addResult = await service.AddAsync(CurrentDto);
                    if (addResult.Status)
                    {
                        TodoDtos.Add(addResult.Result);
                        IsRightDrawerOpen = false;
                    }
                }
            }
            catch (Exception ex) 
            { 
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        //定义待办内容删除方法
        private async void DeleteToDo(ToDoDto dto)
        {
            try
            {
                var dialogResult = await dialogHost.Question("温馨提示", $"您确定删除待办事项：{dto.Title}?");
                if(dialogResult.Result != ButtonResult.OK)
                { 
                    return; 
                }

                UpdateLoading(true);
                var deleteResult = await service.DeleteAsync(dto.Id);
                if (deleteResult.Status)
                {
                    var model = TodoDtos.FirstOrDefault(t => t.Id.Equals(dto.Id));
                    if (model != null)
                        TodoDtos.Remove(model);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                UpdateLoading(false);
            }
         
        }

    }
}
