using IDP.Common.Domains;
using IDP.Entities;
using IDP.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace IDP.Common.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;
    private readonly Lazy<IPermissionRepository> _permissionRepository;

    public RepositoryManager(IUnitOfWork unitOfWork, ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        UserManager = userManager;
        RoleManager = roleManager;
        _permissionRepository = new Lazy<IPermissionRepository>(() => new PermissionRepository(_context, unitOfWork));
    }

    public UserManager<User> UserManager { get; }

    public RoleManager<IdentityRole> RoleManager { get; }

    public IPermissionRepository Permission => throw new NotImplementedException();

    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        throw new NotImplementedException();
    }

    public Task EndTransactionAsync()
    {
        throw new NotImplementedException();
    }

    public void RollbackTransaction()
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveAsync()
    {
        throw new NotImplementedException();
    }
}
