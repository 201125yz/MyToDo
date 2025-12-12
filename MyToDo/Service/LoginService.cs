using MyToDo.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class LoginService : ILoginService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName = "Login";

        public LoginService(HttpRestClient client) 
        {
            this.client = client;
        }
        public async Task<Api.Service.ApiResponse<UserDto>> LoginAsync(UserDto user)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;//方法类型
            request.Route = $"api/{serviceName}/login";//路由
            request.Parameter = user;
            return await client.ExecuteAsync<UserDto>(request);
        }

        public async Task<Api.Service.ApiResponse> ResgiterAsync(UserDto user)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;//方法类型
            request.Route = $"api/{serviceName}/Resgiter";//路由
            request.Parameter = user;
            return await client.ExecuteAsync(request);
        }
    }
}
