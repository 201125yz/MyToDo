namespace MyToDo.Api.Context
{
    public class User : BaseEntity
    {
        //定义用户表属性
        //账号
        public string Account {  get; set; }

        //用户
        public string UserName {  get; set; }

        //密码
        public string Password { get; set; }
    }
}
