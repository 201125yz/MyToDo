using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Service
{
    //通用接口
    public interface IBaseService<T>
    {
        //通用的增删改查方法
        //获取所有数据
        Task<ApiResponse> GetAllAsync(QueryParameter query);

        //用id获取单条数据
        Task<ApiResponse> GetSingleAsync(int id);

        //增添
        Task<ApiResponse> AddAsync(T model);

        //更新
        Task<ApiResponse> UpdateAsync(T model);

        //删除
        Task<ApiResponse> DeleteAsync(int id);
    }
}
