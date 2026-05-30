using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RCS.Core.Modules.Sys.Entities
{
    public class Permission
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        // 权限标识码 (如: "wms:location:lock")
        public string Code { get; set; } = null!; 
        public string Name { get; set; } = null!; 
        public string Type { get; set; } = "Button"; // Menu, Button, Api
        public Guid? ParentId { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}