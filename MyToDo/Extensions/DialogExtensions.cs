using MyToDo.Common;
using MyToDo.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Extensions
{
    //定义扩展方法去控制界面内容加载动画
    public static class DialogExtensions
    {
        /// <summary>
        /// 公共消息弹窗提示方法
        /// </summary>
        public static async Task<IDialogResult> Question(this IDialogHostService dialogHost,string title,string message,string dialogHostName = "RootDialog")
        {
            DialogParameters param = new DialogParameters();
            param.Add("Title", title);
            param.Add("Message", message);
            param.Add("dialogHostName", dialogHostName);
            var dialogResult = await dialogHost.ShowDialog("MsgView", param, dialogHostName);
            return dialogResult;
        }

        //发布等待消息旋转动画
        public static void UpdateLoading(this IEventAggregator aggregator, UpdateModel model)
        {
            aggregator.GetEvent<UpdateLoadingEvent>().Publish(model);  
        }

        //订阅等待消息旋转动画
        public static void Resgiter(this IEventAggregator aggregator, Action<UpdateModel> action)
        {
            aggregator.GetEvent<UpdateLoadingEvent>().Subscribe(action);
        }

        //发布提示消息信息
        public static void UpdateMessage(this IEventAggregator aggregator, string message, string filterName = "Main")
        {
            aggregator.GetEvent<MDMessageEvent>().Publish(new MessageModel
            {
                Message = message,
                Filter = filterName
            });
        }
         
        //订阅提示消息行为
        public static void ResgiterMessage(this IEventAggregator aggregator, Action<MessageModel> action, string filterName = "Main")
        {
            aggregator.GetEvent<MDMessageEvent>().Subscribe(action,ThreadOption.PublisherThread ,true, (m) =>
            {
                return m.Filter.Equals(filterName);
            });
        }
    }
}
