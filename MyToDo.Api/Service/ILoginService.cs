using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Service
{
    //登录注册服务
    public interface ILoginService
    {
        //添加登录与注册方法
        Task<ApiResponse<UserDto>> LoginAsync(string Account, string Password,string UserName);

        //注册方法的定义需要传递一个用户的实体
        Task<ApiResponse> Resgiter(UserDto user);
    }
}
