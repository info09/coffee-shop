using Microsoft.EntityFrameworkCore;

namespace IDP.Infrastructure.Domains;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync();
}