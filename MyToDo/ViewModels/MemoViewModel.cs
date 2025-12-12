using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    internal class MemoViewModel :NavigationViewModel
    {
        private readonly IDialogHostService dialogHost;
        public MemoViewModel(IMemoService service, IContainerProvider provider):base(provider)
        {
            //实例化TodoDtos
            MemoDtos = new ObservableCollection<MemoDto>();

            //实例化备忘命令与方法
            ExcuteMemoCommand = new DelegateCommand<string>(ExcuteMemo);

            //实例化备忘内容选中命令
            SelectedCommand = new DelegateCommand<MemoDto>(SelectedMemoMethod);

            //实例化备忘内容删除命令
            DeleteCommand = new DelegateCommand<MemoDto>(DeleteToDo);
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
        private void AddMemo()
        {
            CurrentDto = new MemoDto();
            IsRightDrawerOpen = true;
        }

        //定义DoToDto类型的集合属性
        private ObservableCollection<MemoDto> memoDtos;
        private readonly IMemoService service;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }

        //定义编辑/新增操作的选中备忘内容的属性状态
        private MemoDto currentDto;
        public MemoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }

        //定义备忘内容搜索属性
        private string search;
        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }


        //定义添加备忘命令
        public DelegateCommand<string> ExcuteMemoCommand { get; private set; }

        //定义备忘内容选中命令
        public DelegateCommand<MemoDto> SelectedCommand { get; private set; }

        //定义备忘内容删除命令
        public DelegateCommand<MemoDto> DeleteCommand { get; private set; }

        //获取备忘数据
        async Task GetCreateMemoMethodData()
        {

            UpdateLoading(true);//打开等待窗口
            var memoResult = await service.GetAllAsync(new Shared.Parameters.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
            });

            if (memoResult.Status)
            {
                MemoDtos.Clear();
                foreach (var item in memoResult.Result.Items)
                {
                    MemoDtos.Add(item);
                }
            }
            UpdateLoading(false);//关闭等待窗口
        }

        //调用导航数据加载方法
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            GetCreateMemoMethodData();
        }

        private void ExcuteMemo(string obj)
        {

            switch (obj)
            {
                case "新增":
                    AddMemo();
                    break;
                case "搜索":
                    Query();
                    break;
                case "保存":
                    Save();
                    break;
            }
        }

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
                        var memo = MemoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (memo != null)
                        {
                            memo.Title = CurrentDto.Title;
                            memo.Content = CurrentDto.Content;
                        }
                    }
                    IsRightDrawerOpen = false;
                }
                else//id等于0为新增数据
                {
                    var addResult = await service.AddAsync(CurrentDto);
                    if (addResult.Status)
                    {
                        MemoDtos.Add(addResult.Result);
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

        private void Query()
        {
            GetCreateMemoMethodData();
        }

        private async void SelectedMemoMethod(MemoDto dto)
        {
            try
            {
                UpdateLoading(true);
                //加载数据库中的数据
                var memoResule = await service.GetFirstofDefaultAsync(dto.Id);
                if (memoResule.Status)
                {
                    CurrentDto = memoResule.Result;
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

        private async void DeleteToDo(MemoDto dto)
        {
            try
            {
                var dialogResult = await dialogHost.Question("温馨提示", $"您确定删除此项备忘录吗：{dto.Title}?");
                if (dialogResult.Result != ButtonResult.OK)
                {
                    return;
                }
                UpdateLoading(true);
                var deleteResult = await service.DeleteAsync(dto.Id);
                if (deleteResult.Status)
                {
                    var model = MemoDtos.FirstOrDefault(t => t.Id.Equals(dto.Id));
                    if (model != null)
                        MemoDtos.Remove(model);
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
