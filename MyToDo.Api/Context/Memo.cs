namespace MyToDo.Api.Context
{
    public class Memo : BaseEntity
    {
        //属性定义
        //标题
        public string Title { get; set; }

        //内容
        public string Content { get; set; }
    }
}
