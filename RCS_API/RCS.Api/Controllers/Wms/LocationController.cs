using MediatR;
using Microsoft.AspNetCore.Mvc;
using RCS.Application.Modules.Wms.Commands;

namespace RCS.Api.Controllers.Wms
{
    [ApiController]
    [Route("api/wms/locations")]
    public class LocationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{locationCode}/lock")]
        public async Task<IActionResult> LockLocation(string locationCode, [FromBody] LockLocationRequest request)
        {
            var command = new LockLocationCommand(locationCode, request.TaskId);
            var result = await _mediator.Send(command);

            return Ok(new { Success = result ,Message = "库位锁定成功！"});
        }

        // 接收前端 JSON 报文的极简 DTO
        public class LockLocationRequest
        {
            public Guid TaskId { get; set; }
        }
    }
}