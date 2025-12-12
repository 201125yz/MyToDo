using Newtonsoft.Json;

namespace MyToDo.Api.Service
{
    //通用返回结果类型
    public class ApiResponse
    {
        public ApiResponse()
        {
        }

        public ApiResponse(string message, bool status = false)//操作失败时
        {
            this.Message = message;
            this.Status = status;
        }

        public ApiResponse(bool status, object Result)
        {
            this.Status = status;
            this.Result = Result;
        }

        //定义字段
        public string Message { get; set; }
        public bool Status { get; set; }
        public object Result { get; set; }


    }
    /// <summary>
    /// 泛型通用返回结果类型
    /// </summary>
    public class ApiResponse<T>
    {
        public ApiResponse() { }

        // 失败，只传 message
        public ApiResponse(string message, bool status = false)
        {
            Message = message;
            Status = status;
            Result = default;
        }

        // 成功，传 Result，可选 Message
        public ApiResponse(T result, string message = "")
        {
            Status = true;
            Result = result;
            Message = message;
        }

        // 明确 Status + Result + Message
        public ApiResponse(bool status, T result, string message = "")
        {
            Status = status;
            Result = result;
            Message = message;
        }

        public string Message { get; set; }

        public bool Status { get; set; }

        public T? Result { get; set; }
    }  
}
