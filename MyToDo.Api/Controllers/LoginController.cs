using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Controllers
{
    /// <summary>
    /// 控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService service;
        public LoginController(ILoginService service)
        {
            this.service = service;
        }
        //写入控制器方法
        [HttpPost]
        public async Task<ApiResponse<UserDto>> Login([FromBody] UserDto param) => await service.LoginAsync(param.Account,param.PassWord,param.UserName);

        [HttpPost]
        public async Task<ApiResponse> Resgiter([FromBody] UserDto param) => await service.Resgiter(param);

    }
}

