using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RCS.Core.Common.Security;
using RCS.Core.Modules.Sys.Entities;

namespace RCS.Infrastructure.Auth;

internal class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _configuration;

    public JwtProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Generate(User user, string[] permissions)
    {
        // 1. 获取我们在 AuthExtensions 里配置的密钥 (必须一致)
        var secret = _configuration["JwtSettings:Secret"] ?? "YourSuperSecretKeyForRcsSystem_2026!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 2. 组装 Token 里的基础信息 (载荷 Payload)
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // 用户ID
            new Claim(JwtRegisteredClaimNames.Name, user.Username),     // 用户名
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // 3. 把权限也塞进去 (注意：如果权限太多，极客做法是不塞进 Token，而是让 AOP 去 Redis 查。我们这里为了快速闭环，先塞进 Token 作为演示)
        foreach (var permission in permissions)
        {
            claims.Add(new Claim("permissions", permission));
        }

        // 4. 生成 Token (有效期给长一点，比如 7 天)
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}