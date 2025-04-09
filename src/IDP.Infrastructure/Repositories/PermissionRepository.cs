using IDP.Infrastructure.Domains;
using IDP.Infrastructure.Entities;
using IDP.Persistence;

namespace IDP.Infrastructure.Repositories;

public class PermissionRepository : RepositoryBase<Permission, long>, IPermissionRepository
{
    public PermissionRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }

    public Task<IEnumerable<Permission>> GetPermissionsByRole(string roleId, bool trackChanges = false)
    {
        throw new NotImplementedException();
    }

    public void UpdatePermissionsByRoleId(string roleId, IEnumerable<Permission> permissionCollection, bool trackChanges = false)
    {
        throw new NotImplementedException();
    }
}
