using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace RCS.Api.Controllers.Webhooks;

/// <summary>
/// 供新松 WCS 系统回调的专属 Webhook
/// </summary>
[ApiController]
[Route("api/v1/xinsong")]
public class XinSongWebhookController : ControllerBase
{
    private readonly ILogger<XinSongWebhookController> _logger;

    public XinSongWebhookController(ILogger<XinSongWebhookController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 10. 接收上报车辆状态 
    /// 请求地址: http://{{server}}:9000/api/v1/xinsong/report_robot_status 
    /// </summary>
    [HttpPost("report_robot_status")]
    public IActionResult ReportRobotStatus([FromBody] XinSongRobotStatusRequest dto)
    {
        // 这里只是简单日志，实际业务中应该利用 MediatR 将状态广播出去，或者推送到 Redis
        _logger.LogInformation(
            "收到新松AGV状态更新 - 车辆:[{Agv}] 状态:[{Status}] 坐标:({X},{Y}) 角度:{Theta} 电量:{Battery}%",
            dto.AgvSerial, dto.Status, dto.XPosition, dto.YPosition, dto.Theta, dto.BatteryInfo?.Soc); // [cite: 269]

        // 必须按要求返回成功格式
        return Ok(new XinSongWebhookResponse { Result = true, ErrMsg = "" }); 
    }
}

// ==========================================
// Webhook 入站模型
// ==========================================

public class XinSongWebhookResponse
{
    [JsonPropertyName("Result")]
    public bool Result { get; set; } // true-上位系统收到 [cite: 277]

    [JsonPropertyName("ErrMsg")]
    public string ErrMsg { get; set; } = string.Empty; // [cite: 277]
}

public class XinSongRobotStatusRequest
{
    [JsonPropertyName("AGV_serial")]
    public string AgvSerial { get; set; } = string.Empty; // 车辆编号 [cite: 269]

    [JsonPropertyName("status")]
    public int Status { get; set; } // 0-空闲,1-运行,2-离线... [cite: 269]

    [JsonPropertyName("x_position")]
    public string XPosition { get; set; } = string.Empty; // X坐标 [cite: 269]

    [JsonPropertyName("y_position")]
    public string YPosition { get; set; } = string.Empty; // Y坐标 [cite: 269]

    [JsonPropertyName("theta")]
    public string Theta { get; set; } = string.Empty; // 车辆朝向 [cite: 269]

    [JsonPropertyName("battery_info")]
    public XinSongBatteryInfo? BatteryInfo { get; set; } // [cite: 269]
}

public class XinSongBatteryInfo
{
    [JsonPropertyName("soc")]
    public string Soc { get; set; } = string.Empty; // 电池满电容量 Ah [cite: 269]
}