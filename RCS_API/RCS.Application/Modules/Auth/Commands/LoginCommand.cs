using MediatR;

namespace RCS.Application.Modules.Auth.Commands;

// 返回的是生成的 JWT Token 字符串
public record LoginCommand(string Username, string Password) : IRequest<string>;