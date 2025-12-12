using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    //勇于汇总主页所有信息的实体类
    public class SummaryDto : BaseDto
    {
        /// <summary>
        /// 实体类属性定义
        /// </summary>
        //统计待办数量
        private int sum;
        public int Sum
        {
            get { return sum; }
            set { sum = value; OnPropertyChanged(); }
        }

        //统计完成数量
        private int completedCount;
        public int CompletedCount
        {
            get { return completedCount; }
            set { completedCount = value; OnPropertyChanged(); }
        }

        //完成比例
        private string completedRadio;
        public string CompletedRadio
        {
            get { return completedRadio; }
            set { completedRadio = value; OnPropertyChanged(); }
        }

        //统计备忘录数量
        private int memoCount;
        public int MemoCount
        {
            get { return memoCount; }
            set { memoCount = value; OnPropertyChanged(); }
        }

        //动态列表集合
        private ObservableCollection<ToDoDto> toDoList;
        public ObservableCollection<ToDoDto> ToDoList
        {
            get { return toDoList; }
            set { toDoList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<MemoDto> memoList;
        public ObservableCollection<MemoDto> MemoList
        {
            get { return memoList; }
            set { memoList = value; OnPropertyChanged(); }
        }
    }
}
