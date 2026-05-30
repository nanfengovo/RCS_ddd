
namespace RCS.Core.Common
{
    /// <summary>
    /// 企业级 API 统一返回基类（不带数据负载）
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// 业务状态码 (200 表示成功，其他表示各种业务失败)
        /// 注意：这与 HTTP 状态码不同，HTTP 状态码我们统一给 200，用这里的 Code 区分业务结果
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 提示信息 (直接展示给前端用户的友好提示)
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 是否成功 (便捷属性，方便前端 if(res.isSuccess) 判断)
        /// </summary>
        public bool IsSuccess => Code == 200;

        // 受保护的构造函数，强制使用静态工厂方法创建实例
        protected ApiResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }

        #region 静态工厂方法 (供 Controller 丝滑调用)

        // 成功（无数据返回）
        public static ApiResponse Success(string message = "操作成功")
        {
            return new ApiResponse(200, message);
        }

        // 成功（有数据返回，泛型推导）
        public static ApiResponse<T> Success<T>(T data, string message = "操作成功")
        {
            return new ApiResponse<T>(200, message, data);
        }

        // 失败
        public static ApiResponse Fail(string message, int code = 400)
        {
            return new ApiResponse(code, message);
        }

        #endregion
    }

    /// <summary>
    /// 企业级 API 统一返回泛型类（带数据负载）
    /// </summary>
    /// <typeparam name="T">核心数据类型</typeparam>
    public class ApiResponse<T> : ApiResponse
    {
        /// <summary>
        /// 核心数据负载
        /// </summary>
        public T? Data { get; set; }

        // 内部构造函数
        internal ApiResponse(int code, string message, T? data) : base(code, message)
        {
            Data = data;
        }
    }
}