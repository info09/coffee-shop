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

    public IPermissionRepository Permission => _permissionRepository.Value;

    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return _context.Database.BeginTransactionAsync();
    }

    public Task EndTransactionAsync()
    {
        return _context.Database.CommitTransactionAsync();
    }

    public void RollbackTransaction()
    {
        _context.Database.RollbackTransaction();
    }

    public async Task<int> SaveAsync()
    {
        return await _unitOfWork.CommitAsync();
    }
}
