using SmartWorkspace.Domain.Common;

namespace SmartWorkspace.Domain.Users
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = default!;
        public ICollection<RolePermission> Permissions { get; set; } = new List<RolePermission>();
    }
}
