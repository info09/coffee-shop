using IDP.Common.Domains;
using IDP.Entities;

namespace IDP.Common.Repositories;

public interface IPermissionRepository : IRepositoryBase<Permission, long>
{
    Task<IEnumerable<Permission>> GetPermissionsByRole(string roleId, bool trackChanges = false);

    void UpdatePermissionsByRoleId(string roleId, IEnumerable<Permission> permissionCollection,
        bool trackChanges = false);
}
