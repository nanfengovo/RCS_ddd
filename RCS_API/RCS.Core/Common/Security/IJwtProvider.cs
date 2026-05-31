using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RCS.Core.Modules.Sys.Entities;

namespace RCS.Core.Common.Security
{
    /// <summary>
    /// 全局 JWT Token 生成契约
    /// </summary>
    public interface IJwtProvider
    {
        /// <summary>
        /// 根据用户和他的权限列表，生成 Token
        /// </summary>
        string Generate(User user, string[] permissions);
    }
}