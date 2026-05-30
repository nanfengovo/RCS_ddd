using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RCS.Core.Modules.Sys.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!; 
        public bool IsActive { get; set; } = true;

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}