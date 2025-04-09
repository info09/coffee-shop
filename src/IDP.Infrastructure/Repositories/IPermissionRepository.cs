using IDP.Infrastructure.Domains;
using IDP.Infrastructure.Entities;

namespace IDP.Infrastructure.Repositories;

public interface IPermissionRepository : IRepositoryBase<Permission, long>
{
    Task<IEnumerable<Permission>> GetPermissionsByRole(string roleId, bool trackChanges = false);

    void UpdatePermissionsByRoleId(string roleId, IEnumerable<Permission> permissionCollection,
        bool trackChanges = false);
}
