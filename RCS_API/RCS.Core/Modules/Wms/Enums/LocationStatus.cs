namespace RCS.Core.Modules.Wms.Enums
{
    public enum LocationStatus
    {
        /// <summary>
        /// 空闲
        /// </summary>
        Idle = 0,

        /// <summary>
        /// 被锁定（可能正在被某个操作占用，但还未正式占用）
        /// </summary>
        Locked = 1,

        /// <summary>
        /// 占用（正在被某个操作正式占用，不能被其他操作使用）
        /// </summary>
        Occupied = 2,

        /// <summary>
        /// 禁用（该库位不可用，可能是损坏、维修中或其他原因导致的不可用状态）
        /// </summary>
        Disable = 3
    }
}