using MyToDo.Api.Context;
using MyToDo.Common;
using MyToDo.Common.Events;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Views.Dialogs;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MyToDo.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {
        //通过IContainerProvider provider拿到待办与备忘的服务
        private readonly ITodoService todoService;
        private readonly IMemoService memoService;
        private readonly IDialogHostService dialog;
        private readonly IRegionManager regionManager;
        //定义构造函数
        public IndexViewModel(IContainerProvider provider, IDialogHostService dialog) : base(provider)
        {
            //Title = $"你好,柠檬儿！  今天是：{DateTime.Now.GetDateTimeFormats('D')[1].ToString()}";
            StartClock();//标题与时间初始化
            TaskBarMethod();
            //实例化定义的集合列表属性，并执行对应的方法
            //TaskBars = new ObservableCollection<TaskBar>();
            //TodoDtos = new ObservableCollection<ToDoDto>();
            //MemoDtos = new ObservableCollection<MemoDto>();

            ExecuteCommand = new DelegateCommand<string>(Execute);
            EditToDoCommand = new DelegateCommand<ToDoDto>(AddToDo);
            EditMemoCommand = new DelegateCommand<MemoDto>(AddMemo);
            CompltedtodoCommand = new DelegateCommand<ToDoDto>(Compltedtodo);
            NavigateCommand = new DelegateCommand<TaskBar>(Navigate);
            this.todoService = provider.Resolve<ITodoService>();
            this.memoService = provider.Resolve<IMemoService>();
            this.regionManager = provider.Resolve<IRegionManager>();    
            this.dialog = dialog;
        }

        /// <summary>
        /// 属性定义
        /// </summary>
        //定义TaskBar类型的内部集合列表属性
        private ObservableCollection<TaskBar> taskBars;
        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }
        }

        //private ObservableCollection<ToDoDto> todoDtos;
        //public ObservableCollection<ToDoDto> TodoDtos
        //{
        //    get { return todoDtos; }
        //    set { todoDtos = value; RaisePropertyChanged(); }
        //}
        //private ObservableCollection<MemoDto> memoDtos;
        //public ObservableCollection<MemoDto> MemoDtos
        //{
        //    get { return memoDtos; }
        //    set { memoDtos = value; RaisePropertyChanged(); }
        //}

        private SummaryDto summary;
        public SummaryDto Summary
        {
            get { return summary; }
            set { summary = value; RaisePropertyChanged(); }
        }

        //标题（名字与日期）属性
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 命令定义
        /// </summary>
        //添加待办与备忘弹窗
        public DelegateCommand<string> ExecuteCommand {  get; private set; }
        public DelegateCommand<ToDoDto>EditToDoCommand { get; private set; }
        public DelegateCommand<MemoDto> EditMemoCommand {  get; private set; }
        public DelegateCommand<ToDoDto> CompltedtodoCommand {  get; private set; }
        public DelegateCommand<TaskBar>NavigateCommand { get; private set; }

        //定义集合列表属性方法
        void TaskBarMethod()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            TaskBars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总",  Bordercolor = "#DB402B", Targetname = "ToDoView"});
            TaskBars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成",  Bordercolor = "#DBA92B", Targetname = "ToDoView" });
            TaskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成比例",  Bordercolor = "#86DB2B", Targetname = "" });
            TaskBars.Add(new TaskBar() { Icon = "Playliststar", Title = "备忘录",  Bordercolor = "#2BBBDB", Targetname = "MemoView" });
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var summaryResult = await todoService.SummaryAsync();
            if (summaryResult.Status)
            {
                Summary = summaryResult.Result;
                Refresh();
            }
            base.OnNavigatedTo(navigationContext);
        }

        void Refresh()
        {
            TaskBars[0].Concent = summary.Sum.ToString();
            TaskBars[1].Concent = summary.CompletedCount.ToString();
            TaskBars[2].Concent = summary.CompletedRadio;
            TaskBars[3].Concent = summary.MemoCount.ToString();
        }


        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增待办":
                    AddToDo(null);
                    break;
                case "新增备忘":
                    AddMemo(null);
                    break;
            }
        }

        //添加待办方法
        async void AddToDo(ToDoDto model)
        {
            DialogParameters param = new DialogParameters();
            if (model != null)
                param.Add("Value",model);

            var dialogResult = await dialog.ShowDialog("AddToDoView",param);
            if(dialogResult.Result == ButtonResult.OK)
            {
                try
                {
                    UpdateLoading(true);
                    var todo = dialogResult.Parameters.GetValue<ToDoDto>("Value");
                    if (todo.Id > 0)//更新数据
                    {
                        var updateResult = await todoService.UpdateAsync(todo);
                        if (updateResult.Status)
                        {
                            var todoModel = summary.ToDoList.FirstOrDefault(t => t.Id.Equals(todo.Id));
                            if (todoModel != null)
                            {
                                todoModel.Title = todo.Title;
                                todoModel.Content = todo.Content;
                            }
                        }
                    }
                    else//新增数据
                    {
                        var addResult = await todoService.AddAsync(todo);
                        if (addResult.Status)
                        {

                            summary.ToDoList.Add(addResult.Result);
                            summary.Sum += 1;
                            summary.CompletedRadio = (summary.CompletedCount / (double)(summary.Sum)).ToString("0%");
                            this.Refresh();
                        }
                    }
                }
                finally
                {
                    UpdateLoading(false);
                }
               
            }
        }

        //添加备忘方法
        async void AddMemo(MemoDto model)
        {
            DialogParameters param = new DialogParameters();
            if (model != null)
                param.Add("Value", model);

            var dialogResult = await dialog.ShowDialog("AddMemoView", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                try
                {
                    UpdateLoading(true);
                    var memo = dialogResult.Parameters.GetValue<MemoDto>("Value");
                    if (memo.Id > 0)//更新数据
                    {
                        var updateResult = await memoService.UpdateAsync(memo);
                        if (updateResult.Status)
                        {
                            //如果更新成功就"通过ID找到"更新结果给memoModel
                            var memoModel = summary.MemoList.FirstOrDefault(t => t.Id.Equals(memo.Id));
                            if (memoModel != null)
                            {
                                memoModel.Title = memo.Title;
                                memoModel.Content = memo.Content;

                            }
                        }
                    }
                    else//新增数据
                    {
                        //将获取的弹窗里的值写入服务端
                        var addResult = await memoService.AddAsync(memo);
                        if (addResult.Status)
                        {

                            summary.MemoList.Add(addResult.Result);
                            summary.MemoCount += 1;
                            this.Refresh();
                        }
                    }
                }
                finally
                {
                    UpdateLoading(false);
                }
              
            }
        }

        //待办完成方法
        private async void Compltedtodo(ToDoDto dto)
        {
            try
            {
                UpdateLoading(true);
                var updateResult = await todoService.UpdateAsync(dto);
                if (updateResult.Status)
                {
                    var todoModel = summary.ToDoList.FirstOrDefault(t => t.Id.Equals(dto.Id));
                    if (todoModel != null)
                    {
                        summary.ToDoList.Remove(todoModel);
                        summary.CompletedCount += 1;
                        summary.CompletedRadio = (summary.CompletedCount / (double)(summary.Sum)).ToString("0%");
                        this.Refresh();
                    }
                }
                aggregator.UpdateMessage( "已完成该待办" );
            }
            finally
            {
                UpdateLoading(false);
            }  
        }

        private void StartClock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // 每秒触发一次
            timer.Tick += (s, e) =>
            {
                Title = $"你好,{AppSession.UserName}！ 今天是：{DateTime.Now:yyyy年MM月dd日 dddd HH:mm:ss}";
            };
            timer.Start();
        }

        private void Navigate(TaskBar bar)
        {
            if (string.IsNullOrWhiteSpace(bar.Targetname))
                return;
            NavigationParameters param = new NavigationParameters();

            if(bar.Title == "已完成")
            {
                param.Add("Value", 2);
            }
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(bar.Targetname, param);
             
        }

    }
}
