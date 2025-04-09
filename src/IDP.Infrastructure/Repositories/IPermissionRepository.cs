using IDP.Infrastructure.Domains;
using IDP.Infrastructure.Entities;
using IDP.Infrastructure.ViewModels;

namespace IDP.Infrastructure.Repositories;

public interface IPermissionRepository : IRepositoryBase<Permission, long>
{
    Task<IReadOnlyList<PermissionViewModel>> GetPermissionsByRole(string roleId);
    Task<PermissionViewModel?> CreatePermission(string roleId, PermissionAddModel model);
    Task DeletePermission(string roleId, string function, string command);
    Task UpdatePermissionsByRoleId(string roleId, IEnumerable<PermissionAddModel> permissionCollection);
    Task<IEnumerable<PermissionUserViewModel>> GetPermissionsByUser(User user);
}
