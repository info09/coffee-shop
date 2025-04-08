using IDP.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace IDP.Common.Repositories;

public interface IRepositoryManager
{
    UserManager<User> UserManager { get; }
    RoleManager<IdentityRole> RoleManager { get; }
    IPermissionRepository Permission { get; }
    Task<int> SaveAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task EndTransactionAsync();
    void RollbackTransaction();
}
