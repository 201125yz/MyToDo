using MyToDo.Api.Service;
using MyToDo.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface ILoginService
    {
        //添加登录与注册方法
        Task<ApiResponse<UserDto>> LoginAsync(UserDto user);

        //注册方法的定义需要传递一个用户的实体
        Task<ApiResponse> ResgiterAsync(UserDto user);
    }
}
