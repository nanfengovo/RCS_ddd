using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RCS.Application.Modules.Auth.Commands;
using RCS.Core.Common;

namespace RCS.Api.Controllers.Sys;

[ApiController]
[Route("api/sys/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous] // 💡 极客细节：登录接口绝对不能被拦截！
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var token = await _mediator.Send(command);
        
        // 优雅返回
        return Ok(ApiResponse.Success(new { Token = token }, "登录成功！"));
    }
}