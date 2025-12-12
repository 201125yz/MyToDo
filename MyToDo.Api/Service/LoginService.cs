using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Extensions;

namespace MyToDo.Api.Service
{
    /// <summary>
    /// 登录注册的服务的具体实现
    /// </summary>
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper Mapper;
        public LoginService(IUnitOfWork work, IMapper mapper) 
        {
            this.work = work;
            this.Mapper = mapper;
        }
        public async Task<ApiResponse<UserDto>> LoginAsync(string Account, string Password, string UserName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Account) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(UserName))
                {
                    return new ApiResponse<UserDto>(status: false, result: null, message: "账号或密码或用户名不能为空！");
                }

                var hashedPassword = Password.GetMD5();//加密
                var model = await work.GetRepository<User>().GetFirstOrDefaultAsync(predicate:
                    x => (x.Account.Equals(Account)) &&
                    (x.Password.Equals(hashedPassword)) && (x.UserName.Equals(UserName)));

                if (model == null)

                    return new ApiResponse<UserDto>(status: false, result: null, message: "账号或密码或用户名错误，请重试！");

                var userDto = new UserDto
                {
                    Id = model.Id,
                    Account = model.Account,
                    UserName = model.UserName
                };

                return new ApiResponse<UserDto>(status: true, result: userDto, message: "登录成功");

            }
            catch (Exception ex)
            {
                return new ApiResponse<UserDto>(status: false, result: null, message: $"登录失败：{ex.Message}");
            }
        }

        public async Task<ApiResponse> Resgiter(UserDto user)
        {
            try
            {
                var model = Mapper.Map<User>(user);
                var repository = work.GetRepository<User>();
                var userModel = await repository.GetFirstOrDefaultAsync(predicate: x => x.Account.Equals(model.Account));

                if (userModel != null)
                    return new ApiResponse($"当前账号：{model.Account}已存在，请重新注册！");

                model.CreateDate = DateTime.Now;
                model.Password = model.Password.GetMD5();
                await repository.InsertAsync(model);

                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, model);

                return new ApiResponse("注册失败，请稍后重试！");
            }

            catch(Exception ex)
            {
                return new ApiResponse("注册账号失败！");
            }
        }
    }
}
