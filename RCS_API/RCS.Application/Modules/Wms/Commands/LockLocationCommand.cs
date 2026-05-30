using MediatR;
using RCS.Core.Modules.Wms.Repositories;

namespace RCS.Application.Modules.Wms.Commands
{   
    /// <summary>
    /// 锁定库位命令
    /// </summary>
    public class LockLocationCommand : IRequest<bool>
    {
        public string LocationCode { get; set; } = default!;

        public Guid TaskId { get; set; }

        public LockLocationCommand(string locationCode, Guid taskId)
        {
            LocationCode = locationCode;
            TaskId = taskId;
        }
    }

    public class LockLocationCommandHandler : IRequestHandler<LockLocationCommand, bool>
    {
        private readonly ILocationRepository _locationRepository;

        public LockLocationCommandHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<bool> Handle(LockLocationCommand request, CancellationToken cancellationToken)
        {
            var location = await _locationRepository.GetByCodeAsync(request.LocationCode);
            if(location == null)
            {
                throw new Exception($"库位 {request.LocationCode} 不存在，无法锁定！");
            }

            location.LockForTask(request.TaskId);
            await _locationRepository.UpdateAsync(location);
            return true;
        }



        
    }
}