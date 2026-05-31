namespace RCS.Core.Common.Security;

/// <summary>
/// 极客架构：增强型细粒度权限拦截标签
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class RequirePermissionAttribute : Attribute
{
    public string Code { get; }

    /// <summary>
    /// 指定需要的权限码 (例如: "wms:location:lock")
    /// </summary>
    public RequirePermissionAttribute(string code)
    {
        Code = code;
    }
}