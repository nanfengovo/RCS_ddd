using MediatR;
using RCS.Core.Common.Security;
using RCS.Core.Exceptions;
using RCS.Core.Modules.Auth.Repositories; // 👈 纯洁的抽象接口

namespace RCS.Application.Modules.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly IUserRepository _userRepository; // 👈 干掉 DbContext，换成仓储！
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. 根据用户名获取用户
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        // (极客注意：生产环境必须是 Hash 比对！这里为了快速跑通先直接比对字符串)
        if (user == null || user.PasswordHash != request.Password)
        {
            throw new DomainException("用户名或密码错误！");
        }

        if (!user.IsActive)
        {
            throw new DomainException("该账号已被禁用！");
        }

        // 2. 获取该用户的全部权限码
        var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);

        // 3. 组装 Token
        var token = _jwtProvider.Generate(user, permissions);

        return token;
    }
}