namespace MyToDo.Api.Context
{
    public class ToDo : BaseEntity
    {
        //属性定义
        //标题
        public string Title { get; set; }

        //内容
        public string Content {  get; set; }

        //是否完成的状态
        public int Status {  get; set; }
    }
}
