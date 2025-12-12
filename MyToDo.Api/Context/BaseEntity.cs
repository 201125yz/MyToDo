namespace MyToDo.Api.Context
{
    public class BaseEntity
    {
        //基类
        //定义ID
        public int Id { get; set; }

        //定义时间
        public DateTime CreateDate {  get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
