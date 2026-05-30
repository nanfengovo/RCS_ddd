using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RCS.Core.Exceptions
{
    /// <summary>
    /// 领域层业务异常基类
    /// </summary>
    public class DomainException : Exception
    {
        public string? ErrorCode { get; }

        public DomainException(string message, string? errorCode = null) 
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}