using SmartWorkspace.Domain.Common;

namespace SmartWorkspace.Domain.Entities.Users
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = default!;
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public ICollection<UserWorkspaceRole> UserWorkspaceRoles { get; set; } = new List<UserWorkspaceRole>();
    }
}
