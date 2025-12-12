using MyToDo.Api.Service;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class HttpRestClient 
    { 
        private readonly string apiUrl;

        //创建客户端请求
        private readonly RestClient client; 

        //注册地址
        public HttpRestClient(string apiUrl) 
        {
            this.apiUrl = apiUrl;
            client = new RestClient(apiUrl);
        }

        /// <summary>
        /// 发送 HTTP 请求到你的 Web API 并返回结果
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <returns></returns>
        //发起请求的异步通用方法
        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            //在构造请求时传入 Route（接口路径）和 Method（请求方法）
            var request = new RestRequest(baseRequest.Route, baseRequest.Method);
            //设置 Content-Type 头
            request.AddHeader("Content-Type", baseRequest.ContentType);
            //添加请求
            if (baseRequest.Parameter != null)
                //request.AddParameter("application/json", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
                request.AddStringBody(JsonConvert.SerializeObject(baseRequest.Parameter), baseRequest.ContentType);
            //client.BaseUrl = new Uri(apiUrl + baseRequest.Route);
            var response = await client.ExecuteAsync(request);

            //var settings = new JsonSerializerSettings
            //{
            //    MissingMemberHandling = MissingMemberHandling.Ignore,
            //    NullValueHandling = NullValueHandling.Ignore,
            //    ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()
            //};
            //return JsonConvert.DeserializeObject<ApiResponse>(response.Content, settings);

            return JsonConvert.DeserializeObject<ApiResponse>(response.Content); 
        }

        //发起请求的异步通用方法(泛型)反序列化
        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            //在构造请求时传入 Route（接口路径）和 Method（请求方法）
            var request = new RestRequest(baseRequest.Route, baseRequest.Method);
            //设置 Content-Type 头
            request.AddHeader("Content-Type", baseRequest.ContentType);
            //添加请求
            if (baseRequest.Parameter != null)
                //request.AddParameter("application/json", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
                request.AddStringBody(JsonConvert.SerializeObject(baseRequest.Parameter), baseRequest.ContentType);
            var response = await client.ExecuteAsync(request);

            //var settings = new JsonSerializerSettings
            //{
            //    MissingMemberHandling = MissingMemberHandling.Ignore,
            //    NullValueHandling = NullValueHandling.Ignore,
            //    ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()
            //};

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                //return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content, settings);
                return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
            //return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
            else
                return new ApiResponse<T>()
                {
                    Status = false,
                    Message = response.ErrorMessage
                };

            // ⭐ 添加调试输出
            //System.Diagnostics.Debug.WriteLine($"[调试输出] 请求地址: {client.Options.BaseUrl}{baseRequest.Route}");
            //System.Diagnostics.Debug.WriteLine($"[调试输出] 响应状态码: {response.StatusCode}");
            //System.Diagnostics.Debug.WriteLine($"[调试输出] 响应内容: {response.Content}");

            //if (string.IsNullOrWhiteSpace(response.Content))
            //{
            //    System.Diagnostics.Debug.WriteLine("⚠️ 返回内容为空！");
            //    return null;
            //}

            //try
            //{
            //    var result = JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine($"⚠️ 反序列化异常: {ex.Message}");
            //    return null;
            //}
        }
    }
}
